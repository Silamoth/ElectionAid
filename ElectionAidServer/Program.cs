using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace ElectionAidServer
{
    struct Link
    {
        public String Title { get; set; }
        public String URL { get; set; }
        public float Rating { get; set; }
        public String Person { get; set; }
        public int RatingSum { get; set; }
        public int RatingNum { get; set; }
    }
    
    struct RatingToAdd
    {
        public int Rating { get; set; }
        public int Index { get; set; }
        public String ZIP { get; set; }
    }

    class Program
    {
        static Dictionary<String, List<Link>> linkLists;
        static Dictionary<String, Link> linksToAdd;
        static int timer;
        static List<RatingToAdd> ratingsToAdd;

        static Dictionary<String, List<Link>> stateLinkLists;
        static Dictionary<String, Link> stateLinksToAdd;
        static List<RatingToAdd> stateRatingsToAdd;

        static Dictionary<String, List<String>> discussionEntries;

        static void Main(string[] args)
        {
            linkLists = new Dictionary<String, List<Link>>();
            linksToAdd = new Dictionary<String, Link>();
            timer = 0;
            ratingsToAdd = new List<RatingToAdd>();

            stateLinkLists = new Dictionary<String, List<Link>>();
            stateLinksToAdd = new Dictionary<String, Link>();
            stateRatingsToAdd = new List<RatingToAdd>();

            discussionEntries = new Dictionary<String, List<String>>();

            DirectoryInfo data = new DirectoryInfo("Data/Local");

            foreach (DirectoryInfo directory in data.GetDirectories())
            {
                List<Link> zipLinks = new List<Link>();

                foreach (FileInfo file in directory.GetFiles())
                {
                    StreamReader reader = new StreamReader(file.FullName);
                    String[] contents = reader.ReadToEnd().Split('\n');

                    Link link = new Link();
                    link.Title = contents[0];
                    link.URL = contents[1];

                    int rating;
                    int.TryParse(contents[2], out rating);
                    link.Rating = rating;
                    link.Person = contents[3];

                    int ratingSum, ratingNum;
                    int.TryParse(contents[4], out ratingSum);
                    int.TryParse(contents[5], out ratingNum);
                    link.RatingSum = ratingSum;
                    link.RatingNum = ratingNum;

                    zipLinks.Add(link);

                    reader.Close();
                }

                linkLists.Add(directory.Name, zipLinks);
            }

            data = new DirectoryInfo("Data/State");
            foreach (DirectoryInfo directory in data.GetDirectories())
            {
                List<Link> stateLinks = new List<Link>();

                foreach (FileInfo file in directory.GetFiles())
                {
                    StreamReader reader = new StreamReader(file.FullName);
                    String[] contents = reader.ReadToEnd().Split('\n');

                    Link link = new Link();
                    link.Title = contents[0];
                    link.URL = contents[1];

                    int rating;
                    int.TryParse(contents[2], out rating);
                    link.Rating = rating;
                    link.Person = contents[3];

                    int ratingSum, ratingNum;
                    int.TryParse(contents[4], out ratingSum);
                    int.TryParse(contents[5], out ratingNum);
                    link.RatingSum = ratingSum;
                    link.RatingNum = ratingNum;

                    stateLinks.Add(link);

                    reader.Close();
                }

                stateLinkLists.Add(directory.Name, stateLinks);
            }

            data = new DirectoryInfo("Data/Discussions");
            foreach (DirectoryInfo directory in data.GetDirectories())
            {
                List<String> issueEntries = new List<String>();

                foreach (FileInfo file in directory.GetFiles())
                {
                    StreamReader reader = new StreamReader(file.FullName);

                    issueEntries.Add(reader.ReadToEnd());

                    reader.Close();
                }

                discussionEntries.Add(directory.Name, issueEntries);
            }


            ThreadStart updateStart = new ThreadStart(Update);
            ThreadStart clientStart = new ThreadStart(AcceptClients);

            Thread updateThread = new Thread(updateStart);
            Thread clientThread = new Thread(clientStart);

            updateThread.Start();
            clientThread.Start();
        }

        static void Update()
        {
            while (true)
            {
                if (linksToAdd.Count > 0)
                {
                    Dictionary<String, List<Link>> newLinksList = new Dictionary<String, List<Link>>();

                    foreach (String key in linkLists.Keys)
                    {
                        newLinksList.Add(key, linkLists[key]);
                    }

                    foreach (String key in linksToAdd.Keys)
                    {
                        if (!newLinksList.Keys.ToList<String>().Contains(key))
                        {
                            List<Link> newList = new List<Link>();
                            newList.Add(linksToAdd[key]);
                            newLinksList.Add(key, newList);
                        }
                        else
                            newLinksList[key].Add(linksToAdd[key]);
                    }

                    linkLists = newLinksList;
                    linksToAdd = new Dictionary<String, Link>();
                }
                
                if (ratingsToAdd.Count > 0)
                {
                    foreach (RatingToAdd rating in ratingsToAdd)
                    {
                        Link temp = linkLists[rating.ZIP][rating.Index];
                        temp.RatingSum += rating.Rating;
                        temp.RatingNum++;
                        temp.Rating = (float)temp.RatingSum / (float)temp.RatingNum;
                        linkLists[rating.ZIP][rating.Index] = temp;
                    }
                    ratingsToAdd = new List<RatingToAdd>();
                }

                if (stateLinksToAdd.Count > 0)
                {
                    Dictionary<String, List<Link>> newLinksList = new Dictionary<String, List<Link>>();

                    foreach (String key in stateLinkLists.Keys)
                    {
                        newLinksList.Add(key, stateLinkLists[key]);
                    }

                    foreach (String key in stateLinksToAdd.Keys)
                    {
                        if (!newLinksList.Keys.ToList<String>().Contains(key))
                        {
                            List<Link> newList = new List<Link>();
                            newList.Add(stateLinksToAdd[key]);
                            newLinksList.Add(key, newList);
                        }
                        else
                            newLinksList[key].Add(stateLinksToAdd[key]);
                    }
                    
                    stateLinkLists = newLinksList;
                    stateLinksToAdd = new Dictionary<String, Link>();
                }

                if (stateRatingsToAdd.Count > 0)
                {
                    foreach (RatingToAdd rating in stateRatingsToAdd)
                    {
                        Link temp = stateLinkLists[rating.ZIP][rating.Index];
                        temp.RatingSum += rating.Rating;
                        temp.RatingNum++;
                        temp.Rating = temp.RatingSum / temp.RatingNum;
                        stateLinkLists[rating.ZIP][rating.Index] = temp;
                    }
                    stateRatingsToAdd = new List<RatingToAdd>();
                }

                timer++;

                if (timer > 999999)
                {
                    timer = 0;

                    foreach (String key in linkLists.Keys)
                    {
                        //key is zip code

                        DirectoryInfo directory = new DirectoryInfo("Data/Local/" + key);

                        if (directory.Exists)
                        {
                            foreach (FileInfo file in directory.GetFiles())
                            {
                                try { file.Delete(); }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }
                        else                        
                            directory.Create();                        

                        int counter = 0;

                        foreach (Link link in linkLists[key])
                        {
                            //Looping through every link in this zip

                            try
                            {
                                StreamWriter writer = new StreamWriter("Data/Local/" + key + "/" + counter.ToString() + ".txt");

                                writer.WriteLine(link.Title);
                                writer.WriteLine(link.URL);
                                writer.WriteLine(link.Rating);
                                writer.WriteLine(link.Person);
                                writer.WriteLine(link.RatingSum);
                                writer.WriteLine(link.RatingNum);

                                writer.Close();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                            counter++;
                        }
                    }

                    foreach (String key in stateLinkLists.Keys)
                    {
                        //key is zip code

                        DirectoryInfo directory = new DirectoryInfo("Data/State/" + key);

                        if (directory.Exists)
                        {
                            foreach (FileInfo file in directory.GetFiles())
                            {
                                try { file.Delete(); }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }
                        else
                            directory.Create();

                        int counter = 0;

                        foreach (Link link in stateLinkLists[key])
                        {
                            //Looping through every link in this state abbreviation

                            try
                            {
                                StreamWriter writer = new StreamWriter("Data/State/" + key + "/" + counter.ToString() + ".txt");

                                writer.WriteLine(link.Title);
                                writer.WriteLine(link.URL);
                                writer.WriteLine(link.Rating);
                                writer.WriteLine(link.Person);
                                writer.WriteLine(link.RatingSum);
                                writer.WriteLine(link.RatingNum);

                                writer.Close();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                            counter++;
                        }
                    }
                }
            }
        }

        static void AcceptClients()
        {
            TcpListener listener = new TcpListener(1303);
            listener.Start();

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();

                StreamWriter writer = new StreamWriter(client.GetStream());

                byte[] buffer = new byte[client.Client.ReceiveBufferSize];
                client.Client.Receive(buffer);
                String request = Encoding.ASCII.GetString(buffer).TrimEnd(new char[] { '\n', '\r', '\0' });
                List<String> splitRequest = request.Split(' ').ToList<String>();

                switch (splitRequest[0])
                {
                    case "0":
                        //Requesting links for a given ZIP code

                        String zip = splitRequest[1];
                        byte[] response;
                        String responseString;

                        if (linkLists.ContainsKey(zip))
                        {
                            List<Link> linkList = linkLists[zip];

                            responseString = String.Empty;

                            foreach (Link link in linkList)
                            {
                                responseString += link.Title + "," + link.URL + "," + link.Rating.ToString("n2") + "," + link.Person + " ";
                            }

                            response = Encoding.ASCII.GetBytes(responseString.Trim());
                            writer.BaseStream.Write(response, 0, response.Length);
                        }
                        else
                        {
                            responseString = "None";
                            response = Encoding.ASCII.GetBytes(responseString);
                            writer.BaseStream.Write(response, 0, response.Length);
                        }
                        break;
                    case "1":
                        //Adding a new entry

                        String politician = splitRequest[1];
                        String title = splitRequest[2];
                        String url = splitRequest[3];
                        String zipCode = splitRequest[4];

                        Link newLink = new Link();
                        newLink.Person = politician;
                        newLink.URL = url;
                        newLink.Title = title;
                        newLink.Rating = 0;

                        linksToAdd.Add(zipCode, newLink);
                        Console.WriteLine("Submission received...");

                        response = Encoding.ASCII.GetBytes("1 Success");
                        writer.BaseStream.Write(response, 0, response.Length);
                        break;
                    case "2":
                        //Adding a rating

                        int rating, index;

                        zip = splitRequest[1];
                        int.TryParse(splitRequest[2], out index);
                        int.TryParse(splitRequest[3], out rating);

                        RatingToAdd newRating = new RatingToAdd();
                        newRating.Index = index;
                        newRating.Rating = rating;
                        newRating.ZIP = zip;

                        ratingsToAdd.Add(newRating);
                        break;
                    case "3":
                        //Requesting links for a given state abbreviation

                        String state = splitRequest[1];

                        if (stateLinkLists.ContainsKey(state))
                        {
                            List<Link> linkList = stateLinkLists[state];

                            responseString = String.Empty;

                            foreach (Link link in linkList)
                            {
                                responseString += link.Title + "," + link.URL + "," + link.Rating + "," + link.Person + " ";
                            }

                            response = Encoding.ASCII.GetBytes(responseString.Trim());
                            writer.BaseStream.Write(response, 0, response.Length);
                        }
                        else
                        {
                            responseString = "None";
                            response = Encoding.ASCII.GetBytes(responseString);
                            writer.BaseStream.Write(response, 0, response.Length);
                        }
                        break;
                    case "4":
                        //Submitting new state entry

                        politician = splitRequest[1];
                        title = splitRequest[2];
                        url = splitRequest[3];
                        state = splitRequest[4];

                        newLink = new Link();
                        newLink.Person = politician;
                        newLink.URL = url;
                        newLink.Title = title;
                        newLink.Rating = 0;

                        stateLinksToAdd.Add(state, newLink);
                        Console.WriteLine("Submission received...");

                        response = Encoding.ASCII.GetBytes("4 Success");
                        writer.BaseStream.Write(response, 0, response.Length);
                        break;
                    case "5":
                        //State rating

                        state = splitRequest[1];
                        int.TryParse(splitRequest[2], out index);
                        int.TryParse(splitRequest[3], out rating);

                        newRating = new RatingToAdd();
                        newRating.Index = index;
                        newRating.Rating = rating;
                        newRating.ZIP = state;

                        stateRatingsToAdd.Add(newRating);
                        break;
                    case "6":
                        //Requesting discussion entries

                        String issue = splitRequest[1];
                        responseString = String.Empty;

                        foreach (String entry in discussionEntries[issue])
                        {
                             responseString += entry + "*";
                        }
                        break;
                }
            }
        }
    }
}