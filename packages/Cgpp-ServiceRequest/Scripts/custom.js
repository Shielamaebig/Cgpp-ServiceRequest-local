
$(document).ready(function () {
    Logout();
    signout();
    LoginChecker();
    GetToken();
    //Logout sidenav
    function Logout() {
        // apply function to all elements with class logout

        $('#logout').click(function () {
            // modal please wait
            $('#pleasewait').modal('show');

            var id = $('#logout').attr('data-id');
            //alert(id);

            $.ajax({
                type: 'POST',
                url: '/ServiceRequest/api/Account/ChangeLogStatusToFalse/' + id,
                headers: {
                    'Authorization': 'Bearer ' + localStorage.getItem('access_token')
                },
                success: function (data) {

                    setTimeout(function () {
                        localStorage.removeItem('access_token');
                        localStorage.removeItem('username');
                        // redirect to login page
                    }, 2000),
                        setTimeout(function () {
                            window.location.href = "/ServiceRequest/Login";
                        }, 3000);
                },
                error: function (data) {

                }
            });
        });
    }

    //signout dropdown (Name)
    function signout() {
        $('#signout').click(function () {
            $('#pleasewait').modal('show');
            var id = $('#signout').attr('data-id');
            //alert('id', id);
            $.ajax({
                type: 'POST',
                url: '/ServiceRequest/api/Account/ChangeLogStatusToFalse/' + id,
                headers: {
                    'Authorization': 'Bearer ' + localStorage.getItem('access_token')
                },
                success: function (data) {

                    setTimeout(function () {
                        localStorage.removeItem('access_token');
                        localStorage.removeItem('username');
                        // redirect to login page
                    }, 2000),
                        setTimeout(function () {
                            window.location.href = "/ServiceRequest/Login";
                        }, 3000);
                },
                error: function (data) {

                }
            })
        });
    }

    function BrowserAlert() {
        //console.log('');
        // remove localstorage on windows close 
        window.onbeforeunload = function () {
            localStorage.removeItem('access_token');
            localStorage.removeItem('username');
        }
    }

    //check if name in nav is existing
    function LoginChecker() {
        // find element with class .side-user-name
        var side = $('#side-name');
        // count text count 
        // trim spaces
        var trim = side.text().trim();
        console.log('trim', trim);
        if (trim.length == '') {

            localStorage.removeItem('access_token');
            localStorage.removeItem('username');
            $('#sessionExpired').modal('show');
            $('#sessionExpired').on('hidden.bs.modal', function () {
                window.location.href = "/ServiceRequest/Login";
            }
            );
        }
    }


    //if no username/token log out
    function GetToken() {
        var token = localStorage.getItem('access_token');
        var username = localStorage.getItem('username');

        if (token == null) {
            $('#sessionExpired').modal('show');
            $('#sessionExpired').on('hidden.bs.modal', function () {
                window.location.href = "/ServiceRequest/Login";
            });
        }
        else if (token == '') {
            $('#sessionExpired').modal('show');
        }
        else {
            console.log('username', username);
        }
        window.onload = function () {
            if (token == null) {
                $('#sessionExpired').modal('show');

            }
        }
    }
    //Image Logged
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/users/GetUserImage',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('#userLoggedImg').attr('src', "/ServiceRequest/" + data);
        },
        error: function (data) {
            //  toastr.error("error");
        }
    });

    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/users/GetUserImage',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('#userLoggedImg3').attr('src', "/ServiceRequest/" + data);
        },
        error: function (data) {
            //  toastr.error("error");
        }
    });

    //Image UserProfile
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/users/GetUserImage',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('#userProfileImg').attr('src', "/ServiceRequest/" + data);
        },
        error: function (data) {
            //  toastr.error("error");
        }
    })




    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/users/GetUserFullName',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {

            $('#side-name').text(data);
        },
        //if failed
        error: function (data) {

        }

    });

    //Users HardwareRequest List Function

});
//End of Ajax

function GetDepartments() {
    var Datatables;
    Datatables = $("#departmentList").DataTable({
        ajax: {
            url: "/ServiceRequest/api/dept/get",
            dataSrc: "",
            headers: {
                "Authorization": "Bearer " + localStorage.getItem('access_token')
            }
        },
        autoWidth: false,
        columns: [

            {
                data: "dateAdded",
            },
            {
                data: "name",
            },
            {
                data: null,
                render: function (data) {
                    return '<button class="btn btn-success btn-sm edit" data-id="' + data.id + '"><i class="fas fa-edit"></i> Edit</button>'
                        + '  <button class="btn btn-danger btn-sm delete" data-id="' + data.id + '"><i class="fas fa-trash"></i> Delete</button>'
                },
                "orderable": false,
            }

        ]
    });

    $('#btnSubmit').click(function (e) {
        e.preventDefault();

        //find name
        var name = $('input[name=name]').val();
        // data
        var data = {
            name: name,
        };
        $.ajax({
            type: 'POST',
            url: '/ServiceRequest/api/dept/save',
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                $("#name").val("");

                $('#departmentAdd').modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success("Department Successfully Added!");
                    // hide please wait modal
                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            //if failed
            error: function (data) {
                toastr.error("Name Already Exist In the Database/ Invalid")
            }
        });
    });

    //Get edit
    $("#departmentList").on('click', '.edit', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/dept/getdepartment/' + id;
        //alert(id);
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $("#editmodal").modal('show');
                $('#edit').find('input[name="id"]').val(data.id);
                $('#edit').find('input[name="name"]').val(data.name);
            },
            //if failed
            error: function (data) {
                // console.log(data, data.id, data.name);
                toastr.error("error")
            }
        })
    })

    $("#UpdateRecord").click(function (e) {
        e.preventDefault();
        var data = {
            name: $('#edit').find('input[name=name]').val(),
        };
        var id = $('#edit').find('input[name="id"]').val();
        $.ajax({
            type: 'PUT',
            url: '/ServiceRequest/api/dept/updatedept/' + id,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                $('#editmodal').modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success("Data Successfully Updated!");
                    // hide please wait modal
                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            //if failed
            error: function (data) {
                toastr.error("Error Saving")
            }
        });
    });

    $('#departmentList').on('click', '.delete', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/department/Delete/' + id;
        $.ajax({
            // confirm before delete
            beforeSend: function (xhr) {
                // use bootbox
                bootbox.confirm({
                    message: "Are you sure you want to delete this record?",
                    buttons: {
                        // yes and no sequence
                        confirm: {
                            label: 'Yes',
                            className: 'btn-success btn-sm'
                        },
                        cancel: {
                            label: 'No',
                            className: 'btn-danger btn-sm'
                        }
                    },
                    callback: function (result) {
                        if (result) {
                            $.ajax({
                                type: 'DELETE',
                                url: url,
                                headers: {
                                    'Authorization': 'Bearer ' + localStorage.getItem('access_token')
                                },
                                success: function (data) {
                                    //show please wait modal
                                    $('#pleasewait').modal('show');
                                    //show toastr after 3
                                    setTimeout(function () {
                                        toastr.success("Department Successfully Deleted!");
                                    }, 2000);
                                    setTimeout(function () {
                                        window.location.reload();
                                    }, 3000);
                                },
                                //if failed
                                error: function (data) {
                                    toastr.error("Error Deleting Data")
                                }
                            });
                        }
                    }
                });

            },
        });
    });
}
//Start of UsersHardware Request
function GetHardwareRequest() {

    var table2 = $("#hardwareRequestList").DataTable({
        "ajax": {
            "url": "/ServiceRequest/api/hr/user",
            "headers": {
                "Authorization": "Bearer " + localStorage.getItem('access_token')
            },
            "type": "GET",
            "dataType": "json",
        },
        "processing": "true",
        "serverSide": "true",
        "ordering": "true",
        "paging": "true",
        "fnInitComplete": function (oSettings, json) {
            // search after pressing enter
            $('#hardwareRequestList_filter input').unbind();
            $('#hardwareRequestList_filter input').bind('keyup', function (e) {
                if (e.keyCode == 13) {
                    // trigger search 
                    $('#pleasewait').modal('show');
                    setTimeout(function () {
                        $('#pleasewait').modal('hide');
                    }, 2500);
                    $('#hardwareRequestList').DataTable().search(this.value).draw();
                }
                // if search is empty, reset the filter
                if (this.value == "") {
                    $('#hardwareRequestList').DataTable().search("").draw();
                }
            }
            );
        },
        // "searching" : " true",
        // "searchDelay" : 1000,
        // order by desc dateuploaded
        "order": [[1, "desc"]],
        "language": {
            "processing": "Loading... Please wait"
        },
        "autoWidth": false,
        "columns": [
            {
                data: "ticket",
                render: function (data, type, row) {
                    if (data == null) {
                        return '<span class="badge bg-danger">No Ticket</span>';
                    }
                    return `<span class="badge bg-info">${(data)}</span>`
                }
            },
            { data: "dateAdded" },
            { data: "fullName" },
            {
                "data": "hardwareId",
                render: function (data, type, row) {
                    return row.hardware.name;
                }
            },
            {
                data: "statusId", render: function (data, type, row) {
                    if (data == 1) {
                        return '<span class="badge  bg-secondary">Open</span>'
                    }
                    else if (data == 2) {
                        return '<span class="badge bg-success">Resolved</span>'
                    }
                    else if (data == 3) {
                        return '<span class="badge  bg-warning">On hold</span>'
                    }
                    else if (data == 4) {
                        return '<span class="badge  bg-info">In Progress</span>'
                    }
                    else if (data == 5) {
                        return '<span class="badge  bg-danger">Cancel Request</span>'
                    }
                    else
                        return '<span class="badge  bg-secondary">Open</span>'
                }
            },
            {
                data: null,
                render: function (data, type, full) {
                    return '<button class="btn btn-info btn-sm view ms-1 mb-1 text-white" data-id="' + data.id + '"><i class="far fa-eye"></i> View </button>' +
                        '<button class="btn btn-success  btn-sm attach ms-1 mb-1 text-white" data-id="' + data.id + '"><i class="fas fa-paperclip"></i> Attachments</button>' +
                        '<button class="btn btn-primary btn-sm request ms-1 mb-1 text-white" data-id="' + data.id + '"><i class="far fa-edit"></i> Edit</button>' +
                        '<button class="btn btn-danger btn-sm cancel ms-1 mb-1 text-white" data-id="' + data.id + '"><i class="fas fa-trash-alt"></i> Cancel </button>'
                },
                orderable: false,
                width: "370px",
            },

        ],
    });
    var urls = [];

    $('#hardwareRequestList').on('click', '.attach', function () {
        var id = $(this).attr('data-id');
        // alert(id);
        var url = '/ServiceRequest/api/hardwareRequest/GetRequestbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data, value) {
                $('#hardwareDisplay').modal('show');
                $('#hardwareDisplay').find('.modal-title').text('File Attachment');
                $('#attachementModel').find('input[name="statusId"]').val(data.statusId);
                if (data.statusId == 2)
                    $('.update-image').prop('disabled', true),
                        $('.delete').prop('disabled', true)

                else if (data.statusId == 3)
                    $('.update-image').prop('disabled', true),
                        $('.delete').prop('disabled', true)

                else if (data.statusId == 4)
                    $('.update-image').prop('disabled', true),
                        $('.delete').prop('disabled', true)

                else if (data.statusId == 5)
                    $('.update-image').prop('disabled', true),
                        $('.delete').prop('disabled', true)
                else
                    $('.update-image').prop('disabled', false),
                        $('.delete').prop('disabled', false)
                var refId = $('#hardwareRequestId').val(data.id);
                //alert(data.id);
                //alert(data.statusId);
                var uploadTable = $('#UploadList').DataTable({
                    destroy: true,

                    ajax: {
                        url: "/ServiceRequest/api/v2/uploadDislpay/" + id,
                        dataSrc: '',
                        headers: {
                            "Authorization": "Bearer " + localStorage.getItem('access_token')
                        }
                    },
                    searching: false,
                    ordering: false,

                    columns: [
                        {
                            data: "dateAdded"
                        },
                        {
                            data: "documentBlob",
                            render: function (data, type, row, meta) {
                                var fileType = row.imagePath.split('.').pop();
                                if (fileType == 'pdf') {
                                    return '<i class="fas fa-file-pdf fa-2x" style="color: red; font-size: 50px;"></i>';
                                } else if (fileType == 'doc' || fileType == 'docx') {
                                    return '<i class="fas fa-file-word fa-2x" style="color: blue; font-size: 50px;"> </i>';
                                } else if (fileType == 'xls' || fileType == 'xlsx') {
                                    return '<i class="fas fa-file-excel fa-2x" style="color: green; font-size: 50px;"></i>';
                                } else if (fileType == 'ppt' || fileType == 'pptx') {
                                    return '<i class="fas fa-file-powerpoint fa-2x" style="color: orange; font-size: 50px;"></i>';
                                } else if (fileType == 'jpg' || fileType == 'png' || fileType == 'jpeg') {
                                    return '<img src="data:image/png;base64,' + data + '" class="avatar" width="250" height="250"/>';
                                } else {
                                    return '<i class="bi bi-images" style="font-size: 250px;"></i> ';
                                }
                            },
                            className: "text-center"

                        },
                        {
                            // download link by using anchor link only
                            data: "documentBlob",
                            render: function (data, type, row) {
                                var f = new Blob([s2ab(atob(data))], { type: "" });
                                var filename = row.imagePath.replace(/^.*[\\\/]/, '');
                                var newF = URL.createObjectURL(f);
                                urls.push(newF);
                                var fileType = row.imagePath.split('.').pop();

                                //    check file if is image type
                                if (fileType == 'jpg' || fileType == 'png' || fileType == 'jpeg' || fileType == 'pdf' || fileType == 'gif') {
                                    // return '<a href="/api/v1/download/' + row.id + '" class="btn btn-sm btn-success">Download</a>'
                                    //     +
                                    return '<a href="' + newF + '" class="btn btn-sm btn-success" download="' + filename + '">Download</a>';
                                }
                                else {
                                    return ''

                                }
                            },
                            className: "text-center"
                        },

                    ]
                });

                console.log(data.id);
                $('#hardwareDisplay').find('input[name=cheat]').val(data.id);
                $('#upload2').find('input[name=hardwareRequestId]').val(data.id);
                // refresh page on modal close
                $('#hardwareDisplay').on('hidden.bs.modal', function () {
                    // remove id on select
                    $('select[name=hardwareRequestId]').find('option[value=' + data.id + ']').remove();
                });
            }
        });

    });

    $('#UploadList').on('click', '.delete', function () {
        var id = $(this).attr('data-id');
        $('#hardwareDisplay').modal('hide');
        bootbox.confirm({
            message: "Are you sure you want to delete this record?",
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-success btn-sm'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-danger btn-sm '
                }
            },
            callback: function (result) {
                if (result) {
                    $.ajax({
                        type: 'DELETE',
                        url: '/ServiceRequest/api/v1/deleteImage/' + id,
                        headers: {
                            "Authorization": "Bearer " + localStorage.getItem('access_token')
                        },
                        success: function (data) {
                            $('#pleasewait').modal('show');
                            //show toastr after 3
                            setTimeout(function () {
                                toastr.success("File Successfully Added!");
                                // hide please wait modal

                            }, 2000);
                            setTimeout(function () {
                                location.reload();
                                $('#upload2')[0].reset();
                            }, 3000);
                        }
                    });
                }
            }
        });
    });

    $('#upload2').submit(function (e) {
        e.preventDefault();
        var formData = new FormData(this);
        $.ajax({
            url: '/ServiceRequest/api/v2/saveFile',
            type: 'POST',
            data: formData,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {

                $('#editModal').modal('hide');
                $('#hardwareDisplay').modal('hide');

                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success("File Successfully Added!");
                    // hide please wait modal

                }, 2000);
                setTimeout(function () {
                    location.reload();
                    $('#upload2')[0].reset();
                }, 3000);
            },
            cache: false,
            contentType: false,
            processData: false,
            error: function (data) {
                toastr.error("Error Updating File")
            }
        });

    });

    //get RequestDetailsModal
    $('#hardwareRequestList').on('click', '.request', function () {
        var id = $(this).attr('data-id');
        // alert(id);
        var url = '/ServiceRequest/api/hardwareRequest/GetRequestbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#hardwareRequestModal').modal('show');

                $('#EditRequest').find('input[name="id"]').val(data.id);
                $('#EditRequest').find('input[name="brandName"]').val(data.brandName);
                $('#EditRequest').find('input[name="modelName"]').val(data.modelName);
                $('#EditRequest').find('input[name="FileName"]').val(data.documentLabel);
                $('#EditRequest').find('select[name="unitTypeId"]').val(data.unitTypeId);
                $('#EditRequest').find('select[name="hardwareId"]').val(data.hardwareId);
                $('#EditRequest').find('textarea[name="description"]').val(data.description);


                $('#EditRequest').find('input[name="statusId"]').val(data.statusId);
                if (data.statusId == 2)
                    $('.update-request').prop('disabled', true)

                else if (data.statusId == 3)
                    $('.update-request').prop('disabled', true)

                else if (data.statusId == 4)
                    $('.update-request').prop('disabled', true)

                else if (data.statusId == 5)
                    $('.update-request').prop('disabled', true)
                else
                    $('.update-request').prop('disabled', false)
                //alert(data.statusId);
            }


        });
        // alert(data);
    });


    //save edit
    $('#UpdateRequest').click(function (e) {
        e.preventDefault();

        var id = $('#EditRequest').find('input[name="id"]').val();
        //var data = $('#EditRequest').serialize();

        //alert(id);
        var data = {
            brandName: $('#EditRequest').find('input[name=brandName]').val(),
            hardwareId: $('#EditRequest').find('select[name=hardwareId]').val(),
            modelName: $('#EditRequest').find('input[name=modelName]').val(),
            documentLabel: $('#EditRequest').find('input[name=FileName]').val(),
            unitTypeId: $('#EditRequest').find('select[name=unitTypeId]').val(),
            hardwareId: $('#EditRequest').find('select[name=hardwareId]').val(),
            description: $('#EditRequest').find('textarea[name=description]').val(),
        }
        $.ajax({
            type: 'PUT',
            url: '/ServiceRequest/api/hardwareRequest/Request/' + id,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                $('#hardwareRequestModal').modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success(" Successfully Update Request!");
                    // hide please wait modal

                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            //if failed
            error: function (data) {
                console.log(data.id);
                toastr.error("Fill up alll forms / Invalid")
            }
        })
    })
    //get CancelModal
    $('#hardwareRequestList').on('click', '.cancel', function () {
        var id = $(this).attr('data-id');
        //alert(id);
        var url = '/ServiceRequest/api/hardwareRequest/GetRequestbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#CancelRequestModal').modal('show');

                $('#cancelRequest').find('input[name="id"]').val(data.id);
                $('#cancelRequest').find('input[name="brandName"]').val(data.brandName);
                $('#cancelRequest').find('input[name="modelName"]').val(data.modelName);
                $('#cancelRequest').find('select[name="unitTypeId"]').val(data.unitTypeId);
                $('#cancelRequest').find('select[name="hardwareId"]').val(data.hardwareId);
                $('#cancelRequest').find('textarea[name="description"]').val(data.description);

                $('#cancleRequest').find('input[name="statusId"]').val(data.statusId);
                //alert(data.statusId);
                if (data.statusId == 2)
                    $('.cancel-request').attr('disabled', true);
                else if (data.statusId == 3)
                    $('.cancel-request').attr('disabled', true);
                else if (data.statusId == 4)
                    $('.cancel-request').attr('disabled', true);
                else if (data.statusId == 5)
                    $('.cancel-request').attr('disabled', true);
                else
                    $('.cancel-request').attr('disabled', false);
            }
        })
        // alert(data);
    });
    // save cancel
    $('#cancel').click(function (e) {
        e.preventDefault();
        var id = $('#cancelRequest').find('input[name="id"]').val();
        var data = $('#cancelRequest').serialize();
        // alert(id);
        $.ajax({
            type: 'PUT',
            url: '/ServiceRequest/api/hardwareRequest/cancel/' + id,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                $('#CancelRequestModal').modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success(" Successfully Cancelled a Request!");
                    // hide please wait modal
                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            //if failed
            error: function (data) {
                toastr.error("Error Canceling a Request")
            }
        })
    })

    //get Details
    $('#hardwareRequestList').on('click', '.view', function () {
        var id = $(this).attr('data-id');
        //alert(id);
        var url = '/ServiceRequest/api/hardwareRequest/GetRequestbyId/' + id;


        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#viewmodal').modal('show');
                $('#viewRequest').find('input[name="id"]').val(data.id);
                $('#viewRequest').find('input[name="fullName"]').val(data.fullName);
                $('#viewRequest').find('input[name="mobileNumber"]').val(data.mobileNumber);
                $('#viewRequest').find('input[name="modelName"]').val(data.modelName);
                $('#viewRequest').find('select[name="departmentsId"]').val(data.departmentsId);
                $('#viewRequest').find('select[name="divisionsId"]').val(data.divisionsId);
                $('#viewRequest').find('select[name="hardwareId"]').val(data.hardwareId);
                $('#viewRequest').find('select[name="unitTypeId"]').val(data.unitTypeId);
                $('#viewRequest').find('input[name="brandName"]').val(data.brandName);
                $('#viewRequest').find('input[name="FileName"]').val(data.documentLabel);
                $('#viewRequest').find('textarea[name="description"]').val(data.description);
                $('#viewRequest').find('select[name="statusId"]').val(data.statusId);
                $('#viewRequest').find('select[name="findingId"]').val(data.findingId);
                $('#viewRequest').find('select[name="hardwareTechnicianId"]').val(data.hardwareTechnicianId);
                $('#viewRequest').find('input[name="possibleCause"]').val(data.possibleCause);
                $('#viewRequest').find('textarea[name="remarks"]').val(data.remarks);
                $('#viewRequest').find('input[name="dateStarted"]').val(data.dateStarted);
                $('#viewRequest').find('input[name="dateEnded"]').val(data.dateEnded);
                $('#viewRequest').find('input[name="timeStarted"]').val(data.timeStarted);
                $('#viewRequest').find('input[name="timeEnded"]').val(data.timeEnded);
                $('#viewRequest').find('input[name="approver"]').val(data.approver);
                $('#viewRequest').find('input[name="dateApproved"]').val(data.dateApproved);
                $('#viewRequest').find('input[name="viewer"]').val(data.viewer);
                $('#viewRequest').find('input[name="dateView"]').val(data.dateView);
                $('#viewRequest').find('input[name="serialNumber"]').val(data.serialNumber);
                $('#viewRequest').find('input[name="controlNumber"]').val(data.controlNumber);

                $('.firstBlur').removeClass('modalBlur');

            }
        })
        // alert(data);
    });

    //UnitType
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/unitType',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $.each(data, function (index, value) {
                $('select[name=unitTypeId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });

    //Department
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/department',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=departmentsId]').append('<option value=""> Select Department</option>');
            $.each(data, function (index, value) {
                $('select[name=departmentsId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    //Division
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/division',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=divisionsId]').append('<option value=""> Select Division</option>');
            $.each(data, function (index, value) {
                $('select[name=divisionsId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    //hardwareCategory
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/hardware',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=hardwareId]').append('<option value=""> Select Hardware Service Category</option>');
            $.each(data, function (index, value) {
                $('select[name=hardwareId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    //Technician
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/Technician',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=hardwareTechnicianId]').append('<option value=""> Select Technician</option>');
            $.each(data, function (index, value) {
                $('select[name=hardwareTechnicianId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    //Status
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/status',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=statusId]').append('<option value=""> Select Status </option>');
            $.each(data, function (index, value) {
                $('select[name=statusId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    //Findings
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/Findings',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=findingId]').append('<option value=""> Select Result</option>');
            $.each(data, function (index, value) {
                $('select[name=findingId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },
    });
    //Delete
    $('#hardwareRequestList').on('click', '.delete', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/hardwareRequest/Delete/' + id;
        $.ajax({
            // confirm before delete
            beforeSend: function (xhr) {
                // use bootbox
                bootbox.confirm({
                    message: "Are you sure you want to delete this record?",
                    buttons: {
                        // yes and no sequence

                        confirm: {
                            label: 'Yes',
                            className: 'btn-success btn-sm'
                        },
                        cancel: {
                            label: 'No',
                            className: 'btn-danger btn-sm'
                        }
                    },
                    callback: function (result) {
                        if (result) {
                            $.ajax({
                                type: 'DELETE',
                                url: url,
                                headers: {
                                    'Authorization': 'Bearer ' + localStorage.getItem('access_token')
                                },
                                success: function (data) {
                                    //toastr.success("Data Successfully Deleted!");
                                    //tables.ajax.reload();

                                    //$('#CancelRequestModal').modal('hide');
                                    //show please wait modal
                                    $('#pleasewait').modal('show');
                                    //show toastr after 3
                                    setTimeout(function () {
                                        toastr.success("Data Successfully Deleted!");
                                        // hide please wait modal
                                    }, 2000);
                                    setTimeout(function () {
                                        window.location.reload();
                                    }, 3000);
                                },
                                //if failed
                                error: function (data) {
                                    toastr.error("Error")
                                }
                            });
                        }
                    }
                });

            },
        });
    });

}
//Start of Departments
function Departments() {
    var Datatables;
    Datatables = $("#departmentList").DataTable({
        ajax: {
            url: "/ServiceRequest/api/dept/get",
            dataSrc: "",
            headers: {
                "Authorization": "Bearer " + localStorage.getItem('access_token')
            }
        },
        autoWidth: false,
        columns: [

            {
                data: "dateAdded",
            },
            {
                data: "name",
            },

        ]
    });

    $('#btnSubmit').click(function (e) {
        e.preventDefault();

        //find name
        var name = $('input[name=name]').val();
        // data
        var data = {
            name: name,
        };
        $.ajax({
            type: 'POST',
            url: '/ServiceRequest/api/dept/save',
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                //toastr.success("Directory Successfully Added!");
                //Datatables.ajax.reload();
                //$("#departmentAdd").modal('hide');
                $("#name").val("");
                $('#departmentAdd').modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success("Department Successfully Updated!");
                    // hide please wait modal
                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            //if failed
            error: function (data) {
                toastr.error("Name Already Exist In the Database/ Invalid")
            }
        });
    });

    //Get edit
    $("#departmentList").on('click', '.edit', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/dept/getdepartment/' + id;
        //alert(id);
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $("#editmodal").modal('show');
                $('#edit').find('input[name="id"]').val(data.id);
                $('#edit').find('input[name="name"]').val(data.name);
            },
            //if failed
            error: function (data) {
                // console.log(data, data.id, data.name);
                toastr.error("Error")
            }
        })
    })

    $("#UpdateRecord").click(function (e) {
        e.preventDefault();
        var data = {
            name: $('#edit').find('input[name=name]').val(),
        };
        var id = $('#edit').find('input[name="id"]').val();
        $.ajax({
            type: 'PUT',
            url: '/ServiceRequest/api/dept/updatedept/' + id,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                toastr.success("Data Successfully Updated!");
                Datatables.ajax.reload();
                $('#editmodal').modal('hide');

            },
            //if failed
            error: function (data) {
                toastr.error("Error")
            }
        });
    });


    $('#departmentList').on('click', '.delete', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/department/Delete/' + id;
        $.ajax({
            // confirm before delete
            beforeSend: function (xhr) {
                // use bootbox
                bootbox.confirm({
                    message: "Are you sure you want to delete this record?",
                    buttons: {
                        // yes and no sequence

                        confirm: {
                            label: 'Yes',
                            className: 'btn-success btn-sm'
                        },
                        cancel: {
                            label: 'No',
                            className: 'btn-danger btn-sm'
                        }
                    },
                    callback: function (result) {
                        if (result) {
                            $.ajax({
                                type: 'DELETE',
                                url: url,
                                headers: {
                                    'Authorization': 'Bearer ' + localStorage.getItem('access_token')
                                },
                                success: function (data) {
                                    //show please wait modal
                                    $('#pleasewait').modal('show');
                                    //show toastr after 3
                                    setTimeout(function () {
                                        toastr.success("Data Successfully Deleted!");
                                        // hide please wait modal
                                    }, 2000);
                                    setTimeout(function () {
                                        window.location.reload();
                                    }, 3000);
                                },
                                //if failed
                                error: function (data) {
                                    toastr.error("Error")
                                }
                            });
                        }
                    }
                });

            },
        });
    });
}
//Start of Division
function Divisions() {
    var Datatables;
    Datatables = $("#divisionList").DataTable({
        ajax: {
            type: "GET",
            url: "/ServiceRequest/api/divisions/getdivisions",
            dataSrc: "",
        },
        autoWidth: false,
        ordering: false,
        columns: [
            {
                "data": "dateAdded"

            },
            { "data": "name" },

            {
                "data": "departmentsId",
                render: function (data, type, row) {
                    return row.departments.name;
                }
            },
            {
                data: null,
                render: function (data) {
                    return '<button class="btn btn-success btn-sm edit" data-id="' + data.id + '"><i class="fas fa-edit"></i> Edit</button>'
                        + '  <button class="btn btn-danger btn-sm delete" data-id="' + data.id + '"><i class="fas fa-trash"></i> Delete</button>'
                },
                "orderable": false,
            },
        ],

    });



    //get department
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/div/department',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=departmentsId]').append('<option value="-1"> Select Department</option>');
            $.each(data, function (index, value) {
                $('select[name=departmentsId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });


    //post 
    $("#btnSubmit").click(function (e) {
        //Serialize the form datas
        e.preventDefault();
        var name = $('input[name=name]').val();
        var departmentsId = $('select[name=departmentsId]').val();

        var data = {
            name: name,
            departmentsId: departmentsId
        };

        $.ajax({
            url: "/ServiceRequest/api/division/save",
            type: "POST",
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            data: data,
            success: function (data) {
                $('#divisionAdd').modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success("Data Successfully Added!");
                    // hide please wait modal
                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            error: function (data) {
                toastr.error("error");
            }
        });
    });

    //Get edit
    $("#divisionList").on('click', '.edit', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/div/getdivisions/' + id;
        console.log(id);
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $("#editmodal").modal('show');
                $('#edit').find('input[name="id"]').val(data.id);
                $('#edit').find('input[name="name"]').val(data.name);
                $('#edit').find('select[name="departmentsId"]').val(data.departmentsId);
            },
            //if failed
            error: function (data) {
                // console.log(data, data.id, data.name);
                toastr.error("Error")
            }
        })
    })

    $("#updatediv").click(function (e) {
        e.preventDefault();
        // find isActive vaue

        // find select department
        var departmentsId = $('#edit').find('select[name=departmentsId]').val();
        // find id
        var id = $('#edit').find('input[name="id"]').val();
        var data = {
            name: $('#edit').find('input[name=name]').val(),
            departmentsId: departmentsId,
        };
        $.ajax({
            type: 'PUT',
            url: '/ServiceRequest/api/division/updateDiv/' + id,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                $('#editmodal').modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success("Division Successfully Updated!");
                    // hide please wait modal
                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            //if failed
            error: function (data) {

                toastr.error("Error")
            }
        });
    });

    $('#divisionList').on('click', '.delete', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/division/Delete/' + id;
        $.ajax({  //make ajax request to delete
            // confirm before delete
            beforeSend: function (xhr) {
                // use bootbox
                bootbox.confirm({
                    message: "Are you sure you want to delete this record?",
                    buttons: {
                        // yes and no sequence

                        confirm: {
                            label: 'Yes',
                            className: 'btn-success btn-sm'
                        },
                        cancel: {
                            label: 'No',
                            className: 'btn-danger btn-sm'
                        }
                    },
                    callback: function (result) {
                        if (result) {
                            $.ajax({
                                type: 'DELETE',
                                url: url,
                                headers: {
                                    'Authorization': 'Bearer ' + localStorage.getItem('access_token')
                                },
                                success: function (data) {
                                    //show please wait modal
                                    $('#pleasewait').modal('show');
                                    //show toastr after 3
                                    setTimeout(function () {
                                        toastr.success("Data Successfully Deleted!");
                                        // hide please wait modal
                                    }, 2000);
                                    setTimeout(function () {
                                        window.location.reload();
                                    }, 3000);
                                },
                                //if failed
                                error: function (data) {
                                    toastr.error("Error")
                                }
                            });
                        }
                    }
                });

            },
        });
    });
}
//HardwareTEchnician
function HardwareTechnician() {
    var Datatables;
    Datatables = $("#hardwareList").DataTable({
        ajax: {
            url: "/ServiceRequest/api/htech/GetTech",
            dataSrc: "",
            headers: {
                "Authorization": "Bearer " + localStorage.getItem('access_token')
            }
        },
        autoWidth: false,
        columns: [
            {
                data: "dateAdded",
            },
            {
                data: "name",
            },
            {
                data: "email",
            }

        ]
    });

    $('#btnSubmit').click(function (e) {
        e.preventDefault();
        //find name
        var name = $('input[name=name]').val();
        // data
        var data = {
            name: name,
        };
        $.ajax({
            type: 'POST',
            url: '/ServiceRequest/api/htech/save',
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                //toastr.success("Directory Successfully Added!");
                //Datatables.ajax.reload();
                //$("#hardwareTechModal").modal('hide');

                $("#name").val("");
                $('#hardwareTechModal').modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success("Technician Successfully Added!");
                    // hide please wait modal
                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            //if failed
            error: function (data) {
                toastr.error("Name Already Exist In the Database/ Invalid")
            }
        });
    });

    //Get edit
    $("#hardwareList").on('click', '.edit', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/htech/getTechbyId/' + id;
        // alert(id);
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $("#editmodal").modal('show');
                $('#edit').find('input[name="id"]').val(data.id);
                $('#edit').find('input[name="name"]').val(data.name);
            },
            //if failed
            error: function (data) {
                // console.log(data, data.id, data.name);
                toastr.error("Error")
            }
        })
    })


    $("#Updatetech").click(function (e) {
        e.preventDefault();
        var data = {
            name: $('#edit').find('input[name=name]').val(),
        };
        var id = $('#edit').find('input[name="id"]').val();
        $.ajax({
            type: 'PUT',
            url: '/ServiceRequest/api/htech/updateHardwareTech/' + id,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {

                $('#editmodal').modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success("Technician Successfully Updated!");
                    // hide please wait modal
                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);

            },
            //if failed
            error: function (data) {
                toastr.error("Error Updating")
            }
        });
    });

    $('#hardwareList').on('click', '.delete', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/htech/Delete/' + id;
        $.ajax({
            // confirm before delete
            beforeSend: function (xhr) {
                // use bootbox
                bootbox.confirm({
                    message: "Are you sure you want to delete this record?",
                    buttons: {
                        // yes and no sequence

                        confirm: {
                            label: 'Yes',
                            className: 'btn-success btn-sm'
                        },
                        cancel: {
                            label: 'No',
                            className: 'btn-danger btn-sm'
                        }
                    },
                    callback: function (result) {
                        if (result) {
                            $.ajax({
                                type: 'DELETE',
                                url: url,
                                headers: {
                                    'Authorization': 'Bearer ' + localStorage.getItem('access_token')
                                },
                                success: function (data) {

                                    //show please wait modal
                                    $('#pleasewait').modal('show');
                                    //show toastr after 3
                                    setTimeout(function () {
                                        toastr.success("Data Successfully Deleted!");
                                        // hide please wait modal
                                    }, 2000);
                                    setTimeout(function () {
                                        window.location.reload();
                                    }, 3000);
                                },
                                //if failed
                                error: function (data) {
                                    toastr.error("Error")
                                }
                            });
                        }
                    }
                });

            },
        });
    });
}

