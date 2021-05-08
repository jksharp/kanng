/**
 * ==========================================
 * base.js
 * Copyright (c) 2010 wwww.114la.com
 * Author: cai@115.com
 * ==========================================
 */
/*$("#date").el.innerHTML = Calendar.show();//显示日期*/

function LocalData_callback(){
	if(typeof Local_city_arr!="undefined"){
		var Sites = Local_city_arr; 
		var TPL = '<dt id="loc_t"><a href="#{url}">#{name}</a></dt><dd id="loc_c" class="l">#{sites}</dd><dd id="loc_m" class="m"><a href="#{url}">更多 &raquo;</a></dd>';
		var site_arr = '';
		for(var i=0,len = Sites.son.length; i<len ; i++){
			site_arr+='<a href="'+Sites.son[i]['url']+'">'+Sites.son[i]['name']+'</a>';
		}
		var result = format(TPL,{
			url:Sites["url"],
			name:Sites["name"],
			sites:site_arr
		});
		$("#local").el.innerHTML = result;
		$("#local").el.style.visibility = 'visible';
	}
}

(function(){	
	var CityCookieName = "citydata";
	var defaultCID = '101010100';
	var getData = function(cid){
		var script = document.createElement("script");
		var head = document.getElementsByTagName("head")[0];
		script.charset = "utf-8";
		script.src = 'static/js/city/'+cid+'.js';
		
		
		
		
		
		if (Browser.isIE) {
			try{
			if(Local_city_arr){
				Local_city_arr = null;
				delete Local_city_arr;
			}
			}catch(e){}
			script.onreadystatechange = (function(){
				if (script.readyState == 'loaded' || script.readyState == 'complete') {
					if(typeof Local_city_arr == "undefined"){
						getData(defaultCID);
					}
				}
			})
		}else{
			script.onerror = (function(){
				getData(defaultCID);
			});
		}

		head.insertBefore(script, head.firstChild);
		
	}
	var i=0;
	var hockLocal = window.setInterval(function(){
		i++;
		if(Cookie.get(CityCookieName) || i<30){
			//console.log(i);
			window.clearInterval(hockLocal);
			var CookieArray = Cookie.get(CityCookieName).split(",");
			var cityid = CookieArray[1];
			if(cityid){
				getData(cityid);
			}else{
				getData(defaultCID);
			}
		}else if(i>30 && typeof Local_city_arr=="undefined"){
			getData(defaultCID);
			window.clearInterval(hockLocal);
			return;
		}
	},13);
})();

Date.prototype.format = function(format){
	var o = {
		"M+" : this.getMonth()+1, //month
		"d+" : this.getDate(),    //day
		"h+" : this.getHours(),   //hour
		"m+" : this.getMinutes(), //minute
		"s+" : this.getSeconds(), //second
		"q+" : Math.floor((this.getMonth()+3)/3),  //quarter
		"S" : this.getMilliseconds() //millisecond
	}
	if(/(y+)/.test(format)){
		format=format.replace(RegExp.$1,(this.getFullYear()+"").substr(4 - RegExp.$1.length));
	}
	for(var k in o){
		if(new RegExp("("+ k +")").test(format)){
			format = format.replace(RegExp.$1,RegExp.$1.length==1 ? o[k] :("00"+ o[k]).substr((""+ o[k]).length));
		}
	}
	return format;
}

$("#jp_today,#jd_fromDate,#jd_toDate").each(function(el){
	el.value = new Date(new Date().getTime()+3600*24*1000).format("yyyy-MM-dd");
});	

$("#skinlist a").on('click',function(el){
	var num = parseInt(el.innerHTML)-1;
	Cookie.set("style",num);
	skinSelector.set("style",num);	
});

