var pgeditor=new jQuery.pge({pgePath:"#",pgeId:"_ocx_password",pgeEdittype:0,pgeEreg1:"",pgeEreg2:"",pgeMaxlength:20,pgeTabindex:2,pgeClass:"text_pge",pgeInstallClass:"text_pge",pgeOnkeydown:"$('#loginsubmit').click();",tabCallback:"autoLogin"});$(function(){pgeditor.pgInitialize(),pgeditor.checkInstall()&&($("#chkOpenCtrl").attr("checked","checked"),$("#nloginpwd").hide(),pgeditor.checkUpdate()&&$("#updata").show(),$("#sloginpwd").show())});