function LoginActivity() {
    var table2 = $("#activity").DataTable({
        "ajax": {
            "url": "/ServiceRequest/api/all/activity",
            "headers": {
                "Authorization": "Bearer " + localStorage.getItem('access_token')
            },
            "type": "GET",
            "dataType": "json",
        },
        "processing": "true",
        "serverSide": "true",
        "ordering": "true",
        "paging": "true",
        "fnInitComplete": function (oSettings, json) {
            // search after pressing enter
            $('#activity input').unbind();
            $('#activity input').bind('keyup', function (e) {
                if (e.keyCode == 13) {
                    // trigger search 
                    $('#pleasewait').modal('show');
                    setTimeout(function () {
                        $('#pleasewait').modal('hide');
                    }, 2500);
                    $('#activity').DataTable().search(this.value).draw();
                }
                // if search is empty, reset the filter
                if (this.value == "") {
                    $('#activity').DataTable().search("").draw();
                }
            }
            );
        },
        // "searching" : " true",
        // "searchDelay" : 1000,
        // order by desc dateuploaded
        "order": [[0, "desc"]],
        "language": {
            "processing": "Loading... Please wait"

        },
        "autoWidth": false,
        "columns": [
            {
                data: 'userName'
            },
            {
                data: 'activityMessage'
            },
            {
                data: "activityDate",
            }

        ],
    });

}

function RequestActivity() {
    var table2 = $("#requestActivity").DataTable({
        "ajax": {
            "url": "/ServiceRequest/api/all/Request",
            "headers": {
                "Authorization": "Bearer " + localStorage.getItem('access_token')
            },
            "type": "GET",
            "dataType": "json",
        },
        "processing": "true",
        "serverSide": "true",
        "ordering": "true",
        "paging": "true",
        "fnInitComplete": function (oSettings, json) {
            // search after pressing enter
            $('#requestActivity_filter input').unbind();
            $('#requestActivity_filter input').bind('keyup', function (e) {
                if (e.keyCode == 13) {
                    // trigger search 
                    $('#pleasewait').modal('show');
                    setTimeout(function () {
                        $('#pleasewait').modal('hide');
                    }, 2500);
                    $('#requestActivity').DataTable().search(this.value).draw();
                }
                // if search is empty, reset the filter
                if (this.value == "") {
                    $('#requestActivity').DataTable().search("").draw();
                }
            }
            );
        },
        // "searching" : " true",
        // "searchDelay" : 1000,
        // order by desc dateuploaded
        "order": [[0, "desc"]],
        "language": {
            "processing": "Loading... Please wait"

        },
        "autoWidth": false,
        "columns": [

            {
                "data": "userName",
            },
            {
                "data": "departmentsId",
                render: function (data, type, row) {
                    return row.departments.name
                }
            },
            {
                "data": "divisionsId",
                "render": function (data, type, row) {
                    return row.divisions.name;
                }
            },
            {
                "data": "uploadMessage",
            },
            {
                "data": "uploadDate",
            }

        ],
    });

}


function SoftwareActivity() {
    $('#SoftwareActivities').DataTable({
        ajax: {
            url: '/ServiceRequest/api/v2/SoftwareActivity/get',
            dataSrc: "",
            headers: {
                "Authorization": "Bearer " + localStorage.getItem('access_token')
            }
        },
        autoWidth: false,
        columns: [
            {
                // display count
                data: null,
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            {
                data: 'userName'
            },
            {
                "data": "departmentsId",
                render: function (data, type, row) {
                    return row.departments.name;
                }
            },
            {
                "data": "divisionsId",
                render: function (data, type, row) {
                    return row.divisions.name;
                }
            },
            {
                data: 'requetMessage'
            },
            {
                data: "requetDate",


            }
        ]
    });

}
function HardwareActivity() {
    $('#HardwareActivities').DataTable({
        ajax: {
            url: '/ServiceRequest/api/v2/HardwareActivity/get',
            dataSrc: "",
            headers: {
                "Authorization": "Bearer " + localStorage.getItem('access_token')
            }
        },
        autoWidth: false,
        columns: [
            {
                // display count
                data: null,
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            {
                data: 'userName'
            },
            {
                "data": "departmentsId",
                render: function (data, type, row) {
                    return row.departments.name;
                }
            },
            {
                "data": "divisionsId",
                render: function (data, type, row) {
                    return row.divisions.name;
                }
            },
            {
                data: 'requetMessage'
            },
            {
                data: "requetDate",

            }
        ]
    });

}

function DashBoard() {
    $.ajax({
        type: 'GET',
        url: 'api/acitivi/get',
        headers: {
            "Authorization": "Bearer " + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('#activityLogs tbody').html('');
            $.each(data, function (index, value) {
                $('#activityLogs tbody').append(
                    '<tr>' +
                    '<td>' + '<span style="font-size: 15px;">' + value.userName + '</span>' + '</td>' +
                    '<td>' + '<span style="font-size: 15px;">' + value.activityMessage + '</span>' + '</td>' +
                    '<td>' + '<span style="font-size: 15px;">' + value.activityDate + '</span>' + '</td>' + '</tr>'
                );
            });
        },

        //if failed
        error: function (data) {

            // toastr.info("Success")
        }

    })
    $.ajax({
        type: 'GET',
        url: 'api/SoftwareActivity/get',
        headers: {
            "Authorization": "Bearer " + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('#softwareActivity tbody').html('');
            $.each(data, function (index, value) {
                $('#softwareActivity tbody').append(
                    '<tr>' +
                    '<td>' + '<span style="font-size: 15px;">' + value.userName + '</span>' + '</td>' +
                    '<td>' + '<span style="font-size: 15px;">' + value.departments.name + '</span>' + '</td>' +
                    '<td>' + '<span style="font-size: 15px;">' + value.divisions.name + '</span>' + '</td>' +

                    '<td>' + '<span style="font-size: 15px;">' + value.requetMessage + '</span>' + '</td>' +
                    '<td>' + '<span style="font-size: 15px;">' + value.requetDate + '</span>' + '</td>' + '</tr>'
                );
            });
        },

        //if failed
        error: function (data) {

            // toastr.info("Success")
        }
    });

    $.ajax({
        type: 'GET',
        url: 'api/v2/sr/assign',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('#softwareProgrammer tbody').html('');
            $.each(data, function (index, value) {
                $('#softwareProgrammer tbody').append(
                    '<tr>' +
                    '<td>' + '<span class="badge bg-info">' + value.ticket + '</span>' + '</td>' +
                    '<td>' + value.dateAdded + '</td>' +
                    '<td>' + value.fullName + '</td>' +
                    '<td>' + value.departments.name + '</td>' +
                    '<td>' + value.divisions.name + '</td>' +
                    '<td>' + value.software.name + '</td>' +

                    '<td>' + (value.statusId == 1 ? '<span class="badge bg-secondary" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">Open</span>' : (value.statusId == 2 ? '<span class="badge bg-success" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">Resolved</span>' : (value.statusId == 3 ? '<span class="badge bg-warning" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">On hold</span>' : (value.statusId == 4 ? '<span class="badge bg-info" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">In Progress</span>' : (value.statusId == 5 ? '<span class="badge bg-danger" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">Cancel Request</span>' : '<span class="badge bg-secondary" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">Open</span>'))))) + '</td>' + '</tr>'
                );
            });
        },

        //if failed
        error: function (data) {

            // toastr.info("Success")
        }

    });

    $.ajax({
        type: 'GET',
        url: 'api/v2/hr/assign',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('#hardwareProgrammer tbody').html('');
            $.each(data, function (index, value) {
                $('#hardwareProgrammer tbody').append(
                    '<tr>' +
                    '<td>' + '<span class="badge bg-info">' + value.ticket + '</span>' + '</td>' +
                    '<td>' + value.dateAdded + '</td>' +
                    '<td>' + value.fullName + '</td>' +
                    '<td>' + value.departments.name + '</td>' +
                    '<td>' + value.divisions.name + '</td>' +
                    '<td>' + value.hardware.name + '</td>' +

                    '<td>' + (value.statusId == 1 ? '<span class="badge bg-secondary" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">Open</span>' : (value.statusId == 2 ? '<span class="badge bg-success" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">Resolved</span>' : (value.statusId == 3 ? '<span class="badge bg-warning" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">On hold</span>' : (value.statusId == 4 ? '<span class="badge bg-info" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">In Progress</span>' : (value.statusId == 5 ? '<span class="badge bg-danger" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">Cancel Request</span>' : '<span class="badge bg-secondary" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">Open</span>'))))) + '</td>' + '</tr>'
                );
            });
        },

        //if failed
        error: function (data) {

            // toastr.info("Success")
        }

    });

    $.ajax({
        type: 'GET',
        url: 'api/softwareRequest/getSoftware',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('#softwareProgrammerSP tbody').html('');
            $.each(data, function (index, value) {
                $('#softwareProgrammerSP tbody').append(
                    '<tr>' +
                    '<td>' + '<span class="badge bg-info">' + value.ticket + '</span>' + '</td>' +
                    '<td>' + value.dateAdded + '</td>' +
                    '<td>' + value.fullName + '</td>' +
                    '<td>' + value.departments.name + '</td>' +
                    '<td>' + value.divisions.name + '</td>' +
                    '<td>' + value.software.name + '</td>' +

                    '<td>' + (value.statusId == 1 ? '<span class="badge bg-secondary" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">Open</span>' : (value.statusId == 2 ? '<span class="badge bg-success" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">Resolved</span>' : (value.statusId == 3 ? '<span class="badge bg-warning" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">On hold</span>' : (value.statusId == 4 ? '<span class="badge bg-info" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">In Progress</span>' : (value.statusId == 5 ? '<span class="badge bg-danger" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">Cancel Request</span>' : '<span class="badge bg-secondary" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">Open</span>'))))) + '</td>' + '</tr>'
                );
            });
        },

        //if failed
        error: function (data) {

            // toastr.info("Success")
        }

    });

    $.ajax({
        type: 'GET',
        url: 'api/hardware/gethardware',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('#hardwareProgrammerSP tbody').html('');
            $.each(data, function (index, value) {
                $('#hardwareProgrammerSP tbody').append(
                    '<tr>' +
                    '<td>' + '<span class="badge bg-info">' + value.ticket + '</span>' + '</td>' +
                    '<td>' + value.dateAdded + '</td>' +
                    '<td>' + value.fullName + '</td>' +
                    '<td>' + value.departments.name + '</td>' +
                    '<td>' + value.divisions.name + '</td>' +
                    '<td>' + value.hardware.name + '</td>' +

                    '<td>' + (value.statusId == 1 ? '<span class="badge bg-secondary" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">Open</span>' : (value.statusId == 2 ? '<span class="badge bg-success" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">Resolved</span>' : (value.statusId == 3 ? '<span class="badge bg-warning" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">On hold</span>' : (value.statusId == 4 ? '<span class="badge bg-info" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">In Progress</span>' : (value.statusId == 5 ? '<span class="badge bg-danger" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">Cancel Request</span>' : '<span class="badge bg-secondary" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">Open</span>'))))) + '</td>' + '</tr>'
                );
            });
        },

        //if failed
        error: function (data) {

            // toastr.info("Success")
        }

    });

    $.ajax({
        type: 'GET',
        url: 'api/SoftwareActivity/get',
        headers: {
            "Authorization": "Bearer " + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('#softwareActivityAdmin tbody').html('');
            $.each(data, function (index, value) {
                $('#softwareActivityAdmin tbody').append(
                    '<tr>' +
                    '<td>' + '<span style="font-size: 15px;">' + value.userName + '</span>' + '</td>' +
                    '<td>' + '<span style="font-size: 15px;">' + value.departments.name + '</span>' + '</td>' +
                    '<td>' + '<span style="font-size: 15px;">' + value.divisions.name + '</span>' + '</td>' +

                    '<td>' + '<span style="font-size: 15px;">' + value.requetMessage + '</span>' + '</td>' +
                    '<td>' + '<span style="font-size: 15px;">' + value.requetDate + '</span>' + '</td>' + '</tr>'
                );
            });
        },

        //if failed
        error: function (data) {

            // toastr.info("Success")
        }

    });

    $.ajax({
        type: 'GET',
        url: 'api/HardwareActivity/get',
        headers: {
            "Authorization": "Bearer " + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('#HardwareActivity tbody').html('');
            $.each(data, function (index, value) {
                $('#HardwareActivity tbody').append(
                    '<tr>' +
                    '<td>' + '<span style="font-size: 15px;">' + value.userName + '</span>' + '</td>' +
                    '<td>' + '<span style="font-size: 15px;">' + value.departments.name + '</span>' + '</td>' +
                    '<td>' + '<span style="font-size: 15px;">' + value.divisions.name + '</span>' + '</td>' +
                    '<td>' + '<span style="font-size: 15px;">' + value.requetMessage + '</span>' + '</td>' +
                    '<td>' + '<span style="font-size: 15px;">' + value.requetDate + '</span>' + '</td>' + '</tr>'
                );
            });
        },

        //if failed
        error: function (data) {

            // toastr.info("Success")
        }

    });


    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/DashRequest/get',
        headers: {
            "Authorization": "Bearer " + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('#requestActivity tbody').html('');
            $.each(data, function (index, value) {
                $('#requestActivity tbody').append(
                    '<tr>' +
                    '<td>' + '<span style="font-size: 15px;">' + value.userName + '</span>' + '</td>' +
                    '<td>' + '<span style="font-size: 15px;">' + value.departments.name + '</span>' + '</td>' +
                    '<td>' + '<span style="font-size: 15px;">' + value.divisions.name + '</span>' + '</td>' +
                    '<td>' + '<span style="font-size: 15px;">' + value.uploadMessage + '</span>' + '</td>' +
                    '<td>' + '<span style="font-size: 15px;">' + value.uploadDate + '</span>' + '</td>' + '</tr>'
                );
            });
        },

        //if failed
        error: function (data) {

            // toastr.info("Success")
        }

    });



    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/sftStatus/count',
        headers: {
            "Authorization": "Bearer " + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('#statusListcount tbody').html('');
            $.each(data, function (index, value) {
                $('#statusListcount tbody').append(
                    '<tr>' +
                    '<td>' + '<span style="font-size: 15px;">' + value.name + '</span>' + '</td>' +
                    '<td>' + '<span style="font-size: 15px;">' + value.count + '</span>' + '</td>' + '</tr>'
                );
            });
        },

        //if failed
        error: function (data) {

            // toastr.info("Success")
        }
    });

    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hrdStatus/count',
        headers: {
            "Authorization": "Bearer " + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('#statusListcount2 tbody').html('');
            $.each(data, function (index, value) {
                $('#statusListcount2 tbody').append(
                    '<tr>' +
                    '<td>' + '<span style="font-size: 15px;">' + value.name + '</span>' + '</td>' +
                    '<td>' + '<span style="font-size: 15px;">' + value.count + '</span>' + '</td>' + '</tr>'
                );
            });
        },

        //if failed
        error: function (data) {

            // toastr.info("Success")
        }
    });

    $.ajax({
        type: 'GET',
        url: 'api/software/requestbyUserList',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('#UsersoftwareRequest tbody').html('');
            $.each(data, function (index, value) {
                $('#UsersoftwareRequest tbody').append(
                    '<tr>' +
                    '<td>' + '<span class="badge bg-info">' + value.ticket + '</span>' + '</td>' +
                    '<td>' + value.dateAdded + '</td>' +
                    '<td>' + value.software.name + '</td>' +
                    '<td>' + (value.statusId == 1 ? '<span class="badge bg-secondary" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">Open</span>' : (value.statusId == 2 ? '<span class="badge bg-success" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">Resolved</span>' : (value.statusId == 3 ? '<span class="badge bg-warning" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">On hold</span>' : (value.statusId == 4 ? '<span class="badge bg-info" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">In Progress</span>' : (value.statusId == 5 ? '<span class="badge bg-danger" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">Cancel Request</span>' : '<span class="badge bg-secondary" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">Open</span>'))))) + '</td>' + '</tr>'
                );
            });
        },

        //if failed
        error: function (data) {

            // toastr.info("Success")
        }

    });


    $.ajax({
        type: 'GET',
        url: 'api/hardware/requestbyUserList',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('#UserhardwareRequest tbody').html('');
            $.each(data, function (index, value) {
                $('#UserhardwareRequest tbody').append(
                    '<tr>' +
                    // activityDate momentjs
                    '<td>' + '<span class="badge bg-info">' + value.ticket + '</span>' + '</td>' +
                    '<td>' + moment(value.dateAdded).format('M/D/Y LT') + '</td>' +
                    '<td>' + value.hardware.name + '</td>' +
                    '<td>' + (value.statusId == 1 ? '<span class="badge bg-secondary" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">Open</span>' : (value.statusId == 2 ? '<span class="badge bg-success" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">Resolved</span>' : (value.statusId == 3 ? '<span class="badge bg-warning" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">On hold</span>' : (value.statusId == 4 ? '<span class="badge bg-info" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">In Progress</span>' : (value.statusId == 5 ? '<span class="badge bg-danger" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">Cancel Request</span>' : '<span class="badge bg-secondary" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">Open</span>'))))) + '</td>' + '</tr>'
                );
            });
        },

        //if failed
        error: function (data) {

            // toastr.info("Success")
        }

    });

}


//Query of Systems Cgpp
function ExistingSystem() {
    var Datatables;
    Datatables = $("#systemList").DataTable({
        ajax: {
            url: "/ServiceRequest/api/information/getSystem",
            dataSrc: "",
            headers: {
                "Authorization": "Bearer " + localStorage.getItem('access_token')
            }
        },
        autoWidth: false,
        columns: [

            {
                data: "dateAdded"
            },
            {
                data: "name",
            },
            {
                data: null,
                render: function (data) {
                    return '<button class="btn btn-success btn-sm edit" data-id="' + data.id + '"><i class="fas fa-edit"></i> Edit</button>'
                        + '  <button class="btn btn-danger btn-sm delete" data-id="' + data.id + '"><i class="fas fa-trash"></i> Delete</button>'
                },
                "orderable": false,
            }

        ]
    });

    $('#btnSubmit').click(function (e) {
        e.preventDefault();

        //find name
        var name = $('input[name=name]').val();
        // data
        var data = {
            name: name,
        };
        $.ajax({
            type: 'POST',
            url: '/ServiceRequest/api/information/saveSystem',
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                $("#name").val("");
                //hide modal
                $('#SystemModal').modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success("Successfully Added New System!");
                    // hide please wait modal

                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            //if failed
            error: function (data) {
                toastr.error("Name Already Exist In the Database/ Invalid")
            }
        });
    });

    //Get edit
    $("#systemList").on('click', '.edit', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/information/getSystembyId/' + id;
        //alert(id);
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $("#editmodal").modal('show');
                $('#edit').find('input[name="id"]').val(data.id);
                $('#edit').find('input[name="name"]').val(data.name);
            },
            //if failed
            error: function (data) {
                // console.log(data, data.id, data.name);
                toastr.error("Error")
            }
        })
    });


    $("#UpdateData").click(function (e) {
        e.preventDefault();
        var data = {
            name: $('#edit').find('input[name=name]').val(),
        };
        var id = $('#edit').find('input[name="id"]').val();
        $.ajax({
            type: 'PUT',
            url: '/ServiceRequest/api/information/updateSystem//' + id,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                $('#editmodal').modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success("Data Successfully Updated!");
                    // hide please wait modal
                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);

            },
            //if failed
            error: function (data) {
                toastr.error("Error Updating")
            }
        });
    });



    $('#systemList').on('click', '.delete', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/information/Delete/' + id;
        $.ajax({
            // confirm before delete
            beforeSend: function (xhr) {
                // use bootbox
                bootbox.confirm({
                    message: "Are you sure you want to delete this record?",
                    buttons: {
                        // yes and no sequence

                        confirm: {
                            label: 'Yes',
                            className: 'btn-success btn-sm'
                        },
                        cancel: {
                            label: 'No',
                            className: 'btn-danger btn-sm'
                        }
                    },
                    callback: function (result) {
                        if (result) {
                            $.ajax({
                                type: 'DELETE',
                                url: url,
                                headers: {
                                    'Authorization': 'Bearer ' + localStorage.getItem('access_token')
                                },
                                success: function (data) {
                                    //toastr.success("Data Successfully Deleted!");
                                    //Datatables.ajax.reload();  $('#editmodal').modal('hide');
                                    //show please wait modal
                                    $('#pleasewait').modal('show');
                                    //show toastr after 3
                                    setTimeout(function () {
                                        toastr.success("Data Successfully Updated!");
                                        // hide please wait modal
                                    }, 2000);
                                    setTimeout(function () {
                                        window.location.reload();
                                    }, 3000);
                                },
                                //if failed
                                error: function (data) {
                                    toastr.error("Error")
                                }
                            });
                        }
                    }
                });

            },
        });
    });
}

function ManageUsers() {
    $('#UsersList').DataTable();
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/v1/users/GetDepartments',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            var html = '<option value="">Select Department</option>';
            $.each(data, function (i, item) {
                html += '<option value="' + item.id + '">' + item.name + '</option>';
            });
            $('select[name=departmentsId2]').html(html);
            // render divisionsId2 select
            // console log on select change
            $('select[name=departmentsId2]').on('change', function () {
                var depId = $('select[name=departmentsId2]').val();
                console.log(depId);
                $.ajax({
                    type: 'GET',
                    url: '/ServiceRequest/api/div/fetchbyid/' + depId,
                    headers: {
                        'Authorization': 'Bearer ' + localStorage.getItem('access_token')
                    },
                    success: function (data) {
                        var html = '<option value="">Select Division</option>';
                        $.each(data, function (i, item) {
                            html += '<option value="' + item.id + '">' + item.name + '</option>';
                        });
                        console.log(data);
                        $('select[name=divisionsId2]').html(html);
                    }
                });
            });
        }
    });
    //Department
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/User/department',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=departmentsId]').append('<option value=""> Select Department</option>');
            $.each(data, function (index, value) {
                $('select[name=departmentsId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },
    });
    //Division
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/User/division',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=divisionsId]').append('<option value=""> Select Division</option>');
            $.each(data, function (index, value) {
                $('select[name=divisionsId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },
    });
    $('#UsersList').on('click', '.edit-details', function () {
        var id = $(this).attr('id');
        var url = '/ServiceRequest/api/v1/users/getid/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                $('#UserDetailsModal').modal('show');
                $('#userdetails').find('input[name="id"]').val(data.id);
                $('#userdetails').find('input[name="fullName"]').val(data.fullName);
                $('#userdetails').find('input[name="mobileNumber"]').val(data.mobileNumber);
                $('#userdetails').find('input[name="email"]').val(data.email);
                //$('#userdetails').find('select[name="departmentsId"]').val(data.departmentsId);
                //$('#userdetails').find('select[name="divisionsId"]').val(data.divisionsId);
                $('#userdetails').find('select[name=departmentsId]').val(data.departmentsId);
                if (data.departmentsId == null || data.departmentsId == "") {
                    // render option not applicable
                    $('select[name=departmentsId]').append('<option value="">Not Applicable</option>');
                }
                // find division
                $('#userdetails').find('select[name=divisionsId]').val(data.divisionsId);
                if (data.divisionsId == null || data.divisionsId == "") {
                    // render option not applicable
                    $('select[name=divisionsId]').append('<option value="">Not Applicable</option>');
                }
                // alert(data.id);
            }
        });
    });

    //save technician
    $('#UserDetailsModal').submit(function (e) {
        e.preventDefault();
        var fullName = $('#userdetails').find('input[name="fullName"]').val();
        var email = $('#userdetails').find('input[name="email"]').val();
        var mobileNumber = $('#userdetails').find('input[name="mobileNumber"]').val();
        var depId = $('#userdetails').find('select[name="departmentsId"]').val();
        var divId = $('#userdetails').find('select[name="divisionsId"]').val();
        var id = $('#userdetails').find('input[name="id"]').val();
        var data = {
            "id": id,
            "fullName": fullName,
            "email": email,
            "mobileNumber": mobileNumber,
            "departmentsId": depId,
            "divisionsId": divId
        };

        $.ajax({
            type: 'POST',
            url: '/ServiceRequest/api/Account/UpdateName/' + id,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                //hide modal
                $('#UserDetailsModal').modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success("Details Changed");
                    // hide please wait modal

                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            // ignore error
            error: function (data) {

            }
        })
    });

    $('#UsersList').on('click', '.reset-password', function () {
        var id = $(this).attr('id');
        var url = '/ServiceRequest/api/v1/users/getid/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                $('#resetPassord').modal('show');
                $('#reset').find('input[name="id"]').val(data.id);
                $('#reset').find('input[name="newpassword"]').val(data.newpassword);
                $('#reset').find('input[name="changepassword"]').val(data.changepassword);

                // alert(data.id);
            }
        });
    });


    $("#reset").submit(function (e) {
        e.preventDefault();
        // find new pass
        var newpassword = $('#reset').find('input[name="newpassword"]').val();

        // find id
        var id = $('#reset').find('input[name="id"]').val();
        // find confirm pass
        var confirmpassword = $('#reset').find('input[name="confirmpassword"]').val();
        var data = {
            newpassword: newpassword,
            confirmpassword: confirmpassword,
            id: id
        };
        $.ajax({
            type: 'POST',
            url: '/ServiceRequest/api/Account/ChangePassword/' + id,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {

                //hide modal
                $('#resetPassord').modal('hide');
                // show please wait modal
                $('#pleasewait').modal('show');
                // show toastr after 3
                setTimeout(function () {
                    toastr.success("Password Changed");
                    // hide please wait modal

                }, 3000),
                    setTimeout(function () {
                        window.location.reload();
                    }, 6000);
            },
            //if failed
            error: function (data) {

                toastr.error("Error")
            }
        });
    });

    $('#UsersList').on('click', '.delete-account', function () {
        var id = $(this).attr('id');
        //console.log(id);
        // bootbox
        bootbox.confirm({
            message: "Are you sure you want to delete this file?",
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-success'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-danger'
                }
            },
            callback: function (result) {
                if (result) {
                    $.ajax({
                        url: '/ServiceRequest/api/Account/DeleteUser/' + id,
                        type: 'DELETE',
                        headers: {
                            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
                        },
                        success: function (data) {
                            toastr.success('File Deleted Successfully');
                            setTimeout(() => {
                                location.reload();
                            }, 1000);
                        },
                        error: function (data) {
                            toastr.error('Delete Failed');
                        }
                    });
                }
            }

        });
    });

    $('#addUser').submit(function (e) {
        e.preventDefault();
        var depId = $('select[name="departmentsId2"]').val();
        var divId = $('select[name="divisionsId2"]').val();
        var formData = $(this).serialize() + '&departmentsId=' + depId + '&divisionsId=' + divId;
        $.ajax({
            url: '/ServiceRequest/api/Account/Register',
            type: 'POST',
            data: formData,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                $('#createModal').modal('hide');
                $('#pleasewait').modal('show');

                setTimeout(function () {
                    toastr.success("Account Created Successfully");
                    // hide please wait modal
                }, 2000),
                    setTimeout(function () {
                        location.reload();
                    }, 3000);
            },
            error: function (data) {
                toastr.error('Register Failed');
            }
        });
    });
}

function UserProfile() {
    //Department
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/User/department',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=departmentsId]').append('<option value=""> Select Department</option>');
            $.each(data, function (index, value) {
                $('select[name=departmentsId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });

    //Division
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/User/division',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=divisionsId]').append('<option value=""> Select Division</option>');
            $.each(data, function (index, value) {
                $('select[name=divisionsId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });

    $('#userProfile').on('click', '.update-details', function () {
        var id = $(this).attr('id');
        var url = '/ServiceRequest/api/v1/users/getid/' + id;

        $.ajax({
            type: 'GET',
            url: url,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                $('#updateName').find('input[name="id"]').val(data.id);
                $('#updateName').find('input[name="fullName"]').val(data.fullName);
                $('#updateName').find('input[name="email"]').val(data.email);
                $('#updateName').find('input[name="mobileNumber"]').val(data.mobileNumber);
                $('#updateName').find('select[name="departmentsId"]').val(data.departmentsId);
                $('#updateName').find('select[name="divisionsId"]').val(data.divisionsId);
            },
            error: function (data) {
                toastr.error('error')
            }
        })

    });

    //save technician
    $('#changeName').click(function (e) {
        e.preventDefault();
        var fullName = $('#updateName').find('input[name="fullName"]').val();
        var email = $('#updateName').find('input[name="email"]').val();
        var mobileNumber = $('#updateName').find('input[name="mobileNumber"]').val();
        var depId = $('#updateName').find('select[name="departmentsId"]').val();
        var divId = $('#updateName').find('select[name="divisionsId"]').val();
        var id = $('#updateName').find('input[name="id"]').val();
        var data = {
            "id": id,
            "fullName": fullName,
            "email": email,
            "mobileNumber": mobileNumber,
            "departmentsId": depId,
            "divisionsId": divId
        };

        $.ajax({
            type: 'POST',
            url: '/ServiceRequest/api/Account/UpdateName/' + id,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                /*how please wait modal*/
                $('#loadingModal').modal('show');
                /*show toastr after 3*/
                setTimeout(function () {
                    toastr.success("Details Changed");
                    /*hide please wait modal*/

                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            // ignore error
            error: function (data) {

            }
        })
    });

    $('#userProfile').on('click', '.update-details', function () {
        var id = $(this).attr('id');
        var url = '/ServiceRequest/api/v1/users/getid/' + id;

        $.ajax({
            type: 'GET',
            url: url,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                $('#chageDetails').find('input[name="id"]').val(data.id);
                $('#chageDetails').find('input[name="fullName"]').val(data.fullName);
                $('#chageDetails').find('input[name="mobileNumber"]').val(data.mobileNumber);
            },
            error: function (data) {
                toastr.error('error')
            }
        })

    });

    $('#updateDetails').click(function (e) {
        e.preventDefault();
        var fullName = $('#chageDetails').find('input[name="fullName"]').val();
        var mobileNumber = $('#chageDetails').find('input[name="mobileNumber"]').val();
        var id = $('#chageDetails').find('input[name="id"]').val();
        var data = {
            "id": id,
            "fullName": fullName,
            "mobileNumber": mobileNumber,
        };
        $.ajax({
            type: 'POST',
            url: '/ServiceRequest/api/Account/changeNmae/' + id,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                //hide modal
                // $('#updateDetails').modal('hide');
                // show please wait modal
                $('#loadingModal').modal('show');
                // show toastr after 3
                setTimeout(function () {
                    toastr.success("Details Changed");
                    // hide please wait modal

                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            // ignore error
            error: function (data) {

            }
        })
    });


    $('#userProfile').on('click', '.update-image', function () {
        var id = $(this).attr('id');
        var url = '/ServiceRequest/api/v1/users/getid/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                $('#changeImageForm').find('input[name="id"]').val(data.id);
                // alert(data.id);
            },
            error: function (data) {
                toastr.error('error')
            }
        })

    });

    $("#changeImageForm").submit(function (e) {
        e.preventDefault();
        var formData = new FormData(this);
        $.ajax({
            type: 'POST',
            url: '/ServiceRequest/api/Account/ChangeProfileImage',
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (data) {
                /*alert(data);*/
                $('#loadingModal').modal('show');
                // show toastr after 3
                setTimeout(function () {
                    toastr.success("Profile Image Changed Successfully");
                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            //if failed
            error: function (data) {
                toastr.error("Error Saving")
            }
        });
    });

    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/users/GetUserFullName',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {

            $('#user-name').text(data);
        },
        //if failed
        error: function (data) {
        }
    });


    $("#userProfile").on('click', '.change-pass', function () {
        // get id on edit button
        var id = $(this).attr('id');
        var url = '/ServiceRequest/api/v1/users/getid/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                $('#changePasswordForm').find('input[name="id"]').val(data.id);
                $('#changePasswordForm').find('input[name="newpassword"]').val(data.newpassword);
                $('#changePasswordForm').find('input[name="changepassword"]').val(data.changepassword);
                //alert(data.id);
            },
            //if failed
            error: function (data) {
                toastr.error("Error")
            }
        });
    });


    $("#changePasswordForm").submit(function (e) {
        e.preventDefault();
        // find new pass
        var newpassword = $('#changePasswordForm').find('input[name="newpassword"]').val();
        // find id
        var id = $('#changePasswordForm').find('input[name="id"]').val();
        // find confirm pass
        var confirmpassword = $('#changePasswordForm').find('input[name="confirmpassword"]').val();
        var data = {
            newpassword: newpassword,
            confirmpassword: confirmpassword,
            id: id
        };
        $.ajax({
            type: 'POST',
            url: '/ServiceRequest/api/Account/ChangePassword/' + id,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                $('#loadingModal').modal('show');
                // show toastr after 3
                setTimeout(function () {
                    toastr.success("Password Changed");
                    // hide please wait modal
                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 5000);
            },
            //if failed
            error: function (data) {

                toastr.error("Error")
            }
        });
    });
}

function SoftwareTechnician() {
    var Datatables;
    Datatables = $("#softwaretechList").DataTable({
        ajax: {
            url: "/ServiceRequest/api/stech/Gettech",
            dataSrc: "",
            headers: {
                "Authorization": "Bearer " + localStorage.getItem('access_token')
            }
        },
        autoWidth: false,
        columns: [

            {
                data: "dateAdded",
            },
            {
                data: "name",
            },
            {
                data: "techEmail"
            },
            {
                data: null,
                render: function (data) {
                    return '<button class="btn btn-success btn-sm edit" data-id="' + data.id + '"><i class="fas fa-edit"></i> Edit</button>'
                        + '  <button class="btn btn-danger btn-sm delete" data-id="' + data.id + '"><i class="fas fa-trash"></i> Delete</button>'
                },
                "orderable": false,
            }

        ]
    });

    $('#btnSubmit').click(function (e) {
        e.preventDefault();

        //find name
        var name = $('#save').find('input[name=name]').val();
        var techEmail = $('#save').find('input[name=techEmail]').val();
        // data
        var data = {
            name: name,
            techEmail, techEmail
        };
        $.ajax({
            type: 'POST',
            url: '/ServiceRequest/api/stech/save',
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                //toastr.success("Directory Successfully Added!");
                //Datatables.ajax.reload();
                $("#Stechnicianmodal").modal('hide');

                $("#name").val("");
                $("#techEmail").val("");

                $('#pleasewait').modal('show');
                // show toastr after 3
                setTimeout(function () {
                    toastr.success("Programmer Successfully Added!");
                    // hide please wait modal
                }, 1000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            //if failed
            error: function (data) {
                toastr.error("Name Already Exist In the Database/ Invalid")
            }
        });
    });

    //Get edit
    $("#softwaretechList").on('click', '.edit', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/stech/getTechbyId/' + id;
        // alert(id);
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $("#editmodal").modal('show');
                $('#edit').find('input[name="id"]').val(data.id);
                $('#edit').find('input[name="name"]').val(data.name);
                $('#edit').find('input[name="techEmail"]').val(data.techEmail);

                //alert(data.techEmail);
            },
            //if failed
            error: function (data) {
                // console.log(data, data.id, data.name);
                toastr.error("Error")
            }
        })
    })

    $("#Updatetech").click(function (e) {
        e.preventDefault();
        var data = {
            name: $('#edit').find('input[name=name]').val(),
            techEmail: $('#edit').find('input[name=techEmail]').val(),
        };
        var id = $('#edit').find('input[name="id"]').val();
        $.ajax({
            type: 'PUT',
            url: '/ServiceRequest/api/stech/updateSoftwareTech/' + id,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                $("#editmodal").modal('hide');

                $('#pleasewait').modal('show');
                // show toastr after 3
                setTimeout(function () {
                    toastr.success("Successfully Updated!");
                    // hide please wait modal
                }, 1000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);

            },
            //if failed
            error: function (data) {
                toastr.error("Error")
            }
        });
    });

    $('#softwaretechList').on('click', '.delete', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/stech/Delete/' + id;
        $.ajax({
            // confirm before delete
            beforeSend: function (xhr) {
                // use bootbox
                bootbox.confirm({
                    message: "Are you sure you want to delete this record?",
                    buttons: {
                        // yes and no sequence

                        confirm: {
                            label: 'Yes',
                            className: 'btn-success btn-sm'
                        },
                        cancel: {
                            label: 'No',
                            className: 'btn-danger btn-sm'
                        }
                    },
                    callback: function (result) {
                        if (result) {
                            $.ajax({
                                type: 'DELETE',
                                url: url,
                                headers: {
                                    'Authorization': 'Bearer ' + localStorage.getItem('access_token')
                                },
                                success: function (data) {
                                    //toastr.success("Data Successfully Deleted!");
                                    //Datatables.ajax.reload();
                                    $('#pleasewait').modal('show');
                                    //show toastr after 3
                                    setTimeout(function () {
                                        toastr.success("Data Successfully Updated!");
                                        // hide please wait modal
                                    }, 2000);
                                    setTimeout(function () {
                                        window.location.reload();
                                    }, 3000);
                                },
                                //if failed
                                error: function (data) {
                                    toastr.error("Error")
                                }
                            });
                        }
                    }
                });

            },
        });
    });
}


