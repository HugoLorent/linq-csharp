using linq_csharp.JSON;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace linq_csharp
{
    class Program
    {

        static void Main(string[] args)
        {
            bool restart = true;
            while (restart)
            {

                List<string> sources = new List<string>();
                sources.Add("json");
                sources.Add("xml");
                Console.WriteLine("Vos deux sources sont : json et xml");
                Console.WriteLine("Tapez pour continuer");
                Console.ReadLine();
                Console.WriteLine("Veuillez saisir votre source d'informations");
                string sourceVoulue = Console.ReadLine();
                bool isSourceCorrect = false;

                if (sourceVoulue == "json")
                {
                    TrainJSON json = new TrainJSON();
                    json.proposerDépart();
                }
                else if (sourceVoulue == "xml")
                {
                    Xml xml = new Xml();
                    xml.main();
                }
                else
                {
                    foreach (var source in sources)
                    {
                        if (source == sourceVoulue)
                        {
                            isSourceCorrect = true;
                        }
                    }
                    while (!isSourceCorrect)
                    {
                        Console.WriteLine("Votre source n'est pas correct, veulliez recommencer");
                        sourceVoulue = Console.ReadLine();

                        foreach (var source in sources)
                        {
                            if (source == sourceVoulue)
                            {
                                isSourceCorrect = true;
                            }
                        }
                    }
                }

                Console.WriteLine("Voulez-vous refaire une recherche ? Répondez par o ou n");
                string choix = Console.ReadLine();
                if (choix == "n")
                {
                    restart = false;
                }
            }
        }
    }
}
