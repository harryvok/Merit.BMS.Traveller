function MessagesViewModel() {
    var self = this;

   

    this.newMessage = function () {
        $("#messageDialog").dialog("open");
        $("#messageDialog").css("background-color", "rgb(182, 197, 224)");
    };

    //get webAlias to append onto ajax url
    var alias = $('#webAppAlias').val();

    //initialise the table
    $('#messagesList').jtable({
        
        sorting: true, //Enable sorting
        selecting: true,
        paging: true,
        actions: {
            listAction: 'Messages/getMessagesList'
        },
        fields: {
            messageID: {
                title: 'ID',
                create: false,
                edit: false,
                visibility: 'hidden'
            },
            read: {
                title: 'read',
                create: false,
                edit: false,
                width: "3%"
            },
            from: {
                title: 'From',
                create: false,
                edit: false
            },
            subject: {
                title: 'Subject',
                create: false,
                edit: false
            },
            received: {
                title: 'Received',
                create: false,
                edit: false
            },
            contents: {
                title: 'contents',
                create: false,
                edit: false,
                visibility: 'hidden'
            }
        },
        
        selectionChanged: function (data) {

            //get selected row
            var selectedRows = $('#messagesList').jtable('selectedRows');
           
            //extract id 
            var record = selectedRows.data('record');
      
            if (record.messageID != "-") {
                $.ajax({
                    type: "POST",
                    url: "Messages/messageDetails",
                    async: false,
                    data: {
                        messageID: record.messageID
                    },
                    success: function (result) {
                        $("#messageView").fadeIn("fast");
                        $("#messageView").html("Subject:<b>" + result.subject +
                                               "</b><br/>From:<b>" + result.from +
                                               "</b><br/>received:<b>" + result.datereceived+
                                               "</b><br/><hr class=\"greyhr\"/><br/>" + result.contents);
                    }
                });
            }

        }
    });

    //load jtable
    $('#messagesList').jtable('load');


    //disallow row editing
    //$('.jtable-page-size-change').hide();

    //initialize the dialogs
    $("#newmessage").dialog({
        resizable: false,
        dialogClass: 'teststyle',
        minHeight: 385,
        width: 430,
        show: { effect: 'fade', duration: 500 },
        hide: { effect: 'fade', duration: 200 },
        autoOpen: false,
        title: "New Message",
        close: function (event, ui) { }
    });

    $("#replymessage").dialog({
        resizable: false,
        dialogClass: 'teststyle',
        minHeight: 385,
        width: 430,
        show: { effect: 'fade', duration: 500 },
        hide: { effect: 'fade', duration: 200 },
        autoOpen: false,
        title: "Reply Message",
        close: function (event, ui) { }
    });

    $("#forwardmessage").dialog({
        resizable: false,
        dialogClass: 'teststyle',
        minHeight: 385,
        width: 430,
        show: { effect: 'fade', duration: 500 },
        hide: { effect: 'fade', duration: 200 },
        autoOpen: false,
        title: "Forward Message",
        close: function (event, ui) { }
    });

    //override default css
    $(".ui-widget-content").css("background", "none");
    

    /*$("#permitprocesssummary .ui-dialog-titlebar").css("display", "none");
    $("#permitprocesssummary .ui-dialog-title").css("font-size", "5px");
    $("#permitprocesssummary .ui-widget-header").css("display", "none");
    $("#permitprocesssummary .ui-widget-content").css("background-color", "#DD3355");
    $("#permitprocesssummary .ui-widget-content").css("font-size", "12px");
    $(".ui-corner-all").css("background-color", "#e4e4e4");

    $("#permitprocesssummary .ui-dialog-titlebar-close").css("background-image", "url(Scripts/jtable/themes/basic/close.png)");*/

    this.sendNewMessage = function (type) {
        if (type == "new") {
            $.ajax({
                type: "GET",
                url: "Messages/sendMessage",
                async: false,
                data: {
                    recipients: $("#newmessage .msgrecipient").val(),
                    subject_txt:$("#newmessage .msgsubject").val(),
                    message_txt:$("#newmessage .MessageTextArea").val()
                },
                success: function (result) {
                    if (result == "success") {
                        alert("message sent");
                        $("#messageDialog").dialog("close");
                    } else {
                        $("#messageDialog").dialog("close");
                        alert(result);
                    }
                }
            });
        } else if (type == "reply") {
            $.ajax({
                type: "GET",
                url: "Messages/sendMessage",
                async: false,
                data: {
                    recipients: $("#replymessage .msgrecipient").val(),
                    subject_txt: $("#replymessage .msgsubject").val(),
                    message_txt: $("#replymessage .MessageTextArea").val()
                },
                success: function (result) {
                    if (result == "success") {
                        alert("message sent");
                        $("#messageDialog").dialog("close");
                    } else {
                        $("#messageDialog").dialog("close");
                        alert(result);
                    }
                }
            });
        }else if (type == "forward"){
            $.ajax({
                type: "GET",
                url: "Messages/sendMessage",
                async: false,
                data: {
                    recipients: $("#forwardmessage .msgrecipient").val(),
                    subject_txt: $("#forwardmessage .msgsubject").val(),
                    message_txt: $("#forwardmessage .MessageTextArea").val()
                },
                success: function (result) {
                    if (result == "success") {
                        alert("message sent");
                        $("#messageDialog").dialog("close");
                    } else {
                        $("#messageDialog").dialog("close");
                        alert(result);
                    }
                }
            });
        }
        
    }

    this.openNewMsgWindow = function () {
        $("#replymessage").dialog("close");
        $("#forwardmessage").dialog("close");

        $("#newmessage").dialog("open");
        $("#newmessage").css("background-color", "rgb(182, 197, 224)");
    }
    this.replyMsgWindow = function () {
     
        if ($('#messagesList').jtable('selectedRows').data('record') != null) {
            $("#newmessage").dialog("close");
            $("#forwardmessage").dialog("close");


            $("#replymessage .msgrecipient").val($('#messagesList').jtable('selectedRows').data('record').from);
            $("#replymessage .msgsubject").val("RE: " + $('#messagesList').jtable('selectedRows').data('record').subject);
            $("#replymessage .MessageTextArea").val("\n----------------------------------\n"+ $('#messagesList').jtable('selectedRows').data('record').contents);
            $("#replymessage").dialog("open");
            $("#replymessage").css("background-color", "rgb(182, 197, 224)");

        }

        
    }
    this.forwardMsgWindow = function () {
        if ($('#messagesList').jtable('selectedRows').data('record') != null) {
            $("#newmessage").dialog("close");
            $("#replymessage").dialog("close");

            $("#forwardmessage .msgsubject").val($('#messagesList').jtable('selectedRows').data('record').subject);
            $("#forwardmessage .MessageTextArea").val($('#messagesList').jtable('selectedRows').data('record').contents);
            $("#forwardmessage").dialog("open");
            $("#forwardmessage").css("background-color", "rgb(182, 197, 224)");
        }
    }

    this.deleteMessage = function () {
        if ($('#messagesList').jtable('selectedRows').data('record') != null) {

            var selectedRows = $('#messagesList').jtable('selectedRows');
            var record = selectedRows.data('record');

            if (confirm('Are you sure you want to delete this message?')) {
                $.ajax({
                    type: "POST",
                    url: "Messages/deleteMessage",
                    async: false,
                    data: {
                        messageID: record.messageID
                    },
                    success: function (result) {
                        
                    }
                });
            } else {
                // Do nothing!
            }
        }
    }

}