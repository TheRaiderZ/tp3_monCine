using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;

namespace MonCine.Data
{
    public class Abonne : Personne
    {
        public Abonne(string prenom, string nom, int age, string username, bool isAdmin=false) : base(prenom, nom, age)
        {
            this.Username = username;
            this.DateAdhesion = DateTime.Now;
            this.Reservations = new List<Reservation>();
            this.Recompenses = new List<Recompense>();
            this.isAdmin = isAdmin;

        }

        public Abonne(string prenom, string nom, int age, string username, Preferences preferences, bool isAdmin=false) : base(prenom, nom, age)
        {
            this.Username = username;
            this.DateAdhesion = DateTime.Now;
            this.Preferences = preferences;
            this.Reservations = new List<Reservation>();
            this.Recompenses = new List<Recompense>();
            this.isAdmin = isAdmin;
        }


        //public ObjectId Id { get; set; }
        public string Username { get; set; }
        public DateTime DateAdhesion { get; set; }
        public List<Recompense> Recompenses { get; set; }
        public Preferences Preferences { get; set; }
        public List<Reservation> Reservations { get; set; }
        public bool isAdmin { get; set; }

        public override string ToString()
        {
            return Username;
        }

    }
}
