function RequestIntrayViewModel() {
    var self = this;

    //variables
    this.filterNumber = ko.observable("596492");
    this.searchText = ko.observable("");

    //computed functions
    this.filter = ko.computed(function () {

        if (this.filterNumber().length > 0) {
            $.ajax({
                type: "GET",
                url: "/Requests/RequestIntray",
                async: false,
                data: {
                    filterNum: self.filterNumber()
                },
                success: function (result) {
                   
                   if (result != null)
                        $("#requestIntray").html(result);
                    else
                        $("#requestIntray").html("");

                    var rowCount = $('#rt tr').length - 1;
                    $('#totalNumText').html("Found <b>" + rowCount + "</b> for the filter selected");
                    
                },
            });

        }
        return this.filterNumber();
        

    }, this);


    this.search = ko.computed(function () {

        $('#rt .intrayRows').each(function () { $(this).show() })

        if (this.searchText().length > 0) {

            $('#rt .intrayRows').each(function () {

                var idcell = new String($(this).children().eq(0).html()).toLowerCase();
                var descriptionCell = new String($(this).children().eq(1).html()).toLowerCase();
                var categoryCell = new String($(this).children().eq(2).html()).toLowerCase();
                var locationAddressCell = new String($(this).children().eq(3).html()).toLowerCase();
                var customerCell = new String($(this).children().eq(4).html()).toLowerCase();
                var dueDateCell = new String($(this).children().eq(5).html()).toLowerCase();


                if (idcell.indexOf(self.searchText()) == -1 &&
                    descriptionCell.indexOf(self.searchText()) == -1 &&
                    categoryCell.indexOf(self.searchText()) == -1 &&
                    locationAddressCell.indexOf(self.searchText()) == -1 &&
                    customerCell.indexOf(self.searchText()) == -1 &&
                    dueDateCell.indexOf(self.searchText()) == -1) {

                    $(this).hide();

                }

            });

        };


    }, this);

}