using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonCine.Data
{
    public class Realisateur : Personne
    {
        public Realisateur(string prenom, string nom, int age) : base(prenom, nom, age)
        { }

    }
}
