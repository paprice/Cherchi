using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Visualisateur.Other
{
    public class VideosSee
    {
        private string path;
        private List<string> sees;

        public VideosSee()
        {

        }

        public VideosSee(string p)
        {
            path = p;
            sees = new List<string>();
        }

        public void SaveVideo(string name)
        {
            sees.Add(name);
            XmlDocument doc = new XmlDocument();
            XmlNode rootNode = doc.CreateElement("videos");
            doc.AppendChild(rootNode);

            foreach (string s in sees)
            {
                XmlNode video = doc.CreateElement("video");
                rootNode.AppendChild(video);
                video.InnerText = s;
            }

            doc.Save(path);
        }

        public List<string> ListSeeVideo()
        {
            List<string> s = new List<string>();
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(path);
            }
            catch (Exception ex)
            {
                Console.Out.Write(ex);
                doc.AppendChild(doc.CreateElement("videos"));

                doc.Save(path);
            }

            foreach(XmlNode v in doc.FirstChild)
            {
                s.Add(v.InnerText);
            }

            sees = s;

            return s;
        }
    }
}
