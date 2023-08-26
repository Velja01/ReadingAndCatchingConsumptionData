using Common;
using DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Service : IReadCSV
    {
        public List<Load> ReadCSV(string path)
        {
            List<Load> list = new List<Load>();
            ChannelFactory<IRead> DataBaseRead = new ChannelFactory<IRead>("DataBase");
            IRead database_channel=DataBaseRead.CreateChannel();
            List<Audit>errors=new List<Audit>();
            list = database_channel.ReadingCsv(path, out errors);

        }
    }
}