function UnitTypes() {
    var Datatables;
    Datatables = $("#unitList").DataTable({
        ajax: {
            url: "/ServiceRequest/api/unit/get",
            dataSrc: "",
            headers: {
                "Authorization": "Bearer " + localStorage.getItem('access_token')
            }
        },
        autoWidth: false,
        columns: [

            {
                data: "dateAdded"
            },
            {
                data: "name",
            },
            {
                data: null,
                render: function (data) {
                    return '<button class="btn btn-success btn-sm edit" data-id="' + data.id + '"><i class="fas fa-edit"></i> Edit</button>'
                        + '  <button class="btn btn-danger btn-sm delete" data-id="' + data.id + '"><i class="fas fa-trash"></i> Delete</button>'
                },
                "orderable": false,
            }

        ]
    });

    $('#btnSubmit').click(function (e) {
        e.preventDefault();
        //find name
        var name = $('input[name=name]').val();
        // data
        var data = {
            name: name,
        };
        $.ajax({
            type: 'POST',
            url: '/ServiceRequest/api/unit/save',
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                $("#name").val("");
                $('#UnitTypeAdd').modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success("Data Successfully Added!");
                    // hide please wait modal
                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            //if failed
            error: function (data) {
                toastr.error("Name Already Exist In the Database/ Invalid")
            }
        });
    });

    //Get edit
    $("#unitList").on('click', '.edit', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/unit/get/' + id;
        // alert(id);
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $("#editmodal").modal('show');
                $('#edit').find('input[name="id"]').val(data.id);
                $('#edit').find('input[name="name"]').val(data.name);
            },
            //if failed
            error: function (data) {
                // console.log(data, data.id, data.name);
                toastr.error("Error")
            }
        })
    });

    $("#UpdateData").click(function (e) {
        e.preventDefault();
        var data = {
            name: $('#edit').find('input[name=name]').val(),
        };
        var id = $('#edit').find('input[name="id"]').val();
        $.ajax({
            type: 'PUT',
            url: '/ServiceRequest/api/unit/updateUnit/' + id,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                $('#editmodal').modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success("Data Successfully Updated!");
                    // hide please wait modal
                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            //if failed
            error: function (data) {
                toastr.error("Error")
            }
        });
    });

    $('#unitList').on('click', '.delete', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/unit/Delete/' + id;
        $.ajax({
            // confirm before delete
            beforeSend: function (xhr) {
                // use bootbox
                bootbox.confirm({
                    message: "Are you sure you want to delete this record?",
                    buttons: {
                        // yes and no sequence

                        confirm: {
                            label: 'Yes',
                            className: 'btn-success btn-sm'
                        },
                        cancel: {
                            label: 'No',
                            className: 'btn-danger btn-sm'
                        }
                    },
                    callback: function (result) {
                        if (result) {
                            $.ajax({
                                type: 'DELETE',
                                url: url,
                                headers: {
                                    'Authorization': 'Bearer ' + localStorage.getItem('access_token')
                                },
                                success: function (data) {
                                    $('#editmodal').modal('hide');
                                    //show please wait modal
                                    $('#pleasewait').modal('show');
                                    //show toastr after 3
                                    setTimeout(function () {
                                        toastr.success("Data Successfully Deleted!");
                                        // hide please wait modal
                                    }, 2000);
                                    setTimeout(function () {
                                        window.location.reload();
                                    }, 3000);

                                },
                                //if failed
                                error: function (data) {
                                    toastr.error("Error")
                                }
                            });
                        }
                    }
                });

            },
        });
    });
}
var urls = [];
function HardwareAdmin() {
    var table2 = $("#hardwareRequestList").DataTable({
        "ajax": {
            "url": "/ServiceRequest/api/all/hardReq",
            "headers": {
                "Authorization": "Bearer " + localStorage.getItem('access_token')
            },
            "type": "GET",
            "dataType": "json",
        },
        "processing": "true",
        "serverSide": "true",
        "ordering": "true",
        "paging": "true",
        "fnInitComplete": function (oSettings, json) {
            // search after pressing enter
            $('#hardwareRequestList_filter input').unbind();
            $('#hardwareRequestList_filter input').bind('keyup', function (e) {
                if (e.keyCode == 13) {
                    // trigger search 
                    $('#pleasewait').modal('show');
                    setTimeout(function () {
                        $('#pleasewait').modal('hide');
                    }, 2500);
                    $('#hardwareRequestList').DataTable().search(this.value).draw();
                }
                // if search is empty, reset the filter
                if (this.value == "") {
                    $('#hardwareRequestList').DataTable().search("").draw();
                }
            }
            );
        },
        // "searching" : " true",
        // "searchDelay" : 1000,
        // order by desc dateuploaded
        "order": [[0, "desc"]],
        "language": {
            "processing": "Loading... Please wait"

        },
        "autoWidth": false,
        "columns": [

            {
                data: "ticket",
                render: function (data, type, row) {
                    if (data == null) {
                        return '<span class="badge bg-danger">No Ticket</span>';
                    }
                    return `<span class="badge bg-info">${(data)}</span>`
                }
            },
            { data: "dateAdded" },
            { data: "fullName" },
            {
                data: "hardwareId",
                render: function (data, type, row) {
                    return row.hardware.name;
                }
            },
            {
                data: "hardwareTechnician.name",
                render: function (data, type, row) {
                    if (data == null) {
                        return '<span class="badge bg-success">Not Assigned</span>'
                    }
                    else
                        return row.hardwareTechnician.name;
                }
            },
            {
                data: "statusId", render: function (data, type, row) {
                    if (data == 1) {
                        return '<span class="badge bg-secondary">Open</span>'
                    }
                    else if (data == 2) {
                        return '<span class="badge bg-success">Resolved</span>'
                    }
                    else if (data == 3) {
                        return '<span class="badge bg-warning">On hold</span>'
                    }
                    else if (data == 4) {
                        return '<span class="badge bg-info">In Progress</span>'
                    }
                    else if (data == 5) {
                        return '<span class="badge  bg-danger">Cancel Request</span>'
                    }
                    else
                        return '<span class="badge  bg-secondary">Open</span>'
                }
            },
            {
                data: "isVerified", render: function (data, type, row) {
                    if (data == true) {
                        return '<i class="fas fa-check-circle fa-2x ms-4" style="color:green;"></i>'

                    }
                    else
                        return ''
                }
            },
            {
                data: null,
                render: function (data) {
                    return '<button class="btn btn-info text-light btn-sm request ms-1 mb-1" data-id="' + data.id + '"><i class="fas fa-eye"></i> View </button>'
                        + '<button class=" btn btn-success btn-sm attach ms-1 mb-1 text-white" data-id="' + data.id + '"><i class="fas fa-paperclip"></i> Attachment</button>'
                        + '<button class=" btn btn-primary btn-sm technician ms-1 mb-1" data-id="' + data.id + '"><i class="fas fa-users-cog"></i> Technician</button>'
                        + '<button class="btn btn-secondary btn-sm approvedby ms-1 mb-1 text-white" data-id="' + data.id + '"><i class="fas fa-user-edit"></i> Verified </button>'
                },
                orderable: false,
                width: "420px",
            },

        ],
    });

    $('#hardwareRequestList').on('click', '.attach', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/hardwareRequest/GetRequestbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data, value) {
                $('#hardwareDisplay').modal('show');
                $('#hardwareDisplay').find('.modal-title').text('File Attachment');
                var refId = $('#hardwareRequestId').val(data.id);
                //alert(data.statusId);
                var uploadTable = $('#UploadList').DataTable({
                    destroy: true,
                    ajax: {
                        url: "/ServiceRequest/api/v2/uploadDislpay/" + id,
                        dataSrc: '',
                        headers: {
                            "Authorization": "Bearer " + localStorage.getItem('access_token')
                        }
                    },
                    searching: false,
                    ordering: false,
                    columns: [
                        {
                            data: "dateAdded"
                        },
                        {
                            data: "documentBlob",
                            render: function (data, type, row, meta) {
                                var fileType = row.imagePath.split('.').pop();
                                if (fileType == 'pdf') {
                                    return '<i class="fas fa-file-pdf fa-2x" style="color: red; font-size: 50px;"></i>';
                                } else if (fileType == 'doc' || fileType == 'docx') {
                                    return '<i class="fas fa-file-word fa-2x" style="color: blue; font-size: 50px;"> </i>';
                                } else if (fileType == 'xls' || fileType == 'xlsx') {
                                    return '<i class="fas fa-file-excel fa-2x" style="color: green; font-size: 50px;"></i>';
                                } else if (fileType == 'ppt' || fileType == 'pptx') {
                                    return '<i class="fas fa-file-powerpoint fa-2x" style="color: orange; font-size: 50px;"></i>';
                                } else if (fileType == 'jpg' || fileType == 'png' || fileType == 'jpeg') {
                                    return '<img src="data:image/png;base64,' + data + '" class="avatar" width="250" height="250"/>';
                                } else {
                                    return '<i class="bi bi-images" style="font-size: 250px;"></i> ';
                                }
                            },
                            className: "text-center"
                        },

                        {
                            // download link by using anchor link only
                            data: "documentBlob",
                            render: function (data, type, row) {
                                var f = new Blob([s2ab(atob(data))], { type: "" });
                                var filename = row.imagePath.replace(/^.*[\\\/]/, '');
                                var newF = URL.createObjectURL(f);
                                urls.push(newF);
                                var fileType = row.imagePath.split('.').pop();
                                //    check file if is image type
                                if (fileType == 'jpg' || fileType == 'png' || fileType == 'jpeg' || fileType == 'pdf' || fileType == 'gif') {
                                    // return '<a href="/api/v1/download/' + row.id + '" class="btn btn-sm btn-success">Download</a>'
                                    //     +
                                    return '<a href="' + newF + '" class="btn btn-sm btn-success" download="' + filename + '">Download</a>';
                                }
                                else {
                                    return '';
                                }
                            },
                            className: "text-center"
                        },

                    ]
                });


                $('#UploadList').on('click', '.delete', function () {
                    var id = $(this).attr('data-id');
                    bootbox.confirm({
                        message: "Are you sure you want to delete this record?",
                        buttons: {
                            confirm: {
                                label: 'Yes',
                                className: 'btn-success btn-sm'
                            },
                            cancel: {
                                label: 'No',
                                className: 'btn-danger btn-sm'
                            }
                        },
                        callback: function (result) {
                            if (result) {
                                $.ajax({
                                    type: 'DELETE',
                                    url: '/ServiceRequest/api/v1/deleteImage/' + id,
                                    headers: {
                                        "Authorization": "Bearer " + localStorage.getItem('access_token')
                                    },
                                    success: function (data) {
                                        toastr.success("Data Successfully Deleted!");
                                        window.location.reload();
                                    }
                                });
                            }
                        }
                    });
                });
            },
            error: function (data) {
                toastr.error("Failed")
            }
        });
        // alert(data);
    });



    $('#hardwareRequestList').on('click', '.technician', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/hardwareRequest/GetRequestbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#TechnicianModal').modal('show');
                $('#assignTech').find('input[name="id"]').val(data.id);
                $('#assignTech').find('select[name="hardwareTechnicianId"]').val("");
                $('#assignTech').find('input[name="hardwareTechnicianId"]').val(data.hardwareTechnicianId);

                ////alert(data.hardwareTechnicianId);
                //if (data.hardwareTechnicianId == 0)
                //    $('.update-technician').prop('disabled', false)
                //else
                //    $('.update-technician').prop('disabled', true)

            }
        })
    });
    $('#updateTechnician').click(function (e) {
        // var id = $('input[name=id]').val();
        // var data = $('#assignTech').serialize();
        var hardwareTechnicianId = $('#assignTech').find('select[name=hardwareTechnicianId]').val();
        var hardwareRequestId = $('#assignTech').find('input[name=id]').val();
        var data = {
            "id": hardwareRequestId,
            "hardwareTechnicianId": hardwareTechnicianId
        };
        $.ajax({
            type: 'PUT',
            url: '/ServiceRequest/api/hardwareRequest/updateTechnician/' + hardwareRequestId,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                $("#TechnicianModal").modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success(" Successfully Assign Technician!");
                    // hide please wait modal

                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            //if failed
            error: function (data) {
                //alert(id);
                toastr.error("Please Assign Technician!")
            }
        });
    });
    //get id assign
    $('#hardwareRequestList').on('click', '.assign', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/hardwareRequest/GetRequestbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#StatusModal').modal('show');
                $('#assign').find('input[name="id"]').val(data.id);
                $('#assign').find('select[name="hardwareTechnicianId"]').val(data.hardwareTechnicianId);
                $('#assign').find('select[name="statusId"]').val("");

                // alert(data.statusId);
                if (data.statusId == 5)
                    $('.update-status').prop('disabled', true)
                else if (data.statusId == 3)
                    $('.update-status').prop('disabled', true)
                else
                    $('.update-status').prop('disabled', false)
            }
        })
        // alert(data);
    });
    //submit assign
    $('#updateStatus').click(function (e) {
        e.preventDefault();
        var id = $('input[name=id]').val();
        var data = $('#assign').serialize();
        $.ajax({
            type: 'PUT',
            url: '/ServiceRequest/api/hardwareRequest/updateStatus/' + id,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                $('#StatusModal').modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success("Data Successfully Update Status!");
                    // hide please wait modal
                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            //if failed
            error: function (data) {
                //alert(id);
                toastr.error("Failed!")
            }
        });
    });
    //get RequestDetails
    $('#hardwareRequestList').on('click', '.request', function () {
        var id = $(this).attr('data-id');
        $("#generateReportBtn").attr('data-id', id);

        var url = '/ServiceRequest/api/hardwareRequest/GetRequestbyId/' + id;

        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#hardwareRequestModal').modal('show');
                $('#EditRequest').find('input[name="id"]').val(data.id);
                $('#EditRequest').find('input[name="fullName"]').val(data.fullName);
                $('#EditRequest').find('input[name="mobileNumber"]').val(data.mobileNumber);
                $('#EditRequest').find('input[name="modelName"]').val(data.modelName);
                $('#EditRequest').find('select[name="departmentsId"]').val(data.departmentsId);
                $('#EditRequest').find('select[name="divisionsId"]').val(data.divisionsId);
                $('#EditRequest').find('select[name="hardwareId"]').val(data.hardwareId);
                $('#EditRequest').find('select[name="unitTypeId"]').val(data.unitTypeId);
                $('#EditRequest').find('input[name="brandName"]').val(data.brandName);
                $('#EditRequest').find('input[name="FileName"]').val(data.documentLabel);
                $('#EditRequest').find('textarea[name="description"]').val(data.description);
                $('#EditRequest').find('select[name="statusId"]').val(data.statusId);
                $('#EditRequest').find('select[name="findingId"]').val(data.findingId);
                $('#EditRequest').find('select[name="hardwareTechnicianId"]').val(data.hardwareTechnicianId);
                $('#EditRequest').find('input[name="possibleCause"]').val(data.possibleCause);
                $('#EditRequest').find('textarea[name="remarks"]').val(data.remarks);
                $('#EditRequest').find('input[name="dateStarted"]').val(data.dateStarted);
                $('#EditRequest').find('input[name="dateEnded"]').val(data.dateEnded);
                $('#EditRequest').find('input[name="timeStarted"]').val(data.timeStarted);
                $('#EditRequest').find('input[name="timeEnded"]').val(data.timeEnded);
                $('#EditRequest').find('input[name="approver"]').val(data.approver);
                $('#EditRequest').find('input[name="dateApproved"]').val(data.dateApproved);
                $('#EditRequest').find('input[name="viewer"]').val(data.viewer);
                $('#EditRequest').find('input[name="dateView"]').val(data.dateView);
                $('#EditRequest').find('input[name="serialNumber"]').val(data.serialNumber);
                $('#EditRequest').find('input[name="controlNumber"]').val(data.controlNumber);

                // alert(data.timeEnded);
            }
        })
        // alert(data);
    });
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/department',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=departmentsId]').append('<option value=""> Select Department</option>');
            $.each(data, function (index, value) {
                $('select[name=departmentsId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },
    });
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/UnitType',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=unitTypeId]').append('<option value=""> Select UnitType</option>');
            $.each(data, function (index, value) {
                $('select[name=unitTypeId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },
    });
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/division',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=divisionsId]').append('<option value=""> Select Division</option>');
            $.each(data, function (index, value) {
                $('select[name=divisionsId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },
    });
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/hardware',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=hardwareId]').append('<option value=""> Select Hardware Service Category</option>');
            $.each(data, function (index, value) {
                $('select[name=hardwareId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },
    });
    //GET findings
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/Findings',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=findingId]').append('<option value=""> Select Result</option>');
            $.each(data, function (index, value) {
                $('select[name=findingId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },
    });
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/Technician',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=hardwareTechnicianId]').append('<option value=""> Select Technician</option>');
            $.each(data, function (index, value) {
                $('select[name=hardwareTechnicianId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },
    });
    //status
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/status',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=statusId]').append('<option value=""> Select Status </option>');
            $.each(data, function (index, value) {
                $('select[name=statusId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },
    });
    $('#hardwareRequestList').on('click', '.approvedby', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/hardwareRequest/GetRequestbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#ApprovedByModal').modal('show');
                $('#approve').find('input[name="id"]').val(data.id);
                $('#approve').find('select[name="statusId"]').val(data.statusId);
                $('#approve').find('textarea[name="approverRemarks"]').val(data.approverRemarks);
                if (data.isReported == true)
                    $('.update-verified').prop('disabled', false)
                else
                    $('.update-verified').prop('disabled', true)
                // alert(id);
                // alert(data.statusId);
            }
        })
        // alert(data);
        //AddApproved
    });
    $('#Approved').click(function (e) {
        e.preventDefault();
        var id = $('#approve').find('input[name="id"]').val();
        var data = $('#approve').serialize();
        $.ajax({
            type: 'PUT',
            url: '/ServiceRequest/api/hardwareRequest/approver/' + id,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                $('#ApprovedByModal').modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success(" Successfully Approved!");
                    // hide please wait modal

                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            //if failed
            error: function (data) {
                toastr.error("Fill up Remarks")
            }
        });
    });
    $('#hardwareRequestList').on('click', '.viewedby', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/hardwareRequest/GetRequestbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#ViewedByModal').modal('show');
                $('#viewed').find('input[name="viewedBy"]').val(data.viewer);
                $('#viewed').find('textarea[name="viewedRemarks"]').val(data.viewedRemarks);
            }
        })
    })
}

function HardwareAdmin2() {
    var tables;
    tables = $('#hardwareRequestList').DataTable({
        ajax: {
            url: "/ServiceRequest/api/v2/hardwareRequest/GetRequest",
            dataSrc: "",
            headers: {
                "Authorization": "Bearer" + localStorage.getItem('access_token')
            }
        },
        ordering: false,
        autoWidth: false,
        columns: [
            {
                data: "ticket",
                render: function (data, type, row) {
                    if (data == null) {
                        return '<span class="badge bg-danger">No Ticket</span>';
                    }
                    return `<span class="badge bg-info">${(data)}</span>`
                }
            },
            { data: "dateAdded" },
            { data: "fullName" },
            {
                data: "statusId", render: function (data, type, row) {
                    if (data == 1) {
                        return '<span class="badge bg-secondary">Open</span>'
                    }
                    else if (data == 2) {
                        return '<span class="badge bg-success">Resolved</span>'
                    }
                    else if (data == 3) {
                        return '<span class="badge bg-warning">On hold</span>'
                    }
                    else if (data == 4) {
                        return '<span class="badge bg-info">In Progress</span>'
                    }
                    else if (data == 5) {
                        return '<span class="badge  bg-danger">Cancel Request</span>'
                    }
                    else
                        return '<span class="badge  bg-secondary">Open</span>'
                }
            },
            {
                data: "dateApproved", render: function (data, type, row) {
                    if (data == null) {
                        return '<span class="badge  bg-primary">waiting to verified</span>'
                    }
                    else
                        return `<span>${moment(data).format("D/M/Y LT")}</span>`;
                }
            },
            {
                data: "dateView", render: function (data, type, row) {
                    if (data == null) {
                        return '<span class="badge  bg-primary">waiting to verified</span>'
                    }
                    else
                        return `<span>${moment(data).format("D/M/Y LT")}</span>`;
                }
            },
            {
                data: null,
                render: function (data) {
                    return '<button class="btn btn-info text-light btn-sm request ms-1 mb-1" data-id="' + data.id + '"><i class="fas fa-eye"></i> View Details </button>'
                        + '<button class="btn btn-success btn-sm approvedby ms-1 mb-1 " data-id="' + data.id + '"><i class="fas fa-user-edit"></i> Verified </button>'
                },
                orderable: false,
                width: "250px",
            },
        ]
    });
    $('#hardwareRequestList').on('click', '.technician', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/hardwareRequest/GetRequestbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#TechnicianModal').modal('show');
                $('#assignTech').find('input[name="id"]').val(data.id);
                $('#assignTech').find('select[name="hardwareTechnicianId"]').val("");

                //  alert(data.statusId);
                if (data.statusId == 5)
                    $('.update-technician').prop('disabled', true)
                else if (data.statusId == 3)
                    $('.update-technician').prop('disabled', true)

                else
                    $('.update-technician').prop('disabled', false)
            }
        })
    });
    $('#updateTechnician').click(function (e) {
        // var id = $('input[name=id]').val();
        // var data = $('#assignTech').serialize();
        var hardwareTechnicianId = $('#assignTech').find('select[name=hardwareTechnicianId]').val();
        var hardwareRequestId = $('#assignTech').find('input[name=id]').val();
        var data = {
            "id": hardwareRequestId,
            "hardwareTechnicianId": hardwareTechnicianId
        };
        $.ajax({
            type: 'PUT',
            url: '/ServiceRequest/api/hardwareRequest/updateTechnician/' + hardwareRequestId,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                $("#TechnicianModal").modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success("Successfully Assign Technician!");
                    // hide please wait modal
                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            //if failed
            error: function (data) {
                //alert(id);
                toastr.error("Please Assign Technician!")
            }
        });
    });
    //get id assign
    $('#hardwareRequestList').on('click', '.assign', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/hardwareRequest/GetRequestbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#StatusModal').modal('show');
                $('#assign').find('input[name="id"]').val(data.id);
                $('#assign').find('select[name="hardwareTechnicianId"]').val(data.hardwareTechnicianId);
                $('#assign').find('select[name="statusId"]').val("");

                // alert(data.statusId);
                if (data.statusId == 5)
                    $('.update-status').prop('disabled', true)
                else if (data.statusId == 3)
                    $('.update-status').prop('disabled', true)
                else
                    $('.update-status').prop('disabled', false)
            }
        })
        // alert(data);
    });
    //submit assign
    $('#updateStatus').click(function (e) {
        e.preventDefault();
        var id = $('input[name=id]').val();
        var data = $('#assign').serialize();
        $.ajax({
            type: 'PUT',
            url: '/ServiceRequest/api/hardwareRequest/updateStatus/' + id,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                //toastr.success(" Successfully Update Status!");
                //tables.ajax.reload();
                ////setTimeout(() => {
                ////    location.reload();
                ////}, 1500);
                $("#StatusModal").modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success("Successfully Update Status!");
                    // hide please wait modal
                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            //if failed
            error: function (data) {
                //alert(id);
                toastr.error("Please Update Status!")
            }
        });
    });
    //get RequestDetails
    $('#hardwareRequestList').on('click', '.request', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/hardwareRequest/GetRequestbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#hardwareRequestModal').modal('show');
                $('#EditRequest').find('input[name="id"]').val(data.id);
                $('#EditRequest').find('input[name="fullName"]').val(data.fullName);
                $('#EditRequest').find('input[name="mobileNumber"]').val(data.mobileNumber);
                $('#EditRequest').find('input[name="modelName"]').val(data.modelName);
                $('#EditRequest').find('select[name="departmentsId"]').val(data.departmentsId);
                $('#EditRequest').find('select[name="divisionsId"]').val(data.divisionsId);
                $('#EditRequest').find('select[name="hardwareId"]').val(data.hardwareId);
                $('#EditRequest').find('select[name="unitTypeId"]').val(data.unitTypeId);
                $('#EditRequest').find('input[name="brandName"]').val(data.brandName);
                $('#EditRequest').find('textarea[name="description"]').val(data.description);
                $('#EditRequest').find('select[name="statusId"]').val(data.statusId);
                $('#EditRequest').find('select[name="findingId"]').val(data.findingId);
                $('#EditRequest').find('select[name="hardwareTechnicianId"]').val(data.hardwareTechnicianId);
                $('#EditRequest').find('input[name="possibleCause"]').val(data.possibleCause);
                $('#EditRequest').find('textarea[name="remarks"]').val(data.remarks);
                $('#EditRequest').find('input[name="dateStarted"]').val(data.dateStarted);
                $('#EditRequest').find('input[name="dateEnded"]').val(data.dateEnded);
                $('#EditRequest').find('input[name="timeStarted"]').val(data.timeStarted);
                $('#EditRequest').find('input[name="timeEnded"]').val(data.timeEnded);
                $('#EditRequest').find('input[name="approver"]').val(data.approver);
                $('#EditRequest').find('input[name="dateApproved"]').val(data.dateApproved);
                $('#EditRequest').find('textarea[name="approverRemarks"]').val(data.approverRemarks);
                $('#EditRequest').find('input[name="viewer"]').val(data.viewer);
                $('#EditRequest').find('input[name="dateView"]').val(data.dateView);
                $('#EditRequest').find('textarea[name="viewedRemarks"]').val(data.viewedRemarks);



                // alert(data.timeEnded);
            }
        })
        // alert(data);
    });
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/department',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=departmentsId]').append('<option value=""> Select Department</option>');
            $.each(data, function (index, value) {
                $('select[name=departmentsId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },
    });
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/UnitType',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=unitTypeId]').append('<option value=""> Select UnitType</option>');
            $.each(data, function (index, value) {
                $('select[name=unitTypeId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },
    });
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/division',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=divisionsId]').append('<option value=""> Select Division</option>');
            $.each(data, function (index, value) {
                $('select[name=divisionsId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },
    });
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/hardware',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=hardwareId]').append('<option value=""> Select Hardware Service Category</option>');
            $.each(data, function (index, value) {
                $('select[name=hardwareId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },
    });
    //GET findings
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/Findings',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=findingId]').append('<option value=""> Select Result</option>');
            $.each(data, function (index, value) {
                $('select[name=findingId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },
    });
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/Technician',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=hardwareTechnicianId]').append('<option value=""> Select Technician</option>');
            $.each(data, function (index, value) {
                $('select[name=hardwareTechnicianId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },
    });
    //status
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/status',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=statusId]').append('<option value=""> Select Status </option>');
            $.each(data, function (index, value) {
                $('select[name=statusId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },
    });
    $('#hardwareRequestList').on('click', '.approvedby', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/hardwareRequest/GetRequestbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#ApprovedByModal').modal('show');
                $('#approve').find('input[name="id"]').val(data.id);
                $('#approve').find('select[name="statusId"]').val(data.statusId);
                $('#approve').find('textarea[name="approverRemarks"]').val(data.approverRemarks);
                $('#approve').find('input[name="isReported"]').val(data.isReported);

                if (data.isReported == true)
                    $('.update-verified').prop('disabled', false)
                else
                    $('.update-verified').prop('disabled', true)
                // alert(id);
                // alert(data.statusId);
            }
        })
        // alert(data);
        //AddApproved
    });
    $('#Approved').click(function (e) {
        e.preventDefault();
        var id = $('#approve').find('input[name="id"]').val();
        var data = $('#approve').serialize();
        $.ajax({
            type: 'PUT',
            url: '/ServiceRequest/api/hardwareRequest/approver/' + id,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                tables.ajax.reload();
                $("#ApprovedByModal").modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success("Request Successfully Approved!");
                    // hide please wait modal
                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            //if failed
            error: function (data) {
                toastr.error("Fill up Remarks")
            }
        });
    });
    $('#hardwareRequestList').on('click', '.viewedby', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/hardwareRequest/GetRequestbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#ViewedByModal').modal('show');
                $('#viewed').find('input[name="viewedBy"]').val(data.viewer);
                $('#viewed').find('textarea[name="viewedRemarks"]').val(data.viewedRemarks);
            }
        })
    })
}

