function PermitViewModel() {
    var self = this;

    this.testVar = ko.observable("");

    this.filter = function () {
        alert("ha");
    };

    this.testCompute = ko.computed(function () {

        if (this.testVar().length > 0)
            alert("im here");

    }, this);


}