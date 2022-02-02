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
        public void proposerDepart()
        {
            Console.WriteLine("Ou voulez-vous aller ?");
            var rechercheArrivee = Console.ReadLine();
            var xmlTravels = XElement.Load($@"{Directory.GetCurrentDirectory()}/XML/tarifs-tgv-par-od.xml");

            var travelRequest = from travel in xmlTravels.Descendants("fields")
                                let arrivee = travel.Element("od").Value
                                where arrivee.Contains(rechercheArrivee, StringComparison.InvariantCultureIgnoreCase)
                                select arrivee;

            foreach (var travel in travelRequest)
            {
                Console.WriteLine(travel);
            }
        }
    }
}
