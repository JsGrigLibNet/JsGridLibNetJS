

var gridAppBuilder = function (page, gridElement) {
    var ele = document.getElementById('container');
    if (ele) {
        ele.style.visibility = "visible";
    }
    gridElement = gridElement || '#Grid';
    //var page = "Orders";

    //var hostUrl = 'https://ej2services.syncfusion.com/production/web-services/';
    var hostUrl = '';
    var data = function (tpe) {
        return new ej.data.DataManager({
            url: hostUrl + 'api/' + tpe,
            adaptor: new ej.data.WebApiAdaptor(),
            crossDomain: true,
            //offline:true
        });
    };

    $.get("api/" + page + "Schema/Get?id=0").done(function (composite) {
        console.log(composite);
        var col = [{ type: 'checkbox', allowFiltering: false, allowSorting: false, width: '60' }];
        col = col.concat(composite.FieldsReadable);
        col.push({
            headerText: 'Actions', width: 160,
            commands: [{ type: 'Edit', buttonOption: { iconCss: ' e-icons e-edit', cssClass: 'e-flat' } },
            { type: 'Delete', buttonOption: { iconCss: 'e-icons e-delete', cssClass: 'e-flat' } },
            { type: 'Save', buttonOption: { iconCss: 'e-icons e-update', cssClass: 'e-flat' } },
            { type: 'Cancel', buttonOption: { iconCss: 'e-icons e-cancel-icon', cssClass: 'e-flat' } }]
        });
        //headerTemplate: '#employeetemplate'
        //    rowTemplate: '#rowtemplate',
        //https://ej2.syncfusion.com/demos/#/material/grid/grid-overview.html
        //https://ej2.syncfusion.com/javascript/documentation/grid/columns/?_ga=2.243944193.500114582.1549739902-2018847989.1549546969#column-template
        var grid = new ej.grids.Grid({
            // rowTemplate: "#hello", 
            dataSource: data(page),
            allowReordering: true,
            allowResizing: true,
            actionComplete: function (e) {
                if (e.requestType == 'save') {
                    grid.refresh();
                }
            }, 
            contextMenuItems: ['AutoFit', 'AutoFitAll', 'SortAscending', 'SortDescending',
                'Copy', 'Edit', 'Delete', 'Save', 'Cancel',
                'PdfExport', 'ExcelExport', 'CsvExport', 'FirstPage', 'PrevPage',
                'LastPage', 'NextPage'],
            allowExcelExport: true,
            allowPdfExport: true,
            selectionSettings: { type: 'Multiple' },
            allowTextWrap: true,
            allowRowDragAndDrop: true,
            groupSettings: { showGroupedColumn: true },
            showColumnMenu: true,
            //Default,none, Both, Horizontal,Vertical
            gridLines: 'Default',
            hierarchyPrintMode: 'All',
            allowSorting: true,
            allowFiltering: true,
            filterSettings: { type: 'Excel'/*or Menu */ },
            //allowCsvExport: true,
            editSettings: {
                allowEditing: true,
                allowAdding: true,
                allowDeleting: true,
                //mode: 'Normal',
                newRowPosition: 'Top',
                //remove this to edit inline
                mode: 'Dialog',
                showDeleteConfirmDialog: true
            },
            allowPaging: true,
            pageSettings: {
                pageSize: 12,
                pageSizes: true,
                enableQueryString: true,
                currentPage: parseInt((function (name) {
                    var url = window.location.href;
                    name = name.replace(/[\[\]]/g, '\\$&');
                    var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
                        results = regex.exec(url);
                    if (!results) return null;
                    if (!results[2]) return '';
                    return decodeURIComponent(results[2].replace(/\+/g, ' '));
                })('page'), 10) || 1
            },
            toolbar: ['Print', 'ExcelExport', 'PdfExport'/*, 'CsvExport' */, 'Add', 'Edit', 'Delete', 'Update', 'Cancel', 'Search',

                { text: 'Copy', tooltipText: 'Copy', prefixIcon: 'e-copy', id: 'copy' },
                { text: 'Copy With Header', tooltipText: 'Copy With Header', prefixIcon: 'e-copy', id: 'copyHeader' }
            ],

            actionBegin: actionBegin,
            columns: col,

            toolbarClick: function (args) {
                if (grid.getSelectedRecords().length > 0) {
                    var withHeader = false;
                    if (args.item.id === 'copyHeader') {
                        withHeader = true;
                    }
                    grid.copy(withHeader);
                } else {
                    alert("Please select row before copying");
                }
            }
            //detailTemplate: '#detailtemplate',
            //childGrid: {
            //    dataSource: data('Orders'),
            //    queryString: 'IdForeign',
            //    hierarchyPrintMode: 'All',
            //    columns: col
            //}
        });


        grid.appendTo(gridElement);
        grid.toolbarClick = function (args) {
            if (args.item.id === 'Grid_pdfexport') {
                grid.pdfExport();
            }
            if (args.item.id === 'Grid_excelexport') {
                // grid.excelExport();
                grid.excelExport(getExcelExportProperties());
            }
            if (args.item.id === 'Grid_csvexport') {
                grid.csvExport();
            }
        };
        function actionBegin(args) {
            if (args.requestType === 'save') {
                if (grid.pageSettings.currentPage !== 1 && grid.editSettings.newRowPosition === 'Top') {
                    args.index = (grid.pageSettings.currentPage * grid.pageSettings.pageSize) - grid.pageSettings.pageSize;
                } else if (grid.editSettings.newRowPosition === 'Bottom') {
                    args.index = (grid.pageSettings.currentPage * grid.pageSettings.pageSize) - 1;
                }
            }
        }

        //var dropDownType = new ej.dropdowns.DropDownList({
        //    dataSource: newRowPosition,
        //    fields: {
        //        text: 'newRowPosition',
        //        value: 'id'
        //    },
        //    value: 'Top',
        //    change: function (e) {
        //        var newRowPosition = e.value;
        //        grid.editSettings.newRowPosition = newRowPosition;
        //    }
        //});

        var date = '';
        date += ((new Date()).getMonth().toString()) + '/' + ((new Date()).getDate().toString());
        date += '/' + ((new Date()).getFullYear().toString());
        function getExcelExportProperties() {
            return {

                footer: {
                    footerRows: 8,
                    rows: [
                        { cells: [{ colSpan: 6, value: "Thank you for your business!", style: { fontColor: '#C67878', hAlign: 'Center', bold: true } }] },
                        { cells: [{ colSpan: 6, value: "!Visit Again!", style: { fontColor: '#C67878', hAlign: 'Center', bold: true } }] }
                    ]
                },

                fileName: "My excel.xlsx",
                /*
                header: {
                    headerRows: 7,
                    rows: [
                        { index: 1, cells: [{ index: 1, colSpan: 5, value: 'INVOICE', style: { fontColor: '#C25050', fontSize: 25, hAlign: 'Center', bold: true } }] },
                        {
                            index: 3,
                            cells: [
                                { index: 1, colSpan: 2, value: "Advencture Traders", style: { fontColor: '#C67878', fontSize: 15, bold: true } },
                                { index: 4, value: "INVOICE NUMBER", style: { fontColor: '#C67878', bold: true } },
                                { index: 5, value: "DATE", style: { fontColor: '#C67878', bold: true }, width: 150 }
                            ]
                        },
                        {
                            index: 4,
                            cells: [{ index: 1, colSpan: 2, value: "2501 Aerial Center Parkway" },
                            { index: 4, value: 2034 }, { index: 5, value: date, width: 150 }
        
                            ]
                        },
        
                        {
                            index: 5,
                            cells: [
                                { index: 1, colSpan: 2, value: "Tel +1 888.936.8638 Fax +1 919.573.0306" },
                                { index: 4, value: "CUSOTMER ID", style: { fontColor: '#C67878', bold: true } }, { index: 5, value: "TERMS", width: 150, style: { fontColor: '#C67878', bold: true } }
        
                            ]
                        },
                        {
                            index: 6,
                            cells: [
        
                                { index: 4, value: 564 }, { index: 5, value: "Net 30 days", width: 150 }
                            ]
                        }
                    ]
                }
                */
            };
        }
       // dropDownType.appendTo('#newRowPosition');
    });



};