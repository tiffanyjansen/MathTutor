$('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
    $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
    createSearches();
});

function drawTable(table, value, column) {
    table.draw();
    if (table == window.table) {
        //createTableSearch(column, value);
    } else if (table == window.comTable) {
        createComTableSearch(column, value);
    } else if (table == window.otherTable) {
        createOtherTableSearch(column, value);
    } else {
        createSearches();
    }
}

function createComTableSearch(column, value) {
    $('#comm_select_table thead tr').clone(true).appendTo('#comm_select_table thead');
    $('#comm_select_table thead tr:eq(1) th').each(function (i) {
        if (i != 0) {
            var title = $(this).text();
            $(this).removeClass('sorting sorting_asc sorting_desc');
            if (i != column) {
                $(this).html('<input type="text" placeholder="Search ' + title + '" />');
            } else {
                $(this).html('<input type="text" placeholder="Search ' + title + '" value="' + value + '" />');
            }

            $('input', this).on('keyup keydown change', function (e) {
                if (e.keyCode == 13) {
                    e.preventDefault();
                    e.stopPropagation();
                }
                if (window.comTable.column(i).search() !== this.value) {
                    window.comTable.column(i).search(this.value);
                    drawTable(window.comTable, this.value, i);
                }
            });
        } else {
            this.innerHTML = '';
        }
    });
}

function createOtherTableSearch(column, value) {
    $('#other_select_table thead tr').clone(true).appendTo('#other_select_table thead');
    $('#other_select_table thead tr:eq(1) th').each(function (i) {
        if (i != 0) {
            var title = $(this).text();
            $(this).removeClass('sorting sorting_asc sorting_desc');
            if (i != column) {
                $(this).html('<input type="text" placeholder="Search ' + title + '" />');
            } else {
                $(this).html('<input type="text" placeholder="Search ' + title + '" value="' + value + '" />');
            }

            $('input', this).on('keyup keydown change', function (e) {
                if (e.keyCode == 13) {
                    e.preventDefault();
                    e.stopPropagation();
                }
                if (window.otherTable.column(i).search() !== this.value) {
                    window.otherTable.column(i).search(this.value);
                    drawTable(window.otherTable, this.value, i);
                }
            });
        } else {
            this.innerHTML = '';
        }
    });
}


function createSearches() {
    //$('#classes_select_table thead tr').clone(true).appendTo('#classes_select_table thead');
    //$('#classes_select_table thead tr:eq(1) th').each(function (i) {
    //    if (i != 0) {
    //        var title = $(this).text();
    //        $(this).removeClass('sorting sorting_asc sorting_desc');
    //        $(this).html('<input type="text" placeholder="Search ' + title + '" />');

    //        $('input', this).on('keyup keydown change', function (e) {
    //            if (e.keyCode == 13) {
    //                e.preventDefault();
    //                e.stopPropagation();
    //            }
    //            if (window.table.column(i).search() !== this.value) {
    //                window.table.column(i).search(this.value);
    //                drawTable(window.table, this.value, i);
    //            }
    //        });
    //    } else {
    //        this.innerHTML = '';
    //    }
    //});

    $('#comm_select_table thead tr').clone(true).appendTo('#comm_select_table thead');
    $('#comm_select_table thead tr:eq(1) th').each(function (i) {
        if (i != 0) {
            var title = $(this).text();
            $(this).removeClass('sorting sorting_asc sorting_desc');
            $(this).html('<input type="text" placeholder="Search ' + title + '" />');

            $('input', this).on('keyup keydown change', function (e) {
                if (e.keyCode == 13) {
                    e.preventDefault();
                    e.stopPropagation();
                }
                if (window.comTable.column(i).search() !== this.value) {
                    window.comTable.column(i).search(this.value);
                    drawTable(window.comTable, this.value, i);
                }
            });
        } else {
            this.innerHTML = '';
        }
    });

    $('#other_select_table thead tr').clone(true).appendTo('#other_select_table thead');
    $('#other_select_table thead tr:eq(1) th').each(function (i) {
        if (i != 0) {
            var title = $(this).text();
            $(this).removeClass('sorting sorting_asc sorting_desc');
            $(this).html('<input type="text" placeholder="Search ' + title + '" />');

            $('input', this).on('keyup keydown change', function (e) {
                if (e.keyCode == 13) {
                    e.preventDefault();
                    e.stopPropagation();
                }
                if (window.otherTable.column(i).search() !== this.value) {
                    window.otherTable.column(i).search(this.value);
                    drawTable(window.otherTable, this.value, i);
                }
            });
        } else {
            this.innerHTML = '';
        }
    });
}