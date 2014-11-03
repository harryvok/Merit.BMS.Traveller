function ReturnDocumentViewModel() {
    var self = this;
    this.dateReturned = ko.observable("");
    this.comments = ko.observable("");
    this.permitID = ko.observable("");

    this.processReturnedDocument = function () {

        $.ajax({
            type: "GET",
            url: "processReturneDocument",
            async: false,
            data: {
                permitID: $("#hiddenpermitID").val(),
                returnedDate: self.dateReturned(),
                comments: self.comments(),
                documentNo: $("#docNo").html()
            },
            success: function (result) {
                if (result != "success")
                    alert(result);
                else {
                    $('.formclosebutton').trigger("click");
                }
            }
        });

    }
}