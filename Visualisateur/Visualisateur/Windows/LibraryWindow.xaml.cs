
using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private List<FileInfo> filesInfo;
        private List<Video> videos;
        private int firstIndex;

        private int currentPage;

        public LibraryWindow(User u)
        {
            firstIndex = 0;
            filesInfo = new List<FileInfo>();
            videos = new List<Video>();
            user = u;
            InitializeComponent();
            CreateList();
            PrintList();
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

            TextBlock t = new TextBlock
            {
                Text = fileName,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 75, 0, 0),
                TextWrapping = TextWrapping.Wrap,
                Width = 130,
                Background = System.Windows.Media.Brushes.LightGray,


            };
            t.MouseUp += Tb_Click;

            Grid.SetColumn(t, col);
            Grid.SetRow(t, row);

            main.Children.Add(i);
            main.Children.Add(t);


        }

        private void CreateList()
        {
            userName = Environment.UserName;

            DirectoryInfo dirInfo = new DirectoryInfo("C:\\Users\\" + userName + "\\Videos\\");

            FileInfo[] fi = dirInfo.GetFiles("*.*");

            foreach (FileInfo f in fi)
            {
                if (!f.Extension.Equals(".ini"))
                    filesInfo.Add(f);
            }

            DirectoryInfo[] dirInfos = dirInfo.GetDirectories("*.*");

            foreach (DirectoryInfo di in dirInfos)
            {
                FileInfo[] fis = di.GetFiles("*.*");
                foreach (FileInfo f in fis)
                {
                    if (!f.Extension.Equals(".ini"))
                        filesInfo.Add(f);
                }
            }

        }

        private void PrintList()
        {
            int count = 0;
            for (int i = firstIndex; i < firstIndex + 20; i++)
            {
                if (i < filesInfo.Count)
                {
                    FileInfo files = filesInfo[i];

                    ShellFile shellFile = ShellFile.FromFilePath(files.FullName);
                    Bitmap shellThumb = shellFile.Thumbnail.ExtraLargeBitmap;

                    Video v = new Video(files.FullName, files.Name);
                    videos.Add(v);

                    CreateButton(shellThumb, count, files.Name);
                    count++;
                }
                else
                {
                    break;
                }

            }
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


        public void Tb_Click(object sender, RoutedEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;
            string path = "";
            foreach (Video v in videos)
            {
                if (v.FileName.Equals(tb.Text))
                {
                    path = v.FullPath;
                }
            }

            Process.Start(path);
        }


    }
}
