<%@ Page Title="Election Aid" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="App.aspx.cs" Inherits="App" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="Panel1" runat="server" DefaultButton="goButton">
        <asp:Label ID="instructionLabel" runat="server" Text="Enter ZIP Code:"></asp:Label>
    </asp:Panel>
    <asp:Panel ID="mainPanel" runat="server" Font-Bold="True" Font-Size="Large" DefaultButton="goButton">
        <asp:TextBox ID="zipTextBox" runat="server" Height="39px"></asp:TextBox>
        <asp:Button ID="goButton" runat="server" Height="40px" OnClick="goButton_Click" Text="Go" Width="59px" />
        <br />
        <asp:RadioButtonList ID="radioButtonList" runat="server" OnSelectedIndexChanged="radioButtonList_SelectedIndexChanged">
            <asp:ListItem>Local</asp:ListItem>
            <asp:ListItem>State</asp:ListItem>
        </asp:RadioButtonList>
        <br />
        Confused about ratings?&nbsp; Check out the <a href = "/RatingFAQ">Ratings FAQ</a> for more information about what ratings mean and how to rate submissions.<br />
        <br />
        Links:<br />
        <asp:Label ID="linksLabel" runat="server" BorderStyle="None"></asp:Label>
        <br />
        <br />
        <asp:Label ID="linkLabelOne" runat="server" Text="Label One" Visible="False"></asp:Label>
        <br />
        <ajaxToolkit:Rating ID="ratingOne" runat="server" MaxRating="10"
            StarCssClass = "ratingStar" EmptyStarCssClass = "emptyRatingStar"
            FilledStarCssClass = "filledRatingStar" WaitingStarCssClass = "savedRatingStar" Visible="False" OnChanged="ratingOne_Changed"
            >
        </ajaxToolkit:Rating>
        <br />
        <br />
        <asp:Label ID="linkLabelTwo" runat="server" Text="Label Two" Visible="False"></asp:Label>
        <ajaxToolkit:Rating ID="ratingTwo" runat="server" MaxRating="10"
            StarCssClass = "ratingStar" EmptyStarCssClass = "emptyRatingStar"
            FilledStarCssClass = "filledRatingStar" WaitingStarCssClass = "savedRatingStar" Visible="False" OnChanged="ratingTwo_Changed">
        </ajaxToolkit:Rating>
        <br />
        <br />
        <asp:Label ID="linkLabelThree" runat="server" Text="Label Three" Visible="False"></asp:Label>
        <ajaxToolkit:Rating ID="ratingThree" runat="server" MaxRating="10"
            StarCssClass = "ratingStar" EmptyStarCssClass = "emptyRatingStar"
            FilledStarCssClass = "filledRatingStar" WaitingStarCssClass = "savedRatingStar" Visible="False" OnChanged="ratingThree_Changed">
        </ajaxToolkit:Rating>
        <br />
        <br />
        <asp:Label ID="linkLabelFour" runat="server" Text="Label Four" Visible="False"></asp:Label>
        <br />
        <ajaxToolkit:Rating ID="ratingFour" runat="server" MaxRating="10"
            StarCssClass = "ratingStar" EmptyStarCssClass = "emptyRatingStar"
            FilledStarCssClass = "filledRatingStar" WaitingStarCssClass = "savedRatingStar" Visible="False" OnChanged="ratingFour_Changed">
        </ajaxToolkit:Rating>
        <br />
        <br />
        <asp:Label ID="linkLabelFive" runat="server" Text="Label Five" Visible="False"></asp:Label>
        <br />
        <ajaxToolkit:Rating ID="ratingFive" runat="server" MaxRating="10"
            StarCssClass = "ratingStar" EmptyStarCssClass = "emptyRatingStar"
            FilledStarCssClass = "filledRatingStar" WaitingStarCssClass = "savedRatingStar" Visible="False" OnChanged="ratingFive_Changed">
        </ajaxToolkit:Rating>
        <br />
        <br />
        <br />
    </asp:Panel>

<style type = "text/css">
    .ratingStar
{
      white-space:nowrap;
      margin:3px;
      height:20px;
      width: 18px;
}
.ratingStar .ratingItem {
    font-size: 0pt;
    width: 18px;
    height: 20px;
    margin: 0px;
    padding: 0px;
    display: block;
    background-repeat: no-repeat;
      cursor:pointer;
}
.filledRatingStar {
    background-image: url(../Images/filledStarNew.png);
}
.emptyRatingStar {
    background-image: url(../Images/emptyStarNew.png);
}
.savedRatingStar {
    background-image: url(../Images/filledStarNew.png);
}
</style>

</asp:Content>