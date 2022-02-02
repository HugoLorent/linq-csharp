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
        List<String> gares = new List<string>();
        IEnumerable<String> garesUnique = new List<string>();


        int compteurAffichage = 1;
        int espaceAAjouter = 0;
        int espaceAAjouterADroite = 0;

        Newtonsoft.Json.Linq.JArray myJsonTravels = JArray.Parse(File.ReadAllText($@"{Directory.GetCurrentDirectory()}/JSON/tarifs-tgv-par-od.json"));

        public void proposerDépart()
        {
            //On prépare une requête pour récupérer tous les trajet
            Console.WriteLine("Sélectionnez un départ :");
            var queryTravels = from travel in myJsonTravels
                               let line = travel["fields"]["od"]
                               select line;
            //On ajoute tous les éléments de la requête dans la liste statipns
            foreach (var travel in queryTravels)
            {
                gares.Add(travel.ToString());
            }

            //On prepare une requete sur la liste stations pour supprimer tous ce qu'il y a apres le -,
            //afin de récupérer seulement les gares de départ
            var uniqueTravels = from travel in gares
                                let carac = travel.ToString().IndexOf("-")
                                let line = travel.ToString().Substring(0, carac)
                                orderby line
                                select line;

            //Pour éviter les doublons on trim la liste puis on met tous en majuscule
            uniqueTravels = uniqueTravels.Select(t => t.Trim().ToUpper());

            //Enfin on fait un distinct pour supprimer tous les doublons
            garesUnique = uniqueTravels.Distinct();
            Console.WriteLine("Voici les départs d'où vous pouvez partir : ");

            affiche4par4(garesUnique);
        }

        public void trierParPrix2ndclasse()
        {
            var queryTravels = from travel in myJsonTravels
                               orderby travel["fields"]["plein_tarif_loisir_2nde"]
                               let line = $"Voyage : {travel["fields"]["od"]} Prix (2eme classe) : {travel["fields"]["plein_tarif_loisir_2nde"]} EUR"
                               select line;
            foreach (var travel in queryTravels)
            {
                Console.WriteLine($"{travel} ");
            }
        }

        public void trierParPrix1erclasse()
        {
            var queryTravels = from travel in myJsonTravels
                               orderby travel["fields"]["plein_tarif_loisir_2nde"]
                               let line = $"Voyage : {travel["fields"]["od"]} Prix (1ere classe) : {travel["fields"]["1ere_classe"]} EUR"
                               select line;
            foreach (var travel in queryTravels)
            {
               Console.WriteLine($"{travel} ");     
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
