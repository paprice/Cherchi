using System.Collections.Generic;
using System.Windows;
using System.Xml;

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
            List<User.User> list = new List<User.User>();


            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            foreach (XmlNode n in doc.DocumentElement.ChildNodes)
            {
                string pseudo = n.SelectSingleNode("/pseudo").InnerText;
                string pathName = n.SelectSingleNode("/pseudo/path").InnerText;
                string name = n.SelectSingleNode("/pseudo/name").InnerText;

                User.User u = new User.User(pseudo,pathName,name);
                list.Add(u);
            }

            User.User us = new User.User(Txt_Pseudo.GetLineText(0), @".\\users\\" + Txt_Pseudo + ".xml", Txt_Name.GetLineText(0));

        }
    }
}
