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
        private DAL _dal;
        public Profil(DAL dal)
        {
            InitializeComponent();
            DataContext = this;
            _dal = dal;
            GetFirstAbonne();
            ReadEntities();
            PopulateListViews();
        }
        private void GetFirstAbonne() 
        {
            Abonne = _dal.ReadAbonnes().FirstOrDefault();
            if (Abonne.Preferences == null)
            {
                Abonne.Preferences = new Preferences();
            }
            
        }
        private void PopulateListViews()
        {
            //listeFilms.ItemsSource = Films; listeFilms.DataContext = Films;
            Realisateurs = Realisateurs.OrderBy(a => a.Nom).ToList();
            listeRealisateurs.ItemsSource = Realisateurs;
            Acteurs = Acteurs.OrderBy(a => a.Nom).ToList();
            listeActeurs.ItemsSource = Acteurs;
            listeCategories.ItemsSource = Enum.GetValues(typeof(Categorie));
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
    }
}
