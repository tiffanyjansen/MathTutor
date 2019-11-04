$('.filter').change(function () {
    if ($(this).val() == "all") { //if we selected all in 1 dropdown, reset the page.
        $('.filter').each(function(index, element) {
            $(element).val("all");
        });
    }
    var currentData = $('#classIds').val(); //get the current class ids.
    if (currentData != '') {
        var array = currentData.split(','); //split it into an array.
    } else {
        var array = new Array(); //create new array
    }
    $('.class-checkbox').each(function (index, element) {        
        if ($(element)[0].checked) {
            array.push($(element)[0].value); //add new elements
        } else {
            if ($.inArray($(element)[0].value, array) != -1) {
                var pos = $.inArray($(element)[0].value, array); //if we find an unchecked one in the array, remove it.
                var removed = array.splice(pos, 1);
            }
        }
    });
    array = getUnique(array); //make the array unique
    $('#classIds').val(array); //set the classIds input (so we can access it)
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
    var currentData = $('#classIds').val(); //get the current class ids.
    if (currentData != '' && currentData != null &&  currentData != undefined) {
        var allClasses = currentData.split(','); //split it into an array.
    } else {
        var allClasses = new Array(); //create new array
    }    $('.class-checkbox').each(function (index, element) {
        if ($(element)[0].checked) {
            allClasses.push($(element)[0].value); //add new elements
        } else {
            if ($.inArray($(element)[0].value, allClasses) != -1) {
                var pos = $.inArray($(element)[0].value, allClasses); //if we find an unchecked one in the array, remove it.
                var removed = allClasses.splice(pos, 1);
            }
        }
    });
    allClasses = getUnique(allClasses); //make the array unique
    $('#selected-classes').val(allClasses);
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
    var class_count = $('#selected-count').text(); //get current class count
    if ($(this)[0].checked) {
        class_count++; //if we checked it, add 1
    } else {
        class_count--; //otherwise subtract one
    }

    //update total class count
    $('#selected-count').empty();
    $('#selected-count').text(class_count);
});