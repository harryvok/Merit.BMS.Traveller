
        var size_calendar = 0;

    function getcalendar_officerwork() {
        $.ajax({
            type: "GET",
            url: "PermitIntray/getCalendarFirst",
            async: false,
            success: function (result) {
                var data1 = JSON.parse(result);
                var data2 = data1.Data;
                for (var i = 0; i < data2.Records.length; i++) {
                    var data3 = data2.Records[i];
                    title[i] = data3.varnRef + ' - ' + data3.surname + ' ' + data3.givenName + ' (' + data3.PermitClass + ' )';
                    url[i] = "http://www.google.com";
                    classes[i] = "event-warning";
                    start[i] = data3.DateLodged;
                    end[i] = data3.DateLodged;
                    size_calendar++;
                }
            }
        });
    }

    function getcalendar_officerworknew() {
        $.ajax({
            type: "GET",
            url: "PermitIntray/getCalendarSecond",
            async: false,
            success: function (result) {
                var data1 = JSON.parse(result);
                var data2 = data1.Data;
                for (var i = 0; i < data2.Records.length; i++) {
                    var data3 = data2.Records[i];
                    title[size_calendar] = data3.varnRef + ' - ' + data3.surname + ' ' + data3.givenName + ' (' + data3.PermitClass + ' )';
                    url[size_calendar] = "http://www.google.com";
                    classes[size_calendar] = "event-success";
                    start[size_calendar] = data3.DateLodged;
                    end[size_calendar] = data3.DateLodged;
                    size_calendar++;  
                }
            }
        });
    }
