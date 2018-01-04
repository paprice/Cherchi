
using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Visualisateur.Other;

namespace Visualisateur.Windows
{
    /// <summary>
    /// Interaction logic for LibraryWindow.xaml
    /// </summary>
    public partial class LibraryWindow : Window
    {
        private User user;
        private string userName;
        private List<FileInfo> videos;

        public LibraryWindow(User u)
        {
            videos = new List<FileInfo>();
            user = u;
            InitializeComponent();

            userName = Environment.UserName;

            DirectoryInfo dirInfo = new DirectoryInfo("C:\\Users\\" + userName + "\\Videos\\");

            FileInfo[] fi = dirInfo.GetFiles("*.*");

            foreach (FileInfo f in fi)
            {
                if (!f.Extension.Equals(".ini"))
                    videos.Add(f);
            }

            DirectoryInfo[] dirInfos = dirInfo.GetDirectories("*.*");

            foreach (DirectoryInfo di in dirInfos)
            {
                FileInfo[] fis = di.GetFiles("*.*");
                foreach (FileInfo f in fis)
                {
                    if (!f.Extension.Equals(".ini"))
                        videos.Add(f);
                }
            }

            int count = 0;
            foreach (FileInfo files in videos)
            {
                //testFile.Content += files.Name + "\n";
                ShellFile shellFile = ShellFile.FromFilePath(files.FullName);
                Bitmap shellThumb = shellFile.Thumbnail.ExtraLargeBitmap;

                CreateButton(shellThumb, count, files.Name);
                count++;


            }

            //C:\Users\despa\Videos\XSplit Recordings

        }

        private void CreateButton(Bitmap bit, int count, string fileName)
        {
            int col = count % 5;
            int row = count / 5;

            System.Windows.Controls.Image i = new System.Windows.Controls.Image
            {
                Source = GetImage(bit),
                Width = 130,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top
            };

            Grid.SetColumn(i, col);
            Grid.SetRow(i, row);

            //double witdh = main.ColumnDefinitions[col].ActualWidth;



            TextBlock l = new TextBlock
            {
                Text = fileName,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                TextWrapping = TextWrapping.Wrap,
                Width = 130,
                Background = 
            };

            Grid.SetColumn(l, col);
            Grid.SetRow(l, row);

            main.Children.Add(i);
            main.Children.Add(l);


        }

        private BitmapImage GetImage(Bitmap src)
        {
            BitmapImage image = new BitmapImage();

            MemoryStream ms = new MemoryStream();
            src.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();

            return image;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }

    }
}
