<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<SqrlServerExample.Models.SqrlLoginModel>" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<div>
		<a href="<%= Html.Encode(Model.Url) %>" ><img src="data:image/png;base64,<%= Model.QrCode %>" /></a>
		<%= Html.Encode(Model.Url) %>
	</div>
</body>

