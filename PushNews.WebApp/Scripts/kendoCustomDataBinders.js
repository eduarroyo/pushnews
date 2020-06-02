kendo.data.binders.widget.selectedItem = kendo.data.Binder.extend({
    init: function (widget, bindings, options) {
        var that = this;
        //call the base constructor
        kendo.data.Binder.fn.init.call(this, widget.element[0], bindings, options);

        //listen for the change event of the widget
        $(that.element).getKendoListView().bind("change", function (e) {
            that.change(e); //call the change function
        });
    },
    refresh: function () {
        var that = this,
            value = that.bindings["selectedItem"].get(), //get the value from the View-Model
            list = $(that.element).getKendoListView(),
            item;

        if (value) {
            item = list.items().filter("div[data-uid='" + value.uid + "']");
        }

        if (item && item.length) { //update the widget
            list.select(item);
        } else {
            list.clearSelection();
        }

    },
    change: function (e) {
        var list = $(this.element).getKendoListView(),
            selectedItem = list.select(),
            item;

        item = list.dataSource.getByUid(selectedItem.data("uid"));
        this.bindings["selectedItem"].set(item); //update the ViewModel
    }
});