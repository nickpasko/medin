var calculator = {
    optg: function () {
        if (!checked("optg"))
            return 0;
        var result = 2000; // цена по умолчанию
        if (checked("less14"))
            result = 1500; // цена для детей
        return result;
    },
    TRG_side: function() {
        if (!checked("TRG_side"))
            return 0;
        var result = 2000;
        if (checked("less14"))
            result = 1500; // цена для детей
        if (checked("CountTRG_Ricketts") ||
            checked("CountTRG_McNamara") ||
            checked("CountTRG_Jarabak")) {
            result += 2000;
        }
        return result;
    },
    TRG_front: function() {
        if (!checked("TRG_front"))
            return 0;
        var result = 2000;
        if (checked("less14"))
            result = 1500; // цена для детей
        if ((!checked("TRG_side")) && (checked("CountTRG_Ricketts") ||
            checked("CountTRG_McNamara") ||
            checked("CountTRG_Jarabak"))) {
                result += 1000;
        }
        return result;
    },
    CountTRG: function () {
        var result = 0;
        return result;
    },
    
    endoAndImpl: function () {
        if (!checked("endodontia") && !checked("impl") && !checked("augmentation"))
            return 0;
        var result;

        var lab = $("#labname option:selected").val();
        var hires = !(lab == 'marks' || lab == 'mayak' || lab == 'suhar' || (lab == '' && !checked('endodontia_hires')));

        if (hires) {
            result = checked("endodontia") ? (checked("less14") ? 4000 : 5000) : 4000;
        } else if (checked("rentgen")) {
            result = 4000;
        } else {
            result = 3500;
        }

        if (hires) {
            for (var i = 1; i <= 4; i++) {
                for (var j = 1; j <= 8; j++) {
                    if (checked("endodontia_" + i + "_" + j)) {
                        result += 800;
                    } else if (checked("impl_" + i + "_" + j) || checked("retencia_" + i + "_" + j)) {
                        result += 500;
                    }
                }
            }
        } else {
            var formula = function (topCount, bottomCount) { // сумма, которая прибавляется дополнительно, за каждый один или несколько зубов
                var sumCount = topCount + bottomCount;
                var sum = 0;
                if (!!sumCount) {
                    if (topCount == 0 || bottomCount == 0) {
                        if (sumCount < 3) sum = 0;
                        else if (sumCount < 5) sum = 1000;
                        else if (sumCount < 16) sum = 2000;
                        else sum = 3000;
                    } else {
                        if (sumCount < 5) sum = 2000;
                        else if (sumCount < 8) sum = 3500;
                        else if (sumCount < 32) sum = 5500;
                        else sum = 7000;
                    }
                }
                return sum;
            };

            var intersectTopCount, intersectBottomCount;
            intersectTopCount = intersectBottomCount = 0;

            for (var i = 1; i <= 4; i++) {
                var c = 0;
                for (var j = 1; j <= 8; j++) {
                    var checkedEndo = checked("endodontia_" + i + "_" + j);
                    var checkedImpl = checked("impl_" + i + "_" + j);
                    var checkedRetencia = checked("retencia_" + i + "_" + j);
                    if (checkedEndo || checkedImpl || checkedRetencia) {
                        c ++;
                    } 
                }
                if (i < 3) {
                    intersectTopCount += c;
                } else {
                    intersectBottomCount += c;
                }
            }
            
            var intersectSum = formula(intersectTopCount, intersectBottomCount);

            result += intersectSum;
        }

        if (checked("endo_repeated")) {
            if ($("#endoWithoutProcessing").is(":checked"))
                result = result * 0.1; // скидка 10%
            else
                result = result * 0.5; // скидка 50%
        } else {
            if (checked("impl_repeated")) {
                if ($("#implWithoutProcessing").is(":checked"))
                    result = result * 0.1; // скидка 10%
                else
                    result = result * 0.5; // скидка 50%
            }
        }

        return result;
    },

        /*
    endodontia: function() {
        if (!checked("endodontia"))
            return 0;
        var lab = $("#labname option:selected").val();
        var hires = !(lab == 'marks' || lab == 'mayak' || lab == 'suhar' || (lab == '' && !checked('endodontia_hires')));

        var result = hires ? 5000 : 3500;

        if ($("#endoPrintOnTape").is(":checked")) {
            if (hires) {
                result += $("#endo_teeth_panel input[type=checkbox][name]:checked").length * 800;
            } else {
                var topCount = $("#endodontia_top input[type=checkbox][name]:checked").length;
                var bottomCount = $("#endodontia_bottom input[type=checkbox][name]:checked").length;
                var sumCount = topCount + bottomCount;
                if (!!sumCount) {
                    if (topCount == 0 || bottomCount == 0) {
                        if (sumCount < 3) result = 3500;
                        else if (sumCount < 5) result = 4500;
                        else if (sumCount < 16) result = 5500;
                        else result = 6500;
                    } else {
                        if (sumCount < 5) result = 5500;
                        else if (sumCount < 8) result = 7000;
                        else if (sumCount < 32) result = 9000;
                        else result = 10500;
                    }
                }
            }
        }
        if (checked("endo_repeated")) {
            result = result * 0.5; // скидка 50%
        }
        return result;
    },
    impl: function () { // 3500, для высокого 4000, +500 за каждый зуб, после 9го +250
        if (!checked("impl") && !checked("augmentation"))
            return 0;

        var lab = $("#labname option:selected").val();
        var hires = !(lab == 'marks' || lab == 'mayak' || lab == 'suhar' || (lab == '' && !checked('impl_hires')));

        var result = (hires ? 5000 : 4000);

        if ($("#implPrintOnTape").is(":checked")) {
            var count = $("#impl_teeth_panel input[type=checkbox][name]:checked").length;
            if (count < 9) {
                result += count * 500;
            } else {
                result += 8 * 500;
                result += (count - 8) * 250;
            }
        }
        if (checked("impl_repeated")) {
            if ($("#implWithoutProcessing").is(":checked"))
                result = result * 0.1; // скидка 10%
            else
                result = result * 0.5; // скидка 50%
        }
        return result;
    },*/
    /*retencia: function () {  // пока что считаем вместе с эндодонтией
        // область интереса не влияет на стоимость, стоит 4000, для детей 3000, дистопия не суммирует
        if (!checked("retencia") && !checked("distopia"))
            return 0;
        // не суммируется с имплантацией или эндодонтией
        if (checked("less14"))
            return 3000;
        return 4000;
    },*/
    pazuhi: function () { // (полностью) нет на сухаревской и на маяковской, цена 3500, для детей 2000, не суммируют
        if ((checked("endodontia") || checked("impl") || checked("augmentation")) && !checked("pazuhi_whole"))
            return 0;
        if (!checked("pazuhi_up") && !checked("pazuhi_whole"))
            return 0;
        if (checked("less14"))
            return 2000;
        return 3500;
    },
    VNCHS: function () { // сплинт (только на маркс, кроп, октяб)
        // по одному пункту стоит 2250, для детей 1500
        // если 2 или 3, то 4500, для детей 3000
        if (!checked("VNCHSopen") && !checked("VNCHSclosed") && !checked("VNCHSSplint"))
            return 0;
        var count = $("#vnchs_wrap input[type=checkbox]:checked:visible").length;
        var isChild = checked("less14");
        var result = isChild ? 3000 : 4500;
        if (count == 1) {
            result = result / 2;
        }
        return result;
    },
    visok: function () { // либо кроп, либо октябр
        // стоит 5000, если заключение врача, печать на плёнке
        // стоит 3500 если без заключения
        if (!checked("visok_right") && !checked("visok_left"))
            return 0;
        var result = 3500;
        if (checked("doctor_conclusion")) {
            result = 5000;
        }
        return result;
    },
    scull: function () {
        var result = 0;
        if (checked("scull-23x26"))
            result = 3500;
        else if (checked("scull-17x22"))
            result = 4000;
        return result;
    },
    _3dPhoto_3Dphoto: function () {
        if (checked("_3dPhoto_3Dphoto"))
            return 2000;
        return 0;
    },
    SendToEmail: function() {
        return 0;
    },
    sharefiles: function() {
        if (checked("sharefiles"))
            return 500;
        return 0;
    }
};

