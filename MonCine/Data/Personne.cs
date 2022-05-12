using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonCine.Data
{
    public class Personne
    {
        public string Prenom { get; set; }
        public string Nom { get; set; }
        public int Age { get; set; }

        public ObjectId Id { get; set; }

        public Personne(string prenom, string nom, int age)
        {
            Prenom = prenom;
            Nom = nom;
            Age = age;

        }

        public override string ToString()
        {
            return $"{Prenom} {Nom}";
        }

        public override bool Equals(object obj)
        {
            return obj is Personne personne &&
                   Id.Equals(personne.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Prenom, Nom, Age, Id);
        }
    }
}