function HardwareDeveloper() {
    var table2 = $("#hardwareRequestList").DataTable({
        "ajax": {
            "url": "/ServiceRequest/api/all/hardReq",
            "headers": {
                "Authorization": "Bearer " + localStorage.getItem('access_token')
            },
            "type": "GET",
            "dataType": "json",
        },
        "processing": "true",
        "serverSide": "true",
        "ordering": "true",
        "paging": "true",
        "fnInitComplete": function (oSettings, json) {
            // search after pressing enter
            $('#hardwareRequestList_filter input').unbind();
            $('#hardwareRequestList_filter input').bind('keyup', function (e) {
                if (e.keyCode == 13) {
                    // trigger search 
                    $('#pleasewait').modal('show');
                    setTimeout(function () {
                        $('#pleasewait').modal('hide');
                    }, 2500);
                    $('#hardwareRequestList').DataTable().search(this.value).draw();
                }
                // if search is empty, reset the filter
                if (this.value == "") {
                    $('#hardwareRequestList').DataTable().search("").draw();
                }
            }
            );
        },
        // "searching" : " true",
        // "searchDelay" : 1000,
        // order by desc dateuploaded
        "order": [[0, "desc"]],
        "language": {
            "processing": "Loading... Please wait"

        },
        "autoWidth": false,
        "columns": [

            {
                "data": "ticket",
                render: function (data, type, row) {
                    if (data == null) {
                        return '<span class="badge bg-danger">No Ticket</span>';
                    }
                    return `<span class="badge bg-info">${(data)}</span>`
                }

            },
            { "data": "dateAdded" },
            { "data": "fullName" },
            {
                "data": "hardwareId",
                "render": function (data, type, row) {
                    return row.hardware.name;
                }
            },
            {
                "data": "statusId", render: function (data, type, row) {
                    if (data == 1) {
                        return '<span class="badge  bg-secondary">Open</span>'
                    }
                    else if (data == 2) {
                        return '<span class="badge bg-success">Resolved</span>'
                    }
                    else if (data == 3) {
                        return '<span class="badge  bg-warning">On hold</span>'
                    }
                    else if (data == 4) {
                        return '<span class="badge  bg-info">In Progress</span>'
                    }
                    else if (data == 5) {
                        return '<span class="badge  bg-danger">Cancel Request</span>'
                    }
                    else
                        return '<span class="badge  bg-secondary">Open</span>'

                }
            },

            {
                "data": null,
                "render": function (data, type, full) {
                    return '<button class="btn btn-info btn-sm view ms-1 mb-1 text-white" data-id="' + data.id + '"><i class="far fa-eye"></i> View</button>' +
                        '<button class=" btn btn-success btn-sm attach ms-1 mb-1 text-white" data-id="' + data.id + '"><i class="fas fa-paperclip"></i> Attachments</button>' +
                        '<button class="btn btn-danger btn-sm delete ms-1 mb-1" data-id="' + data.id + '"><i class="far fa-edit"></i> Delete</button>'


                },
                "orderable": false,
                "width": "290px",
            },

        ],
    });

    $('#hardwareRequestList').on('click', '.attach', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/hardwareRequest/GetRequestbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data, value) {
                $('#hardwareDisplay').modal('show');
                $('#hardwareDisplay').find('.modal-title').text('File Attachment');
                var refId = $('#hardwareRequestId').val(data.id);
                //alert(data.statusId);
                var uploadTable = $('#UploadList').DataTable({
                    destroy: true,
                    ajax: {
                        url: "/ServiceRequest/api/v2/uploadDislpay/" + id,
                        dataSrc: '',
                        headers: {
                            "Authorization": "Bearer " + localStorage.getItem('access_token')
                        }
                    },
                    searching: false,
                    ordering: false,
                    columns: [
                        {
                            data: "dateAdded"
                        },
                        {
                            data: "documentBlob",
                            render: function (data, type, row, meta) {
                                var fileType = row.imagePath.split('.').pop();
                                if (fileType == 'pdf') {
                                    return '<i class="fas fa-file-pdf fa-2x" style="color: red; font-size: 50px;"></i>';
                                } else if (fileType == 'doc' || fileType == 'docx') {
                                    return '<i class="fas fa-file-word fa-2x" style="color: blue; font-size: 50px;"> </i>';
                                } else if (fileType == 'xls' || fileType == 'xlsx') {
                                    return '<i class="fas fa-file-excel fa-2x" style="color: green; font-size: 50px;"></i>';
                                } else if (fileType == 'ppt' || fileType == 'pptx') {
                                    return '<i class="fas fa-file-powerpoint fa-2x" style="color: orange; font-size: 50px;"></i>';
                                } else if (fileType == 'jpg' || fileType == 'png' || fileType == 'jpeg') {
                                    return '<img src="data:image/png;base64,' + data + '" class="avatar" width="250" height="250"/>';
                                } else {
                                    return '<i class="bi bi-images" style="font-size: 250px;"></i> ';
                                }
                            }
                        },

                        {
                            // download link by using anchor link only
                            data: "documentBlob",
                            render: function (data, type, row) {
                                var f = new Blob([s2ab(atob(data))], { type: "" });
                                var filename = row.imagePath.replace(/^.*[\\\/]/, '');
                                var newF = URL.createObjectURL(f);
                                urls.push(newF);
                                var fileType = row.imagePath.split('.').pop();
                                //    check file if is image type
                                if (fileType == 'jpg' || fileType == 'png' || fileType == 'jpeg' || fileType == 'pdf' || fileType == 'gif') {
                                    // return '<a href="/api/v1/download/' + row.id + '" class="btn btn-sm btn-success">Download</a>'
                                    //     +
                                    return '<a href="' + newF + '" class="btn btn-sm btn-success" download="' + filename + '">Download</a>';
                                }
                                else {
                                    return '';
                                }
                            },
                            className: "text-center"
                        },
                        {
                            data: null,
                            render: function (data, type, row) {
                                return '<button class="btn btn-danger text-light btn-sm delete ms-1 mb-1" data-id="' + row.id + '"><i class="fas fa-eye"></i> Delete </button>'
                            },
                            className: "action-center"

                        }

                    ]
                });


            },
            error: function (data) {
                toastr.error("Failed")
            }
        });
        // alert(data);
    });

    $('#UploadList').on('click', '.delete', function () {
        var id = $(this).attr('data-id');
        bootbox.confirm({
            message: "Are you sure you want to delete this record?",
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-success btn-sm'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-danger btn-sm'
                }
            },
            callback: function (result) {
                if (result) {
                    $.ajax({
                        type: 'DELETE',
                        url: '/ServiceRequest/api/v1/deleteImage/' + id,
                        headers: {
                            "Authorization": "Bearer " + localStorage.getItem('access_token')
                        },
                        success: function (data) {
                            toastr.success("Data Successfully Deleted!");
                            window.location.reload();
                        }
                    });
                }
            }
        });
    });


    //get RequestDetailsModal
    $('#hardwareRequestList').on('click', '.request', function () {
        var id = $(this).attr('data-id');
        // alert(id);
        var url = '/ServiceRequest/api/hardwareRequest/GetRequestbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#hardwareRequestModal').modal('show');

                $('#EditRequest').find('input[name="id"]').val(data.id);
                $('#EditRequest').find('input[name="brandName"]').val(data.brandName);
                $('#EditRequest').find('input[name="modelName"]').val(data.modelName);
                $('#EditRequest').find('select[name="unitTypeId"]').val(data.unitTypeId);
                $('#EditRequest').find('select[name="hardwareId"]').val(data.hardwareId);
                $('#EditRequest').find('textarea[name="description"]').val(data.description);

                $('#EditRequest').find('input[name="statusId"]').val(data.statusId);
                if (data.statusId == 2)
                    $('.update-request').prop('disabled', true)

                else if (data.statusId == 3)
                    $('.update-request').prop('disabled', true)

                else if (data.statusId == 4)
                    $('.update-request').prop('disabled', true)

                else if (data.statusId == 5)
                    $('.update-request').prop('disabled', true)
                else
                    $('.update-request').prop('disabled', false)




                //alert(data.statusId);
            }
        })
        // alert(data);
    });


    //save edit
    $('#UpdateRequest').click(function (e) {
        e.preventDefault();

        var id = $('#EditRequest').find('input[name="id"]').val();
        var data = $('#EditRequest').serialize();
        //alert(id);
        $.ajax({
            type: 'PUT',
            url: '/ServiceRequest/api/hardwareRequest/Request/' + id,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                // tables.ajax.reload();
                $("#hardwareRequestModal").modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success("Request Successfully Updated!");
                    // hide please wait modal
                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            //if failed
            error: function (data) {
                console.log(data.id);
                toastr.error("Fill up alll forms / Invalid")
            }
        })
    })


    //get CancelModal
    $('#hardwareRequestList').on('click', '.cancel', function () {
        var id = $(this).attr('data-id');
        //alert(id);
        var url = '/ServiceRequest/api/hardwareRequest/GetRequestbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#CancelRequestModal').modal('show');

                $('#cancelRequest').find('input[name="id"]').val(data.id);
                $('#cancelRequest').find('input[name="brandName"]').val(data.brandName);
                $('#cancelRequest').find('input[name="modelName"]').val(data.modelName);
                $('#cancelRequest').find('select[name="unitTypeId"]').val(data.unitTypeId);
                $('#cancelRequest').find('select[name="hardwareId"]').val(data.hardwareId);
                $('#cancelRequest').find('textarea[name="description"]').val(data.description);

                $('#cancleRequest').find('input[name="statusId"]').val(data.statusId);
                //alert(data.statusId);
                if (data.statusId == 2)
                    $('.cancel-request').attr('disabled', true);
                else if (data.statusId == 3)
                    $('.cancel-request').attr('disabled', true);
                else if (data.statusId == 4)
                    $('.cancel-request').attr('disabled', true);
                else if (data.statusId == 5)
                    $('.cancel-request').attr('disabled', true);
                else
                    $('.cancel-request').attr('disabled', false);
            }
        })
        // alert(data);
    });

    // save cancel
    $('#cancel').click(function (e) {
        e.preventDefault();
        var id = $('#cancelRequest').find('input[name="id"]').val();
        var data = $('#cancelRequest').serialize();
        // alert(id);
        $.ajax({
            type: 'PUT',
            url: '/ServiceRequest/api/hardwareRequest/cancel/' + id,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                $("#CancelRequestModal").modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success(" Successfully Cancelled Request!");
                    // hide please wait modal
                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            //if failed
            error: function (data) {
                console.log(data.id);
                toastr.error("Fill up Remarks")
            }
        })
    })



    //get Details
    $('#hardwareRequestList').on('click', '.view', function () {
        var id = $(this).attr('data-id');
        $("#generateReportBtn").attr('data-id', id);

        //alert(id);
        var url = '/ServiceRequest/api/hardwareRequest/GetRequestbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#viewmodal').modal('show');

                $('#viewRequest').find('input[name="id"]').val(data.id);
                $('#viewRequest').find('input[name="fullName"]').val(data.fullName);
                $('#viewRequest').find('input[name="FileName"]').val(data.documentLabel);

                $('#viewRequest').find('input[name="mobileNumber"]').val(data.mobileNumber);
                $('#viewRequest').find('input[name="modelName"]').val(data.modelName);
                $('#viewRequest').find('select[name="departmentsId"]').val(data.departmentsId);
                $('#viewRequest').find('select[name="divisionsId"]').val(data.divisionsId);
                $('#viewRequest').find('select[name="hardwareId"]').val(data.hardwareId);
                $('#viewRequest').find('select[name="unitTypeId"]').val(data.unitTypeId);
                $('#viewRequest').find('input[name="brandName"]').val(data.brandName);
                $('#viewRequest').find('textarea[name="description"]').val(data.description);
                $('#viewRequest').find('select[name="statusId"]').val(data.statusId);
                $('#viewRequest').find('select[name="findingId"]').val(data.findingId);
                $('#viewRequest').find('select[name="hardwareTechnicianId"]').val(data.hardwareTechnicianId);
                $('#viewRequest').find('input[name="possibleCause"]').val(data.possibleCause);
                $('#viewRequest').find('textarea[name="remarks"]').val(data.remarks);
                $('#viewRequest').find('input[name="dateStarted"]').val(data.dateStarted);
                $('#viewRequest').find('input[name="dateEnded"]').val(data.dateEnded);
                $('#viewRequest').find('input[name="timeStarted"]').val(data.timeStarted);
                $('#viewRequest').find('input[name="timeEnded"]').val(data.timeEnded);
                $('#viewRequest').find('input[name="viewer"]').val(data.viewer);
                $('#viewRequest').find('input[name="dateView"]').val(data.dateView);
                $('#viewRequest').find('input[name="approver"]').val(data.viewer);
                $('#viewRequest').find('input[name="dateApproved"]').val(data.dateView);

            }
        })
        // alert(data);
    });
    //unit Type
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/unitType',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=unitTypeId]').append('<option value="-1"> Select UnitType</option>');
            $.each(data, function (index, value) {
                $('select[name=unitTypeId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });

    //Department
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/department',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=departmentsId]').append('<option value=""> Select Department</option>');
            $.each(data, function (index, value) {
                $('select[name=departmentsId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    //Division
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/division',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=divisionsId]').append('<option value=""> Select Division</option>');
            $.each(data, function (index, value) {
                $('select[name=divisionsId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    //hardware Category
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/hardware',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=hardwareId]').append('<option value=""> Select Hardware Service Category</option>');
            $.each(data, function (index, value) {
                $('select[name=hardwareId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    //technician
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/Technician',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=hardwareTechnicianId]').append('<option value=""> Select Technician</option>');
            $.each(data, function (index, value) {
                $('select[name=hardwareTechnicianId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    //status
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/status',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=statusId]').append('<option value=""> Select Status </option>');
            $.each(data, function (index, value) {
                $('select[name=statusId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    //GET findings
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/Findings',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=findingId]').append('<option value=""> Select Result</option>');
            $.each(data, function (index, value) {
                $('select[name=findingId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },
    });
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/Technician',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=hardwareTechnicianId]').append('<option value=""> Select Technician</option>');
            $.each(data, function (index, value) {
                $('select[name=hardwareTechnicianId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },
    });

    //Delete
    $('#hardwareRequestList').on('click', '.delete', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/hardwareRequest/Delete/' + id;
        $.ajax({
            // confirm before delete
            beforeSend: function (xhr) {
                // use bootbox
                bootbox.confirm({
                    message: "Are you sure you want to delete this record?",
                    buttons: {
                        // yes and no sequence

                        confirm: {
                            label: 'Yes',
                            className: 'btn-success btn-sm'
                        },
                        cancel: {
                            label: 'No',
                            className: 'btn-danger btn-sm'
                        }
                    },
                    callback: function (result) {
                        if (result) {
                            $.ajax({
                                type: 'DELETE',
                                url: url,
                                headers: {
                                    'Authorization': 'Bearer ' + localStorage.getItem('access_token')
                                },
                                success: function (data) {
                                    //show please wait modal
                                    $('#pleasewait').modal('show');
                                    //show toastr after 3
                                    setTimeout(function () {
                                        toastr.success("Data Successfully Deleted!");
                                        // hide please wait modal
                                    }, 2000);
                                    setTimeout(function () {
                                        window.location.reload();
                                    }, 3000);
                                },
                                //if failed
                                error: function (data) {
                                    toastr.error("Error")
                                }
                            });
                        }
                    }
                });

            },
        });
    });

    //get RequestDetailsModal
    $('#hardwareRequestList').on('click', '.image', function () {
        var id = $(this).attr('data-id');
        // alert(id);
        var url = '/ServiceRequest/api/hardwareRequest/GetRequestbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#AddImage').modal('show');
                //alert(data.statusId);
            }
        })
        // alert(data);
    });
}
var urls = [];

function HardwareTechnicianForm() {
    var table2 = $("#hardwareRequestList").DataTable({
        "ajax": {
            "url": "/ServiceRequest/api/hr/assigned",
            "headers": {
                "Authorization": "Bearer " + localStorage.getItem('access_token')
            },
            "type": "GET",
            "dataType": "json",
        },
        "processing": "true",
        "serverSide": "true",
        "ordering": "true",
        "paging": "true",
        "fnInitComplete": function (oSettings, json) {
            // search after pressing enter
            $('#hardwareRequestList_filter input').unbind();
            $('#hardwareRequestList_filter input').bind('keyup', function (e) {
                if (e.keyCode == 13) {
                    // trigger search 
                    $('#pleasewait').modal('show');
                    setTimeout(function () {
                        $('#pleasewait').modal('hide');
                    }, 2500);
                    $('#hardwareRequestList').DataTable().search(this.value).draw();
                }
                // if search is empty, reset the filter
                if (this.value == "") {
                    $('#hardwareRequestList').DataTable().search("").draw();
                }
            }
            );
        },
        // "searching" : " true",
        // "searchDelay" : 1000,
        // order by desc dateuploaded
        "order": [[0, "desc"]],
        "language": {
            "processing": "Loading... Please wait"

        },
        "autoWidth": false,
        "columns": [

            {
                "data": "ticket",
                render: function (data, type, row) {
                    if (data == null) {
                        return '<span class="badge bg-danger">No Ticket</span>';
                    }
                    return `<span class="badge bg-info">${(data)}</span>`
                }

            },
            { "data": "dateAdded" },
            { "data": "fullName" },
            {
                "data": "hardwareId",
                "render": function (data, type, row) {
                    return row.hardware.name;
                }
            },
            {
                "data": "statusId", render: function (data, type, row) {
                    if (data == 1) {
                        return '<span class="badge  bg-secondary">Open</span>'
                    }
                    else if (data == 2) {
                        return '<span class="badge bg-success">Resolved</span>'
                    }
                    else if (data == 3) {
                        return '<span class="badge  bg-warning">On hold</span>'
                    }
                    else if (data == 4) {
                        return '<span class="badge  bg-info">In Progress</span>'
                    }
                    else if (data == 5) {
                        return '<span class="badge  bg-danger">Cancel Request</span>'
                    }
                    else
                        return '<span class="badge  bg-secondary">Open</span>'

                }
            },
            {
                data: "isReported", render: function (data, type, row) {
                    if (data == true) {
                        return '<i class="fas fa-check-circle fa-2x ms-4" style="color:green;"></i>'

                    }
                    else
                        return ''
                }
            },
            {
                data: "isVerified", render: function (data, type, row) {
                    if (data == true) {
                        return '<i class="fas fa-check-circle fa-2x ms-4" style="color:green;"></i>'

                    }
                    else
                        return ''
                }
            },
            {
                data: "isApproved", render: function (data, type, row) {
                    if (data == true) {
                        return '<i class="fas fa-check-circle fa-2x ms-4" style="color:green;"></i>'

                    }
                    else
                        return ''
                }
            },


            {
                data: null,
                render: function (data) {
                    return '<button class="btn btn-info text-light btn-sm view ms-1 mb-1" data-id="' + data.id + '"><i class="fas fa-eye"></i> View </button>'
                        + '<button class=" btn btn-success btn-sm attach ms-1 mb-1" data-id="' + data.id + '"><i class="fas fa-paperclip"></i> Attachments </button>'
                        + '<button class=" btn btn-primary btn-sm assign ms-1 mb-1" data-id="' + data.id + '"><i class="fas fa-edit ms-1"></i> Status</button>'
                        + '<button class="btn btn-secondary btn-sm edit ms-1 mb-1 text-white" data-id="' + data.id + '" > <i class="fas fa-user-cog"></i>  Report</button>'
                },
                orderable: false,
                width: "410px",
            },

        ],
    });


    $('#hardwareRequestList').on('click', '.attach', function () {
        var id = $(this).attr('data-id');
        // alert(id);
        var url = '/ServiceRequest/api/hardwareRequest/GetRequestbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data, value) {
                $('#hardwareDisplay').modal('show');
                $('#hardwareDisplay').find('.modal-title').text('File Attachment');
                var refId = $('#hardwareRequestId').val(data.id);
                //alert(data.id);
                //alert(data.statusId);
                var uploadTable = $('#UploadList').DataTable({
                    destroy: true,

                    ajax: {
                        url: "/ServiceRequest/api/v2/uploadDislpay/" + id,
                        dataSrc: '',
                        headers: {
                            "Authorization": "Bearer " + localStorage.getItem('access_token')
                        }
                    },
                    searching: false,
                    ordering: false,

                    columns: [
                        {
                            data: "dateAdded"
                        },
                        {
                            data: "documentBlob",
                            render: function (data, type, row, meta) {
                                var fileType = row.imagePath.split('.').pop();
                                if (fileType == 'pdf') {
                                    return '<i class="fas fa-file-pdf fa-2x" style="color: red; font-size: 50px;"></i>';
                                } else if (fileType == 'doc' || fileType == 'docx') {
                                    return '<i class="fas fa-file-word fa-2x" style="color: blue; font-size: 50px;"> </i>';
                                } else if (fileType == 'xls' || fileType == 'xlsx') {
                                    return '<i class="fas fa-file-excel fa-2x" style="color: green; font-size: 50px;"></i>';
                                } else if (fileType == 'ppt' || fileType == 'pptx') {
                                    return '<i class="fas fa-file-powerpoint fa-2x" style="color: orange; font-size: 50px;"></i>';
                                } else if (fileType == 'jpg' || fileType == 'png' || fileType == 'jpeg') {
                                    return '<img src="data:image/png;base64,' + data + '" class="avatar" width="250" height="250"/>';
                                } else {
                                    return '<i class="bi bi-images" style="font-size: 250px;"></i> ';
                                }
                            },
                            className: "text-center"

                        },
                        {
                            // download link by using anchor link only
                            data: "documentBlob",
                            render: function (data, type, row) {
                                var f = new Blob([s2ab(atob(data))], { type: "" });
                                var filename = row.imagePath.replace(/^.*[\\\/]/, '');
                                var newF = URL.createObjectURL(f);
                                urls.push(newF);
                                var fileType = row.imagePath.split('.').pop();
                                //    check file if is image type
                                if (fileType == 'jpg' || fileType == 'png' || fileType == 'jpeg' || fileType == 'pdf' || fileType == 'gif') {
                                    // return '<a href="/api/v1/download/' + row.id + '" class="btn btn-sm btn-success">Download</a>'
                                    //     +
                                    return '<a href="' + newF + '" class="btn btn-sm btn-success" download="' + filename + '">Download</a>';
                                }
                                else {
                                    return '';
                                }
                            },
                            className: "text-center"
                        },

                    ]
                });

            },
            error: function (data) {
                toastr.error("Failed")
            }

        })
        // alert(data);
    });
    $('#UploadList').on('click', '.delete', function () {
        var id = $(this).attr('data-id');
        bootbox.confirm({
            message: "Are you sure you want to delete this record?",
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-success btn-sm'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-danger btn-sm'
                }
            },
            callback: function (result) {
                if (result) {
                    $.ajax({
                        type: 'DELETE',
                        url: '/ServiceRequest/api/v1/deleteImage/' + id,
                        headers: {
                            "Authorization": "Bearer " + localStorage.getItem('access_token')
                        },
                        success: function (data) {
                            toastr.success("File Successfully Deleted!");
                            location.reload();
                        }
                    });
                }
            }
        });
    });

    //view
    //get RequestDetails
    $('#hardwareRequestList').on('click', '.view', function () {
        var id = $(this).attr('data-id');
        $("#generateReportBtn").attr('data-id', id);


        var url = '/ServiceRequest/api/hardwareRequest/GetRequestbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#hardwareRequestModal').modal('show');

                $('#EditRequest').find('input[name="id"]').val(data.id);
                $('#EditRequest').find('input[name="fullName"]').val(data.fullName);
                $('#EditRequest').find('input[name="mobileNumber"]').val(data.mobileNumber);
                $('#EditRequest').find('input[name="modelName"]').val(data.modelName);
                $('#EditRequest').find('select[name="departmentsId"]').val(data.departmentsId);
                $('#EditRequest').find('select[name="divisionsId"]').val(data.divisionsId);
                $('#EditRequest').find('select[name="hardwareId"]').val(data.hardwareId);
                $('#EditRequest').find('select[name="unitTypeId"]').val(data.unitTypeId);
                $('#EditRequest').find('input[name="brandName"]').val(data.brandName);
                $('#EditRequest').find('input[name="FileName"]').val(data.documentLabel);
                $('#EditRequest').find('textarea[name="description"]').val(data.description);
                $('#EditRequest').find('select[name="statusId"]').val(data.statusId);
                $('#EditRequest').find('select[name="findingId"]').val(data.findingId);
                $('#EditRequest').find('select[name="hardwareTechnicianId"]').val(data.hardwareTechnicianId);
                $('#EditRequest').find('input[name="possibleCause"]').val(data.possibleCause);
                $('#EditRequest').find('textarea[name="remarks"]').val(data.remarks);
                $('#EditRequest').find('input[name="dateStarted"]').val(data.dateStarted);
                $('#EditRequest').find('input[name="dateEnded"]').val(data.dateEnded);
                $('#EditRequest').find('input[name="timeStarted"]').val(data.timeStarted);
                $('#EditRequest').find('input[name="timeEnded"]').val(data.timeEnded);
                $('#EditRequest').find('input[name="approver"]').val(data.approver);
                $('#EditRequest').find('input[name="dateApproved"]').val(data.dateApproved);
                $('#EditRequest').find('input[name="viewer"]').val(data.viewer);
                $('#EditRequest').find('input[name="dateView"]').val(data.dateView);
                $('#EditRequest').find('input[name="serialNumber"]').val(data.serialNumber);
                $('#EditRequest').find('input[name="controlNumber"]').val(data.controlNumber);

            }
        })
        // alert(data);
    });

    //get id assign
    $('#hardwareRequestList').on('click', '.assign', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/hardwareRequest/GetRequestbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#StatusModal').modal('show');
                $('#assign').find('input[name="id"]').val(data.id);
                $('#assign').find('select[name="statusId"]').val("");

                //alert(data.statusId);
                if (data.statusId == 5)
                    $('.update-status').prop('disabled', true)
                else
                    $('.update-status').prop('disabled', false)
            }
        })
        // alert(data);
    });
    //submit assign
    $('#updateStatus').click(function (e) {
        e.preventDefault();
        var id = $('input[name=id]').val();
        var data = $('#assign').serialize();
        $.ajax({
            type: 'PUT',
            url: '/ServiceRequest/api/hardwareRequest/updateStatus/' + id,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                $('#StatusModal').modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success(" Successfully Update Status!");
                    // hide please wait modal

                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            //if failed
            error: function (data) {
                //alert(id);
                toastr.error("Please Update Status!")
            }
        });
    });


    $('#hardwareRequestList').on('click', '.technician', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/hardwareRequest/GetRequestbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#TechnicianModal').modal('show');
                $('#assignTech').find('input[name="id"]').val(data.id);
                //alert(data.statusId);
             

            }
        })
    });


    $('#updateTechnician').click(function (e) {
        var id = $('input[name=id]').val();
        //var data = $('#assignTech').serialize();
        var hardwareTechnicianId = $('#assignTech').find('select[name=hardwareTechnicianId]').val();
        var hardwareRequestId = $('#assignTech').find('input[name=id]').val();
        var data = {
            "id": hardwareRequestId,
            "hardwareTechnicianId": hardwareTechnicianId
        };
        $.ajax({
            type: 'PUT',
            url: '/ServiceRequest/api/hardwareRequest/updateTechnician/' + hardwareRequestId,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                $("#TechnicianModal").modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success("Successfully Assign Technician!");
                    // hide please wait modal
                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            //if failed
            error: function (data) {
                //alert(id);
                toastr.error("Please Assign Technician")
            }
        });
    });

    //Get id
    $('#hardwareRequestList').on('click', '.edit', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/hardwareRequest/GetRequestbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#TechnicianForm').modal('show');
                $('#techForm').find('input[name="id"]').val(data.id);
                $('#techForm').find('input[name="statusId"]').val(data.statusId);
                $('#techForm').find('select[name="findingId"]').val("");
                $('#techForm').find('input[name="possibleCause"]').val("");
                $('#techForm').find('input[name="dateStarted"]').val("");
                $('#techForm').find('input[name="dateEnded"]').val("");
                $('#techForm').find('input[name="timeStarted"]').val("");
                $('#techForm').find('input[name="timeEnded"]').val("");
                $('#techForm').find('textarea[name="remarks"]').val("");
                $('#techForm').find('input[name=controlNumber]').val("");
                $('#techForm').find('input[name=serialNumber]').val("");
                $('#techForm').find('input[name=brandName]').val(data.brandName);
                $('#techForm').find('input[name=modelName]').val(data.modelName);
                $('#techForm').find('select[name=unitTypeId]').val(data.unitTypeId);
                $('#techForm').find('select[name=hardwareId]').val(data.hardwareId);

            }
        })
        // alert(data);
    });

    $('#UpdateRecord').click(function (e) {
        e.preventDefault();

        //var id = $('input[name=id]').val();
        //var data = $('#techForm').serialize();
        var dataId = $('#techForm').find('input[name=id]').val();
        var findingId = $('#techForm').find('select[name=findingId]').val();
        var possibleCause = $('#techForm').find('input[name=possibleCause]').val();
        var dateStarted = $('#techForm').find('input[name=dateStarted]').val();
        var dateEnded = $('#techForm').find('input[name=dateEnded]').val();
        var remarks = $('#techForm').find('textarea[name=remarks]').val();
        var timeStarted = $('#techForm').find('input[name=timeStarted]').val();
        var timeEnded = $('#techForm').find('input[name=timeEnded]').val();
        var serialNumber = $('#techForm').find('input[name=serialNumber]').val();
        var controlNumber = $('#techForm').find('input[name=controlNumber]').val();
        var brandName = $('#techForm').find('input[name=brandName]').val();
        var modelName = $('#techForm').find('input[name=modelName]').val();
        var unitTypeId = $('#techForm').find('select[name=unitTypeId]').val();
        var hardwareId = $('#techForm').find('select[name=hardwareId]').val();

        var data = {
            "id": dataId,
            "findingId": findingId,
            "possibleCause": possibleCause,
            "dateStarted": dateStarted,
            "dateEnded": dateEnded,
            "remarks": remarks,
            "timeStarted": timeStarted,
            "timeEnded": timeEnded,
            "serialNumber": serialNumber,
            "controlNumber": controlNumber,
            "brandName": brandName,
            "modelName": modelName,
            "unitTypeId": unitTypeId,
            "hardwareId": hardwareId,
        };

        $.ajax({
            type: 'PUT',
            url: '/ServiceRequest/api/hardwareRequest/TechnicianForm/' + dataId,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                $('#TechnicianForm').modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success("Request Successfully Reported");
                    // hide please wait modal

                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            //if failed
            error: function (data) {
                //alert(id);
                toastr.error("Fill up alll forms / Invalid")
            }
        });
    });


    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/department',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=departmentsId]').append('<option value=""> Select Department</option>');
            $.each(data, function (index, value) {
                $('select[name=departmentsId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/division',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=divisionsId]').append('<option value=""> Select Division</option>');
            $.each(data, function (index, value) {
                $('select[name=divisionsId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/hardware',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=hardwareId]').append('<option value=""> Select Hardware Service Category</option>');
            $.each(data, function (index, value) {
                $('select[name=hardwareId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    //GET findings
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/Findings',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=findingId]').append('<option value=""> Select Result</option>');
            $.each(data, function (index, value) {
                $('select[name=findingId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    //GET technician
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/Technician',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=hardwareTechnicianId]').append('<option value=""> Select Technician</option>');
            $.each(data, function (index, value) {
                $('select[name=hardwareTechnicianId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    //status
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/status',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=statusId]').append('<option value=""> Select Status </option>');
            $.each(data, function (index, value) {
                $('select[name=statusId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/unitType',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=unitTypeId]').append('<option value=""> Select Status </option>');
            $.each(data, function (index, value) {
                $('select[name=unitTypeId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
}

var urls = [];
function HardwareSuperAdmin() {
    var tables;

    var table2 = $("#hardwareRequestList").DataTable({
        "ajax": {
            "url": "/ServiceRequest/api/all/hardReq",
            "headers": {
                "Authorization": "Bearer " + localStorage.getItem('access_token')
            },
            "type": "GET",
            "dataType": "json",
        },
        "processing": "true",
        "serverSide": "true",
        "ordering": "true",
        "paging": "true",
        "fnInitComplete": function (oSettings, json) {
            // search after pressing enter
            $('#hardwareRequestList_filter input').unbind();
            $('#hardwareRequestList_filter input').bind('keyup', function (e) {
                if (e.keyCode == 13) {
                    // trigger search 
                    $('#pleasewait').modal('show');
                    setTimeout(function () {
                        $('#pleasewait').modal('hide');
                    }, 2500);
                    $('#hardwareRequestList').DataTable().search(this.value).draw();
                }
                // if search is empty, reset the filter
                if (this.value == "") {
                    $('#hardwareRequestList').DataTable().search("").draw();
                }
            }
            );
        },
        // "searching" : " true",
        // "searchDelay" : 1000,
        // order by desc dateuploaded
        "order": [[0, "desc"]],
        "language": {
            "processing": "Loading... Please wait"

        },
        "autoWidth": false,
        "columns": [
            {
                data: "ticket",
                render: function (data, type, row) {
                    if (data == null) {
                        return '<span class="badge bg-danger">No Ticket</span>';
                    }
                    return `<span class="badge bg-info" style="font-size:"12px;"">${(data)}</span>`
                }
            },
            {
                data: "dateAdded"
            },

            { data: "fullName" },

            {
                data: "statusId", render: function (data, type, row) {
                    if (data == 1) {
                        return '<span class="badge bg-secondary">Open</span>'
                    }
                    else if (data == 2) {
                        return '<span class="badge bg-success">Resolved</span>'
                    }
                    else if (data == 3) {
                        return '<span class="badge bg-warning">On hold</span>'
                    }
                    else if (data == 4) {
                        return '<span class="badge bg-info">In Progress</span>'
                    }
                    else if (data == 5) {
                        return '<span class="badge  bg-danger">Cancel Request</span>'
                    }
                    else
                        return '<span class="badge  bg-secondary">Open</span>'
                }
            },
            {
                "data": "dateView", render: function (data, type, row) {
                    if (data == null) {
                        return ' -- '
                    }
                    else
                        return `<span>${moment(data).format("D/M/Y LT")}</span>`;

                }
            },
            {
                "data": "viewer", render: function (data, type, row) {
                    if (data == null) {
                        return ''
                    }
                    else
                        return `<span>${(data)}</span>`
                }
            },
            {
                "data": "dateApproved", render: function (data, type, row) {
                    if (data == null) {
                        return '--'
                    }
                    else
                        return `<span>${moment(data).format("D/M/Y LT")}</span>`;
                }
            },

            {
                data: null,
                render: function (data) {
                    return '<button class="btn btn-info text-light btn-sm request ms-1 mb-1" data-id="' + data.id + '"><i class="fas fa-eye"></i> View Details</button>'
                        + '<button class="btn btn-success text-light btn-sm attach ms-1 mb-1" data-id="' + data.id + '"><i class="fas fa-paperclip"></i> Attachments </button>'
                        + '<button class="btn btn-primary text-light btn-sm ms-1 mb-1 viewedby" data-id="' + data.id + '"><i class="fas fa-user-check"></i> Approved </button>'
                },
                orderable: false,
                width: "360px",
            },

        ],
    });
    $('#hardwareRequestList').on('click', '.attach', function () {
        var id = $(this).attr('data-id');
        // alert(id);
        var url = '/ServiceRequest/api/hardwareRequest/GetRequestbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data, value) {
                $('#hardwareDisplay').modal('show');
                $('#hardwareDisplay').find('.modal-title').text('Add Image');
                var refId = $('#hardwareRequestId').val(data.id);
                //alert(data.id);
                //alert(data.statusId);
                var uploadTable = $('#UploadList').DataTable({
                    destroy: true,

                    ajax: {
                        url: "/ServiceRequest/api/v2/uploadDislpay/" + id,
                        dataSrc: '',
                        headers: {
                            "Authorization": "Bearer " + localStorage.getItem('access_token')
                        }
                    },
                    searching: false,
                    ordering: false,
                    columns: [
                        {
                            data: "dateAdded"
                        },

                        {
                            data: "documentBlob",
                            render: function (data, type, row, meta) {
                                var fileType = row.imagePath.split('.').pop();
                                if (fileType == 'pdf') {
                                    return '<i class="fas fa-file-pdf fa-2x" style="color: red; font-size: 50px;"></i>';
                                } else if (fileType == 'doc' || fileType == 'docx') {
                                    return '<i class="fas fa-file-word fa-2x" style="color: blue; font-size: 50px;"> </i>';
                                } else if (fileType == 'xls' || fileType == 'xlsx') {
                                    return '<i class="fas fa-file-excel fa-2x" style="color: green; font-size: 50px;"></i>';
                                } else if (fileType == 'ppt' || fileType == 'pptx') {
                                    return '<i class="fas fa-file-powerpoint fa-2x" style="color: orange; font-size: 50px;"></i>';
                                } else if (fileType == 'jpg' || fileType == 'png' || fileType == 'jpeg') {
                                    return '<img src="data:image/png;base64,' + data + '" class="avatar" width="250" height="250"/>';
                                } else {
                                    return '<i class="bi bi-images" style="font-size: 250px;"></i> ';
                                }
                            },
                            className: "text-center"

                        },
                        {
                            // download link by using anchor link only
                            data: "documentBlob",
                            render: function (data, type, row) {
                                var f = new Blob([s2ab(atob(data))], { type: "" });
                                var filename = row.imagePath.replace(/^.*[\\\/]/, '');
                                var newF = URL.createObjectURL(f);
                                urls.push(newF);
                                var fileType = row.imagePath.split('.').pop();
                                //    check file if is image type
                                if (fileType == 'jpg' || fileType == 'png' || fileType == 'jpeg' || fileType == 'pdf' || fileType == 'gif') {
                                    // return '<a href="/api/v1/download/' + row.id + '" class="btn btn-sm btn-success">Download</a>'
                                    //     +
                                    return '<a href="' + newF + '" class="btn btn-sm btn-success" download="' + filename + '">Download</a>'
                                        +
                                        '<a href="' + newF + '" target="_blank" class="btn btn-primary btn-sm" style="margin: 2%;">View</a>';
                                }
                                else {
                                    return '';
                                }
                            },
                            className: "text-center"
                        },
                        {
                            data: null,
                            render: function (data, type, row) {
                                return '<button class="btn btn-danger text-light btn-sm delete ms-1 mb-1" data-id="' + row.id + '"><i class="fas fa-eye"></i> Delete </button>'
                            },
                            className: "text-center"

                        }

                    ]
                })
            }
        })
        // alert(data);
    });

    $('#UploadList').on('click', '.delete', function () {
        var id = $(this).attr('data-id');
        $('#hardwareDisplay').modal('hide');

        bootbox.confirm({
            message: "Are you sure you want to delete this record?",
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-success btn-sm'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-danger btn-sm'
                }
            },
            callback: function (result) {
                if (result) {
                    $.ajax({
                        type: 'DELETE',
                        url: '/ServiceRequest/api/v1/deleteImage/' + id,
                        headers: {
                            "Authorization": "Bearer " + localStorage.getItem('access_token')
                        },
                        success: function (data) {
                            toastr.success("Data Successfully Deleted!");
                            window.location.reload();
                        }
                    });
                }
            }
        });
    });


    //get RequestDetails
    $('#hardwareRequestList').on('click', '.request', function () {
        var id = $(this).attr('data-id');
        $("#generateReportBtn").attr('data-id', id);


        var url = '/ServiceRequest/api/hardwareRequest/GetRequestbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#hardwareRequestModal').modal('show');

                $('#EditRequest').find('input[name="id"]').val(data.id);
                $('#EditRequest').find('input[name="fullName"]').val(data.fullName);
                $('#EditRequest').find('input[name="dateAdded"]').val(data.dateAdded);
                $('#EditRequest').find('input[name="FileName"]').val(data.documentLabel);

                $('#EditRequest').find('input[name="mobileNumber"]').val(data.mobileNumber);
                $('#EditRequest').find('input[name="modelName"]').val(data.modelName);
                $('#EditRequest').find('select[name="departmentsId"]').val(data.departmentsId);
                $('#EditRequest').find('select[name="divisionsId"]').val(data.divisionsId);
                $('#EditRequest').find('select[name="hardwareId"]').val(data.hardwareId);
                $('#EditRequest').find('select[name="unitTypeId"]').val(data.unitTypeId);
                $('#EditRequest').find('input[name="brandName"]').val(data.brandName);
                $('#EditRequest').find('textarea[name="description"]').val(data.description);
                $('#EditRequest').find('select[name="statusId"]').val(data.statusId);
                $('#EditRequest').find('select[name="findingId"]').val(data.findingId);
                $('#EditRequest').find('select[name="hardwareTechnicianId"]').val(data.hardwareTechnicianId);
                $('#EditRequest').find('input[name="possibleCause"]').val(data.possibleCause);
                $('#EditRequest').find('textarea[name="remarks"]').val(data.remarks);
                $('#EditRequest').find('input[name="dateStarted"]').val(data.dateStarted);
                $('#EditRequest').find('input[name="dateEnded"]').val(data.dateEnded);
                $('#EditRequest').find('input[name="timeStarted"]').val(data.timeStarted);
                $('#EditRequest').find('input[name="timeEnded"]').val(data.timeEnded);
                $('#EditRequest').find('input[name="approver"]').val(data.approver);
                $('#EditRequest').find('input[name="dateApproved"]').val(data.dateApproved);
                $('#EditRequest').find('input[name="serialNumber"]').val(data.serialNumber);
                $('#EditRequest').find('input[name="controlNumber"]').val(data.controlNumber);
                $('#EditRequest').find('input[name="viewer"]').val(data.viewer);
                $('#EditRequest').find('input[name="dateView"]').val(data.dateView);
            }
        })
        // alert(data);
    });

    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/department',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=departmentsId]').append('<option value=""> Select Department</option>');
            $.each(data, function (index, value) {
                $('select[name=departmentsId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/UnitType',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=unitTypeId]').append('<option value=""> Select UnitType</option>');
            $.each(data, function (index, value) {
                $('select[name=unitTypeId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/division',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=divisionsId]').append('<option value=""> Select Division</option>');
            $.each(data, function (index, value) {
                $('select[name=divisionsId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/hardware',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=hardwareId]').append('<option value=""> Select Hardware Service Category</option>');
            $.each(data, function (index, value) {
                $('select[name=hardwareId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/Technician',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=hardwareTechnicianId]').append('<option value=""> Select Technician</option>');
            $.each(data, function (index, value) {
                $('select[name=hardwareTechnicianId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });

    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/Findings',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=findingId]').append('<option value=""> Select Result</option>');
            $.each(data, function (index, value) {
                $('select[name=findingId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    //status
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/status',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=statusId]').append('<option value="0">Open </option>');
            $.each(data, function (index, value) {
                $('select[name=statusId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });

    $('#hardwareRequestList').on('click', '.viewedby', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/hardwareRequest/GetRequestbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#ViewedByModal').modal('show');
                $('#viewed').find('input[name="id"]').val(data.id);
                $('#viewed').find('input[name="isVerified"]').val(data.isVerified);
                $('#viewed').find('textarea[name="viewedRemarks"]').val(data.viewedRemarks);

                //alert(data.isApproved);
                if (data.isVerified == true)
                    $('.update-viewed').prop('disabled', false)

                else
                    $('.update-viewed').prop('disabled', true)
                //alert(id);

            }
        })
    });

    $('#ViewwdBySubmit').click(function (e) {
        e.preventDefault();

        var id = $('#viewed').find('input[name="id"]').val();
        var data = $('#viewed').serialize();

        $.ajax({
            type: 'PUT',
            url: '/ServiceRequest/api/hardwareRequest/viewer/' + id,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                $('#ViewedByModal').modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success(" Successfully Approved!");
                    // hide please wait modal

                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            //if failed
            error: function (data) {
                toastr.error("Fill up Remarks")
            }
        });
    });

    $('#hardwareRequestList').on('click', '.approvedby', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/hardwareRequest/GetRequestbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#ApprovedByModal').modal('show');
                $('#approve').find('input[name="id"]').val(data.id);
                $('#approve').find('input[name="approvedBy"]').val("Jose Pepito G. Bonete");
                $('#approve').find('select[name="statusId"]').val(data.statusId);
                $('#approve').find('textarea[name="approveRemarks"]').val(data.approveRemarks);
            }
        })
    });


}

function GetHardwareServices() {
    var Datatables;
    Datatables = $("#hardwareList").DataTable({
        ajax: {
            url: "/ServiceRequest/api/hard/get",
            dataSrc: "",
            headers: {
                "Authorization": "Bearer " + localStorage.getItem('access_token')
            }
        },
        ordering: false,
        autoWidth: false,
        columns: [

            {
                data: "name",
            },
            {
                data: null,
                render: function (data) {
                    return '<button class="btn btn-primary btn-sm request" data-id="' + data.id + '"><i class="far fa-folder"></i> Request</button>'
                },
                orderable: false,
            },
            {
                data: null,
                render: function (data) {
                    return '<button class="btn btn-success btn-sm edit" data-id="' + data.id + '"><i class="fas fa-edit"></i> Edit</button>'
                        + '  <button class="btn btn-danger btn-sm delete" data-id="' + data.id + '"><i class="fas fa-trash"></i> Delete</button>'
                },
                orderable: false,
            }

        ]
    });

    $('#btnSubmit').click(function (e) {
        e.preventDefault();

        //find name
        var name = $('input[name=name]').val();
        // data
        var data = {
            name: name,
        };
        $.ajax({
            type: 'POST',
            url: '/ServiceRequest/api/hard/save',
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                $("#name").val("");
                $('#AddHardware').modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success("Data Successfully Added!");
                    // hide please wait modal
                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            //if failed
            error: function (data) {
                toastr.error("Name Already Exist In the Database/ Invalid")
            }
        });
    });

    //Get edit
    $("#hardwareList").on('click', '.edit', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/hard/gethardware/' + id;
        // alert(id);
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $("#editmodal").modal('show');
                $('#edit').find('input[name="id"]').val(data.id);
                $('#edit').find('input[name="name"]').val(data.name);
            },
            //if failed
            error: function (data) {
                // console.log(data, data.id, data.name);
                toastr.error("Error")
            }
        })
    })

    //Update
    $("#updateDate").click(function (e) {
        e.preventDefault();
        var data = {
            name: $('#edit').find('input[name=name]').val(),
        };
        var id = $('#edit').find('input[name="id"]').val();
        $.ajax({
            type: 'PUT',
            url: '/ServiceRequest/api/hard/updateHardware/' + id,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                $('#editmodal').modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success("Data Successfully Updated!");
                    // hide please wait modal
                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            //if failed
            error: function (data) {
                toastr.error("Error")
            }
        });
    });

    $('#hardwareList').on('click', '.delete', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/hard/Delete/' + id;
        $.ajax({
            // confirm before delete
            beforeSend: function (xhr) {
                // use bootbox
                bootbox.confirm({
                    message: "Are you sure you want to delete this record?",
                    buttons: {
                        // yes and no sequence

                        confirm: {
                            label: 'Yes',
                            className: 'btn-success btn-sm'
                        },
                        cancel: {
                            label: 'No',
                            className: 'btn-danger btn-sm'
                        }
                    },
                    callback: function (result) {
                        if (result) {
                            $.ajax({
                                type: 'DELETE',
                                url: url,
                                headers: {
                                    'Authorization': 'Bearer ' + localStorage.getItem('access_token')
                                },
                                success: function (data) {
                                    $('#pleasewait').modal('show');
                                    //show toastr after 3
                                    setTimeout(function () {
                                        toastr.success("File Successfully Deleted!");
                                        // hide please wait modal
                                    }, 2000);
                                    setTimeout(function () {
                                        window.location.reload();
                                    }, 3000);

                                },
                                //if failed
                                error: function (data) {
                                    toastr.error("Error")
                                }
                            });
                        }
                    }
                });

            },
        });
    });

    //Get id hardware Request
    $('#hardwareList').on('click', '.request', function () {
        var data = $(this).attr('data-id');
        $('#hardwareeRequestModal').modal('show');
        $('#hardwareeRequestModal').find('input[name=hardwareId]').val(data);
        $('#hardwareeRequestModal').find('select[name=unitTypeId]').val("-1");
        $('#hardwareeRequestModal').find('input[name=brandName]').val("");
        $('#hardwareeRequestModal').find('input[name=modelName]').val("");

        // alert(data);
    });

    $('#btnRequest').click(function (e) {
        e.preventDefault();
        var id = $('input[name=hardwareId]').val();
        var valdata = $('#SaveRequest').serialize();
        $.ajax({
            type: 'POST',
            url: '/ServiceRequest/api/hardwareRequest/SaveRequest',
            data: valdata,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                $("#hardwareeRequestModal").modal('hide');
                $('#description').val(" ");
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success("Data Successfully Added!");
                    // hide please wait modal
                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            //if failed
            error: function (data) {
                //   alert(id);
                toastr.error("Fill up alll forms / Invalid")
            }
        });
    });


    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/unitType',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=unitTypeId]').append('<option value="-1"> Select UnitType</option>');
            $.each(data, function (index, value) {
                $('select[name=unitTypeId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
}

function UserHardwareServices() {

    var Datatables;
    Datatables = $("#hardwareList").DataTable({
        ajax: {
            url: "/ServiceRequest/api/hard/get",
            dataSrc: "",
            headers: {
                "Authorization": "Bearer " + localStorage.getItem('access_token')
            }
        },
        ordering: false,
        autoWidth: false,
        columns: [

            {
                data: "name",
            },

            {
                data: null,
                render: function (data) {
                    return '<button class="btn btn-primary btn-sm request" data-id="' + data.id + '"><i class="far fa-folder"></i> Request</button>'
                },
                orderable: false,
            }

        ]
    });


    //Get id Software Request
    $('#hardwareList').on('click', '.request', function () {
        var data = $(this).attr('data-id');
        $('#hardwareeRequestModal').modal('show');
        $('#hardwareeRequestModal').find('input[name=hardwareId]').val(data);
        $('#hardwareeRequestModal').find('select[name=unitTypeId]').val("-1");
        $('#hardwareeRequestModal').find('input[name=brandName]').val("");
        $('#hardwareeRequestModal').find('input[name=modelName]').val("");
        $('#hardwareeRequestModal').find('textarea[name=description]').val("");
        $('#hardwareeRequestModal').find('input[name=FileName]').val("");

        // alert(data);
    });

    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/unitType',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=unitTypeId]').append('<option value="-1"> Select UnitType</option>');
            $.each(data, function (index, value) {
                $('select[name=unitTypeId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
}

function GetTechnician() {
    var Datatables;
    Datatables = $("#hardwareList").DataTable({
        ajax: {
            url: "/ServiceRequest/api/htech/GetTech",
            dataSrc: "",
            headers: {
                "Authorization": "Bearer " + localStorage.getItem('access_token')
            }
        },
        autoWidth: false,
        columns: [

            {
                data: "dateAdded",
            },
            {
                data: "name",
            },
            {
                data: "email"
            },
            {
                data: null,
                render: function (data) {
                    return '<button class="btn btn-success btn-sm edit" data-id="' + data.id + '"><i class="fas fa-edit"></i> Edit</button>'
                        + '  <button class="btn btn-danger btn-sm delete" data-id="' + data.id + '"><i class="fas fa-trash"></i> Delete</button>'
                },
                "orderable": false,
            }

        ]
    });

    $('#btnSubmit').click(function (e) {
        e.preventDefault();

        //find name
        var name = $('input[name=name]').val();
        var email = $('input[name=email]').val();

        // data
        var data = {
            name: name,
            email: email
        };
        $.ajax({
            type: 'POST',
            url: '/ServiceRequest/api/htech/save',
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                $("#hardwareTechModal").modal('hide');

                $("#name").val("");
                $('#editmodal').modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success("Data Successfully Added!");
                    // hide please wait modal
                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            //if failed
            error: function (data) {
                toastr.error("Name Already Exist In the Database/ Invalid")
            }
        });
    });

    //Get edit
    $("#hardwareList").on('click', '.edit', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/htech/getTechbyId/' + id;
        //alert(id);
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $("#editmodal").modal('show');
                $('#edit').find('input[name="id"]').val(data.id);
                $('#edit').find('input[name="name"]').val(data.name);
                $('#edit').find('input[name="email"]').val(data.email);

            },
            //if failed
            error: function (data) {
                // console.log(data, data.id, data.name);
                toastr.error("Error")
            }
        })
    })


    $("#Updatetech").click(function (e) {
        e.preventDefault();
        var data = {
            name: $('#edit').find('input[name=name]').val(),
            email: $('#edit').find('input[name=email]').val(),

        };
        var id = $('#edit').find('input[name="id"]').val();
        $.ajax({
            type: 'PUT',
            url: '/ServiceRequest/api/htech/updateHardwareTech/' + id,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                $('#editmodal').modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success("Data Successfully Updated!");
                    // hide please wait modal
                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            //if failed
            error: function (data) {
                toastr.error("Error")
            }
        });
    });

    $('#hardwareList').on('click', '.delete', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/htech/Delete/' + id;
        $.ajax({
            // confirm before delete
            beforeSend: function (xhr) {
                // use bootbox
                bootbox.confirm({
                    message: "Are you sure you want to delete this record?",
                    buttons: {
                        // yes and no sequence

                        confirm: {
                            label: 'Yes',
                            className: 'btn-success btn-sm'
                        },
                        cancel: {
                            label: 'No',
                            className: 'btn-danger btn-sm'
                        }
                    },
                    callback: function (result) {
                        if (result) {
                            $.ajax({
                                type: 'DELETE',
                                url: url,
                                headers: {
                                    'Authorization': 'Bearer ' + localStorage.getItem('access_token')
                                },
                                success: function (data) {
                                    //show please wait modal
                                    $('#pleasewait').modal('show');
                                    //show toastr after 3
                                    setTimeout(function () {
                                        toastr.success("Data Successfully Deleted!");
                                        // hide please wait modal
                                    }, 2000);
                                    setTimeout(function () {
                                        window.location.reload();
                                    }, 3000);
                                },
                                //if failed
                                error: function (data) {
                                    toastr.error("Error")
                                }
                            });
                        }
                    }
                });

            },
        });
    });
}

function GetSoftwareTechnician() {
    var Datatables;
    Datatables = $("#softwaretechList").DataTable({
        ajax: {
            url: "/ServiceRequest/api/stech/Gettech",
            dataSrc: "",
            headers: {
                "Authorization": "Bearer " + localStorage.getItem('access_token')
            }
        },
        autoWidth: false,
        columns: [

            {
                data: "dateAdded",
            },
            {
                data: "name",
            },
            {
                data: "techEmail"
            }
        ]
    });

    $('#btnSubmit').click(function (e) {
        e.preventDefault();

        //find name
        var name = $('input[name=name]').val();
        // data
        var data = {
            name: name,
        };
        $.ajax({
            type: 'POST',
            url: '/ServiceRequest/api/stech/save',
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                $("#name").val("");
                $('#Stechnicianmodal').modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success("Data Successfully Added!");
                    // hide please wait modal
                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            //if failed
            error: function (data) {
                toastr.error("Name Already Exist In the Database/ Invalid")
            }
        });
    });

    //Get edit
    $("#softwaretechList").on('click', '.edit', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/stech/getTechbyId/' + id;
        // alert(id);
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $("#editmodal").modal('show');
                $('#edit').find('input[name="id"]').val(data.id);
                $('#edit').find('input[name="name"]').val(data.name);
            },
            //if failed
            error: function (data) {
                // console.log(data, data.id, data.name);
                toastr.error("Error")
            }
        })
    })

    $("#Updatetech").click(function (e) {
        e.preventDefault();
        var data = {
            name: $('#edit').find('input[name=name]').val(),
        };
        var id = $('#edit').find('input[name="id"]').val();
        $.ajax({
            type: 'PUT',
            url: '/ServiceRequest/api/stech/updateSoftwareTech/' + id,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {

                $('#editmodal').modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success("Data Successfully Updated!");
                    // hide please wait modal
                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            //if failed
            error: function (data) {
                toastr.error("Error")
            }
        });
    });

    $('#softwaretechList').on('click', '.delete', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/stech/Delete/' + id;
        $.ajax({
            // confirm before delete
            beforeSend: function (xhr) {
                // use bootbox
                bootbox.confirm({
                    message: "Are you sure you want to delete this record?",
                    buttons: {
                        // yes and no sequence

                        confirm: {
                            label: 'Yes',
                            className: 'btn-success btn-sm'
                        },
                        cancel: {
                            label: 'No',
                            className: 'btn-danger btn-sm'
                        }
                    },
                    callback: function (result) {
                        if (result) {
                            $.ajax({
                                type: 'DELETE',
                                url: url,
                                headers: {
                                    'Authorization': 'Bearer ' + localStorage.getItem('access_token')
                                },
                                success: function (data) {
                                    //show please wait modal
                                    $('#pleasewait').modal('show');
                                    //show toastr after 3
                                    setTimeout(function () {
                                        toastr.success("Data Successfully Deleted!");
                                        // hide please wait modal
                                    }, 2000);
                                    setTimeout(function () {
                                        window.location.reload();
                                    }, 3000);
                                },
                                //if failed
                                error: function (data) {
                                    toastr.error("Error")
                                }
                            });
                        }
                    }
                });

            },
        });
    });
}

function SoftwareServices() {
    var Datatables;
    Datatables = $("#softwareList").DataTable({
        ajax: {
            url: "/ServiceRequest/api/soft/get",
            dataSrc: "",
            headers: {
                "Authorization": "Bearer " + localStorage.getItem('access_token')
            }
        },
        ordering: false,
        autoWidth: false,
        columns: [

            {
                data: "name",
            },

            {
                data: null,
                render: function (data) {
                    return '<button class="btn btn-primary btn-sm request" data-id="' + data.id + '"><i class="far fa-folder"></i> Request</button>'
                },
                orderable: false,
            },
            {
                data: null,
                render: function (data) {
                    return '<button class="btn btn-success btn-sm edit" data-id="' + data.id + '"><i class="fas fa-edit"></i> Edit</button>'
                        + '  <button class="btn btn-danger btn-sm delete" data-id="' + data.id + '"><i class="fas fa-trash"></i> Delete</button>'
                },
                orderable: false,
            }

        ]
    });

    $('#btnSubmit').click(function (e) {
        e.preventDefault();

        //find name
        var name = $('input[name=name]').val();
        // data
        var data = {
            name: name,
        };
        $.ajax({
            type: 'POST',
            url: '/ServiceRequest/api/soft/save',
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {

                $("#AddSoftware").modal('hide');
                $("#name").val("");
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success("Data Successfully Added!");
                    // hide please wait modal
                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            //if failed
            error: function (data) {
                toastr.error("Name Already Exist In the Database/ Invalid")
            }
        });
    });

    //Get edit
    $("#softwareList").on('click', '.edit', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/soft/getsoftware/' + id;
        // alert(id);
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $("#editmodal").modal('show');
                $('#edit').find('input[name="id"]').val(data.id);
                $('#edit').find('input[name="name"]').val(data.name);
            },
            //if failed
            error: function (data) {
                // console.log(data, data.id, data.name);
                toastr.error("Error")
            }
        })
    })

    //Update
    $("#updateDate").click(function (e) {
        e.preventDefault();
        var data = {
            name: $('#edit').find('input[name=name]').val(),
        };
        var id = $('#edit').find('input[name="id"]').val();
        $.ajax({
            type: 'PUT',
            url: '/ServiceRequest/api/soft/updateSoftware/' + id,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                $('#editmodal').modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success("Data Successfully Updated!");
                    // hide please wait modal
                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            //if failed
            error: function (data) {
                toastr.error("Error")
            }
        });
    });

    $('#softwareList').on('click', '.delete', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/soft/Delete/' + id;
        $.ajax({
            // confirm before delete
            beforeSend: function (xhr) {
                // use bootbox
                bootbox.confirm({
                    message: "Are you sure you want to delete this record?",
                    buttons: {
                        // yes and no sequence

                        confirm: {
                            label: 'Yes',
                            className: 'btn-success btn-sm'
                        },
                        cancel: {
                            label: 'No',
                            className: 'btn-danger btn-sm'
                        }
                    },
                    callback: function (result) {
                        if (result) {
                            $.ajax({
                                type: 'DELETE',
                                url: url,
                                headers: {
                                    'Authorization': 'Bearer ' + localStorage.getItem('access_token')
                                },
                                success: function (data) {
                                    //show please wait modal
                                    $('#pleasewait').modal('show');
                                    //show toastr after 3
                                    setTimeout(function () {
                                        toastr.success("Data Successfully Deleted!");
                                        // hide please wait modal
                                    }, 2000);
                                    setTimeout(function () {
                                        window.location.reload();
                                    }, 3000);
                                },
                                //if failed
                                error: function (data) {
                                    toastr.error("Error")
                                }
                            });
                        }
                    }
                });

            },
        });
    });

    //Get id Software Request
    $('#softwareList').on('click', '.request', function () {
        var data = $(this).attr('data-id');
        $('#softwarerequest').modal('show');
        $('#softwarerequest').find('input[name=softwareId]').val(data);
        $('#softwarerequest').find('select[name=informationSystemId]').val("");
        $('#softwarerequest').find('input[name=requestFor]').val("");
        $('#softwarerequest').find('textarea[name=description]').val("");
        //alert(data);
    })

    //get Information Dropdown
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/info/System',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name = informationSystemId]').append('<option value=""> Select System</option>');
            $.each(data, function (index, value) {
                $('select[name=informationSystemId]').append('<option value="' + value.id + '">' + value.name + '</option>');

            })
        }
    });


    //save
    $('#btnRequest').click(function (e) {
        e.preventDefault();
        var id = $('input[name=softwareId]').val();
        var valdata = $('#SaveRequest').serialize();
        $.ajax({
            type: 'POST',
            url: '/ServiceRequest/api/softwareRequest/saveRequest',
            data: valdata,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                toastr.success("Directory Successfully Added!");
                //Datatables.ajax.reload();
                $("#softwarerequest").modal('hide');
                $('#description').val(" ");
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success("Data Successfully Updated!");
                    // hide please wait modal
                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            //if failed
            error: function (data) {
                // alert(id);
                toastr.error("Fill up alll forms / Invalid")
            }
        });
    });
}

