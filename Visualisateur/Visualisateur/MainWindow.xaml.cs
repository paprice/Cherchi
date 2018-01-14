using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Visualisateur.Windows;
using Visualisateur.Other;
using System.Threading.Tasks;

namespace Visualisateur
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private string pathDirectory;
        private string pathUsersFile;
        private CreateWindow cw;

        private User currentUser;

        private List<User> users;


        public MainWindow()
        {
            string userName = Environment.UserName;
            pathDirectory = @"C:\\Users\\" + userName + "\\Documents\\Cherchi\\";

            pathUsersFile = pathDirectory + "users.xml";
            cw = new CreateWindow(pathDirectory);
            InitializeComponent();
            CreateDirectory();
            CreateButton();
        }

        private void CreateDirectory()
        {
            if (!Directory.Exists(pathDirectory))
            {
                DirectoryInfo di = Directory.CreateDirectory(pathDirectory);
            }
        }

        private void Btn_create_Click(object sender, RoutedEventArgs e)
        {
            cw.ShowDialog();

            CreateButton();

        }

        private void CreateButton()
        {
            usersGrid.Children.RemoveRange(0, usersGrid.Children.Capacity);

            users = User.ReadXmlUser(pathUsersFile);
            int count = 0;

            foreach (User u in users)
            {
                if (count >= 10)
                {
                    break;
                }
                Button b = new Button
                {
                    Content = u.GetPseudo(),
                    Width = 115,
                    Height = 115
                };
                b.Click += Btn_User_Click;

                usersGrid.Children.Add(b);

                int col = count % 4;
                int ran = count / 4;

                Grid.SetColumn(b, col);
                Grid.SetRow(b, ran);


                count++;
            }
        }

        private void Btn_delete_Click(object sender, RoutedEventArgs e)
        {
            if (currentUser != null)
            {
                var result = this.ShowModalMessageExternal("Supprimer", "Voulez-vous suppirmer cet utilisateur ?", MessageDialogStyle.AffirmativeAndNegative);

                if(result.Equals(MessageDialogResult.Affirmative))
                {
                    users.Remove(currentUser);
                    User.WriteXmlUser(users,pathUsersFile);
                    CreateButton();
                }
            }
        }

        private void Btn_User_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            currentUser = FindUser(b.Content.ToString());
        }

        private User FindUser(string v)
        {
            foreach (User u in users)
            {
                if (u.GetPseudo().Equals(v))
                {
                    return u;
                }
            }
            return null;
        }

        private void Btn_connexion_Click(object sender, RoutedEventArgs e)
        {
            if (currentUser != null)
            {
                LibraryWindow lw = new LibraryWindow(currentUser,pathUsersFile);
                lw.Show();
                this.Hide();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }
    }
}
