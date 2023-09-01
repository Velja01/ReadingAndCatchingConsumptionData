using Common;
using Server;
using System;
using System.CodeDom.Compiler;
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
        public List<Load> sendCommand(string path)
        {

            MemoryStream ms = new MemoryStream();
            string csvPath = path;

            ChannelFactory<IReadCSV> channelReadCSV = new ChannelFactory<IReadCSV>("ReadServer");
            IReadCSV proxy = channelReadCSV.CreateChannel();
            using (FileStream csvStream = new FileStream(csvPath, FileMode.Open, FileAccess.Read))
            {
                csvStream.CopyTo(ms);
                csvStream.Dispose();
            }
            ms.Position = 0;

            using (IImportedFile file = new ImportedFile(DateTime.Now, "csvData", ms))
            {

                List<Load> csvInf = proxy.ReadCSV(ms);

                return csvInf;
            }

        }


    }
}