function UserSoftwareRequestCategory() {
    var Datatables;
    Datatables = $("#softwareList").DataTable({
        ajax: {
            url: "/ServiceRequest/api/soft/get",
            dataSrc: "",
            headers: {
                "Authorization": "Bearer " + localStorage.getItem('access_token')
            }
        },
        ordering: false,
        autoWidth: false,
        columns: [

            {
                data: "name",
            },
            {
                data: null,
                render: function (data) {
                    return '<button class="btn btn-primary btn-sm request" data-id="' + data.id + '"><i class="far fa-folder"></i> Request</button>'
                },
                orderable: false,
            },

        ]
    });

    //$('#btnSubmit').click(function (e) {
    //    e.preventDefault();

    //    //find name
    //    var name = $('input[name=name]').val();
    //    // data
    //    var data = {
    //        name: name,
    //    };
    //    $.ajax({
    //        type: 'POST',
    //        url: '/ServiceRequest/api/soft/save',
    //        data: data,
    //        headers: {
    //            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
    //        },
    //        success: function (data) {
    //            $('#AddSoftware').modal('hide');
    //            //show please wait modal
    //            $('#pleasewait').modal('show');
    //            //show toastr after 3
    //            setTimeout(function () {
    //                toastr.success("Software Service Successfully Added!");
    //                // hide please wait modal
    //            }, 2000);
    //            setTimeout(function () {
    //                window.location.reload();
    //            }, 3000);
    //        },
    //        //if failed
    //        error: function (data) {
    //            toastr.error("Name Already Exist In the Database/ Invalid")
    //        }
    //    });
    //});

    ////Get edit
    //$("#softwareList").on('click', '.edit', function () {
    //    var id = $(this).attr('data-id');
    //    var url = '/ServiceRequest/api/soft/getsoftware/' + id;
    //    // alert(id);
    //    $.ajax({
    //        type: 'GET',
    //        url: url,
    //        success: function (data) {
    //            $("#editmodal").modal('show');
    //            $('#edit').find('input[name="id"]').val(data.id);
    //            $('#edit').find('input[name="name"]').val(data.name);
    //        },
    //        //if failed
    //        error: function (data) {
    //            // console.log(data, data.id, data.name);
    //            toastr.error("ErrorS")
    //        }
    //    })
    //})

    ////Update
    //$("#updateDate").click(function (e) {
    //    e.preventDefault();
    //    var data = {
    //        name: $('#edit').find('input[name=name]').val(),
    //    };
    //    var id = $('#edit').find('input[name="id"]').val();
    //    $.ajax({
    //        type: 'PUT',
    //        url: '/ServiceRequest/api/soft/updateSoftware/' + id,
    //        data: data,
    //        headers: {
    //            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
    //        },
    //        success: function (data) {
    //            $('#editmodal').modal('hide');
    //            $('#editmodal').modal('hide');
    //            //show please wait modal
    //            $('#pleasewait').modal('show');
    //            //show toastr after 3
    //            setTimeout(function () {
    //                toastr.success("Data Successfully Updated!");
    //                // hide please wait modal
    //            }, 2000);
    //            setTimeout(function () {
    //                window.location.reload();
    //            }, 3000);
    //        },
    //        //if failed
    //        error: function (data) {
    //            toastr.error("Error")
    //        }
    //    });
    //});

    //$('#softwareList').on('click', '.delete', function () {
    //    var id = $(this).attr('data-id');
    //    var url = '/ServiceRequest/api/soft/Delete/' + id;
    //    $.ajax({
    //        // confirm before delete
    //        beforeSend: function (xhr) {
    //            // use bootbox
    //            bootbox.confirm({
    //                message: "Are you sure you want to delete this record?",
    //                buttons: {
    //                    // yes and no sequence

    //                    confirm: {
    //                        label: 'Yes',
    //                        className: 'btn-success btn-sm'
    //                    },
    //                    cancel: {
    //                        label: 'No',
    //                        className: 'btn-danger btn-sm'
    //                    }
    //                },
    //                callback: function (result) {
    //                    if (result) {
    //                        $.ajax({
    //                            type: 'DELETE',
    //                            url: url,
    //                            headers: {
    //                                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
    //                            },
    //                            success: function (data) {

    //                                $('#editmodal').modal('hide');
    //                                //show please wait modal
    //                                $('#pleasewait').modal('show');
    //                                //show toastr after 3
    //                                setTimeout(function () {
    //                                    toastr.success("Data Successfully Deleted!");
    //                                    // hide please wait modal
    //                                }, 2000);
    //                                setTimeout(function () {
    //                                    window.location.reload();
    //                                }, 3000);
    //                            },
    //                            //if failed
    //                            error: function (data) {
    //                                toastr.error("Error")
    //                            }
    //                        });
    //                    }
    //                }
    //            });

    //        },
    //    });
    //});

    //Get id Software Request
    $('#softwareList').on('click', '.request', function () {
        var data = $(this).attr('data-id');
        $('#softwarerequest').modal('show');
        $('#softwarerequest').find('input[name=softwareId]').val(data);
        $('#softwarerequest').find('select[name=informationSystemId]').val("");
        $('#softwarerequest').find('input[name=requestFor]').val("");
        $('#softwarerequest').find('textarea[name=description]').val("");
        $('#softwarerequest').find('input[name=documentLabel]').val("");

        // alert(data);
    })

    //get Information Dropdown
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/info/System',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name = informationSystemId]').append('<option value=""> Select System</option>');
            $.each(data, function (index, value) {
                $('select[name=informationSystemId]').append('<option value="' + value.id + '">' + value.name + '</option>');

            })
        }
    });
}

function SoftwareAdmin2() {
    var tables;
    tables = $('#softwareRequestList').DataTable({
        ajax: {
            url: "/ServiceRequest/api/softwareRequest/requestTech",
            dataSrc: "",
            headers: {
                "Authorization": "Bearer" + localStorage.getItem('access_token')
            }
        },
        autoWidth: false,
        ordering: false,
        columns: [
            {
                data: "ticket",
                render: function (data, type, row) {
                    if (data == null) {
                        return '<span class="badge bg-danger">No Ticket</span>';
                    }
                    return `<span class="badge bg-info">${(data)}</span>`
                }

            },
            { data: "dateAdded" },
            { data: "fullName" },
            {
                data: "softwareId",
                render: function (data, type, row) {
                    return row.software.name;
                }
            },
            {
                data: "statusId", render: function (data, type, row) {
                    if (data == 1) {
                        return '<span class="badge  bg-secondary">Open</span>'
                    }
                    else if (data == 2) {
                        return '<span class="badge  bg-success">Resolved</span>'
                    }
                    else if (data == 3) {
                        return '<span class="badge bg-warning">On hold</span>'
                    }
                    else if (data == 4) {
                        return '<span class="badge bg-info">In Progress</span>'
                    }
                    else if (data == 5) {
                        return '<span class="badge  bg-danger">Cancel Request</span>'
                    }
                    else
                        return '<span class="badge bg-secondary">Open</span>'
                }
            },
            {
                data: "dateApproved", render: function (data, type, row) {
                    if (data == null) {
                        return '<span class="badge  bg-primary">waiting to verified</span>'
                    }
                    else
                        return `<span>${moment(data).format("D/M/Y LT")}</span>`;
                }
            },
            {
                data: "dateView", render: function (data, type, row) {
                    if (data == null) {
                        return '<span class="badge  bg-primary">waiting to verified</span>'
                    }
                    else
                        return `<span>${moment(data).format("D/M/Y LT")}</span>`;
                }
            },
            {
                data: null,
                render: function (data) {
                    return '<button class="btn btn-info text-light btn-sm view ms-1 mb-1" data-id="' + data.id + '"><i class="fas fa-eye"></i> View Details </button>'
                        + '<button class="btn btn-success btn-sm approvedby ms-1 mb-1 " data-id="' + data.id + '"><i class="fas fa-user-edit"></i> Verified </button>'
                },
                orderable: false,
                width: "250px",
            },
        ]
    });


    //get Details
    $('#softwareRequestList').on('click', '.view', function () {
        var id = $(this).attr('data-id');
        //alert(id);
        var url = '/ServiceRequest/api/softwareRequest/GetbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#softwareDetailsModal').modal('show');

                $('#viewRequest').find('input[name="id"]').val(data.id);
                $('#viewRequest').find('input[name="fullName"]').val(data.fullName);
                $('#viewRequest').find('input[name="mobileNumber"]').val(data.mobileNumber);
                $('#viewRequest').find('input[name="modelName"]').val(data.modelName);
                $('#viewRequest').find('select[name="departmentsId"]').val(data.departmentsId);
                $('#viewRequest').find('select[name="divisionsId"]').val(data.divisionsId);
                $('#viewRequest').find('select[name="softwareId"]').val(data.softwareId);
                $('#viewRequest').find('select[name="informationSystemId"]').val(data.informationSystemId);
                $('#viewRequest').find('input[name="requestFor"]').val(data.requestFor);
                $('#viewRequest').find('textarea[name="description"]').val(data.description);
                $('#viewRequest').find('select[name="statusId"]').val(data.statusId);
                $('#viewRequest').find('select[name="softwareTechnicianId"]').val(data.softwareTechnicianId);
                $('#viewRequest').find('textarea[name="remarks"]').val(data.remarks);
                $('#viewRequest').find('input[name="dateStarted"]').val(data.dateStarted);
                $('#viewRequest').find('input[name="dateEnded"]').val(data.dateEnded);
                $('#viewRequest').find('input[name="timeStarted"]').val(data.timeStarted);
                $('#viewRequest').find('input[name="timeEnded"]').val(data.timeEnded);
                $('#viewRequest').find('input[name="approver"]').val(data.approver);
                $('#viewRequest').find('input[name="dateApproved"]').val(data.dateApproved);
                $('#viewRequest').find('textarea[name="approverRemarks"]').val(data.approverRemarks);
                $('#viewRequest').find('input[name="viewer"]').val(data.viewer);
                $('#viewRequest').find('input[name="dateView"]').val(data.dateView);
                $('#viewRequest').find('textarea[name="viewedRemarks"]').val(data.viewedRemarks);
            }
        })
        // alert(data);
    });


    //Department
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/department',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=departmentsId]').append('<option value=""> Select Department</option>');
            $.each(data, function (index, value) {
                $('select[name=departmentsId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });

    //Division
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/division',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=divisionsId]').append('<option value=""> Select Division</option>');
            $.each(data, function (index, value) {
                $('select[name=divisionsId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });


    //Software
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/software',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=softwareId]').append('<option value=""> Select Software</option>');
            $.each(data, function (index, value) {
                $('select[name=softwareId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });


    //Software
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/infoSystem',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=informationSystemId]').append('<option value=""> Select System</option>');
            $.each(data, function (index, value) {
                $('select[name=informationSystemId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },
    });
    //Status
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/status',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=statusId]').append('<option value=""> Select Status</option>');
            $.each(data, function (index, value) {
                $('select[name=statusId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    //Technician
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/Technician',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=softwareTechnicianId]').append('<option value=""> Select Technician</option>');
            $.each(data, function (index, value) {
                $('select[name=softwareTechnicianId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });



    //get id assign
    $('#softwareRequestList').on('click', '.assign', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/softwareRequest/GetbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#StatusModal').modal('show');
                $('#assign').find('input[name="id"]').val(data.id);
                $('#assign').find('select[name="statusId"]').val("");
            }
        })
        // alert(data);
    });


    //submit assign
    $('#updateStatus').click(function (e) {
        e.preventDefault();

        var id = $('input[name=id]').val();
        var data = $('#assign').serialize();
        $.ajax({
            type: 'PUT',
            url: '/ServiceRequest/api/softwareRequest/updateStatus/' + id,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {

                $('#editmodal').modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success("Successfully Updat Status!");
                    // hide please wait modal
                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            //if failed
            error: function (data) {
                // alert(id);
                toastr.error("Please update Status!")
            }
        });
    });



    $('#softwareRequestList').on('click', '.technician', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/softwareRequest/GetbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#TechnicianModal').modal('show');
                $('#assignTech').find('input[name="id"]').val(data.id);
                $('#assignTech').find('select[name="softwareTechnicianId"]').val("")
            }
        })
    });

    $('#updateTechnician').click(function (e) {
        var id = $('input[name=id]').val();

        //var data = $('#assignTech').serialize();
        var softwareTechnicianId = $('#assignTech').find('select[name="softwareTechnicianId"]').val();
        var softwareRequestId = $('#assignTech').find('input[name=id]').val();
        var data = {
            "id": softwareRequestId,
            "softwareTechnicianId": softwareTechnicianId
        };
        $.ajax({
            type: 'PUT',
            url: '/ServiceRequest/api/softwareRequest/UpdateTechnician/' + softwareRequestId,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success("Successfully Assign Technician!");
                    // hide please wait modal
                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            //if failed
            error: function (data) {
                //alert(id);
                toastr.error("Please Assign Technician")
            }
        });
    });


    $('#softwareRequestList').on('click', '.approvedby', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/softwareRequest/GetbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#ApprovedByModal').modal('show');
                $('#approve').find('input[name="id"]').val(data.id);
                $('#approve').find('select[name="statusId"]').val(data.statusId);
                $('#approve').find('textarea[name="approverRemarks"]').val(data.approverRemarks);

                if (data.statusId == 2)
                    $('.update-verified').prop('disabled', false)
                else
                    $('.update-verified').prop('disabled', true)
            }
        })
    });

    //AdminSaveRemarks
    $('#Approved').click(function (e) {
        e.preventDefault();
        var id = $('#approve').find('input[name="id"]').val();
        var data = $('#approve').serialize();
        $.ajax({
            type: 'PUT',
            url: '/ServiceRequest/api/softwareRequest/verify/' + id,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {

                $("#ApprovedByModal").modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success("Successfully Approved!");
                    // hide please wait modal
                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            //if failed
            error: function (data) {
                toastr.error("Fill up Remarks")
            }
        });
    });
}


