using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Xml;
using Visualisateur.User;

namespace Visualisateur.Windows
{
    /// <summary>
    /// Interaction logic for CreateWindow.xaml
    /// </summary>
    public partial class CreateWindow : Window
    {
        private string path;

        public CreateWindow(string p)
        {
            path = p + "users.xml";
            InitializeComponent();
        }

        private void Btn_Create_Click(object sender, RoutedEventArgs e)
        {
            List<User.User> list = ReadXmlUser();
            User.User us = new User.User(Txt_Pseudo.Text, @".\\users\\" + Txt_Pseudo.Text + ".xml", Txt_Name.Text);

            if (SinglePseudo(list,us.Pseudo))
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

        private bool SinglePseudo(List<User.User> list, string pseudo)
        {
            foreach (User.User u in list)
            {
                if (u.Pseudo == pseudo)
                {
                    return false;
                }
            }
            return true;
        }

        private void WriteXmlUser( List<User.User> list)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode rootNode = doc.CreateElement("users");
            doc.AppendChild(rootNode);

            foreach (User.User u in list)
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

        public List<User.User> ReadXmlUser()
        {
            List<User.User> list = new List<User.User>();
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

            foreach (XmlNode n in doc.DocumentElement.ChildNodes)
            {
                string pseudo = n.SelectSingleNode("/users/user/pseudo").InnerText;
                string pathName = n.SelectSingleNode("/users/user/path").InnerText;
                string name = n.SelectSingleNode("/users/user/name").InnerText;

                User.User u = new User.User(pseudo, pathName, name);
                list.Add(u);
            }

            return list;
        }

    }
}
