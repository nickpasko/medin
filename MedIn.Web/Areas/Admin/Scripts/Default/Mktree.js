$(function () {

    var new_tree = new tree();
    new_tree.create('modules-ul', true);

});


var tree = function(){
    
    this.ulID = '';
    this.doLines = false;
    this.levels = [];
}

tree.prototype = {

    create: function (ul_id, doLines) {

        var ul = $('#' + ul_id), obj = this;
        this.ulID = ul_id;
        this.doLines = doLines;
        this.processList(ul, 1);
        var span = ul.find('span.folder');
        span.each(function () {

            var el = $(this);
            el.click(function (e) { obj.toggleListFolder(el, e) });

        })

    },

    processList: function (ul, level) {

        if (!ul.find(' > li').length) { return; }
        // Iterate LIs

        var obj = this,
            list = ul.find(' > li');//get offset li childs

        list.each(function (index) {

            index++;
            var item = $(this),
                item_id = obj.ulID + '_' + index + '_' + level;

            if (index == list.length)//last li in the ul
            {
                obj.levels[level] = false;
                var lastClass = true;
            }
            else {
                obj.levels[level] = true;
                var lastClass = false;
            }



            var subLists = false;

            if (item.find(' > ul') && item.find(' > ul').length) {

                var slevel = level + 1, ulInner = item.find(' > ul');
                obj.processList(ulInner, slevel);
                subLists = true;

            }

            if (subLists) {

                if (!item.attr('class')) {

                    var cn = (GetCookie(item_id) == 1) ? 'liOpen' : 'liClosed';
                    item.attr('class', cn);
                }
            }
            else item.attr('class', 'liBullet');


            var nbsp = '\u00A0'; // &nbsp;

            for (k = 0; k < level; k++) {

                var span = $('<span></span>');

                if (!k) {//это нод

                    if (lastClass) span.attr('class', 'bullet last');
                    else span.attr('class', 'bullet');

                    if (subLists) {

                        span.click(function (e) { obj.toggleListBullet(span, item_id, e) })
                        span.attr('lang', item_id);
                        
                    }
                }
                else if (obj.doLines) {
                    if (obj.levels[level - k])
                        span.attr('class', 'level');
                    else
                        span.attr('class', 'blank');
                }

                var li = item.get(0), domEl = span.get(0);//for normal append child we are need the normal DOM elements
                li.insertBefore(domEl, li.firstChild);
                span.html(nbsp);
            }

        });

    },
    toggleListFolder: function (el, e) {

        e.stopPropagation();
        e.preventDefault();
        var elParent = el.parent(), itemId = elParent.find(' > span.bullet').attr('lang');
        if (elParent.attr('class') == 'liOpen') {
            elParent.attr('class', 'liClosed')
            SetCookie(itemId, 0);
        }
        else {
            elParent.attr('class', 'liOpen')
            SetCookie(itemId, 1);
        }

    },
    toggleListBullet: function (el, itemId, e) {

        e.stopPropagation();
        e.preventDefault();
        var elParent = el.parent();
        if (elParent.attr('class') == 'liOpen') {
            elParent.attr('class', 'liClosed')
            SetCookie(itemId, 0);
        }
        else {
            elParent.attr('class', 'liOpen')
            SetCookie(itemId, 1);
        }
    
    }
}


function SetCookie(name, value, expires, path, domain, secure) {
    document.cookie = name + "=" + escape(value) +
        ((expires) ? "; expires=" + expires : "") +
        ((path) ? "; path=" + path : "") +
        ((domain) ? "; domain=" + domain : "") +
        ((secure) ? "; secure" : "");
}


function GetCookie(name) {
    var cookie = " " + document.cookie;
    var search = " " + name + "=";
    var setStr = null;
    var offset = 0;
    var end = 0;
    if (cookie.length > 0) {
        offset = cookie.indexOf(search);
        if (offset != -1) {
            offset += search.length;
            end = cookie.indexOf(";", offset)
            if (end == -1) {
                end = cookie.length;
            }
            setStr = unescape(cookie.substring(offset, end));
        }
    }
    return (setStr);
}