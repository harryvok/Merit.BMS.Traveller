function MakeRecommendationsViewModel() {
    var self = this;

    this.comments = ko.observable("");
    this.reject = ko.observable("");
    this.validfrom = ko.observable($("#validFrom").val());
    this.validTo = ko.observable($("#validTo").val());
    this.permitID = ko.observable($("#hiddenPermitID").val());
    this.intrayID = ko.observable($("#hiddenIntrayID").val());

    if ($("#hiddenEditable").val() == "Y") {
        $("#rejectButton").prop('disabled', true);
        $("#approveButton").prop('disabled', true);
    } else {
        $("#rejectButton").prop('disabled', false);
        $("#approveButton").prop('disabled', false);
    }

    //get webAlias to append onto ajax url
    var alias = $('#webAppAlias').val();

    /*$("#validFrom").datepicker({
        dateFormat: 'dd-MM-yy',
        setDate: new Date(),
        changeMonth: true,
        changeYear: true,
        yearRange: '-70:+10',
        constrainInput: false,
        duration: '',
        gotoCurrent: true
    });*/

   

    /*$("#validTo").datepicker({
        dateFormat: 'dd-M-yy',
        changeMonth: true,
        changeYear: true,
        yearRange: '-70:+10',
        constrainInput: false,
        duration: '',
        gotoCurrent: true
    });*/

    this.recommendProcess = function (reject) {

        $.validator.addMethod("futureDateValidate", function (value, element) {

            var outcome;
            $.ajax({
                type: "GET",
                url: "VisaRegistration/checkDate",
                async: false,
                data: {
                    date: value,
                    type: "F"
                },
                success: function (result) {
                    outcome = result;  
                }
            });

            if (outcome == "") {
                return false;
            }
            else {
                $(element).val(outcome);
                return true;
            }
        }, "Invalid date.");

        $.validator.addMethod("pastDateValidate", function (value, element) {

            var outcome;
            $.ajax({
                type: "GET",
                url: "VisaRegistration/checkDate",
                async: false,
                data: {
                    date: value,
                    type: "P"
                },
                success: function (result) {
                    outcome = result;
                }
            });

            if (outcome == "") {
                return false;
            }
            else {
                $(element).val(outcome);
                return true;
            }
        }, "Invalid date.");


        $('#recommendationsForm').validate({
            errorContainer: "#errorContainer",
            errorLabelContainer: "#errorContainer",
            errorElement: "li",

            rules: {
                validFrom: {required : true},
                validTo: { required: true }
            }

        }).form();

        if ($("#recommendationsForm").validate().numberOfInvalids() == 0) {
            
            $.ajax({
                type: "GET",
                url: "recommendProcess",
                async: false,
                data: {
                    comments: self.comments(),
                    reject: reject,
                    validfrom: self.validfrom(),
                    validTo: self.validTo(),
                    permitID: self.permitID(),
                    intrayID: self.intrayID()
                },
                success: function (result) {
                    if (result == "0") {
                        $('.formclosebutton').trigger("click");
                    } else {
                        alert("there was an error");
                    }
                }
            });

        }

       
    };


} 