using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Sockets;
using System.IO;
using System.Text;

public partial class Discuss : System.Web.UI.Page
{
public string ip = "52.162.213.216";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            Session.Clear();

        try
        {
            entriesLabel.Text = (String)Session["entries"];
        }
        catch { }
    }

    protected void goButton_Click(object sender, EventArgs e)
    {
        entriesLabel.Text = String.Empty;

        TcpClient client = new TcpClient(ip, 1303);
        StreamWriter writer = new StreamWriter(client.GetStream());
        String messageString = "6 " + issueList.SelectedValue;

        byte[] message = Encoding.ASCII.GetBytes(messageString);
        writer.BaseStream.Write(message, 0, message.Length);

        byte[] buffer = new byte[client.Client.ReceiveBufferSize];
        client.Client.Receive(buffer);

        String response = Encoding.ASCII.GetString(buffer).TrimEnd(new char[] { '\n', '\r', default(char) });

        String[] splitResponses = response.Split('*');

        foreach (String entry in splitResponses)
        {
            entriesLabel.Text += entry + "<br><br>";
        }

        Session["entries"] = entriesLabel.Text;
    }
}