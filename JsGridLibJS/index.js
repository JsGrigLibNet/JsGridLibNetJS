var gridAppBuilder = function (jsGridRef, gridApp) {
    var uuid = function () {
        return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
            (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
        );
    };

    var uniqueId = uuid();

    $.get(gridApp.GetSchemaAndSettings).done(function (composite) {
        console.log(composite);
        var data = composite.data;

        var fields = composite.fields;
        var forEachField = function (f) {
            for (let index = 0; index < fields.length; index++) {
                var element = fields[index];
                if (element.type !== "control") {
                    f(element);
                }

            }
        }
        var jsGridSetup = {
            height: composite.settings.height,
            width: composite.settings.width,
            editing: composite.settings.editing,
            autoload: composite.settings.autoload,
            paging: composite.settings.paging,
        };


        jsGridSetup.fields = fields;
        jsGridSetup.fields.push({
            type: "control",
            modeSwitchButton: false,
            editButton: true,
            headerTemplate: function () {
                return $("<button>").attr("type", "button").text("Add")
                    .on("click", function () {
                        showDetailsDialog("Add", {});
                    });
            }
        });
        jsGridSetup.pagerContainer = ".pagerContainer";
        jsGridSetup.controller = {
            loadData: function (filter) {
                var deferred = jQuery.Deferred();
                var res = $.ajax({
                    type: "GET",
                    url: gridApp.GetAll,
                    data: filter
                }).done(function (data) {
                    deferred.resolve(data.results);
                });
                return deferred.promise();
            },
            insertItem: function (item) {
                return $.ajax({
                    type: "POST",
                    url: gridApp.Put,
                    data: item
                });
            },

            updateItem: function (item) {
                return $.ajax({
                    type: "POST",
                    url: gridApp.Post,
                    data: item
                });
            },

            deleteItem: function (item) {
                return $.ajax({
                    type: "POST",
                    url: gridApp.Delete,
                    data: item
                });
            }
        },
            jsGridSetup.rowClass = function (item, itemIndex) { };
        jsGridSetup.rowClick = function (args) {
            showDetailsDialog("Edit", args.item);
        };
        jsGridSetup.deleteConfirm = function (item) {
            return "The client \"" + item.name + "\" will be removed. Are you sure?";
        };
        jsGridSetup.rowDoubleClick = function (args) { };
        jsGridSetup.invalidNotify = function (args) {
            var messages = $.map(args.errors, function (error) {
                return error.field + ": " + error.message;
            });

            console.log(messages);
        };
        jsGridSetup.loadIndicator = {
            show: function () {
                console.log("loading started");
            },
            hide: function () {
                console.log("loading finished");
            }
        };
        $(jsGridRef).jsGrid(jsGridSetup);


        var getDialogUI = function () {


            var dat = "";
            forEachField(function (e) {
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
                                              <button  id='save` + `-` + uniqueId + `'>Save</button>
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

        function showDetailsDialog(dialogType, client) {


            forEachField(function (e) {
                if (e.type === "checkbox") {
                    $("#" + e.name + `-` + uniqueId).prop("checked", client[e.name]);
                } else {
                    $("#" + e.name + `-` + uniqueId).val(client[e.name])
                }

            });

            formSubmitHandler = function () {
                saveClient(client, dialogType === "Add");
            };

            $("#detailsDialog" + `-` + uniqueId).dialog("option", "title", dialogType + " Client")
                .dialog("open");
        };

        function saveClient(client, isNew) {
            var dat = {};
            forEachField(function (e) {
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