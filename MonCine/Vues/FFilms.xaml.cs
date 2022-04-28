// Marvin Laieb

using MonCine.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
    /// Logique d'interaction pour Films.xaml
    /// </summary>
    public partial class FFilms : Page
    {
        public List<Film> Films = new List<Film>();
        public List<Realisateur> Realisateurs = new List<Realisateur>();
        public List<Acteur> Acteurs = new List<Acteur>();
        public Film SelectedFilm { get; set; }
        private DAL _dal;
        public FFilms(DAL dal)
        {
            InitializeComponent();
            _dal = dal;
            ReadEntities();
            PopulateListViews();
            
        }

        private void PopulateListViews()
        {
            listeFilms.ItemsSource = Films; listeFilms.DataContext = Films;
            listeRealisateurs.ItemsSource = Realisateurs; listeRealisateurs.DataContext = Realisateurs;
            listeActeurs.ItemsSource = Acteurs; listeRealisateurs.DataContext = Acteurs;
            listeCategories.ItemsSource = Enum.GetValues(typeof(Categorie));
        }

        private void ReadEntities()
        {
            Films = _dal.ReadFilms();
            Realisateurs = _dal.ReadRealisateurs();
            Acteurs = _dal.ReadActeurs();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        public static List<Film> FiltrerFilmsParNom(List<Film> films, string filtre)
        {
            List<Film> resultat = new List<Film>();

            resultat = films.Where(x => x.NomCorrespondFiltre(filtre)).ToList();
            return resultat;
        }

        //Barre de recherche
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            listeFilms.SelectedItem = null;
            SelectedFilm = null;
            if (String.IsNullOrWhiteSpace(txtRecherche.Text))
            {
                listeFilms.ItemsSource = Films;
                return;
            }
            List<Film> resultat=new List<Film>();
            resultat = FiltrerFilmsParNom(Films, txtRecherche.Text);
            listeFilms.ItemsSource = resultat;

        }

        private void listeFilms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedFilm = (Film) listeFilms.SelectedItem;
            if (SelectedFilm == null) { return; }

            listeRealisateurs.SelectedItem = SelectedFilm.Realisateur;

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            EnregistrerChangements();
        }

        public void EnregistrerChangements() {
            foreach (var item in Films)
            {
                _dal.UpdateFilm(item);
            }
        }

        private void listeCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedFilm==null)
            {
                return;
            }
            ((Film)listeFilms.SelectedItem).Categories = listeCategories.SelectedItems.Cast<Categorie>().ToList();
        }

        private void listeActeurs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedFilm == null)
            {
                return;
            }
            ((Film)listeFilms.SelectedItem).Acteurs = listeActeurs.SelectedItems.Cast<Acteur>().ToList();
        }
    }

    // Set ListBox SelectedItems based on another collection (https://stackoverflow.com/a/15540770) 
    public static class ListBoxExtensions
    {
        // Using a DependencyProperty as the backing store for SearchValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemListProperty =
            DependencyProperty.RegisterAttached("SelectedItemList", typeof(IList), typeof(ListBoxExtensions),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnSelectedItemListChanged)));

        public static IList GetSelectedItemList(DependencyObject obj)
        {
            return (IList)obj.GetValue(SelectedItemListProperty);
        }

        public static void SetSelectedItemList(DependencyObject obj, IList value)
        {
            obj.SetValue(SelectedItemListProperty, value);
        }

        private static void OnSelectedItemListChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var listbox = d as ListBox;
            if (listbox != null)
            {
                listbox.SelectedItems.Clear();
                var selectedItems = e.NewValue as IList;
                if (selectedItems != null)
                {
                    foreach (var item in selectedItems)
                    {
                        listbox.SelectedItems.Add(item);
                    }
                }
            }
        }
    }
}
