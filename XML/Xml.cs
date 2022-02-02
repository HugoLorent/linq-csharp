using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace linq_csharp
{
    class Xml
    {
        XElement xmlTravels = XElement.Load($@"{Directory.GetCurrentDirectory()}/XML/tarifs-tgv-par-od.xml");

        List<String> travels = new List<string>();
        IEnumerable<String> departs = new List<string>();
        IEnumerable<String> arrivees = new List<string>();

        int compteurAffichage = 1;
        int espaceAAjouter = 0;
        int espaceAAjouterADroite = 0;

        string rechercheDepart;
        string rechercheArrivee;

        public void main()
        {
            showAllDeparts();
            askDepart();
            askArrivee();
        }

        public void showAllDeparts()
        {
            Console.WriteLine("Veuillez taper une touche pour voir la liste de toutes les gares de départs disponibles");
            Console.ReadLine();
            var allTravels = from travel in xmlTravels.Descendants("fields")
                             let od = travel.Element("od").Value
                             select od;

            foreach (var travel in allTravels)
            {
                travels.Add(travel.ToString());
            }

            var departReq = from travel in travels
                            let carac = travel.ToString().IndexOf("-")
                            let line = travel.ToString().Substring(0, carac)
                            orderby line
                            select line;

            departReq = departReq.Select(t => t.Trim().ToUpper());
            departs = departReq.Distinct();
            print4By4(departs);
        }

        public void askDepart()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("D'où voulez-vous partir ?");
            rechercheDepart = Console.ReadLine().ToUpper();
            Console.WriteLine();
            bool isGareCorrect = false;

            foreach (var gare in departs)
            {
                if (rechercheDepart == gare)
                {
                    isGareCorrect = true;
                }
            }
            if (!isGareCorrect)
            {
                while (!isGareCorrect)
                {
                    Console.WriteLine("Cette gare n'existe pas, veuillez retaper votre destination :");
                    rechercheDepart = Console.ReadLine().ToUpper();
                    foreach (var gare in departs)
                    {
                        if (rechercheDepart == gare)
                        {
                            isGareCorrect = true;
                        }
                    }
                }
            }

            var departRequest = from travel in xmlTravels.Descendants("fields")
                                let od = travel.Element("od").Value
                                where od.Contains(rechercheDepart)
                                select od
                                into tousTrajets
                                let carac = tousTrajets.ToString().IndexOf("-") + 1
                                let arrivee = tousTrajets.ToString().Substring(carac)
                                select arrivee;

            departRequest = departRequest.Select(t => t.Trim().ToUpper());
            arrivees = departRequest.Where(i => !i.Contains(rechercheDepart));
            print4By4(arrivees);
        }

        public void askArrivee()
        {
            Console.WriteLine("");
            Console.WriteLine("Où voulez-vous arriver ? ");
            rechercheArrivee = Console.ReadLine().ToUpper();
            bool isGareCorrect = false;

            //On vérifie que la gare recherchée existe avec le départ choisi
            foreach (var gare in arrivees)
            {
                if (rechercheArrivee == gare)
                {
                    isGareCorrect = true;
                }
            }
            if (!isGareCorrect)
            {
                while (!isGareCorrect)
                {
                    Console.WriteLine("Cette gare d'arrivée n'est pas disponible avec votre départ, veuillez entrer une arrivée disponible");
                    rechercheArrivee = Console.ReadLine().ToUpper();
                    foreach (var gare in arrivees)
                    {
                        if (rechercheArrivee == gare)
                        {
                            isGareCorrect = true;
                        }
                    }
                }
            }

            var arriveeRequest = from travel in xmlTravels.Descendants("fields")
                                 let od = travel.Element("od").Value
                                 let secondPrice = travel.Element("plein_tarif_loisir_2nde").Value
                                 let firstPrice = travel.Element("premiere_classe").Value
                                 where od.Contains(rechercheDepart) && od.Contains(rechercheArrivee) && od.StartsWith(rechercheDepart)
                                 select $"Votre trajet : {od}, pour le prix 2nd classe {secondPrice}EUR, pour le prix 1er classe {firstPrice}EUR";


            foreach (var travel in arriveeRequest)
            {
                Console.WriteLine(travel);
            }
        }

        public void sortBySecondPrice()
        {
            var reqSecond = from travel in xmlTravels.Descendants("fields")
                            orderby travel.Element("plein_tarif_loisir_2nde")
                            let ligne = $"Voyage : {travel.Element("od")} Prix (2eme classe) : {travel.Element("plein_tarif_loisir_2nde")} EUR"
                            select ligne;

            foreach (var travel in reqSecond)
            {
                Console.WriteLine(travel);
            }
        }

        public void sortByFirstPrice()
        {
            var reqFirst = from travel in xmlTravels.Descendants("fields")
                            orderby travel.Element("premiere_classe")
                            let ligne = $"Voyage : {travel.Element("od")} Prix (1ere classe) : {travel.Element("premiere_classe")} EUR"
                            select ligne;

            foreach (var travel in reqFirst)
            {
                Console.WriteLine(travel);
            }
        }

        public void print4By4(IEnumerable<String> listeAAfficher)
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
