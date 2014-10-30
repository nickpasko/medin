var files = [];

var FileItem = function(item) {
    var self = this;
    self.Id = ko.observable(item.Id);
    self.Url = ko.observable(item.Url);
    self.SourceName = ko.observable(item.SourceName);
    self.Alt = ko.observable(item.Alt);
    self.Title = ko.observable(item.Title);
    self.Description = ko.observable(item.Description);
    self.Visibility = ko.observable(item.Visibility);
};

var saveOrder = function (name, url, objId, propName, reference) {
    var list = files[name].files;
    var data = ko.toJSON({ items: list });
    $.post(url, { model: data, objId: objId, propName: propName, reference: reference }, function (result) {
        if (result.success) {
            toastr.success("Данные сохранены");
        } else {
            toastr.error(result.error);
        }
    });
};

$(function () {
    $(".upload-panel").each(function () {
        var self = this;
        var propName = $(this).data("name");
        var objId = $(this).data("id");
        var list = files[propName + "_array"].files;
        var singleFile = $(this).data("single");
        $(this).fineUploader({
            request: {
                endpoint: $(this).data("url")
            },
            multiple: !singleFile
        }).on("submit", function (event, id, filename) {
            $(this).fineUploader("setParams", {
                'id': objId,
                'propName': propName
            });
        }).on("complete", function (event, id, filename, responseJSON) {
            if (responseJSON.success) {
                if (singleFile) {
                    list.removeAll();
                }
                list.push(new FileItem(responseJSON));
                toastr.success("Данные успешно сохранены");
            } else {
                toastr.error("Произошла ошибка: " + responseJSON.error);
            }
        });
        $("#" + propName + "-files-container").on("change", "input, textarea", function () {
            var item = ko.dataFor(this);
            var itemData = ko.toJS(item);
            itemData.PropName = propName;
            itemData.ObjId = objId;
            var url = $(self).data("save-url");
            $.post(url, itemData, function (data) {
                if (data.success) {
                    toastr.success("Данные сохранены");
                } else {
                    toastr.error(data.error);
                }
            });
        });
        $("#" + propName + "-files-container").on("click", ".delete", function (e) {
            e.preventDefault();
            var item = ko.dataFor(this);
            var itemData = ko.toJS(item);
            itemData.PropName = propName;
            itemData.ObjId = objId;
            var url = $(self).data("delete-url");
            $.post(url, itemData, function (data) {
                if (data.success) {
                    toastr.success("Данные сохранены");
                    list.remove(item);
                } else {
                    toastr.error(data.error);
                }
            });
        });
    });
});