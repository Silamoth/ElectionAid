<%@ Page Title="Election Aid" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="RatingFAQ.aspx.cs" Inherits="RatingFAQ" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <p>
        &nbsp;</p>
    <asp:Panel ID="mainPanel" runat="server">
        <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="XX-Large" Text="Ratings FAQ"></asp:Label>
        <br />
        <br />
        <asp:Label ID="Label2" runat="server" Font-Size="X-Large" Text="What do the ratings mean?"></asp:Label>
        <br />
        The ratings are meant to be a reflection of the quality of the submitted article.&nbsp; Highly-rated articles should be informative, largely unbiased, and well-written.&nbsp; Ratings are not meant to reflect whether or not you agree with the position presented in the article.&nbsp; We&#39;re all only human, but please try to rate articles objectively if you are going to submit a rating.<br />
        <br />
        <asp:Label ID="Label3" runat="server" Font-Size="X-Large" Text="How exactly do I submit a rating?"></asp:Label>
        <br />
        When you click on a star rating, that rating is submitted and sent to the server.&nbsp; Spamming the refresh button or the Go button will not submit your ratings.&nbsp; This is done in an attempt to eliminate spamming of ratings.&nbsp; At the very least, it makes spamming ratings more inconvenient.<br />
        <br />
        <asp:Label ID="Label4" runat="server" Font-Size="X-Large" Text="How many times can you rate an article?"></asp:Label>
        <br />
        There is nothing preventing you from rating articles multiple times.&nbsp; However, doing so would defeat the spirit of this website.&nbsp; So, I ask that you please do not do that.&nbsp; If you are worried about others doing so, then consider inviting honest friends to submit and rate articles on the website.&nbsp; Spamming should not be used to counteract spamming, though.&nbsp;
    </asp:Panel>

</asp:Content>