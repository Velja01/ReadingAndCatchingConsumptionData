using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Audit> errors = new List<Audit>();
            
            do
            {
                Console.WriteLine("Unesite datum za koji ocekujete podatke! (format neka bude tipa yyyy_mm_dd) \nza izlazak komanda break!");
                string inputCommand = Console.ReadLine();
                string command = inputCommand.Trim();
                //postavljanje formata datuma koji odgovara nazivu file-a
                string pattern = @"^\d{4}_\d{2}_\d{2}$";
                Regex regex = new Regex(pattern);
                if (inputCommand == "break")
                {
                    break;
                }
                if (regex.IsMatch(command))
                {
                    Console.WriteLine("String je unet u ispravnom formatu");
                    //konkatenacija kako bih dobili pun naziv fajla na osnovu unetog datuma
                    string fileName = "results_" + command + ".csv";
                    //Definisanje relativne putanje sa CSV datotekama
                    string folderPath = @"C:\Users\veljk\Desktop\zadatak_3\primer_kreiranih_csv_datoteka";
                    //Puna putanja do foldera sa CSV datotekama
                    string fullPath = Path.Combine(folderPath, fileName);

                    if (File.Exists(fullPath))
                    {
                        Console.WriteLine("Pronadjena csv datoteka za zadati datum");
                        List<Load> recvMessage = new SendCommand().sendCommand(fullPath);
                        foreach (Load l in recvMessage)
                        {
                            Console.WriteLine($"id:{l.Id}, FValue:{l.ForecastValue}, MValue:{l.MeasuredValue} TimeStamp:{l.Timestamp}");
                        }
                        if (recvMessage.Count != 0)
                        {
                            Console.WriteLine("Ucitani su objekti za odogovarajuci datum");
                            Console.WriteLine("Sledi cuvanje podataka u CSV datoteke");
                            Console.WriteLine("Podaci su sacuvani u TBL_LOAD.xml i TBL_AUDIT.xml na lokaciji C:\\Users\\veljk\\Desktop\\zadatak_3\\Reading And Catching Consumption Data\\ReadingAndCatchingConsumptionData\\Server\\DataBase\\bin\\Debug\\");

                        }
                        else
                        {
                            Console.WriteLine("Nisu ucitani");
                        }

                    }
                    else
                    {
                        Console.WriteLine("Nije pronadjena ni jedna csv datoteka za uneti datum");
                        string recvMessage = new SendCommand().WriteAudit(command);
                        Console.WriteLine(recvMessage);
                    }
                }
                else
                {
                    Console.WriteLine("String nije unet u ispravnom formatu");
                }

            } while (true);
        }
    }
}
