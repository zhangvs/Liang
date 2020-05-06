var commonCheck = window.commonCheckFill;
var Area = window.allAreaNew;
// 提交订单状态
var subStatus = true;
// 判断地市是否为新疆
var provinceXJ = '';
// 所在地区是否重置
var curCity = '';
// 保存初始化号码
var initNum;
// 获取成功返回后的tendencyId
var tendenceId = '';
var reqData = '';
// 主卡信息
var mainCardInfo;
// 成功弹框文案展示标识
var successFlag = true;
// 是否线上购现场购
var disFlag;
// 省份是否可切换号码归属
var changePflag;
// 是否小程序首页进入的订单
var smallRoutineFlag;
var req = {};
$(function () {
    // 姓名特殊字符转化
    $('#certName').keyup(function(e)  {
        var _this = $(e.currentTarget);
        _this.val($.wordChange(_this.val()));
    });
    $.resize();
    // 初始化参数
    //var initParam = $.getUrlParam();
    var initParam = {};
    initParam.sceneFlag = '03';
    initParam.goodsId = '981610241535';
    initParam.productName = '大王卡';
    initParam.channel = '9999';
    initParam.p = 51;
    initParam.c = 510;
    initParam.u = 'nW/F266vVq2sHDKwYzqI8w==';
    initParam.s = '03';

    // 判断是否有参数t,来自小程序的订单,如果有则提交订单的参数增加这两者
    if (initParam.t) {
        req.t = initParam.t;
        req.openId = initParam.openId;
    }
    // true为小程序首页进入, false为码上购小程序分享
    if (initParam.miniprogram) {
        smallRoutineFlag = true;
    } else {
        smallRoutineFlag = false;
    }
    // 腾讯王卡展示：选号别纠结，以后可以免费换号 提示
    var txGoods = $.isTest() ? ['981609180703', '981801120053', '981702226311'] : ['981802085690', '981702278573', '981610241535'];
    if(txGoods.indexOf(initParam.goodsId) >= 0){
        $('.numTips').show();
    } else {
        $('.search-btn,.search-close-btn').css({
            top:0
        });
    }
    // 判断是否是哔哩哔哩卡
    var isBili = ['981703130259', '981703130260', '981703130261', '981703239394', '981703239395', '981703239396'];
    if(isBili.indexOf(initParam.goodsId) >= 0){
        isBili = true;
    } else {
        isBili = false;
    }
    console.log(isBili);
    // popup弹框展示
    var popupShow = function(text){
        $('.numErrorTips').show().html(text);
        setTimeout(function () {
            $('.numErrorTips').hide();
            $.reScroll();
        }, 3000)
    };
    if (initParam.sceneFlag.substr(0,2) == '02') {
        disFlag = false;
        req.snFlag = '0';
        //$('#submit').html('立即提交');
    } else if (initParam.sceneFlag.substr(0,2) == '03') {
        disFlag = true;
        //$('#submit').html('立即提交，免费送货上门');
    }
    if (!disFlag) {
        $('#postDistrict').hide();
    }
    // 主卡信息
    if(initParam.isMainSubFlag == '1'){
        mainCardInfo = JSON.parse(localStorage.getItem('mainCardInfo'));
        /*    var mainCardInfo = {
         province:'11',
         city:'110',
         mainCardNum: '18201695415',
         mainCardName: 'wzb',
         mainCardId: 'wzb'
         };*/
        $('.mainNum').text('（主卡号码：' + mainCardInfo.mainCardNum + '）');
    }
    // 号码查询参数
    var numberParam = {};
    // 并发请求限制
    var requestFlag = false;
    // 有遮罩层时禁止滚动
    var noScroll = function () {
        $('html, body').addClass('no-scroll');
    };
    var reScroll = function () {
        $('html, body').removeClass('no-scroll');
    };
    // 顶部描述
    var _topText = '根据国家实名制要求, 请准确提供身份证信息';
    var _fillDesc = initParam.productName || localStorage.getItem('product_name');
    //$('#fill-desc').find('span').text(decodeURIComponent(_fillDesc));
    // 设置产品信息
    var setProduct = function () {
        $('#top-desc').show().text(_topText);
    };
    $('.privacy').css('width', ($(window).width() / 0.75) + 'px');
    // 请求参数初始化
    var setReq = function () {
        req.numInfo = {};
        // 如果链接没有省份地市,则初始化的省份地市为北京
        if (initParam.p) {
            req.numInfo.essProvince = initParam.p;
        } else {
            req.numInfo.essProvince = "11";
        }
        if (initParam.c) {
            req.numInfo.essCity = initParam.c;
        } else {
            req.numInfo.essCity = "110";
        }
        req.goodInfo = {};
        req.goodInfo.goodsId = initParam.goodsId;
        req.goodInfo.sceneFlag = initParam.sceneFlag.substr(0,2);
    };
    // 生成号码列表
    var listNumber = function () {
        if (numberParam.list.length == 0) {
            $('.number-list').html('无号码');
            return;
        }
        var _start = (numberParam.current - 1) * numberParam.size;
        var _end = _start + numberParam.size;
        if (numberParam.current == numberParam.max) {
            _end = numberParam.list.length;
        }
        var numberHtml = [];
        for (var i = _start; i < _end; i += 1) {
            var numberObj = numberParam.list[i];
            if (numberObj.niceRule == 0) {
                numberHtml.push("<li><a href='javascript:;' data-niceRule='"+numberObj.niceRule+"' data-monthLimit='"+numberObj.monthLimit + "' data-advanceLimit='" + numberObj.advanceLimit + "' >" + numberObj.number+"</a></li>");
            } else {
                numberHtml.push("<li><a href='javascript:;' data-niceRule='"+numberObj.niceRule+"' data-monthLimit='"+numberObj.monthLimit + "' data-advanceLimit='" + numberObj.advanceLimit  +"' ><i>靓</i>" + numberObj.number+"</a></li>");
            }
        }
        numberParam.current += 1;
        $('.number-list').html(numberHtml);
    };
    // 解析号码
    function decompress(number) {
        $('.number-loading').hide();
        var mlist = ['M2', 'M3', 'M4', 'M5'];
        var _key = $('#search').data('val');
        if (number.numArray.length == 0) {
            if ($.inArray(number.code, mlist) > -1) {
                $('.no-number').html('当前网络不给力，请在wifi或其他网络环境下重试！<span class="error-code">'+number.code+'</span>').show();
            } else if (($.inArray(number.code, mlist) == -1)) {
                if (commonCheck.isEmpty(_key)) {
                    $('.no-number').html('当前选号人数过多，请您稍后再试！<span class="error-code">'+number.code+'</span>').show();
                    $('#refresh').text('再试一次');
                } else {
                    $('.no-number').html('抱歉没有匹配的号码.<span class="error-code">'+number.code+'</span>').show();
                    $('#refresh').text('换一批');
                }
            }
            return;
        }
        $('.number-list').show();
        numberParam.list = [];
        numberParam.current = 1;
        var numArray = number.numArray;
        for (var i = 0; i < numArray.length; i += 12) {
            var numberObj = {};
            numberObj.advanceLimit = numArray[i + 1];
            numberObj.niceRule = numArray[i + 5];
            numberObj.monthLimit = numArray[i + 6];
            if (commonCheck.isEmpty(_key)) {
                numberObj.number = numArray[i];
                numberParam.list.push(numberObj);
            } else {
                var len = 11 - _key.length;
                if (numArray[i].toString().substring(len) == _key) {
                    numberObj.number = numArray[i].toString().substring(0, len)+'<span>' + numArray[i].toString().substring(len, 11) + '</span>';
                    numberParam.list.push(numberObj);
                }
            }
        }
        numberParam.max = Math.ceil(numberParam.list.length / numberParam.size);
        $.shuffle(numberParam.list);
        listNumber();
    }

    function decompress2(number) {
        $('.number-loading').hide();
        var _key = $('#search').data('val');
        $('.number-list').show();
        numberParam.list = [];
        numberParam.current = 1;
        var numArray = number;
        for (var i = 0; i < numArray.length; i++) {
            var numberObj = {};
            numberObj.advanceLimit = 0;
            numberObj.niceRule = 1;
            numberObj.monthLimit = 12;
            numberObj.number = numArray[i];
            numberParam.list.push(numberObj);
        }
        numberParam.max = Math.ceil(numberParam.list.length / numberParam.size);
        $.shuffle(numberParam.list);
        listNumber();
    }
    // 初始化号码查询参数
    var setNumberParam = function () {
        numberParam.list = [];
        numberParam.current = 1;
        numberParam.size = 10;
        numberParam.max = 1;
    };
    // 初始化号码
    var setNumber = function (isSearch) {
        $('.number-list, .no-number').hide();
        $('.number-loading').show();
        var param = {
            provinceCode: req.numInfo.essProvince,
            cityCode: req.numInfo.essCity,
            monthFeeLimit: 0,
            goodsId: req.numInfo.essProvince + initParam.goodsId.substr(2),
            searchCategory: 3,
            net: '01',
            amounts: 200,
            codeTypeCode: '',
            searchValue: $('#search').data('val'),
            qryType: '02',
            goodsNet: 4,
            channel: 'msg-xsg'
        };
        if(isSearch){
            param.searchType = '02';
        } else{
            param.searchValue = '';
        }
        if (!commonCheck.isEmpty(param.goodsId)) {

            $.ajax({
                type: 'get',
                url: '/pretty/index/qryPretty',
                data: param,
                dataType: 'json',
                //jsonp: 'callback',
                //jsonpCallback: 'jsonp_queryMoreNums',
                success: function(numberData)  {
                    //alert('fdfadsfads');
                    //alert(numberData[0]);
                    //initNum = numberData;
                    decompress2(numberData);
                }
            });
        } else {
            $('.no-number').text('抱歉没有匹配的号码').show();
            $('#refresh').text('换一批');
            $('.number-list, .number-loading').hide();
        }
    };
    // 点击错误提示消失
    $('.p-content').find('input').click(function(e)  {
        var _this = $(e.currentTarget);
        var par = _this.parents('li');
        var isError = par.hasClass('error');
        if (isError) {
            par.removeClass('error');
            $('#top-desc').removeClass('error').text(_topText);
        }
    });
    $('.p-text-area').click(function () {
        if ($('#delivery-desc').hasClass('error')) {
            $('#delivery-desc').removeClass('error');
            $('#top-desc').removeClass('error').text(_topText);
        }
    });
    $('#delivery').click(function(e)  {
        if ($(e.currentTarget).hasClass('error')) {
            $('#delivery').removeClass('error');
            $('#top-desc').removeClass('error').text(_topText);
        }
    });
    // 提交前校验
    var verify = function () {
        $('li.error').removeClass('error');
        if (!commonCheck.CustCheck.checkReceiverName($('#certName').val())) {
            return false;
        } else if (!commonCheck.CustCheck.checkIdCard($('#certNo').val())) {
            return false;
        } else if (!commonCheck.CustCheck.checkPhone($('#mobilePhone').val())) {
            return false;
        } else if (!commonCheck.CustCheck.checkNumAddress($('#location .p-content').text())) {
            return false;
        } else if (!commonCheck.CustCheck.checkNumber($('#number .p-content').text())) {
            return false;
        } else if (disFlag) {
            if (!commonCheck.CustCheck.checkAddress($('#delivery .p-content').text())) {
                return false;
            } else if (!commonCheck.CustCheck.checkAddressInfo($('#address').val())) {
                return false;
            }
        }
        return true;
    };
    // 从缓存读取数据
    var loadDataFromCache = function () {
        var _cache = sessionStorage.getItem('ANT_CARD');
        if (!commonCheck.isEmpty(_cache)) {
            var _cacheObject = JSON.parse(_cache);
            if (commonCheck.isEmpty($('#certName').val())) {
                var _certInfo = _cacheObject.certInfo;
                $('#certName').val(_certInfo.certName);
                $('#certNo').val(_certInfo.certId);
                $('#mobilePhone').val(_certInfo.contractPhone);
            }
            var _post = _cacheObject.post;
            var _city = $('#post-city li[data-code='+_post.webCity+']');
            if (_city.length == 1) {
                cityChange(_post.webCity, _post.webCounty);
                $('#delivery .p-content').text('请选择区/县').addClass('grey');
                $('#address').val(_post.address);
                req.postInfo = _post;
            }
        }
    };
    // 现场受理省份地市初始化
    function sceneLocation(initReq) {
        var scenePro = [];
        scenePro = Area.PROVINCE_LIST.filter(function(p){
            return p.ESS_PROVINCE_CODE == initReq.numInfo.essProvince;
        })[0];
        var provinceList = [];
        provinceList.push('<li data-code='+scenePro.ESS_PROVINCE_CODE+' pro-code='+scenePro.PROVINCE_CODE+'>'+scenePro.PROVINCE_NAME+'</li>');
        $('#province').html(provinceList).find('li[data-code=' + initReq.numInfo.essProvince + ']').addClass('selected, org');
        var sceneCity = [];
        sceneCity = Area.PROVINCE_MAP[scenePro.PROVINCE_CODE].filter(function(c){
            return c.ESS_CITY_CODE == initReq.numInfo.essCity;
        })[0];
        var cityList = [];
        cityList.push('<li data-code='+sceneCity.ESS_CITY_CODE+' city-code='+sceneCity.CITY_CODE+'>'+sceneCity.CITY_NAME+'</li>');
        $('#city').html(cityList).find('li[data-code=' + initReq.numInfo.essCity + ']').addClass('selected, org');
    }
    // 邮寄信息初始化
    var setPost = function(initReq, data) {
        var postPro = [];
        if (data == undefined) {
            postPro = Area.PROVINCE_LIST.filter(function(p){return p.ESS_PROVINCE_CODE == initReq.numInfo.essProvince;})[0];
        } else {
            postPro = Area.PROVINCE_LIST.filter(function(p){return p.ESS_PROVINCE_CODE == data;})[0];
        }
        var _cityList = [];
        Area.PROVINCE_MAP[postPro.PROVINCE_CODE].forEach(function(c)  {
            _cityList.push('<li data-province-name='+postPro.PROVINCE_NAME+'  data-code='+c.CITY_CODE+' data-ess-code='+c.ESS_CITY_CODE+'>'+c.CITY_NAME+'</li>');
        });
        $('#post-city').html(_cityList);
        var postCity = Area.PROVINCE_MAP[postPro.PROVINCE_CODE].filter(
            function (c) {
                return c.ESS_CITY_CODE == initReq.numInfo.essCity;
            })[0];
        if (postCity == undefined) {
            postCity = Area.PROVINCE_MAP[postPro.PROVINCE_CODE][0];
        }
        var _districtList = [];
        Area.CITY_MAP[postCity.CITY_CODE].forEach(function(d)  {
            _districtList.push('<li data-code=' + d.DISTRICT_CODE+'>' + d.DISTRICT_NAME+'</li>');
        });
        $('#post-district').html(_districtList);
        req.postInfo = {};
        req.postInfo.webProvince = postPro.PROVINCE_CODE;
        req.postInfo.webCity = postCity.CITY_CODE;
    };
    // 获取默认省份地市
    var getLocation = function(locations) {
        var defaultProvince = $('#province li[data-code=' + req.numInfo.essProvince + ']').text();
        var defaultCity = $('#city li[data-code=' + req.numInfo.essCity + ']').text();
        if(initParam.c == '713'){
            $('#location .p-content').text(defaultProvince +' 天门/仙桃/潜江市').removeClass('grey');
        }else{
            $('#location .p-content').text(defaultProvince + ' ' + defaultCity).removeClass('grey');
        }
        var postProvinceList = [];
        locations.PROVINCE_LIST.forEach(function(d) {
            postProvinceList.push('<li data-code=' + d.ESS_PROVINCE_CODE + '  pro-code='+d.PROVINCE_CODE + '>' + d.PROVINCE_NAME + '</li>');
        });
        $('#post-province').html(postProvinceList).find('li[data-code=' + req.numInfo.essProvince + ']').addClass('selected');
        if (isBili) {
            $('#post-province li').hide();
            $('#post-province .selected').show();
        }
    };
    var city = [];
    var province = [];
    // 省份切换
    function provinceChange(pCode, provinceCode, cityCode) {
        $('#province li, #city li').removeClass('selected org');
        $('#province').find('li[data-code=' + provinceCode + ']').addClass('selected org');
        var cityList = [];
        city[pCode].forEach(function(c) {
            cityList.push('<li data-code='+c.ESS_CITY_CODE+'>'+c.CITY_NAME+'</li>');
        });
        $('#city').html(cityList);
        if (!commonCheck.isEmpty(cityCode)) {
            $('#city').find('li[data-code=' + cityCode + ']').addClass('selected org');
        }
    }
    // 省份地市初始化
    function setLocation(locations, initReq) {
        city = locations.PROVINCE_MAP;
        var provinceList = [];
        locations.PROVINCE_LIST.forEach(function(p) {
            provinceList.push("<li data-code='"+p.ESS_PROVINCE_CODE+ "' pro-code=" + p.PROVINCE_CODE +">" + p.PROVINCE_NAME+"</li>");
        });
        $('#province').html(provinceList).find('li[data-code=' + initReq.numInfo.essProvince + ']').addClass('selected');
        var proCode = $('#province').find('.selected').attr('pro-code');
        provinceChange(proCode, req.numInfo.essProvince, req.numInfo.essCity);
    }
    // 改变邮寄省份
    var provincePostList = function (_coda) {
        var provinceList = [];
        if (province != '') {
            province.PROVINCE_LIST.forEach(function(p) {
                if (p.PROVINCE_CODE != '89' && p.PROVINCE_CODE != '79') {
                    provinceList.push('<li data-code='+p.ESS_PROVINCE_CODE+'>'+p.PROVINCE_NAME+'</li>');
                }
            });
        }
        if (_coda == 89) {
            provinceList.length = 0;
            provinceList.push('<li data-code='+_coda+'>新疆</li>');
        } else if (_coda == 79) {
            provinceList.length = 0;
            provinceList.push('<li data-code='+_coda+'>西藏</li>');
        }
        $('#post-province').html(provinceList).find('li[data-code=' + _coda + ']').addClass('selected');
        if (isBili) {
            $('#post-province li').hide();
            $('#post-province .selected').show();
        }
    };
    // 改变邮寄省份
    function provinceChangePost(provinceCode, cityCode) {
        $('#post-province').find('li[data-code=' + provinceCode + ']').addClass('selected');
        var postPro = Area.PROVINCE_LIST.filter(function (p) {
            return p.ESS_PROVINCE_CODE === '' + provinceCode;
        })[0];
        var cityList = [];
        Area.PROVINCE_MAP[postPro.PROVINCE_CODE].forEach(function (c) {
            cityList.push('<li data-code=' + c.CITY_CODE + ' data-ess-code=' + c.ESS_CITY_CODE + '>' + c.CITY_NAME + '</li>');
        });
        $('#post-city').empty();
        $('#post-city').html(cityList);
        if (!commonCheck.isEmpty(cityCode)) {
            $('#post-city').find('li[data-code=' + cityCode + ']').addClass('selected');
        }
    }
    // 邮寄地市切换
    function cityChange(cityCode, districtCode) {
        $('#post-city li, #post-district li').removeClass('selected');
        $('#post-city').find('li[data-code=' + cityCode + ']').addClass('selected');
        var _districtList = [];
        Area.CITY_MAP[cityCode].forEach(function (d) {
            _districtList.push('<li data-code=' + d.DISTRICT_CODE + '>' + d.DISTRICT_NAME + '</li>');
        });
        $('#post-district').html(_districtList);
        if (!commonCheck.isEmpty(districtCode)) {
            $('#post-district').find('li[data-code=' + districtCode + ']').addClass('selected');
        }
    }
    var setProvince = function(data) {
        province = data;
    };
    // 页面初始化
    function init() {
        setReq();
        setProduct();
        setNumberParam();
        loadDataFromCache();
        setProvince(Area);
        if (disFlag&&changePflag) {
            setLocation(Area, req);
            $('#location .p-content').removeClass('arr');
        } else {
            sceneLocation(req);
        }
        getLocation(Area);
        setPost(req);
    }
    // 小程序填写页初始化
    function smallRoutineInit() {
        setReq();
        setProduct();
        setNumberParam();
        loadDataFromCache();
        setProvince(Area);
        setLocation(Area, req);
        $('#location .p-content').removeClass('arr');
        getLocation(Area);
        setPost(req);
    }
    // 页面初始化请求
    var pageInit = function () {
        changePflag = true;
        init();
    };
    // 根据小程序的标记,判断页面初始化走哪个逻辑
    if (smallRoutineFlag) {
        console.log('xiaochengxu');
        smallRoutineInit();
    } else {
        console.log('mashanggou');
        pageInit();
    }
    // 省份地市弹出层
    if (disFlag || smallRoutineFlag) {
        $('#location').on('click', function(e) {
            if ($(e.currentTarget).hasClass('error')) {
                $('#location').removeClass('error');
                $('#top-desc').removeClass('error').text(_topText);
            }
            $('#province .selected').removeClass('selected org');
            $('#province').find('li[data-code=' + req.numInfo.essProvince + ']').addClass('selected org');
            var proCode = $('#province').find('.selected').attr('pro-code');
            provinceChange(proCode, req.numInfo.essProvince, req.numInfo.essCity);
            if ($('#city li').hasClass('selected')) {
                $('#city .selected').addClass('org');
                $('#province .selected').addClass('org');
            }
            var _mask = $('.mask');
            _mask.show();
            noScroll();
            $('#area').addClass('slip');
            _mask.one('click', function () {
                $('#area').removeClass('slip');
                setTimeout(function () {
                    _mask.hide();
                    reScroll();
                }, 300);
            });
        });
    }
    // 省份切换
    $('#province').on('click', 'li', function(e)  {
        var _this = $(e.currentTarget);
        var _proCode = _this.attr('pro-code');
        var _code = _this.data('code');
        _this.addClass('selected org').siblings('li').removeClass('selected ord');
        // req.numInfo.essProvince = _this.data('code') + '';
        provinceChange(_proCode, _code);
    });
    // 地市切换
    $('#city').on('click', 'li', function(e) {
        var _currentP = $('#province li.selected');
        var _this = $(e.currentTarget);
        _this.addClass('selected, org').siblings('li').removeClass('selected, org');
        if(_this.data('code') == '713'){
            $('#location .p-content').text(_currentP.text()+' 天门/仙桃/潜江市').removeClass('grey');
        } else {
            $('#location .p-content').text(_currentP.text()+' '+_this.text()).removeClass('grey');
        }
        req.numInfo.essProvince = _currentP.data('code') + '';
        req.numInfo.essCity = _this.data('code') + '';
        $('#area').removeClass('slip');
        $('#number .p-content').text('');
        provinceXJ = _currentP.text();
        $('#delivery-desc').show();
        if (curCity != _this.text()) {
            $('#delivery .p-content').text('请选择区/县').addClass('grey');
            curCity = _this.text();
        }
        var _code = $('#province li.selected').data('code');
        provincePostList(_code);
        setPost(req);
        setTimeout(function () {
            $('.mask').unbind('click').hide();
            reScroll();
        }, 300);
        $('.numberTips').hide();
        if ($(e.currentTarget).hasClass('error')) {
            $('#location').removeClass('error');
            $('#top-desc').removeClass('error').text(_topText);
        }
    });

    $('#go-protocol').on('click', function () {
        // 查看协议
        var protocolParam = {
            city: req.numInfo.essCity,
            province: req.numInfo.essProvince,
            custName: $('#certName').val().trim(),
            goodsId: req.goodInfo.goodsId,
            number: req.numInfo.number,
            psptType: '02',
            psptTypeCode: $('#certNo').val().trim(),
            // activityType: '11',
            custAddress: $('#address').val()
        };
        $.showProtocal(protocolParam, '/scene-buy/scene/protocol', verify);
    });
    // 关闭弹出层
    $('.popup-close').on('click', function(e) {
        var _this = $(e.currentTarget);
        var closeType = _this.attr('data-type');
        $('.popup, .mask').hide();
        reScroll();
    });
    // 号码弹出层
    /*$('#number').on('click', function () {
        if (commonCheck.CustCheck.checkNumAddress($('#location .p-content').text())) {
            $('#search').data('val', '').val('');
            $('#search-btn').show();
            $('#search-close-btn').hide();
            setNumber();
            $('#number-popup, .mask').show();
            noScroll();
        }
    });*/
    // 刷新号码
    $('#refresh').on('click', function () {
        if (numberParam.current > numberParam.max) {
            // 重新获取号码
            setNumber();
            return;
        }
        listNumber();
    });
    // 号码搜索
    $('#search-btn').on('click', function () {
        var _key = $('#search').val().trim();
        $('#search').data('val', _key);
        $('#search-btn').hide();
        $('#search-close-btn').show();
        if (!commonCheck.isEmpty(_key)) {
            setNumber(true);
        } else {
            setNumber();
        }
    });
    // 关闭搜索
    $('#search-close-btn').on('click', function () {
        $('#search').data('val', '').val('');
        $('#search-btn').show();
        $('#search-close-btn').hide();
        setNumber();
    });
    // 搜索框监控
    $('#search').on('keyup', function(e) {
        var _this = $(e.currentTarget);
        var _preKey = _this.data('val');
        if ('' + _preKey != _this.val().trim()) {
            $('#search-btn').show();
            $('#search-close-btn').hide();
        }
        if (_this.val().trim() == '') {
            _this.data('val', '');
            setNumber();
        }
    });
    // 号码预占
    var occupyNumber = function(number, rule, month)  {
        $('.mask, #number-popup, .occupyTips').hide();
        reScroll();
        requestFlag = false;
        $('#number .p-content').text(number);
        req.numInfo.number = number;
        if (rule == '1' && month != '0') {
            $('.numberTips').show().find('i').text(month);
        } else {
            $('.numberTips').hide();
        }
        if ($('#top-desc').text() == '请选择号码') {
            $('#top-desc').removeClass('error').text(_topText);
        }
    };
    // 选择号码
    $('.number-list').on('click', 'a', function(e)  {
        var adLimit = parseInt($(this).attr('data-advancelimit')) || 0;
        if (adLimit > 0) {
            popupShow('对不起，您选择的号码已被预定，<br />请重新选择号码!');
            return;
        }
        var _number = $(e.currentTarget).text().replace('靓', '');
        var niceRule = $(e.currentTarget).attr('data-niceRule');
        var monthLimit = $(e.currentTarget).attr('data-monthLimit');
        if (!requestFlag) {
            occupyNumber(_number, niceRule, monthLimit);
        }
    });
    // 号码被预占,重新选择号码
    $('#reselect-number').on('click', function () {
        $('#search-btn').show();
        $('#search-close-btn').hide();
        setNumber();
        $('#error').hide();
        $('#number-popup').show();
    });
    // 配送地市弹出层
    $('#delivery').on('click', function () {
        if (commonCheck.CustCheck.checkNumAddress($('#location .p-content').text())) {
            cityChange(req.postInfo.webCity, req.postInfo.webCounty);
            var _mask = $('.mask');
            _mask.show();
            noScroll();
            $('#post').addClass('slip');
            _mask.one('click', function () {
                $('#post').removeClass('slip');
                setTimeout(function () {
                    _mask.hide();
                    reScroll();
                }, 300);
            });
        }
    });
    // 邮寄省份切换
    $('#post-province').on('click', 'li', function(e)  {
        var _this = $(e.currentTarget);
        var _code = _this.data('code');
        _this.addClass('selected').siblings('li').removeClass('selected');
        provinceChangePost(_code);
        setPost(req, _code);
    });
    // 邮寄地市切换
    $('#post-city').on('click', 'li', function(e)  {
        var _this = $(e.currentTarget);
        var _code = _this.data('code');
        _this.addClass('selected').siblings('li').removeClass('selected');
        cityChange(_code);
    });
    // 邮寄区县切换
    $('#post-district').on('click', 'li', function(e)  {
        var _currentP = $('#post-province li.selected');
        var _currentC = $('#post-city li.selected');
        var _this = $(e.currentTarget);
        if (_currentC.length != 0) {
            _this.addClass('selected').siblings('li').removeClass('selected');
            $('#delivery .p-content').text(_currentP.text() + ' ' + _currentC.text() + ' '+ _this.text()).removeClass('grey');
            req.postInfo.webCity = _currentC.data('code') + '';
            req.postInfo.webCounty = _this.data('code') + '';
            $('#post').removeClass('slip');
            if ($('#post-district li.selected').length != 0) {
                $('#delivery').removeClass('error');
                $('#top-desc').text(_topText).removeClass('error');
            }
            setTimeout(function () {
                $('.mask').unbind('click').hide();
                reScroll();
            }, 300);
        }
    });
    // 同意入网协议
    $('#protocol').on('click', function(e)  {
        var _this = $(e.currentTarget);
        if (_this.hasClass('agree')) {
            $('#submit').addClass('disable');
            _this.removeClass('agree');
        } else {
            $('#submit').removeClass('disable');
            _this.addClass('agree');
        }
    });
    // 自提点参数
    var reqSince = {};
    // 自提点弹框
    var getSince = function () {
        reqSince.provinceCode = req.postInfo.webProvince;
        reqSince.cityCode = req.postInfo.webCity;
        reqSince.countyCode = req.postInfo.webCounty;
        reqSince.goodsId = req.goodInfo.goodsId;
    };
    // 提交参数准备
    var preSubmit = function(state)  {
        if (state) {
            req.postInfo.selfFetchCode = $('.since-content').find('input:checked').val();
        }
        req.certInfo = {};
        req.certInfo.certTypeCode = '02';
        req.certInfo.certName = $('#certName').val().trim();
        req.certInfo.certId = $('#certNo').val().trim();
        req.certInfo.contractPhone = $('#mobilePhone').val().trim();
        req.postInfo.address = $('#address').val().trim();
        req.address2 = $('#address_2').html().trim() + ' ' + req.postInfo.address;
        req.exn = $('#exn').val().trim();
        var keyword_label = document.getElementById("keyword");
        if (keyword_label){
            req.keyword = $('#keyword').val().trim();
        }
        req.numInfo.number = $('#num').val().trim();
        req.meal = $('.set_meal:checked').val().trim();
        req.u = initParam.u;
        if(initParam.channel){
            req.channel = initParam.channel;
        }
        var _cache = {};
        _cache.certInfo = req.certInfo;
        _cache.post = req.postInfo;
        sessionStorage.setItem('ANT_CARD', JSON.stringify(_cache));
        if(initParam.isMainSubFlag == '1'){
            req.familyCardInfo = {};
            req.familyCardInfo.mainFlag = '0';
            req.familyCardInfo.familyCardNumber =  mainCardInfo.mainCardNum;
        }
    };
    // 提交请求
    function submit() {
        $('.mask').hide();
        $('.subLoad').show();
        var succHtml;
        reqData = JSON.stringify(req);
        $.ajax({
            type: 'post',
            url: '/pretty/index/buyp',
            data: req,
            dataType: 'json',
            //contentType: 'application/json',
            success: function(data)  {
                $('.subLoad').hide();
                if (data.rspCode == '0000') {
                    var order_id = data.order_id;

                    $('#num_price').html(data.num_price);
                    $('#send_bill').html(data.send_bill);
                    $('#buy_number').html(data.number);
                    $('#order_id').html(data.order_id);
                    $('#success, .mask').show();
                    noScroll();

                    var type = $("#submit").data('type');
                    if(type == 'tx'){
                        gdt('track', 'RESERVATION', {'key1': 'value1', 'key2': 'value2'});
                    }else if(type == 'bd'){
                        window._agl && window._agl.push(['track', ['success', {t: 3}]])
                    }else if(type == 'tt'){
                        var tt_id = $("#submit").data('id');
                        meteor.track('form', {convert_id: tt_id })
                    }else if(type == 'ks'){
                        var tt_id = $("#submit").data('id');
                        _ks_trace.push({event: 'form', convertId: tt_id, cb: function(){}});
                    }

                    var browser = browserRedirect();

                    $('#rg_apply3').on('click', function(e) {
                        $.ajax({
                            type: 'post',
                            url: '/pretty/index/pay',
                            data: {'order_id':order_id,'type':browser},
                            success: function (res1) {
                                if(browser == 1){
                                    if(res1.pay_url){
                                       window.location.href = res1.pay_url;
                                    }else{
                                        alert(res1.rspDesc);
                                    }
                                }else{
                                    $("#qrcode").html('');
                                    $("#qrcode").qrcode({
                                        render: "canvas",
                                        width: 150,
                                        height:150,
                                        background: "#FFF",
                                        text: res1.pay_url
                                    });
                                    $('.popup, .mask').hide();
                                    $('#wxqrcode, .mask').show();
                                }
                            }
                        })
                    });

                    $('#rg_apply4').on('click', function(e) {
                        $.ajax({
                            type: 'post',
                            url: '/pretty/index/pay',
                            data: {'order_id':order_id,'type':browser,'payment':2},
                            success: function (res1) {
                                if(res1.pay_url){
                                    window.location.href = res1.pay_url;
                                }else{
                                    alert(res1.rspDesc);
                                }
                            }
                        })
                    });

                } else if (data.rspCode == '0005') {
                    $('#error, .mask').show();
                    noScroll();
                    $('#reserved-number').html('<span>' + $('#number .p-content').text() + '</span>号码已被抢占。');
                } else if (data.rspCode == '0009') {
                    $.noScroll();
                    $('#errorAll,.mask').show();
                    if (data.rspDesc.length>21) {
                        $('#errorAll .popup-desc').html('该营业厅未绑定发展人，不具备推广权限<br>暂无法下单！');
                    } else {
                        $('#errorAll .popup-desc').html('该营业厅不具备此产品的推广权限<br>暂无法下单！');
                    }
                } else {
                    $('#errorAll, .mask').show();
                    noScroll();
                    $('#errorAll .popup-desc').text(data.rspDesc);
                }
                $('#since').hide();
                subStatus = true;
                requestFlag = false;
            },
            error: function () {
                $('.subLoad').hide();
                $('#overtime, .mask').show();
                noScroll();
                $('#since').hide();
                subStatus = true;
                requestFlag = false;
            }
        });
    }
    // 提交
    $('#submit').on('click', function () {
        preSubmit(false);
        if (requestFlag) {
            return;
        }
        if (!$('#protocol').hasClass('agree')) {
            return;
        }
        if (verify()) {
            getSince();
            requestFlag = true;
            $('#top-desc').text(_topText).removeClass('error');
            preSubmit(false);
            successFlag = true;
            submit();
        }
    });
    // 营业厅自提
    $('.sinceBtn').on('click', function () {
        if (subStatus) {
            preSubmit(true);
            successFlag = false;
            submit();
        }
    });
    // 不自提，物流配送
    $('.noSince').on('click', function () {
        if (subStatus) {
            preSubmit(false);
            successFlag = true;
            submit();
        }
    });
    // 自适应
    $(window).resize(function () {
        $.resize();
    });
    // 地址输入
    $('#address').on({
        keydown: function(e)  {
            if (e.keyCode == 13) {
                e.preventDefault();
            }
        },
        input: function(e)  {
            var _this = $(e.currentTarget);
            var _address = _this.val();
            var _height = $('#address-temp').text(_address).height();
            _this.css('height', _height);
        }
    });

    function browserRedirect() {
        var sUserAgent= navigator.userAgent.toLowerCase();
        var bIsIpad= sUserAgent.match(/ipad/i) == "ipad";
        var bIsIphoneOs= sUserAgent.match(/iphone os/i) == "iphone os";
        var bIsMidp= sUserAgent.match(/midp/i) == "midp";
        var bIsUc7= sUserAgent.match(/rv:1.2.3.4/i) == "rv:1.2.3.4";
        var bIsUc= sUserAgent.match(/ucweb/i) == "ucweb";
        var bIsAndroid= sUserAgent.match(/android/i) == "android";
        var bIsCE= sUserAgent.match(/windows ce/i) == "windows ce";
        var bIsWM= sUserAgent.match(/windows mobile/i) == "windows mobile";

        if (bIsIpad || bIsIphoneOs || bIsMidp || bIsUc7 || bIsUc || bIsAndroid || bIsCE || bIsWM) {
            return 1;
        } else {
            return 3;
        }
    }

});
