//代码参数
var trackToutiaoPara = {};
	trackToutiaoPara.trackOrder = false; //订单提交时转化
	trackToutiaoPara.trackPay 	= false; //点击支付转化
	trackToutiaoPara.trackPayed = false; //支付完成后转化
var tracktGdtPara = {};
	tracktGdtPara.trackOrder = false; //订单提交时转化
	tracktGdtPara.trackPay	  = false; //点击支付转化
	tracktGdtPara.trackPayed = false; //支付完成后转化
var tracktKsPara = {};
	tracktKsPara.trackOrder = false; //订单提交时转化
	tracktKsPara.trackPay	  = false; //点击支付转化
	tracktKsPara.trackPayed = false; //支付完成后转化
var trackObj = {
	//头条基础代码参数
	codeToutiao:function(referer) {
		//当前客户端
		trackToutiaoPara.currReferer = referer;
		// ① 账户C https://vip.u0537.com/mtg/index/referer-8D845619F25DCC0BEB661519BA138D8D.html
		if( referer=='8D845619F25DCC0BEB661519BA138D8D' ){
			trackToutiaoPara.scriptId = '1652424302713870';
			trackToutiaoPara.trackId = '1652506321411075';
			trackToutiaoPara.trackOrder= true; //订单提交时转化
		}
		// ② 账户D https://vip.u0537.com/mtg/index/referer-2E2530F6EE7CFF4631EC79D08C0F357F.html
		if( referer == '2E2530F6EE7CFF4631EC79D08C0F357F' ){
			trackToutiaoPara.scriptId = '1652547365133325';
			trackToutiaoPara.trackId = '1652586647056387';
			trackToutiaoPara.trackOrder= true; //订单提交时转化
		}
		// ④ 抖音靓号X https://vip.u0537.com/mtg/index/referer-E0757E89AE95376C5D42381E7A66AB47.html
		if( referer == 'E0757E89AE95376C5D42381E7A66AB47' ){
			trackToutiaoPara.scriptId = '1652324044091400';
			trackToutiaoPara.trackId = '1657497526747148';
			trackToutiaoPara.trackPayed = true; //支付完成后转化
		}
		// ⑤ 靓号F https://vip.u0537.com/mtg/index/referer-EA6EAD4AEE868272791C571CB184A937.html
		if( referer == 'EA6EAD4AEE868272791C571CB184A937' ){
			trackToutiaoPara.scriptId = '1652590177233928';
			trackToutiaoPara.trackId = '1653314942520334';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		// ⑦ 靓号I https://vip.u0537.com/mtg/index/referer-4DCEBBE3946E4ACAB37E2946B885DD11.html
		if( referer == '4DCEBBE3946E4ACAB37E2946B885DD11' ){
			trackToutiaoPara.scriptId = '1654238889184270';
			trackToutiaoPara.trackId = '1654313602433027';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		// ⑧ 靓号K https://vip.u0537.com/mtg/index/referer-7400AEFFD7FF0BB016A00090B7C253AA.html
		if( referer == '7400AEFFD7FF0BB016A00090B7C253AA' ){
			trackToutiaoPara.scriptId = '1654600554100750';
			trackToutiaoPara.trackId = '1654669560341511';
			trackToutiaoPara.trackPayed = true; //支付完成后转化
		}
		// ⑨ 靓号J https://vip.u0537.com/mtg/index/referer-3C6C4DAD8E0D5AF8287188A8DBD5C57D.html
		if( referer == '3C6C4DAD8E0D5AF8287188A8DBD5C57D' ){
			trackToutiaoPara.scriptId = '1654577528240135';
			trackToutiaoPara.trackId = '1654669629085710';
			trackToutiaoPara.trackPayed = true; //支付完成后转化
		}
		// ⑩ 模式C1 https://vip.u0537.com/mpc/index/referer-153F6BF4569397ADE4D4DA93218154A4.html
		if( referer == '153F6BF4569397ADE4D4DA93218154A4' ){
			trackToutiaoPara.scriptId = '1652547365561351';
			trackToutiaoPara.trackId = '1655416173211662';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		// ①① 模式B1 https://vip.u0537.com/mpb/?referer=08770AFE5B79F2058478B10CBA30190C
		if( referer == '08770AFE5B79F2058478B10CBA30190C' ){
			trackToutiaoPara.scriptId = '1652590239203342';
			trackToutiaoPara.trackId = '1655416084613133';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		//12 https://vip.u0537.com/mpa/index/referer-A817B7FD3BDDE878FA353CE64BECAD78.html（A1）
		if( referer == 'A817B7FD3BDDE878FA353CE64BECAD78' ){
			trackToutiaoPara.scriptId = '1655521361663052';
			trackToutiaoPara.trackId = '1655756321582084';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		//13 https://vip.u0537.com/mpa/index/referer-A21B84784AB51199C902B0F30ED70557.html （A2）
		if( referer == 'A21B84784AB51199C902B0F30ED70557' ){
			trackToutiaoPara.scriptId = '1655575427560455';
			trackToutiaoPara.trackId = '1655756422497288';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		//14 https://vip.u0537.com/mpb/?referer=5D6CBB736D027EE1E55922F3B6FDEB81 （B2）
		if( referer == '5D6CBB736D027EE1E55922F3B6FDEB81' ){
			trackToutiaoPara.scriptId = '1655520452559880';
			trackToutiaoPara.trackId = '1655755312842764';
			trackToutiaoPara.trackPayed = true; //支付完成后转化
		}
		//15 https://vip.u0537.com/mpb/index/referer-2A6C6C70E825098C677818B63997E305.html  （B3）
		if( referer == '2A6C6C70E825098C677818B63997E305' ){
			trackToutiaoPara.scriptId = '1655522134413320';
			trackToutiaoPara.trackId = '1655755447219207';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		//16 https://vip.u0537.com/mpb/?referer=DE0A505F59B6C6E11A30C688CCFE2970  （B4）
		if( referer == 'DE0A505F59B6C6E11A30C688CCFE2970' ){
			trackToutiaoPara.scriptId = '1655522133919758';
			trackToutiaoPara.trackId = '1655755552985096';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		//17 https://vip.u0537.com/mpb/index/referer-06AE5A5F4C2E3DCB7640E0A31998E3CC.html （B5）
		if( referer == '06AE5A5F4C2E3DCB7640E0A31998E3CC' ){
			trackToutiaoPara.scriptId = '1655523126660103';
			trackToutiaoPara.trackId = '1655755865067524';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		//18 https://vip.u0537.com/mpc/index/referer-F688E7F64947F3C25639356CD7D974A8.html （C2）
		if( referer == 'F688E7F64947F3C25639356CD7D974A8' ){
			trackToutiaoPara.scriptId = '1655523127080964';
			trackToutiaoPara.trackId = '1655755941395463';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		//19 https://vip.u0537.com/mpc/index/referer-170092AF27FFAFF0B60BE9B81CF2A989.html （C3）
		if( referer == '170092AF27FFAFF0B60BE9B81CF2A989' ){
			trackToutiaoPara.scriptId = '1655152832477197';
			trackToutiaoPara.trackId = '1655756042833924';
			trackToutiaoPara.trackPayed = true; //支付完成后转化
		}
		//20 https://vip.u0537.com/mpc/index/referer-3CB3ABEBDE5A24E2F0C46195A89F68A5.html  （C4）
		if( referer == '3CB3ABEBDE5A24E2F0C46195A89F68A5' ){
			trackToutiaoPara.scriptId = '1655523580762126';
			trackToutiaoPara.trackId = '1655756129362956';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		//21 https://vip.u0537.com/mpc/?referer=97BB3E181D64FF5150746F22C4884B84 （C5）
		if( referer == '97BB3E181D64FF5150746F22C4884B84' ){
			trackToutiaoPara.scriptId = '1655523581150215';
			trackToutiaoPara.trackId = '1655756241186829';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		//22 账户d1 https://vip.u0537.com/mpd/?referer=3B0CF70D46D0CBFDCDB41024A6C5DF46
		if( referer == '3B0CF70D46D0CBFDCDB41024A6C5DF46' ){
			trackToutiaoPara.scriptId = '1655782779393032';
			trackToutiaoPara.trackId = '1655966792756251';
			trackToutiaoPara.trackPayed = true; //支付完成后转化
		}
		//23 账户e1 https://vip.u0537.com/mpe/?referer=FA3BEB5A0290A4880D374C5D0C4D18F1
		if( referer == 'FA3BEB5A0290A4880D374C5D0C4D18F1' ){
			trackToutiaoPara.scriptId = '1655855894983687';
			trackToutiaoPara.trackId = '1655966996152323';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		//24 账户f1 https://vip.u0537.com/mpf/?referer=5E48DBED3DDF26A1DDD654C1B3E4BAB9
		if( referer == '5E48DBED3DDF26A1DDD654C1B3E4BAB9' ){
			trackToutiaoPara.scriptId = '1655878249721870';
			trackToutiaoPara.trackId = '1655967172609035';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		//25 抖音靓号g1 https://vip.u0537.com/mpg/?referer=2523E70E538390D36D1ED6320EF844DD
		if( referer == '2523E70E538390D36D1ED6320EF844DD' ){
			trackToutiaoPara.scriptId = '1655961421611016';
			trackToutiaoPara.trackId = '1655967326151683';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		//26 靓号h1 https://vip.u0537.com/mph/?referer=C30CCB77EDADCED49AFB06D47E0A2115
		if( referer == 'C30CCB77EDADCED49AFB06D47E0A2115' ){
			trackToutiaoPara.scriptId = '1655961123108877';
			trackToutiaoPara.trackId = '1655967459958792';
			trackToutiaoPara.trackPayed = true; //支付完成后转化
		}
		//27 https://vip.u0537.com/mpd/?referer=8546CDD7D9D4BB4D63B4AF9606E4422A （附件D2）
		if( referer == '8546CDD7D9D4BB4D63B4AF9606E4422A' ){
			trackToutiaoPara.scriptId = '1655522134413320';
			trackToutiaoPara.trackId = '1656866021620747';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		//28 https://vip.u0537.com/mpd/?referer=940F540707E102C410DB00756EB29CAC（附件D3）
		if( referer == '940F540707E102C410DB00756EB29CAC' ){
			trackToutiaoPara.scriptId = '1655522133919758';
			trackToutiaoPara.trackId = '1656866162027531';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		//29 https://vip.u0537.com/mpe/?referer=F601284516D22FE7480DB73A06172D2B （附件E2）
		if( referer == 'F601284516D22FE7480DB73A06172D2B' ){
			trackToutiaoPara.scriptId = '1655523126660103';
			trackToutiaoPara.trackId = '1656866307150860';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		//30 https://vip.u0537.com/mpe/?referer=DA1786E2822826ED1283FB9919258B46 （附件E3）
		if( referer == 'DA1786E2822826ED1283FB9919258B46' ){
			trackToutiaoPara.scriptId = '1655523127080964';
			trackToutiaoPara.trackId = '1656866444923916';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		//31 https://vip.u0537.com/mpf/?referer=7D28ABD05E1A771D87E50F53989B6261 （附件F2）
		if( referer == '7D28ABD05E1A771D87E50F53989B6261' ){
			trackToutiaoPara.scriptId = '1655523580762126';
			trackToutiaoPara.trackId = '1656866598121479';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		//32 https://vip.u0537.com/mpf/?referer=8ACF26297B70834A89FBA91ECA609AAA （附件F3）
		if( referer == '8ACF26297B70834A89FBA91ECA609AAA' ){
			trackToutiaoPara.scriptId = '1655523581150215';
			trackToutiaoPara.trackId = '1656866735520781';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		//33 https://vip.u0537.com/mpg/?referer=92FE530152348C9739838467C66AF690 （附件G2）
		if( referer == '92FE530152348C9739838467C66AF690' ){
			trackToutiaoPara.scriptId = '1655521361663052';
			trackToutiaoPara.trackId = '1656866841517067';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		//34 https://vip.u0537.com/mpg/?referer=3A36851D8CFCA1D81800FA7B66882650 （附件G3）
		if( referer == '3A36851D8CFCA1D81800FA7B66882650' ){
			trackToutiaoPara.scriptId = '1655575427560455';
			trackToutiaoPara.trackId = '1656866970750990';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		//35 https://vip.u0537.com/mpd/?referer=4CBE86D5E75CA21C57D9D95F134C7848 （D5）
		if( referer == '4CBE86D5E75CA21C57D9D95F134C7848' ){
			trackToutiaoPara.scriptId = '1657122094194696';
			trackToutiaoPara.trackId = '1657167258248196';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		//36 https://vip.u0537.com/mpd/?referer=15C114103AD5AEECD07313BFA3937463 （D6）
		if( referer == '15C114103AD5AEECD07313BFA3937463' ){
			trackToutiaoPara.scriptId = '1657122094631949';
			trackToutiaoPara.trackId = '1657167512794120';
			trackToutiaoPara.trackPayed = true; //支付完成后转化
		}
		//https://vip.u0537.com/mpd/?referer=7F8971A0357574ECAE6D2AE91D764825 （D7）
		if( referer == '7F8971A0357574ECAE6D2AE91D764825' ){
			trackToutiaoPara.scriptId = '1657122095052808';
			trackToutiaoPara.trackId = '1657167660476419';
			trackToutiaoPara.trackPayed = true; //支付完成后转化
		}
		//37 https://vip.u0537.com/mpd/?referer=6C40330560420A303E04BDA646E71915 （D8）
		if( referer == '6C40330560420A303E04BDA646E71915' ){
			trackToutiaoPara.scriptId = '1657122095501320';
			trackToutiaoPara.trackId = '1657167757778947';
			trackToutiaoPara.trackPayed = true; //支付完成后转化
		}
		//38 https://vip.u0537.com/mpd/?referer=1D8E2C86462094FE8BDE987A68B783D1 （D9）
		if( referer == '1D8E2C86462094FE8BDE987A68B783D1' ){
			trackToutiaoPara.scriptId = '1657122095997959';
			trackToutiaoPara.trackId = '1657167898582023';
			trackToutiaoPara.trackPayed = true; //支付完成后转化
		}
		//https://vip.u0537.com/mpd/?referer=ED787DA5244428E72656CFC2451EDEE5 (D10)
		if( referer == 'ED787DA5244428E72656CFC2451EDEE5' ){
			trackToutiaoPara.scriptId = '1657122366152711';
			trackToutiaoPara.trackId = '1657167975460877';
			trackToutiaoPara.trackPayed = true; //支付完成后转化
		}
		//39 https://vip.u0537.com/mpd/?referer=6AB5A5C623D9A39B44B9749515941E7E （D11）
		if( referer == '6AB5A5C623D9A39B44B9749515941E7E' ){
			trackToutiaoPara.scriptId = '1657122366599182';
			trackToutiaoPara.trackId = '1657168057621518';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		//40 https://vip.u0537.com/mpd/?referer=E2D31809B7745D3206CFFDC010E59B33 （D12）
		if( referer == 'E2D31809B7745D3206CFFDC010E59B33' ){
			trackToutiaoPara.scriptId = '1657122367007748';
			trackToutiaoPara.trackId = '1657168263599108';
			trackToutiaoPara.trackPayed = true; //支付完成后转化
		}
		//41 https://vip.u0537.com/mpd/?referer=8829CE22E3AF080ACD92D0F99C18E0F4 （D13）
		if( referer == '8829CE22E3AF080ACD92D0F99C18E0F4' ){
			trackToutiaoPara.scriptId = '1657122367355918';
			trackToutiaoPara.trackId = '1657168360803335';
			trackToutiaoPara.trackPayed = true; //支付完成后转化
		}
		//42 https://vip.u0537.com/mpd/?referer=72B06DA79D679A62E31651DEADB5096B （D14）
		if( referer == '72B06DA79D679A62E31651DEADB5096B' ){
			trackToutiaoPara.scriptId = '1657122367891464';
			trackToutiaoPara.trackId = '1657168435427340';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		//43 https://vip.u0537.com/mpe/?referer=FBB639F0A369E9A823C504A84C6BBE59 （E5）
		if( referer == 'FBB639F0A369E9A823C504A84C6BBE59' ){
			trackToutiaoPara.scriptId = '1657229498103815';
			trackToutiaoPara.trackId = '1657251731923979';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		//44 https://vip.u0537.com/mpe/?referer=FA4BEFD330628ECAAF7B24EA4035CDFD （E6）
		if( referer == 'FA4BEFD330628ECAAF7B24EA4035CDFD' ){
			trackToutiaoPara.scriptId = '1657233427739652';
			trackToutiaoPara.trackId = '1657251881301003';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		//45 https://vip.u0537.com/mpe/?referer=0213AA00E54B72EF335F6E0B83E8515F （E7）
		if( referer == '0213AA00E54B72EF335F6E0B83E8515F' ){
			trackToutiaoPara.scriptId = '1655521362625550';
			trackToutiaoPara.trackId = '1657252048529420';
			trackToutiaoPara.trackPayed = true; //支付完成后转化
		}
		//46 https://vip.u0537.com/mpe/?referer=081E144EA52EF2C59338FADA86494CA9 （E8）
		if( referer == '081E144EA52EF2C59338FADA86494CA9' ){
			trackToutiaoPara.scriptId = '1655521362152461';
			trackToutiaoPara.trackId = '1657252193208328';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		//47 https://vip.u0537.com/mpe/?referer=F0AA685B6D5C6D1523353181297D6DD3 （E9）
		if( referer == 'F0AA685B6D5C6D1523353181297D6DD3' ){
			trackToutiaoPara.scriptId = '1655521363425287';
			trackToutiaoPara.trackId = '1657252284054540';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		//48 https://vip.u0537.com/mpe/?referer=EFC6BF7B49E7299605CA7C97B6C28415 （E10）
		if( referer == 'EFC6BF7B49E7299605CA7C97B6C28415' ){
			trackToutiaoPara.scriptId = '1655521363057678';
			trackToutiaoPara.trackId = '1657252539867147';
			trackToutiaoPara.trackPayed = true; //支付完成后转化
		}
		//49 https://vip.u0537.com/mpe/?referer=7CD32FFA3D0E2D3DC180240B072A845A （E11）
		if( referer == '7CD32FFA3D0E2D3DC180240B072A845A' ){
			trackToutiaoPara.scriptId = '1657229159715843';
			trackToutiaoPara.trackId = '1657252612091908';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		//50 https://vip.u0537.com/mpe/?referer=232BC38362384CB940061F77581A9068 （E12）
		if( referer == '232BC38362384CB940061F77581A9068' ){
			trackToutiaoPara.scriptId = '1657229160104968';
			trackToutiaoPara.trackId = '1657252714597387';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		//51 https://vip.u0537.com/mpe/?referer=3B79B449686B1FE5D943251E6FC75393 （E13）
		if( referer == '3B79B449686B1FE5D943251E6FC75393' ){
			trackToutiaoPara.scriptId = '1657228851812365';
			trackToutiaoPara.trackId = '1657252823929870';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		//52 https://vip.u0537.com/mpe/?referer=9E52A105414C487B6DC6AF64023B2073 （E14）
		if( referer == '9E52A105414C487B6DC6AF64023B2073' ){
			trackToutiaoPara.scriptId = '1657228852249613';
			trackToutiaoPara.trackId = '1657252999310348';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		//53 https://vip.u0537.com/mtg/index/referer-2E2530F6EE7CFF4631EC79D08C0F357F.html（D下单成功）
		if( referer == '2E2530F6EE7CFF4631EC79D08C0F357F' ){
			trackToutiaoPara.scriptId = '1652547365133325';
			trackToutiaoPara.trackId = '1657527605874691';
			trackToutiaoPara.trackPayed = true; //支付完成后转化
		}
		//54 https://vip.u0537.com/mpc/index/referer-153F6BF4569397ADE4D4DA93218154A4.html（E下单成功）
		if( referer == '153F6BF4569397ADE4D4DA93218154A4' ){
			trackToutiaoPara.scriptId = '1652547365561351';
			trackToutiaoPara.trackId = '1657527505375243';
			trackToutiaoPara.trackPayed = true; //支付完成后转化
		}
		//55 https://vip.u0537.com/mpb/?referer=08770AFE5B79F2058478B10CBA30190C（H下单成功）
		if( referer == '08770AFE5B79F2058478B10CBA30190C' ){
			trackToutiaoPara.scriptId = '1652590239203342';
			trackToutiaoPara.trackId = '1657527244709899';
			trackToutiaoPara.trackPayed = true; //支付完成后转化
		}
		//56 https://vip.u0537.com/mtg/?referer=473C5424CF5EDFF0889FA47383C168D2（TG1下单成功）
		if( referer == '473C5424CF5EDFF0889FA47383C168D2' ){
			trackToutiaoPara.scriptId = '1652590239203342';
			trackToutiaoPara.trackId = '1657799556953099';
			trackToutiaoPara.trackPayed = true; //支付完成后转化
		}
		//57 https://vip.u0537.com/mpf/?referer=FC98518A1113890F942ED6D75A7E9D70 （F5）
		if( referer == 'FC98518A1113890F942ED6D75A7E9D70' ){
			trackToutiaoPara.scriptId = '1658581023044616';
			trackToutiaoPara.trackId = '1658950587947022';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		//58 https://vip.u0537.com/mpf/?referer=CF1CEE238B6BD0D610D00CDCAC5CB3FA （F6）
		if( referer == 'CF1CEE238B6BD0D610D00CDCAC5CB3FA' ){
			trackToutiaoPara.scriptId = '1658581023420445';
			trackToutiaoPara.trackId = '1658950930932747';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		//59 https://vip.u0537.com/mpf/?referer=E0CB78637C81C70E838837A70780BCE8 （ F7）
		if( referer == 'E0CB78637C81C70E838837A70780BCE8' ){
			trackToutiaoPara.scriptId = '1658581023848461';
			trackToutiaoPara.trackId = '1658951042075652';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		//60 https://vip.u0537.com/mpf/?referer=9E2CA185166FD8C895F310F9F30D038D （F8）
		if( referer == '9E2CA185166FD8C895F310F9F30D038D' ){
			trackToutiaoPara.scriptId = '1658581247642637';
			trackToutiaoPara.trackId = '1658951173756932';
			trackToutiaoPara.trackPayed = true; //支付完成后转化
		}
		//61 https://vip.u0537.com/mpf/?referer=B81EC6B0EEBBB136ABC6C3AD9D110D7F （F9）
		if( referer == 'B81EC6B0EEBBB136ABC6C3AD9D110D7F' ){
			trackToutiaoPara.scriptId = '1658581313618958';
			trackToutiaoPara.trackId = '1658951263955980';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		//62 https://vip.u0537.com/mpf/?referer=46B5A4F9118AB129956DB9FFCBFA0A03 （F10）
		if( referer == '46B5A4F9118AB129956DB9FFCBFA0A03' ){
			trackToutiaoPara.scriptId = '1658581314005000';
			trackToutiaoPara.trackId = '1658951396612100';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		//63 https://vip.u0537.com/mpf/?referer=13450EAE408F506347063E136417F969 （F11）
		if( referer == '13450EAE408F506347063E136417F969' ){
			trackToutiaoPara.scriptId = '1658588174505998';
			trackToutiaoPara.trackId = '1658951521014791';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		//64 https://vip.u0537.com/mpf/?referer=B25B64F062597796855AB0F369063720 （F12）
		if( referer == 'B25B64F062597796855AB0F369063720' ){
			trackToutiaoPara.scriptId = '1658588174842894';
			trackToutiaoPara.trackId = '1658951634671624';
			trackToutiaoPara.trackOrder = true; //订单提交时转化
		}
		//65 https://vip.u0537.com/mpf/?referer=0D2A9C7182ED7BCAFF025F33451BF058 （F13）
		if( referer == '0D2A9C7182ED7BCAFF025F33451BF058' ){
			trackToutiaoPara.scriptId = '1658590429987848';
			trackToutiaoPara.trackId = '1658951763913732';
			trackToutiaoPara.trackPayed = true; //支付完成后转化
		}
		//66 https://vip.u0537.com/mpf/?referer=48C569C5553E1CDB543DD36E3B776177 （F14）
		if( referer == '48C569C5553E1CDB543DD36E3B776177' ){
			trackToutiaoPara.scriptId = '1658590437898247';
			trackToutiaoPara.trackId = '1658951873502228';
			trackToutiaoPara.trackPayed = true; //支付完成后转化
		}
		//77 https://vip.u0537.com/mpa/?referer=8Y0kJV
		if( referer == '8Y0kJV' ){
			trackToutiaoPara.scriptId = '1659572947732552';
			trackToutiaoPara.trackId = '1660108781679620';
			trackToutiaoPara.trackPay = true; //点击支付转化
		}
		//https://vip.u0537.com/mpa/?referer=dE6VZ0 a6
		if( referer == 'dE6VZ0' ){
			trackToutiaoPara.scriptId = '1659572948238349';
			trackToutiaoPara.trackId = '1660220779716619';
			trackToutiaoPara.trackPay = true; //点击支付转化
		}
		//https://vip.u0537.com/mpa/?referer=dl43N5 a7
		if( referer == 'dl43N5' ){
			trackToutiaoPara.scriptId = '1659572948695054';
			trackToutiaoPara.trackId = '1660220932421636';
			trackToutiaoPara.trackPay = true; //点击支付转化
		}
		//https://vip.u0537.com/mpa/?referer=8J0Plj a8
		if( referer == '8J0Plj' ){
			trackToutiaoPara.scriptId = '1659572949084167';
			trackToutiaoPara.trackId = '1660221473798158';
			trackToutiaoPara.trackPay = true; //点击支付转化
		}
		//https://vip.u0537.com/mpa/?referer=vKXel2 a9
		if( referer == 'vKXel2' ){
			trackToutiaoPara.scriptId = '1659572949537806';
			trackToutiaoPara.trackId = '1660221536894030';
			trackToutiaoPara.trackPay = true; //点击支付转化
		}
		//https://vip.u0537.com/mpa/?referer=8XRZX4 a10
		if( referer == '8XRZX4' ){
			trackToutiaoPara.scriptId = '1659572949958663';
			trackToutiaoPara.trackId = '1660221623364619';
			trackToutiaoPara.trackPay = true; //点击支付转化
		}
		//https://vip.u0537.com/mpa/?referer=vA0EoR a11
		if( referer == 'vA0EoR' ){
			trackToutiaoPara.scriptId = '1659572950471758';
			trackToutiaoPara.trackId = '1660223754345485';
			trackToutiaoPara.trackPay = true; //点击支付转化
		}
		//https://vip.u0537.com/mpa/?referer=8VeKln a12
		if( referer == '8VeKln' ){
			trackToutiaoPara.scriptId = '1659572950883342';
			trackToutiaoPara.trackId = '1660223835021324';
			trackToutiaoPara.trackPay = true; //点击支付转化
		}
		//https://vip.u0537.com/mpa/?referer=dj3jOk a13
		if( referer == 'dj3jOk' ){
			trackToutiaoPara.scriptId = '1659572951266318';
			trackToutiaoPara.trackId = '1660223913575427';
			trackToutiaoPara.trackPay = true; //点击支付转化
		}
		//https://vip.u0537.com/mpa/?referer=8x51xP a14
		if( referer == '8x51xP' ){
			trackToutiaoPara.scriptId = '1659572951672846';
			trackToutiaoPara.trackId = '1660223975820302';
			trackToutiaoPara.trackPay = true; //点击支付转化
		}
		//https://vip.u0537.com/mpd/?referer=QD47xB
		if( referer == 'QD47xB' ){
			trackToutiaoPara.scriptId = '1660111565536270';
			trackToutiaoPara.trackId = '1660497955915787';
			trackToutiaoPara.trackPay = true; //点击支付转化
		}
		//https://vip.u0537.com/mpd/?referer=vKX7zD d15
		if( referer == 'vKX7zD' ){
			trackToutiaoPara.scriptId = '1659473580436487';
			trackToutiaoPara.trackId = '1660687244945420';
			trackToutiaoPara.trackPay = true; //点击支付转化
		}
		//https://vip.u0537.com/mpd/?referer=8XRoV3 d16
		if( referer == '8XRoV3' ){
			trackToutiaoPara.scriptId = '1659473580825607';
			trackToutiaoPara.trackId = '1660687432062988';
			trackToutiaoPara.trackPay = true; //点击支付转化
		}
		//https://vip.u0537.com/mpd/?referer=vA0YMM d17
		if( referer == 'vA0YMM' ){
			trackToutiaoPara.scriptId = '1659473581153287';
			trackToutiaoPara.trackId = '1660687506427917';
			trackToutiaoPara.trackPay = true; //点击支付转化
		}
		//https://vip.u0537.com/mpd/?referer=8Ve2JW d18
		if( referer == '8Ve2JW' ){
			trackToutiaoPara.scriptId = '1659473460690957';
			trackToutiaoPara.trackId = '1660688170535950';
			trackToutiaoPara.trackPay = true; //点击支付转化
		}
		//https://vip.u0537.com/mpd/?referer=dj3zBP d19
		if( referer == 'dj3zBP' ){
			trackToutiaoPara.scriptId = '1659473460154382';
			trackToutiaoPara.trackId = '1660690783209476';
			trackToutiaoPara.trackPay = true; //点击支付转化
		}
		//https://vip.u0537.com/mpd/?referer=8x5YZB d20
		if( referer == '8x5YZB' ){
			trackToutiaoPara.scriptId = '1659386393302024';
			trackToutiaoPara.trackId = '1660690983446531';
			trackToutiaoPara.trackPay = true; //点击支付转化
		}
		//https://vip.u0537.com/mpd/?referer=QWp5zo d22
		if( referer == 'QWp5zo' ){
			trackToutiaoPara.scriptId = '1659386392892429';
			trackToutiaoPara.trackId = '1660691062158343';
			trackToutiaoPara.trackPay = true; //点击支付转化
		}
		//https://vip.u0537.com/mpd/?referer=QbjP1N d23
		if( referer == 'QbjP1N' ){
			trackToutiaoPara.scriptId = '1659386392537096';
			trackToutiaoPara.trackId = '1660691109433355';
			trackToutiaoPara.trackPay = true; //点击支付转化
		}
		//https://vip.u0537.com/mpd/?referer=dw51j2 d24
		if( referer == 'dw51j2' ){
			trackToutiaoPara.scriptId = '1659381432867853';
			trackToutiaoPara.trackId = '1660691238583303';
			trackToutiaoPara.trackPay = true; //点击支付转化
		}
		//https://vip.u0537.com/mpd/?referer=8eMGb1 d25
		if( referer == '8eMGb1' ){
			trackToutiaoPara.scriptId = '1659381432488968';
			trackToutiaoPara.trackId = '1660691297174531';
			trackToutiaoPara.trackPay = true; //点击支付转化
		}
		//https://vip.u0537.com/mpd/?referer=QWp6po
		if( referer == 'QWp6po' ){
			trackToutiaoPara.scriptId = '1660773666347086';
			trackToutiaoPara.trackId = '1661045586149388';
			trackToutiaoPara.trackPay = true; //点击支付转化
		}
		//https://ju.gzjxwlkj.cn/mpd/?referer=8Y09N6
		if( referer == '8Y09N6' ){
			trackToutiaoPara.scriptId = '1661317768223752';
			trackToutiaoPara.trackId = '1661666272692237';
			trackToutiaoPara.trackPay = true; //点击支付转化
		}
		//https://ju.gzjxwlkj.cn/mpd/?referer=dw5Yep
		if( referer == 'dw5Yep' ){
			trackToutiaoPara.scriptId = '1661317767719949';
			trackToutiaoPara.trackId = '1662299670713358';
			trackToutiaoPara.trackPay = true; //点击支付转化
		}
		//https://vip.u0537.com/mpg/?referer=862Njg
		if( referer == '862Njg' ){
			trackToutiaoPara.scriptId = '1661556496384008';
			trackToutiaoPara.trackId = '1661739446007820';
			trackToutiaoPara.trackPay = true; //点击支付转化
		}
		//https://vip.u0537.com/mpg/?referer=8M2LE0
		if( referer == '8M2LE0' ){
			trackToutiaoPara.scriptId = '1661556496796680';
			trackToutiaoPara.trackId = '1661739697278989';
			trackToutiaoPara.trackPay = true; //点击支付转化
		}
		//https://vip.u0537.com/mpg/?referer=vNVL7P
		if( referer == 'vNVL7P' ){
			trackToutiaoPara.scriptId = '1661556497209357';
			trackToutiaoPara.trackId = '1661739772717064';
			trackToutiaoPara.trackPay = true; //点击支付转化
		}
		//https://vip.u0537.com/mpg/?referer=83pZon
		if( referer == '83pZon' ){
			trackToutiaoPara.scriptId = '1661556497612813';
			trackToutiaoPara.trackId = '1661739847120899';
			trackToutiaoPara.trackPay = true; //点击支付转化
		}
		//https://vip.u0537.com/mpg/?referer=8aKn2o
		if( referer == '8aKn2o' ){
			trackToutiaoPara.scriptId = '1661556497983501';
			trackToutiaoPara.trackId = '1661739970864131';
			trackToutiaoPara.trackPay = true; //点击支付转化
		}
		//https://vip.u0537.com/mpg/?referer=QoooNm
		if( referer == 'QoooNm' ){
			trackToutiaoPara.scriptId = '1661919427161102';
			trackToutiaoPara.trackId = '1661920318984204';
			trackToutiaoPara.trackPay = true; //点击支付转化
		}
		//https://vip.u0537.com/mpg/?referer=Q522px
		if( referer == 'Q522px' ){
			trackToutiaoPara.scriptId = '1661919427575816';
			trackToutiaoPara.trackId = '1661920457217028';
			trackToutiaoPara.trackPay = true; //点击支付转化
		}
		//https://vip.u0537.com/mpg/?referer=vZ55rL
		if( referer == 'vZ55rL' ){
			trackToutiaoPara.scriptId = '1661919427956743';
			trackToutiaoPara.trackId = '1661920521742340';
			trackToutiaoPara.trackPay = true; //点击支付转化
		}
		//https://vip.u0537.com/mpg/?referer=d0bbga
		if( referer == 'd0bbga' ){
			trackToutiaoPara.scriptId = '1661919428334606';
			trackToutiaoPara.trackId = '1661920586300427';
			trackToutiaoPara.trackPay = true; //点击支付转化
		}
		//https://vip.u0537.com/mpg/?referer=8622e3
		if( referer == '8622e3' ){
			trackToutiaoPara.scriptId = '1661919428726798';
			trackToutiaoPara.trackId = '1661920666450951';
			trackToutiaoPara.trackPay = true; //点击支付转化
		}

		//https://vip.u0537.com/mpg/?referer=862Mzk
		if( referer == '862Mzk' ){
			trackToutiaoPara.scriptId = '1662281160285191';
			trackToutiaoPara.trackId = '1662287433330695';
			trackToutiaoPara.trackPay = true; //点击支付转化
		}
		//https://vip.u0537.com/mpg/?referer=vNVaqY
		if( referer == 'vNVaqY' ){
			trackToutiaoPara.scriptId = '1662281160632328';
			trackToutiaoPara.trackId = '1662287391303687';
			trackToutiaoPara.trackPay = true; //点击支付转化
		}
		//https://vip.u0537.com/mpg/?referer=83p0zm
		if( referer == '83p0zm' ){
			trackToutiaoPara.scriptId = '1662281160989703';
			trackToutiaoPara.trackId = '1662287281696782';
			trackToutiaoPara.trackPay = true; //点击支付转化
		}
		//https://vip.u0537.com/mpg/?referer=82e0zo
		if( referer == '82e0zo' ){
			trackToutiaoPara.scriptId = '1662281161496583';
			trackToutiaoPara.trackId = '1662287218954252';
			trackToutiaoPara.trackPay = true; //点击支付转化
		}
		//https://vip.u0537.com/mpg/?referer=8aKJYr
		if( referer == '8aKJYr' ){
			trackToutiaoPara.scriptId = '1662281161894926';
			trackToutiaoPara.trackId = '1662287059417092';
			trackToutiaoPara.trackPay = true; //点击支付转化
		}
		
		if( typeof(trackToutiaoPara.scriptId) != 'undefined' ){
			console.log(trackToutiaoPara);
		}
		return trackToutiaoPara;
	},
	//头条转化点击
	trackToutiao:function(trackId) {
		meteor.track('shopping', {convert_id: trackId });
		console.log('trackToutiao ok:'+trackId);
	},
	//广点通基础代码参数
	codeGdt:function(referer) {
		//当前客户端
		tracktGdtPara.currReferer = referer;
		
		//https://vip.u0537.com/mph/
		tracktGdtPara.init = '1110259670';
		tracktGdtPara.trackPay = true; //点击支付转化
		//https://vip.u0537.com/mpe/?referer=83pp5j
		if( referer == '83pp5j' ){
			tracktGdtPara.init = '1110284173';
			tracktGdtPara.trackPay = true; //点击支付转化
		}
		//https://vip.u0537.com/mpe/?referer=82eem6
		if( referer == '82eem6' ){
			tracktGdtPara.init = '1110284173';
			tracktGdtPara.trackPay = true; //点击支付转化
		}
		//https://vip.u0537.com/mpe/?referer=8aKKka
		if( referer == '8aKKka' ){
			tracktGdtPara.init = '1110284173';
			tracktGdtPara.trackPay = true; //点击支付转化
		}
		
		if( typeof(tracktGdtPara.init) != 'undefined' ){
			console.log(tracktGdtPara);
		}
		return tracktGdtPara;
	},
	//广点通转化点击
	trackGdt:function(init) {
		gdt('track', 'COMPLETE_ORDER', {'init':tracktGdtPara.init});
		console.log('trackGdt ok:'+init);
	},
	//快手基础代码参数
	codeKs:function(referer) {
		//当前客户端
		tracktKsPara.currReferer = referer;
		//https://vip.u0537.com/mpd/?referer=72B06DA79D679A62E31651DEADB5096B
		if( referer == '72B06DA79D679A62E31651DEADB5096B' ){
			tracktKsPara.trackId = '17689';
			tracktKsPara.trackPay = true; //点击支付转化
		}
		//https://vip.u0537.com/mpe/?referer=8x5Nm2 （99元 ks6）
		if( referer == '8x5Nm2' ){
			tracktKsPara.trackId = '19307';
			tracktKsPara.trackPay = true; //点击支付转化
		}
		//https://vip.u0537.com/mpe/?referer=8Y093w （99 ks8）
		if( referer == '8Y093w' ){
			tracktKsPara.trackId = '19308';
			tracktKsPara.trackPay = true; //点击支付转化
		}
		if( typeof(tracktKsPara.trackId) != 'undefined' ){
			console.log(tracktKsPara);
		}
		return tracktKsPara;
	},
	//快手转化点击
	trackKs:function(convertId) {
		_ks_trace.push({event: 'form', convertId: convertId, cb: function(){
			console.log('trackKs ok:'+convertId);
		}})
	},
};