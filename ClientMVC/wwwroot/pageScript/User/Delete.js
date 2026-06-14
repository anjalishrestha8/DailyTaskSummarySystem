var UserDelete = function () {
    this.init = function () {
        $("#userDeleteForm").on("submit", function (e) {
            e.preventDefault();
            var form = $(this);
            var id = form.find('input[name="Id"]').val();

            $.ajax({
                url: "/Users/DeleteConfirmed",
                type: "POST",
                data: { id: id },
                success: function (response) {
                    if (response.success) {
                        showSuccessMessage(response.message, function () {
                            window.location.href = "/Users/Index";
                        })
                    } else {
                        showErrorMessage(response.message);
                    }
                },
                error: function (xhr, status, error) {
                    showErrorMessage('An unexpected error occurred. Please try again later.');
                }
            });
        });
    }
};