<?xml version="1.0" encoding="utf-8"?>
<rss version="2.0">
<channel>
<title><![CDATA[hao123IT博客]]></title> 
<description><![CDATA[hao123IT导航 IT博客]]></description>
<link>http://kanng.net/blog/</link>
<language>zh-cn</language>
<generator>www.emlog.net</generator>
<item>
	<title>查询数据库中某个值在哪个表</title>
	<link>http://kanng.net/blog/?post=21</link>
	<description><![CDATA[<p>
	<span>数据库不是自己做的时候，又没有文档，想查询某个值在哪个表，可以用这种方式：</span>
</p>
<p>
	<span><br />
</span>
</p>
<p>
	<span style="color:#99BB00;">DROP PROCEDURE Full_Search</span><span style="color:#99BB00;"></span>
</p>
<span style="color:#99BB00;">CREATE proc Full_Search(@string varchar(50)) &nbsp;&nbsp;</span><br />
<span style="color:#99BB00;">as &nbsp;&nbsp;</span><br />
<span style="color:#99BB00;">begin &nbsp;&nbsp;</span><br />
<span style="color:#99BB00;">&nbsp;</span><br />
<span style="color:#99BB00;">declare @tbname varchar(50) &nbsp;&nbsp;</span><br />
<span style="color:#99BB00;">declare tbroy cursor for select name from sysobjects &nbsp;&nbsp;</span><br />
<span style="color:#99BB00;">where xtype= 'u ' --第一个游标遍历所有的表 &nbsp;&nbsp;</span><br />
<span style="color:#99BB00;">&nbsp;</span><br />
<span style="color:#99BB00;">open tbroy &nbsp;&nbsp;</span><br />
<span style="color:#99BB00;">fetch next from tbroy into @tbname &nbsp;&nbsp;</span><br />
<span style="color:#99BB00;">while @@fetch_status=0 &nbsp;&nbsp;</span><br />
<span style="color:#99BB00;">begin &nbsp;&nbsp;</span><br />
<span style="color:#99BB00;">&nbsp;</span><br />
<span style="color:#99BB00;">declare @colname varchar(50) &nbsp;&nbsp;</span><br />
<span style="color:#99BB00;">declare colroy cursor for select name from syscolumns &nbsp;&nbsp;</span><br />
<span style="color:#99BB00;">where id=object_id(@tbname) and xtype in ( &nbsp;&nbsp;</span><br />
<span style="color:#99BB00;">select xtype from systypes &nbsp;&nbsp;</span><br />
<span style="color:#99BB00;">where name in ( 'varchar ', 'nvarchar ', 'char ', 'nchar ') --数据类型为字符型的字段 &nbsp;&nbsp;</span><br />
<span style="color:#99BB00;">) --第二个游标是第一个游标的嵌套游标，遍历某个表的所有字段 &nbsp;&nbsp;</span><br />
<span style="color:#99BB00;">&nbsp;</span><br />
<span style="color:#99BB00;">open colroy &nbsp;&nbsp;</span><br />
<span style="color:#99BB00;">fetch next from colroy into @colname &nbsp;&nbsp;</span><br />
<span style="color:#99BB00;">while @@fetch_status=0 &nbsp;&nbsp;</span><br />
<span style="color:#99BB00;">begin &nbsp;&nbsp;</span><br />
<span style="color:#99BB00;">&nbsp;</span><br />
<span style="color:#99BB00;">declare @sql nvarchar(1000),@j int &nbsp;&nbsp;</span><br />
<span style="color:#99BB00;">select @sql= 'select @i=count(1) from ' +@tbname + ' where '+ @colname+ ' like '+ '''%'+@string+ '%''' &nbsp;&nbsp;</span><br />
<span style="color:#99BB00;">exec sp_executesql @sql,N'@i int output',@i=@j output --输出满足条件表的记录数 &nbsp;&nbsp;</span><br />
<span style="color:#99BB00;">if @j&gt; 0 &nbsp;&nbsp;</span><br />
<span style="color:#99BB00;">BEGIN &nbsp;</span><br />
<span style="color:#99BB00;">select 包含字串的表名=@tbname &nbsp;</span><br />
<span style="color:#99BB00;">--exec( 'select distinct '+@colname+' from ' +@tbname + ' where '+ @colname+ ' like '+ '''%'+@string+ '%''') &nbsp;&nbsp;</span><br />
<span style="color:#99BB00;">END &nbsp;</span><br />
<span style="color:#99BB00;">fetch next from colroy into @colname &nbsp;&nbsp;</span><br />
<span style="color:#99BB00;">end &nbsp;&nbsp;</span><br />
<span style="color:#99BB00;">&nbsp;</span><br />
<span style="color:#99BB00;">close colroy &nbsp;&nbsp;</span><br />
<span style="color:#99BB00;">deallocate colroy &nbsp;&nbsp;</span><br />
<span style="color:#99BB00;">&nbsp;</span><br />
<span style="color:#99BB00;">fetch next from tbroy into @tbname &nbsp;&nbsp;</span><br />
<span style="color:#99BB00;">end &nbsp;&nbsp;</span><br />
<span style="color:#99BB00;">close tbroy &nbsp;&nbsp;</span><br />
<span style="color:#99BB00;">deallocate tbroy &nbsp;&nbsp;</span><br />
<span style="color:#99BB00;">end &nbsp;&nbsp;</span><br />
<p>
	<span style="color:#99BB00;">go &nbsp;</span>
</p>
<p>
	<span style="color:#99BB00;">----调用</span>
</p>
<p>
	<span style="color:#99BB00;">exec Full_Search '123' &nbsp;&nbsp;</span><span style="color:#99BB00;"></span>
</p>
<p>
	<br />
</p>]]></description>
	<pubDate>Thu, 12 Feb 2015 01:38:13 +0000</pubDate>
	<author>admin</author>
	<guid>http://kanng.net/blog/?post=21</guid>

