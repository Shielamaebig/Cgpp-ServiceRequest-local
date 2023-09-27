
$(document).ready(function () {
    ManageMaintenance();
    GetMaintenance();
    GetMaintenanceNotif();
    GetMaintenanceNotif2();
    GetMaintenanceNotif3();
    GetMaintenanceNotif4();
});
function ManageMaintenance() {
    $('#saveMaintenance').click(function (e) {
        e.preventDefault();

        //find name
        var isActive = $('select[name=isActive]').val();
        //check true or false
        if (isActive == 1) {
            isActive = true;
        } else {
            isActive = false;
        }
        var mainId = $('input[name=mainId]').val();
        var message = $('textarea[name=maintenanceMessage]').val();
        // data
        var data = {
            isActive: isActive,
            id: 1,
            label: "Maintenance Mode",
            message: message
        };
        $.ajax({
            type: 'PUT',
            url: '/ServiceRequest/api/maintenance/UpdateMaintenance/' + 1,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                toastr.success("Successfully Changed!");
                $('#WarningModal').modal('hide');
                setTimeout(() => {
                    // location.reload();
                    // clear form
                    // $('#saveMaintenance')[0].reset();
                }, 5000);
            },//if failed
            error: function (data) {
                // toastr.info("Success")
            }
        });
    });
}

function GetMaintenance() {
    // ajax get render to table
    setInterval(function () {
        $.ajax({
            type: 'GET',
            url: '/ServiceRequest/api/maintenance/GetMaintenance',
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            // to json
            dataType: 'json',

            success: function (data) {
                // console log the id
                //console.log(data);
                // loop through the data
                $.each(data, function (key, value) {
                    // set the value to the id
                    $('input[name=mainId]').val(value.id);
                    //console.log(value.id);
                    // check if value.isActive is true
                    if (value.isActive == true) {
                        // show modal
                        $('#maintenanceUpdate').modal('show');
                        // render message to .devnotes span
                        $('.devnotes').html(value.message);
                    }
                    // check if value.isActive is false
                    if (value.isActive == false) {
                        // console.log(false);
                    }

                });
            },
            //if failed
            error: function (data) {
                // toastr.error("failed")

            }
        });
    }, 5000);

    // if modal is clicked
    $('#maintenanceUpdate').click(function () {

        localStorage.removeItem('access_token');
        localStorage.removeItem('username');

        window.location.href = "/ServiceRequest/login";
    });

}

function GetMaintenanceNotif() {
    setInterval(function () {
        $.ajax({
            type: 'GET',
            url: '/ServiceRequest/api/sf/getnewRequest',
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
    }, 30000);
}
function GetMaintenanceNotif2() {
    setInterval(function () {
        $.ajax({
            type: 'GET',
            url: '/ServiceRequest/api/sf/getnewRequest',
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                // count the data
                var count = data.length;
                $.each(data, function (key, value) {
                    // render text to span
                    $("#countReps2").html(count);
                });
                if (count == 0) {
                    $('#notifBar1').html(`
                            <a class="dropdown-item" href="/ServiceRequest/SoftwareRequest/Admin"> 
                                <i class="fas fa-check mr-2" aira-hidden="true"></i>
                                <span>No New Repair</span>
                            </a>
                        `);
                }
                var currentCount = $('#notifBar1').children().length;
                for (var i = currentCount; i < count; i++) {
                    var value = data[i];
                    $('#notifBar1').append(`
                        <a class="dropdown-item" href="/ServiceRequest/SoftwareRequest/Admin"> 
                            <i class="fas fa-cog mr-2" aira-hidden="true"></i>
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
    }, 30000);
}

function GetMaintenanceNotif3() {
    setInterval(function () {
        $.ajax({
            type: 'GET',
            url: '/ServiceRequest/api/hd/getnewRequesth',
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                // count the data
                var count = data.length;
                $.each(data, function (key, value) {
                    // render text to span
                    $("#countReps3").html(count);
                });
                if (count == 0) {
                    $('#notifBar3').html(`
                            <a class="dropdown-item" href="/ServiceRequest/HardwareRequest/RequestList"> 
                                <i class="fas fa-check mr-2" aira-hidden="true"></i>
                                <span>No New Repair</span>
                            </a>
                        `);
                }
                var currentCount = $('#notifBar3').children().length;
                for (var i = currentCount; i < count; i++) {
                    var value = data[i];
                    $('#notifBar3').append(`
                        <a class="dropdown-item" href="/ServiceRequest/HardwareRequest/RequestList"> 
                            <i class="fas fa-cog mr-2" aira-hidden="true"></i>
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
    }, 30000);
}

function GetMaintenanceNotif4() {
    setInterval(function () {
        $.ajax({
            type: 'GET',
            url: '/ServiceRequest/api/hd/getnewRequesth',
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                // count the data
                var count = data.length;
                $.each(data, function (key, value) {
                    // render text to span
                    $("#countReps4").html(count);
                });
                if (count == 0) {
                    $('#notifBar4').html(`
                            <a class="dropdown-item" href="/ServiceRequest/HardwareRequest/Admin"> 
                                <i class="fas fa-check mr-2" aira-hidden="true"></i>
                                <span>No New Repair</span>
                            </a>
                        `);
                }
                var currentCount = $('#notifBar4').children().length;
                for (var i = currentCount; i < count; i++) {
                    var value = data[i];
                    $('#notifBar4').append(`
                        <a class="dropdown-item" href="/ServiceRequest/HardwareRequest/Admin"> 
                            <i class="fas fa-cog mr-2" aira-hidden="true"></i>
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
    }, 30000);
}