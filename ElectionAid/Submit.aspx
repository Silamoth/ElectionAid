<%@ Page Title="Election Aid" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Submit.aspx.cs" Inherits="Submit" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="Panel1" runat="server" style="margin-left: 5px">
        <asp:Panel ID="Panel2" runat="server">
            <br />
            <asp:RadioButtonList ID="radioButtonList" runat="server" OnSelectedIndexChanged="radioButtonList_SelectedIndexChanged">
                <asp:ListItem>Local</asp:ListItem>
                <asp:ListItem>State</asp:ListItem>
            </asp:RadioButtonList>
            <br />
            Politician Name:<br />
            <asp:TextBox ID="politicianTextBox" runat="server"></asp:TextBox>
            <br />
            <br />
            Submission Title:<br />
            <asp:TextBox ID="titleTextBox" runat="server"></asp:TextBox>
            <br />
            <br />
            Article Link:<br />
            <asp:TextBox ID="linkTextBox" runat="server"></asp:TextBox>
            <br />
            <br />
            <asp:Label ID="zipStateLabel" runat="server" Text="ZIP Code/State Abbreviation:"></asp:Label>
            <br />
            <asp:TextBox ID="zipTextBox" runat="server"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="submitButton" runat="server" OnClick="submitButton_Click" Text="Submit" />
        </asp:Panel>
    </asp:Panel>
    </asp:Content>