document.onclick = function(e){
    var e = e || window.event, obj = e.srcElement || e.target, tid = obj.id;

	if(tid!=="showSetting"){
		if($("#settingBox").el){
			$("#settingBox").hide();
		}
	}

    if (
		(obj.tagName && obj.tagName.toUpperCase()== "A") || 
		(obj.parentNode.tagName && obj.parentNode.tagName.toUpperCase() == "A") || 
		(obj.parentNode.parentNode.tagName && obj.parentNode.parentNode.tagName.toUpperCase() == "A")){
		
		
		if(obj.rel && obj.rel=='nr'){ return;}
		
		var L,T;
		if(obj.parentNode.tagName.toUpperCase() == "A" && obj.tagName.toUpperCase() =="IMG"){
			L = obj.parentNode.href,T = obj.alt;
		}else if(obj.parentNode.parentNode.tagName.toUpperCase() == "A"){
			L = obj.parentNode.parentNode.href,
			T = obj.innerHTML;
		}else{
			L = obj.href , T = obj.innerHTML;
			if(Yl.trim(L)=="javascript:void(0);" || Yl.trim(L)=="javascript:void(0)"){
				L = T;
			}
			if(obj.getAttribute("rel")){
				L=T = obj.innerHTML;
			}
		}
		KeywordCount({
			u: L,
			n: T,
			q: 0
		});
		UserTrack.add(obj);
    }
	Config.Track.forEach(function(element){
		if(tid==element[0]){
			KeywordCount(element[1]);
		}
	});
};

$("#showSetting").on('click',function(el){
	el.blur();
	if($('#settingBox').el){
		$('#settingBox').show();
		return;
	}
	$("#wrap").create('div',{id:'settingBox'},function(el){
		var html = Yl.createFrame({
			src: '/public/widget/setting/index.html',
			width: "260",
			height: "230"
		});
		var h2 = '<span class="h2">个性设置</span>';
		el.innerHTML = h2 + '<div id="LSIMG" class="loading"></div><div id="loadSettingBox" style="display:none;">'+html+'</div>';
		this.append(el);
		var iframe = el.getElementsByTagName("iframe")[0];
		Yl.loadFrame(iframe, function(){
			$("#LSIMG").hide();
			el.innerHTML = h2+html;
			$("#settingBox .h2").on('click',function(el){
				$('#settingBox').hide();
			});
		});
	});
});

(function(){  
	if(!$("#topsite em").el) return;	  
	var timer = 200; //下拉菜单延时
	var activeContent;
	var hideState = true;
	var delayInterval;
	var hide = function(){
		if(hideState && activeContent){
			activeContent.style.display = "none";
			
		}
	}
	$("#topsite em").each(function(el){
		el.onmouseover = function(){
			hide();
			var box = el.parentNode.getElementsByTagName("div")[0];
			delayInterval = window.setTimeout(function(){$(box).show()},timer);
			activeContent = box;
			hideState = false;
			if(!box.onmouseover){
				box.onmouseover = function(){
					hideState = false;
				}
				box.onmouseout = function(){
					hideState = true;
					window.setTimeout(hide,timer);
				}
			}
		}
		el.onmouseout = function(){
			hideState = true;
			window.setTimeout(hide,timer);
			if(delayInterval!=undefined){
				window.clearTimeout(delayInterval);
			}

		}
	});
})();//结束名站子站点菜单


var HoverTab = function(el,fn){
 var evt = ["click", "mouseover"], 
	MouseDelayTime = 300, //鼠标延停时间
 	waitInterval;
	var rel = el.getAttribute("rel");
	evt.forEach(function(element){
		switch (element) {
			case "click":
				if(waitInterval){
					window.clearTimeout(waitInterval);
				}
				el["on" + element] = function(){
					fn();
					if(rel){
						KeywordCount({
							u: rel,
							n: rel,
							q: 0
						});
					}
				}
				break;
			case "mouseover":
				el["on" + element] = function(){
					if(waitInterval){
						window.clearTimeout(waitInterval);
					}
					waitInterval = window.setTimeout(function(){
						fn();
						if(rel){
							KeywordCount({
								u: rel,
								n: rel,
								q: 0
							});
						}
					}, MouseDelayTime);
					
					
				}
				el["onmouseout"] = function(){
					if (waitInterval != undefined) {
						window.clearTimeout(waitInterval);
					}
				}
				break;
		}
	});	
}


