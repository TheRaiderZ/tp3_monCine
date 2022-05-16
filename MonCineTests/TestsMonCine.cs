using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonCine.Data;
using MonCine.Vues;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MonCineTests
{

    [TestClass]
    public class DALtest
    {
        private Mock<IMongoClient> mongoClient;
        private Mock<IMongoDatabase> mongodb;
        private Mock<IMongoCollection<Film>> filmCollection;
        private List<Film> filmList;
        private Mock<IAsyncCursor<Film>> filmCursor;

        public DALtest()
        {
            this.mongoClient = new Mock<IMongoClient>();
            this.mongodb = new Mock<IMongoDatabase>();

            this.filmCollection = new Mock<IMongoCollection<Film>>();
            this.filmCursor = new Mock<IAsyncCursor<Film>>();


            this.filmList = new List<Film>() {
                new Film (
                    "Film 1",
                    DateTime.UtcNow
                ),
                new Film (
                    "Film 2",
                    DateTime.UtcNow
                )
            };
        }

        private void InitializeMongoDb()
        {
            this.mongodb.Setup(x => x.GetCollection<Film>("Films", default)).Returns(this.filmCollection.Object);

            this.mongoClient.Setup(x => x.GetDatabase(It.IsAny<string>(), default)).Returns(this.mongodb.Object);
        }

        private void InitializeMongoFilmsCollection()
        {
            this.filmCursor.Setup(x => x.Current).Returns(this.filmList);

            this.filmCursor.SetupSequence(x => x.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);


            this.filmCollection
                .Setup(
                    x => x.FindSync(Builders<Film>.Filter.Empty, It.IsAny<FindOptions<Film>>(), default))
                .Returns(filmCursor.Object);



            this.InitializeMongoDb();
        }

        [TestMethod]
        public void ReadAliments_moqFindSyncCall()
        {
            // Création des faux objets
            this.InitializeMongoFilmsCollection();

            var dal = new DAL(mongoClient.Object);

            // Act : appel de la méthode qui contient le faux objet
            var documents = dal.ReadFilms();

            // Assert
            CollectionAssert.AreEqual(filmList, documents);

        }
    }

    [TestClass]
    public class RealisateursTests
    {
        private Mock<IMongoClient> mongoClient;
        private Mock<IMongoDatabase> mongodb;
        private Mock<IMongoCollection<Realisateur>> realisateurCollection;
        private List<Realisateur> realisateurList;
        private Mock<IAsyncCursor<Realisateur>> realisateurCursor;

        public RealisateursTests()
        {
            this.mongoClient = new Mock<IMongoClient>();
            this.mongodb = new Mock<IMongoDatabase>();

            this.realisateurCollection = new Mock<IMongoCollection<Realisateur>>();
            this.realisateurCursor = new Mock<IAsyncCursor<Realisateur>>();


            this.realisateurList = new List<Realisateur>() {
                new Realisateur (
                    "Realisateur",
                    "1",
                    1
                ),
                new Realisateur (
                    "Realisateur",
                    "2",
                    2
                )
            };
        }

        private void InitializeMongoDb()
        {
            this.mongodb.Setup(x => x.GetCollection<Realisateur>("Realisateurs", default)).Returns(this.realisateurCollection.Object);

            this.mongoClient.Setup(x => x.GetDatabase(It.IsAny<string>(), default)).Returns(this.mongodb.Object);
        }

        private void InitializeMongoRealisateursCollection()
        {
            this.realisateurCursor.Setup(x => x.Current).Returns(this.realisateurList);

            this.realisateurCursor.SetupSequence(x => x.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);


            this.realisateurCollection
                .Setup(
                    x => x.FindSync(Builders<Realisateur>.Filter.Empty, It.IsAny<FindOptions<Realisateur>>(), default))
                .Returns(realisateurCursor.Object);



            this.InitializeMongoDb();
        }

        [TestMethod]
        public void ReadRealisateurTest()
        {
            // Création des faux objets
            this.InitializeMongoRealisateursCollection();

            var dal = new DAL(mongoClient.Object);

            // Act : appel de la méthode qui contient le faux objet
            var documents = dal.ReadRealisateurs();

            // Assert
            CollectionAssert.AreEqual(realisateurList, documents);

        }

        Realisateur realisateurTestUnitaire = new Realisateur("Bob", "PierreTest", 43);

        [TestMethod]
        public void AjouterRealisateurTest()
        {
            DAL dal = new DAL();

            dal.AddRealisateur(realisateurTestUnitaire);
            var realisateurs = dal.ReadRealisateurs();
            var result = realisateurs.Find(x => x.Nom == realisateurTestUnitaire.Nom);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ReadRealisateursNonNullTest()
        {
            DAL dal = new DAL();
            List<Realisateur> realisateurs = new List<Realisateur>();

            realisateurs = dal.ReadRealisateurs();

            Assert.IsNotNull(realisateurs);

        }

        [TestMethod]
        public void ModifyRealisateurTest()
        {
            DAL dal = new DAL();
            //Film film = new Film("Film TestUnitaire", DateTime.UtcNow);
            var foundRealisateur = dal.ReadRealisateurs().Find(x => x.Nom == realisateurTestUnitaire.Nom);
            foundRealisateur.Age = 55;
            dal.UpdateRealisateur(foundRealisateur);

            var actualRealisateur = dal.ReadRealisateurs().Find(x => x.Nom == realisateurTestUnitaire.Nom);

            Assert.AreEqual(55, actualRealisateur.Age);
        }

        [TestMethod]
        public void RemoveRealisateurTest()
        {
            DAL dal = new DAL();
            //Film film = new Film("Film TestUnitaire", DateTime.UtcNow);
            Realisateur realisateur = dal.ReadRealisateurs().Find(x => x.Nom == realisateurTestUnitaire.Nom);
            dal.RemoveRealisateur(realisateur);
            var realisateurs = dal.ReadRealisateurs();
            var result = realisateurs.Find(x => x.Id == realisateur.Id);
            Assert.IsNull(result);
        }

    }

    [TestClass]
    public class ActeursTests
    {
        private Mock<IMongoClient> mongoClient;
        private Mock<IMongoDatabase> mongodb;
        private Mock<IMongoCollection<Acteur>> acteurCollection;
        private List<Acteur> acteurList;
        private Mock<IAsyncCursor<Acteur>> acteurCursor;

        public ActeursTests()
        {
            this.mongoClient = new Mock<IMongoClient>();
            this.mongodb = new Mock<IMongoDatabase>();

            this.acteurCollection = new Mock<IMongoCollection<Acteur>>();
            this.acteurCursor = new Mock<IAsyncCursor<Acteur>>();


            this.acteurList = new List<Acteur>() {
                new Acteur (
                    "Acteur",
                    "1",
                    1
                ),
                new Acteur (
                    "Acteur",
                    "2",
                    2
                )
            };
        }

        private void InitializeMongoDb()
        {
            this.mongodb.Setup(x => x.GetCollection<Acteur>("Acteurs", default)).Returns(this.acteurCollection.Object);

            this.mongoClient.Setup(x => x.GetDatabase(It.IsAny<string>(), default)).Returns(this.mongodb.Object);
        }

        private void InitializeMongoActeursCollection()
        {
            this.acteurCursor.Setup(x => x.Current).Returns(this.acteurList);

            this.acteurCursor.SetupSequence(x => x.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);


            this.acteurCollection
                .Setup(
                    x => x.FindSync(Builders<Acteur>.Filter.Empty, It.IsAny<FindOptions<Acteur>>(), default))
                .Returns(acteurCursor.Object);

            this.InitializeMongoDb();
        }

        [TestMethod]
        public void ReadActeurTest()
        {
            // Création des faux objets
            this.InitializeMongoActeursCollection();

            var dal = new DAL(mongoClient.Object);

            // Act : appel de la méthode qui contient le faux objet
            var documents = dal.ReadActeurs();

            // Assert
            CollectionAssert.AreEqual(acteurList, documents);

        }

        Acteur acteurTestUnitaire = new Acteur("Bob", "PierreTest", 43);

        [TestMethod]
        public void AjouterActeurTest()
        {
            DAL dal = new DAL();

            dal.AddActeur(acteurTestUnitaire);
            var acteurs = dal.ReadActeurs();
            var result = acteurs.Find(x => x.Nom == acteurTestUnitaire.Nom);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ReadActeursNonNullTest()
        {
            this.InitializeMongoActeursCollection();
            DAL dal = new DAL(mongoClient.Object);
            List<Acteur> acteurs = new List<Acteur>();

            acteurs = dal.ReadActeurs();

            Assert.IsNotNull(acteurs);

        }

        //[TestMethod]
        //public void ModifyActeurTest()
        //{

        //    DAL dal = new DAL();
        //    var foundActeur = dal.ReadActeurs().Find(x => x.Nom == acteurTestUnitaire.Nom);
        //    foundActeur.Age = 55;
        //    dal.UpdateActeur(foundActeur);

        //    var actualActeur = dal.ReadActeurs().Find(x => x.Nom == acteurTestUnitaire.Nom);

        //    Assert.AreEqual(55, actualActeur.Age);
        //}

        [TestMethod]
        public void RemoveActeurTest()
        {
            DAL dal = new DAL();
            Acteur acteur = dal.ReadActeurs().Find(x => x.Nom == acteurTestUnitaire.Nom);
            dal.RemoveActeur(acteur);
            var acteurs = dal.ReadActeurs();
            var result = acteurs.Find(x => x.Id == acteur.Id);
            Assert.IsNull(result);
        }

    }

    [TestClass]
    public class FilmsTests
    {
        private Mock<IMongoClient> mongoClient;
        private Mock<IMongoDatabase> mongodb;
        private Mock<IMongoCollection<Film>> filmCollection;
        private List<Film> filmList;
        private Mock<IAsyncCursor<Film>> filmCursor;

        public FilmsTests()
        {
            this.mongoClient = new Mock<IMongoClient>();
            this.mongodb = new Mock<IMongoDatabase>();

            this.filmCollection = new Mock<IMongoCollection<Film>>();
            this.filmCursor = new Mock<IAsyncCursor<Film>>();


            this.filmList = new List<Film>() {
                new Film (
                    "Film 1",
                    DateTime.UtcNow
                ),
                new Film (
                    "Film 2",
                    DateTime.UtcNow
                )
            };
        }

        private void InitializeMongoDb()
        {
            this.mongodb.Setup(x => x.GetCollection<Film>("Films", default)).Returns(this.filmCollection.Object);

            this.mongoClient.Setup(x => x.GetDatabase(It.IsAny<string>(), default)).Returns(this.mongodb.Object);
        }

        private void InitializeMongoFilmsCollection()
        {
            this.filmCursor.Setup(x => x.Current).Returns(this.filmList);

            this.filmCursor.SetupSequence(x => x.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);


            this.filmCollection
                .Setup(
                    x => x.FindSync(Builders<Film>.Filter.Empty, It.IsAny<FindOptions<Film>>(), default))
                .Returns(filmCursor.Object);



            this.InitializeMongoDb();
        }

        [TestMethod]
        public void ReadFilmTest()
        {
            // Création des faux objets
            this.InitializeMongoFilmsCollection();

            var dal = new DAL(mongoClient.Object);

            // Act : appel de la méthode qui contient le faux objet
            var documents = dal.ReadFilms();

            // Assert
            CollectionAssert.AreEqual(filmList, documents);

        }


        Film filmTestUnitaire = new Film("Film TestUnitaire", DateTime.UtcNow);

        [TestMethod]
        public void AjouterFilmTest()
        {
            DAL dal = new DAL();

            filmTestUnitaire.SurAffiche = true;
            dal.AddFilm(filmTestUnitaire);
            var films = dal.ReadFilms();
            var result = films.Find(x => x.Nom == filmTestUnitaire.Nom);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ReadFilmsNonNullTest()
        {
            DAL dal = new DAL();
            List<Film> films = new List<Film>();

            films = dal.ReadFilms();

            Assert.IsNotNull(films);

        }
        [TestMethod]
        public void FindFilmParNomTest()
        {
            DAL dal = new DAL();

            Film result = dal.FindFilmByName(filmTestUnitaire.Nom);

            Assert.IsNotNull(result);

        }
        [TestMethod]
        public void ModifyFilmTest()
        {
            DAL dal = new DAL();
            var foundfilm = dal.FindFilmByName(filmTestUnitaire.Nom);
            foundfilm.Realisateur = new Realisateur("Bob", "Bob1", 10);
            dal.UpdateFilm(foundfilm);
            var actualFilm = dal.FindFilmByName(filmTestUnitaire.Nom);

            Assert.AreEqual("Bob", actualFilm.Realisateur.Prenom);
        }
        [TestMethod]
        public void ReadFilmAfficheTest()
        {
            DAL dal = new DAL();
            //Film film = new Film("Film TestUnitaire", DateTime.UtcNow);
            var films = dal.ReadFilmsAffiche();
            var result = films.Find(x => x.Nom == filmTestUnitaire.Nom);
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void RemoveFilmTest()
        {
            DAL dal = new DAL();
            //Film film = new Film("Film TestUnitaire", DateTime.UtcNow);
            Film film = dal.FindFilmByName(filmTestUnitaire.Nom);
            dal.RemoveFilm(film);
            var films = dal.ReadFilms();
            var result = films.Find(x => x.Id == film.Id);
            Assert.IsNull(result);
        }
        [TestMethod]
        public void RechercherFilmsParNom()
        {
            DateTime sameDate = System.DateTime.UtcNow;
            Film item1 = new Film("Un Bon Film", sameDate);
            Film item2 = new Film("Le Film 3", sameDate);
            List<Film> mockListefilms = new List<Film>()
            {
                new Film("BlaBla", sameDate),
                new Film("BlaBla 2", sameDate),
                new Film("Truc", sameDate),
                new Film("Chose 4", sameDate),
                item1,
                item2,
            };
            List<Film> resultatAttendu = new List<Film>()
            {
                item1,
                item2,
            };

            List<Film> resultat = FFilms.FiltrerFilmsParNom(mockListefilms, "film");

            CollectionAssert.AreEquivalent(resultatAttendu, resultat);


        }

        [TestMethod]
        public void AjouterNotes()
        {
            DAL dal = new DAL();
            var foundfilm = dal.FindFilmByName(filmTestUnitaire.Nom);
            if (foundfilm!=null)
            {

                foundfilm.Notes = new List<int>() { 10 };
                foundfilm.Notes.Add(10);
                dal.UpdateFilm(foundfilm);
                var actualFilm = dal.FindFilmByName(filmTestUnitaire.Nom);
                Assert.AreEqual(10, actualFilm.Notes.First());
            }
                
        }
        [TestMethod]
        public void GetNoteSur5()
        {
            DAL dal = new DAL();
            var foundfilm = dal.FindFilmByName(filmTestUnitaire.Nom);
            if (foundfilm != null)
            {
                List<int> notes = new List<int>() { 10, 5, 9 };
                foundfilm.Notes = notes;
                dal.UpdateFilm(foundfilm);
                double moyenneNotes = notes.Average();
                double noteSur5 = (moyenneNotes * 5) / 10;
                var actualFilm = dal.FindFilmByName(filmTestUnitaire.Nom);
                Assert.AreEqual(noteSur5, actualFilm.ValeurSur5);
            }
        }

    }

    [TestClass]
    public class AbonnesTests
    {
        private Mock<IMongoClient> mongoClient;
        private Mock<IMongoDatabase> mongodb;
        private Mock<IMongoCollection<Abonne>> abonneCollection;
        private List<Abonne> abonneList;
        private Mock<IAsyncCursor<Abonne>> abonneCursor;

        public AbonnesTests()
        {
            this.mongoClient = new Mock<IMongoClient>();
            this.mongodb = new Mock<IMongoDatabase>();

            this.abonneCollection = new Mock<IMongoCollection<Abonne>>();
            this.abonneCursor = new Mock<IAsyncCursor<Abonne>>();


            this.abonneList = new List<Abonne>() {
                new Abonne("Abonne1", "TestUnitaire", 1, "aboTest1"),
                new Abonne("Abonne2", "TestUnitaire", 1, "aboTest2")
            };
        }

        private void InitializeMongoDb()
        {
            this.mongodb.Setup(x => x.GetCollection<Abonne>("Abonnes", default)).Returns(this.abonneCollection.Object);

            this.mongoClient.Setup(x => x.GetDatabase(It.IsAny<string>(), default)).Returns(this.mongodb.Object);
        }

        private void InitializeMongoAbonnesCollection()
        {
            this.abonneCursor.Setup(x => x.Current).Returns(this.abonneList);

            this.abonneCursor.SetupSequence(x => x.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);


            this.abonneCollection
                .Setup(
                    x => x.FindSync(Builders<Abonne>.Filter.Empty, It.IsAny<FindOptions<Abonne>>(), default))
                .Returns(abonneCursor.Object);



            this.InitializeMongoDb();
        }

        [TestMethod]
        public void ReadAbonneTest()
        {
            // Création des faux objets
            this.InitializeMongoAbonnesCollection();

            var dal = new DAL(mongoClient.Object);

            // Act : appel de la méthode qui contient le faux objet
            var documents = dal.ReadAbonnes();

            // Assert
            CollectionAssert.AreEqual(abonneList, documents);

        }


        Abonne abonneTestUnitaire = new Abonne("Abonne", "TestUnitaire", 1, "abo1");
        Abonne adminTestUnitaire = new Abonne("Admin", "TestUnitaire", 1, "admin1", true);

        [TestMethod]
        public void AjouterAbonneTest()
        {
            DAL dal = new DAL();

            dal.AddAbonne(abonneTestUnitaire);
            var abonnes = dal.ReadAbonnes();
            var result = abonnes.Find(x => x.Username == abonneTestUnitaire.Username);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AjouterAdminTest()
        {
            DAL dal = new DAL();

            dal.AddAbonne(adminTestUnitaire);
            var abonnes = dal.ReadAbonnes();
            var result = abonnes.Find(x => x.Username == adminTestUnitaire.Username);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ReadAbonnesNonNullTest()
        {
            DAL dal = new DAL();
            List<Abonne> abonnes = new List<Abonne>();

            abonnes = dal.ReadAbonnes();

            Assert.IsNotNull(abonnes);

        }
        [TestMethod]
        public void FindAbonneParNomTest()
        {
            DAL dal = new DAL();

            Abonne result = dal.FindAbonneByUserName(abonneTestUnitaire.Username);

            Assert.IsNotNull(result);

        }
        [TestMethod]
        public void ModifyAbonneTest()
        {
            DAL dal = new DAL();
            var foundabonne = dal.FindAbonneByUserName(abonneTestUnitaire.Username);
            foundabonne.Age = 4;
            dal.UpdateAbonne(foundabonne);
            var actualAbonne = dal.FindAbonneByUserName(abonneTestUnitaire.Username);

            Assert.AreEqual(4, actualAbonne.Age);
        }
        [TestMethod]
        public void RemoveAbonneTest()
        {
            DAL dal = new DAL();
            Abonne abonne = dal.FindAbonneByUserName(abonneTestUnitaire.Username);
            dal.RemoveAbonne(abonne);
            var abonnes = dal.ReadAbonnes();
            var result = abonnes.Find(x => x.Id == abonne.Id);
            Assert.IsNull(result);
        }
        
        [TestMethod]
        public void GetAdminsTest()
        {
            DAL dal = new DAL();
            List<Abonne> admins = new List<Abonne>();
            admins = dal.GetAdmins();
            Assert.IsNotNull(admins);

            //Removing the admin after the test
            Abonne admin = dal.FindAbonneByUserName(adminTestUnitaire.Username);
            dal.RemoveAbonne(admin);

        }
    }
}