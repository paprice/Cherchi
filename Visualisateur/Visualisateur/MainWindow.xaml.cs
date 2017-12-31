using MahApps.Metro.Controls;
using System;
using System.IO;
using System.Windows;


namespace Visualisateur
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CreateDirectory();
        }

        private void CreateDirectory()
        {
            string path = @".\\users\\";

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
            Windows.CreateWindow cw = new Windows.CreateWindow();
            cw.Show();
        }
    }
}
