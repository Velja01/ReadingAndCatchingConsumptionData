using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    [ServiceContract]
    public interface IRead
    {
        [OperationContract]
        List<Load> ReadingCsvFile(MemoryStream ms, out List<Audit> errors);
    }
}
