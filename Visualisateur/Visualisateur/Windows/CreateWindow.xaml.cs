using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Xml;
using Visualisateur.Other;

namespace Visualisateur.Windows
{
    /// <summary>
    /// Interaction logic for CreateWindow.xaml
    /// </summary>
    public partial class CreateWindow : MetroWindow
    {
        private string path;

        public CreateWindow(string p)
        {
            path = p + "users.xml";
            InitializeComponent();
        }

        private void Btn_Create_Click(object sender, RoutedEventArgs e)
        {
            List<User> list = ReadXmlUser();
            User us = new User(Txt_Pseudo.Text, @".\\users\\" + Txt_Pseudo.Text + ".xml", Txt_Name.Text);

            if (SinglePseudo(list, us.Pseudo))
            {
                list.Add(us);
                WriteXmlUser(list);
                this.Close();
            }
            else
            {
                MessageBox.Show("Pseudonyme déjà utilisé", "Pseudonymne", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Txt_Pseudo.Text = "";
                Txt_Name.Text = "";

            }

        }

        private bool SinglePseudo(List<User> list, string pseudo)
        {
            foreach (User u in list)
            {
                if (u.Pseudo == pseudo)
                {
                    return false;
                }
            }
            return true;
        }

        public void WriteXmlUser(List<User> list)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode rootNode = doc.CreateElement("users");
            doc.AppendChild(rootNode);

            foreach (User u in list)
            {
                XmlNode user = doc.CreateElement("user");
                rootNode.AppendChild(user);

                XmlNode pseudoNode = doc.CreateElement("pseudo");
                pseudoNode.InnerText = u.Pseudo;
                user.AppendChild(pseudoNode);

                XmlNode nameNode = doc.CreateElement("name");
                nameNode.InnerText = u.Name;
                user.AppendChild(nameNode);

                XmlNode pathNode = doc.CreateElement("path");
                pathNode.InnerText = u.Path;
                user.AppendChild(pathNode);
            }

            doc.Save(path);

        }

        public List<User> ReadXmlUser()
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
                            u.Pseudo = n.InnerText;
                            break;
                        case "name":
                            u.Name = n.InnerText;
                            break;
                        case "path":
                            u.Path = n.InnerText;
                            break;
                    }
                }
                list.Add(u);
            }
            return list;
        }

    }
}
