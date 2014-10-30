$(function () {
	$("#sort-button").click
    (
        function () {
        	$("#list-form").attr("action", _sorlListActionUrl);
        	$("#list-form").submit();
        }
    );

	var submitValues = function (element) {
	    var url = location.href;
	    var parts = url.split("?");
	    var res = {};
	    if (parts.length == 2) {
	        var query = parts[1];
	        var qq = query.split("&");
	        for (var i = 0; i < qq.length; i++) {
	            var pair = qq[i].split("=");
	            res[pair[0]] = decodeURI(pair[1]);
	        }
	    }
	    var $el = $(element);
	    var v = $el.val();
	    var n = $el.attr("name");
	    if (typeof n != "undefined" && n != null) {
	        res[n] = v;
	    }
	    var data = {};
	    $.each($el.data(), function (name, value) {
	        data[name] = value;
	    });
	    for (var prop in data) {
	        var isIt = false;
	        for (var p in res) {
	            if (p.toLowerCase() == prop.toLowerCase()) {
	                res[p] = data[prop];
	                isIt = true;
	                break;
	            }
	        }
	        if (!isIt) {
                res[prop] = data[prop];
	        }
	    }
	    //$.extend(res, data);
	    var form = $("<form />").attr("action", $("#list-form").attr("action")).attr("method", "get");
	    $.each(res, function (name, value) {
	        if (typeof value != "undefined" && value != null && value != '') {
	            var input = $("<input />").attr("name", name).val(value);
	            form.append(input);
	        }
	    });
	    form.submit();
	};

    $("#list-form").on("click", ".get-click", function (e) {
	    e.preventDefault();
        submitValues(this);
    });

	$("#list-form").on("change", ".get-change", function (e) {
	    e.preventDefault();
	    submitValues(this);
	});

	$("#list-form").on("keypress", ".get-blur", function (e) {
	    var code = (e.keyCode ? e.keyCode : e.which);
	    if (code == 13) { 
	        submitValues(this);
	    }
	});

	$("#list-form").on("keyup", ".get-blur", function (e) {
	    var code = (e.keyCode ? e.keyCode : e.which);
	    if (code == 27) {
	        $("[data-target-show=" + $(this).attr("id") + "]").show();
	        $(this).hide();
	    }
	});

	$("[data-target-show]").click(function () {
	    var $this = $(this);
	    var val = $this.attr("data-target-show");
	    $("#" + val).show().focus();
	    if ($this.data("hide")) {
	        $this.hide();
	    }
	});
});
/*
var submitFilter = function () {
	var url = $("#list-form").attr("action") + "?";
	$(".get").each(function () {
		url += $(this).attr("name") + "=" + $(this).val() + "&";
	});
	window.location.href = url;
}*/