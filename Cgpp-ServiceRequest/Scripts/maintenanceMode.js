
$(document).ready(function () {
    GetMaintenance();
});

function GetMaintenance() {
    // ajax get render to table
    setInterval(function () {
        $.ajax({
            type: 'GET',
            url: '/api/maintenance/GetMaintenance',
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
    }, 10000);

    // if modal is clicked
    $('#maintenanceUpdate').click(function () {

        localStorage.removeItem('access_token');
        localStorage.removeItem('username');

        window.location.href = "/login";
    });

}
