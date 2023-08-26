using System;
using System.IO;

namespace Common
{
    public class ImportedFile : IImportedFile
    {
        public int Id { get; set; } // ID fajla
        public DateTime Timestamp { get; set; } // Vreme kada je fajl uvezen
        public string FileName { get; set; } // Naziv fajla
        public long FileSize { get; set; } // Veličina fajla
        public MemoryStream FileContent { get; set; } // MemoryStream koji sadrži sadržaj fajla

        public ImportedFile(int id, DateTime timestamp, string fileName, long fileSize, MemoryStream fileContent)
        {
            Id = id;
            Timestamp = timestamp;
            FileName = fileName;
            FileSize = fileSize;

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
