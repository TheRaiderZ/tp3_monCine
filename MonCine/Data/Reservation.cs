using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;

namespace MonCine.Data
{
    public class Reservation
    {
        public ObjectId Id { get; set; }
        public DateTime Date { get; set; }
        public Abonne Personne { get; set; }
        public int NbPlaces { get; set; }
        public Projection Projection { get; set; }

        public Reservation(DateTime date, Abonne personne, int nbPlaces, Projection projection)
        {
            this.Date = date;
            this.Personne = personne;
            this.NbPlaces = nbPlaces;
            this.Projection = projection;
        }
    }
}
