
        var size_calendar = 0;

    function getcalendar_days(date) {
        $.ajax({
            type: "GET",
            url: "../../PermitIntray/getCalendarDetailsdays",
            data: {
                Date: date
            },
            async: false,
            success: function (result) {
                //alert(result);
                var data1 = JSON.parse(result);
                var data2 = data1.Data;
                //alert('1');
                var j = 0;

                title[j] = "Null";
                url[j] = "http://www.google.com";
                classes[j] = "event-warning";
                start[j] = "10-" + data3.Day + "-1000";
                end[j] = "10-" + data3.Day + "-1000";
                j++;
                size_calendar++;

                //alert(data2.Records.length);
                for (var i = 0; i < data2.Records.length; i++) {
                    var data3 = data2.Records[i];
                    if (data3.unapproval == 0 && data3.unwork == 0 && data3.unprocess == 0 && data3.finpermit == 0) {
                    }
                    else {
                        title[j] = data3.unapproval + ' - ' + data3.unwork + ' - ' + data3.unprocess + ' - ' + data3.finpermit;
                        url[j] = "http://www.google.com";
                        classes[j] = "event-warning";
                        start[j] = "10-" + data3.Day + "-2014";
                        end[j] = "10-" + data3.Day + "-2014";
                        j++;
                        size_calendar++;
                    }
                }
            }
        });
    }

    function getcalendar_month(date) {
        $.ajax({
            type: "GET",
            url: "PermitIntray/getCalendarDetailsmonths",
            data: {
                Date: date
            },
            async: false,
            success: function (result) {
                alert(result);
                /*var data1 = JSON.parse(result);
                var data2 = data1.Data;
                for (var i = 0; i < data2.Records.length; i++) {
                    var data3 = data2.Records[i];
                    title[size_calendar] = data3.varnRef + ' - ' + data3.surname + ' ' + data3.givenName + ' (' + data3.PermitClass + ' )';
                    url[size_calendar] = "http://www.google.com";
                    classes[size_calendar] = "event-success";
                    start[size_calendar] = data3.DateLodged;
                    end[size_calendar] = data3.DateLodged;
                    size_calendar++;  
                }*/
            }
        });
    }