$(function () {
    recalculate();
    // отслеживаем изменения на форме для инпутов
    $("#refferal").on("change", "checkbox, input, textarea, select", function () {
        recalculate();
    });

    // прячем маркеры и исследования уникальные для лаборатрий
    $("select[name='labname']").on("change", function() {
        var val = $(this).val();
        if (!!val) {
            $(".red, .mark").hide();
        } else {
            $(".red, .mark").show();
        }
        
        switch (val) {
            case "marks": // низкое
                showElem("#scull-wrap-23x26");
                show("3dPhoto_3Dphoto");
                show("VNCHSsplint");
                showElem("#pazuhi_whole_wrap");
                showElem("#splint_wrap");
                hideElem("#visok_wrap");
                hideElem("#scull-wrap-17x22");
                showElem("#threeD-wrap");
                
                uncheck("#Endodontia_hires");
                disable("#Endodontia_hires");
                //uncheck("#endoWithoutProcessing");
                check("#endoPrintOnTape");
                //disable("#endoWithoutProcessing");

                uncheck("#impl_hires");
                disable("#impl_hires");
                //uncheck("#implWithoutProcessing");
                check("#implPrintOnTape");
                //disable("#implWithoutProcessing");

                break;
            case "mayak": // низкое
                hide("3dPhoto_3Dphoto");
                hide("VNCHSsplint", true);
                hideElem("#scull-wrap-23x26");
                hideElem("#scull-wrap-17x22");
                hideElem("#pazuhi_whole_wrap");
                hideElem("#splint_wrap");
                hideElem("#visok_wrap");
                hideElem("#threeD-wrap");
                
                uncheck("#Endodontia_hires");
                disable("#Endodontia_hires");
                //uncheck("#endoWithoutProcessing");
                check("#endoPrintOnTape");
                //disable("#endoWithoutProcessing");

                uncheck("#impl_hires");
                disable("#impl_hires");
                //uncheck("#implWithoutProcessing");
                check("#implPrintOnTape");
                //disable("#implWithoutProcessing");

                break;
            case "suhar": // низкое
                hide("3dPhoto_3Dphoto");
                hide("VNCHSsplint", true);
                hideElem("#scull-wrap-23x26");
                hideElem("#scull-wrap-17x22");
                hideElem("#pazuhi_whole_wrap");
                hideElem("#splint_wrap");
                hideElem("#visok_wrap");
                hideElem("#threeD-wrap");

                uncheck("#Endodontia_hires");
                disable("#Endodontia_hires");
                //uncheck("#endoWithoutProcessing");
                check("#endoPrintOnTape");
                //disable("#endoWithoutProcessing");

                uncheck("#impl_hires");
                disable("#impl_hires");
                //uncheck("#implWithoutProcessing");
                check("#implPrintOnTape");
                //disable("#implWithoutProcessing");

                break;
            case "kropt": // высокое
                hide("3dPhoto_3Dphoto");
                hide("VNCHSsplint", true);
                hideElem("#scull-wrap-23x26");
                showElem("#scull-wrap-17x22");
                showElem("#pazuhi_whole_wrap");
                showElem("#splint_wrap");
                showElem("#visok_wrap");
                hideElem("#threeD-wrap");
                
                check("#Endodontia_hires");
                disable("#Endodontia_hires");
                //enable("#endoWithoutProcessing");

                check("#impl_hires");
                enable("#impl_hires");
                //enable("#implWithoutProcessing");

                break;
            case "octob": // высокое
                hide("3dPhoto_3Dphoto");
                hide("VNCHSsplint", true);
                hideElem("#scull-wrap-23x26");
                showElem("#scull-wrap-17x22");
                showElem("#pazuhi_whole_wrap");
                showElem("#splint_wrap");
                showElem("#visok_wrap");
                hideElem("#threeD-wrap");

                check("#Endodontia_hires");
                disable("#Endodontia_hires");
                //enable("#endoWithoutProcessing");

                check("#impl_hires");
                enable("#impl_hires");
                //enable("#implWithoutProcessing");

                break;
            default:
                show("3dPhoto_3Dphoto");
                show("VNCHSsplint");
                showElem("#scull-wrap");
                showElem("#pazuhi_whole_wrap");
                showElem("#splint_wrap");
                showElem("#scull-wrap-23x26");
                showElem("#scull-wrap-17x22");
                showElem("#threeD-wrap");

                enable("#Endodontia_hires");
                //enable("#endoWithoutProcessing");

                enable("#impl_hires");
                //enable("#implWithoutProcessing");

                break;
        }

        
    });

    $("input:not([data-role='other'])").on('change', function () {
        if ($("input:not([data-role='other']):checked:visible").length || $("input:not([data-role='other']):visible").val()) {
            showElem("#otherConditions");
        } else {
            hideElem("#otherConditions");
        }
    });

    // отмечаем чекбоксы, связанные с инпутами
    $('[data-ref-checkbox]').on('input', function () {
        $("#" + $(this).data("ref-checkbox")).prop("checked", !!$(this).val());
    });
    // очистка формы
    $(".clear-form").on("click", function () {
        $("#refferal input").val("");
        $("#refferal input").prop("checked", false);
    });
    // отмечаем группу чекбоксов
    $(".change-group").on("click", function () {
        var id = $(this).data("td");
        var val = $(this).is(":checked");
        var group = $("#" + id + " input");
        group.prop("checked", val);
        $(this).change();
    });

    // дополнительные настройки исследований
    $("[data-toggle-panel]").on('change', function() {
        var $this = $(this);
        var targets = $this.data("toggle-panel").split(",");

        $.each(targets, function(index, target) {
            var items = $("[data-toggle-panel*='" + target + "']");
            if (items.is(":checked")) {
                $(target).show("fast");
            } else {
                $(target).hide("fast");
            }
        });
    });

    $("[data-show-panel]").on('change', function () {
        var panel = $(this).data("show-panel");
        $(panel).show('fast');
    });
    $("[data-hide-panel]").on('change', function () {
        var panel = $(this).data("hide-panel");
        $(panel).hide('fast');
    });
});

