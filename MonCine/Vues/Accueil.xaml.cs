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
    public partial class Accueil : Page
    {
        private DAL dal;
        public Accueil()
        {
            InitializeComponent();
            dal = new DAL();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FAbonnes frmAbonnes = new FAbonnes(dal);

            this.NavigationService.Navigate(frmAbonnes);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            FProjections frmProjections = new FProjections(dal);
            
            this.NavigationService.Navigate(frmProjections);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            FFilms frmFilms = new FFilms(dal);

            this.NavigationService.Navigate(frmFilms);
        }
    }
}
