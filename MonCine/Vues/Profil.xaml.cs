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

        public Abonne Abonne { get; set; }

        public List<Realisateur> Realisateurs = new List<Realisateur>();
        public List<Acteur> Acteurs = new List<Acteur>();
        //public List<Film> FilmsVisionnés = new List<Film>();
        public Film SelectedFilm { get; set; }

        private DAL _dal;
        public Profil(DAL dal)
        {

            InitializeComponent();
            ratings1.StarSize = 20;
            ratings1.Value = 0.5M;
            //ratings1.NumberOfStars = 5;
            ratings1.BackgroundColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF042A2B");
            ratings1.StarForegroundColor = Brushes.Orange;
            ratings1.StarOutlineColor = Brushes.DarkGray;
            DataContext = this;
            _dal = dal;
            GetUser();
            ReadEntities();
            PopulateListViews();
        }
        private void GetUser()
        {
            Abonne = App.Current.Properties["CurrentUser"] as Abonne;
            if (Abonne == null)
            {
                MessageBox.Show("Veuillez vous connecter");
                this.Close();
            }
            if (Abonne.Preferences == null)
            {
                Abonne.Preferences = new Preferences();
            }

        }

        private List<Film> GetFilmsVisionnes()
        {
            List<Film> filmsVisionnes = new List<Film>();
            foreach (var film in Abonne.FilmVisionnés)
            {
                if (film.DateProjection<DateTime.Now)
                {
                    filmsVisionnes.Add(film);
                }
            }
            return filmsVisionnes;
        }
        
        private void PopulateListViews()
        {
            //listeFilms.ItemsSource = Films; listeFilms.DataContext = Films;
            Realisateurs = Realisateurs.OrderBy(a => a.Nom).ToList();
            listeRealisateurs.ItemsSource = Realisateurs;
            Acteurs = Acteurs.OrderBy(a => a.Nom).ToList();
            listeActeurs.ItemsSource = Acteurs;
            listeCategories.ItemsSource = Enum.GetValues(typeof(Categorie));
            lstFilms.ItemsSource = GetFilmsVisionnes();

        }

        private void ReadEntities()
        {
            //Films = _dal.ReadFilms();
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
            //TODO: Enregistrer les changements
            
            _dal.UpdateAbonne(Abonne);
        }

        private void listeCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Abonne == null)
            {
                return;
            }
            if (this.listeCategories.SelectedItems.Count > Preferences.MAX_CATEGORIES)
            {
                this.listeCategories.SelectedItems.RemoveAt(Preferences.MAX_CATEGORIES - 1);
            }
            else
            {
                Abonne.Preferences.CategoriesFavoris = listeCategories.SelectedItems.Cast<Categorie>().ToList();
                //Abonne.Preferences.AjouterCategorie(listeCategories.SelectedItems.Cast<Categorie>().ToList().Last());
            }
        }

        private void listeActeurs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Abonne == null)
            {
                return;
            }
            if (this.listeActeurs.SelectedItems.Count > Preferences.MAX_ACTEURS)
            {
                this.listeActeurs.SelectedItems.RemoveAt(Preferences.MAX_ACTEURS - 1);
            }
            else
            {
                Abonne.Preferences.ActeursFavoris = listeActeurs.SelectedItems.Cast<Acteur>().ToList();
            }
        }

        private void listeRealisateurs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Abonne == null)
            {
                return;
            }
            if (this.listeRealisateurs.SelectedItems.Count > Preferences.MAX_REALISATEURS)
            {
                this.listeRealisateurs.SelectedItems.RemoveAt(Preferences.MAX_REALISATEURS - 1);
            }
            else
            {
                Abonne.Preferences.RealisateursFavoris = listeRealisateurs.SelectedItems.Cast<Realisateur>().ToList();
            }
        }

        private void sliderNote_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            txtCurrentNote.Content = sliderNote.Value.ToString();

            if (ratings1 != null)
            {
                ratings1.Value = (decimal)sliderNote.Value/2;;

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
                lstFilms.ItemsSource = Abonne.FilmVisionnés;
                return;
            }
            List<Film> resultat = new List<Film>();
            resultat = FiltrerFilmsParNom(Abonne.FilmVisionnés, txtRecherche.Text);
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
