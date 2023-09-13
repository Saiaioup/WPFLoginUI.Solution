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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFLoginUI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        //private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //}

        private void button_Click(object sender, RoutedEventArgs e)
        {
            register();
            //this.Close();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            login();
        }
        private void login()
        {
            if (this.txt_L_Name.Text.Trim().Length < 1)
            {
                MessageBox.Show("用户名不能为空！");
                return;
            }
            AppHelper.UserName = this.txt_L_Name.Text.Trim();
            ClientWindow Cilent1 = new ClientWindow();  //this.Content = new Main();//wpf:实现界面转换
            Cilent1.Show();
            this.Close();
        }
        private void register()
        {
            if (this.txt_L_Name.Text.Trim().Length < 1)
            {
                MessageBox.Show("用户名不能为空！");
                return;
            }
            else
            {
                MessageBox.Show("注册成功！");
                return;
            }
        }

        private void txt_L_Name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                login();
            }
        }
    }
}
