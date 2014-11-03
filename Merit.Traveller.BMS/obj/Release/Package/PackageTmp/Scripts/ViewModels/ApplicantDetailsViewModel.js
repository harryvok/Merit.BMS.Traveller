function ApplicantDetailsViewModel() {
    var self = this;
    this.isEditable = ko.observable("");

    //variable to determine whether applicant details can
    //be edited or not
    this.isEditable($("#hiddenInputField").val());


    //applicant details
    this.permitID = ko.observable($('#hiddenPermitID').val());
    this.applicantID = ko.observable($('#hiddenApplicantID').val());
    this.fileNo = ko.observable("");
    this.permitType = ko.observable("");
    this.dateLodged = ko.observable("");
    this.whereLodged = ko.observable("");
    this.permitPeriod = ko.observable("");
    this.entriesAllowed = ko.observable("");
    this.dependents = ko.observable("");
    this.express = ko.observable("");
    this.feeAmount = ko.observable("");
    this.paidAt = ko.observable("");
    this.paidOn = ko.observable("");
    this.receiptNo = ko.observable("");
    this.poffshore = ko.observable("");
    this.sReturn = ko.observable("");
    this.offShore = ko.observable("");
    this.title = ko.observable($('#titleSelect').val());
    this.surname = ko.observable($('#surnameInput').val());
    this.givenName = ko.observable($('#givenNameInput').val());
    this.nationality = ko.observable($('#nationalityInput').val());
    this.cob = ko.observable($('#cobInput').val());
    this.pob = ko.observable($('#pobInput').val());
    this.occupation = ko.observable($('#occupationInput').val());
    this.gender = ko.observable($('#genderInput').val());
    this.dob = ko.observable($('#dobInput').val());
    this.maritalStatus = ko.observable($('#maritalStatusInput').val());
    this.passportNo = ko.observable($('#ppNoInput').val());
    this.add1 = ko.observable($('#add1Input').val());
    this.add2 = ko.observable($('#add2Input').val());
    this.add3 = ko.observable($('#add3Input').val());
    this.email = ko.observable($('#emailInput').val());
    this.phone_no = ko.observable($('#phoneNoInput').val());
    this.workPermNo = ko.observable($('#workPermitNoInput').val());
    this.docExpiry = ko.observable($('#expiryDateInput').val());
    this.positionNo = ko.observable($('#positionNumberInput').val());
    this.appWorkPermExpiry = ko.observable($('#workPermExpiryInput').val());
    this.officerID = ko.observable($('#hiddenOfficerID').val());
    this.addrInCountry = ko.observable($('#appAddInCountryInput').val());
    this.imageID = ko.observable($('#hiddenImageID').val());

    //get webAlias to append onto ajax url
    var alias = $('#webAppAlias').val();


    //apply jquery plugin to date pickers
    /*$("#dobInput").datepicker({ dateFormat: 'dd/mm/yy' });
    $("#workPermExpiryInput").datepicker({ dateFormat: 'dd/mm/yy' });
    $("#expiryDateInput").datepicker({ dateFormat: 'dd/mm/yy' });*/


    /*if (this.isEditable() == 'N') {
        $('input[type=text]').prop('disabled', false);
        $('#completebutton').prop('disabled', false);
        $('#incompletebutton').prop('disabled', false);
        $('select').prop('disabled', false);

    }
    else {
        $('input[type=text]').prop('disabled', true);
        $('#processButton').prop('disabled', true);
        $('#completebutton').prop('disabled', true);
        $('#incompletebutton').prop('disabled', true);
        $('select').prop('disabled', true);
    }*/


    this.test = function () {
        alert(self.isEditable());
    };


    this.processApplication = function () {

        $('#applicantUpdateForm').validate({
            errorContainer: "#errorContainer",
            errorLabelContainer: "#errorContainer",
            errorElement: "li",

            rules: {
                surnameInput: 'required',
                givenNameInput: 'required',
                dobInput: 'required',
                nationalityInput: 'required',
                cobInput: 'required'
            }

        }).form();
        
        if ($("#applicantUpdateForm").validate().numberOfInvalids() == 0) {
            self.updateApplicantDetails();

        }

    };

    this.submitForm = function (completeIndicator) {
        
        var options = {
            target: "#FeedbackSuccess",
            data: {
                permitID: self.permitID(),
                fileNo: self.fileNo(),
                permitType: self.permitType(),
                dateLodged: self.dateLodged(),
                whereLodged: self.whereLodged(),
                permitPeriod: self.permitPeriod(),
                entriesAllowed: self.entriesAllowed(),
                dependents: self.dependents(),
                express: self.express(),
                feeAmount: self.feeAmount(),
                paidAt: self.paidAt(),
                paidOn: self.paidOn(),
                receptNo: self.receiptNo(),
                poffshor: self.poffshore(),
                sReturn: self.sReturn(),
                offShore: self.offShore(),
                title: self.title(),
                surname: self.surname(),
                givenName: self.givenName(),
                nationality: self.nationality(),
                cob: self.cob(),
                pob: self.pob(),
                occupation: self.occupation(),
                gender: self.gender(),
                dob: self.dob(),
                maritalStatus: self.maritalStatus(),
                passportNo: self.passportNo(),
                add1: self.add1(),
                add2: self.add2(),
                add3: self.add3(),
                email: self.email(),
                phone_no: self.phone_no(),
                docExpiry: self.docExpiry(),
                positionNo: self.positionNo(),
                appWorkPermExpiry: self.appWorkPermExpiry(),
                addrInCountry: self.addrInCountry(),
                workPermNo: self.workPermNo(),
                officerID: self.officerID(),
                applicantID: self.applicantID(),
                s_complete_ind: completeIndicator,
                imageguid: $("#hiddenImageGuid").val()

            },
            beforeSubmit: function () {
                
            },
            uploadProgress: function () {
             
            },
            success: function (json) {

                //alert("details have been updated");
                //$("#uploadImageDiv").css("background-image", "url(../pdf/" + result.imagepath + ")");
                if (json.Result == "0") {
                    $('.formclosebutton').trigger("click");
                } else {
                    alert("there was an error");
                }
            }
        };


        if (completeIndicator == "Y") {

            $('#inputForm').validate({
                errorContainer: "#errorContainer",
                errorLabelContainer: "#errorContainer",
                errorElement: "li",

                rules: {
                    surnameInput: 'required',
                    givenNameInput: 'required',
                    dobInput: 'required',
                    nationalityInput: 'required',
                    cobInput: 'required',
                    occupationInput: 'required'
                }

            }).form();

        } else {
            $('#inputForm').validate({
                errorContainer: "#errorContainer",
                errorLabelContainer: "#errorContainer",
                errorElement: "li",

                rules: {

                }

            }).form();
        }

        if ($("#inputForm").validate().numberOfInvalids() == 0) {
            $('#inputForm').ajaxForm(options);

        }
                
        
        return true;

    }

    this.updateApplicantDetails = function () {

        $.ajax({
            type: "GET",
            url: "/Permit/updateApplicantDetails",
            async: false,
            data: {

                permitID: self.permitID(),
                fileNo: self.fileNo(),
                permitType: self.permitType(),
                dateLodged: self.dateLodged(),
                whereLodged: self.whereLodged(),
                permitPeriod: self.permitPeriod(),
                entriesAllowed: self.entriesAllowed(),
                dependents: self.dependents(),
                express: self.express(),
                feeAmount: self.feeAmount(),
                paidAt: self.paidAt(),
                paidOn: self.paidOn(),
                receptNo: self.receiptNo(),
                poffshor: self.poffshore(),
                sReturn: self.sReturn(),
                offShore: self.offShore(),
                title: self.title(),
                surname: self.surname(),
                givenName: self.givenName(),
                nationality: self.nationality(),
                cob: self.cob(),
                pob: self.pob(),
                occupation: self.occupation(),
                gender: self.gender(),
                dob: self.dob(),
                maritalStatus: self.maritalStatus(),
                passportNo: self.passportNo(),
                add1: self.add1(),
                add2: self.add2(),
                add3: self.add3(),
                email: self.email(),
                phone_no: self.phone_no(),
                docExpiry: self.docExpiry(),
                positionNo: self.positionNo(),
                appWorkPermExpiry: self.appWorkPermExpiry(),
                addrInCountry: self.addrInCountry(),
                workPermNo: self.workPermNo(),
                officerID: self.officerID(),
                applicantID: self.applicantID()


            },
            success: function (result) {
                alert("success");
            }
        });

    }
    this.goToPermitList = function () {
        
        var sPageURL = window.location.search.substring(1);
        var sURLVariables = sPageURL.split('&');
        var permitID;
        for (var i = 0; i < sURLVariables.length; i++) {
            var sParameterName = sURLVariables[i].split('=');
            if (sParameterName[0] == "permitID") {
                permitID = sParameterName[1];
            }
        }

        location.href = '/?permitID=' + permitID;
    }


}