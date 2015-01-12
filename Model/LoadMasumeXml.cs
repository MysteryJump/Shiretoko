using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shiretoko.Model
{
    public class LoadMasumeXml
    {
        public List<Masume> MasumeList { get; set; }
        public LoadMasumeXml()
        {
            var xdoc = XDocument.Parse(Properties.Resources.DefineMasume);
            var data = xdoc.Element("GameData").Element("Collection").Elements("Masume");
            var masuList = new List<Masume>();
            data.ToList().ForEach((x) =>
                {

                    var masume = new Masume
                    {
                        Index = int.Parse(x.Element("Index").Value),
                        Coordinate = x.Element("Coordinate")?.Value,
                        FunctionName = x.Element("Function").Value,
                        Name = x.Element("Name").Value
                    };
                    int i = 0;
                    masume.JctIndex = int.TryParse((x.Element("JctIndex")?.Value), out i) ? i : -1;
                    masuList.Add(masume);
                });
            this.MasumeList = masuList;
        }

    }
}
