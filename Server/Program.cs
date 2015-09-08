using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Web;


namespace inin.Bridge.WebServices.Datadip.Impl
{
    class Program
    {
        static void Main(string[] args)
        {
            string port = "8889";
            if (args == null || args.Length <= 0)
            {
                throw new Exception("Not enough arguments... Required arguments: Storage directory, optional arguments: port");
            }
            String storageDir = args[0];
            if (args.Length > 1)
            {
                port = args[1];
            }
            Console.WriteLine("Storage directory: " + storageDir);
            Console.WriteLine("Listening on port: " + port);
            SampleWebServicesImplementation DemoServices = new SampleWebServicesImplementation(storageDir);
            WebServiceHost _serviceHost = new WebServiceHost(DemoServices, new Uri("http://127.0.0.1:" + port));

            _serviceHost.Open();
            Console.ReadKey();
            _serviceHost.Close();

        }
    }
}
