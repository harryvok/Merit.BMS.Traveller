function PrintLetterViewModel() {
    var self = this;
   


    this.openWindow = function () {
    
        $.ajax({
            type: "GET",
            url: "createLetter",
            async: false,
            data: {
                permit_guid: $("#hiddenPermitGUID").val(),
                intray_guid: $("#hiddenIntrayGUID").val(),
                comment_txt: $("#comment_txt").html()

            },
            success: function (result) {
                window.open("../pdf/letter" + $("#hiddenPermitGUID").val() + ".pdf", "letter", 'width=800,height=800')
                
            }
        });
    };

}