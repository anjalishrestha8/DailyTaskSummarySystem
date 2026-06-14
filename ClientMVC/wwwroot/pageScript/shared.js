var showSuccessMessage = function (message, callback) {
    debugger
    Swal.fire({
        icon: 'success',
        title: 'Success',
        text: message,
        confirmButtonText: 'OK'
    }).then(() => {
        callback()
    });
}
var showErrorMessage = function (message) {
    Swal.fire({
        icon: 'error',
        title: 'Error',
        text: message,
        confirmButtonText: 'OK'
    });
}

