using Chat.ADO;
using Chat.Data;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Chat
{
    /// <summary>
    /// Логика взаимодействия для RegPage.xaml
    /// </summary>
    public partial class RegPage : Window
    {
        public RegPage()
        {
            InitializeComponent();
        }

        private void registration_bt_Click(object sender, RoutedEventArgs e)
        {
            if (login_tb.Text != "" && password_pb.Password != "")
            {
                var userData = DBConnection.connection.User.Where(x => x.Login == login_tb.Text).FirstOrDefault();
                if (userData == null)
                {
                    User newLogin = new User()
                    {
                        Login = login_tb.Text,
                        Password = password_pb.Password,
                    };
                    MyChat.CurrentUser = newLogin;
                    DBConnection.connection.User.Add(newLogin);
                    DBConnection.connection.SaveChanges();
                    MyChat newWindow = new MyChat();
                    newWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Пользователь с таким логином уже существует!");
                }
            }
            else
            {
                MessageBox.Show("Заполните все данные!");
            }
        }

        private void sign_in_bt_Click(object sender, RoutedEventArgs e)
        {
            MainWindow newWindow = new MainWindow();
            newWindow.Show();
            this.Close();
        }
    }
}
