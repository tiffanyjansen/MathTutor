function DatatableCreator() {
    this.table = null;
    this.domClass = null;
    this.tableID = null;
    this.columns = [];
    this.buttons = {};
    this.orderColumn = null;
    this.orderType = 'asc';
    this.$table = null;
    this.inputClass = null;
    this.skipFirst = false;

    this.init = function (opts) {
        this.table = opts.table;
        this.domClass = opts.domClass;
        this.tableID = opts.tableID;
        this.columns = opts.columns;
        this.buttons = opts.buttons;
        this.orderColumn = opts.orderColumn;
        this.orderType = 'asc';
        this.$table = $('#' + this.tableID);
        this.inputClass = opts.inputClass;
        this.skipFirst = opts.skipFirst;

        this.createTable();
        this.createSearch();
        this.bindEvents();
    }

    this.bindEvents = function () {
        var self = this;

        self.$table.on('column-sizing.dt', function (e, settings, column, state) {
            if (!$('.' + self.inputClass).length) {
                 self.createSearchFromValue(-1, 'Search');
                 self.bindEvents();
             }
        });

        self.$table.on('order.dt', function (e, settings, orderArr) {
            if (!$('.' + self.inputClass).length) {
                self.createSearchFromValue(-1, 'Search');
                self.bindEvents();
            }
        });

        $('.' + self.inputClass).on('keyup keydown', function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                e.stopPropagation();
            }

            var placeholder = $(this).attr('placeholder');
            var index = self.getColumnIndexFromPlaceholder(placeholder);

            if (self.table.column(index).search() !== this.value) {
                self.table.column(index).search(this.value);
                self.drawTable(this.value, placeholder);
            }
        });
    }

    this.getColumnIndexFromPlaceholder = function(placeholder) {
        var self = this;
        var title = placeholder.replace('Search ', '').trim();
        return self.getColumnIndexFromTitle(title);
    }

    this.getColumnIndexFromTitle = function (title) {
        var self = this;
        var index = 0;
        while (index < self.table.columns()[0].length) {
            var header = self.table.column(index).header();
            var col_title = $(header).text().trim();
            if (col_title == title) {
                return index;
            }
            index++;
        }
        return -1;
    }

    this.createTable = function () {
        var self = this;
        self.table = self.$table.DataTable({
            "paging": false,
            "ordering": true,
            "info": false,
            "scrollY": 310,
            "scrollX": true,
            "dom": self.domClass,
            "buttons": self.buttons,
            "bSortCellsTop": true,
            "columns": self.columns,
            "order": [[self.orderColumn, self.orderType]],
        });
    }

    this.createSearch = function () {
        var self = this;
        if ($('#' + self.tableID + ' thead tr').length == 1) {
            $('#' + self.tableID + ' thead tr').clone(true).appendTo('#' + self.tableID + ' thead');
        }
        $('#' + self.tableID + ' thead tr:eq(1) th').each(function (i) {
            if (self.skipFirst && i == 0) {
                this.innerHTML = '';
            } else {
                var title = $($('#' + self.tableID + ' thead tr:eq(0) th')[i]).text();
                $(this).removeClass('sorting sorting_asc sorting_desc');
                $(this).html('<input type="text" class="' + self.inputClass + '" placeholder="Search ' + title + '" />');
            }
        });
    }

    this.drawTable = function (value, placeholder) {
        var self = this;
        self.table.draw();
        self.createSearchFromValue(value, placeholder);
    }

    this.createSearchFromValue = function (value, placeholder) {
        var self = this;
        var index = self.getColumnIndexFromPlaceholder(placeholder);
        
        if($('#' + self.tableID + ' thead tr').length == 1) {
            $('#' + self.tableID + ' thead tr').clone(true).appendTo('#' + self.tableID + ' thead');
        }
        $('#' + self.tableID + ' thead tr:eq(1) th').each(function (i) {
            var title = $($('#' + self.tableID + ' thead tr:eq(0) th')[i]).text();
            var table_index = self.getColumnIndexFromTitle(title);
            var valuesByIndex = self.getCurrentValues();
            this.innerHTML = '';

            if (table_index == 0 && self.skipFirst) {
                this.innerHTML = '';
            } else {
                $(this).removeClass('sorting sorting_asc sorting_desc');

                if (valuesByIndex[table_index]) {
                    $(this).html('<input type="text" class="' + self.inputClass + '" placeholder="Search ' + title + '" value="' + valuesByIndex[table_index] + '" />');

                } else {
                    $(this).html('<input type="text" class="' + self.inputClass + '" placeholder="Search ' + title + '" />');
                }
                if (table_index == index) {
                    $(this).children()[0].focus();
                    $(this).children()[0].setSelectionRange(value.length, value.length);
                }
            }

        });
        self.bindEvents(); //rebind events
        console.log(value);
    }

    this.getCurrentValues = function() {
        var self = this;
        var retArray = [];

        var index = 0;
        while (index < self.table.columns()[0].length) {
            retArray[index] = self.table.column(index).search();
            index++;
        }
        return retArray;
    }
}