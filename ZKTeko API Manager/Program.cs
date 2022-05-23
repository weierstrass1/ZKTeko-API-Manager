using System;
using System.Collections.Generic;
using ZKTekoLibrary;
using ZKTekoLibrary.DAO;

namespace ZKTeko_API_Manager
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> argsL = new List<string>();
            if (args != null && args.Length > 0)
                argsL = new List<string>(args);

            Settings.UseSerialNumberList = argsL.Contains("--serial");

            if (argsL.Contains("--hour"))
                ZKTekoAPIManager.GetPlatformHourProcess();
            else if(argsL.Contains("--hour"))
                ZKTekoAPIManager.EnviarRegistrosIgnorandoEstados();
            else
                ZKTekoAPIManager.EnviarRegistros();
            Console.ReadLine();
        }
    }
}
