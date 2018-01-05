using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visualisateur.Other
{
    class Video
    {
        public string FullPath { get; set; }

        public string FileName { get; set; }

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
