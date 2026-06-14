var Register = function () {
    this.init = function () {

        $.validator.addMethod("strongPassword", function (value, element) {
            return this.optional(element) ||
                /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{5,10}$/.test(value);
        }, "Password must be 5–10 characters long, with uppercase, lowercase, number, and symbol.");
        
        $(".flatpickr").flatpickr({
            dateFormat: "Y-m-d",
            maxDate: "today",
            minDate: "1940-01-01"
        });
        $("#registerForm").validate({
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
                    email:true
                },
                Password: {
                    required: true,
                    strongPassword:true
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
            errorClass: "text-danger",
            submitHandler: function (form) {
                
                $.ajax({
                    url: "/Account/Register",
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