</item>
<item>
	<title>支付宝收款主页另一种实现，可用于接受捐赠</title>
	<link>http://kanng.net/blog/?post=20</link>
	<description><![CDATA[<div style="margin:0px;padding:0px;font-family:'Gotham Narrow SSm', 'Microsoft Jhenghei', 'Microsoft Yahei', 'Helvetica Neue', HelveticaNeue, Arial, sans-serif;font-size:14px;background-color:#FAFAFA;">
	<p style="text-indent:2em;">
		大家都知道PayPal有一個非常使用的「一鍵<a href="http://www.arefly.com/tag/%e8%bd%89%e8%b3%ac/" target="_blank">轉賬</a>」按鈕功能，但是自從<a href="http://www.arefly.com/tag/%e6%94%af%e4%bb%98%e5%af%b6/" target="_blank">支付寶</a>的個人收款主頁停止服務後，支付寶已經無法實現該類功能了。不過方法總還是有的，今天「暢想資源」就來教大家使用一個極其簡單的&nbsp;form&nbsp;標籤經過&nbsp;POST&nbsp;可以一鍵自動填寫「<a href="http://www.arefly.com/tag/%e6%94%af%e4%bb%98%e5%af%b6%e8%bd%89%e8%b3%ac/" target="_blank">支付寶轉賬</a>頁面」的信息，讓大家更方便、更快捷的轉賬！
	</p>
</div>
<span id="more-7641" style="font-family:'Gotham Narrow SSm', 'Microsoft Jhenghei', 'Microsoft Yahei', 'Helvetica Neue', HelveticaNeue, Arial, sans-serif;font-size:14px;line-height:21px;background-color:#FAFAFA;"></span>
<h3 style="font-size:15px;font-weight:normal;font-family:'Gotham Narrow SSm', 'Microsoft Jhenghei', 'Microsoft Yahei', 'Helvetica Neue', HelveticaNeue, Arial, sans-serif;background:#EDEDED;">
	預覽
</h3>
<p class="text-center blue" style="text-align:center;color:blue;text-indent:2em;font-family:'Gotham Narrow SSm', 'Microsoft Jhenghei', 'Microsoft Yahei', 'Helvetica Neue', HelveticaNeue, Arial, sans-serif;font-size:14px;background-color:#FAFAFA;">
	按鈕：（試試按下去吧！&nbsp;<img src="http://www.arefly.com/wp-content/themes/weisaysimple/emoticon/grin.png" alt=":grin:" class="wp-smiley" />&nbsp;）
</p>
<br />
<p class="text-center blue" style="text-align:center;color:blue;text-indent:2em;font-family:'Gotham Narrow SSm', 'Microsoft Jhenghei', 'Microsoft Yahei', 'Helvetica Neue', HelveticaNeue, Arial, sans-serif;font-size:14px;background-color:#FAFAFA;">
	結果：
</p>
<p class="text-center" style="text-align:center;text-indent:2em;font-family:'Gotham Narrow SSm', 'Microsoft Jhenghei', 'Microsoft Yahei', 'Helvetica Neue', HelveticaNeue, Arial, sans-serif;font-size:14px;background-color:#FAFAFA;">
	<a href="http://file.arefly.com/2014/09/alipay-transfer-post-button-demo.png"><img src="http://file.arefly.com/2014/09/alipay-transfer-post-button-demo-600x408.png" alt="alipay-transfer-post-button-demo" width="600" height="408" class="aligncenter size-large wp-image-7650" style="height:auto;" /></a>
</p>
<h3 style="font-size:15px;font-weight:normal;font-family:'Gotham Narrow SSm', 'Microsoft Jhenghei', 'Microsoft Yahei', 'Helvetica Neue', HelveticaNeue, Arial, sans-serif;background:#EDEDED;">
	教學
</h3>
<p style="text-indent:2em;font-family:'Gotham Narrow SSm', 'Microsoft Jhenghei', 'Microsoft Yahei', 'Helvetica Neue', HelveticaNeue, Arial, sans-serif;font-size:14px;background-color:#FAFAFA;">
	將下列代碼添加至你想放置即時到賬按鈕的位置即可～<span class="red" style="color:red;">（注意將逐項信息進行適當修改！）</span>
</p>
<p class="blue" style="color:blue;text-indent:2em;font-family:'Gotham Narrow SSm', 'Microsoft Jhenghei', 'Microsoft Yahei', 'Helvetica Neue', HelveticaNeue, Arial, sans-serif;font-size:14px;background-color:#FAFAFA;">
	提示：如果你想使用「<span class="filename" style="color:#4169E1;">捐款本站</span>」（而不是「<span class="filename" style="color:#4169E1;">向TA付款</span>」）按鈕，可將&nbsp;name="pay"&nbsp;後的&nbsp;src修改為&nbsp;http://file.arefly.com/alipay.png&nbsp;！
</p>
<div id="crayon-5467617994d34223324763" class="crayon-syntax crayon-theme-github crayon-font-ubuntu-mono crayon-os-pc print-yes notranslate" style="margin:5px 0px;padding:0px;font-family:Monaco, MonacoRegular, 'Courier New', monospace;border:1px solid #DEDEDE !important;background-color:#F8F8FF !important;">
	<div class="crayon-toolbar" style="margin:0px;padding:0px;font-family:'Ubuntu Mono', UbuntuMonoRegular, 'Courier New', monospace !important;background-color:#EEEEEE !important;">
		<span class="crayon-title" style="font-family:inherit;line-height:inherit !important;font-weight:inherit !important;color:#666666 !important;"></span>
		<div class="crayon-tools" style="margin:0px;padding:0px;font-family:inherit;border:0px;font-weight:inherit !important;background:0px 50%;">
			<div class="crayon-button crayon-nums-button crayon-pressed" style="margin:0px;padding:0px;font-family:inherit;border:0px;font-size:inherit !important;font-weight:inherit !important;background:-24px 0px no-repeat;">
				<div class="crayon-button-icon" style="margin:-8px 0px 0px;padding:0px;font-family:inherit;border:0px;font-size:inherit !important;font-weight:inherit !important;background:url(http://www.arefly.com/wp-content/plugins/crayon-syntax-highlighter/css/images/toolbar/buttons.png) -24px -32px;">
				</div>
			</div>
			<div class="crayon-button crayon-plain-button" style="margin:0px;padding:0px;font-family:inherit;border:0px;font-size:inherit !important;font-weight:inherit !important;background:0px 50% no-repeat;">
				<div class="crayon-button-icon" style="margin:-8px 0px 0px;padding:0px;font-family:inherit;border:0px;font-size:inherit !important;font-weight:inherit !important;background:url(http://www.arefly.com/wp-content/plugins/crayon-syntax-highlighter/css/images/toolbar/buttons.png) 0px -48px;">
				</div>
			</div>
			<div class="crayon-button crayon-wrap-button" style="margin:0px;padding:0px;font-family:inherit;border:0px;font-size:inherit !important;font-weight:inherit !important;background:0px 50% no-repeat;">
				<div class="crayon-button-icon" style="margin:-8px 0px 0px;padding:0px;font-family:inherit;border:0px;font-size:inherit !important;font-weight:inherit !important;background:url(http://www.arefly.com/wp-content/plugins/crayon-syntax-highlighter/css/images/toolbar/buttons.png) 0px -112px;">
				</div>
			</div>
			<div class="crayon-button crayon-expand-button" style="margin:0px;padding:0px;font-family:inherit;border:0px;font-size:inherit !important;font-weight:inherit !important;background:0px 50% no-repeat;">
				<div class="crayon-button-icon" style="margin:-8px 0px 0px;padding:0px;font-family:inherit;border:0px;font-size:inherit !important;font-weight:inherit !important;background:url(http://www.arefly.com/wp-content/plugins/crayon-syntax-highlighter/css/images/toolbar/buttons.png) 0px -96px;">
				</div>
			</div>
			<div class="crayon-button crayon-copy-button" style="margin:0px;padding:0px;font-family:inherit;border:0px;font-size:inherit !important;font-weight:inherit !important;background:0px 50% no-repeat;">
				<div class="crayon-button-icon" style="margin:-8px 0px 0px;padding:0px;font-family:inherit;border:0px;font-size:inherit !important;font-weight:inherit !important;background:url(http://www.arefly.com/wp-content/plugins/crayon-syntax-highlighter/css/images/toolbar/buttons.png) 0px -16px;">
				</div>
			</div>
			<div class="crayon-button crayon-popup-button" style="margin:0px;padding:0px;font-family:inherit;border:0px;font-size:inherit !important;font-weight:inherit !important;background:0px 50% no-repeat;">
				<div class="crayon-button-icon" style="margin:-8px 0px 0px;padding:0px;font-family:inherit;border:0px;font-size:inherit !important;font-weight:inherit !important;background:url(http://www.arefly.com/wp-content/plugins/crayon-syntax-highlighter/css/images/toolbar/buttons.png) 0px 0px;">
				</div>
			</div>
		</div>
	</div>
	<div class="crayon-plain-wrap" style="border:0px;margin:0px !important;padding:0px !important;font-family:'Ubuntu Mono', UbuntuMonoRegular, 'Courier New', monospace !important;background:0px 50%;">
	</div>
	<div class="crayon-main" style="margin:0px;padding:0px;border:0px;font-family:'Ubuntu Mono', UbuntuMonoRegular, 'Courier New', monospace !important;background:0px 50%;">
		<table class="crayon-table" style="font-size:12px;padding:0px !important;border:none !important;width:auto !important;background:none !important;">
			<tbody>
				<tr class="crayon-row">
					<td class="crayon-nums " style="font-family:'Ubuntu Mono', UbuntuMonoRegular, 'Courier New', monospace !important;vertical-align:top !important;border:0px;background-color:#EEEEEE !important;">
						<div class="crayon-nums-content" style="margin:0px;border:0px;background:0px 50%;">
							<div class="crayon-num" style="margin:0px;padding:0px 5px;font-family:inherit;text-align:right !important;font-size:inherit !important;font-weight:inherit !important;color:#AAAAAA !important;background:0px 50%;">
								1
							</div>
							<div class="crayon-num crayon-marked-num crayon-top crayon-striped-num" style="margin:0px;padding:0px 5px;font-family:inherit;text-align:right !important;font-size:inherit !important;font-weight:inherit !important;color:#AAAAAA !important;background:#B3D3F4 !important;">
								2
							</div>
							<div class="crayon-num crayon-marked-num" style="margin:0px;padding:0px 5px;font-family:inherit;text-align:right !important;font-size:inherit !important;font-weight:inherit !important;color:#AAAAAA !important;background:#B3D3F4 !important;">
								3
							</div>
							<div class="crayon-num crayon-marked-num crayon-striped-num" style="margin:0px;padding:0px 5px;font-family:inherit;text-align:right !important;font-size:inherit !important;font-weight:inherit !important;color:#AAAAAA !important;background:#B3D3F4 !important;">
								4
							</div>
							<div class="crayon-num crayon-marked-num" style="margin:0px;padding:0px 5px;font-family:inherit;text-align:right !important;font-size:inherit !important;font-weight:inherit !important;color:#AAAAAA !important;background:#B3D3F4 !important;">
								5
							</div>
							<div class="crayon-num crayon-marked-num crayon-bottom crayon-striped-num" style="margin:0px;padding:0px 5px;font-family:inherit;text-align:right !important;font-size:inherit !important;font-weight:inherit !important;color:#AAAAAA !important;background:#B3D3F4 !important;">
								6
							</div>
							<div class="crayon-num" style="margin:0px;padding:0px 5px;font-family:inherit;text-align:right !important;font-size:inherit !important;font-weight:inherit !important;color:#AAAAAA !important;background:0px 50%;">
								7
							</div>
						</div>
					</td>
					<td class="crayon-code" style="font-family:'Ubuntu Mono', UbuntuMonoRegular, 'Courier New', monospace !important;vertical-align:top !important;border:0px;background:0px 50%;">
						<p>
							<div class="crayon-line" id="crayon-5467617994d34223324763-1" style="margin:0px;padding:0px 5px;font-family:inherit;border:0px;font-size:inherit !important;font-weight:inherit !important;background:0px 50%;">
								<br />
							</div>
							<p style="font-family:inherit;font-size:inherit !important;font-weight:inherit !important;background:0px 50%;">
								<span>转载自：http://www.arefly.com/alipay-transfer-post-button/</span>
							</p>
						</p>
					</td>
				</tr>
			</tbody>
		</table>
	</div>
</div>]]></description>
	<pubDate>Sat, 15 Nov 2014 14:25:07 +0000</pubDate>
	<author>admin</author>
	<guid>http://kanng.net/blog/?post=20</guid>