var hide = function (name, elementOnly) {
    if (!!elementOnly) {
        $("[name='" + name + "']").hide();
    } else {
        $("[name='" + name + "']").parents(".d-item").hide();
    }
};

var show = function (name) {
    $("[name='" + name + "']").show();
    $("[name='" + name + "']").parents(".d-item").show();
};

var hideElem = function (selector) {
    $(selector).hide();
};

var showElem = function (selector) {
    $(selector).show();
};

var check = function (selector) {
    $(selector).prop("checked", true).change();
};

var uncheck = function(selector) {
    $(selector).prop("checked", false).change();
};

var disable = function (selector) {
    $(selector).prop("disabled", true);
};

var enable = function (selector) {
    $(selector).prop("disabled", false);
};

var checked = function (name) {
    var elem = $("[name='" + name + "']");
    return elem.is(":checked") && elem.is(":visible");
};

Math.sum = function (arr) {
    var r = 0;
    $.each(arr, function () { r += this; });
    return Math.round(r * 100) / 100;
};

var recalculate = function () {
    var result = 0;
    for (var item in calculator) {
        var c = calculator[item];
        var cost = 0;
        if (!!c) {
            try {
                cost = c();
            } catch(e) {
                cost = 0;
            } 
        }
        if (!!cost) {
            result += cost;
        }
    }
    $("#cost").text(result);
};