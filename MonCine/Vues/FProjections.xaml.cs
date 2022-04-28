using MonCine.Data;
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
using System.Windows.Shapes;

namespace MonCine.Vues
{
    /// <summary>
    /// Logique d'interaction pour Projections.xaml
    /// </summary>
    public partial class FProjections : Page
    {
        public List<Projection> Projections = new List<Projection>();
        public Projection SelectedProjection { get; set; }
        private DAL _dal;

        public FProjections(DAL dal)
        {
            InitializeComponent();
            _dal = dal;
            InitializeLists();
        }

        private void InitializeLists()
        {
            Projections = _dal.ReadProjections();
            listeProjections.ItemsSource = Projections;
            listeSalle.ItemsSource = Enum.GetValues(typeof(Salle));
        }

        private void listeProjections_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedProjection = (Projection)listeProjections.SelectedItem;
            if (SelectedProjection == null)
            {
                return;
            }
            dtpDebut.SelectedDate = SelectedProjection.DateDebut;
            dtpFin.SelectedDate = SelectedProjection.DateFin;
            listeSalle.SelectedItem = SelectedProjection.Salle;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Enregistrer();
        }

        private void Enregistrer()
        {
            if (SelectedProjection.DateDebut == null || SelectedProjection.DateFin == null)
            {
                MessageBox.Show("Vous devez entrer les dates de début et de fin.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (SelectedProjection.DateDebut > SelectedProjection.DateFin)
            {
                MessageBox.Show("Vous ne pouvez pas mettre la date de fin de projection avant le début de la projection.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                _dal.UpdateProjection(SelectedProjection);
            }
        }

        private void btnCreer_Click(object sender, RoutedEventArgs e)
        {
            FCreerProjection fCreerProjection = new FCreerProjection(_dal);

            this.NavigationService.Navigate(fCreerProjection);
        }

        private void listeSalle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedProjection == null)
            {
                return;
            }
            ((Projection)listeProjections.SelectedItem).Salle = (Salle)listeSalle.SelectedItem;
        }

        private void dtpDebut_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedProjection == null)
            {
                return;
            }
            ((Projection)listeProjections.SelectedItem).DateDebut = (DateTime)dtpDebut.SelectedDate;
        }

        private void dtpFin_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedProjection == null)
            {
                return;
            }
            ((Projection)listeProjections.SelectedItem).DateFin = (DateTime)dtpFin.SelectedDate;

        }
    }
}
