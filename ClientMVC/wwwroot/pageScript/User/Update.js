var UpdateUser = function () {
    this.init = function () {
        $(document).on("click", ".updateUserRoleBtn", function (e) {
            e.preventDefault();
            var userId = $(this).data("id");

            $.ajax({
                url: "/Users/UpdateUserRolePartial",
                type: "GET",
                data: { id: userId },
                success: function (response) {
                    $("#updateUserRoleModal .modal-content").html(response);
                    $("#updateUserRoleModal").modal("show");

                    $(".select2").select2({
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

    }
}

