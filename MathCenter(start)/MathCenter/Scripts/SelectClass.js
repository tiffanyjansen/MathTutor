$('#Departments').change(function () {
    var id = $('#Departments').val().toString(); //get the department prefix.    
    var source = "/Ajax/GetNumbers/" + id;   //the source for the json object.                   
    $.ajax({ //ajax call
        type: "GET",
        dataType: "json",
        url: source,
        success: successDepartmentsAjax,
        error: errorAjax
    });
});
$('#Numbers').change(function () {
    var id = $('#Departments').val().toString(); //get the department prefix.
    var num = $('#Numbers').val().toString(); //get the class number. 
    var source = "/Ajax/GetInstructors/" + id + "/" + num;   //the source for the json object.                   
    $.ajax({ //ajax call
        type: "GET",
        dataType: "json",
        url: source,
        success: successInstructorsAjax,
        error: errorAjax
    });
});
$('#Instructors').change(function () {
    var id = $('#Departments').val().toString(); //get the department prefix.
    var num = $('#Numbers').val().toString(); //get the class number. 
    var instructor = $('#Instructors').val().toString(); //get the Instructor. 
    console.log("instructor = " + instructor);
    console.log(instructor == "Community College");
    if (instructor == "Community College") {
        var source = "/Ajax/GetTimes/" + id + "/" + num + "/" + instructor; //the source for the json object.                   
        $.ajax({ //ajax call
            type: "GET",
            dataType: "json",
            url: source,
            success: successCCAjax,
            error: errorAjax
        });
    }
    else {
        var source = "/Ajax/GetTimes/" + id + "/" + num + "/" + instructor; //the source for the json object.                   
        $.ajax({ //ajax call
            type: "GET",
            dataType: "json",
            url: source,
            success: successTimesAjax,
            error: errorAjax
        });
    }
});
function successDepartmentsAjax(numbers) {
    var json = JSON.parse(numbers); //parse the json object.
    if (json.length > 0) {
        $('#Numbers').empty(); //This clears the Drop Down.
        $('#Instructors').empty(); //clear the Instructor Drop Down
        $('#Times').empty(); //clear the Time Drop Down
        i = 0; //counter for the while loop
        while (i < json.length) {
            $('#Numbers').append('<option>' + json[i]["ClassNum"] + '</option>');
            i++; //increment the counter
        }

        var id = $('#Departments').val().toString(); //get the department prefix.
        var num = $('#Numbers').val().toString(); //get the class number. 
        var source = "/Ajax/GetInstructors/" + id + "/" + num;   //the source for the json object.                   
        $.ajax({ //ajax call
            type: "GET",
            dataType: "json",
            url: source,
            success: successInstructorsAjax,
            error: errorAjax
        });
    }
}
function successInstructorsAjax(instructors) {
    var json = JSON.parse(instructors); //parse the json object.
    if (json.length > 0) {
        $('#Instructors').empty(); //clear the Drop Down
        $('#Times').empty(); //clear the Time Drop Down
        i = 0; //counter for the while loop
        while (i < json.length) {
            $('#Instructors').append('<option>' + json[i]["Instructor"] + '</option>');
            i++; //increment the counter
        }
        $('#Instructors').append('<option>' + 'Community College' + '</option>');

        var id = $('#Departments').val().toString(); //get the department prefix.
        var num = $('#Numbers').val().toString(); //get the class number. 
        var instructor = $('#Instructors').val().toString(); //get the Instructor. 
        console.log("instructor = " + instructor);
        console.log(instructor == "Community College");
        if (instructor == "Community College") {
            var source = "/Ajax/GetTimes/" + id + "/" + num + "/" + instructor; //the source for the json object.                   
            $.ajax({ //ajax call
                type: "GET",
                dataType: "json",
                url: source,
                success: successCCAjax,
                error: errorAjax
            });
        }
        else {
            var source = "/Ajax/GetTimes/" + id + "/" + num + "/" + instructor; //the source for the json object.                   
            $.ajax({ //ajax call
                type: "GET",
                dataType: "json",
                url: source,
                success: successTimesAjax,
                error: errorAjax
            });
        }
    }
}
function successTimesAjax(times) {
    console.log("In the normal success.");
    var json = JSON.parse(times); //parse the json object.
    if (json.length > 0) {
        $('#LabelTimes').empty(); //clear the Label
        $('#LabelTimes').append('Class Time')
        $('#Times').empty(); //clear the Drop Down
        i = 0; //counter for the while loop
        while (i < json.length) {
            $('#Times').append('<option value=' + json[i]["ClassID"] + '>' + json[i]["Time"] + ' ' + json[i]["Days"] + '</option>');
            i++; //increment the counter
        }
    }
}
function successCCAjax(times) {
    console.log("In the Community College success.");
    var json = JSON.parse(times); //parse the json object.
    if (json.length > 0) {
        $('#LabelTimes').empty(); //clear the Label
        $('#LabelTimes').append('Institution')
        $('#Times').empty(); //clear the Drop Down
        i = 0; //counter for the while loop
        while (i < json.length) {
            $('#Times').append('<option value=' + json[i]["ClassID"] + '>' + json[i]["Instructor"] + ' Community College' + '</option>');
            i++; //increment the counter
        }
    }
}
//This is the error message.
function errorAjax() {
    console.log("An error has occurred");
}