//名站TAB菜单开始

$("#bm_tab li").each(function(el){
		HoverTab(el,run);
        function run(){
            $("#bm_tab li").removeClass("active");
            el.className = "active";
            show(el);
        }
        function show(el){
			var stm;
            var boxid = el.getAttribute("rel"), url = el.getAttribute("url"), noCache = el.getAttribute("nocache");
            if (!boxid) {
                return;
            }
            var Tabs = cache.get("BOARD_BOX_TAB");
            if(('#'+boxid)==cache.get("LAST_BOXTAB")){
				return;
			}
            if (cache.is("LAST_BOXTAB")) {
                var box = $(cache.get("LAST_BOXTAB"));
                if (box.el.className == "nocache") {
					box.el.getElementsByTagName("iframe")[0].src="";
					box.hide();
                }else {
                    box.hide();
                }
            }
            else {
                $("#fm").hide();
            }
            if (!url) {
                $("#" + boxid).show();
                cache.set("LAST_BOXTAB", "#" + boxid);
                return;
            }else if (cache.is("BOARD_BOX_TAB") == false) {
                    createTabBox();					
            }else {
				if (Tabs.indexOf(boxid) == -1) {
					createTabBox();
					
				}
            }
            
            function createTabBox(){
				    var newNode = document.createElement("div");
					newNode.id=boxid;
					var newHtml = '<iframe width="100%" height="272" frameborder="0" scrolling="no" allowtransparency="true" src="'+url+'"></iframe>';
					newNode.innerHTML=newHtml;
					$("#bb .box").el.insertBefore(newNode,$("#bb .box").el.firstChild);
					if(noCache) {
                        newNode.className = "nocache";
						cache.set("BOARD_BOX_TAB", boxid, 1);
                    }else {
                        cache.set("BOARD_BOX_TAB", boxid, 1);
                    }
            }
            cache.set("LAST_BOXTAB", "#" + boxid);
			if(noCache){
				$("#" + boxid).show();
				$("#"+boxid).el.getElementsByTagName("iframe")[0].src=url;
			}else {$("#" + boxid).show();}
        }//show
    });
//结束名站切换版块Tab菜单







/*历史记录*/
var UserTrack = (function(){
	function add(o){
		try{
			if(o.tagName.toUpperCase() ==("A") && o.href.indexOf("http://") == 0 && o.href.indexOf("http://" + Yl.getHost())!= 0 ){
				if(o.rel && o.rel=="nr"){
					return;
				}
				var Track ={
					url: o.href,
					content: o.innerHTML
				},
				data = Track.url+"+"+Track.content+"_[YLMF]_",
				oldData = Cookie.get("history");
				if(oldData){
					if(oldData.indexOf(data)>-1){
						oldData = oldData.replace(data,"");
					}				
					data += oldData;
				}
				//Cookie.set("history",data,null,null,'114la.com');
				Cookie.set("history",data);
				var Hbox;
				if( document.getElementById('bb1')){
					Hbox = document.getElementById('bb1').getElementsByTagName("iframe");
				}
				if(Hbox && Hbox.length){
					Hbox[0].contentWindow.History.show();
				}
			}
			
		}catch(e){}
	
	};
	
	return{
		add: add
	}
	
})(); 

