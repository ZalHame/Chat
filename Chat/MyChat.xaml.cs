using Chat.ADO;
using Chat.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Chat.ADO;
using Microsoft.Win32;
using System.Security.Cryptography;
using System.IO;

namespace Chat
{
    /// <summary>
    /// Логика взаимодействия для MyChat.xaml
    /// </summary>
    public partial class MyChat : Window
    {
        public static User CurrentUser;
        public int TabCount = 2;
        public MyChat()
        {
            InitializeComponent();
            //lb_Chat.ItemsSource = DBConnection.connection.Message.ToList();
            foreach(var item in DBConnection.connection.Message.ToList())
            {
                if(item.Id_chat == null)
                {
                    var user = DBConnection.connection.User.Where(x => x.Id_user == item.Id_user).FirstOrDefault();
                    lb_Chat.Items.Add($"{user.Login}: {item.text}");
                }
            }
            foreach(var item in DBConnection.connection.UserChat.Where(x => x.Id_user == CurrentUser.Id_user).ToList())
            {
                ADO.Chat chat = DBConnection.connection.Chat.Where(x => x.Id_chat == item.Id_chat).FirstOrDefault();
                Button newBtn = new Button { Name = "btn_Send", Content = "Send", HorizontalAlignment = HorizontalAlignment.Left, Margin = new Thickness(487, 351, 0, 0), VerticalAlignment = VerticalAlignment.Top, Height = 43, Width = 115, FontWeight = FontWeights.Bold, FontSize = 16 };
                newBtn.Click += new RoutedEventHandler(btn_Send_Click);
                TabItem newTabControl = new TabItem() { Header = chat.Name_chat, Name = $"ChatNum_{item.Id_chat}" };
                Grid newGrid = new Grid() { Background = new SolidColorBrush(Colors.LightGray) };
                ListBox newLB = new ListBox { Name = "lb_Chat", Margin = new Thickness(10, 0, 0, 82) };
                newGrid.Children.Add(newLB);
                TextBox newTB = new TextBox { Name = "tb_Message", HorizontalAlignment = HorizontalAlignment.Left, Margin = new Thickness(10, 351, 0, 0), TextWrapping = TextWrapping.Wrap, VerticalAlignment = VerticalAlignment.Top, Width = 452, Height = 43, FontWeight = FontWeights.Bold, FontSize = 16 };
                newTB.KeyDown += new KeyEventHandler(tb_Message_KeyDown);
                newGrid.Children.Add(newTB);
                newGrid.Children.Add(newBtn);
                newTabControl.Content = newGrid;
                foreach (var msg in DBConnection.connection.Message.Where(x => x.Id_chat == chat.Id_chat).ToList())
                {
                    var user = DBConnection.connection.User.Where(x => x.Id_user == item.Id_user).FirstOrDefault();
                    newLB.Items.Add($"{DBConnection.connection.User.Where(x => x.Id_user == msg.Id_user).FirstOrDefault().Login}: {msg.text}");
                }
                Tabs.Items.Insert(TabCount - 1, newTabControl);
                TabCount++;
            }
            UsersList.ItemsSource = DBConnection.connection.User.ToList();
        }