function SoftwareAdmin() {
    var tables;
    $("#softwareRequestList").DataTable({
        "ajax": {
            "url": "/ServiceRequest/api/all/softReq",
            "headers": {
                "Authorization": "Bearer " + localStorage.getItem('access_token')
            },
            "type": "GET",
            "dataType": "json",
        },
        "processing": "true",
        "serverSide": "true",
        "ordering": "true",
        "paging": "true",
        "fnInitComplete": function (oSettings, json) {
            // search after pressing enter
            $('#softwareRequestList_filter input').unbind();
            $('#softwareRequestList_filter input').bind('keyup', function (e) {
                if (e.keyCode == 13) {
                    // trigger search 
                    $('#pleasewait').modal('show');
                    setTimeout(function () {
                        $('#pleasewait').modal('hide');
                    }, 2500);
                    $('#softwareRequestList').DataTable().search(this.value).draw();
                }
                // if search is empty, reset the filter
                if (this.value == "") {
                    $('#softwareRequestList').DataTable().search("").draw();
                }
            }
            );
        },
        // "searching" : " true",
        // "searchDelay" : 1000,
        // order by desc dateuploaded
        "order": [[0, "desc"]],
        "language": {
            "processing": "Loading... Please wait"

        },
        "autoWidth": false,
        "columns": [
            {
                data: "ticket",
                render: function (data, type, row) {
                    if (data == null) {
                        return '<span class="badge bg-danger">No Ticket</span>';
                    }
                    return `<span class="badge bg-info">${(data)}</span>`
                }
            },
            { data: "dateAdded" },
            { data: "fullName" },
            {
                data: "softwareId",
                render: function (data, type, row) {
                    return row.software.name;
                }
            },

            {
                data: "softwareTechnician.name",
                render: function (data, type, row) {
                    if (data == null) {
                        return '<span class="badge bg-success">Not Assigned</span>'
                    }
                    else
                        return row.softwareTechnician.name;
                }
            },
            {
                data: "statusId", render: function (data, type, row) {
                    if (data == 1) {
                        return '<span class="badge  bg-secondary">Open</span>'
                    }
                    else if (data == 2) {
                        return '<span class="badge  bg-success">Resolved</span>'
                    }
                    else if (data == 3) {
                        return '<span class="badge bg-warning">On hold</span>'
                    }
                    else if (data == 4) {
                        return '<span class="badge bg-info">In Progress</span>'
                    }
                    else if (data == 5) {
                        return '<span class="badge  bg-danger">Cancel Request</span>'
                    }
                    else
                        return '<span class="badge bg-secondary">Open</span>'
                }
            },
            {
                data: "isViewed", render: function (data, type, row) {
                    if (data == true) {
                        return '<i class="fas fa-check-circle fa-2x ms-4" style="color:green;"></i>'
                    }
                    else
                        return ''
                }
            },
            {
                data: null,
                render: function (data) {
                    return '<button class="btn btn-info text-light btn-sm view ms-1 mb-1 " data-id="' + data.id + '"><i class="fas fa-eye"></i> View  </button>'
                        + '<button class="btn btn-success text-light btn-sm attach ms-1 mb-1" data-id="' + data.id + '"><i class="fas fa-paperclip"></i> Attachments </button>'
                        + '<button class=" btn btn-primary btn-sm technician ms-1 mb-1 btn-Admin " data-id="' + data.id + '"><i class="fas fa-users-cog"></i> Technician</button>'
                        + '<button class="btn btn-secondary btn-sm approvedby text-center ms-1 mb-1 btn-verified  " data-id="' + data.id + '"><i class="fas fa-user-edit ms-2 me-1"></i> Verified </button>'
                },
                orderable: false,
                width: "440px",
            },

        ],
    });
    var urls = [];

    $('#softwareRequestList').on('click', '.attach', function () {
        var id = $(this).attr('data-id');
        // alert(id);
        var url = '/ServiceRequest/api/softwareRequest/GetbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data, value) {
                $('#softwareDisplay').modal('show');
                $('#softwareDisplay').find('.modal-title').text('File Attachment');
                var refId = $('#softwareRequestId').val(data.id);
                //alert(data.id);
                //alert(data.statusId);
                var uploadTable = $('#UploadList').DataTable({
                    destroy: true,

                    ajax: {
                        url: "/ServiceRequest/api/v1/uploads/" + id,
                        dataSrc: '',
                        headers: {
                            "Authorization": "Bearer " + localStorage.getItem('access_token')
                        }
                    },
                    searching: false,
                    ordering: false,
                    columns: [
                        {
                            data: "dateAdded"
                        },
                        {
                            data: "documentBlob",
                            render: function (data, type, row, meta) {
                                var fileType = row.imagePath.split('.').pop();
                                if (fileType == 'pdf') {
                                    return '<i class="fas fa-file-pdf fa-2x" style="color: red; font-size: 50px;"></i>';
                                } else if (fileType == 'doc' || fileType == 'docx') {
                                    return '<i class="fas fa-file-word fa-2x" style="color: blue; font-size: 50px;"> </i>';
                                } else if (fileType == 'xls' || fileType == 'xlsx') {
                                    return '<i class="fas fa-file-excel fa-2x" style="color: green; font-size: 50px;"></i>';
                                } else if (fileType == 'ppt' || fileType == 'pptx') {
                                    return '<i class="fas fa-file-powerpoint fa-2x" style="color: orange; font-size: 50px;"></i>';
                                } else if (fileType == 'jpg' || fileType == 'png' || fileType == 'jpeg') {
                                    return '<img src="data:image/png;base64,' + data + '" class="avatar" width="250" height="250"/>';
                                } else {
                                    return '<i class="bi bi-images" style="font-size: 250px;"></i> ';
                                }
                            },
                            className: "text-center"
                        },

                        {
                            // download link by using anchor link only
                            data: "documentBlob",
                            render: function (data, type, row) {
                                var f = new Blob([s2ab(atob(data))], { type: "" });
                                var filename = row.imagePath.replace(/^.*[\\\/]/, '');
                                var newF = URL.createObjectURL(f);
                                urls.push(newF);
                                var fileType = row.imagePath.split('.').pop();
                                //    check file if is image type
                                if (fileType == 'jpg' || fileType == 'png' || fileType == 'jpeg' || fileType == 'pdf' || fileType == 'gif') {
                                    // return '<a href="/api/v1/download/' + row.id + '" class="btn btn-sm btn-success">Download</a>'
                                    //     +
                                    return '<a href="' + newF + '" class="btn btn-sm btn-success" download="' + filename + '">Download</a>';
                                }
                                else {
                                    return '';
                                }
                            },
                            className: "text-center"
                        },

                    ]
                });
            },
            error: function (data) {
                toastr.error("Failed")
            }

        })
        // alert(data);
    });
    $('#UploadList').on('click', '.delete', function () {
        var id = $(this).attr('data-id');
        bootbox.confirm({
            message: "Are you sure you want to delete this record?",
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-success btn-sm'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-danger btn-sm'
                }
            },
            callback: function (result) {
                if (result) {
                    $.ajax({
                        type: 'DELETE',
                        url: '/ServiceRequest/api/v1/deteleImage/' + id,
                        headers: {
                            "Authorization": "Bearer " + localStorage.getItem('access_token')
                        },
                        success: function (data) {
                            toastr.success("Data Successfully Deleted!");
                            window.location.reload();
                        }
                    });
                }
            }
        });
    });

    $('#softwareRequestList').on('click', '.view', function () {
        var id = $(this).attr('data-id');
        $("#generateReportBtn").attr('data-id', id);

        //alert(id);
        var url = '/ServiceRequest/api/softwareRequest/GetbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#softwareDetailsModal').modal('show');

                $('#viewRequest').find('input[name="id"]').val(data.id);
                $('#viewRequest').find('input[name="fullName"]').val(data.fullName);
                $('#viewRequest').find('input[name="mobileNumber"]').val(data.mobileNumber);
                $('#viewRequest').find('input[name="modelName"]').val(data.modelName);
                $('#viewRequest').find('input[name="FileName"]').val(data.documentLabel);

                $('#viewRequest').find('select[name="departmentsId"]').val(data.departmentsId);
                $('#viewRequest').find('select[name="divisionsId"]').val(data.divisionsId);
                $('#viewRequest').find('select[name="softwareId"]').val(data.softwareId);
                $('#viewRequest').find('select[name="informationSystemId"]').val(data.informationSystemId);
                $('#viewRequest').find('input[name="requestFor"]').val(data.requestFor);
                $('#viewRequest').find('textarea[name="description"]').val(data.description);
                $('#viewRequest').find('select[name="statusId"]').val(data.statusId);
                $('#viewRequest').find('select[name="softwareTechnicianId"]').val(data.softwareTechnicianId);
                $('#viewRequest').find('textarea[name="remarks"]').val(data.remarks);
                $('#viewRequest').find('input[name="dateStarted"]').val(data.dateStarted);
                $('#viewRequest').find('input[name="dateEnded"]').val(data.dateEnded);
                $('#viewRequest').find('input[name="timeStarted"]').val(data.timeStarted);
                $('#viewRequest').find('input[name="timeEnded"]').val(data.timeEnded);
                $('#viewRequest').find('input[name="approver"]').val(data.approver);
                $('#viewRequest').find('input[name="dateApproved"]').val(data.dateApproved);
                $('#viewRequest').find('input[name="viewer"]').val(data.viewer);
                $('#viewRequest').find('input[name="dateView"]').val(data.dateView);
            }
        })
        // alert(data);
    });


    //Department
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/department',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=departmentsId]').append('<option value=""> Select Department</option>');
            $.each(data, function (index, value) {
                $('select[name=departmentsId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });

    //Division
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/division',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=divisionsId]').append('<option value=""> Select Division</option>');
            $.each(data, function (index, value) {
                $('select[name=divisionsId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });

    //Software
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/software',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=softwareId]').append('<option value=""> Select Software</option>');
            $.each(data, function (index, value) {
                $('select[name=softwareId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });


    //Software
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/infoSystem',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=informationSystemId]').append('<option value=""> Select System</option>');
            $.each(data, function (index, value) {
                $('select[name=informationSystemId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },
    });
    //Status
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/status',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=statusId]').append('<option value=""> Select Status</option>');
            $.each(data, function (index, value) {
                $('select[name=statusId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    //Technician
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/Technician',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=softwareTechnicianId]').append('<option value=""> Select Technician</option>');
            $.each(data, function (index, value) {
                $('select[name=softwareTechnicianId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });

    $('#softwareRequestList').on('click', '.technician', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/softwareRequest/GetbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#TechnicianModal').modal('show');
                $('#assignTech').find('input[name="id"]').val(data.id);
                $('#assignTech').find('input[name="statusId"]').val(data.statusId);
                //alert(data.statusId);

                if (data.statusId == 2)
                    $('.assigned-tech').prop('disabled', true)
                else if (data.statusId == 5)
                    $('.assigned-tech').prop('disabled', true)
                else if (data.statusId == 3)
                    $('.assigned-tech').prop('disabled', true)
                else if (data.statusId == 4)
                    $('.assigned-tech').prop('disabled', true)
                else if (data.statusId == 1)
                    $('.assigned-tech').prop('disabled', false)
                else
                    $('.assigned-tech').prop('disabled', false)

            }
        })
    });

    $('#updateTechnician').click(function (e) {
        var id = $('input[name=id]').val();
        //var data = $('#assignTech').serialize();
        var softwareTechnicianId = $('#assignTech').find('select[name="softwareTechnicianId"]').val();
        var softwareRequestId = $('#assignTech').find('input[name=id]').val();
        var data = {
            "id": softwareRequestId,
            "softwareTechnicianId": softwareTechnicianId
        };
        $.ajax({
            type: 'PUT',
            url: '/ServiceRequest/api/softwareRequest/UpdateTechnician/' + softwareRequestId,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                $('#TechnicianModal').modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success(" Successfully Assign Technician!");
                    // hide please wait modal

                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },

            //if failed
            error: function (data) {
                //alert(id);
                toastr.error("Please Assign Technician")
            }
        });
    });


    $('#softwareRequestList').on('click', '.approvedby', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/softwareRequest/GetbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#ApprovedByModal').modal('show');
                $('#approve').find('input[name="id"]').val(data.id);
                $('#approve').find('select[name="statusId"]').val(data.statusId);
                $('#approve').find('input[name="isReported"]').val(data.isReported);
                // alert(data.isReported);


                if (data.isReported == true)
                    $('.update-verified').prop('disabled', false)
                else
                    $('.update-verified').prop('disabled', true)
            }
        })
    });

    //AdminSaveRemarks
    $('#Approved').click(function (e) {
        e.preventDefault();
        var id = $('#approve').find('input[name="id"]').val();
        var data = $('#approve').serialize();
        $.ajax({
            type: 'PUT',
            url: '/ServiceRequest/api/softwareRequest/verify/' + id,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                $('#ApprovedByModal').modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success(" Successfully Approved!");
                    // hide please wait modal

                }, 1000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            //if failed
            error: function (data) {
                toastr.error("Fill up Remarks")
            }
        });
    });
}
function SoftwareTechForm() {

    $("#softwareRequestList").DataTable({
        "ajax": {
            "url": "/ServiceRequest/api/sr/assigned",
            "headers": {
                "Authorization": "Bearer " + localStorage.getItem('access_token')
            },
            "type": "GET",
            "dataType": "json",
        },
        "processing": "true",
        "serverSide": "true",
        "ordering": "true",
        "paging": "true",
        "fnInitComplete": function (oSettings, json) {
            // search after pressing enter
            $('#softwareRequestList_filter input').unbind();
            $('#softwareRequestList_filter input').bind('keyup', function (e) {
                if (e.keyCode == 13) {
                    // trigger search 
                    $('#pleasewait').modal('show');
                    setTimeout(function () {
                        $('#pleasewait').modal('hide');
                    }, 2500);
                    $('#softwareRequestList').DataTable().search(this.value).draw();
                }
                // if search is empty, reset the filter
                if (this.value == "") {
                    $('#softwareRequestList').DataTable().search("").draw();
                }
            }
            );
        },
        // "searching" : " true",
        // "searchDelay" : 1000,
        // order by desc dateuploaded
        "order": [[1, "desc"]],
        "language": {
            "processing": "Loading... Please wait"

        },
        "autoWidth": false,
        "columns": [

            {
                "data": "ticket",
                render: function (data, type, row) {
                    if (data == null) {
                        return '<span class="badge bg-danger">No Ticket</span>';
                    }
                    return `<span class="badge bg-info">${(data)}</span>`
                }

            },
            { "data": "dateAdded" },
            { "data": "fullName" },
            {
                "data": "softwareId",
                "render": function (data, type, row) {
                    return row.software.name;
                }
            },

            {
                "data": "statusId", render: function (data, type, row) {
                    if (data == 1) {
                        return '<span class="badge  bg-secondary">Open</span>'
                    }
                    else if (data == 2) {
                        return '<span class="badge bg-success">Resolved</span>'
                    }
                    else if (data == 3) {
                        return '<span class="badge  bg-warning">On hold</span>'
                    }
                    else if (data == 4) {
                        return '<span class="badge  bg-info">In Progress</span>'
                    }
                    else if (data == 5) {
                        return '<span class="badge  bg-danger">Cancel Request</span>'
                    }
                    else
                        return '<span class="badge  bg-secondary">Open</span>'

                }
            },
            {
                data: "isReported", render: function (data, type, row) {
                    if (data == true) {
                        return '<i class="fas fa-check-circle fa-2x ms-4" style="color:green;"></i>'

                    }
                    else
                        return ''
                }
            },
            {
                data: "isViewed", render: function (data, type, row) {
                    if (data == true) {
                        return '<i class="fas fa-check-circle fa-2x ms-4" style="color:green;"></i>'

                    }
                    else
                        return ''
                }
            },
            {
                data: "isApproved", render: function (data, type, row) {
                    if (data == true) {
                        return '<i class="fas fa-check-circle fa-2x ms-4" style="color:green;"></i>'

                    }
                    else
                        return ''
                }
            },


            {
                data: null,
                render: function (data) {
                    return '<button class="btn btn-info text-light btn-sm view ms-1 mb-1 btn-view" data-id="' + data.id + '"><i class="fas fa-eye"></i> View </button>'
                        + '<button class="btn btn-success text-light btn-sm attach ms-1 mb-1" data-id="' + data.id + '"><i class="fas fa-paperclip"></i> Attachments </button>'
                        + '<button class=" btn btn-primary btn-sm assign ms-1 mb-1 btn-status" data-id="' + data.id + '"><i class="fas fa-edit"></i> Status</button>'
                        + '<button class="btn btn-secondary btn-sm report ms-1 mb-1" data-id="' + data.id + '" > <i class="fas fa-user-cog"></i> Report</button>'

                },
                orderable: false,
                width: "370px",
            },

        ],
    });
    var urls = [];
    $('#softwareRequestList').on('click', '.attach', function () {
        var id = $(this).attr('data-id');
        // alert(id);
        var url = '/ServiceRequest/api/softwareRequest/GetbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data, value) {
                $('#softwareDisplay').modal('show');
                $('#softwareDisplay').find('.modal-title').text('File Attachment');
                var refId = $('#softwareRequestId').val(data.id);
                //alert(data.id);
                //alert(data.statusId);
                var uploadTable = $('#UploadList').DataTable({
                    destroy: true,

                    ajax: {
                        url: "/ServiceRequest/api/v1/uploads/" + id,
                        dataSrc: '',
                        headers: {
                            "Authorization": "Bearer " + localStorage.getItem('access_token')
                        }
                    },
                    searching: false,
                    ordering: false,
                    columns: [
                        {
                            data: "dateAdded"
                        },
                        {
                            data: "documentBlob",
                            render: function (data, type, row, meta) {
                                var fileType = row.imagePath.split('.').pop();
                                if (fileType == 'pdf') {
                                    return '<i class="fas fa-file-pdf fa-2x" style="color: red; font-size: 50px;"></i>';
                                } else if (fileType == 'doc' || fileType == 'docx') {
                                    return '<i class="fas fa-file-word fa-2x" style="color: blue; font-size: 50px;"> </i>';
                                } else if (fileType == 'xls' || fileType == 'xlsx') {
                                    return '<i class="fas fa-file-excel fa-2x" style="color: green; font-size: 50px;"></i>';
                                } else if (fileType == 'ppt' || fileType == 'pptx') {
                                    return '<i class="fas fa-file-powerpoint fa-2x" style="color: orange; font-size: 50px;"></i>';
                                } else if (fileType == 'jpg' || fileType == 'png' || fileType == 'jpeg') {
                                    return '<img src="data:image/png;base64,' + data + '" class="avatar" width="250" height="250"/>';
                                } else {
                                    return '<i class="bi bi-images" style="font-size: 250px;"></i> ';
                                }
                            },
                            className: "text-center"
                        },

                        {
                            // download link by using anchor link only
                            data: "documentBlob",
                            render: function (data, type, row) {
                                var f = new Blob([s2ab(atob(data))], { type: "" });
                                var filename = row.imagePath.replace(/^.*[\\\/]/, '');
                                var newF = URL.createObjectURL(f);
                                urls.push(newF);
                                var fileType = row.imagePath.split('.').pop();
                                //    check file if is image type
                                if (fileType == 'jpg' || fileType == 'png' || fileType == 'jpeg' || fileType == 'pdf' || fileType == 'gif') {
                                    // return '<a href="/api/v1/download/' + row.id + '" class="btn btn-sm btn-success">Download</a>'
                                    //     +
                                    return '<a href="' + newF + '" class="btn btn-sm btn-success" download="' + filename + '">Download</a>';
                                }
                                else {
                                    return '';
                                }
                            },
                            className: "text-center"
                        },

                    ]
                });

            },
            error: function (data) {
                toastr.error("Failed")
            }

        })
        // alert(data);
    });
    $('#UploadList').on('click', '.delete', function () {
        var id = $(this).attr('data-id');
        bootbox.confirm({
            message: "Are you sure you want to delete this record?",
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-success btn-sm'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-danger btn-sm'
                }
            },
            callback: function (result) {
                if (result) {
                    $.ajax({
                        type: 'DELETE',
                        url: '/ServiceRequest/api/v1/deteleImage/' + id,
                        headers: {
                            "Authorization": "Bearer " + localStorage.getItem('access_token')
                        },
                        success: function (data) {
                            toastr.success("Data Successfully Updated!");
                            window.location.reload();
                        }
                    });
                }
            }
        });
    });

    //get Details
    $('#softwareRequestList').on('click', '.view', function () {
        var id = $(this).attr('data-id');
        //alert(id);
        $("#generateReportBtn").attr('data-id', id);

        var url = '/ServiceRequest/api/softwareRequest/GetbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#viewmodal').modal('show');

                $('#viewRequest').find('input[name="id"]').val(data.id);
                $('#viewRequest').find('input[name="fullName"]').val(data.fullName);
                $('#viewRequest').find('input[name="mobileNumber"]').val(data.mobileNumber);
                $('#viewRequest').find('input[name="modelName"]').val(data.modelName);
                $('#viewRequest').find('select[name="departmentsId"]').val(data.departmentsId);
                $('#viewRequest').find('select[name="divisionsId"]').val(data.divisionsId);
                $('#viewRequest').find('select[name="softwareId"]').val(data.softwareId);
                $('#viewRequest').find('select[name="informationSystemId"]').val(data.informationSystemId);
                $('#viewRequest').find('input[name="requestFor"]').val(data.requestFor);
                $('#viewRequest').find('input[name="FileName"]').val(data.documentLabel);

                $('#viewRequest').find('textarea[name="description"]').val(data.description);
                $('#viewRequest').find('select[name="statusId"]').val(data.statusId);
                $('#viewRequest').find('select[name="softwareTechnicianId"]').val(data.softwareTechnicianId);
                $('#viewRequest').find('textarea[name="remarks"]').val(data.remarks);
                $('#viewRequest').find('input[name="dateStarted"]').val(data.dateStarted);
                $('#viewRequest').find('input[name="dateEnded"]').val(data.dateEnded);
                $('#viewRequest').find('input[name="timeStarted"]').val(data.timeStarted);
                $('#viewRequest').find('input[name="timeEnded"]').val(data.timeEnded);
                $('#viewRequest').find('input[name="approver"]').val(data.approver);
                $('#viewRequest').find('input[name="dateApproved"]').val(data.dateApproved);
                $('#viewRequest').find('input[name="viewer"]').val(data.viewer);
                $('#viewRequest').find('input[name="dateView"]').val(data.dateView);
            }
        })
        // alert(data);
    });


    //Department
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/department',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=departmentsId]').append('<option value=""> Select Department</option>');
            $.each(data, function (index, value) {
                $('select[name=departmentsId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });

    //Division
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/division',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=divisionsId]').append('<option value=""> Select Division</option>');
            $.each(data, function (index, value) {
                $('select[name=divisionsId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });


    //Software
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/software',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=softwareId]').append('<option value=""> Select Software</option>');
            $.each(data, function (index, value) {
                $('select[name=softwareId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });


    //Software
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/infoSystem',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=informationSystemId]').append('<option value=""> Select System</option>');
            $.each(data, function (index, value) {
                $('select[name=informationSystemId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },
    });
    //Status
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/status',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=statusId]').append('<option value=""> Select Status</option>');
            $.each(data, function (index, value) {
                $('select[name=statusId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    //Technician
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/Technician',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=softwareTechnicianId]').append('<option value=""> Select Technician</option>');
            $.each(data, function (index, value) {
                $('select[name=softwareTechnicianId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });



    //get id assign
    $('#softwareRequestList').on('click', '.assign', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/softwareRequest/GetbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#StatusModal').modal('show');
                $('#assign').find('input[name="id"]').val(data.id);
                $('#assign').find('select[name="statusId"]').val("");
               


            }
        })
        // alert(data);
    });


    //submit assign
    $('#updateStatus').click(function (e) {
        e.preventDefault();

        var id = $('input[name=id]').val();
        var data = $('#assign').serialize();
        $.ajax({
            type: 'PUT',
            url: '/ServiceRequest/api/softwareRequest/updateStatus/' + id,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                //hide modal
                $('#StatusModal').modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success(" Successfully Update Status!");
                    // hide please wait modal

                }, 1000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);

            },
            //if failed
            error: function (data) {
                //alert(id);
                toastr.error("Please update Status!")
            }
        });
    });

    $('#softwareRequestList').on('click', '.report', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/softwareRequest/GetbyId/' + id;

        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#softwarerequestModal').modal('show');
                $('#techForm').find('input[name="id"]').val(data.id);
                $('#techForm').find('select[name="softwareId"]').val(data.softwareId);
                $('#techForm').find('select[name="informationSystemId"]').val(data.informationSystemId);
                $('#techForm').find('textarea[name="remarks"]').val("");
                $('#techForm').find('input[name="dateStarted"]').val("");
                $('#techForm').find('input[name="dateEnded"]').val("");
                $('#techForm').find('input[name="timeStarted"]').val("");
                $('#techForm').find('input[name="timeEnded"]').val("");
                $('#techForm').find('input[name="statusId"]').val(data.statusId);
                //alert(data.statusId);
            }
        })
    });

    $('#updateTechForm').click(function (e) {
        e.preventDefault();

        var dataId = $('#techForm').find('input[name=id]').val();
        var dateStarted = $('#techForm').find('input[name=dateStarted]').val();
        var dateEnded = $('#techForm').find('input[name=dateEnded]').val();
        var remarks = $('#techForm').find('textarea[name=remarks]').val();
        var timeStarted = $('#techForm').find('input[name=timeStarted]').val();
        var timeEnded = $('#techForm').find('input[name=timeEnded]').val();
        var softwareId = $('#techForm').find('select[name=softwareId]').val();
        var informationSystemId = $('#techForm').find('select[name=informationSystemId]').val();

        var data = {
            "id": dataId,
            "dateStarted": dateStarted,
            "dateEnded": dateEnded,
            "remarks": remarks,
            "timeStarted": timeStarted,
            "timeEnded": timeEnded,
            "softwareId": softwareId,
            "informationSystemId": informationSystemId,
        };

        $.ajax({
            type: 'PUT',
            url: '/ServiceRequest/api/softwareRequest/TechnicianForm/' + dataId,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                //hide modal
                $('#softwarerequestModal').modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success("Technician Form Successfully Added!");
                    // hide please wait modal

                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            error: function (data) {
                toastr.error("Fill up alll forms / Invalid")

            }
        })
    });
}

