using System;
using System.Collections.Generic;
using System.Text;
using ZKTekoLibrary.DAO;
using HTTPRequestUtils;
using SQLUtils;
using System.IO;

namespace ZKTekoLibrary
{
    public enum RespuestaEnvio { Error, Exito, Duplicado};
    public class ZKTekoAPIManager
    {
        private static readonly ZKTekoAPIManager instance = new ZKTekoAPIManager();
        internal readonly SQLManager _sqlmanager = new SQLManager(Settings.DatabaseServerIP, 
                                                            Settings.DatabaseServerName,
                                                            Settings.DatabaseUsername, 
                                                            Settings.DatabasePassword);
        internal static SQLManager sqlmanager { get => instance._sqlmanager; }
        private ZKTekoAPIManager()
        {

        }

        public static void EnviarRegistros()
        {
            List<Registro> regs = Registro.GetAll();

            int i = 0;
            int errores = 0;
            int duplicado = 0;
            int exitos = 0;
            RespuestaEnvio res;

            StringBuilder sb = new StringBuilder();

            foreach(Registro reg in regs)
            {
                Console.Clear();
                Console.WriteLine($"Enviando {i}/{regs.Count-1}");
                res = EnviarRegistro(reg);

                switch(res)
                {
                    case RespuestaEnvio.Duplicado:
                        sb.AppendLine($"{reg} - {Settings.APIDuplicateStatus}");
                        duplicado++;
                        break;
                    case RespuestaEnvio.Exito:
                        sb.AppendLine($"{reg} - {Settings.APISuccessStatus}");
                        exitos++;
                        break;
                    default:
                        sb.AppendLine($"{reg} - Error");
                        errores++;
                        break;
                }
                i++;
            }
            
            string log = "Se ha completado el proceso\n" +
                            $"Exitos: {exitos}\n" +
                            $"Duplicados: {duplicado}\n" +
                            $"Errores: {errores}\n" +
                            $"Total: {regs.Count}";
            Console.WriteLine(log);
            Console.WriteLine("Proceso Finalizado - Presione Enter para continuar.");

            if (Settings.PrintLogs)
            {
                sb.AppendLine(log);
                if (!Directory.Exists("logs"))
                    Directory.CreateDirectory("logs");


                File.WriteAllText($"logs/log_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.txt", sb.ToString());
            }
        }
        public static RespuestaEnvio EnviarRegistro(Registro reg)
        {
            string uri = $"{Settings.APIURL}serial={reg.NumeroSerial}&rut={reg.IDPersona}&i={reg.Tipo}&fecha={reg.Fecha}&hora={reg.Hora}";

            string result = HTTPRequest.Get(uri);

            if (result.Trim().Equals(Settings.APISuccessStatus) || result.Trim().Equals(Settings.APIDuplicateStatus))
                Registro.UpdateStatus(reg, Settings.ReSendStatusText);

            if (result.Trim().Equals(Settings.APISuccessStatus))
            {
                return RespuestaEnvio.Exito;
            }
            else if (result.Trim().Equals(Settings.APIDuplicateStatus))
            {
                return RespuestaEnvio.Duplicado;
            }
            return RespuestaEnvio.Error;
        }
    }
}
