var Login = function () {
    this.init = function () {
        $("#loginForm").validate({
            rules: {
                UserNameOrEmail: {
                    required: true,
                    minlength: 3,
                    maxlength: 30
                },
                Password: {
                    required: true,
                    minlength: 5,
                    maxlength: 10
                },
            },
            messages: {
                UserNameOrEmail: {
                    required: "Please enter your UserName or Email",
                    minlength: "Your UserName must be at least 3 characters long",
                    maxlength: "Your UserName must be at most 30 characters long"
                },
                Password: {
                    required: "Please enter your Password",
                    minlength: "Your Password must be at least 5 characters long",
                    maxlength: "Your Password must be at most 10 characters long"
                },
            },
            errorClass: "text-danger",
            submitHandler: function (form) {
                $.ajax({
                    url: "/Account/Login",
                    type: "POST",
                    data: $(form).serialize(),
                    success: function (response) {
                        if (response.success) {
                           
                                window.location.href = "/UserTaskSummary/Index";
                            
                            
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
};
