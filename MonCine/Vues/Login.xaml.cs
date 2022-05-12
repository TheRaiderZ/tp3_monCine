using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MonCine.Data;

namespace MonCine.Vues
{
    /// <summary>
    /// Logique d'interaction pour Accueil.xaml
    /// </summary>
    public partial class Login : Page
    {
        private DAL dal;
        private Abonne selectedAccount;
        
        public Login()
        {
            InitializeComponent();
            dal = new DAL();
            PopulateControls();
        }
        private void PopulateControls() 
        {
            this.listeComptes.ItemsSource = dal.ReadAbonnes();
            
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (selectedAccount==null)
            {
                MessageBox.Show("Vous devez sélectionner un compte!");
                return;
            }
            App.Current.Properties.Add("CurrentUser", selectedAccount);
            
            this.NavigationService.Navigate(new Accueil());
        }

        private void listeComptes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedAccount = (Abonne)listeComptes.SelectedItem;
        }
    }
}
