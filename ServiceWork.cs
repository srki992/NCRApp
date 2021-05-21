using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace NCRApp
{
    public class ServiceWork
    {
        private readonly Timer _timer;

        public ServiceWork()
        {
            _timer = new Timer(1000) { AutoReset=true };
            _timer.Elapsed += TimerElapsed;
        }

        public ServiceWork(int intervalIzvrzavanja,string konekcioniString)
        {
            _timer = new Timer(intervalIzvrzavanja) { AutoReset = true };
            _timer.Elapsed += TimerElapsed;
            UpisiUBazuPodatke(konekcioniString, intervalIzvrzavanja);
        }

        private void UpisiUBazuPodatke(string konekcioniString,int intervalIzvrzavanja)
        {
            SqlConnection cn = new SqlConnection(konekcioniString);
            cn.Open();
            Console.WriteLine("Unesite broj (1,2 ili 3) koji cete HardwareType da upisete u tabelu Records:\n U ponudi su: \n 1	Intel Core i5	CPU, serial number: xxxx  \n 2	Samsung 500gb	HDD1, serial number: xxxx \n 3	Toshiba 250gb	HDD2, serial number xxxx");

            if (int.TryParse(Console.ReadLine(), out int brojRedaUnesen))
            {
                if(brojRedaUnesen > 3 || brojRedaUnesen < 0)
                {
                    Console.WriteLine("Morate uneti broj u ponudi 1,2 ili 3 ,sacekajte " + intervalIzvrzavanja + "ms pa pokusajte ponovo");
                    cn.Close();
                    return;
                }
            }
            else
            {
                Console.WriteLine("Greska kod unosa,sacekajte " + intervalIzvrzavanja + "ms pa pokusajte ponovo");
                cn.Close();
                return;
            }

            SqlCommand cmd = new SqlCommand("INSERT INTO Records(HardwareType, CreateDate) values('" + brojRedaUnesen + "','" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "')", cn);
            int i = cmd.ExecuteNonQuery();

            if (i > 0)
            {
                Console.WriteLine("Uspesno upisan red u bazi.");
            }
            else
            {
                Console.WriteLine("Greska prilikom upisa u tabelu Records.");
            }

            cn.Close();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            //upis u txt fajl da li odabrani izbor sa trenutnim datumom(brojRedaUnesen) ili celu tabele 
            string[] lines = new string[] { DateTime.Now.ToString() };
            File.AppendAllLines(@"C:\Temp\ServiceWork.txt", lines);
        }

        public void Start()
        {
            _timer.Start();
        }
        public void Stop()
        {
            _timer.Stop();
        }
    }
}
