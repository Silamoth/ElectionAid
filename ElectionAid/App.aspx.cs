using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net.Sockets;
using System.Text;

public partial class App : Page
{
    public string ip = "52.162.213.216";

    List<String> linkOutputs;
    int currentPage;
    List<LinkButton> pageButtons;
    String zip;

    protected void goButton_Click(object sender, EventArgs e)
    {
        String zipCode = zipTextBox.Text;

        if (zipCode == zip)
        {
            //Session.Abandon();

            linkLabelOne.Text = String.Empty;
            linkLabelOne.Visible = false;
            ratingOne.Visible = false;

            linkLabelTwo.Text = String.Empty;
            linkLabelTwo.Visible = false;
            ratingTwo.Visible = false;

            linkLabelThree.Text = String.Empty;
            linkLabelThree.Visible = false;
            ratingThree.Visible = false;

            linkLabelFour.Text = String.Empty;
            linkLabelFour.Visible = false;
            ratingFour.Visible = false;

            linkLabelFive.Text = String.Empty;
            linkLabelFive.Visible = false;
            ratingFive.Visible = false;

            foreach (LinkButton button in pageButtons)
            {
                mainPanel.Controls.Remove(button);
            }

            linkOutputs = new List<String>();
            pageButtons = new List<LinkButton>();
        }

        String id;

        if (radioButtonList.SelectedIndex == 0)
            id = "0";
        else
            id = "3";

        TcpClient client = new TcpClient(ip, 1303);
        StreamWriter writer = new StreamWriter(client.GetStream());
        String messageString = id + " " + zipCode;

        byte[] message = Encoding.ASCII.GetBytes(messageString);
        writer.BaseStream.Write(message, 0, message.Length);

        byte[] buffer = new byte[client.Client.ReceiveBufferSize];
        client.Client.Receive(buffer);

        String response = Encoding.ASCII.GetString(buffer).TrimEnd(new char[] { '\n', '\r', default(char) });

        linksLabel.Text = String.Empty;

        if (response == "None")
        {
            linksLabel.Text = "No information has been found for your area.  Consider submitting some for others to view.";

            Session.Abandon();

            linkLabelOne.Text = String.Empty;
            linkLabelOne.Visible = false;
            ratingOne.Visible = false;

            linkLabelTwo.Text = String.Empty;
            linkLabelTwo.Visible = false;
            ratingTwo.Visible = false;

            linkLabelThree.Text = String.Empty;
            linkLabelThree.Visible = false;
            ratingThree.Visible = false;

            linkLabelFour.Text = String.Empty;
            linkLabelFour.Visible = false;
            ratingFour.Visible = false;

            linkLabelFive.Text = String.Empty;
            linkLabelFive.Visible = false;
            ratingFive.Visible = false;
        }
        else
        {
            linkOutputs = new List<String>();
            zip = zipCode;

            List<String> splitResponse = response.Split(' ').ToList<String>();
            foreach (String entry in splitResponse)
            {
                String[] info = entry.Split(',');
                String[] splitTitle = info[0].Split('/');
                String url = info[1];
                String rating = info[2];
                String person = info[3];

                String[] splitPersonName = person.Split('/');

                String name = String.Empty;

                foreach (String word in splitPersonName)
                {
                    name += word + " ";
                }

                String title = String.Empty;

                foreach (String word in splitTitle)
                {
                    title += word + " ";
                }

                linkOutputs.Add(title + "<br>" + "Link: <a href = http://www." + url + ">" + url + "</a>" + "<br>" + "Politician Name: " + name + "<br>" + "Rating: " + rating);
            }

            int numPages = linkOutputs.Count / 5;

            if (linkOutputs.Count % 5 != 0)
                numPages++;

            for (int i = 0; i < numPages; i++)
            {
                LinkButton newPageButton = new LinkButton();
                newPageButton.Text = (i + 1).ToString() + " ";
                newPageButton.ID = (i + 1).ToString();
                mainPanel.Controls.Add(newPageButton);
                pageButtons.Add(newPageButton);
            }

            currentPage = 1;

            linkLabelOne.Text = linkOutputs[0];
            linkLabelOne.Visible = true;
            ratingOne.Visible = true;

            if (linkOutputs.Count >= 2)
            {
                linkLabelTwo.Text = linkOutputs[1];
                linkLabelTwo.Visible = true;
                ratingTwo.Visible = true;
            }

            if (linkOutputs.Count >= 3)
            {
                linkLabelThree.Text = linkOutputs[2];
                linkLabelThree.Visible = true;
                ratingThree.Visible = true;
            }

            if (linkOutputs.Count >= 4)
            {
                linkLabelFour.Text = linkOutputs[3];
                linkLabelFour.Visible = true;
                ratingFour.Visible = true;
            }

            if (linkOutputs.Count >= 5)
            {
                linkLabelFive.Text = linkOutputs[4];
                linkLabelFive.Visible = true;
                ratingFive.Visible = true;
            }

            Session["linkOutputs"] = linkOutputs;
            Session["currentPage"] = currentPage;
            Session["pageButtons"] = pageButtons;
            Session["zip"] = zip;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session.Clear();
            radioButtonList.SelectedIndex = 0;
            linksLabel.Text = "Enter a ZIP code or state abbreviation to find information about politicians in that area.";
        }

        try
        {
            linkOutputs = (List<String>)Session["linkOutputs"];
            currentPage = (int)Session["currentPage"];
            pageButtons = (List<LinkButton>)Session["pageButtons"];
            zip = (String)Session["zip"];
        }
        catch { }

        if (pageButtons == null)
            pageButtons = new List<LinkButton>();
        else
        {
            foreach (LinkButton buttonToAdd in pageButtons)
            {
                mainPanel.Controls.Add(buttonToAdd);
            }

            try
            {
                Control requester;

                String controlName = Request.Params.Get("__EVENTTARGET");
                if (!String.IsNullOrEmpty(controlName))
                {
                    requester = FindControl(controlName);

                    linkLabelOne.Text = String.Empty;
                    linkLabelOne.Visible = false;
                    ratingOne.Visible = false;

                    linkLabelTwo.Text = String.Empty;
                    linkLabelTwo.Visible = false;
                    ratingTwo.Visible = false;

                    linkLabelThree.Text = String.Empty;
                    linkLabelThree.Visible = false;
                    ratingThree.Visible = false;

                    linkLabelFour.Text = String.Empty;
                    linkLabelFour.Visible = false;
                    ratingFour.Visible = false;

                    linkLabelFive.Text = String.Empty;
                    linkLabelFive.Visible = false;
                    ratingFive.Visible = false;

                    LinkButton button = (LinkButton)requester;
                    int page;
                    int.TryParse(button.Text, out page);

                    currentPage = page;

                    int index = currentPage * 5 - 5;

                    linkLabelOne.Text = linkOutputs[index];
                    linkLabelOne.Visible = true;
                    ratingOne.Visible = true;
                    index++;

                    if (linkOutputs.Count > index)
                    {
                        linkLabelTwo.Text = linkOutputs[index];
                        linkLabelTwo.Visible = true;
                        ratingTwo.Visible = true;
                        index++;
                    }

                    if (linkOutputs.Count > index)
                    {
                        linkLabelThree.Text = linkOutputs[index];
                        linkLabelThree.Visible = true;
                        ratingThree.Visible = true;
                        index++;
                    }

                    if (linkOutputs.Count > index)
                    {
                        linkLabelFour.Text = linkOutputs[index];
                        linkLabelFour.Visible = true;
                        ratingFour.Visible = true;
                        index++;
                    }

                    if (linkOutputs.Count > index)
                    {
                        linkLabelFive.Text = linkOutputs[index];
                        linkLabelFive.Visible = true;
                        ratingFive.Visible = true;
                        index++;
                    }
                }
            }
            catch { }
        }
    }

