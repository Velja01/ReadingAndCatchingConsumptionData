using Common;
using DataBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Service : IReadCSV, IWriteCSV
    {
        public List<Load> ReadCSV(MemoryStream ms)
        {
            List<Load> list = new List<Load>();
            ChannelFactory<IRead> DataBaseRead = new ChannelFactory<IRead>("DataBase");
            IRead database_channel=DataBaseRead.CreateChannel();
            List<Audit>errors=new List<Audit>();
            list = database_channel.ReadingCsvFile(ms, out errors);
            return list;
        }
        public string WriteCSV(List<Load> loads, List<Audit> audits)
        {
            string recvMessage = "";
            ChannelFactory<IWrite> DataBaseWrite = new ChannelFactory<IWrite>("DataBase");
            IWrite database_channel = DataBaseWrite.CreateChannel();
            recvMessage = database_channel.WriteInXML(loads, audits);
            return recvMessage;
        }
    }
}
