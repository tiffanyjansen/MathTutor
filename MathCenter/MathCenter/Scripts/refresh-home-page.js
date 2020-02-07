function debounce(func, wait, immediate) {
    var timeout;
    return function () {
        var context = this, args = arguments;
        var later = function () {
            timeout = null;
            if (!immediate) func.apply(context, args);
        };
        var callNow = immediate && !timeout;
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
        if (callNow) func.apply(context, args);
    };
};

var refresh = debounce(function () {
    console.log('refresh');
    location.reload();
}, 480000); // refresh page every 8 min. :)

$(window).on('keyup keydown mousedown mousemove', refresh);