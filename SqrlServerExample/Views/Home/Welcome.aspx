<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<SqrlServerExample.Data.SqrlUser>" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Welcome</title>
</head>
<body>
	<div>
		<h1>Welcome <%: Model.UserName %>!<h1>
	<div>
	<%= Html.ActionLink("Log Off", "LogOff", "Login") %>
</body>
</html>