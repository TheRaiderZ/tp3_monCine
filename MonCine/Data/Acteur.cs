using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonCine.Data
{
    public class Acteur : Personne
    {
        public Acteur(string prenom, string nom, int age) : base(prenom, nom, age)
        {
        }

        //public ObjectId Id { get; set; }

        
    }
}
