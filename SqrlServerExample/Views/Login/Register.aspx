<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<SqrlServerExample.Data.SqrlUser>" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<div>
		<% using(Html.BeginForm("Register", "Login")) %>
		<% { %>
			<%= Html.LabelFor(m => m.UserName) %>
			<%= Html.EditorFor(m => m.UserName) %>
			<%= Html.HiddenFor(m => m.Id) %>
			<input type="submit" value="Submit" />
		<% } %>
	<div>
</body>
</html>
