using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Visualisateur.Windows;
using Visualisateur.Other;

namespace Visualisateur
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string path = @".\\users\\";
        private CreateWindow cw;

        private User currentUser;

        private List<User> users;


        public MainWindow()
        {
            cw = new CreateWindow(path);
            InitializeComponent();
            CreateDirectory();
            CreateButton();
        }

        private void CreateDirectory()
        {

            try
            {
                if (Directory.Exists(path))
                {
                    Console.WriteLine("That path exists already.");
                }
                else
                {
                    DirectoryInfo di = Directory.CreateDirectory(path);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("The process failed: {0}", ex.ToString());
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

            users = cw.ReadXmlUser();
            int count = 0;

            foreach (User u in users)
            {
                if (count >= 10)
                {
                    break;
                }
                Button b = new Button
                {
                    Content = u.Pseudo,
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
                MessageBoxResult msgr = MessageBox.Show("Voulez-vous supprimer cet utilisateur ?", "Supprimer un utilisateur", MessageBoxButton.YesNo);
                if (msgr.Equals(MessageBoxResult.Yes))
                {
                    users.Remove(currentUser);
                    cw.WriteXmlUser(users);
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
                if (u.Pseudo == v)
                {
                    return u;
                }
            }
            return null;
        }

        private void Btn_connexion_Click(object sender, RoutedEventArgs e)
        {
            LibraryWindow lw = new LibraryWindow(currentUser);
            lw.Show();
            this.Close();
        }
    }
}
