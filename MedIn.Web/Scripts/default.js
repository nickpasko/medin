$(function () {

    if ($(".lightbox").size() != 0) {

        $('.lightbox').magnificPopup({
            type: 'image',
            mainClass: 'mfp-with-zoom', 
            zoom: {
                enabled: true,
                duration: 300,
                easing: 'ease-in-out',
                opener: function (openerElement) {
                    return openerElement.is('img') ? openerElement : openerElement.find('img');
                }
            },
            gallery: {
                enabled: true,
                preload: [0, 2],
                navigateByImgClick: true,
                arrowMarkup: '<button title="%title%" type="button" class="mfp-arrow mfp-arrow-%dir%"></button>', // markup of an arrow button
                tPrev: 'Предыдущее изображение',
                tNext: 'Следующее изображение',
                tCounter: '<span class="mfp-counter">%curr% из %total%</span>'
            }
        });

    }

    /*ajax form*/
    $(document).on("submit", ".ajax-form", function (e) {
        e.preventDefault();
        var $this = $(this);
        var btn = $this.find("input[type='submit']") ? $this.find("input[type='submit']") : $this.find("input.button");
        var loader = $("<div id='ajax-loader-db'><img src='/content/i/loading.gif' style='vertical-align:middle;'> sending...</div>");
        var url = $this.attr("action");
        var data = $this.serialize();
        var container = $this.data("container");
        $this.find(".success").remove();
        btn.hide();
        btn.parent().append(loader);
        $.post(url, data, function (resultData) {

            if (!!container) {
                $("#" + container).replaceWith(resultData);
            } else {
                $this.replaceWith(resultData);
            }
            loader.remove();
            btn.show();
        }, "html");
    });
    /*ajax form*/

    setTimeout(moveXblock, 2000);
    var normal = true;
    $("#x-ray-c").click(function () {
        if (normal) {
            $("#invert").attr("href", "/Styles/default-x.css");
            $("#x-ray").attr("src", "/content/i/x-button-x.png");
            invertElements(normal);
            normal = false;
            $.cookie('x-ray', 'true', { path: '/' });
        } else {
            $("#invert").attr("href", "");
            $("#x-ray").attr("src", "/content/i/x-button.png");
            invertElements();
            normal = true;
            $.cookie('x-ray', 'false', { path: '/' });
        }
    });

    var x = $.cookie('x-ray');
    if (!!x && x == "true") {
        $("#x-ray-c a").click();
    }

    /* toggle form */

    $("body").on("click", ".toggle-form", function () {
        var id = $(this).data("target-id");
        if (!id) {
            return;
        }
        $("#" + id).slideToggle();
    });

    /* toggle form */

    $(".edit-clinic").hide();
    $(".delete-clinic").on("click", function () {
        return confirm("Вы действительно хотите удалить клинику?");
    });
    $(".edit-clinic-action").on("click", function (e) {
        e.preventDefault();
        var form = $(this).parents("form");
        $(".display-clinic", form).hide();
        $(".edit-clinic", form).show();
    });
    $(".cancel-edit").on("click", function (e) {
        e.preventDefault();
        var form = $(this).parents("form");
        $(".display-clinic", form).show();
        $(".edit-clinic", form).hide();
    });
    $(".edit-form").on("submit", function (e) {
        e.preventDefault();
        var form = this;
        var $form = $(form);
        if ($form.valid()) {
            $(".display-clinic", form).show();
            $(".edit-clinic", form).hide();
            var data = $form.serialize();
            var loader = $("<div id='ajax-loader-db'><img src='/content/i/skull-blue.gif' style='vertical-align:middle;'> sending...</div>");
            $form.append(loader);
            $.post($(form).attr("action"), data, function (res) {
                $(".name", form).text(res.ClinicName);
                $(".address", form).text(res.ClinicAddress);
                $(".email", form).text(res.ClinicEmail);
                loader.remove();
            });
        }
    });
    $(document).ajaxError(function () {
        toastr.error("Произошла непредвиденная ошибка.");
    });

});

function setChecked(elm, val) {
    if (!!val)
        elm.checked = false;
    else
        elm.checked = true;

}

function moveXblock() {
    $("#x-ray-c").animate({ left: "-=20" }, 500, function () {

        $("#x-ray-c").animate({ left: "-1" }, 500);

    });

}

function invertElements(invert) {

    var key = "normal";

    if (invert) {
        key = "inverted";
    }


    $(".invertable-element").each(function () {
        var $this = $(this);
        var attrs = $this.data(key); // $.parseJSON($this.data(key));

        $.each(attrs, function (name, value) {
            $this.attr(name, value);
        });

    });
}