</item>
<item>
	<title>非API方式调用谷歌翻译</title>
	<link>http://kanng.net/blog/?post=19</link>
	<description><![CDATA[<p>
	谷歌翻译API收费了，但是&nbsp;http://translate.google.cn/ 是免费的，我们模拟这个去发送请求，抓翻译结果吧。
</p>
<p>
	使用大名鼎鼎的Snoopy可以做到模拟浏览器请求。
</p>
<p>
	<span style="color:#99BB00;">require_once 'Snoopy.php';</span><br />
<span style="color:#99BB00;"> $Snoopy=new Snoopy();</span><br />
<span style="color:#99BB00;"> $Snoopy-&gt;agent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.31 (KHTML, like Gecko) Chrome/26.0.1410.64 Safari/537.31";</span><br />
<span style="color:#99BB00;"> $Snoopy-&gt;referer = "http://www.kanng.net/";</span><br />
<span style="color:#99BB00;"> // set some cookies:</span><br />
<span style="color:#99BB00;"> $Snoopy-&gt;cookies["PHPSESSID"] = "fc106b1918bd522cc863f".rand(12987,98765)."0e6fff7"; //伪装sessionid&nbsp;</span><br />
<span style="color:#99BB00;"> $Snoopy-&gt;cookies["SessionID"] = rand(12987,98765).'834723489';</span><br />
<span style="color:#99BB00;"> $Snoopy-&gt;cookies["favoriteColor"] = "blue";</span><br />
<span style="color:#99BB00;"> // set an raw-header:</span><br />
<span style="color:#99BB00;"> $Snoopy-&gt;rawheaders["Pragma"] = "no-cache";</span><br />
<span style="color:#99BB00;"> // set some internal variables:</span><br />
<span style="color:#99BB00;"> $Snoopy-&gt;maxredirs = 2;</span><br />
<span style="color:#99BB00;"> $Snoopy-&gt;offsiteok = false;</span><br />
<span style="color:#99BB00;"> $Snoopy-&gt;expandlinks = false;</span>
</p>
<p>
	<span style="color:#99BB00;">$formvars['q'] = "我要翻译这一段"</span>
</p>
<p>
	<span style="color:#99BB00;">$searchurl="http://translate.google.cn/translate_a/single?client=t&amp;sl=zh-CN&amp;tl=en&amp;hl=zh-CN&amp;dt=bd&amp;dt=ex&amp;dt=ld&amp;dt=md&amp;dt=qc&amp;dt=rw&amp;dt=rm&amp;dt=ss&amp;dt=t&amp;dt=at&amp;dt=sw&amp;ie=UTF-8&amp;oe=UTF-8&amp;prev=btn&amp;ssel=3&amp;tsel=3";</span><br />
<span style="color:#99BB00;"> $Snoopy-&gt;submit($searchurl,$formvars);//$formvars为提交的数组 &nbsp;</span><br />
<span style="color:#99BB00;"> $htmls=$Snoopy-&gt;results; &nbsp;//取得源代码</span>
</p>
<p>
	$htmls就是翻译结果了，好好分析这个结果，就能得到我们要的翻译。
</p>
<p>
	<br />
</p>]]></description>
	<pubDate>Mon, 27 Oct 2014 06:25:34 +0000</pubDate>
	<author>admin</author>
	<guid>http://kanng.net/blog/?post=19</guid>

