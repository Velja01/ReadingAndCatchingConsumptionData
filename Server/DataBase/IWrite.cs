﻿using Common;
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
    public interface IWrite
    {
        [OperationContract]
        string WriteAuditError(MemoryStream ms);
    }
}
