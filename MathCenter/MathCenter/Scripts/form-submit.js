function submitCCForm() {
    event.preventDefault();
    event.stopPropagation();

    var inputs = $('.cc-college-input');
    var data = {}
    $.each(inputs, function (index, value) {
        var name = value.name;
        data[name] = value.value;
    });
    submitForm(data);
}

function submitOtherForm() {
    event.preventDefault();
    event.stopPropagation();

    var inputs = $('.other-input');
    var data = {}
    $.each(inputs, function (index, value) {
        var name = value.name;
        data[name] = value.value;
    });
    submitForm(data);
}

function submitForm(data) {
    var url = '/api/classes?';
    $.each(data, function (name, value) {
        url += name + "=" + value + "&";
    });
    url = url.substring(0, url.length - 1);

    $.ajax({ //The Ajax call
        type: "GET",
        dataType: "json",
        url: url,
        success: successAjax,
        error: errorAjax
    });
}

function successAjax(data) {
    location.reload(true); //reload the page to get the new classes.
    //Find a better solution for this.
}

function errorAjax(data) {
    console.log(data);
}