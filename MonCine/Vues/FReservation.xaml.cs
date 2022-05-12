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
    /// Logique d'interaction pour FReservation.xaml
    /// </summary>
    public partial class FReservation : Page
    {
        public List<Projection> Projections = new List<Projection>();
        public Projection SelectedProjection { get; set; }
        private DAL _dal;
        public FReservation(DAL dal)
        {
            InitializeComponent();
            _dal = dal;
            InitializeList();
        }

        private void InitializeList()
        {
            Projections = _dal.ReadProjections();
            listeProjections.ItemsSource = Projections; listeProjections.DataContext = Projections;
        }

        private void listeProjections_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedProjection = (Projection)listeProjections.SelectedItem;
            if (SelectedProjection == null) { return; }

            txtPlaces.Content = SelectedProjection.NbPlaces.ToString();
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
            int places = 1;

            if (SelectedProjection.NbPlaces < places)
            {
                MessageBox.Show("Il ne reste pas assez de place dans cette projection.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Reservation reservation = new Reservation(DateTime.Now, null, 1, SelectedProjection);
                _dal.AddReservation(reservation);
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }
    }
}
