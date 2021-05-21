using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace NCRApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string conectionString = "Data Source=.;Initial Catalog=NCRtestbaza;Integrated Security=True";
            int vremeIzvrsavanja = 2000;//5 min 300000

            var exitCode = HostFactory.Run(x => 
            {
                x.Service<ServiceWork>(s => 
                {
                    //s.ConstructUsing(serviceWork => new ServiceWork());
                    s.ConstructUsing(serviceWork => new ServiceWork(vremeIzvrsavanja, conectionString));
                    s.WhenStarted(serviceWork => serviceWork.Start());
                    s.WhenStopped(serviceWork => serviceWork.Stop());

                });

                x.RunAsLocalSystem();

                x.SetServiceName("NCRTestService");//nospacename
                x.SetDisplayName("NCR test service");
                x.SetDescription("Ovo je testni servis i radi CRUD operacije nad MSSQL bazom");
            });

            int exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;
        }
    }
}
