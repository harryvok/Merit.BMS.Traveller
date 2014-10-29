function SponsorDetailsViewModel() {
    var self = this;
    
    this.company = ko.observable($('#companyInput').val());
    this.titleID = ko.observable($('#titleSelect').val());
    this.surname = ko.observable($('#surnameInput').val());
    this.givenName = ko.observable($('#givenNameInput').val());
    this.position = ko.observable($('#positionInput').val());
    this.passportNum = ko.observable($('#passportNumInput').val());
    this.email = ko.observable($('#emailAddressInput').val());
    this.lAdd1 = ko.observable($('#ladd1Input').val());
    this.lAdd2 = ko.observable($('#ladd2Input').val());
    this.lAdd3 = ko.observable($('#ladd3Input').val());
    this.pNo = ko.observable($('#pNoInput').val());
    this.fax = ko.observable($('#faxInput').val());
    this.pAdd1 = ko.observable($('#pAdd1Input').val());
    this.pAdd2 = ko.observable($('#pAdd2Input').val());
    this.pAdd3 = ko.observable($('#pAdd3Input').val());
    this.status = ko.observable($('#statusSelect').val());
    this.applicantID = ko.observable($('#hiddenApplicantID').val());
    this.officerID = ko.observable($('#hiddenOfficerID').val());
    this.projEnabled = ko.observable($('#projEnabledText').val());
    this.activeInd = ko.observable($('#activeText').val());
    this.verifiedInd = ko.observable($('#verifiedText').val());
    this.permitID = ko.observable($('#hiddenPermitID').val());
    this.sponsorID = ko.observable($('#hiddenSponsorID').val());
    this.searchQuery = ko.observable($('#searchInput').val());


    

    //get webAlias to append onto ajax url
    var alias = $('#webAppAlias').val();

    this.filter = function () {
        alert("ha");
    };

    $("#sponsorDialog").dialog({
        resizable: false,
        dialogClass: 'teststyle',
        minHeight: 385,
        maxHeight: 1650,
        width: 1366,
        show: { effect: 'fade', duration: 500 },
        hide: { effect: 'fade', duration: 200 },
        autoOpen: false,
        title: "Select Sponsor",
        close: function (event, ui) { }
    });

    $('#sponsorDialogTable').jtable({
        sorting: true, //Enable sorting
        selecting: true,
        width: 500,
        actions: {
            listAction: 'getSponsorsList'
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
            title_guid: {
                title: 'title_guid',
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
            } else if (selectedRows.data('record').guid == "-") {
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
                $("#hiddenTitleGuid").html(record.title_guid);
            }

        }
    });


    if (this.company().length > 0) {
        $("#sponsorDialogTable").jtable("load", { userInput: this.company() });
    }

    this.postSponsor = function () {

        var selectedRows = $('#sponsorDialogTable').jtable('selectedRows');
        //alert(selectedRows.data('record').companyName);
        if (selectedRows.data('record') == null) {
            alert("You need to select a sponsor");
        } else if (selectedRows.data('record').guid == "-") {
            alert("You need to select a sponsor");
        } else {
            self.company($('#sponsorCompany').html());
            self.surname($('#sponsorSurname').html());
            self.givenName($('#sponsorGiveName').html());
            self.position($('#sponsorPrincipalPosition').html());
            self.passportNum($('#sponsorPassportNumber').html());
            self.email($('#sponsorEmail').html());
            self.titleID($('#hiddenTitleGuid').html());

            self.lAdd1($("#sponsorAddr1").html());
            self.lAdd2($("#sponsorAddr2").html());
            self.lAdd3($("#sponsorAddr3").html());

            self.pAdd1($("#sponsorPAddr1").html());
            self.pAdd2($("#sponsorPAddr2").html());
            self.pAdd3($("#sponsorPAddr3").html());
            //self.closeSponsorWindow();
            self.processSponsorDetails();
        }

        
    }
    this.closeSponsorWindow = function () {
        $("#sponsorDialog").dialog("close");
        $("#sponsorDialogTable").jtable("destroy");
        self.reinitialiseSponsorDialog();

    }

    this.searchForSponsor = function () {
        if (self.searchQuery().length > 0) {

            $("#sponsorDialogTable").jtable("load", { userInput: self.searchQuery() });
        }
        else {
            alert("Please type a sponsor name");
        }
    };
    this.getSponsorDialog = function () {

        $("#sponsorDialog").dialog("open");
        $("#sponsorDialog").css("background-color", "RGB(203,220,249)");
        /*if (self.company().length > 0) {
            //self.sponsorName() 

            self.searchForSponsor();
            $("#sponsorDialog").dialog("open");
            $("#sponsorDialog").css("background-color", "RGB(203,220,249)");
        }
        else {
            alert("Please type a sponsor name");
        }*/
    };

    this.DialogProcess = function () {
        var selectedRows = $('#sponsorDialogTable').jtable('selectedRows');
        
        //extract id 
        var record = selectedRows.data('record');
        $.ajax({
            type: "GET",
            dataType: "json",
            url: "getSponsorDetails",
            async: false,
            data: {
                sponsorID: record.guID,
            },
            success: function (result) {
           
                self.company(result.records.company);
                self.titleID(result.records.title);
                self.surname(result.records.surname);
                self.givenName(result.records.name);
                self.position(result.records.position);
                self.passportNum(result.records.passportno);
                self.email(result.records.emailAddress);
                self.lAdd1(result.records.add1);
                self.lAdd2(result.records.add2);
                self.lAdd3(result.records.add3);
                self.pNo(result.records.phone);
                self.fax(result.records.fax);
                self.pAdd1(result.records.padd3);
                self.pAdd2(result.records.padd3);
                self.pAdd3(result.records.padd3);
                self.status(result.records.status);
                self.projEnabled(result.records.projenabled);
                self.activeInd(result.records.active);
                self.verifiedInd(result.records.verified);
                self.sponsorID(result.records.sponsorID);

                $("#sponsorDialog").dialog("close");
            }
        });
    };
    this.processSponsorDetails = function () {
        $('#sponsorDetailsForm').validate({
            errorContainer: "#errorContainer",
            errorLabelContainer: "#errorContainer",
            errorElement: "li",

            rules: {
                /*titleSelect: 'required',
                surnameInput: 'required',
                givenNameInput: 'required',
                positionInput: 'required',
                passportNumInput: 'required',
                emailAddressInput: 'required'*/
            }

        }).form();

        if ($("#sponsorDetailsForm").validate().numberOfInvalids() == 0) {
            self.updateSponsorDetails();

        }

    };

    this.updateSponsorDetails = function () {

        $.ajax({
            type: "GET",
            url: "updateSponsorDetails",
            async: false,
            data: {

                //sponsorGuid: self.sponsorID(),
                sponsorGuid:  $("#hiddenSponsorGUID").val(),
                    companyName: self.company(),
                    lAdd1: self.lAdd1(),
                    lAdd2: self.lAdd2(),
                    lAdd3: self.lAdd3(),
                    phoneNo: self.pNo(),
                    faxNo: self.fax(),
                    email: self.email(),
                    pAdd1: self.pAdd1(),
                    pAdd2: self.pAdd2(),
                    pAdd3: self.pAdd3(),
                    position: self.position(),
                    titleID: self.titleID(),
                    surname: self.surname(),
                    givenName: self.givenName(),
                    passportNo: self.passportNum(),
                    statusID: self.status(),
                    activeInd: self.activeInd(),
                    verifiedInd: self.verifiedInd(),
                    projEnabled: self.projEnabled(),
                    permitID: self.permitID(),
                    applicantID: self.applicantID(),
                    officerID: self.officerID(),
                    completeInd: "Y"
        
            },
            success: function (result) {
                if (result == "0") {
                    $('.formclosebutton').trigger("click");
                } else {
                    alert("there was an error");
                }

            }
        });
    };
    this.reinitialiseSponsorDialog = function () {
        $('#sponsorDialogTable').jtable({
            sorting: true, //Enable sorting
            selecting: true,
            width: 500,
            actions: {
                listAction: 'getSponsorsList'
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
                if (selectedRows.data('record').guID == null) {
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


}