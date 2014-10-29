function PrintViewModel() {
    var self = this;
    var alias = $('#webAppAlias').val();

    this.vlidseries = ko.observable("");
    this.vlidID = ko.observable("");
    this.reVlidseries = ko.observable("");
    this.reVlidID = ko.observable("");
    this.howUsed = ko.observable("");
    this.comments = ko.observable("");
    this.entriesAllowed = ko.observable("");
    this.bypassComments= ko.observable("");
   
    $("#BypassDialog").dialog({
        resizable: false,
        dialogClass: 'teststyle',
        height: 200,
        width: 562,
        show: { effect: 'fade', duration: 500 },
        hide: { effect: 'fade', duration: 200 },
        autoOpen: false,
        title: "Bypass Visa Label ID",
        close: function (event, ui) { }
    });

    $.ajax({
            type: "GET",
            url: "createVisaPDF",
            async: false,
            data: {
                permit_guid: $("#hiddenPermitID").val()
            },
            success: function (result) {
                window.open("../pdf/" + $("#hiddenDocumentURL").val().split('|')[0], "visa permit", "width=400,height=400");
            }
        });

    if ($("#hiddenDocumentURL").val().split('|')[0] == "N") {
        $("#okbutton").prop('disabled', true);
        $("#bypass").prop('disabled', true);
        $("#reprint").prop('disabled', true);
    }
  
    this.printPDF = function () {
        $.ajax({
            type: "GET",
            url: "createVisaPDF",
            async: false,
            data: {
                permit_guid: $("#hiddenPermitID").val()
            },
            success: function (result) {
                window.open("../pdf/" + result.split('|')[0], "visa permit", "width=400,height=400");
            }
        });
    };

    this.processPrintStep = function (processType) {

        $.ajax({
            type: "GET",
            url: "processPrint",
            async: false,
            data: {
                permit_guid: $("#hiddenPermitID").val(),
                vlid_series: self.vlidseries(),
                vlid_id: self.vlidID(),
                how_used: self.howUsed(),
                comments: self.comments(),
                process_type: processType,
                intrayGUID: $("#hiddenIntrayID").val(),
                bypass_comment: self.bypassComments()

            },
            success: function (result) {

                if (result != "success") {
                    alert(result);
                } else {
                    $('.formclosebutton').trigger("click");
                }

            }
        });
    };

    this.processPrint = function (processType) {

        if (processType == "R") {
            self.printPDF();
        }
        else if (processType == "B") {
            $("#BypassDialog").dialog("open");
            $("#BypassDialog").css("background-color", "RGB(203,220,249)");
        }
        else {

            self.processPrintStep(processType);
        }
    };

    this.closeBypassDialog = function () {
        $("#BypassDialog").dialog("close");
    };
}