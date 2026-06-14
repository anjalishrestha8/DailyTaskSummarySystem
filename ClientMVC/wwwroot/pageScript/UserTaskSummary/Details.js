var ViewDetails = function () {
    this.init = function () {

        $("#commentForm").on("submit", function (e) {
            e.preventDefault();
            var form = $(this);
            $.ajax({
                url: "/UserTaskSummary/AddComment",
                data: $(form).serialize(),
                type: "POST",
                success: function (res) {
                    if (res.success) {
                        showSuccessMessage(res.message, function () {
                            location.reload();
                        });
                    }
                    else {
                        showErrorMessage(res.message);
                    }
                },
                error: function () {
                    showErrorMessage('Unexpected error occurred.');
                }
            });
        });

        $(document).on("click", ".edit-comment-btn", function () {
            var commentId = $(this).data("id");
            $.get("/UserTaskSummary/EditCommentPartial?commentId=" + commentId, function (partialView) {
                $('#editCommentModal').remove();
                $("body").append(partialView);
                $("#editCommentModal").modal('show'); 

                $(document).on("submit", "#editCommentForm", function (e) {
                    e.preventDefault();

                    var commentData = {
                        UserId: $("#editCommentUserId").val(),
                        UserTaskSummaryId: $("#editSummaryId").val(),
                        Content: $("#editCommentContent").val()
                    };

                    var commentId = $("#editCommentId").val();

                    $.ajax({
                        url: "/UserTaskSummary/UpdateComment?commentId=" + commentId,
                        type: "PUT",
                        contentType: "application/json",
                        data: JSON.stringify(commentData),
                        success: function (res) {
                            if (res.success) {
                                showSuccessMessage(res.message, function () {
                                    location.reload();
                                });
                            } else {
                                showErrorMessage(res.message);
                            }
                        },
                        error: function () {
                            showErrorMessage("Unexpected error occurred.");
                        }
                    });
                });

            });
        });
    };
};

