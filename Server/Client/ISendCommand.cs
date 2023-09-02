using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    [ServiceContract]
    public interface ISendCommand
    {
        [OperationContract]
        List<Load> sendCommand(string path, List<string>ucitani);

        [OperationContract]
        string WriteAudit(string fileName);
    }
}
