var reload_call = function(){
    var pageURL = window.location.href;
    console.log(pageURL);
    var URLList = pageURL.split('/');
    var id = URLList[URLList.length - 1];
    console.log(id);
    var source = "/Home/SignIn/" + id;
    console.log(source);

}

var interval = 10000;
console.log("interval = " + interval);
window.setInterval(reload_call, interval);
console.log("We are in the Javascript!")