    //protected void zipTextBox_TextChanged(object sender, EventArgs e)
    //{
    //    //currentTypedZip = zipTextBox.Text;

    //    Session.Abandon();

    //    linkLabelOne.Text = String.Empty;
    //    linkLabelOne.Visible = false;
    //    ratingOne.Visible = false;

    //    linkLabelTwo.Text = String.Empty;
    //    linkLabelTwo.Visible = false;
    //    ratingTwo.Visible = false;

    //    linkLabelThree.Text = String.Empty;
    //    linkLabelThree.Visible = false;
    //    ratingThree.Visible = false;

    //    linkLabelFour.Text = String.Empty;
    //    linkLabelFour.Visible = false;
    //    ratingFour.Visible = false;

    //    linkLabelFive.Text = String.Empty;
    //    linkLabelFive.Visible = false;
    //    ratingFive.Visible = false;

    //    foreach (LinkButton button in pageButtons)
    //    {
    //        mainPanel.Controls.Remove(button);
    //    }

    //    linkOutputs = new List<String>();
    //    pageButtons = new List<LinkButton>();
    //}

    protected void ratingOne_Changed(object sender, AjaxControlToolkit.RatingEventArgs e)
    {
        try
        {
            linkOutputs = (List<String>)Session["linkOutputs"];
            currentPage = (int)Session["currentPage"];
            pageButtons = (List<LinkButton>)Session["pageButtons"];
            zip = (String)Session["zip"];
        }
        catch { }

        String id;

        if (radioButtonList.SelectedIndex == 0)
            id = "2";
        else
            id = "5";

        int index = currentPage * 5 - 5;
        int rating;
        int.TryParse(e.Value, out rating);

        TcpClient client = new TcpClient(ip, 1303);
        StreamWriter writer = new StreamWriter(client.GetStream());
        String messageString = id + " " + zip + " " + index.ToString() + " " + rating.ToString();

        byte[] message = Encoding.ASCII.GetBytes(messageString);
        writer.BaseStream.Write(message, 0, message.Length);
    }

