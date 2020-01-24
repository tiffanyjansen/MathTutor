$(document).ready(function () {
    var $progressWrapper = $('.progress-bar-mine');
    var $row = $('<div class="row"></div>');
    var $col = $('<div class="col-10 offset-1"></div>');
    var $progress = $('<div class="progress"></div>');
    $progress.append('<div class="progress-bar progress-bar-striped progress-bar-animated bg-danger" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width:66%">66% Complete</div>');
    $col.append($progress);
    $row.append($col);
    $progressWrapper.append($row);
});