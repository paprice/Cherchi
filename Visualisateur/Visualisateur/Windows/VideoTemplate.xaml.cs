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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Visualisateur.Windows
{
    /// <summary>
    /// Interaction logic for VideoTemplate.xaml
    /// </summary>
    public partial class VideoTemplate : UserControl
    {
        public string path;

        public VideoTemplate()
        {
            
        }

        public VideoTemplate(BitmapImage bitmapImage, string fileName, string p)
        {
            InitializeComponent();
            path = p;

            thumbnail.Source = bitmapImage;
            thumbnail.Width = 130;
            thumbnail.Height = 100;
            //HorizontalAlignment = HorizontalAlignment.Center,
            //VerticalAlignment = VerticalAlignment.Top

            name.Text = fileName;
            name.TextWrapping = TextWrapping.Wrap;
            name.Width = 130;
        }
    }
}
