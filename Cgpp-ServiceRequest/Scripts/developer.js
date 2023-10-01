var apiLocal = '/api';
var apiLive = '/ServiceRequest/api';
$(document).ready(function () {
    ManageMaintenance();
    GetMaintenance();
    GetNewUserNotif();
    GetNewUserForgot();
    GetNewDivRequest();
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
            url: apiLocal + '/maintenance/UpdateMaintenance/' + 1,
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

function GetNewUserNotif() {

    setInterval(function () {
        $.ajax({
            type: 'GET',
            url: apiLocal + '/newusers/get',
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                // count the data
                var count = data.length;
                $.each(data, function (key, value) {
                    // render text to span
                    $("#newusercount").html(count);
                });
                if (count == 0) {
                    $('#newAccountList').html(`
                            <a class="dropdown-item" href="/ServiceRequest/LoginActivity/RegistrationRequest"> 
                                <i class="fas fa-check mr-2" aira-hidden="true"></i>
                                <span>No New Request Account</span>
                            </a>
                        `);
                }
                var currentCount = $('#newAccountList').children().length;
                for (var i = currentCount; i < count; i++) {
                    var value = data[i];
                    $('#newAccountList').append(`
                        <a class="dropdown-item" href="/ServiceRequest/LoginActivity/RegistrationRequest"> 
                            <i class="bi bi-person-fill mr-2" aira-hidden="true"></i>
                            <span>${value.firstName} ${value.lastName}</span>
                           <span class="float-right"><i class="far fa-clock ms-2" aria-hidden="true"></i>
                            <time class="timeago" datetime="${moment(value.dateCreated).format('M/D/Y LT')}">
                                ${moment(value.dateCreated).format('M/D/Y LT')}
                            </time>
                            </span>
                        </a>
                    `);

                    // if count is 0

                }
            }
        });
    }, 10000);
}

function GetNewUserForgot() {
    setInterval(function () {
        $.ajax({
            type: 'GET',
            url:  apiLocal + '/forgotPass/get',
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                // count the data
                var count = data.length;
                $.each(data, function (key, value) {
                    // render text to span
                    $("#newforgot").html(count);
                });
                if (count == 0) {
                    $('#newforgotList').html(`
                            <a class="dropdown-item" href="/ServiceRequest/LoginActivity/ForgotRequest"> 
                                <i class="fas fa-check mr-2" aira-hidden="true"></i>
                                <span>No New Forgot Password</span>
                            </a>
                        `);
                }
                var currentCount = $('#newforgotList').children().length;
                for (var i = currentCount; i < count; i++) {
                    var value = data[i];
                    $('#newforgotList').append(`
                        <a class="dropdown-item" href="/ServiceRequest/LoginActivity/ForgotRequest"> 
                            <i class="bi bi-person-fill mr-2" aira-hidden="true"></i>
                            <span>${value.firstName} ${value.lastName}</span>
                           <span class="float-right"><i class="far fa-clock ms-2" aria-hidden="true"></i>
                            <time class="timeago" datetime="${moment(value.dateCreated).format('M/D/Y LT')}">
                                ${moment(value.dateCreated).format('M/D/Y LT')}
                            </time>
                            </span>
                        </a>
                    `);

                    // if count is 0

                }
            }
        });
    }, 10000);
}

function GetNewDivRequest() {
    setInterval(function () {
        $.ajax({
            type: 'GET',
            url: apiLocal + '/NewDivRequest/get',
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                // count the data
                var count = data.length;
                $.each(data, function (key, value) {
                    // render text to span
                    $("#newRequest").html(count);
                });
                if (count == 0) {
                    $('#newRequestList').html(`
                            <a class="dropdown-item" href="/ServiceRequest/SoftwareRequest/AcceptList"> 
                                <i class="fas fa-check mr-2" aira-hidden="true"></i>
                                <span>No New Request</span>
                            </a>
                        `);
                }
                var currentCount = $('#newRequestList').children().length;
                for (var i = currentCount; i < count; i++) {
                    var value = data[i];
                    $('#newRequestList').append(`
                        <a class="dropdown-item" href="/ServiceRequest/SoftwareRequest/AcceptList"> 
                            <i class="bi bi-person-fill mr-2" aira-hidden="true"></i>
                            <span>${value.fullName}</span>
                           <span class="float-right"><i class="far fa-clock ms-2" aria-hidden="true"></i>
                            <time class="timeago" datetime="${moment(value.dateCreated).format('M/D/Y LT')}">
                                ${moment(value.dateCreated).format('M/D/Y LT')}
                            </time>
                            </span>
                        </a>
                    `);

                    // if count is 0

                }
            }
        });
    }, 10000);
}