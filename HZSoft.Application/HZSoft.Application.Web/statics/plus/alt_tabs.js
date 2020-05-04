/**
* @fileOverview jQuery插件，Tabs切换。
*
* @example
*   $('.alt-tabs').alt_tabs({action:'mouseover'});
*/
$.fn.extend({
    alt_tabs: function (options) {
        var o = {
            action: 'click',
            curClass: 'cur',
            initIndex: -1, /*选中索引*/
            onchange: function () { }
        };
        $.extend(o, options);
        return this.each(function () {
            var $this = $(this);
            $this.find('.alt-tabs-head').each(function (i) {
                $(this).data('i', i).unbind(o.action);
                $(this).data('i', i).bind(o.action, function () {
                    if ($this.find('.alt-tabs-content:eq(' + $(this).data('i') + ')').size() > 0) {
                        $(this).addClass(o.curClass).siblings('.' + o.curClass).removeClass(o.curClass);
                        $this.find('.alt-tabs-content').hide().eq($(this).data('i')).show();
                        if (typeof o.onchange == 'function') {
                            o.onchange.apply(this);
                        }
                    }
                    //                    return false;
                });
            }).css('cursor', 'pointer')
            var curItem = $this.find('.alt-tabs-head').filter('.' + o.curClass);
            if (curItem.size() == 0) {
                curItem = $this.find('.alt-tabs-head:first');
            }
            if (o.initIndex != -1) {
                curItem = $this.find('.alt-tabs-head:eq(' + o.initIndex + ')');
            }
            curItem.trigger(o.action);
        });
    }
})