using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Web;
using inin.Bridge.WebServices.Datadip.Lib;
using System.ServiceModel;


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
            Uri baseUri = new Uri("http://127.0.0.1:" + port);
            Uri customUri = new Uri("http://127.0.0.1:" + port + "/custom");
            WebServiceHost _serviceHost = new WebServiceHost(DemoServices);
            _serviceHost.AddServiceEndpoint(typeof(IWebServicesServer),new WebHttpBinding(),baseUri);

            _serviceHost.AddServiceEndpoint(typeof(ICustomAction),new WebHttpBinding(),customUri);
            

            _serviceHost.Open();
            Console.ReadKey();
            _serviceHost.Close();

        }
    }
}