</item>
<item>
	<title>js判断是否手机平板浏览器</title>
	<link>http://kanng.net/blog/?post=18</link>
	<description><![CDATA[<pre class="prettyprint lang-js linenums">function is_m_browser(){
try {
var urlhash = window.location.hash;
if (!urlhash.match("fromapp"))
{
if ((navigator.userAgent.match(/(iPhone|iPod|Android|ios|iPad)/i)))
{
return true;
}
}else{
return false;
}
}
catch(err)
{
return false;
}
}</pre>
<p>
	&nbsp;
</p>]]></description>
	<pubDate>Mon, 06 Oct 2014 02:10:57 +0000</pubDate>
	<author>admin</author>
	<guid>http://kanng.net/blog/?post=18</guid>

</item>
<item>
	<title>js 倒计时关闭浏览器</title>
	<link>http://kanng.net/blog/?post=16</link>
	<description><![CDATA[<p>
	项目中用到定时任务，需要定时打开浏览器允许一个php,完成之后关闭浏览器,代码如下：
</p>
<p>
	<span style="color:#99BB00;">&lt;script language="javascript"&gt;</span><br />
<span style="color:#99BB00;">function clock(){</span><br />
<span style="color:#99BB00;">i=i-1</span><br />
<span style="color:#99BB00;">if(i&gt;0){setTimeout("clock();",1000);</span><br />
<span style="color:#99BB00;">}</span><br />
<span style="color:#99BB00;">else{</span><br />
<span style="color:#99BB00;">window.opener=null;&nbsp;</span><br />
<span style="color:#99BB00;">window.open('','_self');</span><br />
<span style="color:#99BB00;">window.close();}}</span><br />
<span style="color:#99BB00;">var i=3; //3秒后关闭，自定义</span><br />
<span style="color:#99BB00;">clock();</span><br />
<span style="color:#99BB00;">&lt;/script&gt;</span><br />
</p>]]></description>
	<pubDate>Thu, 06 Mar 2014 07:03:39 +0000</pubDate>
	<author>admin</author>
	<guid>http://kanng.net/blog/?post=16</guid>

