

var gridAppBuilder = function (jsGridRef, gridApp) {
    //========================extensions ========

    var MyDateField = function (config) {
        jsGrid.Field.call(this, config);
    };

    MyDateField.prototype = new jsGrid.Field({

        css: "date-field",            // redefine general property 'css'
        align: "center",              // redefine general property 'align'

        myCustomProperty: "foo",      // custom property

        sorter: function (date1, date2) {
            return new Date(date1) - new Date(date2);
        },

        itemTemplate: function (value) {
            return new Date(value).toDateString();
        },

        insertTemplate: function (value) {
            return this._insertPicker = $("<input>").datepicker({ defaultDate: new Date() });
        },

        editTemplate: function (value) {
            return this._editPicker = $("<input>").datepicker().datepicker("setDate", new Date(value));
        },

        insertValue: function () {
            return this._insertPicker.datepicker("getDate").toISOString();
        },

        editValue: function () {
            return this._editPicker.datepicker("getDate").toISOString();
        }
    });



    //========================END EXTENSIONS =============

    jsGrid.fields.date = MyDateField;

    var uuid = function () {

        if (gridApp.UniqueId) {
            return gridApp.UniqueId;
        } else {

            return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
                (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
            );
        }

    };

    var uniqueId = uuid();
    var selectedItems = [];
    gridApp.alert = gridApp.alert || function (o) { alert(o); };
    $.get(gridApp.GetSchemaAndSettings).done(function (composite) {
        console.log(composite);
        var data = composite.data;

        var fields = composite.fieldsReadable;

        var forEachFieldReadable = function (f) {
            for (let index = 0; index < composite.fieldsReadable.length; index++) {
                var element = composite.fieldsReadable[index];
                if (element.type !== "control") {
                    f(element, index, composite.fieldsReadable.length);
                }
            }
        };
        var forEachFieldUpdateable = function (f) {
            for (let index = 0; index < composite.fieldsUpdateable.length; index++) {
                var element = composite.fieldsUpdateable[index];
                f(element, index, composite.fieldsUpdateable.length);
            }
        };
        var forEachFieldDeletable = function (f) {
            for (let index = 0; index < composite.fieldsDeletable.length; index++) {
                var element = composite.fieldsDeletable[index];
                f(element, index, composite.fieldsDeletable.length);
            }
        };
        var forEachFieldCreatable = function (f) {
            for (let index = 0; index < composite.fieldsCreatable.length; index++) {
                var element = composite.fieldsCreatable[index];
                f(element, index, composite.fieldsCreatable.length);
            }
        };
        var jsGridSetup = {
            height:  gridApp.height ,
            width: composite.settings.width,
            editing: composite.settings.editing,
            autoload: composite.settings.autoload,
            paging: composite.settings.paging,
            sorting: false,

            //searchModeButtonTooltip: "Switch to searching", // tooltip of switching filtering/inserting button in inserting mode
            //insertModeButtonTooltip: "Switch to inserting", // tooltip of switching filtering/inserting button in filtering mode
            //editButtonTooltip: "Edit item",                      // tooltip of edit item button
            //deleteButtonTooltip: "Delete item",                  // tooltip of delete item button
            //searchButtonTooltip: "Search",                  // tooltip of search button
            //clearFilterButtonTooltip: "Clear filter",       // tooltip of clear filter button
            //insertButtonTooltip: "Insert",                  // tooltip of insert button
            //updateButtonTooltip: "Update",                  // tooltip of update item button
            //cancelEditButtonTooltip: "Cancel edit",         // tooltip of cancel editing button


        };
        var deleteClientsFromDb = function (deletingClients) {
            db.clients = $.map(db.clients, function (client) {
                return ($.inArray(client, deletingClients) > -1) ? null : client;
            });
        };
        var deleteSelectedItems = function () {
            if (!selectedItems.length || !confirm("Are you sure?"))
                return;
            
            var $grid = $("#jsGrid");
            $grid.jsGrid("option", "pageIndex", 1);
            $grid.jsGrid("loadData");

            selectedItems = [];
        };

       // var jsGridSetup = {};
        jsGridSetup.fields = [];
        jsGridSetup.fields.push({
            headerTemplate: function () {

                return operarionFactory("BatchOperations", {},"position: relative;");
            },
            itemTemplate: function (_, item) {
                return $("<input>").attr("type", "checkbox")
                    .prop("checked", $.inArray(item, selectedItems) > -1)
                    .on("change", function () {
                        $(this).is(":checked") ? selectItem(item) : unselectItem(item);
                    });
            },
            align: "center",
            width: 50
        });
        
        var operarionFactory = function (name, item, appendToStyle) {
            appendToStyle = appendToStyle || "";
            item = item || {};
            item.id = item.id || uniqueId;
            var temp = "";
            gridApp[name] = gridApp[name] || [];
            for (var i = 0; i < gridApp[name].length; i++) {
                var gop = gridApp[name][i];
                var id = "jsgridrowoperation-" + item.id + "-" + gop.id;
                if (!eventsBounded[id]) {
                    (function(g, thisitem, theid) {
                        $('body').on(g.event || "click",
                            "#" + theid,
                            function(e) {
                                e.stopPropagation();
                                g.handler && g.handler(thisitem, e, selectedItems);
                            });
                    })(gop, item, id);
                    eventsBounded[id] = true;
                }

                temp += `<li><a style="cursor:pointer" class="dropdown-toggle" id="` +
                    id +
                    `" >` +
                    gop.display +
                    `</a></li>`;
            }
            //var $text = $("<p>").text(item.id);
            //var $link = $("<a>").attr("href", item.id).text("Go To Item");
            //return $("<div>").append($text).append($link);
            var el = $(`
                            <div class="btn-group">
                                <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown">
                                ` +
                (gridApp[name+"Name"] || "Run") +
                ` <span class="caret"></span></button>
                                <ul class="dropdown-menu pull-right" role="menu" style="z-index:999 !important;    right: auto; left: auto; `+appendToStyle+` ">
                                ` +
                temp +
                `
                                </ul>
                            </div>
                        `);
            return el;
        };
        jsGridSetup.fields.push({
            type: "control",
            modeSwitchButton: false,
            editButton: true,
            headerTemplate: function () {
                return $("<button>").attr("type", "button").text(gridApp.CreateDialogTitle || "Add new")
                    .on("click", function () {
                        gridApp.showDetailsDialog("Add", {}, gridApp.CreateDialogTitle || "Add new");
                    });
            },
            itemTemplate: function (value, item) {
                return operarionFactory("RowOperations", item);
            },
            /*
                        itemTemplate: function (value, item) {
                            var $thisGrid = this._grid._container.context.id;
                            return this._createGridButton("my-button-class", "Click to Perform Custom Action", function (grid,e) {
                                e.stopPropagation();
                                 $('#' + $thisGrid + ' .' + this.jsGrid.ControlField.prototype.insertModeButtonClass).trigger('click');
                                gridApp.alert(item.id);
                                return true;
                                // grid.editItem(item);
                            });
                        }
             */
        });
        jsGridSetup.fields=jsGridSetup.fields.concat(fields);
        var eventsBounded = {};
        
        //jsGridSetup.fields.push({
        //    type: "date",
        //    modeSwitchButton: false,
        //    editButton: true
        //});
        jsGridSetup.pagerContainer = ".pagerContainer";
        jsGridSetup.controller = {
            loadData: function(filter) {
                var deferred = jQuery.Deferred();
                var res = $.ajax({
                    type: "GET",
                    url: gridApp.GetAll,
                    data: filter
                }).done(function(data) {
                    deferred.resolve(data.results);
                });
                return deferred.promise();
            },
            insertItem: function(item) {

                if (gridApp.GetValidationError) {
                    var error = gridApp.GetValidationError(item);
                    if (error) {
                        $(jsGridRef).jsGrid("cancelEdit");
                        gridApp.alert(error);
                        $(jsGridRef).jsGrid("loadData");
                        return;
                    }
                }

                var deferred = jQuery.Deferred();
                $.ajax({
                    type: "POST",
                    url: gridApp.Put,
                    data: item
                }).done(function(data) {
                    deferred.resolve(data);
                    $(jsGridRef).jsGrid("loadData");
                });
                return deferred.promise();

            },
            updateItem: function(item) {
                if (gridApp.GetValidationError) {
                    var error = gridApp.GetValidationError(item);
                    if (error) {
                        gridApp.alert(error);
                        $(jsGridRef).jsGrid("loadData");
                        return;
                    }
                }
                var deferred = jQuery.Deferred();
                $.ajax({
                    type: "POST",
                    url: gridApp.Post,
                    data: item
                }).done(function(data) {
                    deferred.resolve(data);
                    $(jsGridRef).jsGrid("loadData");
                });
                return deferred.promise();
            },
            onItemUpdating: function(args) {
                // cancel update of the item with empty 'name' field


            },
            onItemUpdated: function(args) {
                console.log(args);
            },

        };

        jsGridSetup.headerRowRenderer = gridApp.headerRowRenderer;
        jsGridSetup.rowRenderer = gridApp.rowRenderer;
            jsGridSetup.rowClass = function (item, itemIndex) { };
        jsGridSetup.rowClick = function (args) {
            //showDetailsDialog("Edit", args.item,  gridApp.UpdateDialogTitle || "Edit");
        };
        //jsGridSetup.deleteConfirm = gridApp.DeleteConfirm|| function (item) {
        //    return "Data will be deleted and may be IRREVERSIBLE!. Are you sure you want to do this ?";
        //};
        jsGridSetup.rowDoubleClick = function (args) { };
        jsGridSetup.invalidNotify = function (args) {
            var messages = $.map(args.errors, function (error) {
                return error.field + ": " + error.message;
            });

            console.log(messages);
        };

        gridApp.deleteItem = function (item) {
            var deferred = jQuery.Deferred();
            if (gridApp.GetValidationError) {
                var error = gridApp.GetValidationError(item);
                if (error) {
                    gridApp.alert(error);
                    $(jsGridRef).jsGrid("loadData");
                    deferred.resolve({});
                    return deferred.promise();
                }
            }

            $.ajax({
                type: "POST",
                url: gridApp.Delete,
                data: item
            }).done(function (data) {
                deferred.resolve(data);
                $(jsGridRef).jsGrid("loadData");
            });
            return deferred.promise();
        };

        jsGridSetup.loadIndicator = {
            show: function () {
                console.log("loading started");
            },
            hide: function () {
                console.log("loading finished");
            }
        };

       

        var selectItem = function (item) {
            selectedItems.push(item);
        };

        var unselectItem = function (item) {
            selectedItems = $.grep(selectedItems, function (i) {
                return i !== item;
            });
        };
        $(jsGridRef).jsGrid(jsGridSetup);


        var getDialogUI = function () {
            var dat = "";
            forEachFieldCreatable(function (e) {
                if (e.type === "select") {

                    var itemDom = "";
                    for (let index = 0; index < e.items.length; index++) {
                        const item = e.items[index];
                        itemDom += "<option value='" + item.id + "'>" + item.name + "</option>";
                    }
                    dat += `
                                     <div class='details-form-field'>
                                              <label for='` + e.name + `'>` + e.title + `:</label>
                                              <select id='` + e.name + `' name='` + e.name + `'>
                                                  <option value=''>(Select)</option>
                                                  ` + itemDom + `
                                              </select>
                                          </div>
                                        `;


                } else {
                    dat += `
                                         <div class='details-form-field'>
                                              <label for='` + e.name + `'>` + e.title + `:</label>
                                              <input id='` + e.name + `-` + uniqueId + `' name='` + e.name + `' type='` + e.type + `' />
                                          </div>
                                        `;
                }

            });


            return `
                                  <div id='detailsDialog` + `-` + uniqueId + `'>
                                      <form id='detailsForm` + `-` + uniqueId + `'>
                                         ` + dat + `
                                          <div class='details-form-field'>
                                              <button `+ (gridApp.SaveButtonAttribute || ``) + `  id='save` + `-` + uniqueId + `'>` + (gridApp.SaveButtonName || "Save Changes") + `</button>
                                          </div>
                                      </form>
                                  </div>
                      `;


        };

        $('body').prepend(getDialogUI())
        $("#detailsDialog" + `-` + uniqueId).dialog({
            autoOpen: false,
            width: 400,
            close: function () {
                $("#detailsForm" + `-` + uniqueId).validate().resetForm();
                $("#detailsForm" + `-` + uniqueId).find(".error").removeClass("error");
            }
        });

        var validationObject = $.ajax({
            type: "GET",
            url: gridApp.GetValidation,
            async: false
        }).responseJSON;
        validationObject.submitHandler = function () {
            formSubmitHandler();
        };
        $("#detailsForm" + `-` + uniqueId).validate(validationObject);


        var formSubmitHandler = $.noop;

        gridApp.showDetailsDialog = function (dialogType, client, title) {


            forEachFieldUpdateable(function (e) {
                if (e.type === "checkbox") {
                    $("#" + e.name + `-` + uniqueId).prop("checked", client[e.name]);
                } else if (e.type === "date") {
                    document.getElementById(e.name + `-` + uniqueId).valueAsDate = new Date(client[e.name]);
                }
                else {
                    $("#" + e.name + `-` + uniqueId).val(client[e.name]);
                }

            });

            formSubmitHandler = function () {
                saveClient(client, dialogType === "Add");
            };

            $("#detailsDialog" + `-` + uniqueId).dialog("option", "title", title)
                .dialog("open");
        };

        function saveClient(client, isNew) {
            var dat = {};
            forEachFieldReadable(function (e) {
                if (e.type === "select" || e.type === "number") {
                    dat[e.name] = parseInt($("#" + e.name + `-` + uniqueId).val(), 10)
                } else if (e.type === "checkbox") {
                    dat[e.name] = $("#" + e.name + `-` + uniqueId).is(":checked")
                } else {
                    dat[e.name] = $("#" + e.name + `-` + uniqueId).val()
                }

            });

            $.extend(client, dat);

            $(jsGridRef).jsGrid(isNew ? "insertItem" : "updateItem", client);

            $("#detailsDialog" + `-` + uniqueId).dialog("close");
        };

    });

};