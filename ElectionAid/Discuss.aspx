<%@ Page Title="Election Aid" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Discuss.aspx.cs" Inherits="Discuss" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="mainPanel" runat="server" Font-Bold="True" Font-Size="Large">
        <br />
        Choose an issue:<br />
        <asp:DropDownList ID="issueList" runat="server">
            <asp:ListItem>Abortion</asp:ListItem>
            <asp:ListItem>Immigration</asp:ListItem>
            <asp:ListItem>Gun Control</asp:ListItem>
            <asp:ListItem>Marijuana Legalization</asp:ListItem>
            <asp:ListItem>Social Security</asp:ListItem>
            <asp:ListItem>Affirmative Action</asp:ListItem>
            <asp:ListItem>Government-Provided Healthcare</asp:ListItem>
            <asp:ListItem>Government-Provided Higher Education</asp:ListItem>
        </asp:DropDownList>
        <br />
        <br />
        <asp:Button ID="goButton" runat="server" Font-Size="X-Large" OnClick="goButton_Click" Text="Go" />
        <br />
        <br />
        <asp:Label ID="entriesLabel" runat="server"></asp:Label>
    </asp:Panel>

</asp:Content>