function DeveloperSoftware() {

    var tables;
    $("#softwareRequestList").DataTable({
        "ajax": {
            "url": "/ServiceRequest/api/all/softReq",
            "headers": {
                "Authorization": "Bearer " + localStorage.getItem('access_token')
            },
            "type": "GET",
            "dataType": "json",
        },
        "processing": "true",
        "serverSide": "true",
        "ordering": "true",
        "paging": "true",
        "fnInitComplete": function (oSettings, json) {
            // search after pressing enter
            $('#softwareRequestList_filter input').unbind();
            $('#softwareRequestList_filter input').bind('keyup', function (e) {
                if (e.keyCode == 13) {
                    // trigger search 
                    $('#pleasewait').modal('show');
                    setTimeout(function () {
                        $('#pleasewait').modal('hide');
                    }, 2500);
                    $('#softwareRequestList').DataTable().search(this.value).draw();
                }
                // if search is empty, reset the filter
                if (this.value == "") {
                    $('#softwareRequestList').DataTable().search("").draw();
                }
            }
            );
        },
        // "searching" : " true",
        // "searchDelay" : 1000,
        // order by desc dateuploaded
        "order": [[0, "desc"]],
        "language": {
            "processing": "Loading... Please wait"

        },
        "autoWidth": false,
        "columns": [
            {
                data: "ticket",
                render: function (data, type, row) {
                    if (data == null) {
                        return '<span class="badge bg-danger">No Ticket</span>';
                    }
                    return `<span class="badge bg-info">${(data)}</span>`
                }
            },
            { data: "dateAdded" },
            { data: "fullName" },
            {
                data: "softwareId",
                render: function (data, type, row) {
                    return row.software.name;
                }
            },
            {
                data: "statusId", render: function (data, type, row) {
                    if (data == 1) {
                        return '<span class="badge  bg-secondary">Open</span>'
                    }
                    else if (data == 2) {
                        return '<span class="badge  bg-success">Resolved</span>'
                    }
                    else if (data == 3) {
                        return '<span class="badge bg-warning">On hold</span>'
                    }
                    else if (data == 4) {
                        return '<span class="badge bg-info">In Progress</span>'
                    }
                    else if (data == 5) {
                        return '<span class="badge  bg-danger">Cancel Request</span>'
                    }
                    else
                        return '<span class="badge bg-secondary">Open</span>'
                }
            },
            {
                data: null,
                render: function (data) {
                    return '<button class="btn btn-info text-light btn-sm view ms-1 mb-1 " data-id="' + data.id + '"><i class="fas fa-eye"></i> View  </button>'
                        + '<button class="btn btn-success text-light btn-sm attach ms-1 mb-1" data-id="' + data.id + '"><i class="fas fa-paperclip"></i> Attachments </button>'
                        + '<button class="btn btn-danger btn-sm delete ms-1 mb-1" data-id="' + data.id + '"><i class="fas fa-trash-alt"></i> Delete</button>'

                },
                orderable: false,
                width: "280px",
            },

        ],
    });

    $('#softwareRequestList').on('click', '.attach', function () {
        var id = $(this).attr('data-id');
        // alert(id);
        var url = '/ServiceRequest/api/softwareRequest/GetbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data, value) {
                $('#softwareDisplay').modal('show');
                $('#softwareDisplay').find('.modal-title').text('File Attachment');
                var refId = $('#softwareRequestId').val(data.id);
                //alert(data.id);
                //alert(data.statusId);
                var uploadTable = $('#UploadList').DataTable({
                    destroy: true,

                    ajax: {
                        url: "/ServiceRequest/api/v1/uploads/" + id,
                        dataSrc: '',
                        headers: {
                            "Authorization": "Bearer " + localStorage.getItem('access_token')
                        }
                    },
                    searching: false,
                    ordering: false,

                    columns: [
                        {
                            data: "dateAdded"
                        },
                        {
                            data: "documentBlob",
                            render: function (data, type, row, meta) {
                                var fileType = row.imagePath.split('.').pop();
                                if (fileType == 'pdf') {
                                    return '<i class="fas fa-file-pdf fa-2x" style="color: red; font-size: 50px;"></i>';
                                } else if (fileType == 'doc' || fileType == 'docx') {
                                    return '<i class="fas fa-file-word fa-2x" style="color: blue; font-size: 50px;"> </i>';
                                } else if (fileType == 'xls' || fileType == 'xlsx') {
                                    return '<i class="fas fa-file-excel fa-2x" style="color: green; font-size: 50px;"></i>';
                                } else if (fileType == 'ppt' || fileType == 'pptx') {
                                    return '<i class="fas fa-file-powerpoint fa-2x" style="color: orange; font-size: 50px;"></i>';
                                } else if (fileType == 'jpg' || fileType == 'png' || fileType == 'jpeg') {
                                    return '<img src="data:image/png;base64,' + data + '" width="150px" height="200px" />';
                                } else {
                                    return '<i class="bi bi-images" style="font-size: 250px;"></i> ';
                                }
                            }
                        },
                        {
                            // download link by using anchor link only
                            data: "documentBlob",
                            render: function (data, type, row) {
                                var f = new Blob([s2ab(atob(data))], { type: "" });
                                var filename = row.imagePath.replace(/^.*[\\\/]/, '');
                                var newF = URL.createObjectURL(f);
                                urls.push(newF);
                                var fileType = row.imagePath.split('.').pop();
                                //    check file if is image type
                                if (fileType == 'jpg' || fileType == 'png' || fileType == 'jpeg' || fileType == 'pdf' || fileType == 'gif') {
                                    // return '<a href="/api/v1/download/' + row.id + '" class="btn btn-sm btn-success">Download</a>'
                                    //     +
                                    return '<a href="' + newF + '" class="btn btn-sm btn-success" download="' + filename + '">Download</a>';
                                }
                                else {
                                    return '';
                                }
                            },
                            className: "text-center"
                        },

                        {
                            data: null,
                            render: function (data, type, row) {
                                return '<button class="btn btn-danger text-light btn-sm delete ms-1 mb-1" data-id="' + row.id + '"><i class="fas fa-eye"></i> Delete </button>'
                            },
                            className: "action-center"

                        }

                    ]
                });
            },
            error: function (data) {
                toastr.error("Failed")
            }

        })
        // alert(data);
    });

    $('#UploadList').on('click', '.delete', function () {
        var id = $(this).attr('data-id');
        bootbox.confirm({
            message: "Are you sure you want to delete this record?",
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-success btn-sm'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-danger btn-sm'
                }
            },
            callback: function (result) {
                if (result) {
                    $.ajax({
                        type: 'DELETE',
                        url: '/ServiceRequest/api/v1/deteleImage/' + id,
                        headers: {
                            "Authorization": "Bearer " + localStorage.getItem('access_token')
                        },
                        success: function (data) {
                            toastr.success("Data Successfully Deleted!");
                            window.location.reload();
                        }
                    });
                }
            }
        });
    });

    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/getuseremail',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            console.log(data);
            localStorage.setItem('email', data);
        },
    });

    //get Details
    $('#softwareRequestList').on('click', '.view', function () {
        var id = $(this).attr('data-id');
        $("#generateReportBtn").attr('data-id', id);

        //alert(id);
        var url = '/ServiceRequest/api/softwareRequest/GetbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#viewmodal').modal('show');

                $('#viewRequest').find('input[name="id"]').val(data.id);
                $('#viewRequest').find('input[name="fullName"]').val(data.fullName);
                $('#viewRequest').find('input[name="FileName"]').val(data.documentLabel);

                $('#viewRequest').find('input[name="mobileNumber"]').val(data.mobileNumber);
                $('#viewRequest').find('input[name="modelName"]').val(data.modelName);
                $('#viewRequest').find('select[name="departmentsId"]').val(data.departmentsId);
                $('#viewRequest').find('select[name="divisionsId"]').val(data.divisionsId);
                $('#viewRequest').find('select[name="softwareId"]').val(data.softwareId);
                $('#viewRequest').find('select[name="informationSystemId"]').val(data.informationSystemId);
                $('#viewRequest').find('input[name="requestFor"]').val(data.requestFor);
                $('#viewRequest').find('textarea[name="description"]').val(data.description);
                $('#viewRequest').find('select[name="statusId"]').val(data.statusId);
                $('#viewRequest').find('select[name="softwareTechnicianId"]').val(data.softwareTechnicianId);
                $('#viewRequest').find('textarea[name="remarks"]').val(data.remarks);
                $('#viewRequest').find('input[name="dateStarted"]').val(data.dateStarted);
                $('#viewRequest').find('input[name="dateEnded"]').val(data.dateEnded);
                $('#viewRequest').find('input[name="timeStarted"]').val(data.timeStarted);
                $('#viewRequest').find('input[name="timeEnded"]').val(data.timeEnded);
                $('#viewRequest').find('input[name="viewer"]').val(data.viewer);
                $('#viewRequest').find('input[name="dateView"]').val(data.dateView);
                $('#viewRequest').find('input[name="approver"]').val(data.approver);
                $('#viewRequest').find('input[name="dateApproved"]').val(data.dateApproved);

            }
        })
        // alert(data);
    });
    //Department
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/department',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=departmentsId]').append('<option value=""> Select Department</option>');
            $.each(data, function (index, value) {
                $('select[name=departmentsId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    //Division
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/division',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=divisionsId]').append('<option value=""> Select Division</option>');
            $.each(data, function (index, value) {
                $('select[name=divisionsId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    //Software
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/software',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=softwareId]').append('<option value=""> Select Software</option>');
            $.each(data, function (index, value) {
                $('select[name=softwareId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    //Software
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/infoSystem',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=informationSystemId]').append('<option value=""> Select System</option>');
            $.each(data, function (index, value) {
                $('select[name=informationSystemId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },
    });
    //Status
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/status',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=statusId]').append('<option value=""> Select Status</option>');
            $.each(data, function (index, value) {
                $('select[name=statusId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    //Technician
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/Technician',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=softwareTechnicianId]').append('<option value=""> Select Technician</option>');
            $.each(data, function (index, value) {
                $('select[name=softwareTechnicianId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });

    $('#softwareRequestList').on('click', '.request', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/softwareRequest/GetbyId/' + id
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#softwarerequestmodal').modal('show');

                $('#EditRequest').find('input[name="id"]').val(data.id);
                $('#EditRequest').find('select[name="informationSystemId"]').val(data.informationSystemId);
                $('#EditRequest').find('select[name="softwareId"]').val(data.softwareId);
                $('#EditRequest').find('input[name="requestFor"]').val(data.requestFor);
                $('#EditRequest').find('textarea[name="description"]').val(data.description);

                $('#EditRequest').find('input[name="statusId"]').val(data.statusId);
                console.log(data.statusId);
                if (data.statusId == 2)
                    $('.update-request').prop('disabled', true)
                else if (data.statusId == 3)
                    $('.update-request').prop('disabled', true)

                else if (data.statusId == 4)
                    $('.update-request').prop('disabled', true)

                else if (data.statusId == 5)
                    $('.update-request').prop('disabled', true)
                else
                    $('.update-request').prop('disabled', false)

            }
        })
    });

    $('#editRequest').click(function (e) {
        e.preventDefault();

        var id = $("#EditRequest").find('input[name="id"]').val();
        var data = $('#EditRequest').serialize();
        //alert(id);
        $.ajax({
            type: 'PUT',
            url: '/ServiceRequest/api/softwareRequest/updateRequest/' + id,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {

                //hide modal
                $('#softwarerequestmodal').modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success("Request Successfully updated!");
                    // hide please wait modal

                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            error: function (data) {
                //if error
            }
        })
    });

    //get CancelModal
    $('#softwareRequestList').on('click', '.cancel', function () {
        var id = $(this).attr('data-id');
        //alert(id);
        var url = '/ServiceRequest/api/softwareRequest/GetbyId/' + id;

        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#CancelRequestModal').modal('show');

                $('#cancelRequest').find('input[name="id"]').val(data.id);
                $('#cancelRequest').find('select[name="informationSystemId"]').val(data.informationSystemId);
                $('#cancelRequest').find('select[name="softwareId"]').val(data.softwareId);
                $('#cancelRequest').find('input[name="requestFor"]').val(data.requestFor);
                $('#cancelRequest').find('textarea[name="description"]').val(data.description);
                // alert(data.statusId);
                //alert(data.softwareId);

                $('#cancelRequest').find('input[name="statusId"]').val(data.statusId);
                //alert(data.statusId);
                if (data.statusId == 2)
                    $('.cancel-request').attr('disabled', true);
                else if (data.statusId == 3)
                    $('.cancel-request').attr('disabled', true);
                else if (data.statusId == 4)
                    $('.cancel-request').attr('disabled', true);
                else if (data.statusId == 5)
                    $('.cancel-request').attr('disabled', true);
                else
                    $('.cancel-request').attr('disabled', false);
            }
        })
        // alert(data);
    });


    $('#softwareRequestList').on('click', '.delete', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/Software/Delete/' + id;
        $.ajax({
            // confirm before delete
            beforeSend: function (xhr) {
                // use bootbox
                bootbox.confirm({
                    message: "Are you sure you want to delete this record?",
                    buttons: {
                        // yes and no sequence

                        confirm: {
                            label: 'Yes',
                            className: 'btn-success btn-sm'
                        },
                        cancel: {
                            label: 'No',
                            className: 'btn-danger btn-sm'
                        }
                    },
                    callback: function (result) {
                        if (result) {
                            $.ajax({
                                type: 'DELETE',
                                url: url,
                                headers: {
                                    'Authorization': 'Bearer ' + localStorage.getItem('access_token')
                                },
                                success: function (data) {

                                    //show please wait modal
                                    $('#pleasewait').modal('show');
                                    //show toastr after 3
                                    setTimeout(function () {
                                        toastr.success("Data Successfully Updated!");
                                        // hide please wait modal
                                    }, 2000);
                                    setTimeout(function () {
                                        window.location.reload();
                                    }, 3000);
                                },
                                //if failed
                                error: function (data) {
                                    toastr.error("Error")
                                }
                            });
                        }
                    }
                });

            },
        });
    });
}


function UserSoftwareRequest() {
    var tables;
    $("#softwareRequestList").DataTable({
        "ajax": {
            "url": "/ServiceRequest/api/sr/user",
            "headers": {
                "Authorization": "Bearer " + localStorage.getItem('access_token')
            },
            "type": "GET",
            "dataType": "json",
        },
        "processing": "true",
        "serverSide": "true",
        "ordering": "true",
        "paging": "true",
        "fnInitComplete": function (oSettings, json) {
            // search after pressing enter
            $('#softwareRequestList_filter input').unbind();
            $('#softwareRequestList_filter input').bind('keyup', function (e) {
                if (e.keyCode == 13) {
                    // trigger search 
                    $('#pleasewait').modal('show');
                    setTimeout(function () {
                        $('#pleasewait').modal('hide');
                    }, 2500);
                    $('#softwareRequestList').DataTable().search(this.value).draw();
                }
                // if search is empty, reset the filter
                if (this.value == "") {
                    $('#softwareRequestList').DataTable().search("").draw();
                }
            }
            );
        },
        // "searching" : " true",
        // "searchDelay" : 1000,
        // order by desc dateuploaded
        "order": [[1, "desc"]],
        "language": {
            "processing": "Loading... Please wait"

        },
        "autoWidth": false,
        "columns": [
            {
                data: "ticket",
                render: function (data, type, row) {
                    if (data == null) {
                        return '<span class="badge bg-danger">No Ticket</span>';
                    }
                    return `<span class="badge bg-info">${(data)}</span>`
                }

            },
            { data: "dateAdded" },
            { data: "fullName" },
            {
                data: "softwareId",
                render: function (data, type, row) {
                    return row.software.name;
                }
            },
            {
                data: "statusId", render: function (data, type, row) {
                    if (data == 1) {
                        return '<span class="badge  bg-secondary">Open</span>'
                    }
                    else if (data == 2) {
                        return '<span class="badge  bg-success">Resolved</span>'
                    }
                    else if (data == 3) {
                        return '<span class="badge bg-warning">On hold</span>'
                    }
                    else if (data == 4) {
                        return '<span class="badge bg-info">In Progress</span>'
                    }
                    else if (data == 5) {
                        return '<span class="badge  bg-danger">Cancel Request</span>'
                    }
                    else
                        return '<span class="badge bg-secondary">Open</span>'
                }
            },
            {
                data: null,
                render: function (data, type, full) {
                    return '<button class="btn btn-info btn-sm view ms-1 mb-1 text-white" data-id="' + data.id + '"><i class="far fa-eye"></i> View </button>' +
                        '<button class="btn btn-success text-light btn-sm attach ms-1 mb-1" data-id="' + data.id + '"><i class="fas fa-paperclip"></i> Attachments </button>' +
                        '<button class="btn btn-primary btn-sm request ms-1 mb-1" data-id="' + data.id + '"><i class="far fa-edit"></i> Edit</button>' +
                        '<button class="btn btn-danger btn-sm cancel ms-1 mb-1" data-id="' + data.id + '"><i class="fas fa-trash-alt"></i> Cancel</button>'
                },
                orderable: false,
                width: "350px",
            },

        ],
    });
    var urls = [];

    $('#softwareRequestList').on('click', '.attach', function () {
        var id = $(this).attr('data-id');
        // alert(id);
        var url = '/ServiceRequest/api/softwareRequest/GetbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data, value) {
                $('#softwareDisplay').modal('show');
                $('#softwareDisplay').find('.modal-title').text('File Attachment');
                $('#attachementModel').find('input[name="statusId"]').val(data.statusId);

                if (data.statusId == 2)
                    $('.update-image').prop('disabled', true)

                else if (data.statusId == 3)
                    $('.update-image').prop('disabled', true)

                else if (data.statusId == 4)
                    $('.update-image').prop('disabled', true)

                else if (data.statusId == 5)
                    $('.update-image').prop('disabled', true)
                else
                    $('.update-image').prop('disabled', false)

                var refId = $('#softwareRequestId').val(data.id);
                //alert(data.id);
                //alert(data.statusId);
                var uploadTable = $('#UploadList').DataTable({
                    destroy: true,

                    ajax: {
                        url: "/ServiceRequest/api/v1/uploads/" + id,
                        dataSrc: '',
                        headers: {
                            "Authorization": "Bearer " + localStorage.getItem('access_token')
                        }
                    },
                    searching: false,
                    ordering: false,

                    columns: [
                        {
                            data: "dateAdded"
                        },
                        {
                            data: "documentBlob",
                            render: function (data, type, row, meta) {
                                var fileType = row.imagePath.split('.').pop();
                                if (fileType == 'pdf') {
                                    return '<i class="fas fa-file-pdf fa-2x" style="color: red; font-size: 50px;"></i>';
                                } else if (fileType == 'doc' || fileType == 'docx') {
                                    return '<i class="fas fa-file-word fa-2x" style="color: blue; font-size: 50px;"> </i>';
                                } else if (fileType == 'xls' || fileType == 'xlsx') {
                                    return '<i class="fas fa-file-excel fa-2x" style="color: green; font-size: 50px;"></i>';
                                } else if (fileType == 'ppt' || fileType == 'pptx') {
                                    return '<i class="fas fa-file-powerpoint fa-2x" style="color: orange; font-size: 50px;"></i>';
                                } else if (fileType == 'jpg' || fileType == 'png' || fileType == 'jpeg') {
                                    return '<img src="data:image/png;base64,' + data + '" class="avatar" width="250" height="250"/>';
                                } else {
                                    return '<i class="bi bi-images" style="font-size: 250px;"></i> ';
                                }
                            },
                            className: "text-center"
                        },
                        {
                            // download link by using anchor link only
                            data: "documentBlob",
                            render: function (data, type, row) {
                                var f = new Blob([s2ab(atob(data))], { type: "" });
                                var filename = row.imagePath.replace(/^.*[\\\/]/, '');
                                var newF = URL.createObjectURL(f);
                                urls.push(newF);
                                var fileType = row.imagePath.split('.').pop();
                                //    check file if is image type
                                if (fileType == 'jpg' || fileType == 'png' || fileType == 'jpeg' || fileType == 'pdf' || fileType == 'gif') {
                                    // return '<a href="/api/v1/download/' + row.id + '" class="btn btn-sm btn-success">Download</a>'
                                    //     +
                                    return '<a href="' + newF + '" class="btn btn-sm btn-success" download="' + filename + '">Download</a>';
                                }
                                else {
                                    return '';
                                }
                            },
                            className: "text-center"
                        }
                    ]
                });
                console.log(data.id);
                $('#upload2').find('input[name=softwareRequestId]').val(data.id);
                $('#softwareDisplay').on('hidden.bs.modal', function () {
                    // remove id on select
                    $('select[name=softwareRequestId]').find('option[value=' + data.id + ']').remove();
                });
            }
        })
        // alert(data);
    });

    $('#upload2').submit(function (e) {
        e.preventDefault();
        var formData = new FormData(this);
        $.ajax({
            url: '/ServiceRequest/api/v2/update/saveFile',
            type: 'POST',
            data: formData,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {

                $('#editModal').modal('hide');
                $('#softwareDisplay').modal('hide');

                ////show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success("File Successfully Added!");
                    // hide please wait modal

                }, 2000);
                setTimeout(function () {
                    location.reload();
                    $('#upload2')[0].reset();
                }, 3000);
            },
            cache: false,
            contentType: false,
            processData: false,
            error: function (data) {

                toastr.error("Error")
            }
        });
    });


    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/getuseremail',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            //  alert(data);
            localStorage.setItem('email', data);
        },
    });

    //get Details
    $('#softwareRequestList').on('click', '.view', function () {
        var id = $(this).attr('data-id');
        //alert(id);
        var url = '/ServiceRequest/api/softwareRequest/GetbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#viewmodal').modal('show');

                $('#viewRequest').find('input[name="id"]').val(data.id);
                $('#viewRequest').find('input[name="fullName"]').val(data.fullName);
                $('#viewRequest').find('input[name="mobileNumber"]').val(data.mobileNumber);
                $('#viewRequest').find('input[name="modelName"]').val(data.modelName);
                $('#viewRequest').find('select[name="departmentsId"]').val(data.departmentsId);
                $('#viewRequest').find('select[name="divisionsId"]').val(data.divisionsId);
                $('#viewRequest').find('select[name="softwareId"]').val(data.softwareId);
                $('#viewRequest').find('select[name="informationSystemId"]').val(data.informationSystemId);
                $('#viewRequest').find('input[name="requestFor"]').val(data.requestFor);
                $('#viewRequest').find('input[name="FileName"]').val(data.documentLabel);

                $('#viewRequest').find('textarea[name="description"]').val(data.description);
                $('#viewRequest').find('select[name="statusId"]').val(data.statusId);
                $('#viewRequest').find('select[name="softwareTechnicianId"]').val(data.softwareTechnicianId);
                $('#viewRequest').find('textarea[name="remarks"]').val(data.remarks);
                $('#viewRequest').find('input[name="dateStarted"]').val(data.dateStarted);
                $('#viewRequest').find('input[name="dateEnded"]').val(data.dateEnded);
                $('#viewRequest').find('input[name="timeStarted"]').val(data.timeStarted);
                $('#viewRequest').find('input[name="timeEnded"]').val(data.timeEnded);
                $('#viewRequest').find('input[name="approver"]').val(data.approver);
                $('#viewRequest').find('input[name="dateApproved"]').val(data.dateApproved);
                $('#viewRequest').find('input[name="viewer"]').val(data.viewer);
                $('#viewRequest').find('input[name="dateView"]').val(data.dateView);

            }
        })
        // alert(data);
    });
    //Department
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/department',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=departmentsId]').append('<option value=""> Select Department</option>');
            $.each(data, function (index, value) {
                $('select[name=departmentsId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    //Division
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/division',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=divisionsId]').append('<option value=""> Select Division</option>');
            $.each(data, function (index, value) {
                $('select[name=divisionsId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    //Software
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/software',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=softwareId]').append('<option value=""> Select Software</option>');
            $.each(data, function (index, value) {
                $('select[name=softwareId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    //Software
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/infoSystem',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=informationSystemId]').append('<option value=""> Select System</option>');
            $.each(data, function (index, value) {
                $('select[name=informationSystemId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },
    });
    //Status
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/status',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=statusId]').append('<option value=""> Select Status</option>');
            $.each(data, function (index, value) {
                $('select[name=statusId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    //Technician
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/Technician',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=softwareTechnicianId]').append('<option value=""> Select Technician</option>');
            $.each(data, function (index, value) {
                $('select[name=softwareTechnicianId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });

    $('#softwareRequestList').on('click', '.request', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/softwareRequest/GetbyId/' + id
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#softwarerequestmodal').modal('show');
                $('#EditRequest').find('select[name="softwareId"]').val(data.softwareId);
                $('#EditRequest').find('input[name="id"]').val(data.id);
                $('#EditRequest').find('select[name="informationSystemId"]').val(data.informationSystemId);
                $('#EditRequest').find('select[name="softwareId"]').val(data.softwareId);
                $('#EditRequest').find('input[name="requestFor"]').val(data.requestFor);
                $('#EditRequest').find('input[name="FileName"]').val(data.documentLabel);
                $('#EditRequest').find('textarea[name="description"]').val(data.description);

                $('#EditRequest').find('input[name="statusId"]').val(data.statusId);
                console.log(data.statusId);
                if (data.statusId == 2)
                    $('.update-request').prop('disabled', true)
                else if (data.statusId == 3)
                    $('.update-request').prop('disabled', true)

                else if (data.statusId == 4)
                    $('.update-request').prop('disabled', true)

                else if (data.statusId == 5)
                    $('.update-request').prop('disabled', true)
                else
                    $('.update-request').prop('disabled', false)

            }
        })
    });

    $('#editRequest').click(function (e) {
        e.preventDefault();

        var id = $("#EditRequest").find('input[name="id"]').val();
        //var data = $('#EditRequest').serialize();
        //alert(id);
        var data = {
            softwareId: $('#EditRequest').find('select[name=softwareId]').val(),
            informationSystemId: $('#EditRequest').find('select[name=informationSystemId]').val(),
            softwareId: $('#EditRequest').find('select[name=softwareId]').val(),
            requestFor: $('#EditRequest').find('input[name=requestFor]').val(),
            documentLabel: $('#EditRequest').find('input[name=FileName]').val(),
            description: $('#EditRequest').find('textarea[name=description]').val(),
        }
        $.ajax({
            type: 'PUT',
            url: '/ServiceRequest/api/softwareRequest/updateRequest/' + id,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                //hide modal
                $('#softwarerequestmodal').modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success("Request Successfully updated!");
                    // hide please wait modal

                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            error: function (data) {
                //if error
            }
        })
    });

    //get CancelModal
    $('#softwareRequestList').on('click', '.cancel', function () {
        var id = $(this).attr('data-id');
        //alert(id);
        var url = '/ServiceRequest/api/softwareRequest/GetbyId/' + id;

        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#CancelRequestModal').modal('show');

                $('#cancelRequest').find('input[name="id"]').val(data.id);
                $('#cancelRequest').find('select[name="informationSystemId"]').val(data.informationSystemId);
                $('#cancelRequest').find('select[name="softwareId"]').val(data.softwareId);
                $('#cancelRequest').find('input[name="requestFor"]').val(data.requestFor);
                $('#cancelRequest').find('textarea[name="description"]').val(data.description);
                // alert(data.statusId);
                //alert(data.softwareId);

                $('#cancelRequest').find('input[name="statusId"]').val(data.statusId);
                //alert(data.statusId);
                if (data.statusId == 2)
                    $('.cancel-request').attr('disabled', true);
                else if (data.statusId == 3)
                    $('.cancel-request').attr('disabled', true);
                else if (data.statusId == 4)
                    $('.cancel-request').attr('disabled', true);
                else if (data.statusId == 5)
                    $('.cancel-request').attr('disabled', true);
                else
                    $('.cancel-request').attr('disabled', false);
            }
        })
        // alert(data);
    });

    $('#cancel').click(function (e) {
        e.preventDefault();
        var id = $('#cancelRequest').find('input[name="id"]').val();
        var data = $('#cancelRequest').serialize();
        // alert(id);
        $.ajax({
            type: 'PUT',
            url: '/ServiceRequest/api/softwareRequest/Cancel/' + id,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {

                $('#CancelRequestModal').modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success(" Successfully Cancelled a Request!");
                    // hide please wait modal

                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            //if failed
            error: function (data) {
                console.log(data.id);
                //alert(id);
                //setTimeout(() => {
                //    location.reload();
                //}, 1500);
                toastr.error("Fill up Remarks")
            }
        })
    });
}

function SoftwareSuperAdmin() {
    var tables;
    $("#softwareRequestList").DataTable({
        "ajax": {
            "url": "/ServiceRequest/api/all/softReq",
            "headers": {
                "Authorization": "Bearer " + localStorage.getItem('access_token')
            },
            "type": "GET",
            "dataType": "json",
        },
        "processing": "true",
        "serverSide": "true",
        "ordering": "true",
        "paging": "true",
        "fnInitComplete": function (oSettings, json) {
            // search after pressing enter
            $('#softwareRequestList_filter input').unbind();
            $('#softwareRequestList_filter input').bind('keyup', function (e) {
                if (e.keyCode == 13) {
                    // trigger search 
                    $('#pleasewait').modal('show');
                    setTimeout(function () {
                        $('#pleasewait').modal('hide');
                    }, 2500);
                    $('#softwareRequestList').DataTable().search(this.value).draw();
                }
                // if search is empty, reset the filter
                if (this.value == "") {
                    $('#softwareRequestList').DataTable().search("").draw();
                }
            }
            );
        },

        "order": [[0, "desc"]],
        "language": {
            "processing": "Loading... Please wait"

        },
        "autoWidth": false,
        "columns": [

            {
                data: "ticket",
                render: function (data, type, row) {
                    if (data == null) {
                        return '<span class="badge bg-danger">No Ticket</span>';
                    }
                    return `<span class="badge bg-info">${(data)}</span>`
                }

            },
            {
                data: "dateAdded",
            },
            { data: "fullName" },
            {
                data: "statusId", render: function (data, type, row) {
                    if (data == 1) {
                        return '<span class="badge bg-secondary">Open</span>'
                    }
                    else if (data == 2) {
                        return '<span class="badge  bg-success">Resolved</span>'

                    }
                    else if (data == 3) {
                        return '<span class="badge bg-warning">On hold</span>'
                    }
                    else if (data == 4) {
                        return '<span class="badge  bg-info">In Progress</span>'
                    }
                    else if (data == 5) {
                        return '<span class="badge  bg-danger">Cancel Request</span>'
                    }
                    else
                        return '<span class="badge  bg-secondary">Open</span>'

                }
            },
            {
                data: "dateView", render: function (data, type, row) {
                    if (data == null) {
                        return ''
                    }
                    else
                        return `<span>${moment(data).format("D/M/Y ")}</span>`;

                }
            },
            {
                data: "viewer", render: function (data, type, row) {
                    if (data == null) {
                        return '--'
                    }
                    else
                        return `<span>${(data)}</span>`
                }
            },

            {
                data: "dateApproved", render: function (data, type, row) {
                    if (data == null) {
                        return ''
                    }
                    else
                        return `<span>${moment(data).format("D/M/Y")}</span>`;
                }
            },
            {
                data: null,
                render: function (data) {
                    return '<button class="btn btn-info text-light btn-sm view ms-1 mb-1 btn-view-admin" data-id="' + data.id + '"><i class="fas fa-eye"></i> View</button>'
                        + '<button class="btn btn-success text-light btn-sm attach ms-1 mb-1" data-id="' + data.id + '"><i class="fas fa-paperclip"></i> Attachments </button>'
                        + '<button class="btn btn-primary text-light btn-sm ms-1 mb-1 viewedby" data-id="' + data.id + '"><i class="fas fa-user-check"></i> Approved by </button>'
                },
                orderable: false,
                width: "330px",
            },

        ],
    });
    var urls = [];
    $('#softwareRequestList').on('click', '.attach', function () {
        var id = $(this).attr('data-id');
        // alert(id);
        var url = '/ServiceRequest/api/softwareRequest/GetbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data, value) {
                $('#softwareDisplay').modal('show');
                $('#softwareDisplay').find('.modal-title').text('File Attachment');
                var refId = $('#softwareRequestId').val(data.id);
                //alert(data.id);
                //alert(data.statusId);
                var uploadTable = $('#UploadList').DataTable({
                    destroy: true,

                    ajax: {
                        url: "/ServiceRequest/api/v1/uploads/" + id,
                        dataSrc: '',
                        headers: {
                            "Authorization": "Bearer " + localStorage.getItem('access_token')
                        }
                    },
                    searching: false,
                    ordering: false,

                    columns: [
                        {
                            data: "dateAdded",
                        },
                        {
                            data: "documentBlob",
                            render: function (data, type, row, meta) {
                                var fileType = row.imagePath.split('.').pop();
                                if (fileType == 'pdf') {
                                    return '<i class="fas fa-file-pdf fa-2x" style="color: red; font-size: 50px;"></i>';
                                } else if (fileType == 'doc' || fileType == 'docx') {
                                    return '<i class="fas fa-file-word fa-2x" style="color: blue; font-size: 50px;"> </i>';
                                } else if (fileType == 'xls' || fileType == 'xlsx') {
                                    return '<i class="fas fa-file-excel fa-2x" style="color: green; font-size: 50px;"></i>';
                                } else if (fileType == 'ppt' || fileType == 'pptx') {
                                    return '<i class="fas fa-file-powerpoint fa-2x" style="color: orange; font-size: 50px;"></i>';
                                } else if (fileType == 'jpg' || fileType == 'png' || fileType == 'jpeg') {
                                    return '<img src="data:image/png;base64,' + data + '" class="avatar" width="250" height="250"/>';
                                } else {
                                    return '<i class="bi bi-images" style="font-size: 250px;"></i> ';
                                }
                            },
                            className: "text-center"
                        },

                        {
                            // download link by using anchor link only
                            data: "documentBlob",
                            render: function (data, type, row) {
                                var f = new Blob([s2ab(atob(data))], { type: "" });
                                var filename = row.imagePath.replace(/^.*[\\\/]/, '');
                                var newF = URL.createObjectURL(f);
                                urls.push(newF);
                                var fileType = row.imagePath.split('.').pop();
                                //    check file if is image type
                                if (fileType == 'jpg' || fileType == 'png' || fileType == 'jpeg' || fileType == 'pdf' || fileType == 'gif') {
                                    // return '<a href="/api/v1/download/' + row.id + '" class="btn btn-sm btn-success">Download</a>'
                                    //     +
                                    return '<a href="' + newF + '" class="btn btn-sm btn-success" download="' + filename + '">Download</a>';
                                }
                                else {
                                    return '';
                                }
                            },
                            className: "text-center"
                        },
                        {
                            data: null,
                            render: function (data, type, row) {
                                return '<button class="btn btn-danger text-light btn-sm delete ms-1 mb-1" data-id="' + row.id + '"><i class="fas fa-eye"></i> Delete </button>'
                            },
                            className: "text-center"

                        }

                    ]
                });
            },
            error: function (data) {
                toastr.error("Failed")
            }

        })
        // alert(data);
    });
    $('#UploadList').on('click', '.delete', function () {
        var id = $(this).attr('data-id');
        $('#softwareDisplay').modal('hide');
        bootbox.confirm({
            message: "Are you sure you want to delete this record?",
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-success btn-sm'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-danger btn-sm'
                }
            },
            callback: function (result) {
                if (result) {
                    $.ajax({
                        type: 'DELETE',
                        url: '/ServiceRequest/api/v1/deteleImage/' + id,
                        headers: {
                            "Authorization": "Bearer " + localStorage.getItem('access_token')
                        },
                        success: function (data) {
                            toastr.success("Data Successfully Deleted!");
                            window.location.reload();
                        }
                    });
                }
            }
        });
    });

    //get Details
    $('#softwareRequestList').on('click', '.view', function () {
        var id = $(this).attr('data-id');
        //alert(id);
        var url = '/ServiceRequest/api/softwareRequest/GetbyId/' + id;

        $("#generateReportBtn").attr('data-id', id);
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#softwarerequestmodal').modal('show');

                $('#viewRequest').find('input[name="id"]').val(data.id);
                $('#viewRequest').find('input[name="fullName"]').val(data.fullName);
                $('#viewRequest').find('input[name="dateAdded"]').val(data.dateAdded);
                $('#viewRequest').find('input[name="FileName"]').val(data.documentLabel);
                $('#viewRequest').find('input[name="mobileNumber"]').val(data.mobileNumber);
                $('#viewRequest').find('input[name="modelName"]').val(data.modelName);
                $('#viewRequest').find('select[name="departmentsId"]').val(data.departmentsId);
                $('#viewRequest').find('select[name="divisionsId"]').val(data.divisionsId);
                $('#viewRequest').find('select[name="softwareId"]').val(data.softwareId);
                $('#viewRequest').find('select[name="informationSystemId"]').val(data.informationSystemId);
                $('#viewRequest').find('input[name="requestFor"]').val(data.requestFor);
                $('#viewRequest').find('textarea[name="description"]').val(data.description);
                $('#viewRequest').find('select[name="statusId"]').val(data.statusId);
                $('#viewRequest').find('select[name="softwareTechnicianId"]').val(data.softwareTechnicianId);
                $('#viewRequest').find('textarea[name="remarks"]').val(data.remarks);
                $('#viewRequest').find('input[name="dateStarted"]').val(data.dateStarted);
                $('#viewRequest').find('input[name="dateEnded"]').val(data.dateEnded);
                $('#viewRequest').find('input[name="timeStarted"]').val(data.timeStarted);
                $('#viewRequest').find('input[name="timeEnded"]').val(data.timeEnded);
                $('#viewRequest').find('input[name="approver"]').val(data.approver);
                $('#viewRequest').find('input[name="dateApproved"]').val(data.dateApproved);
                $('#viewRequest').find('input[name="viewer"]').val(data.viewer);
                $('#viewRequest').find('input[name="dateView"]').val(data.dateView);
            }
        })
        // alert(data);
    });

    //Department
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/department',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=departmentsId]').append('<option value=""> Select Department</option>');
            $.each(data, function (index, value) {
                $('select[name=departmentsId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    //Division
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/division',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=divisionsId]').append('<option value=""> Select Division</option>');
            $.each(data, function (index, value) {
                $('select[name=divisionsId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    //Software
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/software',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=softwareId]').append('<option value=""> Select Software</option>');
            $.each(data, function (index, value) {
                $('select[name=softwareId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    //Software
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/infoSystem',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=informationSystemId]').append('<option value=""> Select System</option>');
            $.each(data, function (index, value) {
                $('select[name=informationSystemId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },
    });
    //Status
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/status',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=statusId]').append('<option value=""> Select Status</option>');
            $.each(data, function (index, value) {
                $('select[name=statusId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    //Technician
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/Technician',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=softwareTechnicianId]').append('<option value=""> Select Technician</option>');
            $.each(data, function (index, value) {
                $('select[name=softwareTechnicianId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });


    $('#softwareRequestList').on('click', '.viewedby', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/softwareRequest/GetbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#ViewedByModal').modal('show');
                $('#viewed').find('input[name="id"]').val(data.id);
                $('#viewed').find('select[name="statusId"]').val(data.statusId);
                $('#viewed').find('textarea[name="viewedRemarks"]').val(data.viewedRemarks);
                $('#viewed').find('input[name="isViewed"]').val(data.isViewed);
                // alert(data.isApproved);
                //alert(data.statusId);
                if (data.isViewed == true)
                    $('.update-viewed').prop('disabled', false)

                else
                    $('.update-viewed').prop('disabled', true)
                //alert(id);

            }
        })
    });

    $('#ViewwdBySubmit').click(function (e) {
        e.preventDefault();

        var id = $('#viewed').find('input[name="id"]').val();
        var data = $('#viewed').serialize();

        $.ajax({
            type: 'PUT',
            url: '/ServiceRequest/api/softwareRequest/Viewed/' + id,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                $('#ViewedByModal').modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success(" Successfully Approved!");
                    // hide please wait modal

                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            },
            //if failed
            error: function (data) {

                toastr.error("Fill up Remarks")
            }
        });
    });
}

function UserdashHardware() {
    // get activity logs
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/v2/hr/get',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('#UserhardwareRequest tbody').html('');
            $.each(data, function (index, value) {
                $('#UserhardwareRequest tbody').append(
                    '<tr>' +
                    // activityDate momentjs
                    '<td>' + '<span class="badge bg-info">' + value.ticket + '</span>' + '</td>' +
                    '<td>' + value.dateAdded + '</td>' +
                    '<td>' + value.fullName + '</td>' +
                    '<td>' + value.departments + '</td>' +

                    '<td>' + (value.statusId == 1 ? '<span class="badge bg-secondary" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">Open</span>' : (value.statusId == 2 ? '<span class="badge bg-success" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">Resolved</span>' : (value.statusId == 3 ? '<span class="badge bg-warning" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">On hold</span>' : (value.statusId == 4 ? '<span class="badge bg-info" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">In Progress</span>' : (value.statusId == 5 ? '<span class="badge bg-danger" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">Cancel Request</span>' : '<span class="badge bg-secondary" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">Open</span>'))))) + '</td>' + '</tr>'
                );
            });
        },

        //if failed
        error: function (data) {

            // toastr.info("Success")
        }

    });
}

// dashboard List User Request
function UserdashSoftware() {
    // get activity logs
    $.ajax({
        type: 'GET',
        url: 'ServiceRequest/api/v2/sr/get',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('#UsersoftwareRequest tbody').html('');
            $.each(data, function (index, value) {
                $('#UsersoftwareRequest tbody').append(
                    '<tr>' +
                    '<td>' + '<span class="badge bg-info">' + value.ticket + '</span>' + '</td>' +
                    '<td>' + moment(value.dateAdded).format('M/D/Y LT') + '</td>' +
                    '<td>' + value.fullName + '</td>' +
                    '<td>' + (value.statusId == 1 ? '<span class="badge bg-secondary" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">Open</span>' : (value.statusId == 2 ? '<span class="badge bg-success" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">Resolved</span>' : (value.statusId == 3 ? '<span class="badge bg-warning" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">On hold</span>' : (value.statusId == 4 ? '<span class="badge bg-info" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">In Progress</span>' : (value.statusId == 5 ? '<span class="badge bg-danger" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">Cancel Request</span>' : '<span class="badge bg-secondary" style="border-radius: 35px; padding: 4px 6px; font-size: 10px;">Open</span>'))))) + '</td>' + '</tr>'
                );
            });
        },

        //if failed
        error: function (data) {

            // toastr.info("Success")
        }

    });
}

//Count Dashboard Tech/Users
function Count() {
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/department/countDept',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {

            $('#depcount').text(data);
        },
        //if failed
        error: function (data) {

            // toastr.info("Success")
        }
    });

    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/software/countReq',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            //alert(data);
            $('#softwareRequest').text(data);
        },
        //if failed
        error: function (data) {

            // toastr.info("Success")
        }
    });

    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardware/countReq',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {

            $('#hardwareRequest').text(data);
        },
        //if failed
        error: function (data) {

            // toastr.info("Success")
        }
    });

    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/software/requestbyUser',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {

            $('#softwareuser').text(data);
        },
        //if failed
        error: function (data) {

            // toastr.info("Success")
        }
    });

    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardware/requestbyUser',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {

            $('#hardwareuser').text(data);
        },
        //if failed
        error: function (data) {

            // toastr.info("Success")
        }
    });

    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/user/countuser',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {

            $('#usercount').text(data);
        },
        //if failed
        error: function (data) {

            // toastr.info("Success")
        }
    });

    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardware/assignbytech',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            //alert(data);
            $('#AssignedRequests').text(data);
        },
        //if failed
        error: function (data) {

            // toastr.info("Success")
        }
    });

    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/software/assignbytech',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            //alert(data);
            $('#AssignedSoftware').text(data);
        },
        //if failed
        error: function (data) {

            // toastr.info("Success")
        }
    });


    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareTechnician/count',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            //alert(data);
            $('#Technician').text(data);
        },
        //if failed
        error: function (data) {

            // toastr.info("Success")
        }
    });

    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareTechnician/count',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            //alert(data);
            $('#ProgCount').text(data);
        },
        //if failed
        error: function (data) {

            // toastr.info("Success")
        }
    });
}
function AssignedTechReq() {

    var tables;
    $("#softwareRequestList").DataTable({
        "ajax": {
            "url": "/ServiceRequest/api/all/softReq",
            "headers": {
                "Authorization": "Bearer " + localStorage.getItem('access_token')
            },
            "type": "GET",
            "dataType": "json",
        },
        "processing": "true",
        "serverSide": "true",
        "ordering": "true",
        "paging": "true",
        "fnInitComplete": function (oSettings, json) {
            // search after pressing enter
            $('#softwareRequestList_filter input').unbind();
            $('#softwareRequestList_filter input').bind('keyup', function (e) {
                if (e.keyCode == 13) {
                    // trigger search 
                    $('#pleasewait').modal('show');
                    setTimeout(function () {
                        $('#pleasewait').modal('hide');
                    }, 2500);
                    $('#softwareRequestList').DataTable().search(this.value).draw();
                }
                // if search is empty, reset the filter
                if (this.value == "") {
                    $('#softwareRequestList').DataTable().search("").draw();
                }
            }
            );
        },
        // "searching" : " true",
        // "searchDelay" : 1000,
        // order by desc dateuploaded
        "order": [[0, "desc"]],
        "language": {
            "processing": "Loading... Please wait"

        },
        "autoWidth": false,
        "columns": [

            {
                "data": "ticket",
                render: function (data, type, row) {
                    if (data == null) {
                        return '<span class="badge bg-danger">No Ticket</span>';
                    }
                    return `<span class="badge bg-info">${(data)}</span>`
                }

            },
            { "data": "dateAdded" },
            { "data": "fullName" },
            {
                "data": "softwareId",
                "render": function (data, type, row) {
                    return row.software.name;
                }
            },
            {
                data: "softwareTechnician.name",
                render: function (data, type, row) {
                    if (data == null) {
                        return '<span class="badge bg-success">Not Assigned</span>'
                    }
                    else
                        return row.softwareTechnician.name;
                }
            },
            {
                "data": "statusId", render: function (data, type, row) {
                    if (data == 1) {
                        return '<span class="badge  bg-secondary">Open</span>'
                    }
                    else if (data == 2) {
                        return '<span class="badge bg-success">Resolved</span>'
                    }
                    else if (data == 3) {
                        return '<span class="badge  bg-warning">On hold</span>'
                    }
                    else if (data == 4) {
                        return '<span class="badge  bg-info">In Progress</span>'
                    }
                    else if (data == 5) {
                        return '<span class="badge  bg-danger">Cancel Request</span>'
                    }
                    else
                        return '<span class="badge  bg-secondary">Open</span>'

                }
            },

            {
                data: null,
                render: function (data) {
                    return '<button class="btn btn-info btn-sm view ms-1 mb-1 text-white" data-id="' + data.id + '"><i class="far fa-eye"></i> View </button>' +
                        '<button class="btn btn-success text-light btn-sm attach ms-1 mb-1" data-id="' + data.id + '"><i class="fas fa-paperclip"></i> Attachments </button>' +
                        '<button class=" btn btn-primary btn-sm assign ms-1 mb-1 btn-status" data-id="' + data.id + '"><i class="fas fa-edit"></i> Assign To me</button>'


                },
                orderable: false,
                width: "340px",
            },

        ],
    });

    $('#softwareRequestList').on('click', '.view', function () {
        var id = $(this).attr('data-id');
        //alert(id);
        var url = '/ServiceRequest/api/softwareRequest/GetbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#viewmodal').modal('show');

                $('#viewRequest').find('input[name="id"]').val(data.id);
                $('#viewRequest').find('input[name="fullName"]').val(data.fullName);
                $('#viewRequest').find('input[name="mobileNumber"]').val(data.mobileNumber);
                $('#viewRequest').find('input[name="modelName"]').val(data.modelName);
                $('#viewRequest').find('input[name="FileName"]').val(data.documentLabel);

                $('#viewRequest').find('select[name="departmentsId"]').val(data.departmentsId);
                $('#viewRequest').find('select[name="divisionsId"]').val(data.divisionsId);
                $('#viewRequest').find('select[name="softwareId"]').val(data.softwareId);
                $('#viewRequest').find('select[name="informationSystemId"]').val(data.informationSystemId);
                $('#viewRequest').find('input[name="requestFor"]').val(data.requestFor);
                $('#viewRequest').find('textarea[name="description"]').val(data.description);
                $('#viewRequest').find('select[name="statusId"]').val(data.statusId);
                $('#viewRequest').find('select[name="softwareTechnicianId"]').val(data.softwareTechnicianId);
                $('#viewRequest').find('textarea[name="remarks"]').val(data.remarks);
                $('#viewRequest').find('input[name="dateStarted"]').val(data.dateStarted);
                $('#viewRequest').find('input[name="dateEnded"]').val(data.dateEnded);
                $('#viewRequest').find('input[name="timeStarted"]').val(data.timeStarted);
                $('#viewRequest').find('input[name="timeEnded"]').val(data.timeEnded);
                $('#viewRequest').find('input[name="approver"]').val(data.approver);
                $('#viewRequest').find('input[name="dateApproved"]').val(data.dateApproved);
                $('#viewRequest').find('input[name="viewer"]').val(data.viewer);
                $('#viewRequest').find('input[name="dateView"]').val(data.dateView);

            }
        })
        // alert(data);
    });
    var urls = [];
    $('#softwareRequestList').on('click', '.attach', function () {
        var id = $(this).attr('data-id');
        // alert(id);
        var url = '/ServiceRequest/api/softwareRequest/GetbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data, value) {
                $('#softwareDisplay').modal('show');
                $('#softwareDisplay').find('.modal-title').text('File Attachment');
                var refId = $('#softwareRequestId').val(data.id);
                //alert(data.id);
                //alert(data.statusId);
                var uploadTable = $('#UploadList').DataTable({
                    destroy: true,

                    ajax: {
                        url: "/ServiceRequest/api/v1/uploads/" + id,
                        dataSrc: '',
                        headers: {
                            "Authorization": "Bearer " + localStorage.getItem('access_token')
                        }
                    },
                    searching: false,
                    ordering: false,

                    columns: [
                        {
                            data: "dateAdded"
                        },
                        {
                            data: "documentBlob",
                            render: function (data, type, row, meta) {
                                var fileType = row.imagePath.split('.').pop();
                                if (fileType == 'pdf') {
                                    return '<i class="fas fa-file-pdf fa-2x" style="color: red; font-size: 50px;"></i>';
                                } else if (fileType == 'doc' || fileType == 'docx') {
                                    return '<i class="fas fa-file-word fa-2x" style="color: blue; font-size: 50px;"> </i>';
                                } else if (fileType == 'xls' || fileType == 'xlsx') {
                                    return '<i class="fas fa-file-excel fa-2x" style="color: green; font-size: 50px;"></i>';
                                } else if (fileType == 'ppt' || fileType == 'pptx') {
                                    return '<i class="fas fa-file-powerpoint fa-2x" style="color: orange; font-size: 50px;"></i>';
                                } else if (fileType == 'jpg' || fileType == 'png' || fileType == 'jpeg') {
                                    return '<img src="data:image/png;base64,' + data + '" class="avatar" width="250" height="250"/>';
                                } else {
                                    return '<i class="bi bi-images" style="font-size: 250px;"></i> ';
                                }
                            },
                            className: "text-center"
                        },
                        {
                            // download link by using anchor link only
                            data: "documentBlob",
                            render: function (data, type, row) {
                                var f = new Blob([s2ab(atob(data))], { type: "" });
                                var filename = row.imagePath.replace(/^.*[\\\/]/, '');
                                var newF = URL.createObjectURL(f);
                                urls.push(newF);
                                var fileType = row.imagePath.split('.').pop();
                                //    check file if is image type
                                if (fileType == 'jpg' || fileType == 'png' || fileType == 'jpeg' || fileType == 'pdf' || fileType == 'gif') {
                                    // return '<a href="/api/v1/download/' + row.id + '" class="btn btn-sm btn-success">Download</a>'
                                    //     +
                                    return '<a href="' + newF + '" class="btn btn-sm btn-success" download="' + filename + '">Download</a>';
                                }
                                else {
                                    return '';
                                }
                            },
                            className: "text-center"
                        },

                    ]
                });
                console.log(data.id);
                $('#upload2').find('input[name=softwareRequestId]').val(data.id);
                $('#softwareDisplay').on('hidden.bs.modal', function () {
                    // remove id on select
                    $('select[name=softwareRequestId]').find('option[value=' + data.id + ']').remove();
                });
            }
        })
        // alert(data);
    });

    $('#softwareRequestList').on('click', '.assign', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/softwareRequest/GetbyId/' + id
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#assignToMe').modal('show');

                $('#assignTome').find('input[name="id"]').val(data.id);
                $('#assignTome').find('select[name="informationSystemId"]').val(data.informationSystemId);
                $('#assignTome').find('select[name="softwareId"]').val(data.softwareId);
                $('#assignTome').find('input[name="requestFor"]').val(data.requestFor);
                $('#assignTome').find('textarea[name="description"]').val(data.description);

                $('#assignTome').find('input[name="statusId"]').val(data.statusId);
                $('#assignTome').find('input[name="proEmail"]').val(data.proEmail);
                /*                alert(data.proEmail);*/
                console.log(data.statusId);
                if (data.statusId == 2)
                    $('.update-request').prop('disabled', true)
                else if (data.statusId == 3)
                    $('.update-request').prop('disabled', true)
                else if (data.statusId == 4)
                    $('.update-request').prop('disabled', true)
                else if (data.statusId == 5)
                    $('.update-request').prop('disabled', true)
                else
                    $('.update-request').prop('disabled', false)

            }
        })
    });
    //Department
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/department',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=departmentsId]').append('<option value=""> Select Department</option>');
            $.each(data, function (index, value) {
                $('select[name=departmentsId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    //Division
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/division',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=divisionsId]').append('<option value=""> Select Division</option>');
            $.each(data, function (index, value) {
                $('select[name=divisionsId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    //Status
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/status',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=statusId]').append('<option value=""> Select Status</option>');
            $.each(data, function (index, value) {
                $('select[name=statusId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    //Technician
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/Technician',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=softwareTechnicianId]').append('<option value=""> Select Technician</option>');
            $.each(data, function (index, value) {
                $('select[name=softwareTechnicianId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/infoSystem',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=informationSystemId]').append('<option value=""> Select System</option>');
            $.each(data, function (index, value) {
                $('select[name=informationSystemId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },
    });
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/software',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=softwareId]').append('<option value=""> Select Software</option>');
            $.each(data, function (index, value) {
                $('select[name=softwareId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });

    $('#Assigntome').click(function (e) {
        e.preventDefault();

        var id = $('#assignTome').find('input[name="id"]').val();
        var data = $('#assignTome').serialize();
        //alert(id);
        $.ajax({
            type: 'PUT',
            url: '/ServiceRequest/api/softwareRequest/Assigntomer/' + id,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {

                $('#assignToMe').modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success(" Successfully Assigned The Request!");
                    // hide please wait modal

                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);

            },
            //if failed
            error: function (data) {
                console.log(data.id);

                toastr.error("Fill up alll forms / Invalid")
            }
        })
    })
};
var urls = [];

function HardwareAssigned() {
    var tables;
    $("#hardwareRequestList").DataTable({
        "ajax": {
            "url": "/ServiceRequest/api/all/hardReq",
            "headers": {
                "Authorization": "Bearer " + localStorage.getItem('access_token')
            },
            "type": "GET",
            "dataType": "json",
        },
        "processing": "true",
        "serverSide": "true",
        "ordering": "true",
        "paging": "true",
        "fnInitComplete": function (oSettings, json) {
            // search after pressing enter
            $('#hardwareRequestList_filter input').unbind();
            $('#hardwareRequestList_filter input').bind('keyup', function (e) {
                if (e.keyCode == 13) {
                    // trigger search 
                    $('#pleasewait').modal('show');
                    setTimeout(function () {
                        $('#pleasewait').modal('hide');
                    }, 2500);
                    $('#hardwareRequestList').DataTable().search(this.value).draw();
                }
                // if search is empty, reset the filter
                if (this.value == "") {
                    $('#hardwareRequestList').DataTable().search("").draw();
                }
            }
            );
        },
        // "searching" : " true",
        // "searchDelay" : 1000,
        // order by desc dateuploaded
        "order": [[0, "desc"]],
        "language": {
            "processing": "Loading... Please wait"

        },
        "autoWidth": false,
        "columns": [

            {
                "data": "ticket",
                render: function (data, type, row) {
                    if (data == null) {
                        return '<span class="badge bg-danger">No Ticket</span>';
                    }
                    return `<span class="badge bg-info">${(data)}</span>`
                }

            },
            { "data": "dateAdded" },
            { "data": "fullName" },
            {
                "data": "hardwareId",
                "render": function (data, type, row) {
                    if (data == null) {
                        return '';
                    }
                    else {
                        return row.hardware.name;
                    }
                }
            },
            {
                "data": "hardwareTechnicianId",
                "render": function (data, type, row) {
                    if (data == null) {
                        return '';
                    }
                    else {
                        return row.hardwareTechnician.name;
                    }
                }
            },
            {
                "data": "statusId", render: function (data, type, row) {
                    if (data == 1) {
                        return '<span class="badge  bg-secondary">Open</span>'
                    }
                    else if (data == 2) {
                        return '<span class="badge bg-success">Resolved</span>'
                    }
                    else if (data == 3) {
                        return '<span class="badge  bg-warning">On hold</span>'
                    }
                    else if (data == 4) {
                        return '<span class="badge  bg-info">In Progress</span>'
                    }
                    else if (data == 5) {
                        return '<span class="badge  bg-danger">Cancel Request</span>'
                    }
                    else
                        return '<span class="badge  bg-secondary">Open</span>'

                }
            },

            {
                data: null,
                render: function (data) {
                    return '<button class="btn btn-info text-light btn-sm view ms-1 mb-1" data-id="' + data.id + '"><i class="fas fa-eye"></i> View </button>' +
                        '<button class="btn btn-success text-light btn-sm attach ms-1 mb-1" data-id="' + data.id + '"><i class="fas fa-paperclip"></i> Attachments </button>' +
                        '<button class="btn btn-primary text-light btn-sm assign ms-1 mb-1" data-id="' + data.id + '"><i class="fas fa-user-cog"></i> Assign To me</button>'

                },
                orderable: false,
                width: "340px",
            },

        ],
    });

    $('#hardwareRequestList').on('click', '.attach', function () {
        var id = $(this).attr('data-id');
        // alert(id);
        var url = '/ServiceRequest/api/hardwareRequest/GetRequestbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data, value) {
                $('#hardwareDisplay').modal('show');
                $('#hardwareDisplay').find('.modal-title').text('Add Image');
                var refId = $('#hardwareRequestId').val(data.id);
                //alert(data.id);
                //alert(data.statusId);
                var uploadTable = $('#UploadList').DataTable({
                    destroy: true,

                    ajax: {
                        url: "/ServiceRequest/api/v2/uploadDislpay/" + id,
                        dataSrc: '',
                        headers: {
                            "Authorization": "Bearer " + localStorage.getItem('access_token')
                        }
                    },
                    searching: false,
                    ordering: false,

                    columns: [
                        {
                            data: "dateAdded"
                        },
                        {
                            data: "documentBlob",
                            render: function (data, type, row, meta) {
                                var fileType = row.imagePath.split('.').pop();
                                if (fileType == 'pdf') {
                                    return '<i class="fas fa-file-pdf fa-2x" style="color: red; font-size: 50px;"></i>';
                                } else if (fileType == 'doc' || fileType == 'docx') {
                                    return '<i class="fas fa-file-word fa-2x" style="color: blue; font-size: 50px;"> </i>';
                                } else if (fileType == 'xls' || fileType == 'xlsx') {
                                    return '<i class="fas fa-file-excel fa-2x" style="color: green; font-size: 50px;"></i>';
                                } else if (fileType == 'ppt' || fileType == 'pptx') {
                                    return '<i class="fas fa-file-powerpoint fa-2x" style="color: orange; font-size: 50px;"></i>';
                                } else if (fileType == 'jpg' || fileType == 'png' || fileType == 'jpeg') {
                                    return '<img src="data:image/png;base64,' + data + '" class="avatar" width="250" height="250"/>';
                                } else {
                                    return '<i class="bi bi-images" style="font-size: 250px;"></i> ';
                                }
                            },
                            className: "text-center"

                        },
                        {
                            // download link by using anchor link only
                            data: "documentBlob",
                            render: function (data, type, row) {
                                var f = new Blob([s2ab(atob(data))], { type: "" });
                                var filename = row.imagePath.replace(/^.*[\\\/]/, '');
                                var newF = URL.createObjectURL(f);
                                urls.push(newF);
                                var fileType = row.imagePath.split('.').pop();
                                //    check file if is image type
                                if (fileType == 'jpg' || fileType == 'png' || fileType == 'jpeg' || fileType == 'pdf' || fileType == 'gif') {
                                    // return '<a href="/api/v1/download/' + row.id + '" class="btn btn-sm btn-success">Download</a>'
                                    //     +
                                    return '<a href="' + newF + '" class="btn btn-sm btn-success" download="' + filename + '">Download</a>';

                                }
                                else {
                                    return '';
                                }
                            },
                            className: "text-center"
                        },

                    ]
                })
            }
        })
        // alert(data);
    });

    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/unitType',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=unitTypeId]').append('<option value="-1"> Select UnitType</option>');
            $.each(data, function (index, value) {
                $('select[name=unitTypeId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });

    //Department
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/department',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=departmentsId]').append('<option value=""> Select Department</option>');
            $.each(data, function (index, value) {
                $('select[name=departmentsId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    //Division
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/division',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=divisionsId]').append('<option value=""> Select Division</option>');
            $.each(data, function (index, value) {
                $('select[name=divisionsId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    //hardware Category
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/hardware',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=hardwareId]').append('<option value=""> Select Hardware Service Category</option>');
            $.each(data, function (index, value) {
                $('select[name=hardwareId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    //technician
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/Technician',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=hardwareTechnicianId]').append('<option value=""> Select Technician</option>');
            $.each(data, function (index, value) {
                $('select[name=hardwareTechnicianId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    //status
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/status',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=statusId]').append('<option value=""> Select Status </option>');
            $.each(data, function (index, value) {
                $('select[name=statusId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });

    //GET findings
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/Findings',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=findingId]').append('<option value=""> Select Result</option>');
            $.each(data, function (index, value) {
                $('select[name=findingId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },
    });

    $('#hardwareRequestList').on('click', '.view', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/hardwareRequest/GetRequestbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#viewmodal').modal('show');
                $('#viewRequest').find('input[name="id"]').val(data.id);
                $('#viewRequest').find('input[name="fullName"]').val(data.fullName);
                $('#viewRequest').find('input[name="mobileNumber"]').val(data.mobileNumber);
                $('#viewRequest').find('input[name="modelName"]').val(data.modelName);
                $('#viewRequest').find('select[name="departmentsId"]').val(data.departmentsId);
                $('#viewRequest').find('select[name="divisionsId"]').val(data.divisionsId);
                $('#viewRequest').find('select[name="hardwareId"]').val(data.hardwareId);
                $('#viewRequest').find('select[name="unitTypeId"]').val(data.unitTypeId);
                $('#viewRequest').find('input[name="brandName"]').val(data.brandName);
                $('#viewRequest').find('textarea[name="description"]').val(data.description);
                $('#viewRequest').find('select[name="statusId"]').val(data.statusId);
                $('#viewRequest').find('select[name="findingId"]').val(data.findingId);
                $('#viewRequest').find('select[name="hardwareTechnicianId"]').val(data.hardwareTechnicianId);
                $('#viewRequest').find('input[name="possibleCause"]').val(data.possibleCause);
                $('#viewRequest').find('input[name="FileName"]').val(data.documentLabel);
                $('#viewRequest').find('textarea[name="remarks"]').val(data.remarks);
                $('#viewRequest').find('input[name="dateStarted"]').val(data.dateStarted);
                $('#viewRequest').find('input[name="dateEnded"]').val(data.dateEnded);
                $('#viewRequest').find('input[name="timeStarted"]').val(data.timeStarted);
                $('#viewRequest').find('input[name="timeEnded"]').val(data.timeEnded);
                $('#viewRequest').find('input[name="approver"]').val(data.approver);
                $('#viewRequest').find('input[name="dateApproved"]').val(data.dateApproved);
                $('#viewRequest').find('input[name="viewer"]').val(data.viewer);
                $('#viewRequest').find('input[name="dateView"]').val(data.dateView);
                $('#viewRequest').find('input[name="serialNumber"]').val(data.serialNumber);
                $('#viewRequest').find('input[name="controlNumber"]').val(data.controlNumber);
            }
        })

    });
    $('#hardwareRequestList').on('click', '.assign', function () {
        var id = $(this).attr('data-id');
        var url = '/ServiceRequest/api/hardwareRequest/GetRequestbyId/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#assignToMe').modal('show');
                $('#assignTome').find('input[name="id"]').val(data.id);

                /*                alert(data.proEmail);*/
                console.log(data.statusId);
                if (data.statusId == 2)
                    $('.update-request').prop('disabled', true)
                else if (data.statusId == 3)
                    $('.update-request').prop('disabled', true)
                else if (data.statusId == 4)
                    $('.update-request').prop('disabled', true)
                else if (data.statusId == 5)
                    $('.update-request').prop('disabled', true)
                else
                    $('.update-request').prop('disabled', false)

            }
        })
    });

    $('#Assigntome').click(function (e) {
        e.preventDefault();

        var id = $('#assignTome').find('input[name="id"]').val();
        var data = $('#assignTome').serialize();
        //alert(id);
        $.ajax({
            type: 'PUT',
            url: '/ServiceRequest/api/hardwareRequest/Assign/' + id,
            data: data,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
            },
            success: function (data) {
                $('#assignToMe').modal('hide');
                //show please wait modal
                $('#pleasewait').modal('show');
                //show toastr after 3
                setTimeout(function () {
                    toastr.success(" Successfully Assigned The Request!");
                    // hide please wait modal

                }, 2000);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);

            },
            //if failed
            error: function (data) {
                console.log(data.id);

                toastr.error("Fill up alll forms / Invalid")
            }
        })
    })
}
//function SoftwareCountList() {

//    $.ajax({
//        type: 'GET',
//        url: '/ServiceRequest/api/countbyName',
//        headers: {
//            "Authorization": "Bearer " + localStorage.getItem('access_token')
//        },
//        success: function (data) {
//            $('#proglistCount tbody').html('');
//            $.each(data, function (index, value) {
//                $('#proglistCount tbody').append(
//                    '<tr>' +
//                    '<td>' + '<span style="font-size: 15px;">' + (value.name == null ? '<span>Not Assigned</span>' : value.name) + '</span>' + '</td>' +
//                    '<td>' + '<span style="font-size: 15px;">' + value.count + '</span>' + '</td>' + '</tr>'
//                );
//            });
//        },

//        //if failed
//        error: function (data) {

//            // toastr.info("Success")
//        }
//    });

//    $.ajax({
//        type: 'GET',
//        url: '/ServiceRequest/api/sftService/count',
//        headers: {
//            "Authorization": "Bearer " + localStorage.getItem('access_token')
//        },
//        success: function (data) {
//            $('#sftListCount tbody').html('');
//            $.each(data, function (index, value) {
//                $('#sftListCount tbody').append(
//                    '<tr>' +
//                    '<td>' + '<span style="font-size: 15px;">' + value.name + '</span>' + '</td>' +
//                    '<td>' + '<span style="font-size: 15px;">' + value.count + '</span>' + '</td>' + '</tr>'
//                );
//            });
//        },

//        //if failed
//        error: function (data) {

//            // toastr.info("Success")
//        }
//    });

//    $.ajax({
//        type: 'GET',
//        url: '/ServiceRequest/api/sftSystem/count',
//        headers: {
//            "Authorization": "Bearer " + localStorage.getItem('access_token')
//        },
//        success: function (data) {
//            $('#sftsystemList tbody').html('');
//            $.each(data, function (index, value) {
//                $('#sftsystemList tbody').append(
//                    '<tr>' +
//                    '<td>' + '<span style="font-size: 15px;">' + value.name + '</span>' + '</td>' +
//                    '<td>' + '<span style="font-size: 15px;">' + value.count + '</span>' + '</td>' + '</tr>'
//                );
//            });
//        },

//        //if failed
//        error: function (data) {

//            // toastr.info("Success")
//        }
//    });

//}


//function techlistCount() {
//    $.ajax({
//        type: 'GET',
//        url: '/ServiceRequest/api/v2/hrdTech/count',
//        headers: {
//            "Authorization": "Bearer " + localStorage.getItem('access_token')
//        },
//        success: function (data) {
//            $('#techlistCount tbody').html('');
//            $.each(data, function (index, value) {
//                $('#techlistCount tbody').append(
//                    '<tr>' +
//                    '<td>' + '<span style="font-size: 15px;">' + (value.name == null ? '<span>Not Assigned</span>' : value.name) + '</span>' + '</td>' +
//                    '<td>' + '<span style="font-size: 15px;">' + value.count + '</span>' + '</td>' + '</tr>'
//                );
//            });
//        },

//        //if failed
//        error: function (data) {

//            // toastr.info("Success")
//        }
//    });

//    $.ajax({
//        type: 'GET',
//        url: '/ServiceRequest/api/v2/hrd/count',
//        headers: {
//            "Authorization": "Bearer " + localStorage.getItem('access_token')
//        },
//        success: function (data) {
//            $('#hrdListCount tbody').html('');
//            $.each(data, function (index, value) {
//                $('#hrdListCount tbody').append(
//                    '<tr>' +
//                    '<td>' + '<span style="font-size: 15px;">' + value.name + '</span>' + '</td>' +
//                    '<td>' + '<span style="font-size: 15px;">' + value.count + '</span>' + '</td>' + '</tr>'
//                );
//            });
//        },

//        //if failed
//        error: function (data) {

//            // toastr.info("Success")
//        }
//    });

//    $.ajax({
//        type: 'GET',
//        url: '/ServiceRequest/api/v2/hrdServices/count',
//        headers: {
//            "Authorization": "Bearer " + localStorage.getItem('access_token')
//        },
//        success: function (data) {
//            $('#hrdCatCount tbody').html('');
//            $.each(data, function (index, value) {
//                $('#hrdCatCount tbody').append(
//                    '<tr>' +
//                    '<td>' + '<span style="font-size: 15px;">' + value.name + '</span>' + '</td>' +
//                    '<td>' + '<span style="font-size: 15px;">' + value.count + '</span>' + '</td>' + '</tr>'
//                );
//            });
//        },

//        //if failed
//        error: function (data) {

//            // toastr.info("Success")
//        }
//    });

//}
function s2ab(s) {
    var buf = new ArrayBuffer(s.length);
    var view = new Uint8Array(buf);
    for (var i = 0; i != s.length; ++i) view[i] = s.charCodeAt(i) & 0xFF;
    return buf;
}

function ManualRequest() {
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/department',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            var html = '<option value="">Select Department</option>';
            $.each(data, function (i, item) {
                html += '<option value="' + item.id + '">' + item.name + '</option>';
            });
            $('select[name=departmentsId]').html(html);
            // render divisionsId select
            // console log on select change
            $('select[name=departmentsId]').on('change', function () {
                var depId = $('select[name=departmentsId]').val();
                $.ajax({
                    type: 'GET',
                    url: '/ServiceRequest/api/div/fetchbyid/' + depId,
                    headers: {
                        'Authorization': 'Bearer ' + localStorage.getItem('access_token')
                    },
                    success: function (data) {
                        var html = '<option value="0">Select Division</option>';
                        $.each(data, function (i, item) {
                            html += '<option value="' + item.id + '">' + item.name + '</option>';
                        });
                        //console.log(data);
                        $('select[name=divisionsId]').html(html);
                    }
                });
            });
        }
    });


    $('.departments').select2({
        theme: 'bootstrap-5'
    });

    //Software
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/software',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=softwareId]').append('<option value=""> Select Software</option>');
            $.each(data, function (index, value) {
                $('select[name=softwareId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });


    //Software
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/infoSystem',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=informationSystemId]').append('<option value=""> Select System</option>');
            $.each(data, function (index, value) {
                $('select[name=informationSystemId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },
    });


}

function ManualRequest2() {

    $('#departmentsId').attr("disabled", true);
    $('#divisionsId').attr("disabled", true);

    $('.fullname').select2({
        theme: 'bootstrap-5'
    }).on('change', function (e) {
        var id = $('.fullname option:selected').attr('value');
        console.log(id);
        $.ajax({
            type: 'GET',
            url: '/ServiceRequest/api/v1/users/getid/' + id,
            success: function (data) {
                $('#manual').find('input[name="fullName"]').val(data.fullName);
                $('#manual').find('input[name="email"]').val(data.email);
                $('#manual').find('input[name="mobileNumber"]').val(data.mobileNumber);
                $('#manual').find('select[name="departmentsId"]').val(data.departmentsId);
                $('#manual').find('select[name="divisionsId"]').val(data.divisionsId);

            },
            error: function (data) {

            }
        })

    });
    // Users
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/v1/roles/users',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $.each(data, function (index, value) {
                $('select[name=valueId]').append('<option value="' + value.userId + '">' + value.fullName + '</option>');
            }
            );
        },
    });



    //Department
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/department',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $.each(data, function (index, value) {
                $('select[name=departmentsId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });

    //Division
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/division',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $.each(data, function (index, value) {
                $('select[name=divisionsId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });


    //Software
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/software',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=softwareId]').append('<option value=""> Select Software</option>');
            $.each(data, function (index, value) {
                $('select[name=softwareId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });


    //Software
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/softwareRequest/infoSystem',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=informationSystemId]').append('<option value=""> Select System</option>');
            $.each(data, function (index, value) {
                $('select[name=informationSystemId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },
    });

}
function ManualRequest3() {
    $('.fullname').select2({
        theme: 'bootstrap-5'
    }).on('change', function (e) {
        var id = $('.fullname option:selected').attr('value');
        console.log(id);
        $.ajax({
            type: 'GET',
            url: '/ServiceRequest/api/v1/users/getid/' + id,
            success: function (data) {
                $('#manual').find('input[name="fullName"]').val(data.fullName);
                $('#manual').find('input[name="email"]').val(data.email);
                $('#manual').find('input[name="mobileNumber"]').val(data.mobileNumber);
                $('#manual').find('select[name="departmentsId"]').val(data.departmentsId);
                $('#manual').find('select[name="divisionsId"]').val(data.divisionsId);

            },
            error: function (data) {

            }
        })

    });



    //$('.mobileNumber').select2({
    //    theme: 'bootstrap-5'
    //});

    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/v1/roles/users',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $.each(data, function (index, value) {
                $('select[name=fullName]').append('<option value="' + value.userId + '">' + value.fullName + '</option>');
            }
            );
        },
    });
    //$('.fullname').on('select2:select', function () {
    //    var userId = $(".fullname:selected").attr('data-select2-id');
    //    console.log(userId);
    //})




    //Department
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/department',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=departmentsId]').append('<option value=""> Select Department</option>');
            $.each(data, function (index, value) {
                $('select[name=departmentsId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },
    });
    //Division
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/division',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=divisionsId]').append('<option value=""> Select Division</option>');
            $.each(data, function (index, value) {
                $('select[name=divisionsId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });

    //UnitType
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/unitType',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $.each(data, function (index, value) {
                $('select[name=unitTypeId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });

    //hardwareCategory
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/hardware',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=hardwareId]').append('<option value=""> Select Hardware Service Category</option>');
            $.each(data, function (index, value) {
                $('select[name=hardwareId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    //Findings
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/Findings',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=findingId]').append('<option value=""> Select Result</option>');
            $.each(data, function (index, value) {
                $('select[name=findingId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },
    });

    //$('#btnSubmit').click(function (e) {
    //    e.preventDefault();
    //    var data = {
    //        fullName: $('input[name=fullName]').val(),
    //        email: $('input[name=email]').val(),
    //        mobileNumber: $('input[name=mobileNumber]').val(),
    //        departmentsId: $('select[name=departmentsId]').val(),
    //        divisionsId: $('select[name=divisionsId]').val(),
    //        hardwareId: $('#hardwareId').val(),
    //        modelName: $('#modelName').val(),
    //        brandName: $('#brandName').val(),
    //        unitTypeId: $('#unitTypeId').val(),
    //        documentLabel: $('#documentLabel').val(),
    //        description: $('#description').val(),

    //        possibleCause: $('#possibleCause').val(),
    //        findingId: $('#findingId').val(),
    //        serialNumber: $('#serialNumber').val(),
    //        controlNumber: $('#controlNumber').val(),
    //        dateStarted: $('#dateStarted').val(),
    //        dateEnded: $('#dateEnded').val(),
    //        timeStarted: $('#timeStarted').val(),
    //        timeEnded: $('#timeEnded').val(),
    //        remarks: $('#remarks').val(),
    //    };
    //    $.ajax({
    //        type: 'POST',
    //        url: '/ServiceRequest/api/manual/save/',
    //        data: data,
    //        headers: {
    //            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
    //        },
    //        success: function (data) {
    //            $('#pleasewait').modal('show');
    //            setTimeout(function () {
    //                toastr.success(" Successfully Create Hardware Request!");
    //                // hide please wait modal

    //            }, 2000);
    //            setTimeout(function () {
    //                window.location.href = "/ServiceRequest/HardwareRequest/Index";
    //            }, 3000);
    //        },
    //        //if failed
    //        error: function (data) {
    //            toastr.error("Name Already Exist In the Database/ Invalid")
    //        }
    //    });
    //});
}


function ManualRequest4() {
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/department',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            var html = '<option value="">Select Department</option>';
            $.each(data, function (i, item) {
                html += '<option value="' + item.id + '">' + item.name + '</option>';
            });
            $('select[name=departmentsId]').html(html);
            // render divisionsId select
            // console log on select change
            $('select[name=departmentsId]').on('change', function () {
                var depId = $('select[name=departmentsId]').val();
                $.ajax({
                    type: 'GET',
                    url: '/ServiceRequest/api/div/fetchbyid/' + depId,
                    headers: {
                        'Authorization': 'Bearer ' + localStorage.getItem('access_token')
                    },
                    success: function (data) {
                        var html = '<option value="0">Select Division</option>';
                        $.each(data, function (i, item) {
                            html += '<option value="' + item.id + '">' + item.name + '</option>';
                        });
                        //console.log(data);
                        $('select[name=divisionsId]').html(html);
                    }
                });
            });
        }
    });

    $('.departments').select2({
        theme: 'bootstrap-5'
    });

    //UnitType
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/unitType',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $.each(data, function (index, value) {
                $('select[name=unitTypeId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });

    //hardwareCategory
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/hardware',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=hardwareId]').append('<option value=""> Select Hardware Service Category</option>');
            $.each(data, function (index, value) {
                $('select[name=hardwareId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },

    });
    //Findings
    $.ajax({
        type: 'GET',
        url: '/ServiceRequest/api/hardwareRequest/Findings',
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('access_token')
        },
        success: function (data) {
            $('select[name=findingId]').append('<option value=""> Select Result</option>');
            $.each(data, function (index, value) {
                $('select[name=findingId]').append('<option value="' + value.id + '">' + value.name + '</option>');
            }
            );
        },
    });


//    $('#btnSubmit').click(function (e) {
//        e.preventDefault();
//        var data = {
//            fullName: $('input[name=fullName]').val(),
//            email: $('input[name=email]').val(),
//            mobileNumber: $('input[name=mobileNumber]').val(),
//            departmentsId: $('select[name=departmentsId]').val(),
//            divisionsId: $('select[name=divisionsId]').val(),
//            hardwareId: $('select[name=hardwareId]').val(),
//            modelName: $('input[name=modelName]').val(),
//            brandName: $('input[name=brandName]').val(),
//            unitTypeId: $('select[name=unitTypeId]').val(),
//            documentLabel: $('input[name=FileName]').val(),
//            description: $('textarea[name=description]').val(),

//            possibleCause: $('input[name=possibleCause]').val(),
//            findingId: $('select[name=findingId]').val(),
//            serialNumber: $('input[name=serialNumber]').val(),
//            controlNumber: $('input[name=controlNumber]').val(),
//            dateStarted: $('input[name=dateStarted]').val(),
//            dateEnded: $('input[name=dateEnded]').val(),
//            timeStarted: $('input[name=timeStarted]').val(),
//            timeEnded: $('input[name=timeEnded]').val(),
//            remarks: $('textarea[name=remarks]').val(),
//        };
//        $.ajax({
//            type: 'POST',
//            url: '/ServiceRequest/api/manual/save/',
//            data: data,
//            headers: {
//                'Authorization': 'Bearer ' + localStorage.getItem('access_token')
//            },
//            success: function (data) {
//                console.log(data);
//                $('#pleasewait').modal('show');
//                setTimeout(function () {
//                    toastr.success(" Successfully Create Hardware Request!");
//                    // hide please wait modal

//                }, 2000);
//                setTimeout(function () {
//                    window.location.href = "/ServiceRequest/HardwareRequest/Index";
//                }, 3000);
//            },
//            //if failed
//            error: function (data) {
//                toastr.error("Name Already Exist In the Database/ Invalid")
//                console.log(data)
//            }
//        });
//    });
}