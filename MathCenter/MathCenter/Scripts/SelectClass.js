$('.filter').change(function () {
    if ($(this).val() == "all") {
        $('.filter').each(function(index, element) {
            $(element).val("all");
        });
    }
    var currentData = $('#classIds').val();
    if (currentData != '') {
        var array = currentData.split(',');
    } else {
        var array = new Array();
    }
    $('.class-checkbox').each(function (index, element) {        
        if ($(element)[0].checked) {
            array.push($(element)[0].value);
        } else {
            if ($.inArray($(element)[0].value, array) != -1) {
                var pos = $.inArray($(element)[0].value, array);
                var removed = array.splice(pos, 1);
            }
        }
    });
    array = getUnique(array);
    $('#classIds').val(array);
    document.filterForm.submit(); //submit the get form
});

function getUnique(array) {
    //source: https://www.tutorialrepublic.com/faq/how-to-remove-duplicate-values-from-a-javascript-array.php
    var uniqueArray = [];

    // Loop through array values
    for (i = 0; i < array.length; i++) {
        if (uniqueArray.indexOf(array[i]) === -1) {
            uniqueArray.push(array[i]);
        }
    }
    return uniqueArray;
}

function submitForm(route = null) {
    if (route != null) {
        var currentRoute = document.classForm.action;
        var split = currentRoute.split('/');
        split.pop();
        split.push(route);
        var newRoute = split.join('/');
        document.classForm.action = newRoute; //set the route of the form to the new route. :)
    }    
    document.classForm.submit(); //submit the post form
}

$('.class-checkbox').click(function () {
    //count the number of checked classes
    var class_count = $('#selected-count').text();
    console.log(class_count);
    console.log($(this));
    if ($(this)[0].checked) {
        class_count++;
    } else {
        class_count--;
    }

    //update total class count
    $('#selected-count').empty();
    $('#selected-count').text(class_count);
});