</item>
<item>
	<title>php备份mysql数据库包括所有数据</title>
	<link>http://kanng.net/blog/?post=15</link>
	<description><![CDATA[<p>
	给客户做的网站，需要加一个一键备份数据库的功能。找到了两个很好用的函数，出处记得哪里的了。
</p>
<p>
	1，下面函数是获取表机构，返回字符串。参数是数据表名称。
</p>
<p>
	<span style="color:#99BB00;">function table2sql($table) &nbsp;&nbsp;</span><br />
<span style="color:#99BB00;">{ &nbsp;</span><br />
<span style="color:#99BB00;">$tabledump = "DROP TABLE IF EXISTS $table;\n"; &nbsp;</span><br />
<span style="color:#99BB00;">$createtable = <span style="font-family:Simsun;font-size:14px;line-height:26px;background-color:#FFF4E8;">mysql_query</span>("SHOW CREATE TABLE $table"); &nbsp;</span><br />
<span style="color:#99BB00;">$create = <span style="font-family:Simsun;font-size:14px;line-height:30px;background-color:#FFF4E8;">mysql_fetch_array</span>($createtable); &nbsp;</span><br />
<span style="color:#99BB00;">$tabledump .= $create[1].";\n\n"; &nbsp;&nbsp;</span>
</p>
<p>
	<span style="color:#99BB00;">return $tabledump; &nbsp;</span><br />
<span style="color:#99BB00;">}&nbsp;</span>
</p>
<p>
	2，下面函数获取数据表的数据，返回所有数据的insert 语句。参数是表名称。
</p>
<p>
	<span style="color:#99BB00;">function data2sql($table) &nbsp;&nbsp;</span><br />
<span style="color:#99BB00;">{ &nbsp;</span><br />
<span style="color:#99BB00;">$tabledump = "DROP TABLE IF EXISTS $table;\n"; &nbsp;</span><br />
<span style="color:#99BB00;">$createtable = <span style="color:#99BB00;font-family:Simsun;font-size:14px;line-height:26px;background-color:#FFF4E8;">mysql_query</span>("SHOW CREATE TABLE $table"); &nbsp;</span><br />
<span style="color:#99BB00;">$create = <span style="color:#99BB00;font-family:Simsun;font-size:14px;line-height:30px;background-color:#FFF4E8;">mysql_fetch_array</span>($createtable); &nbsp;</span><br />
<span style="color:#99BB00;">$tabledump .= $create[1].";\n\n"; &nbsp;</span><br />
<span style="color:#99BB00;">$rows = <span style="color:#99BB00;font-family:Simsun;font-size:14px;line-height:26px;background-color:#FFF4E8;">mysql_query</span>("SELECT * FROM $table"); &nbsp;</span><br />
<span style="color:#99BB00;">$numfields = mysql_num_fields($rows); &nbsp;</span><br />
<span style="color:#99BB00;">$numrows = mysql_num_rows($rows); &nbsp;</span><br />
<span style="color:#99BB00;">while ($row = <span style="color:#99BB00;font-family:Simsun;font-size:14px;line-height:30px;background-color:#FFF4E8;">mysql_fetch_array</span>($rows)) &nbsp;</span><br />
<span style="color:#99BB00;">{ &nbsp;</span><br />
<span style="color:#99BB00;">$comma = ""; &nbsp;</span><br />
<span style="color:#99BB00;">$tabledump .= "INSERT INTO $table VALUES("; &nbsp;</span><br />
<span style="color:#99BB00;">for($i = 0; $i &lt; $numfields; $i++) &nbsp;&nbsp;</span><br />
<span style="color:#99BB00;">{ &nbsp;</span><br />
<span style="color:#99BB00;">$tabledump .= $comma."'".MySQL_escape_string($row[$i])."'"; &nbsp;</span><br />
<span style="color:#99BB00;">$comma = ","; &nbsp;</span><br />
<span style="color:#99BB00;">} &nbsp;</span><br />
<span style="color:#99BB00;">$tabledump .= ");\n"; &nbsp;</span><br />
<span style="color:#99BB00;">} &nbsp;</span><br />
<span style="color:#99BB00;">$tabledump .= "\n"; &nbsp;</span><br />
<span style="color:#99BB00;">return $tabledump; &nbsp;</span><br />
<span style="color:#99BB00;">} &nbsp;</span>
</p>
<p>
	<span>有了这两个函数就简单了，只有再获取所有表，循环调用这些函数就可以：</span>
</p>
<p>
	<span><span style="color:#99BB00;">$allsql="";</span><br />
<span style="color:#99BB00;">&nbsp;$result = <span style="color:#99BB00;font-family:Simsun;font-size:14px;line-height:26px;background-color:#FFF4E8;">mysql_query</span>("SHOW tables"); &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</span><br />
<span style="color:#99BB00;">while ($currow=<span style="color:#99BB00;font-family:Simsun;font-size:14px;line-height:30px;background-color:#FFF4E8;">mysql_fetch_array</span>($result))&nbsp;</span><br />
<span style="color:#99BB00;">{ &nbsp;&nbsp;</span><br />
<span style="color:#99BB00;">$allsql.=table2sql($currow['0']);</span><br />
<span style="color:#99BB00;">$allsql.=data2sql($currow['0']);</span><br />
<span style="color:#99BB00;">}</span><br />
<span style="color:#99BB00;"> echo&nbsp;</span><span style="color:#99BB00;">$allsql;</span><span style="color:#99BB00;"> 输出表结构和数据字符串</span><br />
</span>
</p>]]></description>
	<pubDate>Mon, 03 Mar 2014 07:57:42 +0000</pubDate>
	<author>admin</author>
	<guid>http://kanng.net/blog/?post=15</guid>

