ko.bindingHandlers.fadeVisible = {
    init: function (element, valueAccessor) {
        // Initially set the element to be instantly visible/hidden depending on the value
        var value = valueAccessor();
        $(element).toggle(ko.utils.unwrapObservable(value)); // Use "unwrapObservable" so we can handle values that may or may not be observable
    },
    update: function (element, valueAccessor) {
        // Whenever the value subsequently changes, slowly fade the element in or out
        var value = valueAccessor();
        ko.utils.unwrapObservable(value) ? $(element).fadeIn("fast") : $(element).fadeOut("fast");
    }
};

function VisaRegistrationViewModel() {
    var self = this;

    //office details
    this.lodgedfor = ko.observable($("#HiddenLodgedFor").val());

    //visa details
    this.visaCode = ko.observable("");
    this.visaClass = ko.observable("");
    this.receiptNo = ko.observable("");
    this.feeText = ko.observable("");
    this.confirmed = ko.observable("");
    this.receiptDialogSelect = ko.observable("");

    //document details
    this.docType = ko.observable("");
    this.nationality = ko.observable("");
    this.docNumber = ko.observable("");
    this.reDocNumber = ko.observable("");
    this.expiryDate = ko.observable("");
    this.surname = ko.observable("");
    this.givenName = ko.observable("");
    this.sex = ko.observable("");
    this.dob = ko.observable("");

    //sponsor details
    this.sponsorName = ko.observable("");
    this.sponsorGuid = ko.observable("");
    this.sponsorSelected = ko.observable(false);
    
    //project details
    this.projectSelected = ko.observable("");
    this.projectID = ko.observable("");
    this.primeEmployer = ko.observable("");
    this.headEmployer = ko.observable("");
    this.subEmployer = ko.observable("");
    this.comments = ko.observable("");

    //misc
    this.receiptNotice = ko.observable("");
    this.docNumNotice = ko.observable("");
    this.surNameNotice = ko.observable("");
    this.nameNotice = ko.observable("");

    //sections
    this.applicantDetails = ko.observable(false);
    this.documentDetails = ko.observable(false);
    this.adtnlDocDets = ko.observable(false);
    this.sponsorDetails = ko.observable(false);
    this.projectDetails = ko.observable(false);
    this.extraDetails = ko.observable(false);

    //true if web service says sponsor is required
    this.SponsorEnabled = ko.observable(false);

    //undertakings
    this.checkBox1 = ko.observable(false);
    this.checkBox2 = ko.observable(false);
    this.checkBox3 = ko.observable(false);
    this.checkBox4 = ko.observable(false);
    this.checkBox5 = ko.observable(false);
    this.checkBox6 = ko.observable(false);
    this.checkBox7 = ko.observable(false);

    //misc
    this.imgguid = ko.observable("");

    //setInterval(function () { alert("Hello") }, 3000);

    //get webAlias to append onto ajax url
    var alias = $('#webAppAlias').val();


    //get current date and display in logden on field
    var date = new Date().toUTCString().split(',')[1].split(' ');
    $("#lodgedOnDateInput").html( date[1] + "-" + date[2] + "-" + date[3]);

    $("#expDate").datepicker({
        dateFormat: 'dd-M-yy', //e.g 10-Oct-2013
        changeMonth: true,
        changeYear: true,
        yearRange: '-70:+10',
        constrainInput: false,
        duration: '',
        gotoCurrent: true
    });
    $("#dob").datepicker({
        dateFormat: 'dd-M-yy', //e.g 10-Oct-2013
        changeMonth: true,
        changeYear: true,
        yearRange: '-70:+10',
        constrainInput: false,
        duration: '',
        gotoCurrent: true
    });

    /*$("#sponsorDialog").dialog({
        columnSelectable: false,
        resizable: false,
        dialogClass: 'teststyle',
        minHeight: 385,
        maxHeight:500,
        width: 1360,
        show: { effect: 'fade', duration: 500 },
        hide: { effect: 'fade', duration: 200 },
        autoOpen: false,
        title: "Select Sponsor",
        close: function (event, ui) { }
    });

    $("#countryDialog").dialog({
        resizable: false,
        dialogClass: 'teststyle',
        minHeight: 385,
        width: 495,
        height:340,
        show: { effect: 'fade', duration: 500 },
        hide: { effect: 'fade', duration: 200 },
        autoOpen: false,
        title: "Select a nationality",
        close: function (event, ui) { }
    });
    $("#successMessage").dialog({
        resizable: false,
        dialogClass: 'teststyle',
        height: 150,
        width: 462,
        show: { effect: 'fade', duration: 500 },
        hide: { effect: 'fade', duration: 200 },
        autoOpen: false,
        title: "Visa Application Reference Number",
        close: function (event, ui) { }
    });
    $("#receiptMessage").dialog({
        resizable: false,
        dialogClass: 'teststyle',
        height: 150,
        width: 300,
        show: { effect: 'fade', duration: 500 },
        hide: { effect: 'fade', duration: 200 },
        autoOpen: false,
        title: "Note:",
        close: function (event, ui) { }
    });

    $("#NoReceiptDialog").dialog({
        resizable: false,
        dialogClass: 'teststyle',
        height: 150,
        width: 320,
        show: { effect: 'fade', duration: 500 },
        hide: { effect: 'fade', duration: 200 },
        autoOpen: false,
        title: "No Receipt Number",
        close: function (event, ui) { }
    });

    $("#sponsorInput").click(function () {
        self.sponsorName("");
        self.sponsorSelected(false);
        $("#sponsorDetailsInfoBox").html("");
    });

    $('#sponsorInput').keydown(function (event) {
        if (event.which == 9) {
            self.getSponsorDialog();
        }
    });

    $('#sponsorDialogTable').jtable({
        sorting: true, //Enable sorting
        selecting: true,
        width: 500,
        actions: {
            listAction: 'VisaRegistration/getSponsorsList'
        },
        fields: {
            guID: {
                title: 'ID',
                create: false,
                edit: false,
                visibility: 'hidden'
            },
            companyName: {
                title: 'NAME',
                create: false,
                edit: false,
                key:true
            },
            address: {
                title: 'ADDRESS',
                create: false,
                edit: false,
            },
            address2: {
                title: 'ADDRESS2',
                create: false,
                edit: false,
                visibility: 'hidden'
            },
            address3: {
                title: 'ADDRESS3',
                create: false,
                edit: false,
                visibility: 'hidden'
            },
            email_addr: {
                title: 'email_Addr',
                create: false,
                edit: false,
                visibility: 'hidden'
            },
            fax_no: {
                title: 'fax_no',
                create: false,
                edit: false,
                visibility: 'hidden'
            },
            given_name: {
                title: 'given_name',
                create: false,
                edit: false,
                visibility: 'hidden'
            },
            phone_no: {
                title: 'phone_no',
                create: false,
                edit: false,
                visibility: 'hidden'
            },
            postal_addr_1: {
                title: 'postal_addr_1',
                create: false,
                edit: false,
                visibility: 'hidden'
            },
            postal_addr_2: {
                title: 'postal_addr_2',
                create: false,
                edit: false,
                visibility: 'hidden'
            },
            postal_addr_3: {
                title: 'postal_addr_3',
                create: false,
                edit: false,
                visibility: 'hidden'
            },
            principal_position: {
                title: 'principal_position',
                create: false,
                edit: false,
                visibility: 'hidden'
            },
            surname: {
                title: 'surname',
                create: false,
                edit: false,
                visibility: 'hidden'
            },
            title_description: {
                title: 'title_description',
                create: false,
                edit: false,
                visibility: 'hidden'
            },
            status_name: {
                title: 'status_name',
                create: false,
                edit: false,
                visibility: 'hidden'
            },
            passport_number: {
                title: 'passport_number',
                create: false,
                edit: false,
                visibility: 'hidden'
            }
        },
        recordsLoaded: function (data) {
            var firstRow = $("#sponsorDialogTable .jtable-main-container tbody tr:first ");
            $('#sponsorDialogTable').jtable('selectRows', firstRow);
        },
        selectionChanged: function (data) {

            var selectedRows = $('#sponsorDialogTable').jtable('selectedRows');
            //alert(selectedRows.data('record').companyName);
            if (selectedRows.data('record') == null) {
                $("#sponsorCompany").html("-");
                $("#sponsorAddr1").html("-");
                $("#sponsorAddr2").html("-");
                $("#sponsorAddr3").html("-");
                $("#sponsorPAddr1").html("-");
                $("#sponsorPAddr2").html("-");
                $("#sponsorPAddr3").html("-");
                $("#sponsorEmail").html("-");
                $("#sponsorPrincipalPosition").html("-");
                $("#sponsorTitle").html("-");
                $("#sponsorEmail").html("-");
                $("#sponsorTitle").html("-");
                $("#sponsorSurname").html("-");
                $("#sponsorPassportNumber").html("-");
                $("#sponsorStatus").html("-");
            } else {

                //extract id 
                var record = selectedRows.data('record');
               
                $("#hiddenSponsorCompanyName").val(record.companyName);
                $("#hiddenSponsorGUID").val(record.guID);
                $("#hiddenSponsorAddress").val(record.address);
                $("#hiddenSponsorAddress2").val(record.address2);

                $("#sponsorCompany").html(record.companyName);
                $("#sponsorAddr1").html(record.address);
                $("#sponsorAddr2").html(record.address2);
                $("#sponsorAddr3").html(record.address3);
                $("#sponsorPAddr1").html(record.postal_addr_1);
                $("#sponsorPAddr2").html(record.postal_addr_2);
                $("#sponsorPAddr3").html(record.postal_addr_3);
                $("#sponsorEmail").html(record.email_addr);
                $("#sponsorPrincipalPosition").html(record.principal_position);
                $("#sponsorTitle").html(record.email_addr);
                $("#sponsorEmail").html(record.email_addr);
                $("#sponsorTitle").html(record.title_description);
                $("#sponsorSurname").html(record.surname);
                $("#sponsorPassportNumber").html(record.passport_number);
                $("#sponsorPhone").html(record.phone_no);
                $("#sponsorFax").html(record.fax_no);
                $("#sponsorGiveName").html(record.given_name);
                $("#sponsorStatus").html(record.status_guid);
            }

        }
    });

    this.postSponsor = function () {
        self.sponsorSelected(true);
        self.sponsorName($("#hiddenSponsorCompanyName").val());
        self.sponsorGuid($("#hiddenSponsorGUID").val());
        $('#selectedSponsor').html("<b> Selected: " + $("#hiddenSponsorCompanyName").val() + "</b>");
        $('#sponsorDetailsInfoBox').html("Sponsor Details:" + "<br/>" + "<b>" + $("#hiddenSponsorCompanyName").val() + "</b><br/>" + $("#hiddenSponsorAddress").val() + "<br/>" + $("#hiddenSponsorAddress2").val() + "<br/>");
        self.closeSponsorWindow();
    }

    $('#countryDialogTable').jtable({


        sorting: true, //Enable sorting
        selecting: true,
        width: 300,
        actions: {
            listAction: 'VisaRegistration/getCountriesList'
        },
        fields: {
            name: {
                title: 'Country Name',
                create: false,
                edit: false
            },
            code: {
                title: 'Country Code',
                create: false,
                edit: false
            }
        },
        selectionChanged: function (data) {

            var selectedRows = $('#countryDialogTable').jtable('selectedRows');

            //extract id 
            var record = selectedRows.data('record');
            $('#countryDialog').dialog("close");
            self.nationality(record.code);

           
        }
    });
    $("#countryDialogTable").jtable("load");*/


    //computed functions
    this.visaCodeChanged = ko.computed(function () {

        if (this.visaCode().length > 0) {

            this.filterVisaClassesByTypeID(this.visaCode());

        }
        else {
            //if default option is picked:
            $('#visaClassesDropDown').html('');
            $('#visaClassesDropDown').prop('disabled', true);
            $("#conditionsList").html('<i>[none]</i>');
        }
        return this.visaCode();
    }, this);
    this.receiptDialogChanged = ko.computed(function () {
      
        if (this.receiptDialogSelect().length > 0) {

            this.receiptNo(this.receiptDialogSelect());

        }
    
    }, this);
    this.nationalityCompute = ko.computed(function () {
        if (this.nationality().length > 0) {

            $("#countryFlagImage").attr("src", "Content/Images/Flags/" + this.nationality() + ".gif");
          
        }
   
    }, this);
    this.SponsorEnabledCompute  = ko.computed(function () {
        if (!this.SponsorEnabled()) {
            this.sponsorDetails(false);
        }

    }, this);
    this.visaClassComputed = ko.computed(function () {

        if (this.visaClass().length > 0) {
            this.changeVisaDetails(this.visaClass());
            this.getProjectsList();
        }
        else {
            $("#conditionsList").html('<i>[none]</i>');
            $("#feeText").html('');
        }
    }, this);

    this.visaDetailsCompute = ko.computed(function () {

        if (this.confirmed() == "N") {
            self.receiptDialogSelect("");
            self.docType("");
            self.nationality("");
            self.docNumber("");
            self.reDocNumber("");
            self.expiryDate("");
            self.surname("");
            self.givenName("");
            self.sex("");
            self.dob("");
            self.sponsorName("");
            self.sponsorGuid("");
            self.projectSelected("");
            self.projectID("");
            self.primeEmployer("");
            self.headEmployer("");
            self.subEmployer("");
            self.comments("");
            self.receiptNotice("");
            self.docNumNotice("");
            self.surNameNotice("");
            self.nameNotice("");
            self.applicantDetails(false);
            self.documentDetails(false);
            self.adtnlDocDets(false);
            self.sponsorDetails(false);
            self.projectDetails(false);
            self.sponsorSelected(false);
            self.checkBox1 = ko.observable(false);
            self.checkBox2 = ko.observable(false);
            self.checkBox3 = ko.observable(false);
            self.checkBox4 = ko.observable(false);
            self.checkBox5 = ko.observable(false);
            self.checkBox6 = ko.observable(false);
            self.checkBox7 = ko.observable(false);
            $("#sponsorDetailsInfoBox").html("");
        }

        if (this.visaCode().length > 0 && 
            this.visaClass().length > 0 &&
            this.receiptNo().length > 0 &&
            this.confirmed() == "Y")
        {
            this.documentDetails(true);
            this.docType("P");
            //this.checkReceiptNumber();
        }
        else {
            this.documentDetails(false);
            this.sponsorDetails(false);
        }
        
        return this.visaCode() + this.visaClass() + this.receiptNo() + this.confirmed();

    }, this);

    this.ReceiptNoCompute = ko.computed(function () {

        if (this.receiptNo().length > 0) {
            this.checkReceiptNumber();
        }
    }, this);

    this.docNumberCompute = ko.computed(function () {

        if (this.docNumber().length > 0 &&
            this.nationality().length > 0 &&
            this.docType().length > 0) {
                this.getBioDataDetails();
        }

        return this.docNumber();
    }, this);
    this.documentDetailsCompute = ko.computed(function () {

        if (this.docType().length > 0 &&
           this.nationality().length > 0 &&
           this.docNumber().length > 0 &&
           this.expiryDate().length > 0 ) {

            this.applicantDetails(true);
        }
        else {
            this.applicantDetails(false);
        }

        return this.docType() + this.nationality() + this.docNumber() + this.expiryDate()+this.surname()+this.givenName()+this.sex()+this.dob();
    }, this);
    this.applicantDetailsCompute = ko.computed(function () {
        if (this.surname().length > 0 &&
           this.givenName().length > 0 &&
           this.sex().length > 0 &&
           this.dob().length > 0) {

            if (this.SponsorEnabled()) {
                this.sponsorDetails(true);
                this.extraDetails(true);
            }
            else {
                self.extraDetails(true);
            }
            
        }
        else {
            this.sponsorDetails(false);
            self.extraDetails(false);
        }
    }, this);
    this.projectCompute = ko.computed(function () {

        if (this.projectSelected() == "Y")
            this.projectDetails(true);
        else
            this.projectDetails(false);

    }, this);
    this.projectIDCompute = ko.computed(function () {

        if (this.projectID().length > 0) {
            $.ajax({
                dataType: "json",
                url: "VisaRegistration/getPrimeEmployerDetails",
                async: false,
                data: { projectID: self.projectID() },
                success: function (result) {

                    $("#primeEmployerDropDown").empty();
                    $("#primeEmployerDropDown").append('<option value="">-Prime-</option>');


                    $("#headEmployerDropDown").empty();
                    $("#headEmployerDropDown").append('<option value="">-Head-</option>');

                    $("#subEmployerDropDown").empty();
                    $("#subEmployerDropDown").append('<option value="">-Sub-</option>');

                    $.each(result.Records.common_codes_dets, function (i, item) {
                        $("#primeEmployerDropDown").append('<option value="' + item.code_guid + '">' + item.code_desc + '</option>');

                    });

                    $.ajax({
                        dataType: "json",
                        url: "VisaRegistration/getProjectUndertakings",
                        async: false,
                        data: {
                            ptype: self.visaCode(),
                            pclass: self.visaClass(),
                            projectID: self.projectID()
                        },
                        success: function (result) {

                            if (result.Number > 0) {
                                var records = result.Records.checklist_dets;

                                var i = 0;
                                for (i; i < result.Number; i++) {

                                    var id = i + 1;
                                    $("#cb" + id).removeClass();
                                    $("#cb" + id).val(records[i].check_guid);
                                    $("#cbt" + id).removeClass();
                                    $("#cbt" + id).html(records[i].check_desc);
                                }

                                $("#cb6").removeClass();
                                $("#cb7").removeClass();
                                $("#cbt6").removeClass();
                                $("#cbt7").removeClass();
                            }

                        }
                    });
                }
            });
        }
        else {
            //undertakings
            this.checkBox1("");
            this.checkBox2("");
            this.checkBox3("");
            this.checkBox4("");
            this.checkBox5("");
            this.checkBox6("");
            this.checkBox7("");
        }

    }, this);
    this.primeEmployerCompute = ko.computed(function () {

        if (this.primeEmployer().length > 0) {

            $.ajax({
                dataType: "json",
                url: "VisaRegistration/getHeadEmployerDetails",
                async: false,
                data: {
                    projectID: self.projectID(),
                    primeID: self.primeEmployer()
                },
                success: function (result) {

                    $("#headEmployerDropDown").empty();
                    $("#headEmployerDropDown").append('<option value="">-Head-</option>');

                    $("#subEmployerDropDown").empty();
                    $("#subEmployerDropDown").append('<option value="">-Sub-</option>');

                    $.each(result.Records.common_codes_dets, function (i, item) {
                        $("#headEmployerDropDown").append('<option value=' + item.code_guid + '>' + item.code_desc + '</option>');
                    });


                }
            });
        }
        else {
            $("#headEmployerDropDown").empty();
            $("#headEmployerDropDown").append('<option value="">-Head-</option>');

            $("#subEmployerDropDown").empty();
            $("#subEmployerDropDown").append('<option value="">-Sub-</option>');
        }

    }, this);
    this.headEmployerCompute = ko.computed(function () {

        if (this.headEmployer().length > 0) {

            $.ajax({
                dataType: "json",
                url: "VisaRegistration/getSubEmployerDetails",
                async: false,
                data: {
                    projectID: self.projectID(),
                    primeID: self.primeEmployer(),
                    headID: self.headEmployer()
                },
                success: function (result) {

                    $("#subEmployerDropDown").empty();
                    $("#subEmployerDropDown").append('<option value="">-Sub-</option>');

                    $.each(result.Records.common_codes_dets, function (i, item) {
                        $("#subEmployerDropDown").append('<option value=' + item.code_guid + '>' + item.code_desc + '</option>');
                    });


                }
            });

        }
        

    }, this);
    this.CheckAllUndertakings = ko.computed(function () {

        if (this.checkBox7()) {
            $('#projectDetailsInfoBox').find('input[type="checkbox"]').each(function (i) {
                if (!$(this).hasClass("hidden")) 
                    $(this).prop('checked', true);   
            });
        }
        //uncheck all
        else {
            $('#projectDetailsInfoBox').find('input[type="checkbox"]').each(function (i) {
                if (!$(this).hasClass("hidden"))
                    $(this).prop('checked', false);
            });
        }
    }, this);


    //normal functions
    this.getProjectsList = function () {
        if (this.visaClass().length > 0) {
            var self = this;
            $.ajax({
                dataType: "json",
                url: "VisaRegistration/getProjectsList",
                data: {
                    visaType: self.visaCode(),
                    visaClass: self.visaClass()
                },
                success: function (result) {
          
                    var data = result;
                    var i = 0;
                    $('#sponsorProjectDropDown').html('');
                    $("#sponsorProjectDropDown").append('<option value="">-select-</option>');

                    if (result.Records.common_codes_dets.length > 0) {
                        for (i; i < result.Records.common_codes_dets.length; i++) {

                            $("#sponsorProjectDropDown").prop('disabled', false);
                            $("#primeEmployerDropDown").prop('disabled', false);
                            $("#headEmployerDropDown").prop('disabled', false);
                            $("#subEmployerDropDown").prop('disabled', false);
                            $("#sponsorProjectDropDown").append('<option value=' + result.Records.common_codes_dets[i].code_guid + '>' + result.Records.common_codes_dets[i].code_desc + '</option>');
                        }
                    } else {
                        $("#sponsorProjectDropDown").prop('disabled', true);
                        $("#primeEmployerDropDown").prop('disabled', true);
                        $("#headEmployerDropDown").prop('disabled', true);
                        $("#subEmployerDropDown").prop('disabled', true);
                    }

                    
                    if (result.Records.common_codes_dets.length > 0)
                        self.projectSelected(true);
                    else
                        self.projectSelected(false);

                    self.visaClass($("#visaClassesDropDown").val());
                }
            });
        }
    }
    
    this.closeSuccessWindow = function () {
        $("#successMessage").dialog("close");
        $('#documentDetailsForm').submit();
        self.eraseForm();
    }
    this.closeWindow = function (window) {
        $('#' + window+'').fadeOut("fast");
    }
    this.closeCountryWindow = function () {
        $('#countryDialog').dialog("close");
    }

    this.filterVisaClassesByTypeID = function (visatypeId) {
        
        var self = this;
        $.ajax({
            dataType: "json",
            url: "VisaRegistration/getFilteredClasses",
            data: { visaId: visatypeId },
            success: function (result) {
                
                var data = result;
                var i = 0;
                $('#visaClassesDropDown').html('');
                $("#visaClassesDropDown").append('<option value="">-select-</option>');

                for (i; i < data.length; i++) {
                    var select = document.getElementById("visaClassesDropDown");

                    var details = data[i].split("|");

                    $("#visaClassesDropDown").append('<option value=' + details[1] + '>' + details[0] + '</option>');
                    $('#visaClassesDropDown').prop('disabled', false);
                }
                self.visaClass($("#visaClassesDropDown").val());
            }
        });
    };
    this.changeVisaDetails = function (visaClassId) {
     
        $("#sponsorDetailsInfoBox").html("To search for a sponsor, input a search query and click the info icon");

        var self = this;
        $.ajax({
            dataType: "json",
            url: "VisaRegistration/changeVisaDetails",
            data: { guid: visaClassId },
            success: function (result) {
    
                var data = result;
                var data = data.toString().split("|");
                $('#conditionsList').html('');

                var i = 0;
                //minus 2 so the final "|" isnt processed as a separate element
                for (i; i < data.length-2; i++) {
                    var details = data[i];

                    $("#conditionsList").append('<li>'+data[i]+'</li>');
                }
                self.feeText("PGK " + data[data.length - 2]);
                $("#feeText").html("PGK " + data[data.length - 2]);

       
                if (data[data.length - 1] == 'N') {
                    self.SponsorEnabled(false);

                }
                else {
                    self.SponsorEnabled(true);

                }
            }
        });
    };
    this.registerApplication = function (withReceipt) {
       
        //special rule for contact number dependencies.
        $.validator.addMethod("nameValidate", function (value, element) {
            var outcome;

            $.ajax({
                type: "GET",
                url: "VisaRegistration/checkBioDataString",
                async: false,
                data: {
                    biodata: value.toUpperCase()
                },
                success: function (result) {
                   
                    outcome = result;
                }
            });
            
            if (outcome == "Y")
                return true;
            else
                return false;

        }, "Invalid name");

        $.validator.addMethod("surnameValidate", function (value, element) {
            var outcome;

            $.ajax({
                type: "GET",
                url: "VisaRegistration/checkBioDataString",
                async: false,
                data: {
                    biodata: value.toUpperCase()
                },
                success: function (result) {

                    outcome = result;
                }
            });

            if (outcome == "Y")
                return true;
            else
                return false;
        }, "Invalid surname");

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
        
        $('#documentDetailsForm').validate({
            errorContainer: "#errorContainer",
            errorLabelContainer: "#errorContainer",
            errorElement: "li",

            rules: {
                applicantName: { nameValidate: true },
                applicantSurname: { surnameValidate: true },
                reDocNum: { equalTo: '#docNum' },
                dob: {
                    required: true,
                    pastDateValidate: true
                },
                expDate: {
                    required: true,
                    futureDateValidate: true
                }
            }

        }).form();
        
        if ($("#documentDetailsForm").validate().numberOfInvalids() == 0) {
            self.createNewVisa(withReceipt);
        }
        else {
            $("#registerErrorText").show();
        }

    };
    this.checkReceiptNumber = function () {
        var self = this;

        $.ajax({
            type: "GET",
            url: "VisaRegistration/checkReceiptNo",
            async: false,
            data: {
                receiptNo: self.receiptNo()
            },
            success: function (result) {

                if (result > 0) {
                    self.receiptNotice("Times used: " + result);
                    $("#receiptMessage").fadeIn("fast");
                } else {
                    self.receiptNotice("");
                }
            }
        });
        
    };
    this.getBioDataDetails = function () {
        var self = this;

        var dt = this.docType();
        var nat = this.nationality();
        var dn = this.docNumber();

        $.ajax({
            type: "GET",
            url: "VisaRegistration/getBioDataDetails",
            async: false,
            data: {
                docType: dt,
                nationality: nat,
                docNum: dn
            },
            success: function (result) {


                if (result.Result == "notfound") {
                    self.adtnlDocDets(true);
                    self.reDocNumber("");
                    self.expiryDate("");
                    self.surname("");
                    self.givenName("");
                    self.sex("");
                    self.dob("");
                    $("#profileImagecontainer img").attr("src", "Content/Images/defaultprofilepic2.png");
                }
                else {
                    self.adtnlDocDets(true);
                    self.reDocNumber(result.docNum);
                    self.expiryDate(result.expirydate);
                    self.surname(result.surname);
                    self.givenName(result.givenName);
                    self.sex(result.sex);
                    self.dob(result.dob);
                    if (result.imgguid != "") {
                        $("#profileImagecontainer img").attr("src", "pdf/" + result.imgguid + ".jpg");
                        self.imgguid(result.imgguid);
                    }
                }

              

                if (self.docNumber().length == 0) {
                    self.adtnlDocDets(false);
                }
            }
        });

    };
    this.searchForSponsor = function () {
        $.ajax({
            type: "GET",
            url: "VisaRegistration/sponsorList",
            data: {
                searchQuery: self.sponsorName()
            },
            success: function (result) {
                $("#sponsorPopup").html(result);
                $("#sponsorPopup").fadeIn("fast");
                var oTable = $('#sponsorTable').dataTable({
                    iDisplayLength: 50,
                    "aaSorting": [[0, "desc"]],
                    "bLengthChange": false,
                    "oLanguage": {
                        "sSearch": "<b>Enquiry Filter: </b>"
                    }
                });
                
            }
        });

    };
    this.getSponsorDialog = function () {
        if (self.sponsorName().length > 0) {
            //self.sponsorName() 
            
            self.searchForSponsor();
            $("#sponsorDialog").dialog("open");
            $("#sponsorDialog").css("background-color", "RGB(203,220,249)");
        }
        else {
            alert("Please type a sponsor name");
        }
    };
    this.closeSponsorWindow = function () {
        $("#sponsorDialog").dialog("close");
        $("#sponsorDialogTable").jtable("destroy");
        self.reinitialiseSponsorDialog();

    }
    this.openNoReceiptDialog = function () {
        $("#NoReceiptDialog").fadeIn("fast");
    };
    this.getCountryList = function () {
        
        $('#countryDialog').dialog("open");
        $('#countryDialog').css("background-color", "rgb(182, 197, 224)");

    };
    this.sponsorRowClicked = function (name,guid) {
     
        name = name.replace(/¬/g, "'");
        self.sponsorSelected(true);
        self.sponsorName(name);
        self.sponsorGuid(guid);
        $('#selectedSponsorText').html("<b> Sponsor selected:" + name + "</b>");
        $('#selectedSponsor').html("<b> Selected: " + name + "</b>");
     
    };
    this.countryRowClicked = function (name,code) {

        name = name.replace(/¬/g, "'");

        self.nationality(code);
        $('#selectedCountryText').html("<b> Country selected:" + name + "</b>");
    };
    this.createNewVisa = function (withReceipt) {
  

        $("#registerErrorText").hide();

        //save all checkIDs to array 
        var checkIDs = new Array();
        $('#projectDetailsInfoBox').find('input[type="checkbox"]').each(function (i) {
            if (!$(this).hasClass("hidden"))
                if ($(this).prop('checked'))
                    checkIDs.push($(this).val());
        });

        var newpermitNumber;
        $.ajax({
            type: "json",
            url: "VisaRegistration/registerApplication",
            async: false,
            traditional: true,
            data: {

                pTypeID: self.visaCode(),
                pClassID: self.visaClass(),
                receiptNo: self.receiptNo(),
                docType: self.docType(),
                nationID: self.nationality(),
                docNo: self.docNumber(),
                expiryDate: self.expiryDate(),
                surname: self.surname().toUpperCase(),
                givenName: self.givenName().toUpperCase(),
                genderCode: self.sex(),
                dob: self.dob(),
                sponsorGuid: self.sponsorGuid(),
                sponsorName: self.sponsorName(),
                projectID: self.projectID(),
                comments: self.comments(),
                primeID: self.primeEmployer(),
                headID: self.headEmployer(),
                subID: self.subEmployer(),
                checksIDs: checkIDs,
                curPerGuid:  "",
                guarantee: "",
                withReceipt: withReceipt,
                imgguid: self.imgguid()

            },
            success: function (result) {

                newpermitNumber = result.varn;

                if (result.fileName == "none") {
                    $("#idspan").html("<b> " + newpermitNumber + "</b>");
                    $("#successMessage").dialog("open");
                    $("#successMessage").css("background-color", "rgb(182, 197, 224)");
                }
                else {
                    var wind = window.open(result.fileName, "custody receipt", 'width=800,height=800');
                    self.eraseForm();
                }
            }
        });

    };
    this.closeNewVisaDialogue = function () {
        $("#NewVisaFormWindow").fadeOut(200);

        self.eraseForm();
    };
    this.reinitialiseSponsorDialog = function () {
        $('#sponsorDialogTable').jtable({
            sorting: true, //Enable sorting
            selecting: true,
            width: 500,
            actions: {
                listAction: 'VisaRegistration/getSponsorsList'
            },
            fields: {
                guID: {
                    title: 'ID',
                    create: false,
                    edit: false,
                    visibility: 'hidden'
                },
                companyName: {
                    title: 'NAME',
                    create: false,
                    edit: false,
                    key: true
                },
                address: {
                    title: 'ADDRESS',
                    create: false,
                    edit: false,
                },
                address2: {
                    title: 'ADDRESS2',
                    create: false,
                    edit: false,
                    visibility: 'hidden'
                },
                address3: {
                    title: 'ADDRESS3',
                    create: false,
                    edit: false,
                    visibility: 'hidden'
                },
                email_addr: {
                    title: 'email_Addr',
                    create: false,
                    edit: false,
                    visibility: 'hidden'
                },
                fax_no: {
                    title: 'fax_no',
                    create: false,
                    edit: false,
                    visibility: 'hidden'
                },
                given_name: {
                    title: 'given_name',
                    create: false,
                    edit: false,
                    visibility: 'hidden'
                },
                phone_no: {
                    title: 'phone_no',
                    create: false,
                    edit: false,
                    visibility: 'hidden'
                },
                postal_addr_1: {
                    title: 'postal_addr_1',
                    create: false,
                    edit: false,
                    visibility: 'hidden'
                },
                postal_addr_2: {
                    title: 'postal_addr_2',
                    create: false,
                    edit: false,
                    visibility: 'hidden'
                },
                postal_addr_3: {
                    title: 'postal_addr_3',
                    create: false,
                    edit: false,
                    visibility: 'hidden'
                },
                principal_position: {
                    title: 'principal_position',
                    create: false,
                    edit: false,
                    visibility: 'hidden'
                },
                surname: {
                    title: 'surname',
                    create: false,
                    edit: false,
                    visibility: 'hidden'
                },
                title_description: {
                    title: 'title_description',
                    create: false,
                    edit: false,
                    visibility: 'hidden'
                },
                status_name: {
                    title: 'status_name',
                    create: false,
                    edit: false,
                    visibility: 'hidden'
                },
                passport_number: {
                    title: 'passport_number',
                    create: false,
                    edit: false,
                    visibility: 'hidden'
                }
            },
            recordsLoaded: function (data) {
                var firstRow = $("#sponsorDialogTable .jtable-main-container tbody tr:first ");
                $('#sponsorDialogTable').jtable('selectRows', firstRow);
            },
            selectionChanged: function (data) {

                var selectedRows = $('#sponsorDialogTable').jtable('selectedRows');
                //alert(selectedRows.data('record').companyName);
                if (selectedRows.data('record') == null) {
                    $("#sponsorCompany").html("-");
                    $("#sponsorAddr1").html("-");
                    $("#sponsorAddr2").html("-");
                    $("#sponsorAddr3").html("-");
                    $("#sponsorPAddr1").html("-");
                    $("#sponsorPAddr2").html("-");
                    $("#sponsorPAddr3").html("-");
                    $("#sponsorEmail").html("-");
                    $("#sponsorPrincipalPosition").html("-");
                    $("#sponsorTitle").html("-");
                    $("#sponsorEmail").html("-");
                    $("#sponsorTitle").html("-");
                    $("#sponsorSurname").html("-");
                    $("#sponsorPassportNumber").html("-");
                    $("#sponsorStatus").html("-");
                } else {

                    //extract id 
                    var record = selectedRows.data('record');
             
                    $("#hiddenSponsorCompanyName").val(record.companyName);
                    $("#hiddenSponsorGUID").val(record.guID);
                    $("#hiddenSponsorAddress").val(record.address);
                    $("#hiddenSponsorAddress2").val(record.address2);
              
                    $("#sponsorCompany").html(record.companyName);
                    $("#sponsorAddr1").html(record.address);
                    $("#sponsorAddr2").html(record.address2);
                    $("#sponsorAddr3").html(record.address3);
                    $("#sponsorPAddr1").html(record.postal_addr_1);
                    $("#sponsorPAddr2").html(record.postal_addr_2);
                    $("#sponsorPAddr3").html(record.postal_addr_3);
                    $("#sponsorEmail").html(record.email_addr);
                    $("#sponsorPrincipalPosition").html(record.principal_position);
                    $("#sponsorTitle").html(record.email_addr);
                    $("#sponsorEmail").html(record.email_addr);
                    $("#sponsorTitle").html(record.title_description);
                    $("#sponsorSurname").html(record.surname);
                    $("#sponsorPassportNumber").html(record.passport_number);
                    $("#sponsorPhone").html(record.phone_no);
                    $("#sponsorFax").html(record.fax_no);
                    $("#sponsorGiveName").html(record.given_name);
                    $("#sponsorStatus").html(record.status_guid);
                }

            }
        });
    }
    
    this.eraseForm = function () {
        
        self.visaCode("");
        self.visaClass("");
        self.receiptNo("");
        self.feeText("");
        self.confirmed("");
        self.receiptDialogSelect("");
        self.docType("");
        self.nationality("");
        self.docNumber("");
        self.reDocNumber("");
        self.expiryDate("");
        self.surname("");
        self.givenName("");
        self.sex("");
        self.dob("");
        self.sponsorName("");
        self.sponsorGuid("");
        self.projectSelected("");
        self.projectID("");
        self.primeEmployer("");
        self.headEmployer("");
        self.subEmployer("");
        self.comments("");
        self.receiptNotice("");
        self.docNumNotice("");
        self.surNameNotice("");
        self.nameNotice("");
        self.applicantDetails(false);
        self.documentDetails(false);
        self.adtnlDocDets(false);
        self.sponsorDetails(false);
        self.projectDetails(false);
        self.SponsorEnabled(false);
        self.sponsorSelected(false);
        self.checkBox1 = ko.observable(false);
        self.checkBox2 = ko.observable(false);
        self.checkBox3 = ko.observable(false);
        self.checkBox4 = ko.observable(false);
        self.checkBox5 = ko.observable(false);
        self.checkBox6 = ko.observable(false);
        self.checkBox7 = ko.observable(false);
        $("#sponsorDetailsInfoBox").html("");
        $("#documentDetailsForm").unbind("submit");
        $("#registerErrorText").hide();
    };
}
