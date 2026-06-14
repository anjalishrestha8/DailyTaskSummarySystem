var UserIndex = function () {
    this.init = function () {
        $("#userTable").DataTable({
            responsive: true,
            paging: true,
            searching: true,
            ordering: true,
            info: true
        });
       
        $(document).on("click", ".updateUserRoleBtn", function (e) {
            e.preventDefault();
            var userId = $(this).data("id");

            $.ajax({
                url: "/Users/UpdateUserRolePartial",
                type: "GET",
                data: { id: userId },
                success: function (response) {
                    $("#updateUserRoleModal").remove();
                    $("body").append(response);
                    $("#updateUserRoleModal").modal("show");

                    $(".select2").select2({
                        dropdownParent: $("#updateUserRoleModal"),
                        tags: true,
                        placeholder: "--Select Role--",
                        allowClear: true
                    });

                    $("#updateUserRoleForm").on("submit", function (e) {
                        e.preventDefault();
                        var form = this;
                        $.ajax({
                            url: "/Users/UpdateUserRole",
                            type: "POST",
                            data: $(form).serialize(),
                            success: function (resp) {
                                if (resp.success) {
                                    showSuccessMessage(resp.message, function () {
                                        location.reload();
                                    });
                                } else {
                                    showErrorMessage(resp.message);
                                }
                            },
                            error: function () {
                                showErrorMessage("An unexpected error occurred.");
                            }
                        });
                    });
                },
                error: function () {
                    showErrorMessage("Failed to load role data.");
                }
            });
        });

        $("#openAdminRegisterUserModal").on("click", function (e) {
            e.preventDefault();
            $.ajax({
                url: "/Users/AdminRegisterUserPartial",
                type: "GET",
                data: "html",
                success: function (response) {
                    $("#adminRegisterUserModal").remove();
                    $("body").append(response);
                    $("#adminRegisterUserModal").modal("show");

                    $(".flatpickr").flatpickr({
                        dropdownParent: $("#adminRegisterUserModal"),
                        dateFormat: "Y-m-d",
                        maxDate: "today",
                        minDate: "1940-01-01"
                    });
                    $(".select2").select2({
                        dropdownParent: $("#adminRegisterUserModal"),
                        tags: true,
                        placeholder: "--Select Role--",
                        allowClear: true
                    });

                    $("#adminRegisterUserForm").validate({
                        rules: {
                            UserName: {
                                required: true,
                                minlength: 3,
                                maxlength: 20
                            },
                            FullName: {
                                required: true,
                                minlength: 3,
                                maxlength: 30
                            },
                            Email: {
                                required: true,
                                email: true
                            },
                            Password: {
                                required: true,
                                strongPassword: true
                            },
                            DateOfBirth: {
                                required: true,
                                date: true,
                            }
                        },

                        messages: {
                            UserName: {
                                required: "Please enter your UserName",
                                minlength: "Your UserName must be at least 3 characters long",
                                maxlength: "Your UserName must be at most 20 characters long"
                            },
                            FullName: {
                                required: "Please enter your FullName",
                                minlength: "Your FullName must be at least 3 characters long",
                                maxlength: "Your FullName must be at most 30 characters long"
                            },
                            Email: {
                                required: "Please enter your Email address",
                            },
                            Password: {
                                required: "Please enter your Password",
                            },
                            DateOfBirth: {
                                required: "Please enter your Date of Birth",
                            }
                        },
                        errorClass: "text-danger"
                    });

                    $("#adminRegisterUserForm").on("submit", function (e) {
                        e.preventDefault();
                        var form = this;
                        $.ajax({
                            url: "/Users/Register",
                            type: "POST",
                            data: $(form).serialize(),
                            success: function (resp) {
                                if (resp.success) {
                                    showSuccessMessage(resp.message, function () {
                                        location.reload();
                                    });
                                } else {
                                    showErrorMessage(resp.message);
                                }
                            },
                            error: function () {
                                showErrorMessage("An unexpected error occurred.");
                            }
                        });
                    });
                },
                error: function () {
                    showErrorMessage("Failed to load register form.");
                }
            });
        });
    };
}