        private void btn_Send_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Message.Text != null)
            {
                string name = (((sender as Button).Parent as Grid).Parent as TabItem).Header.ToString();
                if (name != "Общий чат")
                {
                    int num = int.Parse((((sender as Button).Parent as Grid).Parent as TabItem).Name.Split('_')[1].ToString());
                    ListBox currentLB = ((sender as Button).Parent as Grid).Children[0] as ListBox;
                    TextBox currentTB = ((sender as Button).Parent as Grid).Children[1] as TextBox;
                    currentLB.Items.Add($"{CurrentUser.Login}: {currentTB.Text}");
                    Message newMessage = new Message()
                    {
                        text = currentTB.Text,
                        Id_user = CurrentUser.Id_user,
                        Id_chat = num
                    };
                    currentTB.Text = null;
                    DBConnection.connection.Message.Add(newMessage);
                    DBConnection.connection.SaveChanges();
                }
                else
                {
                    Message newMessage = new Message()
                    {
                        text = tb_Message.Text,
                        Id_user = CurrentUser.Id_user,
                    };
                    lb_Chat.Items.Add($"{CurrentUser.Login}: {tb_Message.Text}");
                    tb_Message.Text = null;
                    DBConnection.connection.Message.Add(newMessage);
                    DBConnection.connection.SaveChanges();
                }
            }
        }

        private void UsersList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(UsersList.SelectedItem != null)
            {
                SelectedUserList.Items.Add(UsersList.SelectedItem);
            }
        }

        private void Create_btn_Click(object sender, RoutedEventArgs e)
        {
            ADO.Chat newChat = new ADO.Chat()
            {
                Name_chat = ChatName_TB.Text,
            };
            DBConnection.connection.Chat.Add(newChat);
            UserChat newUserChat = new UserChat()
            {
                Id_chat = newChat.Id_chat,
                Id_user = CurrentUser.Id_user,
            };
            DBConnection.connection.UserChat.Add(newUserChat);
            foreach (User user in SelectedUserList.Items)
            {
                UserChat newUserChat1 = new UserChat()
                {
                    Id_chat = newChat.Id_chat,
                    Id_user = user.Id_user,
                };
                DBConnection.connection.UserChat.Add(newUserChat1);
            }
            DBConnection.connection.SaveChanges();
            Button newBtn = new Button { Name = "btn_Send", Content = "Send", HorizontalAlignment = HorizontalAlignment.Left, Margin = new Thickness(487, 351, 0, 0), VerticalAlignment = VerticalAlignment.Top, Height = 43, Width = 115, FontWeight = FontWeights.Bold, FontSize = 16 };
            newBtn.Click += new RoutedEventHandler(btn_Send_Click);
            TabItem newTabControl = new TabItem() { Header = ChatName_TB.Text, Name = $"ChatNum_{newChat.Id_chat}" };
            Grid newGrid = new Grid() { Background = new SolidColorBrush(Colors.LightGray) };
            newGrid.Children.Add(new ListBox { Name = "lb_Chat", Margin = new Thickness(10, 0, 0, 82) });
            TextBox newTB = new TextBox { Name = "tb_Message", HorizontalAlignment = HorizontalAlignment.Left, Margin = new Thickness(10, 351, 0, 0), TextWrapping = TextWrapping.Wrap, VerticalAlignment = VerticalAlignment.Top, Width = 452, Height = 43, FontWeight = FontWeights.Bold, FontSize = 16 };
            newTB.KeyDown += new KeyEventHandler(tb_Message_KeyDown);
            newGrid.Children.Add(newTB);
            newGrid.Children.Add(newBtn);
            newTabControl.Content = newGrid;
            Tabs.Items.Insert(TabCount - 1, newTabControl);
            TabCount++;
        }

        private void Tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void tb_Message_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void btn_InputFile_Click(object sender, RoutedEventArgs e)
        {
            string s = "";
            OpenFileDialog win = new OpenFileDialog();
            string path = "";
            if (win.ShowDialog() == true)
            {
                path = win.FileName;
            }
            Image image = new Image();
            image.Source = new BitmapImage(new Uri(path));
            image.Width = 100;
            image.Height = 100;
            lb_Chat.Items.Add($"{CurrentUser.Login}:");
            lb_Chat.Items.Add(image);
            byte[] b = System.IO.File.ReadAllBytes(path);
            for (int i = 0; i < b.Length; i++)
            {
                s += Convert.ToString(b[i], 2);
            }
            lb_Chat.Items.Add($"{s}:");

            var image1 = new BitmapImage();
            File.WriteAllBytes("D:\\file1.png", byte.Parse(s));
            File.Delete("D:\\file1.png");
            File.Delete("file1.png");/*
            using (var mem = new MemoryStream(b))
            {
                mem.Position = 0;
                image1.BeginInit();
                image1.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image1.CacheOption = BitmapCacheOption.OnLoad;
                image1.UriSource = null;
                image1.StreamSource = mem;
                image1.EndInit();
            }*/
        }
    }
    public class Img
    {
        public Img(string value, System.Windows.Controls.Image img) { Str = value; Image = img; }
        public string Str { get; set; }
        public System.Windows.Controls.Image Image { get; set; }
    }
}
