using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    [ServiceContract]
    public interface IWriteCSV
    {
        [OperationContract]
        string WriteCSV(List<Load> loads, List<Audit> audits);
    }
}
