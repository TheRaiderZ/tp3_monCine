using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;

namespace MonCine.Data
{
    public class Abonne : Personne
    {
        public Abonne(string prenom, string nom, int age, string username) : base(prenom, nom, age)
        {
            this.Username = username;
            this.DateAdhesion = DateTime.Now;
        }

        //public ObjectId Id { get; set; }
        public string Username { get; set; }
        public DateTime DateAdhesion { get; set; }
        public List<Recompense> Recompenses { get; set; }



        public override string ToString()
        {
            return Username;
        }

    }
}
