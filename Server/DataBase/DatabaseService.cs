using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    public class DatabaseService
    {
        public string ParseFile(MemoryStream csv, out List<Audit> errors)
        {
            errors = new List<Audit>();
            List<Load> loads = new List<Load>();

            int line = 1;

            using (StreamReader csvStream=new StreamReader(csv))
            {
                string csvData=csvStream.ReadToEnd();
                string[] csvRows = csvData.Split('\n');
                string[] rows = csvRows.Take(csvRows.Length - 1).ToArray();
                foreach(var row in rows)
                {
                    string[] split=row.Split(',');

                }
            }

        }
    }
}
