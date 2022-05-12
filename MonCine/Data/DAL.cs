using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using System.Windows;
using System.Linq;

namespace MonCine.Data
{
    public class DAL
    {
        private IMongoClient mongoDBClient;
        private IMongoDatabase database;

        public DAL(IMongoClient client = null)
        {
            mongoDBClient = client ?? OuvrirConnexion();
            database = ConnectDatabase();
        }
        private IMongoClient OuvrirConnexion()
        {
            MongoClient dbClient = null;
            try
            {
                dbClient = new MongoClient("mongodb://localhost:27017/");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Impossible de se connecter à la base de données " + ex.Message, "Erreur");
            }
            return dbClient;
        }

        private IMongoDatabase ConnectDatabase()
        {
            IMongoDatabase db = null;
            try
            {
                db = mongoDBClient.GetDatabase("TP2DB");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Impossible de se connecter à la base de données " + ex.Message, "Erreur");
            }
            return db;
        }


        #region CRUD Abonne
        public List<Abonne> ReadAbonnes()
        {
            var abonnes = new List<Abonne>();

            try
            {
                var collection = database.GetCollection<Abonne>("Abonnes");
                if (collection != null)
                {
                    abonnes = collection.FindSync(Builders<Abonne>.Filter.Empty).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossible d'obtenir la collection " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            return abonnes;
        }
        public void AddAbonne(Abonne abonne)
        {
            try
            {
                var collection = database.GetCollection<Abonne>("Abonnes");
                collection.InsertOne(abonne);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossible d'ajouter un abonne " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        public void UpdateAbonne(Abonne abonne)
        {
            try
            {
                var collection = database.GetCollection<Abonne>("Abonnes");
                collection.ReplaceOne((x => x.Id == abonne.Id), abonne);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossible de modifier un abonne " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
        
        public void RemoveAbonne(Abonne abonne)
        {
            try
            {
                var collection = database.GetCollection<Abonne>("Abonnes");
                collection.DeleteOne((x => x.Id == abonne.Id));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossible de supprimer un abonne " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        public void AddRecompense( Abonne abonne)
        {
            try
            {
                var collection = database.GetCollection<Abonne>("Abonnes");
                collection.ReplaceOne((x => x.Id == abonne.Id), abonne);
            }
            catch (Exception ex)
            {

                MessageBox.Show("Impossible d'ajouter la récompense. " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public Abonne FindAbonneByUserName(string username)
        {
            try
            {
                var collection = database.GetCollection<Abonne>("Abonnes");
                var abonne = collection.Find(x => x.Username == username).FirstOrDefault();
                return abonne;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossible d'obtenir la collection " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            return null;
        }

        public List<Abonne> GetAdmins()
        {
            try
            {
                List<Abonne> admins = new List<Abonne>();
                var collection = database.GetCollection<Abonne>("Abonnes");
                admins = collection.Find(x => x.isAdmin).ToList();
                return admins;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossible d'obtenir la collection " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            return null;
        }
        #endregion

        #region CRUD Projection
        public List<Projection> ReadProjections()
        {
            var projections = new List<Projection>();
            try
            {
                var collection = database.GetCollection<Projection>("Projections");
                if (collection != null)
                {
                    projections = collection.FindSync(Builders<Projection>.Filter.Empty).ToList();
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("Impossible d'obtenir la collection " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return projections;
        }
        public void AddProjection(Projection projection)
        {
            try
            {
                var collection = database.GetCollection<Projection>("Projections");
                collection.InsertOne(projection);
            }
            catch (Exception ex)
            {

                MessageBox.Show("Impossible d'ajouter la récompense. " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        public void UpdateProjection(Projection projection)
        {
            try
            {
                var collection = database.GetCollection<Projection>("Projections");
                collection.ReplaceOne((x => x.Id == projection.Id), projection);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossible de modifier une projection " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

            }

        }

        #endregion

        #region CRUD Films
        public List<Film> ReadFilms()
        {
            var films = new List<Film>();

            try
            {
                var collection = database.GetCollection<Film>("Films");
                if (collection!=null)
                {
                    films = collection.FindSync(Builders<Film>.Filter.Empty).ToList(); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossible d'obtenir la collection " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            return films;
        }

        public Film FindFilmByName(string nom)
        {
            

            try
            {
                var collection = database.GetCollection<Film>("Films");
                var film = collection.Find(x=>x.Nom==nom).FirstOrDefault();
                return film;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossible d'obtenir la collection " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            return null;
        }

        public void AddFilm(Film film)
        {
            

            try
            {
                var collection = database.GetCollection<Film>("Films");
                collection.InsertOne(film);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossible d'ajouter un film " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        public void UpdateFilm(Film film)
        {


            try
            {
                var collection = database.GetCollection<Film>("Films");
                collection.ReplaceOne((x=> x.Id==film.Id), film);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossible de modifier un film " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        public void RemoveFilm(Film film)
        {

            try
            {
                var collection = database.GetCollection<Film>("Films");
                collection.DeleteOne((x => x.Id == film.Id));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossible de supprimer un film " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        public List<Film> ReadFilmsAffiche()
        {
            var films = new List<Film>();

            try
            {
                var collection = database.GetCollection<Film>("Films");
                films = collection.Find(x=>x.SurAffiche==true).ToList();    
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossible d'obtenir la collection " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            return films;
        }

        #endregion

        #region CRUD Acteur
        public List<Acteur> ReadActeurs()
        {
            var acteurs = new List<Acteur>();

            try
            {
                var collection = database.GetCollection<Acteur>("Acteurs");
                //acteurs = collection.Aggregate().ToList();
                if (collection != null)
                {
                    acteurs = collection.FindSync(Builders<Acteur>.Filter.Empty).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossible d'obtenir la collection " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            return acteurs;
        }
        public void AddActeur(Acteur acteur)
        {
            try
            {
                var collection = database.GetCollection<Acteur>("Acteurs");
                collection.InsertOne(acteur);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossible d'ajouter un acteur " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        public void UpdateActeur(Acteur acteur)
        {
            try
            {
                var collection = database.GetCollection<Acteur>("Acteurs");
                collection.ReplaceOne((x => x.Id == acteur.Id), acteur);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossible de modifier un acteur " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        public void RemoveActeur(Acteur acteur)
        {

            try
            {
                var collection = database.GetCollection<Acteur>("Acteurs");
                collection.DeleteOne((x => x.Id == acteur.Id));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossible de supprimer un acteur " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
        #endregion

        #region CRUD Realisateur
        public List<Realisateur> ReadRealisateurs()
        {
            var realisateurs = new List<Realisateur>();

            try
            {
                var collection = database.GetCollection<Realisateur>("Realisateurs");
                if (collection != null)
                {
                    realisateurs = collection.FindSync(Builders<Realisateur>.Filter.Empty).ToList();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossible d'obtenir la collection " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            return realisateurs;
        }

        public void AddRealisateur(Realisateur realisateur)
        {
            try
            {
                var collection = database.GetCollection<Realisateur>("Realisateurs");
                collection.InsertOne(realisateur);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossible d'ajouter un realisateur " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        public void UpdateRealisateur(Realisateur realisateur)
        {
            try
            {
                var collection = database.GetCollection<Realisateur>("Realisateurs");
                collection.ReplaceOne((x => x.Id == realisateur.Id), realisateur);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossible de modifier un realisateur " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        public void RemoveRealisateur(Realisateur realisateur)
        {
            try
            {
                var collection = database.GetCollection<Realisateur>("Realisateurs");
                collection.DeleteOne((x => x.Id == realisateur.Id));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossible de supprimer un realisateur " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        
        #endregion

        #region CRUD Reservation
        public List<Reservation> ReadReservation()
        {
            var reservations = new List<Reservation>();

            try
            {
                var collection = database.GetCollection<Reservation>("Reservations");
                if (collection != null)
                {
                    reservations = collection.FindSync(Builders<Reservation>.Filter.Empty).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossible d'obtenir la collection " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            return reservations;
        }

        public List<Reservation> ReadReservationOfAbonne(Abonne abonne)
        {
            var reservations = new List<Reservation>();

            try
            {
                var collection = database.GetCollection<Reservation>("Reservations");
                if (collection != null)
                {
                    reservations = collection.FindSync(x => x.Personne.Id == abonne.Id).ToList();
                        //.a(x => x.Personne == abonne);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossible d'obtenir la collection " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            return reservations;
        }

        public void AddReservation(Reservation reservation)
        {


            try
            {
                var collection = database.GetCollection<Reservation>("Reservations");
                collection.InsertOne(reservation);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossible d'ajouter une réservation " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }


        #endregion

        #region CRUD Reservation
        public List<Reservation> ReadReservation()
        {
            var reservations = new List<Reservation>();

            try
            {
                var collection = database.GetCollection<Reservation>("Reservations");
                if (collection != null)
                {
                    reservations = collection.FindSync(Builders<Reservation>.Filter.Empty).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossible d'obtenir la collection " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            return reservations;
        }

        public void AddReservation(Reservation reservation)
        {


            try
            {
                var collection = database.GetCollection<Reservation>("Reservations");
                collection.InsertOne(reservation);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossible d'ajouter une réservation " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }


        #endregion
    }
}
