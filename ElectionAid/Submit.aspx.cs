using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

public partial class Submit : System.Web.UI.Page
{
    public string ip = "52.162.213.216";

    protected void submitButton_Click(object sender, EventArgs e)
    {
        if (politicianTextBox.Text == String.Empty)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "poliAlert", "alert('You must enter a politician name.')", true);
            return;
        }

        if (titleTextBox.Text == String.Empty)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "titleAlert", "alert('You must enter a title.')", true);
            return;
        }

        if (linkTextBox.Text == String.Empty)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "linkAlert", "alert('You must enter an article URL.')", true);
            return;
        }

        if (zipTextBox.Text == String.Empty)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "zipAlert", "alert('You must enter a ZIP code.')", true);
            return;
        }

        String[] splitName = politicianTextBox.Text.Split(' ');
        String name = String.Empty;
        foreach (String word in splitName)
        {
            name += word + "/";
        }

        String[] splitTitle = titleTextBox.Text.Split(' ');
        String title = String.Empty;
        foreach (String word in splitTitle)
        {
            title += word + "/";
        }

        String url = linkTextBox.Text;
        if (url.Contains("https://"))
            url = url.Remove(url.IndexOf("https://"), 8);
        if (url.Contains("www."))
            url = url.Remove(url.IndexOf("www."), 4);
        if (url.Contains("http://"))
            url = url.Remove(url.IndexOf("http://"), 7);

        String zip = zipTextBox.Text;
        String id;

        if (radioButtonList.SelectedIndex == 0)
        {
            if (!Regex.IsMatch(zip, "^[0-9]*$"))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "zipAlert", "alert('You must enter a valid ZIP code.')", true);
                return;
            }

            id = "1";
        }
        else
        {
            if (!Regex.IsMatch(zip.ToUpper(), "^[A-Z]*$"))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "zipAlert", "alert('You must enter a valid ZIP code.')", true);
                return;
            }

            id = "4";
            zip = zip.ToUpper();
        }

        TcpClient client = new TcpClient(ip, 1303);
        StreamWriter writer = new StreamWriter(client.GetStream());
        String messageString = id + " " + name.TrimEnd(new char[] { '/' }) + " " + title.TrimEnd(new char[] { '/' }) + " " + url + " " + zipTextBox.Text;

        byte[] message = Encoding.ASCII.GetBytes(messageString);
        writer.BaseStream.Write(message, 0, message.Length);

        byte[] buffer = new byte[client.Client.ReceiveBufferSize];
        client.Client.Receive(buffer);
        String response = Encoding.ASCII.GetString(buffer).TrimEnd(new char[] { '\n', '\r', '\0' });
        List<String> splitResponse = response.Split(' ').ToList<String>();

        if (splitResponse[0] == "1" || splitResponse[0] == "4")
        {
            if (splitResponse[1] == "Success")
            {
                Server.Transfer("SuccessfulSubmission.aspx", true);
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            radioButtonList.SelectedIndex = 0;
    }

    protected void radioButtonList_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Postbacks and resets...

        if (radioButtonList.SelectedIndex == 0)
            zipStateLabel.Text = "Zip Code:";
        else
            zipStateLabel.Text = "State Abbreviation:";
    }
}