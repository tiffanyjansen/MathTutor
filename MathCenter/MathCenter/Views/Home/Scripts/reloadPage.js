//The function for redirecting the page to the "Sign In" Page.
var reload_call = function () {
    var pageURL = window.location.href; //Get the current url.
    var URLList = pageURL.split('/'); //Split the url Up by /s
    var id = URLList[URLList.length - 1];//Get the last part of the url (Week #)
    var source = "/Home/SignIn/" + id; //Create the new source to be redirected to
    window.location.href = source;//Use it to redirect there.
}

//This is the timing part, so the page is only there for 'x' seconds before redirecting.
var interval = 15000; //This is the 'x' part. It will be (interval/1000) seconds.
window.setInterval(reload_call, interval); //Call the function above on the given interval.

