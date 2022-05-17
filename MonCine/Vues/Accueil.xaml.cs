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
        public Abonne AbonneConnecte;
        public Accueil()
        {
            InitializeComponent();
            dal = new DAL();
            AbonneConnecte = App.Current.Properties["CurrentUser"] as Abonne;
            if (AbonneConnecte.isAdmin)
            {
                this.btnProjection.Content = "Gestion des projections";
            }
            else
            {
                this.btnProjection.Content = "Gestion des Réservations";
                this.btnAbonne.Visibility = Visibility.Collapsed;
                this.btnFilms.Visibility = Visibility.Collapsed;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FAbonnes frmAbonnes = new FAbonnes(dal);

            this.NavigationService.Navigate(frmAbonnes);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            if (AbonneConnecte.isAdmin)
            {
                FProjections frmProjections = new FProjections(dal);

                this.NavigationService.Navigate(frmProjections);
            }
            else
            {
                FReservation frmReservation = new FReservation(dal);
                this.NavigationService.Navigate(frmReservation);
            }

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            FFilms frmFilms = new FFilms(dal);

            this.NavigationService.Navigate(frmFilms);
        }

        //private void Button_Click_3(object sender, RoutedEventArgs e)
        //{
        //    Profil profil = new Profil(dal);
        //    profil.ShowDialog();
        //}

        private void btnProfil_Click(object sender, RoutedEventArgs e)
        {
            Profil profil = new Profil(dal);
            profil.ShowDialog();
        }
    }
}
