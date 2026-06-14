var ForgotPassword = function () {
    this.init = function () {
        $("#ForgotPasswordForm").validate({
            rules: {
                Email: {
                    required: true,
                    email: true
                }
            },
            messages: {
                Email: {
                    required: "Please enter your Email address"
                }
            },
            errorClass: "text-danger",
            submitHandler: function (form) {
                $.ajax({
                    url: "/Account/ForgotPassword",
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
