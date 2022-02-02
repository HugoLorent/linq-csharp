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
        List<String> gares = new List<string>();
        IEnumerable<String> garesUnique = new List<string>();


        int compteurAffichage = 1;
        int espaceAAjouter = 0;
        int espaceAAjouterADroite = 0;

        string rechercheDepart;
        string rechercheArrivee;

        public void main()
        {
            showAllTravels();
            askDepart();
            askArrivee();
        }

        public void showAllTravels()
        {
            var xmlTravels = XElement.Load($@"{Directory.GetCurrentDirectory()}/XML/tarifs-tgv-par-od.xml");
            Console.WriteLine("Veuillez taper une touche pour voir la liste de tous les trajets disponibles");
            Console.ReadLine();
            var allTravels = from travel in xmlTravels.Descendants("fields")
                             let od = travel.Element("od").Value
                             select od;

            foreach (var travel in allTravels)
            {
                Console.WriteLine(travel);
            }
            Console.WriteLine();
        }

        public void askDepart()
        {
            var xmlTravels = XElement.Load($@"{Directory.GetCurrentDirectory()}/XML/tarifs-tgv-par-od.xml");
            Console.WriteLine("D'où voulez-vous partir ?");
            rechercheDepart = Console.ReadLine();
            Console.WriteLine();

            var departRequest = from travel in xmlTravels.Descendants("fields")
                                let od = travel.Element("od").Value
                                where od.Contains(rechercheDepart, StringComparison.InvariantCultureIgnoreCase)
                                select getDepartFromRequest(od, rechercheDepart);

            foreach (var travel in departRequest)
            {
                if (travel != "")
                {
                    Console.WriteLine(travel);
                }
            }
            Console.WriteLine();
        }

        public void askArrivee()
        {
            var xmlTravels = XElement.Load($@"{Directory.GetCurrentDirectory()}/XML/tarifs-tgv-par-od.xml");
            Console.WriteLine("Où voulez-vous aller depuis votre destination de départ ?");
            rechercheArrivee = Console.ReadLine();
            Console.WriteLine();

            var arriveeRequest = from travel in xmlTravels.Descendants("fields")
                                 let od = travel.Element("od").Value
                                 let priceSecond = travel.Element("plein_tarif_loisir_2nde").Value
                                 let priceFirst = travel.Element("premiere_classe").Value
                                 where od.Contains(rechercheArrivee, StringComparison.InvariantCultureIgnoreCase)
                                 select getArriveeFromDepart(od, rechercheDepart, rechercheArrivee);

            foreach (var travel in arriveeRequest)
            {
                if (travel != "")
                {
                    Console.WriteLine(travel);
                }
            }
        }

        public string getArriveeFromDepart(string od, string rechercheDepart, string rechercheArrivee)
        {
            if (od.Substring(0, od.IndexOf("-")).Contains(rechercheDepart, StringComparison.InvariantCultureIgnoreCase) &&
                od.Substring(od.IndexOf("-")).Contains(rechercheArrivee, StringComparison.InvariantCultureIgnoreCase))
            {
                return od;
            }
            else
            {
                return "";
            }
        }

        public string getDepartFromRequest(string od, string recherche)
        {
            if (od.Substring(0, od.IndexOf("-")).Contains(recherche, StringComparison.InvariantCultureIgnoreCase))
            {
                return od;
            } 
            else
            {
                return "";
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
