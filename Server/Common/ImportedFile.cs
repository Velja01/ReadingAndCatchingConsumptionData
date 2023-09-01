using System;
using System.IO;
using System.Runtime.Serialization;

namespace Common
{
    [DataContract]
    public class ImportedFile : IImportedFile
    {
        
        [DataMember]
        public DateTime Timestamp { get; set; } // Vreme kada je fajl uvezen
        [DataMember]
        public string FileName { get; set; } // Naziv fajla
        [DataMember]
        public MemoryStream MemoryStream { get; set; } // MemoryStream koji sadrži sadržaj fajla

        public ImportedFile(DateTime timestamp, string fileName, MemoryStream fileContent)
        {
           
            Timestamp = timestamp;
            FileName = fileName;
            

            MemoryStream = fileContent;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //oslobadjanje upravljanih resursa
                if (MemoryStream != null)
                {
                    MemoryStream.Dispose();
                    MemoryStream = null;
                }
            }
        }
        ~ImportedFile()
        {
            Dispose(false);
        }
    }
}
