function ActionIntrayViewModel() {
    var self = this;

    //variables
    this.filterNumber = ko.observable("598439");
    this.searchText = ko.observable("");

   
    //computed functions
    this.filter = ko.computed(function () {

        if (this.filterNumber().length > 0) {

            $.ajax({
                type: "GET",
                url: "/Actions/ActionIntray",
                async: false,
                data: {
                    filterNum: self.filterNumber()
                },
                success: function (result) {

                    if(result != null)
                        $("#actionIntray").html(result);
                    else
                        $("#actionIntray").html("");

                    var rowCount = $('#act tr').length -1;
                    $('#totalNumText').html("Found <b>" + rowCount + "</b> for the filter selected");

                },
            });
        
        }
        return this.filterNumber();
    }, this);

    this.search = ko.computed(function () {
        
        $('#act .intrayRows').each(function () { $(this).show() })

        if (this.searchText().length > 0) {
           
            
            $('#act .intrayRows').each(function () {
                
                var idcell = new String($(this).children().eq(0).html()).toLowerCase();
                var actionRequiredCell = new String($(this).children().eq(1).html()).toLowerCase();
                var categoryCell = new String($(this).children().eq(2).html()).toLowerCase();
                var locationAddressCell = new String($(this).children().eq(3).html()).toLowerCase();
                var customerCell = new String($(this).children().eq(4).html()).toLowerCase();
                var receivedCell = new String($(this).children().eq(5).html()).toLowerCase();
                var dueDateCell = new String($(this).children().eq(6).html()).toLowerCase();
                    
             
                if (idcell.indexOf(self.searchText()) == -1 &&
                    actionRequiredCell.indexOf(self.searchText()) == -1 &&
                    categoryCell.indexOf(self.searchText()) == -1 &&
                    locationAddressCell.indexOf(self.searchText()) == -1 &&
                    customerCell.indexOf(self.searchText()) == -1 &&
                    receivedCell.indexOf(self.searchText()) == -1 &&
                    dueDateCell.indexOf(self.searchText()) == -1) {

                    $(this).hide();

                }
                    
            });
      
        };
        

    }, this);

    //normal functions



}