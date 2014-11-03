function SearchViewModel() {
    var self = this;


    var response = function (event, ui) {
        var label = "";
        var code = "";
        if (typeof ui.content != "undefined" && ui.content.length === 1) {
            label = ui.content[0].label;
            code = ui.content[0].code;
        }
        else if (typeof ui.item != "undefined" && ui.item.label.length > 0) {
            label = ui.item.label;
            code = ui.item.code;
        }
        if (label.length > 0 || code.length > 0) {
            // Stuff that it needs to do once clicked (hint: open the process permit based on it’s varn
        }
    }


    this.getData = function () {
        $("#searchInput").autoCompleteInit("PermitIntray/getList", response);
    };

}