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
        public MemoryStream FileContent { get; set; } // MemoryStream koji sadrži sadržaj fajla

        public ImportedFile(DateTime timestamp, string fileName, MemoryStream fileContent)
        {
           
            Timestamp = timestamp;
            FileName = fileName;
            

            FileContent = fileContent;
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
                if (FileContent != null)
                {
                    FileContent.Dispose();
                    FileContent = null;
                }
            }
        }
        ~ImportedFile()
        {
            Dispose(false);
        }
    }
}
