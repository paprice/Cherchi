﻿using MahApps.Metro.Controls;
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
            path = p;
            InitializeComponent();
        }

        private void Btn_Create_Click(object sender, RoutedEventArgs e)
        {
            List<User> list = User.ReadXmlUser(path);
            User us = new User(Txt_Pseudo.Text, @".\\users\\" + Txt_Pseudo.Text + ".xml", Txt_Name.Text);

            if (SinglePseudo(list, us.GetPseudo()))
            {
                list.Add(us);
                User.WriteXmlUser(list,path);
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
                if (u.GetPseudo() == pseudo)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
