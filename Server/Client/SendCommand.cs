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
using System.Xml.Serialization;

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

        public string WriteAudit(string fileName)
        {
            MemoryStream ms=new MemoryStream();
            ChannelFactory<IWriteCSV>ChannelWriteAudit=new ChannelFactory<IWriteCSV>("WriteServer");
            IWriteCSV proxy = ChannelWriteAudit.CreateChannel();
            Audit a1 = new Audit(1, DateTime.Now, MESSAGETYPE.ERROR, "U bazi podataka ne postoje podaci za datum " + fileName);
            XmlSerializer serializer=new XmlSerializer(typeof(Audit));
            serializer.Serialize(ms, a1);
            ms.Position = 0;
            string result=proxy.WriteCSV(ms);
            return result;
        }
    }
}
