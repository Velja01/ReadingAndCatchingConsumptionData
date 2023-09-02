using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class In_Memory
    {
        [DataMember]
        public Dictionary<int, Load> dictionary { get; private set; }

        // Događaj o obaveštenju brisanja zastarelih podataka
        public event EventHandler CleanupEvent;

        private int DataTimeoutMinutes;

        public In_Memory(int dataTimeoutMinutes)
        {
            DataTimeoutMinutes = dataTimeoutMinutes;
            dictionary = new Dictionary<int, Load>();
        }

        public void AddOrUpdate(int key, Load value)
        {
            lock (dictionary)
            {
                dictionary[key] = value;
            }
        }

        public bool TryGetValue(int key, out Load value)
        {
            lock (dictionary)
            {
                if (dictionary.TryGetValue(key, out value))
                {
                    // Provera zastarelosti podataka
                    if (IsDataExpired(value))
                    {
                        dictionary.Remove(key);
                        value = null;
                        // Dizanje događaja o čišćenju
                        OnCleanupEvent();
                        return false;
                    }
                    return true;
                }
                return false;
            }
        }

        // Ostale metode i funkcionalnosti

        private bool IsDataExpired(Load load)
        {
            if (load == null)
            {
                return false; // Ako podatak ne postoji, smatramo da nije istekao
            }

            // Računanje vremena proteklog od trenutka dodavanja podatka
            var elapsedMinutes = (DateTime.Now - load.Timestamp).TotalMinutes;

            // Provera da li je vreme proteklo prema definisanom vremenu zastarelosti
            return elapsedMinutes >= DataTimeoutMinutes;
        }

        // Dizanje događaja o čišćenju
        protected virtual void OnCleanupEvent()
        {
            CleanupEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
