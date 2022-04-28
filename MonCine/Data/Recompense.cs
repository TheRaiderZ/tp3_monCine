using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;


namespace MonCine.Data
{
    public class Recompense
    {
        public TypeRecompense Type { get; set; }
        public Film Film { get; set; }

        public Recompense(TypeRecompense type, Film film)
        {
            this.Type = type;
            this.Film = film;
        }

        public override string ToString()
        {
            return Type + " : " + Film.Nom;
        }
    }
}
