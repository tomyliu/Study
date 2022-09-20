using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ActiveMQMonitor
{
    public partial class QueueTest : Form
    {
        XElement QueueList = null; // Queue Information
        public QueueTest()
        {
            InitializeComponent();
        }
         private void Form1_Load(object sender, EventArgs e)
        {
            txtActiveMQIP.Text = Properties.Settings.Default.ActiveMQIP;
            txtAdminPort.Text = Properties.Settings.Default.AdminPort.ToString();
            txtUserID.Text = Properties.Settings.Default.UserID;
            txtPassword.Text = Properties.Settings.Default.Password;
            txtMQPort.Text = Properties.Settings.Default.MQPort.ToString();
        }
        private void btnPushMessage_Click(object sender, EventArgs e)
        {
            //string brokerUri = "activemq:failover:(tcp://10.48.160.49:61616,tcp://10.48.167.52:61616)?randomize=false";
            string brokerUri = "activemq:tcp://10.48.167.52:61616?alwaysSessionAsync=true";
            //IConnectionFactory factory = new NMSConnectionFactory(brokerUri);
            //IConnection connection = factory.CreateConnection();
            //connection.Start();

            //ISession session = connection.CreateSession();


            Uri connecturl = new Uri("activemq:tcp://10.48.167.52:61616");
            IConnectionFactory factory = new ConnectionFactory(connecturl);
            using (IConnection connection = factory.CreateConnection())
            {
                using (ISession session = connection.CreateSession())
                {
                    IDestination destination = SessionUtil.GetDestination(session, "queue://test");
                    using (IMessageProducer producer = session.CreateProducer(destination))
                    {
                        //producer.DeliveryMode = MsgDeliveryMode.Persistent;
                        //producer.RequestTimeout = TimeSpan.FromSeconds(2);
                        for (int i = 1; i < 7; i++)
                        {
                            ITextMessage request = session.CreateTextMessage("oh,my friend" + i);
                            txtInMessage.Text += ("oh,my friend" + i);
                            producer.Send(request);
                        }
                    }
                }
            }


        }
        protected void onMessage(IMessage receivedMsg)
        {
            ITextMessage message = receivedMsg as ITextMessage;
            if (message != null)
            {
                //查詢出訊息
                Console.WriteLine(message.Text);
                // txtInMessage.Text += message.Text;
            }
        }
        private void btnPullMessage_Click(object sender, EventArgs e)
        {
            Uri connecturl = new Uri("activemq:tcp://10.48.167.52:61616");
            IConnectionFactory factory = new ConnectionFactory(connecturl);
            using (IConnection connection = factory.CreateConnection())
            {
                using (ISession session = connection.CreateSession())
                {
                    IDestination destination = SessionUtil.GetDestination(session, "queue://test");
                    using (IMessageConsumer consumer = session.CreateConsumer(destination))
                    {
                        connection.Start();
                        consumer.Listener += new MessageListener(onMessage);
                        //Console.Read();
                        //while (true)
                        //{
                        //    Thread.Sleep(500);
                        //    var message = (ITextMessage)consumer.Receive(TimeSpan.FromSeconds(4));
                        //    if (message != null)
                        //    {
                        //        Console.WriteLine(message.NMSMessageId);
                        //        Console.WriteLine(message.Text);
                        //    }
                        //}
                    }
                }
            }
        }

        private void btnPullMessage1_Click(object sender, EventArgs e)
        {
            Uri connecturl = new Uri("activemq:tcp://10.48.167.52:61616");
            IConnectionFactory factory = new ConnectionFactory(connecturl);
            using (IConnection connection = factory.CreateConnection())
            {
                using (ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge))
                {
                    IDestination destination = SessionUtil.GetDestination(session, "queue://test"); //session.GetQueue("TEST.ReceiveBrowseReceive");
                    IMessageProducer producer = session.CreateProducer(destination);
                    IMessageConsumer consumer = session.CreateConsumer(destination);
                    connection.Start();



                    IMessage[] outbound = new IMessage[]{session.CreateTextMessage("First Message"),
                                                         session.CreateTextMessage("Second Message"),
                                                         session.CreateTextMessage("Third Message")};

                    // lets consume any outstanding messages from previous test runs
                    while (consumer.Receive(TimeSpan.FromMilliseconds(1000)) != null)
                    {
                    }

                    producer.Send(outbound[0]);
                    producer.Send(outbound[1]);
                    producer.Send(outbound[2]);

                    IMessage msg = consumer.Receive(TimeSpan.FromMilliseconds(1000));

                    // Get the first.
                    Console.Write(((ITextMessage)outbound[0]).Text);
                    Console.Write(((ITextMessage)msg).Text);
                    consumer.Close();

                    IQueueBrowser browser = session.CreateBrowser((IQueue)destination);
                    IEnumerator enumeration = browser.GetEnumerator();

                    // browse the second
                    Console.Write(enumeration.MoveNext());
                    Console.Write("should have received the second message");
                    Console.Write(((ITextMessage)outbound[1]).Text);
                    Console.Write(((ITextMessage)enumeration.Current).Text);

                    // browse the third.
                    Console.Write(enumeration.MoveNext());
                    Console.Write("Should have received the third message");
                    Console.Write(((ITextMessage)outbound[2]).Text);
                    Console.Write(((ITextMessage)enumeration.Current).Text);

                    // There should be no more.
                    bool tooMany = false;
                    while (enumeration.MoveNext())
                    {
                        Console.Write("Got extra message: {0}", ((ITextMessage)enumeration.Current).Text);
                        tooMany = true;
                    }
                    Console.Write(tooMany);

                    //Reset should take us back to the start.
                    enumeration.Reset();

                    // browse the second
                    Console.Write(enumeration.MoveNext());
                    Console.Write("should have received the second message");
                    Console.Write(((ITextMessage)outbound[1]).Text);
                    Console.Write(((ITextMessage)enumeration.Current).Text);

                    // browse the third.
                    Console.Write(enumeration.MoveNext());
                    Console.Write("Should have received the third message");
                    Console.Write(((ITextMessage)outbound[2]).Text);
                    Console.Write(((ITextMessage)enumeration.Current).Text);

                    // There should be no more.
                    tooMany = false;
                    while (enumeration.MoveNext())
                    {
                        Console.Write("Got extra message: {0}", ((ITextMessage)enumeration.Current).Text);
                        tooMany = true;
                    }
                    Console.Write(tooMany);

                    browser.Close();

                    // Re-open the consumer.
                    consumer = session.CreateConsumer(destination);

                    // Receive the second.
                    Console.Write(((ITextMessage)outbound[1]).Text);
                    Console.Write(((ITextMessage)consumer.Receive(TimeSpan.FromMilliseconds(1000))).Text);
                    // Receive the third.
                    Console.Write(((ITextMessage)outbound[2]).Text);
                    Console.Write(((ITextMessage)consumer.Receive(TimeSpan.FromMilliseconds(1000))).Text);
                    consumer.Close();
                }
            }
        }

        private void btnQueryQueue_Click(object sender, EventArgs e)
        {
            if (QueueList != null && !txtQueryQueueItem.Text.Trim().Equals(""))
            {
                foreach (XElement Item in QueueList.Elements())
                {
                    if (txtQueryQueueItem.Text == Item.Attribute("name").Value)
                    {
                        txtEnqueued.Text = Item.Element("stats").Attribute("enqueueCount").Value;
                        txtDequeued.Text = Item.Element("stats").Attribute("dequeueCount").Value;
                        txtPending.Text = Item.Element("stats").Attribute("size").Value;
                        txtConsumers.Text = Item.Element("stats").Attribute("consumerCount").Value;
                    }
                }
            }




        }

       

        private void btnSaveSetting_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ActiveMQIP = txtActiveMQIP.Text;
            Properties.Settings.Default.AdminPort = Convert.ToInt32(txtAdminPort.Text);
            Properties.Settings.Default.UserID = txtUserID.Text;
            Properties.Settings.Default.Password = txtPassword.Text;
            Properties.Settings.Default.MQPort = Convert.ToInt32(txtMQPort.Text);
            Properties.Settings.Default.Save();
        }

        private void btnGetQueueList_Click(object sender, EventArgs e)
        {
            string RtVal = null;
            string ActiveMQUrl = string.Format(@"http://{0}:{1}/admin/xml/queues.jsp", txtActiveMQIP.Text, txtAdminPort.Text);

            var webRequest = System.Net.WebRequest.Create(ActiveMQUrl);
            webRequest.Credentials = new System.Net.NetworkCredential(txtUserID.Text, txtPassword.Text);
            if (webRequest != null)
            {
                webRequest.Method = "GET";
                webRequest.ContentType = "application/xml";

                try
                {
                    using (var response = (System.Net.HttpWebResponse)webRequest.GetResponse())
                    {

                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            using (System.IO.Stream s = response.GetResponseStream())
                            {
                                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                                {
                                    RtVal = sr.ReadToEnd();
                                    QueueList = XElement.Parse(RtVal);
                                    listQueueName.Items.Clear();
                                    foreach (XElement Item in QueueList.Elements())
                                    {
                                        listQueueName.Items.Add(Item.Attribute("name").Value);
                                    }
                                    listQueueName.Sorted = true;
                                }
                            }
                        }
                    }
                }
                catch (WebException webex)
                {
                    var webResponse = webex.Response as System.Net.HttpWebResponse;

                    if (webResponse != null)
                    {
                        //Entrylog.Error(((int)webResponse.StatusCode).ToString() + "-" + webResponse.StatusCode.ToString());
                    }
                }
            }
        }



        private void listQueueName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox LB = (ListBox)sender;

            txtQueryQueueItem.Text = LB.SelectedItem.ToString();
        }

        private void btnBrowserQueue_Click(object sender, EventArgs e)
        {
            txtInMessage.Text = "";
            if (QueueList != null && !txtQueryQueueItem.Text.Trim().Equals(""))
            {
                Uri connecturl = new Uri(string.Format("activemq:tcp://{0}:{1}", txtActiveMQIP.Text, txtMQPort.Text));
                string QueueName = string.Format("queue://{0}", txtQueryQueueItem.Text);

                IConnectionFactory factory = new ConnectionFactory(connecturl);
                using (IConnection connection = factory.CreateConnection())
                {
                    using (ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge))
                    {
                        IDestination destination = SessionUtil.GetDestination(session, QueueName); //session.GetQueue("TEST.ReceiveBrowseReceive");
                        IMessageProducer producer = session.CreateProducer(destination);
                        IMessageConsumer consumer = session.CreateConsumer(destination);
                        connection.Start();
                        IQueueBrowser browser = session.CreateBrowser((IQueue)destination);

                        IEnumerator enumeration = browser.GetEnumerator();
                        while (enumeration.MoveNext())
                        {
                            txtInMessage.Text += ((ITextMessage)enumeration.Current).Text + "\r\n";

                        }
                        enumeration.Reset();

                    }
                }
            }
        }

        private void btnCleanQueue_Click(object sender, EventArgs e)
        {
            txtInMessage.Text = "";
            if (QueueList != null && !txtQueryQueueItem.Text.Trim().Equals(""))
            {

                Uri connecturl = new Uri(string.Format("activemq:tcp://{0}:{1}", txtActiveMQIP.Text, txtMQPort.Text));
                string QueueName = string.Format("queue://{0}", txtQueryQueueItem.Text);

                IConnectionFactory factory = new ConnectionFactory(connecturl);
                using (IConnection connection = factory.CreateConnection())
                {
                    using (ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge))
                    {
                        IDestination destination = SessionUtil.GetDestination(session, QueueName); //session.GetQueue("TEST.ReceiveBrowseReceive");

                        IMessageProducer producer = session.CreateProducer(destination);
                        IMessageConsumer consumer = session.CreateConsumer(destination);
                        connection.Start();

                        while (true)
                        {
                            var message = (ITextMessage)consumer.Receive(TimeSpan.FromMilliseconds(100));
                            if (message != null)
                            {
                                //Console.WriteLine(message.NMSMessageId);
                                //Console.WriteLine(message.Text);
                                txtInMessage.Text += message.Text;
                            }
                            else
                            {
                                break;
                            }
                            Thread.Sleep(100);
                        }

                    }
                }
            }
        }
    }
}