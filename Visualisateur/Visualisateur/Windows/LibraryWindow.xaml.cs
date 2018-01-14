
using MahApps.Metro.Controls;
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
    public partial class LibraryWindow : MetroWindow
    {
        private string userName;
        private List<FileInfo> filesInfo;
        private List<Video> videos;
        private List<string> sees;
        private int firstIndex;
        private string path;
        private string usersPath;
        private User user;

        private int currentPage;

        private VideosSee see;

        public LibraryWindow(User u, string up)
        {
            InitializeComponent();
            userName = Environment.UserName;
            user = u;
            if (user.GetLastPath() != "")
            {
                path = user.GetLastPath();
            }
            else
            {
                path = "C:\\Users\\" + userName + "\\Videos\\";
            }

            usersPath = up;


            txt_currentPath.Text = path;

            this.Title = "Librairie de " + user.GetName();
            firstIndex = 0;
            currentPage = 1;


            videos = new List<Video>();

            see = new VideosSee(user.GetPath());
            sees = see.ListSeeVideo();

            CreateList();
            PrintList();
        }

        private void CreateButton(Bitmap bit, int count, string fileName, Video v)
        {
            int col = count % 5;
            int row = count / 5;

            System.Windows.Controls.Image i = new System.Windows.Controls.Image
            {
                Source = GetImage(bit),
                Width = 130,
                Height = 100,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top
            };
            v.Image = i;
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
            };

            if (!sees.Contains(fileName))
            {
                t.Background = System.Windows.Media.Brushes.LightGray;
            }
            else
            {
                t.Background = System.Windows.Media.Brushes.LightGreen;
            }

            t.MouseUp += Tb_Click;
            t.MouseMove += Tb_Over;
            //t.MouseLeave += Tb_Exit;

            v.TextBlock = t;
            Grid.SetColumn(t, col);
            Grid.SetRow(t, row);

            main.Children.Add(i);
            main.Children.Add(t);


        }

        private void CreateList()
        {
            using (new WaitCursor())
            {
                filesInfo = new List<FileInfo>();

                DirectoryInfo dirInfo = new DirectoryInfo(path);

                FileInfo[] fi = dirInfo.GetFiles("*.*", SearchOption.AllDirectories);

                foreach (FileInfo f in fi)
                {
                    if (f.Extension.Equals(".avi") || f.Extension.Equals(".mkv") || f.Extension.Equals(".mp4"))
                        filesInfo.Add(f);
                }

                Comparison<FileInfo> comparison = new Comparison<FileInfo>(delegate (FileInfo a, FileInfo b)
                {
                    return String.Compare(a.Name, b.Name);
                });

                filesInfo.Sort(comparison);
            }
        }

        private void GetFileInfo(DirectoryInfo[] dirs)
        {

            foreach (DirectoryInfo di in dirs)
            {
                FileInfo[] fis = di.GetFiles("*.*");
                foreach (FileInfo f in fis)
                {
                    if (!f.Extension.Equals(".ini"))
                        filesInfo.Add(f);
                }
                GetFileInfo(di.GetDirectories());
            }
        }

        private void PrintList()
        {
            using (new WaitCursor())
            {
                RemoveVideos();
                int count = 0;
                for (int i = firstIndex; i < firstIndex + 20; i++)
                {
                    if (i < filesInfo.Count)
                    {
                        FileInfo files = filesInfo[i];

                        ShellFile shellFile = ShellFile.FromFilePath(files.FullName);
                        Bitmap shellThumb = shellFile.Thumbnail.ExtraLargeBitmap;

                        shellFile.Dispose();

                        Video v = new Video(files.FullName, files.Name);
                        videos.Add(v);

                        CreateButton(shellThumb, count, files.Name, v);
                        count++;
                    }
                    else
                    {
                        break;
                    }

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

        private void Tb_Over(object sender, RoutedEventArgs e)
        {
            if (System.Windows.Forms.Cursor.Current != System.Windows.Forms.Cursors.Hand)
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Hand;
        }

        private void Tb_Click(object sender, RoutedEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;
            string path = "";
            foreach (Video v in videos)
            {
                if (v.FileName.Equals(tb.Text))
                {
                    path = v.FullPath;

                    see.SaveVideo(v.FileName);
                }
            }
            Process.Start(path);
            PrintList();
        }

        private void RemoveVideos()
        {
            foreach (Video v in videos)
            {
                main.Children.Remove(v.Image);
                main.Children.Remove(v.TextBlock);
            }
            videos.RemoveRange(0, videos.Count);

        }

        private void Btn_previousPage_Click(object sender, RoutedEventArgs e)
        {

            if (firstIndex >= 20)
            {
                firstIndex -= 20;
                PrintList();
                lbl_currentPage.Content = --currentPage;
            }

        }

        private void Btn_firstPage_Click(object sender, RoutedEventArgs e)
        {
            firstIndex = 0;
            currentPage = 1;
            lbl_currentPage.Content = currentPage;
            PrintList();

        }

        private void Btn_nextPage_Click(object sender, RoutedEventArgs e)
        {
            if (filesInfo.Count > firstIndex + 20)
            {
                firstIndex += 20;
                lbl_currentPage.Content = ++currentPage;
                PrintList();
            }
        }

        private void Btn_lastPage_Click(object sender, RoutedEventArgs e)
        {
            int t = filesInfo.Count % 20;
            if (t != 0)
            {
                int i = (filesInfo.Count / 20);
                currentPage = i + 1;
                firstIndex = i * 20;
                lbl_currentPage.Content = currentPage;
                PrintList();
            }
        }

        private void Btn_changePath_Click(object sender, RoutedEventArgs e)
        {
            txt_currentPath.Text = txt_newPath.Text;
            txt_newPath.Text = "";
            path = txt_currentPath.Text;
            firstIndex = 0;
            currentPage = 1;
            lbl_currentPage.Content = currentPage;
            CreateList();
            PrintList();

            List<User> userList = User.ReadXmlUser(usersPath);
            User u = userList.Find(x => user.Equals(x));
            userList.Remove(user);
            u.SetLastVideoPath(txt_currentPath.Text);
            userList.Add(u);
            User.WriteXmlUser(userList, usersPath);
        }

        private void CheckRepository_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    txt_newPath.Text = dialog.SelectedPath;
                }
            }
        }
    }
}
