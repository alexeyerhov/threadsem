using System.Net;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using System.Threading;


namespace threadsem
{
    class Program
    {
        private static int[] arrayFirst = { 1, 2, 3 };
        private static int[] arraySecond = { 6, 7, 8 };
        private static int sum1 = 0;
        private static int sum2 = 0;

        static void Main(string[] args)
        {
            //Напише приложение для одновременного выполнения двух задач в потоках. Нужно подсчитать сумму элементов каждого из массивов а потом сложить эти суммы полученные после выполнения каждого из потоков и вывести результат на экран.

            Thread thread1 = new Thread(SetSum1);
            Thread thread2 = new Thread(SetSum2);

            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();

            Console.WriteLine($"{sum1} + {sum2} = {sum1 + sum2}");
            Console.WriteLine();


            //Напишите многопоточное приложение, которое определяет все IP-адреса интернет-ресурса и определяет до которого из них лучше Ping.

            const string yaru = "ya.ru";

            IPAddress[] iPAddress = Dns.GetHostAddresses(yaru, System.Net.Sockets.AddressFamily.InterNetwork);


            Dictionary<IPAddress, long> pings = new Dictionary<IPAddress, long>();
            List<Thread> threads = new List<Thread>();

            foreach (var item in iPAddress)
            {
                var threadnew = new Thread(()=>
                {
                    Ping p = new Ping();
                    PingReply pingReply = p.Send(item);
                    pings.Add(item, pingReply.RoundtripTime);
                    Console.WriteLine($"{item} ping= {pingReply.RoundtripTime}");
                });
                threads.Add(threadnew);
                threadnew.Start();
            }

            foreach (var item in threads)
            {
                item.Join();
            }

            long minPing = pings.OrderByDescending( x => x.Value ).First().Value;
            Console.WriteLine(minPing.ToString());
        }

        public static void SetSum1()
        {
            sum1 = arrayFirst.Sum();
        }

        public static void SetSum2()
        {
            sum2 = arraySecond.Sum();
        }
    }
}