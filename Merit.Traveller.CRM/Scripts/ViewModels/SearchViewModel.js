function SearchViewModel() {
    var self = this;

    //variables
    this.searchText = ko.observable("");

    //computed functions
    this.search = ko.computed(function () {

        if (this.searchText().length > 0) {
           
           

        }


    }, this);

    //normal functions
    this.find = function () {
        $('#searchLoad').css('display', 'block');

        $.ajax({
            type: "GET",
            url: "/Search/SearchResults",
            async: false,
            data: {
                searchText: self.searchText()
            },
            success: function (result) {

                if (result != null)
                    $("#searchResults").html(result);
                else
                    $("#searchResults").html("");

            },
        });

        $('#searchLoad').css('display', 'none');
    };

}