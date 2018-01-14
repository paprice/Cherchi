
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
        private List<VideoTemplate> videos;
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


            videos = new List<VideoTemplate>();

            see = new VideosSee(user.GetPath());
            sees = see.ListSeeVideo();

            CreateList("");
            PrintList();
        }

        private VideoTemplate CreateButton(Bitmap bit, int count, string fileName, string path)
        {
            int col = count % 5;
            int row = count / 5;

            VideoTemplate vt = new VideoTemplate(GetImage(bit), fileName, path)
            {
                Height = 175
            };

            if (!sees.Contains(fileName))
            {
                vt.name.Background = System.Windows.Media.Brushes.LightGray;
            }
            else
            {
               vt.name.Background = System.Windows.Media.Brushes.LightGreen;
            }

            vt.MouseUp += Tb_Click;
            vt.MouseMove += Tb_Over;

            wrap.Children.Add(vt);

            return vt;
        }

        private void CreateList(string filters)
        {
            using (new WaitCursor())
            {
                filesInfo = new List<FileInfo>();

                DirectoryInfo dirInfo = new DirectoryInfo(path);

                FileInfo[] fi = dirInfo.GetFiles("*"+filters+"*.*", SearchOption.AllDirectories);

                foreach (FileInfo f in fi)
                {
                    /*
                     if (f.Extension.Equals(".avi") || f.Extension.Equals(".mkv") || f.Extension.Equals(".mp4"))
                         filesInfo.Add(f);*/
                    var contentType = MimeTypes.GetContentType(f.FullName);
                    if (contentType.StartsWith("video"))
                    {
                        filesInfo.Add(f);
                    }
                }

                Comparison<FileInfo> comparison = new Comparison<FileInfo>(delegate (FileInfo a, FileInfo b)
                {
                    return String.Compare(a.Name, b.Name);
                });

                filesInfo.Sort(comparison);
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

                        //Video v = new Video(files.FullName, files.Name);
                        //videos.Add(v);

                        videos.Add(CreateButton(shellThumb, count, files.Name, files.FullName));
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
            VideoTemplate vt = (VideoTemplate)sender;
            string path = "";
            foreach (VideoTemplate v in videos)
            {
                if (v.name.Text.Equals(vt.name.Text))
                {
                    path = v.path;

                    see.SaveVideo(v.name.Text);
                    break;
                }
            }
            Process.Start(path);
            PrintList();
        }

        private void RemoveVideos()
        {
            foreach (VideoTemplate v in videos)
            {
                wrap.Children.Remove(v);
                //main.Children.Remove(v.TextBlock);
            }
            videos.RemoveRange(0, videos.Count);

        }

        private void Btn_previousPage_Click(object sender, RoutedEventArgs e)
        {

            if (firstIndex >= 20)
            {
                ChangePage(currentPage-1);
                PrintList();
            }

        }

        private void Btn_firstPage_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;
            ChangePage(1);
            PrintList();

        }

        private void ChangePage(int newPage)
        {
            currentPage = newPage;
            lbl_currentPage.Content = currentPage;
            firstIndex = (currentPage - 1) * 20;
        }

        private void Btn_nextPage_Click(object sender, RoutedEventArgs e)
        {
            if (filesInfo.Count > firstIndex + 20)
            {
                ChangePage(currentPage+1);
                PrintList();
            }
        }

        private void Btn_lastPage_Click(object sender, RoutedEventArgs e)
        {
            int t = filesInfo.Count % 20;
            if (t != 0)
            {
                int i = (filesInfo.Count / 20);
                ChangePage(i+1);
                PrintList();
            }
        }

        private void Btn_changePath_Click(object sender, RoutedEventArgs e)
        {
            txt_currentPath.Text = txt_newPath.Text;
            txt_newPath.Text = "";
            path = txt_currentPath.Text;
            ChangePage(1);
            CreateList("");
            PrintList();

            if (chk_lastPath.IsChecked.Value)
            {
                List<User> userList = User.ReadXmlUser(usersPath);
                User u = userList.Find(x => user.Equals(x));
                userList.Remove(user);
                u.SetLastVideoPath(txt_currentPath.Text);
                userList.Add(u);
                User.WriteXmlUser(userList, usersPath);
            }
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

        private void Btn_NameSearch_Click(object sender, RoutedEventArgs e)
        {
            if(txt_NameSearch.Text != "")
            {
                ChangePage(1);
                CreateList(txt_NameSearch.Text);
                PrintList();
                txt_NameSearch.Text = "";
                tb_filters.IsChecked = false;
            }
        }

        private void Btn_NoFilter_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(1);
            CreateList("");
            PrintList();
            txt_NameSearch.Text = "";
            tb_filters.IsChecked = false;
        }
    }
}
