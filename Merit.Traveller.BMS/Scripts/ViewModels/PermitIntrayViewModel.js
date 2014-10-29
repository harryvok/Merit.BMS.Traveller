function PermitIntrayViewModel() {
    var self = this;
    //variables
    this.filterNumber = ko.observable($("#filter").val());
    this.filterInput = ko.observable($("#hiddenfilterString").val());
    this.dateLodgedFrom = ko.observable("");
    this.dateLodgedTo = ko.observable("");
    this.dateApprovedFrom = ko.observable("");
    this.dateApprovedTo = ko.observable("");
    this.varn = ko.observable("");
    this.permitNo = ko.observable("");
    this.permitType = ko.observable("");
    this.permitClass = ko.observable("");
    this.mission = ko.observable("");
    this.outcome  = ko.observable("");
    this.finalised = ko.observable("");
    this.hiddenPermit = ko.observable($("#urlVariable").val());
    this.officeGUID = ko.observable("");
    this.reassignTo = ko.observable("");
    this.reassignComments = ko.observable("");
    this.intrayID = ko.observable("");
    this.currentAction = ko.observable("");
    //sections
    this.advFilterSection = ko.observable(false);

    //Search variables
    this.searchDate_lodged_to = ko.observable("");
    this.searchDate_approved_from = ko.observable("");
    this.searchVarn_ref = ko.observable("");
    this.searchPermit_no = ko.observable("");
    this.searchPermit_class = ko.observable("");
    this.searchPermit_type = ko.observable("");
    this.searchSite = ko.observable("");
    this.searchOutcome = ko.observable("");
    this.searchFinalized_ind = ko.observable("");
    this.searchSurname = ko.observable("");
    this.searchGiven_name = ko.observable("");
    this.searchDoc_number = ko.observable("");


    //get webAlias to append onto ajax url
    var alias = $('#webAppAlias').val();
    
    function rowClicked() {

        if ($(this).hasClass("selectedRow")) {
            $(this).removeClass("selectedRow");
        } else {
            $("#enquiryList tr").removeClass("selectedRow");
            $(this).addClass("selectedRow");
            var permitID = $(this).attr("data-permitID");

            $.ajax({
                type: "GET",
                url: "PermitIntray/getPermitProcessSummary",
                data: {
                    permitID: permitID
                },
                success: function (result) {
                    $("#permitProcessSummary").html(result);
                    $("#permitProcessSummary").fadeIn("Fast");
                }
            });
        }
    }
    function initializeTable() {
        var oTable = $('#enquiryTable').dataTable({
            iDisplayLength: 50,
            "aaSorting": [[9, "desc"]],
            "bLengthChange": false,

            "oLanguage": {
                "sSearch": "<b>Enquiry Filter: </b>"
            }
        });
    }

    $("#filterOption").change(function () {
        var filterOption = $("#filterOption").val();
        if (filterOption != "") {
            $.ajax({
                type: "GET",
                url: "PermitIntray/getEnquiryList",
                data: {
                    filterOption: filterOption
                },
                success: function (result) {
                    $("#enquiryList").html(result);
                    $(".enquiryRow").click(rowClicked);
                    initializeTable();

                }
            });
        }
    });
    $("#filterOption").val("1").trigger('change');
    



    $("#filterInput").click(function () {
        self.filterInput("");
    });

    /*$("#changeSiteDialog").dialog({
        resizable: false,
        dialogClass: 'teststyle',
        minHeight: 137,
        width: 350,
        show: { effect: 'fade', duration: 500 },
        hide: { effect: 'fade', duration: 200 },
        autoOpen: false,
        title: "Search",
        close: function (event, ui) { }
    });

    $("#dateRangeDialog").dialog({
        resizable: false,
        dialogClass: 'teststyle',
        minHeight: 137,
        width: 350,
        show: { effect: 'fade', duration: 500 },
        hide: { effect: 'fade', duration: 200 },
        autoOpen: false,
        title: "Search",
        close: function (event, ui) { }
    });

    $("#reassignDialog").dialog({
        resizable: false,
        dialogClass: 'teststyle',
        minHeight: 274,
        width: 650,
        show: { effect: 'fade', duration: 500 },
        hide: { effect: 'fade', duration: 200 },
        autoOpen: false,
        title: "Reassign",
        close: function (event, ui) { }
    });*/

    //initialise table of processes for pop up window
    //$('#processContainerTable').jtable({
    //    selecting: true,
    //    columnSelectable: false,
    //    actions: {
    //        listAction: 'PermitIntray/getPermitProcesses'
    //    },
    //    fields: {
    //        processGUID: {
    //            title: 'processGUID',
    //            create: false,
    //            edit: false,
    //            visibility: "hidden",
    //            width: "0%"
    //        },
    //        processNumber: {
    //            title: 'processNo',
    //            create: false,
    //            edit: false,
    //            width: "0%",
    //            visibility: "hidden",
    //        },
    //        IsModifiable: {
    //            title: 'IsModifiable',
    //            create: false,
    //            edit: false,
    //            width: "0%",
    //            visibility: "hidden",
    //        },
    //        MustReassign: {
    //            title: 'MustReassign',
    //            create: false,
    //            edit: false,
    //            width: "0%",
    //            visibility: "hidden",
                
    //        },
    //        canModify: {
    //            title: 'canModify',
    //            create: false,
    //            edit: false,
    //            width: "0%",
    //            visibility: "hidden"
    //        },
    //        Status: {
    //            title: 'Status',
    //            create: false,
    //            edit: false,
    //            width: "10%",
    //            display: function (data) {

    //                if (data.record.Status == "Y")
    //                    return '<img src=' + "Content/Images/tick.png" + ' />';
    //                else
    //                    return '<img src=' + "Content/Images/process-cross.png" + ' />';
    //            }
    //        },
    //        Process: {
    //            title: 'Process',
    //            create: false,
    //            edit: false,
    //            width: "90%"
    //        }
    //    },
    //    recordsLoaded: function (event, data) {

    //        //centre dialog box

    //        $("#permitprocesssummary").dialog("option", "position", ['center', 'center']);
    //        $("#permitprocesssummary .ui-dialog-titlebar-close").css("background-image", "url(../../Scripts/jtable/themes/basic/close.png)");

    //        $("#hiddenDependentID").val(data.serverResponse.dependentGUID);
    //        $("#hiddenSponsorID").val(data.serverResponse.sponsorGUID);
    //        $("#hiddenSelectedVarn").val(data.serverResponse.varnRef);
    //        $("#permitDetailsLine1").html(data.serverResponse.varnRef +" "+ data.serverResponse.permitType+ "/" + data.serverResponse.permitClass );
    //        $("#permitDetailsLine2").html(data.serverResponse.applicant + " " + data.serverResponse.dob + " " + data.serverResponse.nationality);
    //        $("#permitDetailsLine3").html(data.serverResponse.datelodged + " <b> at </b> " + data.serverResponse.lodgedAt);

    //        //if permit processes havnt been completed yet
    //        if (data.serverResponse.decideDate == "") {
    //            $("#permitDetailsLine4").html(data.serverResponse.permitNo + "<b> Decided </b> - ");
    //        } else {
    //            $("#permitDetailsLine4").html(data.serverResponse.permitNo + "<b> Decided </b>" + data.serverResponse.decideDate);
    //        }

    //        //if we are on a print step, have the register button
    //        /*if ((data.serverResponse.lastProc == "456" || data.serverResponse.lastProc == "453") && data.serverResponse.lastProcCompleted == "N") {
    //            $("#registerButton").show();
    //        } else {
    //            $("#registerButton").hide();
    //        }*/
            
    //        //changed background colour based on outcome
    //        if (data.serverResponse.outcome == "Declined") {
    //            $("#processDialogPermitDets").css("background-color", "red");
    //            $("#processDialogPermitDets").css("border-top", "5px solid darkred");

    //        }
    //        else if (data.serverResponse.outcome == "Undecided") {
    //            $("#processDialogPermitDets").css("background-color", "orange");
    //            $("#processDialogPermitDets").css("border-top", "5px solid darkorange");

    //        } else {
    //            $("#processDialogPermitDets").css("background-color", "#00FF00");
    //            $("#processDialogPermitDets").css("border-top", "5px solid #00d400");
    //        }

            
    //        $('#processContainerTable').jtable('selectRows', "");
    //    },
    //    selectionChanged: function (data) {


    //        /*
    //        * If a process has been clicked, link the user to 
    //        * the specific page that it points to.
    //        */
    //        var selectedRows = $('#processContainerTable').jtable('selectedRows');
           
    //        //extract id 
    //        var process = selectedRows.data('record');

    //        //if a row was unselected, it still counts as a selectionChanged event
    //        //therefore return if no row has been selected.
    //        if (process == null)
    //            return;
            
    //        //if you dont have to reassign
    //        if (process.MustReassign == "Y" && process.Status != "Y") {
    //            $("#reassignDialog").dialog("open");
    //            self.intrayID(process.processGUID);

    //            $("#currentFilter").html($("#filters").find(":selected").html());
    //            $("#currentAction").html(process.Process);
    //            //$("#permitprocesssummary").dialog("close");
    //            $("#reassignDialog").css("background-color", "rgb(182, 197, 224)");
    //        }else{

    //            //applicant details screen
    //            if (process.processNumber == "ApplicantDetails" && process.Process != "Sponsor - No sponsor required") {
    //                location.href = "Permit/ApplicantDetails?permitID=" + $("#hiddenPermitID").val() + "&dependentID=" + $("#hiddenDependentID").val() + "&modify=" + process.canModify;
    //            }
    //                //sponsor details
    //            else if (process.processNumber == "p999") {
    //                location.href = "Permit/p999?permitID=" + $("#hiddenPermitID").val() + "&sponsorID=" + $("#hiddenSponsorID").val() + "&modify=" + process.canModify;
    //            }
    //                //no sponsor required
    //            else if (process.processNumber == "p998") {
    //                /*do nothing*/
    //            }
    //                //check requirements
    //            else if (process.processNumber == "p461") {
    //                location.href = "Permit/p461?permitID=" + $("#hiddenPermitID").val() + "&dependentID=" + $("#hiddenDependentID").val() + "&intrayID=" + process.processGUID + "&modify=" + process.canModify;
    //            }
    //                //make recommendations
    //            else if (process.processNumber == "p462") {
    //                location.href = "Permit/p462?permitID=" + $("#hiddenPermitID").val() + "&dependentID=" + $("#hiddenDependentID").val() + "&intrayID=" + process.processGUID + "&modify=" + process.canModify;
    //            }
    //                //make decision
    //            else if (process.processNumber == "p455") {
    //                location.href = "Permit/p455?permitID=" + $("#hiddenPermitID").val() + "&dependentID=" + $("#hiddenDependentID").val() + "&intrayID=" + process.processGUID + "&modify=" + process.canModify;
    //            }
    //                //print permit
    //            else if (process.processNumber == "p456") {
    //                location.href = "Permit/p456?permitID=" + $("#hiddenPermitID").val() + "&dependentID=" + $("#hiddenDependentID").val() + "&intrayID=" + process.processGUID + "&modify=" + process.canModify;
    //            }
    //                //print letter of authorisation/rejection
    //            else if (process.processNumber == "p453") {
    //                location.href = "Permit/p453?permitID=" + $("#hiddenPermitID").val() + "&intrayID=" + process.processGUID + "&title=" + process.Process + "&modify=" + process.canModify
    //            }
    //            else if (process.processNumber == "pDocument") {
    //                location.href = "Permit/pDocument?permitID=" + $("#hiddenPermitID").val() + "&dependentID=" + $("#hiddenDependentID").val() + "&intrayID=" + process.processGUID + "&modify=" + process.canModify;
    //            }
    //            else if (process.processNumber == "NoSponsor") {

    //                //unselect
    //                $('#processContainerTable').jtable('selectRows', "");
    //            }
    //            else {
    //                alert("Error - Invalid Process " + process.processNumber);
    //                //unselect
    //                $('#processContainerTable').jtable('selectRows', "");
    //            }
    //        }
            
            
    //    }
    //});


    //initialise the table
    //$('#permitList').jtable({
    //    paging: true, //Enable paging
    //    pageSize: 25, //Set page size (default: 25)
    //    sorting: true, //Enable sorting
    //    selecting: true,
    //    columnSelectable: false,
    //    paging:true,
    //    defaultSorting: 'DateLodged DESC',
    //    actions: {
    //        listAction: 'PermitIntray/getList'
    //    },
    //    fields: {
    //        permID: {
    //            title: 'PermitID',
    //            create: false,
    //            edit: false,
    //            visibility: 'hidden',
    //            key:true
                
    //        },
    //        varnRef: {
    //            title: '<b>Varn Ref</b>',
    //            create: false,
    //            edit: false,
    //            width: '6%'
    //        },
    //        PermitNo: {
    //            title: '<b>Permit No.</b>',
    //            create: false,
    //            edit: false,
    //            width: '5%'
    //        },
    //        surname: {
    //            title: '<b>Surname</b>',
    //            create: false,
    //            edit: false,
    //            width: '10%'
    //        },
    //        givenName: {
    //            title: '<b>Given Name</b>',
    //            create: false,
    //            edit: false,
    //            width: '10%'
    //        },
    //        PermitClass: {
    //            title: '<b>Permit Class</b>',
    //            create: false,
    //            edit: false,
    //            width: '20%'
    //        },
    //        NextAction: {
    //            title: '<b>Next Action</b>',
    //            create: false,
    //            edit: false,
    //            width: '13%'
    //        },
    //        Status: {
    //            title: '<b>Status<b/>',
    //            create: false,
    //            edit: false,
    //            width: '10%'
    //        },
    //        Finalised: {
    //            title: '<b>Final?</b>',
    //            create: false,
    //            edit: false,
    //            width: '2%'
    //        },
    //        DocumentNumber: {
    //            title: '<b>Doc. No.</b>',
    //            create: false,
    //            edit: false,
    //            width: '7%'

    //        },
    //        DateLodged: {
    //            title: '<b>Registered</b>',
    //            create: false,
    //            edit: false,
    //            width: '6%'
    //        },
    //        Site: {
    //            title: '<b>Site</b>',
    //            create: false,
    //            edit: false,
    //            width: '15%'
    //        }
            

    //    },
    //    //called when all data has been loaded
    //    recordsLoaded: function (data) {
    //        if (self.hiddenPermit().length > 0) {
    //            var selectedRows = $('#permitList').jtable("getRowByKey", self.hiddenPermit());

    //            //if it exists in the current lists, select it.
    //            if (selectedRows != null) {
    //                $('#permitList').jtable('selectRows', selectedRows);
    //            } else {
    //                //fload jtable and display dialog
    //                $("#hiddenPermitID").val(self.hiddenPermit());
    //                $('#processContainerTable').jtable("load", { permitID: self.hiddenPermit() });
    //                $("#permitprocesssummary").dialog("open");
    //            }
    //        }
            
    //    },
    //    //when user has clicked on a row
    //    selectionChanged: function (data) {
            
    //        //get selected row
    //        var selectedRows = $('#permitList').jtable('selectedRows');
           
    //        //extract id 
    //        var record = selectedRows.data('record');
            
    //        //if the user has not unselected a row
    //        if (record != null) {
    //            $("#hiddenPermitID").val(record.permID);

    //            //fload jtable and display dialog
    //            $('#processContainerTable').jtable("load", { permitID: record.permID });
    //            $("#permitprocesssummary").dialog("open");

    //            $('#viewButton').prop('disabled', false);
    //            $('#completeButton').prop('disabled', false);

    //        } else {
    //            $('#viewButton').prop('disabled', true);
    //            $('#completeButton').prop('disabled', true);
    //        }
    //    }
    //});
    
    //disallow row editing
    //$('.jtable-page-size-change').hide();
    $('.jtable-page-size-change span').html("Show:");

    
    $('.jtable-page-number').css("display", "none");
    $('.jtable-page-number-previous jtable-page-number-disabled').css("background-color", "none");

    //initialize the dialog
    /*$("#permitprocesssummary").dialog({
        resizable: false,
        dialogClass: 'teststyle',
        minHeight: 100,
        width: 450,
        show: { effect: 'fade', duration: 500 },
        hide: { effect: 'fade', duration: 200 },
        autoOpen: false,
        title: "PROCESS PERMIT",
        close: function (event, ui) {}
    });

    $("#filterDialog").dialog({
        resizable: false,
        dialogClass: 'teststyle',
        minHeight: 337,
        width: 520,
        show: { effect: 'fade', duration: 500 },
        hide: { effect: 'fade', duration: 200 },
        autoOpen: false,
        title: "Search",
        close: function (event, ui) { }
    });*/
    
    //$("#filters").change(function () {
    //    if ($("#filters").val() == "4") {
    //        $('#dateRangeDialog').dialog("open");
    //        $('#dateRangeDialog').css("background-color", "rgb(182, 197, 224)");
    //    } else {
    //        $('#permitList').jtable('load', { filterNum: $("#filters").val(), filterString: self.filterInput(), fromDate: $("#dateRangeFrom").val(), toDate: $("#dateRangeTo").val() });
    //    }
    //});

    //this.filter = ko.computed(function () {
        
    //    if (self.filterNumber().length > 0) {
    //        $('#permitList').jtable('load', { filterNum: self.filterNumber(), filterString: self.filterInput(), fromDate: $("#dateRangeFrom").val(), toDate: $("#dateRangeTo").val() });
    //    }
    //    return this.filterNumber();
    //}, this);

    //this.filterDateRange = function () {
    //    $('#permitList').jtable('load', { filterNum: self.filterNumber(), filterString: self.filterInput(), fromDate: $("#dateRangeFrom").val(), toDate: $("#dateRangeTo").val() });
    //    $("#dateRangeDialog").dialog("close");
    //}

    /*this.filterPermitIntray = ko.computed(function () {
            $('#permitList').jtable('load', { filterNum: self.filterNumber(), filterString: self.filterInput() });
    }, this);

    //normal functions
    this.filterPermitTable = function (sel) {

            $.ajax({
                type: "GET",
                url: "PermitIntray/getList",
                async: false,
                data: {
                    filterNum: sel
                },
                success: function (result) {

                    //$("#permitIntray").html(result);
                }
            });
    };*/

    //this.clearAdvancedFilter = function () {
    //    self.dateLodgedFrom("");
    //    self.dateLodgedTo("");
    //    self.dateApprovedFrom("");
    //    self.dateApprovedTo("");
    //    self.varn("");
    //    self.permitNo("");
    //    self.permitType("");
    //    self.permitClass("");
    //    self.mission("");
    //    self.outcome("");
    //    self.finalised("");
    //};

    //this.displayFilterMenu = function () {
    //    $("#filterDialog").dialog("open");
    //    $("#filterDialog").css("background-color", "rgb(182, 197, 224)");
    //};

    //this.toggleNewVisaForm = function () {
        
    //    $("#permitprocesssummary").dialog("close");
    //    if ($("#NewVisaFormWindow").is(":visible")) {
    //        $("#NewVisaFormWindow").fadeOut(200);
    //    } else {
    //        $("#NewVisaFormWindow").fadeIn(200);
    //    }
    //};
    //this.toggleMessageListDialogue = function () {
    //    if ($("#messageListDialogue").is(":visible")) {
    //        $("#messageListDialogue").fadeOut(200);
    //    } else {
    //        $("#messageListDialogue").fadeIn(200);
    //    }
    //};
    //this.closeMessages = function () {
    //    $("#messageListDialogue").fadeOut(200);
    //};

    //this.closePopup = function () {
    //    self.advFilterSection(false);
    //};

    //this.openPermit = function () {
    //    alert("this was selected");

    //};
    //this.clearFilter = function () {
    //    self.filterInput("");
    //    $('#permitList').jtable('load', { filterNum: self.filterNumber(), filterString: self.filterInput(), fromDate: $("#dateRangeFrom").val(), toDate: $("#dateRangeTo").val() });
    //}

    //this.viewPermitProcesses = function () {

    //    var selectedRows = $('#permitList').jtable('selectedRows');
    //    var record = selectedRows.data('record');

    //    //do operation only if row has been selected
    //    if (record != null) {
    //        $('#processContainerTable').jtable("load", { permitID: record.permID });
    //        $("#permitprocesssummary").dialog("open");
            
    //    }
    //};

    //this.openUncompletedProcess = function () {
    //    var selectedRows = $('#permitList').jtable('selectedRows');
    //    var record = selectedRows.data('record');

    //    //do operation only if row has been selected
    //    if (record != null) {

        
    //        $.ajax({
    //            type: "GET",
    //            url: "Permit/getNextUncompletedProcess",
    //            async: false,
    //            data: {
    //                permitID: record.permID
    //            },
    //            success: function (result) {

    //                alert(result);
    //            }
    //        });
    //    }

    //};

    //this.advancedFilter = function () {

    //    //check for valid dates
    //    //if valid, update with preferred value
    //    self.dateLodgedFrom(self.checkDate(self.dateLodgedFrom(), "P"));
    //    self.dateLodgedTo(self.checkDate(self.dateLodgedTo(), "F"));
    //    self.dateApprovedFrom(self.checkDate(self.dateApprovedFrom(), "P"));
    //    self.dateApprovedTo(self.checkDate(self.dateApprovedTo(), "F"));

    //    $.ajax({
    //        type: "GET",
    //        url: "PermitIntray/advancedPermitIntray",
    //        async: false,
    //        data: {
    //            dateLodgedFrom: self.dateLodgedFrom(),
    //            dateLodgedTo: self.dateLodgedTo(),
    //            dateApprovedFrom: self.dateApprovedFrom(),
    //            dateApprovedTo: self.dateApprovedTo(),
    //            varn: self.varn(),
    //            permitNo: self.permitNo(),
    //            permitType: self.permitType(),
    //            permitClass: self.permitClass(),
    //            mission: self.mission(),
    //            outcome: self.outcome(),
    //            finalised: self.finalised()

    //        },
    //        success: function (result) {
    //            //$("#permitIntray").html(result);
    //        }
    //    });

    //};

    //this.closeReassignDialog = function () {
     
    //    //$("#reassignDialog").dialog("close");
    //    //$('#processContainerTable').jtable('selectRows', "");
    //    /*
    //        * If a process has been clicked, link the user to 
    //        * the specific page that it points to.
    //        */
    //    var selectedRows = $('#processContainerTable').jtable('selectedRows');

    //    //extract id 
    //    var process = selectedRows.data('record');

    //    //if a row was unselected, it still counts as a selectionChanged event
    //    //therefore return if no row has been selected.
    //    if (process == null)
    //        return;

    //        //applicant details screen
    //        if (process.processNumber == "ApplicantDetails" && process.Process != "Sponsor - No sponsor required") {
    //            location.href = "Permit/ApplicantDetails?permitID=" + $("#hiddenPermitID").val() + "&dependentID=" + $("#hiddenDependentID").val();
    //        }
    //            //sponsor details
    //        else if (process.processNumber == "p999") {
    //            location.href = "Permit/p999?permitID=" + $("#hiddenPermitID").val() + "&sponsorID=" + $("#hiddenSponsorID").val() + "&modify=N";
    //        }
    //            //no sponsor required
    //        else if (process.processNumber == "p998") {
    //            /*do nothing*/
    //        }
    //            //check requirements
    //        else if (process.processNumber == "p461") {
    //            location.href = "Permit/p461?permitID=" + $("#hiddenPermitID").val() + "&dependentID=" + $("#hiddenDependentID").val() + "&intrayID=" + process.processGUID + "&modify=N";
    //        }
    //            //make recommendations
    //        else if (process.processNumber == "p462") {
    //            location.href = "Permit/p462?permitID=" + $("#hiddenPermitID").val() + "&dependentID=" + $("#hiddenDependentID").val() + "&intrayID=" + process.processGUID + "&modify=N";
    //        }
    //            //make decision
    //        else if (process.processNumber == "p455") {
    //            location.href = "Permit/p455?permitID=" + $("#hiddenPermitID").val() + "&dependentID=" + $("#hiddenDependentID").val() + "&intrayID=" + process.processGUID + "&modify=N";
    //        }
    //            //print permit
    //        else if (process.processNumber == "p456") {
    //            location.href = "Permit/p456?permitID=" + $("#hiddenPermitID").val() + "&dependentID=" + $("#hiddenDependentID").val() + "&intrayID=" + process.processGUID + "&modify=N";
    //        }
    //            //print letter of authorisation/rejection
    //        else if (process.processNumber == "p453") {
    //            location.href = "Permit/p453?permitID=" + $("#hiddenPermitID").val() + "&intrayID=" + process.processGUID + "&title=" + process.Process + "&modify=N";
    //        }
    //        else if (process.processNumber == "pDocument") {
    //            location.href = "Permit/pDocument?permitID=" + $("#hiddenPermitID").val() + "&dependentID=" + $("#hiddenDependentID").val() + "&intrayID=" + process.processGUID + "&modify=N";
    //        }
    //        else if (process.processNumber == "NoSponsor") {


    //        }
    //        else {
    //            alert("Error - Invalid Process " + process.processNumber);
    //        }
    //        $('#processContainerTable').jtable('selectRows', "");
        
    //};
    //this.reassignAction = function () {
    //    $.ajax({
    //        type: "GET",
    //        url: "PermitIntray/reassignAction",
    //        async: false,
    //        data: {
    //            comments: self.reassignComments(),
    //            intray_guid:  self.intrayID(),
    //            permit_guid: $("#hiddenPermitID").val()
    //        },
    //        success: function (result) {
    //            if (result != "success")
    //                alert(result);
    //            else {
    //                /*$("#reassignDialog").dialog("close");
    //                $('#permitList').jtable('load', { filterNum: self.filterNumber(), filterString: self.filterInput(), fromDate: $("#dateRangeFrom").val(), toDate: $("#dateRangeTo").val() });
    //                $('#processContainerTable').jtable('selectRows', "");
    //                $("#permitprocesssummary").dialog("close");*/
    //                var selectedRows = $('#processContainerTable').jtable('selectedRows');


    //                //extract id 
    //                var process = selectedRows.data('record');

    //                //if a row was unselected, it still counts as a selectionChanged event
    //                //therefore return if no row has been selected.
    //                if (process == null)
    //                    return;

    //                //applicant details screen
    //                if (process.processNumber == "ApplicantDetails" && process.Process != "Sponsor - No sponsor required") {
    //                    location.href = "Permit/ApplicantDetails?permitID=" + $("#hiddenPermitID").val() + "&dependentID=" + $("#hiddenDependentID").val() + "&modify=Y";
    //                }
    //                    //sponsor details
    //                else if (process.processNumber == "p999") {
    //                    location.href = "Permit/p999?permitID=" + $("#hiddenPermitID").val() + "&sponsorID=" + $("#hiddenSponsorID").val() + "&modify=Y";
    //                }
    //                    //no sponsor required
    //                else if (process.processNumber == "p998") {
    //                    /*do nothing*/
    //                }
    //                    //check requirements
    //                else if (process.processNumber == "p461") {
    //                    location.href = "Permit/p461?permitID=" + $("#hiddenPermitID").val() + "&dependentID=" + $("#hiddenDependentID").val() + "&intrayID=" + process.processGUID + "&modify=Y";
    //                }
    //                    //make recommendations
    //                else if (process.processNumber == "p462") {
    //                    location.href = "Permit/p462?permitID=" + $("#hiddenPermitID").val() + "&dependentID=" + $("#hiddenDependentID").val() + "&intrayID=" + process.processGUID + "&modify=Y";
    //                }
    //                    //make decision
    //                else if (process.processNumber == "p455") {
    //                    location.href = "Permit/p455?permitID=" + $("#hiddenPermitID").val() + "&dependentID=" + $("#hiddenDependentID").val() + "&intrayID=" + process.processGUID + "&modify=Y";
    //                }
    //                    //print permit
    //                else if (process.processNumber == "p456") {
    //                    location.href = "Permit/p456?permitID=" + $("#hiddenPermitID").val() + "&dependentID=" + $("#hiddenDependentID").val() + "&intrayID=" + process.processGUID + "&modify=Y";
    //                }
    //                    //print letter of authorisation/rejection
    //                else if (process.processNumber == "p453") {
    //                    location.href = "Permit/p453?permitID=" + $("#hiddenPermitID").val() + "&intrayID=" + process.processGUID + "&title=" + process.Process + "&modify=Y";
    //                }
    //                else if (process.processNumber == "pDocument") {
    //                    location.href = "Permit/pDocument?permitID=" + $("#hiddenPermitID").val() + "&dependentID=" + $("#hiddenDependentID").val() + "&intrayID=" + process.processGUID + "&modify=Y";
    //                }
    //                else if (process.processNumber == "NoSponsor") {


    //                }
    //                else {
    //                    alert("Error - Invalid Process " + process.processNumber);
    //                }
    //                $('#processContainerTable').jtable('selectRows', "");


    //            }
    //        }
    //    });
    //};

    //this.checkDate = function (date, type) {
    //    var outcome;
    //    $.ajax({
    //        type: "GET",
    //        url: "VisaRegistration/checkDate",
    //        async: false,
    //        data: {
    //            date: date,
    //            type: type
    //        },
    //        success: function (result) {
    //            outcome = result;
    //        }
    //    });

    //    if (outcome == "")
    //        return "";
    //    else
    //        return outcome;

    //};
    
    

    //if ($("#isVisaRegWindowOpen").val() == "Y") {
    //    self.toggleNewVisaForm();
    //}

}