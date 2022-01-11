using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;

namespace linq_csharp
{
    class Program
    {

        static void Main(string[] args)
        {
            //Console.WriteLine("Recherche un trajet :");
            //string recherche = Console.ReadLine();
            var myJsonTravels = JObject.Parse(File.ReadAllText($@"{Directory.GetCurrentDirectory()}/JSON/tarifs-tgv-par-od.json"));

            Console.WriteLine(myJsonTravels);

            var queryTravels = from travel in myJsonTravels["AllTravels"]["fields"]
                            let line = travel["od"].ToString()
                            //where line.Contains(recherche, StringComparison.InvariantCultureIgnoreCase)
                            select line;
            foreach (var travel in queryTravels)
            {
                Console.WriteLine(travel);
            }
        }
    }
}
