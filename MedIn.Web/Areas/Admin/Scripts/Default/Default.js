

function Textarea2CK(textarea) {
    //FCKeditor.BasePath = '/admin/fckeditor/' ;

    var editor = CKEDITOR.replace(textarea,
    {
        //customConfig: '/areas/admin/scripts/myckconfig.js'
        //filebrowserBrowseUrl: '/admin/ckeditor/ckfinder/ckfinder.html',
        //filebrowserImageBrowseUrl: '/admin/ckeditor/ckfinder/ckfinder.html?Type=Images',
        //filebrowserFlashBrowseUrl: 'a/dmin/ckeditor/ckfinder/ckfinder.html?Type=Flash',
        //filebrowserUploadUrl: '/admin/ckeditor/ckfinder/core/connector/php/connector.php?command=QuickUpload&type=Files',
        //filebrowserImageUploadUrl: '/admin/ckeditor/ckfinder/core/connector/php/connector.php?command=QuickUpload&type=Images',
        //filebrowserFlashUploadUrl: '/admin/ckeditor/ckfinder/core/connector/php/connector.php?command=QuickUpload&type=Flash'

    	filebrowserBrowseUrl: '/Areas/admin/ckeditor/plugins/filemanager/index.html'
    });
    editor.config.height = textarea.rows * 20;
}

function split(val) {
    return val.split(/,\s*/);
}

function extractLast(term) {
    return split(term).pop();
}

$(function () {
	$("td[target]").css("cursor", "pointer").click(function () {
		var target = $(this).attr("target");
		document.location = target;
	});
	$("#edit-form-tabs").tabs();


	$(".wysiwyg").each(function () {
	    Textarea2CK(this);
	});

	$(".autocomplete").each(function () {
	    makeAutocomplete(this);
	});

	$(".ozi-currency").autoNumeric('init');

});

var makeAutocomplete = function(elem) {
    var $this = $(elem);
    var d;
    try {
        d = $("body").data($this.data("sourcename"));
        var m = $this.attr("multiple");
        if (m) {
            $this.bind("keydown", function (event) {
                if (event.keyCode === $.ui.keyCode.TAB && $this.data("ui-autocomplete").menu.active) {
                    event.preventDefault();
                }
            });
        }
        $this.autocomplete({
            minlength: 0,
            source: function (request, response) {
                response($.ui.autocomplete.filter(
                  d, extractLast(request.term)));
            },
            focus: function (event, ui) {
                if (!m) {
                    $this.val(ui.item.label);
                }
                return false;
            },
            select: function (event, ui) {
                var id = $this.data("hiddenid");
                if (m) {
                    var terms = split(this.value);
                    terms.pop();
                    terms.push(ui.item.label);
                    terms.push("");
                    this.value = terms.join(", ");
                    return false;
                } else {
                    if (ui.item) {
                        $("#" + id).val(ui.item.value);
                        $this.val(ui.item.label);
                    } else {
                        $("#" + id).val(null);
                    }
                }
                return false;
            }
        }).data("ui-autocomplete")._renderItem = function (ul, item) {
            return $("<li>")
              .append("<a>" + item.label + "</a>")
              .appendTo(ul);
        };
    } catch (e) {
    }
};


var makeFileUpload = function (elem) {
    setTimeout(function () {
        //console.log("вызов в дефолте");
        //alert($("#collection_Floors_2__File").data("url"));
        var $this = $(elem);
        var objId = $this.data("modelid");
        var propName = $this.data("path");
        var parentId = $this.data("parentid");
        $this.fineUploader({
            request: {
                endpoint: $this.data("url")
            },
            multiple: false
        }).on("submit", function (event, id, filename) {
            $(this).fineUploader("setParams", {
                'id': objId,
                'propName': propName,
                'parentId': parentId
            });
        }).on("complete", function (event, id, filename, responseJSON) {
            if (responseJSON.success) {
                $(this).parent().children("img").attr("src", responseJSON.Url);
                toastr.success("Данные успешно сохранены");
            } else {
                toastr.error("Произошла ошибка: " + responseJSON.error);
            }
        });
    }, 1000);
};

//function MustSave() {
//    YUI().use('node', 'node-event-simulate', function (Y) {
//        if (confirm('Для продолжения необходимо сохраниться. Продолжить?')) Y.one('#_apply').simulate('click');
//    });
//}
/*
function MustSave() {
    if (confirm('Для продолжения необходимо сохраниться. Продолжить?')) $('#_apply').trigger('click');
}
function TryMustSave() {
    if (confirm('Для продолжения необходимо сохраниться. Продолжить?')) return true;
}*/