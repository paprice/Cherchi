using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Visualisateur.Other
{
    public class User
    {
        private string pseudo;
        private string name;
        private string path;
        private string lastVideoPath;

        public User()
        {

        }

        public User(string p, string pa, string n)
        {
            pseudo = p;
            name = n;
            path = pa;
            lastVideoPath = "";
        }

        #region Getter and Setter
        public string GetName()
        {
            return name;
        }

        public string GetPath()
        {
            return path;
        }

        public string GetPseudo()
        {
            return pseudo;
        }

        public string GetLastPath()
        {
            return lastVideoPath;
        }

        public void SetLastVideoPath(string pathLast)
        {
            lastVideoPath = pathLast;
        }
        #endregion

        public static void WriteXmlUser(List<User> list, string path)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode rootNode = doc.CreateElement("users");
            doc.AppendChild(rootNode);

            foreach (User u in list)
            {
                XmlNode user = doc.CreateElement("user");
                rootNode.AppendChild(user);

                XmlNode pseudoNode = doc.CreateElement("pseudo");
                pseudoNode.InnerText = u.pseudo;
                user.AppendChild(pseudoNode);

                XmlNode nameNode = doc.CreateElement("name");
                nameNode.InnerText = u.name;
                user.AppendChild(nameNode);

                XmlNode pathNode = doc.CreateElement("pathFile");
                pathNode.InnerText = u.path;
                user.AppendChild(pathNode);

                XmlNode videosNode = doc.CreateElement("videosPath");
                videosNode.InnerText = u.lastVideoPath;
                user.AppendChild(videosNode);
            }

            doc.Save(path);

        }

        public static List<User> ReadXmlUser(string path)
        {
            List<User> list = new List<User>();
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(path);
            }
            catch (Exception ex)
            {
                Console.Out.Write(ex);
                doc.AppendChild(doc.CreateElement("users"));

                doc.Save(path);
            }

            foreach (XmlNode us in doc.FirstChild)
            {
                User u = new User();
                foreach (XmlNode n in us)
                {
                    switch (n.Name)
                    {
                        case "pseudo":
                            u.pseudo = n.InnerText;
                            break;
                        case "name":
                            u.name = n.InnerText;
                            break;
                        case "pathFile":
                            u.path = n.InnerText;
                            break;
                        case "videosPath":
                            u.lastVideoPath = n.InnerText;
                            break;
                    }
                }
                list.Add(u);
            }
            return list;
        }

        public override bool Equals(object obj)
        {
            User u = (User)obj;
            return (pseudo == u.pseudo && name == u.name && path == u.path);
        }

        public override int GetHashCode()
        {
            var hashCode = 538807162;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(pseudo);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(path);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(lastVideoPath);
            return hashCode;
        }
    }
}
