using System;
using System.ServiceModel;

namespace StarRepublic.Ipmc.PrintTinkerer.Proxy
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new ServiceHost(typeof(CcrProxyService));

            host.Open();
            Console.WriteLine("CCR Proxy is running.");
            Console.WriteLine("Press enter to quit...");
            Console.ReadLine();
            host.Close();
        }
    }
}
