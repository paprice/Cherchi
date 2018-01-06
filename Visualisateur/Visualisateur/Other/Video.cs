using System.Windows.Controls;

namespace Visualisateur.Other
{
    class Video
    {
        public string FullPath { get; set; }

        public string FileName { get; set; }

        public Image Image { get; set; }

        public TextBlock TextBlock { get; set; }

        public Video()
        {

        }

        public Video(string fu, string fi)
        {
            FullPath = fu;
            FileName = fi;
        }

    }
}
