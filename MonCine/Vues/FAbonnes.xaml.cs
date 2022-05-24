using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MonCine.Data;

namespace MonCine.Vues
{
    /// <summary>
    /// Logique d'interaction pour FAbonnes.xaml
    /// </summary>
    public partial class FAbonnes : Page
    {
        private List<Abonne> Abonnes;
        private List<Film> Films;
        public Abonne SelectedAbonne { get; set; }
        private DAL _dal;
        public FAbonnes(DAL dal)
        {
            InitializeComponent();
            _dal = dal;
            readListe();
            fillLists();
        }

        private void readListe()
        {
            Abonnes = _dal.ReadAbonnes();
            Films = _dal.ReadFilms();
        }

        private void fillLists()
        {
            listAbonnes.ItemsSource = Abonnes.Where(x => x.isAdmin == false);
            listeFilms.ItemsSource = Films; listeFilms.DataContext = Films;
            listeType.ItemsSource = Enum.GetValues(typeof(TypeRecompense));
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            enregistrement();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void listAbonnes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedAbonne = (Abonne)listAbonnes.SelectedItem;
            if (SelectedAbonne == null) { return; }
        }

        private void enregistrement()
        {
            if (listAbonnes.SelectedItem != null)
            {
                if (listeFilms.SelectedItem != null)
                {
                    if (listeType.SelectedItem != null)
                    {
                        TypeRecompense type = (TypeRecompense)listeType.SelectedItem;
                        Film film = (Film)listeFilms.SelectedItem;
                        Recompense newRecompense = new Recompense( type, film);
                        SelectedAbonne.Recompenses.Add(newRecompense);
                        _dal.AddRecompense(SelectedAbonne);
                        MessageBox.Show("Enregistrement effectué");
                    }
                    else
                    {
                        MessageBox.Show("Il faut que vous sélectionner un type.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Il faut que vous sélectionner un film.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Il faut que vous sélectionner un abonné.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

    }
}