//底部搜索
var miniSearch = (function(){
    var I = "s0";
    var Q = $("#f_int input").el;
    $("#f_radio input.radio").each(function(el){
        if (el.checked == true) {
            I = el.id;
        }
        el.onclick = function(){
            I = this.id;
        }
    });
	function count(){
		KeywordCount({
			type: I,
			word: Q.value,
			url: window.location.href,
			key: cache.is("CLICK_BS_BTN")?"B":13
		},"http://www.tjj.com/click.php");
		cache.remove("CLICK_BS_BTN");
	}
	$("#f_btn input").on("click",function(el){
		cache.set("CLICK_BS_BTN",1);
	});
    function gosearch(_this){
		
        if (I == "s0") {
			count();
            _this.submit();
            return;
        }
        else {
            var url;
            switch (I) {
                case "s1":
                    url = "www.google.com.hk/search?client=pub-0194889602661524&channel=3192690043&forid=1&prog=aff&hl=zh-CN&source=sdo_cts_html&q=";
                    break;
                case "s3":
                    $("#taobao-q").el.value = Q.value;
                    if (!cache.get("TAOBAO_PARM")) {
                        var p = {
							pid:'mm_18036115_2311920_9044980',
							commend: "all",
							search_type: "action"
						}
                        for (var i in p) {
                            $("#taobao-form").create("input", {
                                type: "hidden",
                                name: i,
                                value: p[i]
                            }, function(el){
                                this.append(el);
                            })
                        }
                        cache.set("TAOBAO_PARM", true);
                    }
					count();
                    $("#taobao-form").el.submit();
					
                    return;
					break;
                case "s4":
                    url = "116.com/s?q=";
                    break;
				case "s5":
					window.open("http://114la.gouwuke.com/search-" + Q.value+".html?oid=46962&gsid=126762");
					count();
					return;
                    break;
                default:
                    break;
            }
			
            window.open("http://" + url + encodeURIComponent(Q.value));
			count();
        }
    }
    return {
        gosearch: gosearch
    }
})();

//工具轮换tab
$("#tool-tab li").each(function(el){
	HoverTab(el,function(){
		$("#tool-tab li").removeClass("active");
		el.className = "active";
		show(el.getAttribute("rel"));
		
	});
	var show = function(box){
		$(".tbox").hide();
		$("#"+box).show();
	}
});



function KeywordCount(keyword, Counturl){
    if (!keyword || keyword =="") {
        return
    }
    var url = Counturl || "http://www.tjj.com/index";
	
	
    var rd = new Date().getTime()
    var Count = new Image();
    var countVal = "";
	for (var i in keyword) {

			if(i=='u'){
				countVal += ('?'+ i +'=' + encodeURIComponent(keyword[i]));
			}else{
				countVal += ('&' + i +'=' + encodeURIComponent(keyword[i]));
			}
	}
	if(url=="http://www.tjj.com/index"){
		Count.src = url+countVal+'&i='+rd;
	}else{
		Count.src = url + "?i=" + rd + countVal;
	}
}

DOMReady(function(){
	if(Browser.isIE){			  
		
	}
	var setFocus = function(){
		
	}
    $("#f_int input,#q_int input").on("mouseover", function(el){

		if (cache.get(el.id + "GETFOCUS") && el.parentNode.id =="q_int") {
            return;
        }
		if(document.all){
        	Yl.getFocus(el);
		}else{
			el.focus();
		}
    });
/*	$(".tbox .int_b").on('mouseover',function(el){
		el.select();
	});*/
	
    $("#f_int input,#q_int input").on("blur", function(el){
        cache.remove(el.id + "GETFOCUS");
    });

});
var daodao = (function(){
	return {
		searchTravel:function(){
			var _q = document.getElementById("daodao_travel_q").value;
			var _k = document.getElementById("daodao_travel_k").value;
			var _kw = "http://www.daodao.com/Search?m=13078";
			if(_q&&!_k){ _kw += "&q="+encodeURIComponent(_q)}
			else if(!_q&&_k){ _kw += "&q="+ encodeURIComponent(_k)}
			else if(_q&&_k){_kw += "&q="+  encodeURIComponent(_q + "+" +_k)}
			window.open(_kw);
		}
	}
})();

function ResumeError(){
    return true
}
window.onerror = ResumeError;
