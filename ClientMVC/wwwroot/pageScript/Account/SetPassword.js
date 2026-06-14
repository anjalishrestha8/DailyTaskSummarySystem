var SetPassword = function () {
    this.init = function () {
        $.validator.addMethod("strongPassword", function (value, element) {
            return this.optional(element) ||
                /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{5,10}$/.test(value);
        }, "Password must be 5–10 characters, uppercase, lowercase, number, symbol.");

        $("#setPasswordForm").validate({
            rules: {
                NewPassword: { required: true, strongPassword: true },
                ConfirmPassword: { required: true, equalTo: "#NewPassword" }
            },
            messages: {
                NewPassword: { required: "Enter your new password" },
                ConfirmPassword: { required: "Confirm your password", equalTo: "Passwords do not match" }
            },
            errorClass: "text-danger",
            submitHandler: function (form) {
                $.ajax({
                    url: "/Account/SetPassword",
                    type: "POST",
                    data: $(form).serialize(),
                    success: function (response) {
                        if (response.success) {
                            showSuccessMessage(response.message, function () {
                                window.location.href = "/Account/Login";
                            })
                        } else {
                            showErrorMessage(response.message);
                        }
                    },
                    error: function (xhr, status, error) {
                        showErrorMessage('An unexpected error occurred. Please try again later.');
                    }
                });
            }
        });
    }
}
