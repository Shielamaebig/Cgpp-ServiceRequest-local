var apiLocal = '/api';
var apiLive = '/ServiceRequest/api';
$(document).ready(function () {
    GetMaintenanceNotif()
});



function GetMaintenanceNotif() {

    setInterval(function () {
        $.ajax({
            type: 'GET',
            url: apiLocal + '/sf/getnewRequest',
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                // count the data
                var count = data.length;
                $.each(data, function (key, value) {
                    // render text to span
                    $("#countReps5").html(count);
                });
                if (count == 0) {
                    $('#notifBar6').html(`
                            <a class="dropdown-item" href="/ServiceRequest/SoftwareRequest/AcceptList"> 
                                <i class="fas fa-check mr-2" aira-hidden="true"></i>
                                <span>No New Repair</span>
                            </a>
                        `);
                }
                var currentCount = $('#notifBar6').children().length;
                for (var i = currentCount; i < count; i++) {
                    var value = data[i];
                    $('#notifBar6').append(`
                        <a class="dropdown-item" href="/ServiceRequest/SoftwareRequest/AcceptList"> 
                            <i class="bi bi-person-fill mr-2" aira-hidden="true"></i>
                            <span>${value.fullName}</span>
                           <span class="float-right"><i class="far fa-clock ms-2" aria-hidden="true"></i>
                            <time class="timeago" datetime="${moment(value.dateAdded).format('M/D/Y LT')}">
                                ${moment(value.dateAdded).format('M/D/Y LT')}
                            </time>
                            </span>
                        </a>
                    `);

                    // if count is 0

                }
            }
        });
    }, 1000);

    setInterval(function () {
        $.ajax({
            type: 'GET',
            url: apiLocal + '/sf/v4/getnewRequesth',
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                // count the data
                var count = data.length;
                $.each(data, function (key, value) {
                    // render text to span
                    $("#countReps").html(count);
                });
                if (count == 0) {
                    $('#notifBar').html(`
                            <a class="dropdown-item" href="/ServiceRequest/SoftwareRequest/RequestList2"> 
                                <i class="fas fa-check mr-2" aira-hidden="true"></i>
                                <span>No New Repair</span>
                            </a>
                        `);
                }
                var currentCount = $('#notifBar').children().length;
                for (var i = currentCount; i < count; i++) {
                    var value = data[i];
                    $('#notifBar').append(`
                        <a class="dropdown-item" href="/ServiceRequest/SoftwareRequest/RequestList2"> 
                            <i class="bi bi-person-fill mr-2" aira-hidden="true"></i>
                            <span>${value.fullName}</span>
                           <span class="float-right"><i class="far fa-clock ms-2" aria-hidden="true"></i>
                            <time class="timeago" datetime="${moment(value.dateAdded).format('M/D/Y LT')}">
                                ${moment(value.dateAdded).format('M/D/Y LT')}
                            </time>
                            </span>
                        </a>
                    `);

                    // if count is 0

                }
            }
        });
    }, 1000);

}
