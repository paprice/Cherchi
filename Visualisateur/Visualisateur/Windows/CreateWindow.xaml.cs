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
        public CreateWindow()
        {
            InitializeComponent();
        }

        private void Btn_Create_Click(object sender, RoutedEventArgs e)
        {
            string path = @".\\users\\users.xml";

            List<User.User> list = ReadXml(path);
            User.User us = new User.User(Txt_Pseudo.Text, @".\\users\\" + Txt_Pseudo.Text + ".xml", Txt_Name.Text);

            if (SinglePseudo(list,us.GetPseudo()))
            {
                list.Add(us);
                WriteXml(path, list);
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
                if (u.GetPseudo().Equals(pseudo))
                {
                    return false;
                }
            }
            return true;
        }

        private void WriteXml(string path, List<User.User> list)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode rootNode = doc.CreateElement("users");
            doc.AppendChild(rootNode);

            foreach (User.User u in list)
            {
                XmlNode user = doc.CreateElement("user");
                rootNode.AppendChild(user);

                XmlNode pseudoNode = doc.CreateElement("pseudo");
                pseudoNode.InnerText = u.GetPseudo();
                user.AppendChild(pseudoNode);

                XmlNode nameNode = doc.CreateElement("name");
                nameNode.InnerText = u.GetName();
                user.AppendChild(nameNode);

                XmlNode pathNode = doc.CreateElement("path");
                pathNode.InnerText = u.GetPath();
                user.AppendChild(pathNode);
            }

            doc.Save(path);

        }

        private List<User.User> ReadXml(string path)
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
