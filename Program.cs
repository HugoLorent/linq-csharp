using linq_csharp.JSON;
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
            Xml xml = new Xml();
            xml.main();
            TrainJSON json = new TrainJSON();
            // json.proposerDépart();


        }
    }
}
