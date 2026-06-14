var UserTaskSummaryIndex = function () {
    var url;
    var type;
    this.init = function () {

        $(document).on('click', '#btnSaveSummary', function () {
            if ($.validator && $("#summaryForm").data("validator")) {
                $("#summaryForm").validate().destroy();
            }
            $('#summaryForm').validate({
                rules: {
                    Title: {
                        required: true,
                        minlength: 3
                    },
                    Description: {
                        required: true,
                        minlength: 5
                    }
                },
                messages: {
                    Title: {
                        required: "Please enter summary name",
                        minlength: "Title must be at least 3 characters"
                    },
                    Description: {
                        required: "Please enter summary description",
                        minlength: "Description must be at least 5 characters"
                    },
                },
                errorClass: "text-danger",
                submitHandler: function (form) {
                    $.ajax({
                        url: url,
                        data: $(form).serialize(),
                        type: type,
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
                }
            });

            $('#summaryForm').trigger('submit')
        })

        $("#btnAddSummary").on("click", function () {
            url = "/UserTaskSummary/AddTaskSummary"
            type = 'post'
            $.get("/UserTaskSummary/AddSummaryPartial", function (partialView) {
                $('#summaryModal').remove();
                $("body").append(partialView);
                $("#summaryModal").modal('show');
            });
        });

        $(document).on("click", ".edit-summary", function (e) {
            e.preventDefault();
            var summaryId = $(this).data("id");
            url = "/UserTaskSummary/UpdateSummary?summaryId=" + summaryId;
            type = "PUT";

            $.get("/UserTaskSummary/UpdateSummaryPartial?summaryId=" + summaryId, function (partialView) {
                $('#summaryModal').remove();
                $("body").append(partialView);
                $("#summaryModal").modal('show');
            });

            //$.ajax({
            //    url: url,
            //    type: 'get',
            //    dataType: 'html',
            //    success: function (res) {
            //        $("body").append(res);
            //        $("#summaryModal").modal('show');
            //    },
            //    error: function (err, er, e) {
            //        showErrorMessage('Unexpected error occurred.');
            //    }
            //});
        });


        $(document).on("hidden.bs.modal", "#summaryModal", function () {
            $("#summaryModal").remove();
        });

        var startDatePickr = flatpickr("#fromDate", {
            dateFormat: "Y-m-d",
            maxDate:"today",
            onChange: function (selectedDate, dateStr) {
                endDatePickr.set('minDate', dateStr);
            }
        });
        var endDatePickr = flatpickr("#toDate", {
            dateFormat: "Y-m-d",
            maxDate: "today",
            onChange: function (selectedDate, dateStr) {
                startDatePickr.set('maxDate', dateStr);
            }
        });

        function loadTaskSummary(pageNumber = 1) {
            var searchTermVal = $("#searchTerm").val();
            var sortByVal = $("#sortBy").val();
            var sortOrderVal = $("#sortOrder").val();
            var fromDateVal = $("#fromDate").val();
            var toDateVal = $("#toDate").val();
            var pageSize = 10;
            $.ajax({
                url: "/UserTaskSummary/Index",
                type: "get",
                data: {
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    searchTerm: searchTermVal,
                    sortBy: sortByVal,
                    sortOrder: sortOrderVal,
                    fromDate: fromDateVal,
                    toDate: toDateVal,
                },
                success: function (res) {
                    $("#userTaskSummaryPartial").html(res);
                },
                error: function () {
                    showErrorMessage: "Failed to load summary"
                }
            });
        }

        $("#searchBtn").on("click", function (e) {
            e.preventDefault();

            var fromDate = $("#fromDate").val();
            var toDate = $("#toDate").val();

            var today = new Date();
            var formattedDate = today.toISOString().split('T')[0];
            var aMonthAgo = new Date(today.getFullYear(), today.getMonth() - 1, today.getDate());


            if (fromDate && !toDate) {
                endDatePickr.setDate(formattedDate);
                toDate = formattedDate;

                fromDateVal = new Date(fromDate);
                toDateVal = new Date(formattedDate);
            }
            if (!fromDate && toDate) {
                startDatePickr.setDate(aMonthAgo);
                fromDate = aMonthAgo;

                fromDateVal = new Date(aMonthAgo);
                toDateVal = new Date(toDate);
            }
            loadTaskSummary();
        });

        $(document).on("click", ".pagination .pagination-btn", function (e) {
            e.preventDefault();
            var page = $(this).data("page");
            loadTaskSummary(page);
        });
       
    };
};