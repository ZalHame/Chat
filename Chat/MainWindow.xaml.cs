using Chat.ADO;
using Chat.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Principal;
using System.Text;
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

namespace Chat
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Sign_in_bt_Click(object sender, RoutedEventArgs e)
        {
            if (login_tb.Text != null && password_pb.Password != "")
            {
                User newAccount = DBConnection.connection.User.Where(x => x.Login == login_tb.Text).FirstOrDefault();
                if (newAccount != null)
                {
                    if (newAccount.Password == password_pb.Password)
                    {
                        MyChat.CurrentUser = newAccount;
                        MyChat newWindow = new MyChat();
                        newWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Неверный логин или пароль!");
                    }
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль!");
                }
            }
            else
            {
                MessageBox.Show("Заполните все данные!");
            }
        }

        private void sign_up_bt_Click(object sender, RoutedEventArgs e)
        {
            RegPage newWindow = new RegPage();
            newWindow.Show();
            this.Close();
        }
    }
}
