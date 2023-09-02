﻿using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DataBase
{

    public class DatabaseService : IRead, IWrite
    {

        public List<Load> ReadingCsvFile(MemoryStream csv)
        {
           
            List<Load> loads = new List<Load>();

            int line = 1;

            DateTime currentTime=DateTime.Now;
            TimeSpan dataTimeout=TimeSpan.FromMinutes(15);

            using (StreamReader csvStream = new StreamReader(csv))
            {
                string csvData = csvStream.ReadToEnd();
                string[] csvRows = csvData.Split('\n');
                Dictionary<int, Load> map = new Dictionary<int, Load>();
                // Preskačemo prvi red kako bismo izbegli zaglavlje (naslovnu liniju).
                // Koristimo i Length - 1 kako bismo izbegli poslednji red koji može biti prazan.
                string[] rows = csvRows.Skip(1).Take(csvRows.Length - 2).ToArray();

                foreach (var row in rows)
                {
                    string[] split = row.Split(',');

                    // Očekujemo da split ima četiri elementa
                    if (split.Length == 4)
                    {
                        if (DateTime.TryParse(split[0] + " " + split[1], out DateTime timestamp) &&
                            double.TryParse(split[2], out double forecastValue) &&
                            double.TryParse(split[3], out double measuredValue))
                        {
                            // Ako su svi podaci ispravno parsirani, kreiramo Load objekat i dodajemo ga u listu.
                            Load load = new Load(line, timestamp, forecastValue, measuredValue);
                            loads.Add(load);
                            
                            
                        }
                        
                    }
                    

                    line++; // Povećavamo brojač linija.
                }
            }
            Audit a1=(new Audit(line, DateTime.Now, MESSAGETYPE.INFO, "Podaci uspesno procitani i prosledjeni"));
            string recvMessage = WriteInXML(loads, a1);
            return loads; // Vraćamo listu Load objekata.
        }
        public string WriteInXML(List<Load> loads, Audit audit)
        {
            WriteAudit(audit, ConfigurationManager.AppSettings["DataBaseAudit"]);
            return WriteLoad(loads, ConfigurationManager.AppSettings["DataBaseLoads"]);
        }
        private static string WriteLoad(List<Load> loads, string path)
        {
            int rows = 0;
            IImportedFile file = new DatabaseService().ReadFile(path);

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(((ImportedFile)file).MemoryStream);
            ((ImportedFile)file).MemoryStream.Position = 0;

            XDocument xmlfile = XDocument.Load(((ImportedFile)file).MemoryStream);
            var items = xmlfile.Element("rows");


            foreach (Load l in loads)
            {
                string isNull = "//row[TIME_STAMP='" + l.Timestamp.ToString("yyyy-MM-dd HH:mm") + "']";
                XmlNode element = null;

                try
                {
                    element = xmlDocument.SelectSingleNode(isNull);
                }
                catch { }


                if (element == null)
                {
                    XElement insertElement = new XElement("row",
                        new XElement("TIME_STAMP", l.Timestamp.ToString("yyyy-MM-dd HH:mm")),
                        new XElement("FORECAST_VALUE", l.ForecastValue.ToString()),
                        new XElement("MEASURED_VALUE", l.MeasuredValue.ToString()));

                    items.Add(insertElement);
                    xmlfile.Save(ConfigurationManager.AppSettings["DataBaseLoads"]);

                    rows++;

                }
                else
                {
                    element.SelectSingleNode("FORECAST_VALUE").InnerText = l.ForecastValue.ToString();
                    element.SelectSingleNode("MEASURED_VALUE").InnerText = l.MeasuredValue.ToString();
                    xmlDocument.Save(ConfigurationManager.AppSettings["DataBaseLoads"]);
                    rows++;
                }

            }

            return "Izvrsen je upis objekata u TBL_LOAD.xml";

        }
        private static void WriteAudit(Audit audit, string path)
        {
            using (IImportedFile f = new DatabaseService().ReadFile(path))
            {
                XDocument xmlf = XDocument.Load(((ImportedFile)f).MemoryStream);

                // Provera da li postoji "STAVKE" element
                XElement stavkeElement = xmlf.Root.Element("STAVKE");
                if (stavkeElement == null)
                {
                    // Ako ne postoji, kreirajte ga i dodajte u korenski element
                    stavkeElement = new XElement("STAVKE");
                    xmlf.Root.Add(stavkeElement);
                }

                var elements = xmlf.Descendants("ID");
                int maxRows = 0;

                foreach (var element in elements)
                {
                    int value;
                    if (int.TryParse(element.Value, out value))
                    {
                        if (value > maxRows)
                        {
                            maxRows = value;
                        }
                    }
                }

                if (audit.MessageType == MESSAGETYPE.INFO)
                {

                    var items = stavkeElement; // Sada koristimo "STAVKE" element
                    var insertElement = new XElement("row");

                    insertElement.Add(new XElement("ID", ++maxRows));
                    insertElement.Add(new XElement("TIME_STAMP", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                    insertElement.Add(new XElement("MESSAGE_TYPE", "Info"));
                    insertElement.Add(new XElement("MESSAGE", "Podaci uspesno procitani i prosledjeni"));

                    items.Add(insertElement);
                    xmlf.Save(ConfigurationManager.AppSettings["DataBaseAudit"]);
                }
                else
                {
                    var items = stavkeElement; // Sada koristimo "STAVKE" element
                    var insertElement = new XElement("row");

                    insertElement.Add(new XElement("ID", ++maxRows));
                    insertElement.Add(new XElement("TIME_STAMP", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                    insertElement.Add(new XElement("MESSAGE_TYPE", "Error"));
                    insertElement.Add(new XElement("MESSAGE", "U bazi podataka ne postoje podaci za datum "+audit.Message));

                    items.Add(insertElement);
                    xmlf.Save(ConfigurationManager.AppSettings["DataBaseAudit"]);
                }

            }

        }
        public IImportedFile ReadFile(string path)
        {
            MemoryStream ms = new MemoryStream();

            if (!File.Exists(path))
            {
                string root = null;

                //Na osnovu tipa file-a odlucujemo kog tipa ce biti tag
                if (path.ToLower().Contains("audit"))
                {
                    root = "STAVKE";
                }
                else
                {
                    root = "rows";
                }
                XDeclaration xdeclaration = new XDeclaration("1.0", "utf-8", "no");
                XElement xelement = new XElement(root);
                XDocument XMLDocument = new XDocument(xdeclaration, xelement);

                XMLDocument.Save(path);

            }
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                fs.CopyTo(ms);
                fs.Dispose();
            }

            ms.Position = 0;

            return new ImportedFile(DateTime.Now, Path.GetFileName(path), ms);

        }

        public string WriteAuditError(MemoryStream ms)
        {
            ms.Position = 0;
            XmlSerializer serializer = new XmlSerializer(typeof(Audit));
            Audit a1=(Audit)serializer.Deserialize(ms);
            WriteAudit(a1, ConfigurationManager.AppSettings["DataBaseAudit"]);
            return "Podaci su upisani u TBL_AUDIT.xml";
        }
    }
}
