function SignIn() {
    var VNumber = $('#VNum').val.ToString();
    if (VNumber.length != 9) {
        error();
    }
    else {
        success();
    }
}

function error() {
    $('#Error h4').add("Your V Number is invalid. It must be 8 characters. Please do not include the V.");
}

function success() {
    $('#VButton').remove();
}