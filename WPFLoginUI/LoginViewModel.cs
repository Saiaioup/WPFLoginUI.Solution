//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Input;

//namespace WPFLoginUI
//{
//    public class LoginVM:INotifyPropertyChanged
//    {
//        private MainWindow _main;
//        public LoginVM(MainWindow main)
//        {
//            _main = main;
//        }

//        public event PropertyChangedEventHandler PropertyChanged;
//        private void RaisePropertyChanged(string propertyname)
//        {
//            PropertyChangedEventHandler handler = PropertyChanged;
//            if (handler != null)
//                handler(this, new PropertyChangedEventArgs(propertyname));
//        }
//        private LoginModel _LoginM = new LoginModel();


//        public string UserName
//        {
//            get { return _LoginM.UserName; }
//            set { 
//                _LoginM.UserName = value;
//                RaisePropertyChanged("UserName");
//                }
//        }
//        public string PassWord
//        {
//            get { return _LoginM.PassWord; }
//            set
//            {
//                _LoginM.PassWord = value;
//                RaisePropertyChanged("PassWord");
//            }
//        }
//        //void registerFunc()
//        //{
//        //    if (UserName == "" && PassWord == "")
//        //    ///弹出新界面
//        //    {
//        //        MessageBox.Show("请输入用户名和密码");
//        //    }
//        //    else
//        //    {
//        //        string UserName
//        //    }
//        //}
//        //<summary>
//        //登陆方法
//        //
//        void ExitFunc()
//        {
//            _main.Close();
//        }
//        void loginFunc()
//        {
//            if (UserName.Length <1 && PassWord.Length<1)
//            ///弹出新界面
//            {
//                MessageBox.Show("请输入用户名和密码");
//                return;
//            }
//            else
//            {
//                AppHelper.UserName = UserName;
//                ClientWindow clientWindow = new ClientWindow();
//                clientWindow.Show();
//                _main.Close();





//                MessageBox.Show("登陆成功");
//            }
//            ////else
//            /////弹出警告
//            //{
//            //    MessageBox.Show("输入的用户名或者密码不正确");
//            //    UserName = "";
//            //    PassWord = "";
//            //}
//        }

//        bool canloginexecute()
//        {
//            return true;
//        }

//        //命令
//        public ICommand LoginACT
//        {
//            get
//            {
//                return new RelayCommand(loginFunc, canloginexecute);
//            }
//        }
//        public ICommand ExitACT
//        {
//            get
//            {
//                return new RelayCommand(ExitFunc, canloginexecute);
//            }
//        }

//    }
//}
