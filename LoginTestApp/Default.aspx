<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="LoginTestApp._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <asp:Button ID="Button1" runat="server" Text="LINEでログインする"  BackColor="#0DC300" BorderStyle="None" ForeColor="White" OnClick="Button1_Click" />
        <br />
        <br />
        <asp:Button ID="Button2" runat="server" Text="Yhooでログインする"  BackColor="Red" BorderStyle="None" ForeColor="White" OnClick="Button2_Click" />
    </div>
</asp:Content>
