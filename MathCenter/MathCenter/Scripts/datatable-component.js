function DatatableComponent () {
    this.table = null;
    this.domClass = null;
    this.$table = null;
    this.buttons = false;
    this.orderColumn = null;
    this.orderType = 'asc';
    this.columns = [];
    this.tableID = null;
    this.inputClass = null;

    this.init = function (opts) {
        this.table = opts.table;
        this.domClass = opts.domClass;
        this.$table = opts.$table;
        if (opts.buttons) {
            this.buttons = opts.buttons;
        }
        this.orderColumn = opts.orderColumn;
        if (opts.orderType) {
            this.orderType = opts.orderType;
        }
        this.columns = opts.columns;
        this.tableID = opts.tableID;
        this.inputClass = opts.inputClass;

        this.createDataTable();
        this.createSearch();
        this.bindEvents();
    }

    this.bindEvents = function () {
        var self = this;

        $('.sorting').on('click', function () {
            self.createSearch();
        });

        $('.sorting_asc').on('click', function () {
            self.createSearch();
        });

        $('.sorting_desc').on('click', function () {
            self.createSearch();
        });
    }

    this.createDataTable = function () {
        var self = this;
        if (self.buttons) {
            self.table = self.$table.DataTable({
                "paging": false,
                "ordering": true,
                "info": false,
                "scrollY": 310,
                "scrollX": true,
                "dom": self.domClass,
                "buttons": {
                    "buttons": [
                        { extend: 'colvis', className: 'btn-danger' },
                    ]
                },
                "bSortCellsTop": true,
                "columns": self.columns,
                "order": [[self.orderColumn, self.orderType]],
            });
        } else {
            self.table = self.$table.DataTable({
                "paging": false,
                "ordering": true,
                "info": false,
                "scrollY": 310,
                "scrollX": true,
                "dom": self.domClass,
                "bSortCellsTop": true,
                "columns": self.columns,
                "order": [[self.orderColumn, self.orderType]],
            });
        }
    }

    this.createSearch = function () {
        var self = this;
        $('#' + self.tableID + ' thead tr').clone(true).appendTo('#' + self.tableID + ' thead');
        $('#' + self.tableID + ' thead tr:eq(1) th').each(function (i) {
            if (i != 0) {
                var title = $(this).text();
                $(this).removeClass('sorting sorting_asc sorting_desc');
                $(this).html('<input type="text" class="' + self.inputClass + '" placeholder="Search ' + title + '" />');

                $('input', this).on('keyup keydown change', function (e) {
                    if (e.keyCode == 13) {
                        e.preventDefault();
                        e.stopPropagation();
                    }
                    if (self.table.column(i).search() !== this.value) {
                        self.table.column(i).search(this.value);
                        self.drawTable(this.value, i);
                    }
                });
            } else {
                this.innerHTML = '';
            }
        });
    }

    this.drawTable = function (value, index) {
        var self = this;
        self.table.draw();
        self.createSearchFromValue(value, index);
    }

    this.createSearchFromValue = function (value, index) {
        var self = this;
        $('#' + self.tableID + ' thead tr').clone(true).appendTo('#' + self.tableID + ' thead');
        $('#' + self.tableID + ' thead tr:eq(1) th').each(function (i) {
            if (i != 0) {
                var title = $(this).text();
                $(this).removeClass('sorting sorting_asc sorting_desc');
                if (i != index) {
                    $(this).html('<input type="text" class="' + self.inputClass + '" placeholder="Search ' + title + '" />');
                } else {
                    $(this).html('<input type="text" class="' + self.inputClass + '" placeholder="Search ' + title + '" value="' + value + '" />');
                    $(this).children()[0].focus();
                    $(this).children()[0].setSelectionRange(value.length, value.length);
                }

                $('input', this).on('keyup keydown change', function (e) {
                    if (e.keyCode == 13) {
                        e.preventDefault();
                        e.stopPropagation();
                    }
                    if (self.table.column(i).search() !== this.value) {
                        self.table.column(i).search(this.value);
                        self.drawTable(this.value, i);
                    }
                });
            } else {
                this.innerHTML = '';
            }
        });
    }
}