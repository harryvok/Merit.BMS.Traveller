function CheckRequirementsViewModel() {
    var self = this;

    this.permitID = ko.observable($('#hiddenPermitGuidField').val());
    this.intrayID = ko.observable($('#hiddenIntrayIDField').val());
    this.comments = ko.observable($('#commentsTextArea').val());
    this.isEditable = ko.observable($('#hiddenEditable').val());

    //get webAlias to append onto ajax url
    var alias = $('#webAppAlias').val();
   
    if (this.isEditable() == "Y") {
        $('#metButton').prop('disabled', true);
        $('#notMetButton').prop('disabled', true);
    }
    else {
        $('#metButton').prop('disabled', false);
        $('#notMetButton').prop('disabled', false);
    }


    //initialise the table
    $('#metList').jtable({
        /*paging: true, //Enable paging
        pageSize: 10,*/ //Set page size (default: 10)
        sorting: true, //Enable sorting
        selecting: true,
        columnSelectable: false,
        actions: {
            listAction: 'getMetRequirements'
        },
        fields: {
            description: {
                title: '<b>Description</b>',
                create: false,
                edit: false,
                key: true,
                width: '32%'

            },
            officerName: {
                title: '<b>Officer Name</b>',
                create: false,
                edit: false,
                width: '27%'
            },
            checkDate: {
                title: '<b>Check Date</b>',
                create: false,
                edit: false,
                width: '7%'
            },
            comments: {
                title: '<b>Comments</b>',
                create: false,
                edit: false,
                width: '27%'
            },
            atPost: {
                title: '<b>At Post?</b>',
                create: false,
                edit: false,
                width: '7%'
            }
        },
        //called when all data has been loaded
        recordsLoaded: function (data) {

        },
        //when user has clicked on a row
        selectionChanged: function (data) { 

        }
    });

    //load jtable
    $('#metList').jtable('load', {
        permitID: $("#hiddenPermitGuidField").val(),
        dependentID: $("#hiddenDependentIDField").val(),
        intrayID: $("#hiddenIntrayIDField").val()
    });

    //initialise the table
    $('#notMetList').jtable({
        /*paging: true, //Enable paging
        pageSize: 10,*/ //Set page size (default: 10)
        sorting: true, //Enable sorting
        selecting: true,
        columnSelectable: false,
        actions: {
            listAction: 'getNotMetRequirements'
        },
        fields: {
            description: {
                title: '<b>Description</b>',
                create: false,
                edit: false,
                key: true,
                width: '32%'

            },
            officerName: {
                title: '<b>Officer Name</b>',
                create: false,
                edit: false,
                width: '27%'
            },
            checkDate: {
                title: '<b>Check Date</b>',
                create: false,
                edit: false,
                width: '7%'
            },
            comments: {
                title: '<b>Comments</b>',
                create: false,
                edit: false,
                width: '27%'
            },
            atPost: {
                title: '<b>At Post?</b>',
                create: false,
                edit: false,
                width: '7%'
            }
        },
        //called when all data has been loaded
        recordsLoaded: function (data) {

        },
        //when user has clicked on a row
        selectionChanged: function (data) {

        }
    });

    //load jtable
    $('#notMetList').jtable('load', {
        permitID: $("#hiddenPermitGuidField").val(),
        dependentID: $("#hiddenDependentIDField").val(),
        intrayID: $("#hiddenIntrayIDField").val()
    });







    this.requirmentsMetClick = function (indicator) {

        if (indicator == "Y") {
            var answer = confirm("Are you sure all requirements will not be met?");
            if (answer) {
                $.ajax({
                    type: "GET",
                    url: "processCheckRequirements",
                    async: false,
                    data: {
                        permitID: self.permitID(),
                        intrayID: self.intrayID(),
                        rejectInd: indicator,
                        comments: self.comments()
                    },
                    success: function (result) {
                        //alert("requirements have been checked");
                        //location.reload();
                        if (result == "0") {
                            $('.formclosebutton').trigger("click");
                        } else {
                            alert("there was an error");
                        }
                    }
                });
            }
        } else {
            $.ajax({
                type: "GET",
                url: "processCheckRequirements",
                async: false,
                data: {
                    permitID: self.permitID(),
                    intrayID: self.intrayID(),
                    rejectInd: indicator,
                    comments: self.comments()
                },
                success: function (result) {
                    //alert("requirements have been checked");
                    //location.reload();
                    if (result == "0") {
                        $('.formclosebutton').trigger("click");
                    } else {
                        alert("there was an error");
                    }
                }
            });
        }

        
    };

    /*this.requirmentsNotMetClick = function () {
        $.ajax({
            type: "GET",
            url: "processCheckRequirements",
            async: false,
            data: {
                permitID: self.permitID(),
                intrayID: self.intrayID(),
                rejectInd: "Y",
                comments: self.comments()
            },
            success: function (result) {
                alert("requirements have been checked");
                location.reload();
            }
        });
    };*/

}