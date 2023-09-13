//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;

//namespace WPFLoginUI
//{
//    /// <summary>
//    /// ClientWindow.xaml 的交互逻辑
//    /// </summary>
//    public partial class ClientWindow : Window
//    {
//        public ClientWindow()
//        {
//            InitializeComponent();
//        }

//        private void btn_Send_Click(object sender, RoutedEventArgs e)
//        {

//        }

//        private void txt_C_Display_MouseDoubleClick(object sender, MouseButtonEventArgs e)
//        {

//        }

//        private void txt_Display_TextChanged(object sender, TextChangedEventArgs e)
//        {

//        }
//    }
//}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

    //    /// <summary>
    //    /// ClientWindow.xaml 的交互逻辑
    //    /// </summary>
    public partial class ClientWindow : Window
    {
        public ClientWindow()
        {
            InitializeComponent();
        }

        private byte[] result = new byte[1024];
        Socket clientSocket;


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            connectToServer();
        }
        //连接服务器
        void connectToServer()
        {
            try
            {
                IPAddress ip = IPAddress.Parse("127.0.0.1");
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                clientSocket.BeginConnect(IPAddress.Loopback, 8989, (args) =>
                {
                    if (args.IsCompleted)
                    {
                        byte[] byteSend = Encoding.UTF8.GetBytes("Login*" + AppHelper.UserName + "*");
                        if (clientSocket.Connected)
                        {
                            clientSocket.Send(byteSend);

                            Thread thread = new Thread(new ThreadStart(DataFromServer));
                            thread.IsBackground = true;
                            thread.Start();
                        }
                    }
                }, null);
            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //退出客户端
        private void CLOSED(object sender, System.ComponentModel.CancelEventArgs e)
        {
            clientSocket.Send(Encoding.UTF8.GetBytes("Login*CLOSED*" + AppHelper.UserName));
        }
        //接收数据
        private void DataFromServer()
        {
            while (true)
            {
                Byte[] bytesFrom = new Byte[4096];
                int len = clientSocket.Receive(bytesFrom);
                string dataFromClient = Encoding.UTF8.GetString(bytesFrom, 0, len);

                if (dataFromClient.StartsWith("Login*"))
                {
                    dataFromClient = dataFromClient.Replace("Login*", "");
                    Thread t = new Thread(UpdateList);
                    t.Start(dataFromClient);
                }
                else if (dataFromClient.StartsWith("MSG*"))
                {
                    dataFromClient = dataFromClient.Replace("MSG*", "");
                    this.txt_C_Display.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        this.txt_C_Display.Text += dataFromClient + "\r\n";
                        this.txt_C_Display.ScrollToEnd();
                    }));
                }
                Thread.Sleep(1000);
            }
        }
        //更新用户列表
        private void UpdateList(object severdate)
        {
            string[] names = severdate.ToString().Split(',');
            this.listBox.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.listBox.Items.Clear();
                foreach (string name in names)
                {
                    if (!string.IsNullOrEmpty(name))
                    {
                        this.listBox.Items.Add(name);
                    }
                }
            }));

        }
        //（全体广播）
        private void btn_Send_Click(object sender, RoutedEventArgs e)
        {
            Send();
        }           
        private void Send()
        {
            if (listBox.SelectedItems.Count < 1)
            {
                try
                {
                    string sendMessage = "MSGALL*" + AppHelper.UserName + "对所有人说：" + txt_C_send.Text.Trim() + ",Time:" + DateTime.Now;
                    clientSocket.Send(Encoding.UTF8.GetBytes(sendMessage));
                    txt_C_send.Text = "";
                }
                catch
                {
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
            }
            else
            {
                try
                {
                    string sendOne = "MSGONE*" + listBox.SelectedItem.ToString() + "$" + AppHelper.UserName + "对你说：" + txt_C_send.Text.Trim() + ",Time:" + DateTime.Now;
                    clientSocket.Send(Encoding.UTF8.GetBytes(sendOne));
                    txt_C_Display.Text += "你对" + listBox.SelectedItem.ToString() + "说：" + txt_C_send.Text.Trim() + ",Time:" + DateTime.Now + "\r\n";
                    txt_C_send.Text = "";
                }
                catch
                {
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
            }
        }
        private void txt_C_send_KeyDown(object sender, KeyEventArgs s)
        {
            if (s.Key == Key.Enter)
            {
                Send();
            }
        }

        private void txt_C_Display_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.listBox.SelectedIndex = -1;
        }

        private void txt_C_send_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

  
        private void btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}