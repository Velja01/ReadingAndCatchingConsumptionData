using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataBase
{
    public class DatabaseService:IRead
    {
        public List<Load> ReadingCsvFile(MemoryStream csv, out List<Audit> errors)
        {
            errors = new List<Audit>();
            List<Load> loads = new List<Load>();

            int line = 1;

            using (StreamReader csvStream = new StreamReader(csv))
            {
                string csvData = csvStream.ReadToEnd();
                string[] csvRows = csvData.Split('\n');

                // Preskačemo prvi red kako bismo izbegli zaglavlje (naslovnu liniju).
                // Koristimo i Length - 1 kako bismo izbegli poslednji red koji može biti prazan.
                string[] rows = csvRows.Skip(1).Take(csvRows.Length - 2).ToArray();

                foreach (var row in rows)
                {
                    string[] split = row.Split(',');

                    // Očekujemo da split ima četiri elementa: vremenski pečat, prognoziranu vrednost,
                    // izmerenu vrednost i nešto drugo (možda indeks sata ili nešto slično).
                    if (split.Length == 4)
                    {
                        if (DateTime.TryParse(split[0]+" " + split[1], out DateTime timestamp) &&
                            double.TryParse(split[2], out double forecastValue) &&
                            double.TryParse(split[3], out double measuredValue))
                        {
                            // Ako su svi podaci ispravno parsirani, kreiramo Load objekat i dodajemo ga u listu.
                            Load load = new Load(line, timestamp, forecastValue, measuredValue);
                            loads.Add(load);
                            Audit error = new Audit(line, DateTime.Now, MESSAGETYPE.INFO, "Data has been successfully loaded");
                            
                        }
                        else
                        {
                            // U suprotnom, dodajemo grešku u listu grešaka.
                            Audit error = new Audit(line, DateTime.Now, MESSAGETYPE.ERROR, "Invalid data format");
                            errors.Add(error);
                        }
                    }
                    else
                    {
                        // Ako split nema četiri elementa, dodajemo grešku u listu grešaka.
                        Audit error = new Audit(line, DateTime.Now, MESSAGETYPE.ERROR, "Invalid number of columns");
                        errors.Add(error);
                    }

                    line++; // Povećavamo brojač linija.
                }
            }

            return loads; // Vraćamo listu Load objekata.
        }
        public void WriteInXML(List<Load> loads, List<Audit> errors)
        {
            WriteAudit(errors, ConfigurationManager.AppSettings["DataBaseAudits"]);
            WriteLoad(loads, ConfigurationManager.AppSettings["DataBaseLoads"]);
        }
        private void WriteLoad(List<Load> loads, string path)
        {
            int rows = 0;
            IImportedFile file = new DatabaseService().OpenFile(path);

        }
        public IImportedFile OpenFile(string path)
        {
            MemoryStream ms= new MemoryStream();

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
                XDocument XMLDocument= new XDocument(xdeclaration, xelement);

                XMLDocument.Save(path);

            }
            using (FileStream fs=new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                fs.CopyTo(fs);
                fs.Dispose();
            }

            ms.Position = 0;

            return new ImportedFile(DateTime.Now, Path.GetFileName(path), ms);

        }
    }
}
