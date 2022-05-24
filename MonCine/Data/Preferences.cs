using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MonCine.Data
{
    public class Preferences
    {
        public const int MAX_CATEGORIES = 3;
        public const int MAX_ACTEURS = 5;
        public const int MAX_REALISATEURS = 5;

        private List<Acteur> _acteurs;
        private List<Realisateur> _realisateurs;
        private List<Categorie> _categories;

        
        public List<Acteur> ActeursFavoris
        {
            get
            {
                return this._acteurs;
            }

            set
            {
                if (value.Count <= MAX_ACTEURS)
                {
                    _acteurs = value;
                }
                else
                {
                    MessageBox.Show($"Vous ne pouvez pas avoir plus de {MAX_ACTEURS} acteurs en favoris.");
                }
            }
        }
        
        public List<Categorie> CategoriesFavoris {
            get
            {
                return this._categories;
            }

            set
            {
                if (value.Count <= MAX_CATEGORIES)
                {
                    _categories = value;
                }
                else
                {
                    MessageBox.Show($"Vous ne pouvez pas avoir plus de {MAX_CATEGORIES} categories en favoris.");
                }
            }
        }
        
        public List<Realisateur> RealisateursFavoris 
        {
            get
            {
                return this._realisateurs;
            }

            set
            {
                if (value.Count <= MAX_REALISATEURS)
                {
                    _realisateurs = value;
                }
                else
                {
                    MessageBox.Show($"Vous ne pouvez pas avoir plus de {MAX_REALISATEURS} realisateurs en favoris.");
                }
            }
        }

        
        public Preferences()
        {
            ActeursFavoris = new List<Acteur>();
            CategoriesFavoris = new List<Categorie>();
            RealisateursFavoris = new List<Realisateur>();
        }

        public Preferences(List<Acteur> acteursFavoris, List<Categorie> categoriesFavoris, List<Realisateur> realisateursFavoris)
        {
            ActeursFavoris = acteursFavoris ?? new List<Acteur>();
            CategoriesFavoris = categoriesFavoris ?? new List<Categorie>();
            RealisateursFavoris = realisateursFavoris ?? new List<Realisateur>();
        }

        public void AjouterActeur(Acteur acteur)
        {
            if (ActeursFavoris.Count < MAX_ACTEURS)
            {
                ActeursFavoris.Add(acteur);
            }
            else
            {
                MessageBox.Show($"Vous ne pouvez pas avoir plus de {MAX_ACTEURS} acteurs en favoris.");
            }
        }

        public void AjouterCategorie(Categorie categorie)
        {
            if (CategoriesFavoris.Count < MAX_CATEGORIES)
            {
                CategoriesFavoris.Add(categorie);
            }
            else
            {
                MessageBox.Show($"Vous ne pouvez pas avoir plus de {MAX_CATEGORIES} categories en favoris.");
            }
        }

        public void AjouterRealisateur(Realisateur realisateur)
        {
            if (RealisateursFavoris.Count < MAX_REALISATEURS)
            {
                RealisateursFavoris.Add(realisateur);
            }
            else
            {
                MessageBox.Show($"Vous ne pouvez pas avoir plus de {MAX_REALISATEURS} realisateurs en favoris.");
            }
        }
    }
}
