using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFLoginUI
{
    /// <summary>
    /// ServerWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ServerWindow : Window
    {
        public ServerWindow()
        {
            InitializeComponent();
            InitSocket();
        }
        private byte[] result = new byte[1024];
        private int MyProt = 8989;//端口号
        static Socket serverSocket;
        Dictionary<string, Socket> clientList = new Dictionary<string, Socket>();
        //启动服务
        void InitSocket()
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(ip, MyProt));

            serverSocket.Listen(10);
            textBox.Text += string.Format("启动监听{0}成功", serverSocket.LocalEndPoint.ToString()) + "\r\n";

            Thread myThread = new Thread(ListenClientConnect);
            myThread.Start();
        }
        //监听客户端
        private void ListenClientConnect()
        {
            while (true)
            {
                Socket clientSocket = serverSocket.Accept();
                byte[] msg = Encoding.UTF8.GetBytes("MSG*Hello!");
                clientSocket.Send(msg);

                Thread receiveThread = new Thread(ReceiveMessage);
                Thread.Sleep(500);
                receiveThread.Start(clientSocket);
            }
        }
        private void ReceiveMessage(object clientSocket)
        {
            Socket myCilentSocket = (Socket)clientSocket;
            while (true)
            {
                try
                {
                    int receiveNumber = myCilentSocket.Receive(result);
                    string uMsg = Encoding.UTF8.GetString(result, 0, receiveNumber);
                    string userName = string.Empty;
                    string Unames = string.Empty;
                    if (uMsg.StartsWith("Login*"))
                    {
                        uMsg = uMsg.Replace("Login*", "");

                        if (uMsg.StartsWith("CLOSED*"))
                        {
                            uMsg = uMsg.Replace("CLOSED*", "");
                            clientList.Remove(uMsg);
                        }
                        else
                        {
                            userName = uMsg.Substring(0, uMsg.IndexOf("*"));
                        }

                        if ((!string.IsNullOrEmpty(userName)) && (!clientList.Keys.Contains(userName)))
                        {
                            clientList.Add(userName, myCilentSocket);
                        }
                        foreach (string n in clientList.Keys)
                        {
                            Unames += n + ",";
                        }
                        foreach (Socket name in clientList.Values)
                        {
                            name.Send(Encoding.UTF8.GetBytes("Login*" + Unames));
                        }
                    }
                    else if (uMsg.StartsWith("MSGALL*"))
                    {
                        uMsg = "MSG*" + uMsg.Replace("MSGALL*", "");
                        foreach (Socket s in clientList.Values)
                        {
                            s.Send(Encoding.UTF8.GetBytes(uMsg));
                        }
                    }
                    else if (uMsg.StartsWith("MSGONE*"))
                    {
                        uMsg = uMsg.Replace("MSGONE*", "");
                        string ToName = uMsg.Substring(0, uMsg.IndexOf("$"));
                        uMsg = "MSG*" + uMsg.Substring(uMsg.IndexOf("$"), uMsg.Length - 1);
                        if (clientList.ContainsKey(ToName))
                        {
                            Socket n = clientList[ToName];
                            n.Send(Encoding.UTF8.GetBytes(uMsg));
                        }
                    }

                    this.textBox.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        this.textBox.Text += string.Format("接收客服端：{0}，消息：{1}", myCilentSocket.RemoteEndPoint.ToString(), Encoding.UTF8.GetString(result, 0, receiveNumber)) + "\r\n";
                    }));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    myCilentSocket.Shutdown(SocketShutdown.Both);
                    myCilentSocket.Close();
                    break;
                }

            }
        }
    }
}
