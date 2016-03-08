using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RoboUtil.managers;

namespace RoboUtil.Examples
{
    /// <summary>
    /// Bu yontemde var olan thread orneklerine bir is paketi esit olarak paylastiriliyor, ve 
    /// her bir thread isini bitirince ManuelResetEvent i setleyerek thread sonlandiriliyor.
    /// Bu yontemde bir is kuyrugu olmadigi icin isler basindan belirli olmali ve dagitilmali
    /// is kuyrugu concurrentDictionary oldugu icin threadleri bekletme ihtimali olabilir, bu yuzden
    /// kuyruksuz bir pool olusturmak istenebilir. her iki yontem icin 10 thread ve 10000 adet 100ms suren is 
    /// karsilastirildi bir fark gorulmedi.
    /// </summary>
    public class ThradPoolExample
    {
        public static void Example1()
        {
            int askerAdedi = 10;
            int kursunAdedi = 1000;

            ManualResetEvent[] hucumBitti = new ManualResetEvent[askerAdedi];

            Console.WriteLine("Saldiri {0} Askerle basliyor", askerAdedi);
            for (int i = 0; i < askerAdedi; i++)
            {
                hucumBitti[i] = new ManualResetEvent(false);
                Asker asker = new Asker(kursunAdedi, hucumBitti[i]);
                ThreadPool.QueueUserWorkItem(asker.Hucum, i);
            }

            WaitHandle.WaitAll(hucumBitti);
            Console.WriteLine("Tum askerler saldiriyi sonlandirdi...");

            //Console.ReadKey();
        }
    }

    public class Asker
    {
        public int _kursun { get; set; }

        public ManualResetEvent _hucumBitti { get; set; }

        public Asker(int kursun, ManualResetEvent hucumBitti)
        {
            _kursun = kursun;
            _hucumBitti = hucumBitti;
        }


        public void Hucum(object askerNo)
        {
            int AskerNo = (int) askerNo;
            Console.WriteLine("Asker {0} saldiriyor...",AskerNo);
            _kursun = AtesEt(_kursun, AskerNo);
            Console.WriteLine("Asker {0} hucumu bitirdi...", AskerNo);
            _hucumBitti.Set();

        }

        private int AtesEt(int kursun, int askerNo)
        {
            for (int i = 0; i < kursun; i++)
            {
                Thread.Sleep(100);
                Console.WriteLine("Asker:{0} Ates {1}.kursun...",askerNo,i);
            }
            return 0;//kursunu bitirene kadar ates etti
        }
    }
}
