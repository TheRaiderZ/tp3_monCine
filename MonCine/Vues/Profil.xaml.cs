using MonCine.Data;
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

namespace MonCine.Vues
{
    /// <summary>
    /// Logique d'interaction pour Profil.xaml
    /// </summary>
    public partial class Profil : Window
    {

        public Abonne AbonneConnecte { get; set; }

        public List<Realisateur> Realisateurs = new List<Realisateur>();
        public List<Acteur> Acteurs = new List<Acteur>();
        public List<Film> FilmsVisionnés = new List<Film>();
        public Film SelectedFilm { get; set; }

        private DAL _dal;
        public Profil(DAL dal)
        {

            InitializeComponent();
            SetupStarsRating();
            DataContext = this;
            _dal = dal;
            GetUser();
            ReadEntities();
            PopulateListViews();
        }
        private void SetupStarsRating()
        {
            ratings.StarSize = 20;
            ratings.Value = 0.5M;
            //ratings1.NumberOfStars = 5;
            ratings.BackgroundColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF042A2B");
            ratings.StarForegroundColor = Brushes.Orange;
            ratings.StarOutlineColor = Brushes.DarkGray;
        }
        private void GetUser()
        {
            AbonneConnecte = App.Current.Properties["CurrentUser"] as Abonne;
            if (AbonneConnecte == null)
            {
                MessageBox.Show("Veuillez vous connecter");
                this.Close();
            }
            if (AbonneConnecte.Preferences == null)
            {
                AbonneConnecte.Preferences = new Preferences();
            }

        }

        private List<Film> GetFilmsVisionnes()
        {
            if (AbonneConnecte.Reservations == null)
            {
                AbonneConnecte.Reservations = new List<Reservation>();
            }
            foreach (var reservation in AbonneConnecte.Reservations)
            {
                if (reservation.Projection.DateDebut<DateTime.Now)
                {
                    FilmsVisionnés.Add(reservation.Projection.Film);
                }
            }
            return FilmsVisionnés;
        }
        
        private void PopulateListViews()
        {
            Realisateurs = Realisateurs.OrderBy(a => a.Nom).ToList();
            listeRealisateurs.ItemsSource = Realisateurs;
            Acteurs = Acteurs.OrderBy(a => a.Nom).ToList();
            listeActeurs.ItemsSource = Acteurs;
            listeCategories.ItemsSource = Enum.GetValues(typeof(Categorie));
            lstFilms.ItemsSource = GetFilmsVisionnes();

        }

        private void ReadEntities()
        {
            Realisateurs = _dal.ReadRealisateurs();
            Acteurs = _dal.ReadActeurs();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            EnregistrerChangements();
        }
        
        public void EnregistrerChangements()
        {
            
            _dal.UpdateAbonne(AbonneConnecte);
            MessageBox.Show("Enregistrement effectué");
        }

        private void listeCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AbonneConnecte == null)
            {
                return;
            }
            int nbCategoriesSelected = this.listeCategories.SelectedItems.Count;
            if (nbCategoriesSelected > Preferences.MAX_CATEGORIES)
            {
                this.listeCategories.SelectedItems.RemoveAt(Preferences.MAX_CATEGORIES - 1);
            }
            else
            {
                AbonneConnecte.Preferences.CategoriesFavoris = listeCategories.SelectedItems.Cast<Categorie>().ToList();
            }
        }

        private void listeActeurs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AbonneConnecte == null)
            {
                return;
            }
            int nbActeursSelected = this.listeActeurs.SelectedItems.Count;
            if (nbActeursSelected > Preferences.MAX_ACTEURS)
            {
                this.listeActeurs.SelectedItems.RemoveAt(Preferences.MAX_ACTEURS - 1);
            }
            else
            {
                AbonneConnecte.Preferences.ActeursFavoris = listeActeurs.SelectedItems.Cast<Acteur>().ToList();
            }
        }

        private void listeRealisateurs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AbonneConnecte == null)
            {
                return;
            }
            int nbRealisateursSelected = this.listeRealisateurs.SelectedItems.Count;
            if (nbRealisateursSelected > Preferences.MAX_REALISATEURS)
            {
                this.listeRealisateurs.SelectedItems.RemoveAt(Preferences.MAX_REALISATEURS - 1);
            }
            else
            {
                AbonneConnecte.Preferences.RealisateursFavoris = listeRealisateurs.SelectedItems.Cast<Realisateur>().ToList();
            }
        }

        private void sliderNote_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            txtCurrentNote.Content = sliderNote.Value.ToString();

            if (ratings != null)
            {
                ratings.Value = (decimal)sliderNote.Value/2;;

            }

        }

        private void btnSaveNote_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedFilm!=null)
            {
                if (SelectedFilm.Notes==null)
                {
                    SelectedFilm.Notes = new List<int>();
                }
                SelectedFilm.Notes.Add((int)sliderNote.Value);
                _dal.UpdateFilm(SelectedFilm);
            }
        }
        
        //Barre de recherche
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            lstFilms.SelectedItem = null;
            SelectedFilm = null;
            
            if (String.IsNullOrWhiteSpace(txtRecherche.Text))
            {
                lstFilms.ItemsSource = AbonneConnecte.Reservations;
                return;
            }
            List<Film> resultat = new List<Film>();
            resultat = FiltrerFilmsParNom(FilmsVisionnés, txtRecherche.Text);
            lstFilms.ItemsSource = resultat;

        }
        public static List<Film> FiltrerFilmsParNom(List<Film> films, string filtre)
        {
            List<Film> resultat = new List<Film>();

            resultat = films.Where(x => x.NomCorrespondFiltre(filtre)).ToList();
            return resultat;
        }
    }
}
