
function PopupViewModel() {
    var self = this;

    this.open = ko.observable(false);
    this.popupHtml = ko.observable("Test");
    this.popupTitle = ko.observable("Test");
    this.popupViewModel = ko.observable();

    /*this.popupOpen = function (data, id, htmlId, title, viewModel ){
        self.open(true);
        self.popupHtml($("#" + id).html());
        self.popupTitle(title);

        //var code = "new " + viewModel + "()";
        var code = "{" + id + ":" + "new " + viewModel + "()" + "}"
        //self.popupViewModel(eval(code));



        ko.applyBindings(self.popupViewModel(), document.getElementById(htmlId));

    };

    this.popupClose = function () {
        self.open(false);
        self.popupHtml("");
        self.popupTitle("");
    };

    this.pptest = function () {
        console.log("test");
    };*/
}