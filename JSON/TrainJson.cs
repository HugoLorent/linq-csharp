using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace linq_csharp.JSON
{
    class TrainJSON
    {
        List<String> trajets = new List<string>();
        IEnumerable<String> départsUniques = new List<string>();
        IEnumerable<String> arrivéeDisponible = new List<string>();
        String départ = "";
        String arrivée = "";


        int compteurAffichage = 1;
        int espaceAAjouter = 0;
        int espaceAAjouterADroite = 0;

        Newtonsoft.Json.Linq.JArray myJsonTravels = JArray.Parse(File.ReadAllText($@"{Directory.GetCurrentDirectory()}/JSON/tarifs-tgv-par-od.json"));

        public void proposerDépart()
        {
            //On prépare une requête pour récupérer tous les trajet
            Console.WriteLine("Sélectionnez un départ :");
            var reqRechercheTousTrajets = from trajet in myJsonTravels
                               let ligne = trajet["fields"]["od"]
                               select ligne;
            //On ajoute tous les éléments de la requête dans la liste statipns
            foreach (var trajet in reqRechercheTousTrajets)
            {
                trajets.Add(trajet.ToString());
            }

            //On prepare une requete sur la liste stations pour supprimer tous ce qu'il y a apres le -,
            //afin de récupérer seulement les gares de départ
            var reqDépartUnique = from travel in trajets
                                let carac = travel.ToString().IndexOf("-")
                                let line = travel.ToString().Substring(0, carac)
                                orderby line
                                select line;

            //Pour éviter les doublons on trim la liste puis on met tous en majuscule
            reqDépartUnique = reqDépartUnique.Select(t => t.Trim().ToUpper());

            //Enfin on fait un distinct pour supprimer tous les doublons
            départsUniques = reqDépartUnique.Distinct();
            Console.WriteLine("Voici les départs d'où vous pouvez partir : ");

            affiche4par4(départsUniques);
            choisirDépart();
        }
        public void choisirDépart()
        {
            Console.WriteLine("");
            Console.WriteLine("D'ou voulez-vous partir ? ");
            this.départ = Console.ReadLine();
            this.départ = this.départ.ToUpper();
            bool gareCorrect = false;

            foreach (var gare in départsUniques)
            {
                if (this.départ == gare)
                {
                    gareCorrect = true;
                }
            }
            if (!gareCorrect)
            {
                while (!gareCorrect)
                {
                    Console.WriteLine("Cette gare n'existe pas, veuillez retaper votre destination :");
                    this.départ = Console.ReadLine();
                    this.départ = this.départ.ToUpper();
                    foreach (var gare in départsUniques)
                    {
                        if (this.départ == gare)
                        {
                            gareCorrect = true;
                        }
                    }
                }
            }

            //On recherche tous les trajets qui ont un départ à this.départ (choix utilisateur)
            var reqContientDépart = from trajet in myJsonTravels
                                 let ligne = trajet["fields"]["od"]
                                 where ligne.ToString().Contains(départ)
                                 select ligne
                                 into tousTrajets
                                 let carac = tousTrajets.ToString().IndexOf("-") + 1
                                 let arrivé = tousTrajets.ToString().Substring(carac)
                                 select arrivé;
            reqContientDépart = reqContientDépart.Select(t => t.Trim().ToUpper());
            arrivéeDisponible = reqContientDépart.Where(i => !i.Contains(départ));
            affiche4par4(arrivéeDisponible);
            choisirArrivée();
        }

        private void choisirArrivée()
        {
            Console.WriteLine("");
            Console.WriteLine("Où voulez-vous arriver ? ");
            this.arrivée = Console.ReadLine();
            this.arrivée = this.arrivée.ToUpper();
            bool gareCorrect = false;

            //On vérifie que la gare recherchée existe avec le départ choisi
            foreach (var gare in arrivéeDisponible)
            {
                if (this.arrivée == gare)
                {
                    gareCorrect = true;
                }
            }
            if (!gareCorrect)
            {
                while (!gareCorrect)
                {
                    Console.WriteLine("Cette gare d'arrivée n'est pas disponible avec votre départ, veuillez entrer une arrivée disponible :");
                    this.arrivée = Console.ReadLine();
                    this.arrivée = this.arrivée.ToUpper();
                    foreach (var gare in arrivéeDisponible)
                    {
                        if (this.arrivée == gare)
                        {
                            gareCorrect = true;
                        }
                    }
                }
            }

            //On fait une requête sur les trajets pour chercher un trajet qui contient le départ et l'arrivée de l'utilisateur et qui commence par le départ
            var reqContientDépartEtArrivée = from trajet in myJsonTravels
                                           let ligne = trajet["fields"]["od"]
                                           let prix2nd = trajet["fields"]["plein_tarif_loisir_2nde"]
                                           let prix1er = trajet["fields"]["1ere_classe"]
                                           where ligne.ToString().Contains(this.départ) && ligne.ToString().Contains(this.arrivée) && ligne.ToString().StartsWith(this.départ)
                                           select $"Votre trajet : {ligne}, pour le prix 2nd classe {prix2nd}EUR, pour le prix 1er classe {prix1er}EUR";
            foreach(var trajetCorrespondantRecherche in reqContientDépartEtArrivée)
            {
                Console.WriteLine(trajetCorrespondantRecherche);
            }
        }

        public void trierParPrix2ndclasse()
        {
            var reqTriParPrix2ndClasse = from trajet in myJsonTravels
                               orderby trajet["fields"]["plein_tarif_loisir_2nde"]
                               let ligne = $"Voyage : {trajet["fields"]["od"]} Prix (2eme classe) : {trajet["fields"]["plein_tarif_loisir_2nde"]} EUR"
                               select ligne;
            foreach (var trajet in reqTriParPrix2ndClasse)
            {
                Console.WriteLine($"{trajet} ");
            }
        }

        public void trierParPrix1erclasse()
        {
            var reqTriParPrix1erClasse = from trajet in myJsonTravels
                               orderby trajet["fields"]["plein_tarif_loisir_2nde"]
                               let ligne = $"Voyage : {trajet["fields"]["od"]} Prix (1ere classe) : {trajet["fields"]["1ere_classe"]} EUR"
                               select ligne;
            foreach (var trajet in reqTriParPrix1erClasse)
            {
               Console.WriteLine($"{trajet} ");     
            }
        }

        public void chercherDestination()
        {
            string rechercheDépart = Console.ReadLine();
            var queryTravels = from travel in myJsonTravels
                               let line = travel["fields"]["od"]
                               where line.ToString().Contains(rechercheDépart, StringComparison.InvariantCultureIgnoreCase)
                               select line;
            foreach (var travel in queryTravels)
            {
                Console.WriteLine(travel);

            }
        }
        public void affiche4par4(IEnumerable<String> listeAAfficher)
        {
            Console.WriteLine("");
            foreach (var travel in listeAAfficher)
            {
                if (compteurAffichage % 4 == 0)
                {
                    Console.WriteLine("");
                    espaceAAjouter = 40 - travel.Length;
                    espaceAAjouterADroite = espaceAAjouter / 2;
                    while (espaceAAjouter > espaceAAjouterADroite)
                    {
                        Console.Write(" ");
                        espaceAAjouter--;
                    }

                    Console.Write(travel);

                    while (espaceAAjouterADroite > 0)
                    {
                        Console.Write(" ");
                        espaceAAjouterADroite--;
                    }

                    espaceAAjouter = 0;
                    espaceAAjouterADroite = 0;
                }
                else
                {
                    espaceAAjouter = 40 - travel.Length;
                    espaceAAjouterADroite = espaceAAjouter / 2;
                    while (espaceAAjouter > espaceAAjouterADroite)
                    {
                        Console.Write(" ");
                        espaceAAjouter--;
                    }

                    Console.Write(travel);

                    while (espaceAAjouterADroite > 0)
                    {
                        Console.Write(" ");
                        espaceAAjouterADroite--;
                    }

                    espaceAAjouter = 0;
                    espaceAAjouterADroite = 0;
                }
                compteurAffichage++;
            }
            compteurAffichage = 0;
        }
    }
}
