
using System.Windows;
using Visualisateur.Other;
using Visualisateur.Windows;

namespace Visualisateur.Windows
{
    /// <summary>
    /// Interaction logic for LibraryWindow.xaml
    /// </summary>
    public partial class LibraryWindow : Window
    {
        private User user;

        public LibraryWindow(User u)
        {
            user = u;
            InitializeComponent();
        }
    }
}
