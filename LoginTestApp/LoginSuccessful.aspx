﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginSuccessful.aspx.cs" Inherits="LoginTestApp.LoginSuccessful" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Label1" runat="server" Text="Label">ログイン完了</asp:Label>
        </div>
        <br />
        <br />
        <div>
            <asp:Button ID="Button1" runat="server" Text="LINEメッセージ送信" Visible="false" OnClick="Button1_Click"/>
        </div>
    </form>
</body>
</html>