    protected void ratingTwo_Changed(object sender, AjaxControlToolkit.RatingEventArgs e)
    {
        String id;

        if (radioButtonList.SelectedIndex == 0)
            id = "2";
        else
            id = "5";

        int index = currentPage * 5 - 5 + 1;
        int rating;
        int.TryParse(e.Value, out rating);

        TcpClient client = new TcpClient(ip, 1303);
        StreamWriter writer = new StreamWriter(client.GetStream());
        String messageString = id + " " + zip + " " + index.ToString() + " " + rating.ToString();

        byte[] message = Encoding.ASCII.GetBytes(messageString);
        writer.BaseStream.Write(message, 0, message.Length);
    }

    protected void ratingThree_Changed(object sender, AjaxControlToolkit.RatingEventArgs e)
    {
        String id;

        if (radioButtonList.SelectedIndex == 0)
            id = "2";
        else
            id = "5";

        int index = currentPage * 5 - 5 + 2;
        int rating;
        int.TryParse(e.Value, out rating);

        TcpClient client = new TcpClient(ip, 1303);
        StreamWriter writer = new StreamWriter(client.GetStream());
        String messageString = id + " " + zip + " " + index.ToString() + " " + rating.ToString();

        byte[] message = Encoding.ASCII.GetBytes(messageString);
        writer.BaseStream.Write(message, 0, message.Length);
    }

    protected void ratingFour_Changed(object sender, AjaxControlToolkit.RatingEventArgs e)
    {
        String id;

        if (radioButtonList.SelectedIndex == 0)
            id = "2";
        else
            id = "5";

        int index = currentPage * 5 - 5 + 3;
        int rating;
        int.TryParse(e.Value, out rating);

        TcpClient client = new TcpClient(ip, 1303);
        StreamWriter writer = new StreamWriter(client.GetStream());
        String messageString = id + " " + zip + " " + index.ToString() + " " + rating.ToString();

        byte[] message = Encoding.ASCII.GetBytes(messageString);
        writer.BaseStream.Write(message, 0, message.Length);
    }

    protected void ratingFive_Changed(object sender, AjaxControlToolkit.RatingEventArgs e)
    {
        String id;

        if (radioButtonList.SelectedIndex == 0)
            id = "2";
        else
            id = "5";

        int index = currentPage * 5 - 5 + 4;
        int rating;
        int.TryParse(e.Value, out rating);

        TcpClient client = new TcpClient(ip, 1303);
        StreamWriter writer = new StreamWriter(client.GetStream());
        String messageString = id + " " + zip + " " + index.ToString() + " " + rating.ToString();

        byte[] message = Encoding.ASCII.GetBytes(messageString);
        writer.BaseStream.Write(message, 0, message.Length);
    }

    protected void radioButtonList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (radioButtonList.SelectedIndex == 0)
            instructionLabel.Text = "Enter ZIP Code:";
        else
            instructionLabel.Text = "Enter State Abbreviation:";
    }
}