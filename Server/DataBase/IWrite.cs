using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    [ServiceContract]
    public interface IWrite
    {
        [OperationContract]
        string WriteInXML(List<Load> loads, List<Audit> audits);
    }
}
