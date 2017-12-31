using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Visualisateur.Windows;

namespace Visualisateur
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string path = @".\\users\\";
        private CreateWindow cw;


        public MainWindow()
        {
            cw = new Windows.CreateWindow(path);
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
            cw.Show();

            CreateButton();

        }

        private void CreateButton()
        {
            List<User.User> users = cw.ReadXmlUser();
            int count = 0;

            foreach (User.User u in users)
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


                Grid.SetColumn(b, count % 5);
                Grid.SetRow(b, count / 5);
                usersGrid.Children.Add(b);

                count++;
            }
        }


    }
}
