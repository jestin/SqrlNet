<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<SqrlServerExample.Models.SqrlLoginModel>" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>SQRL Login</title>
	<script type="text/javascript" src="/Content/scripts/jquery/jquery-1.8.3.min.js"></script>
</head>
<body>
	<script type="text/javascript">
		setInterval(function(){
				//alert("test");
				$.get('<%= Url.Action("IsLoggedInYet", "Login") %>/<%= Model.Nut %>', function(data){
					if(data == 'true'){
						window.location.href = '<%= Url.Action("Welcome", "Home") %>';
					}
				});
			}, 2000);
	</script>
	<div>
		<a href="<%= Html.Encode(Model.Url) %>" ><img src="data:image/png;base64,<%= Model.QrCode %>" /></a>
		<h1><%= Html.Encode(Model.Url) %></h1>
	</div>
</body>