</item>
<item>
	<title>js防止重复点击保存提交表单</title>
	<link>http://kanng.net/blog/?post=14</link>
	<description><![CDATA[<p>
	往往有些用户网络慢或者其他问题，在提交表单的时候使劲点击保存提交按钮，在提交表单的时候加上下面的代码，即可以限制在一定时间内，只有一次点击是有效的。
</p>
<p>
	&lt;script&gt;
</p>
var mypretime=0;<br />
function sub(){<br />
var Today = new Date();&nbsp;<br />
var NowHour = Today.getHours();&nbsp;<br />
var NowMinute = Today.getMinutes();&nbsp;<br />
var NowSecond = Today.getSeconds();&nbsp;<br />
var mysec = (NowHour*3600)+(NowMinute*60)+NowSecond;&nbsp;<br />
if((mysec-mypretime)&gt;10){&nbsp;<br />
//10只是一个时间值，就是10秒内禁止重复提交，值随便设&nbsp;<br />
mypretime=mysec;<br />
}else{<br />
<span style="line-height:1.5;">return;</span><br />
}<br />
document.getElementById('imgform').submit();<br />
}<br />
<p>
	&lt;/script&gt;
</p>
<p>
	<br />
</p>]]></description>
	<pubDate>Wed, 12 Feb 2014 07:32:38 +0000</pubDate>
	<author>admin</author>
	<guid>http://kanng.net/blog/?post=14</guid>

