<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebTest._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
		UserName:<input type="text" name="UserName" /><br />
		Password:<input type="text" name="Password" /><br />
		Question:<input type="text" name="Question" /><br />
		Answer:<input type="text" name="Answer" /><br />
		Email:<input type="text" name="Email" /><br />
		<asp:Button ID="btnAdd" Text="Add" runat="server" /><br />
		ID:<input type="text" name="ID" />
		<asp:Button ID="btnUpdate" Text="Update" runat="server" />
		<asp:Button ID="btnDelete" Text="Delete" runat="server" /><br />
		<asp:GridView ID="gvUser" runat="server"></asp:GridView>
    </div>
    </form>
</body>
</html>
