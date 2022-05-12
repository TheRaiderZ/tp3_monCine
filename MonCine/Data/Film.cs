using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;

namespace MonCine.Data
{
    public class Film
    {
        public ObjectId Id { get; set; }
        public string Nom { get; set; }
        public DateTime DateSortie { get; set; }
        public DateTime DateProjection { get; set; }
        public bool SurAffiche { get; set; }
        public List<Acteur> Acteurs { get; set; }
        public Realisateur Realisateur { get; set; }
        public IEnumerable<Categorie> Categories { get; set; }
        public List<int> Notes { get; set; }
        public int NbProjections { get; set; }

        public string CategorieToString => CategoryToString();

        private string CategoryToString()
        {
            return String.Join(',', this.Categories);
        }

        public bool NomCorrespondFiltre(string filtre) {
            string cleanedLowercaseFilter = filtre.Trim().ToLower();
            string lowercaseName = Nom.ToLower();

            return lowercaseName.Contains(cleanedLowercaseFilter); 
        }

        public override string ToString()
        {
            return Nom;
        }

        public Film(string nom, DateTime dateSortie)
        {
            Nom = nom;
            DateSortie = dateSortie;
        }


    }
}