</item>
<item>
	<title>google地图api 通过地址获得经纬度</title>
	<link>http://kanng.net/blog/?post=13</link>
	<description><![CDATA[<p>
	最近要做一个在门店位置经纬度的接口给手机app端调用在地图上显示，由于有两千多个，所以只能用批量标注的方法，这里使用了谷歌地图的api，只要把地址传过去就可以了。下面是代码：
</p>
<p>
	<span style="color:#99BB00;">$address="";</span><span style="color:#99BB00;">&nbsp;</span>
</p>
<p>
	<span style="color:#99BB00;">$url = "http://ditu.google.cn/maps/api/geocode/json?address=$address&amp;sensor=false";//查询接口，谷歌有限制，每天1000条</span>
</p>
<span style="color:#99BB00;">&nbsp;$result = file_get_contents($url);//最好使用curl函数，我这里偷懒了</span><br />
<span style="color:#99BB00;">&nbsp;$result = json_decode($result);//反json</span><br />
<span style="color:#99BB00;">&nbsp;$result = get_object_vars($result);//处理得到的json，找到自己有用的</span><br />
<br />
<span style="color:#99BB00;">&nbsp;$status = $result['status'];</span><br />
<br />
<span style="color:#99BB00;">&nbsp;$addressInfo = get_object_vars($result['results'][0]);</span><br />
<span style="color:#99BB00;">&nbsp;$lat = $addressInfo['geometry']-&gt;location-&gt;lat; //纬度</span><br />
<span style="color:#99BB00;">&nbsp;$lng = $addressInfo['geometry']-&gt;location-&gt;lng; //经度</span><br />
<span style="color:#99BB00;">&nbsp;if ($status != 'OK') { &nbsp;//////////查询不到</span><br />
<span style="color:#99BB00;">&nbsp;echo "限制了"."$status&lt;br&gt;";</span><br />
<span style="color:#99BB00;">&nbsp;continue;</span><br />
<span style="color:#99BB00;">&nbsp;}</span><br />
<span style="color:#99BB00;">&nbsp;echo $address." lng: ".$lng." lat:".$lat."&lt;br&gt;";</span><br />]]></description>
	<pubDate>Tue, 17 Dec 2013 02:32:37 +0000</pubDate>
	<author>admin</author>
	<guid>http://kanng.net/blog/?post=13</guid>

</item>
<item>
	<title>欧元美元日元黄金白银外汇监视小工具，上班族必备</title>
	<link>http://kanng.net/blog/?post=12</link>
	<description><![CDATA[<p>
	免费下载地址：<a href="http://kanng.net/waihui.zip">http://kanng.net/waihui.zip</a> 
</p>
<p>
	建议在win7系统以上运行，如果是xp系统，请下载<span style="color:#505050;font-family:'Microsoft YaHei', Meiryo, 'Malgun Gothic', 'Segoe UI', Tahoma, sans-serif;font-size:13px;line-height:16.1875px;background-color:#F3F3F3;">Microsoft .NET Framework 4</span>&nbsp;&nbsp;<a href="http://www.microsoft.com/zh-cn/download/details.aspx?id=17851">http://www.microsoft.com/zh-cn/download/details.aspx?id=17851</a>
</p>
<img title="外汇监视提醒小工具" alt="外汇监视提醒小工具" src="http://kanng.net/waihui.jpg" /> 
<p>
	说明：
</p>
<p>
	<strong>种类</strong>：货币种类，目前支持<span style="color:#99BB00;">欧美，磅美，日美，加美，澳美，黄金，白银</span>。选择你要提醒的货币。只支持同时监视一种。
</p>
<p>
	<strong>上行下行</strong>：你要设置提醒的最高最低价。
</p>
<p>
	<strong>预警声音</strong>：勾选后，到达预设的价格，会有声音提醒。
</p>
<p>
	<strong>超出预警提示</strong>：不勾选时，一直提示价格变化，不变化不提示。勾选后，只有达到预设价格才会显示。
</p>
<p>
	设置好之后，点击开始，会缩小到任务栏
</p>
<img alt="" src="http://kanng.net/www.jpg" /> 
<p>
	&nbsp;
</p>
<p>
	&nbsp;
</p>
<p>
	软件绿色安全无需安装，上班族监视外汇必备。如需要更多货比,或者有更多建议请在下面留言，我们会尽快满足您的需求。
</p>
<p>
	&nbsp;
</p>
<p>
	&nbsp;
</p>]]></description>
	<pubDate>Tue, 26 Nov 2013 07:15:57 +0000</pubDate>
	<author>admin</author>
	<guid>http://kanng.net/blog/?post=12</guid>

