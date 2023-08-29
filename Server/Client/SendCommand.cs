using Common;
using Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class SendCommand : ISendCommand
    {
        public string sendCommand(string path, out List<Audit> errors)
        {
            int id = 1;
            MemoryStream ms= new MemoryStream();
            string csvPath = path;
            errors = new List<Audit>();
            ChannelFactory<IReadCSV> channelReadCSV= new ChannelFactory<IReadCSV>("Server");
            IReadCSV proxy = channelReadCSV.CreateChannel();
            using (FileStream csvStream=new FileStream(csvPath, FileMode.Open, FileAccess.Read))
            {
                csvStream.CopyTo(ms);
                csvStream.Dispose();
            }
            ms.Position = 0;

            using (IImportedFile file = new ImportedFile(id, DateTime.Now, "csvData", ms))
            {

                List<Load> csvInf = proxy.ReadCSV(ms);

                return csvInf.ToString();
            }
            
        }
    }
}
