function MakeDecisionViewModel() {
    var self = this;

    //get webAlias to append onto ajax url
    var alias = $('#webAppAlias').val();

    
    this.decision_ind = ko.observable("");
    this.from_date = ko.observable($("#validFromDate").val());
    this.to_date = ko.observable($("#validToDate").val());
    this.comment_txt = ko.observable("");

   

    if ($("#editable").val() == "Y") {
        $("#declineButton").prop('disabled', true);
        $("#approveButton").prop('disabled', true);
    } else {
        $("#declineButton").prop('disabled', false);
        $("#approveButton").prop('disabled', false);
    }

    this.approvePermit = function (decision) {
        
        $.ajax({
            type: "GET",
            url: "makeDecision",
            async: false,
            data: {
                intray_guid: $("#hiddenIntrayID").val(),
                decision_ind: decision,
                from_date: self.from_date(),
                to_date: self.to_date(),
                comment_txt: self.comment_txt()
            },
            success: function (result) {
                if (result == "0") {
                    $('.formclosebutton').trigger("click");
                }
                else {
                    alert("an error occured");
                }
            }
        });
    };

    this.declinePermit = function () {

    };


}