</item>
<item>
	<title>div 始终处于顶部和底部</title>
	<link>http://kanng.net/blog/?post=11</link>
	<description><![CDATA[<p>
	直接上代码：
</p>
<p>
	&nbsp;<span style="color:#99bb00;"> &lt;div class="Wrapper"&gt;</span><br />
<span style="color:#99bb00;">&nbsp;&nbsp;&nbsp; &lt;div class="Header"&gt;tttttttttttttttttttttttttttttt&lt;br&gt;&lt;/div&gt;</span><br />
<span style="color:#99bb00;">&nbsp;&nbsp;&nbsp; &lt;div class="Content"&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;IT导航&lt;br&gt;&lt;/div&gt;</span><br />
<span style="color:#99bb00;">&nbsp;&nbsp;&nbsp; &lt;div class="FooterPush"&gt;&lt;/div&gt;</span><br />
<span style="color:#99bb00;">&nbsp; &lt;/div&gt;</span><br />
<span style="color:#99bb00;">&nbsp; &lt;div class="Footer"&gt;ddddddddddddddddddddddddddddddddddddddddddddd&lt;/div&gt;</span>
</p>
<p>
	<span style="color:#000000;">CSS:</span>
</p>
<p>
	<span style="color:#000000;">&nbsp;
</p>
<p>
	&lt;style type="text/css"&gt;<br />
*{<br />
&nbsp; margin: 0;<br />
&nbsp; padding: 0;<br />
}<br />
html, body, form{<br />
&nbsp; height: 100%;<br />
}<br />
&nbsp;&nbsp; <br />
.Wrapper{<br />
&nbsp; min-height: 100%;<br />
}<br />
&nbsp;&nbsp; <br />
.Wrapper .FooterPush{<br />
&nbsp;&nbsp;&nbsp; height: 120px; /* Footer 的高度 */<br />
&nbsp; }<br />
.Header{<br />
&nbsp;&nbsp;&nbsp; background: #eee;<br />
&nbsp;&nbsp;&nbsp; text-align: center;<br />
&nbsp;&nbsp;&nbsp; top:0px;<br />
&nbsp;&nbsp;&nbsp; position:fixed;<br />
&nbsp;&nbsp;&nbsp; height: 120px;<br />
&nbsp;&nbsp;&nbsp; line-height: 120px;<br />
&nbsp;&nbsp;&nbsp; width: 100%;<br />
&nbsp;&nbsp;&nbsp; _position: absolute; /*兼容IE6*/<br />
&nbsp;&nbsp;&nbsp; _top: expression(offsetParent.scrollTop+document.documentElement.clientHeight-this.offsetHeight); /*兼容IE6*/
</p>
<p>
	}<br />
.Footer{<br />
&nbsp;&nbsp;&nbsp; background: #eee;<br />
&nbsp;&nbsp;&nbsp; text-align: center;<br />
&nbsp;&nbsp;&nbsp; bottom:0px;<br />
&nbsp;&nbsp;&nbsp; position:fixed;<br />
&nbsp;&nbsp;&nbsp; height: 120px;<br />
&nbsp;&nbsp;&nbsp; line-height: 120px;<br />
&nbsp;&nbsp;&nbsp; width: 100%;<br />
&nbsp;&nbsp;&nbsp; _position: absolute; /*兼容IE6*/<br />
&nbsp;&nbsp;&nbsp; _top: expression(offsetParent.scrollTop+document.documentElement.clientHeight-this.offsetHeight); /*兼容IE6*/<br />
}<br />
&lt;/style&gt;
</p>
<p>
	&nbsp;
</p>
<p>
	布局代码和CSS分别借鉴两个地方的，因为他们的都没有达到效果，我整合在一起，就行了。
</p>
</span> 
<p>
	<span style="color:#99bb00;"></span>&nbsp;
</p>
<p>
	&nbsp;
</p>]]></description>
	<pubDate>Fri, 15 Nov 2013 08:05:25 +0000</pubDate>
	<author>admin</author>
	<guid>http://kanng.net/blog/?post=11</guid>

</item></channel>
</rss>