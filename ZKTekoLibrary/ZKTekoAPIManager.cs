using System;
using System.Collections.Generic;
using System.Text;
using ZKTekoLibrary.DAO;
using HTTPRequestUtils;
using SQLUtils;
using System.IO;
using System.Threading;

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
        public static void GetPlatformHourProcess()
        {
            StringBuilder sb = new StringBuilder();
            DateTime platformHour = GetPlatformHour();

            sb.AppendLine($"La hora de la plataforma es: {platformHour:dd/MM/yyyy HH:mm:ss}");
            sb.AppendLine($"La hora del servidor es: {DateTime.UtcNow:dd/MM/yyyy HH:mm:ss}");
            Console.Write(sb);
            if (Settings.PrintLogs)
            {
                if (!Directory.Exists("logs"))
                    Directory.CreateDirectory("logs");
                File.WriteAllText($"logs/log_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.txt", sb.ToString());
            }
            Console.WriteLine("Proceso Finalizado - Presione Enter para continuar.");
        }
        public static DateTime GetPlatformHour()
        {
            string uri = $"{Settings.APIURLHour}";

            string result = HTTPRequest.Get(uri);

            long millis = long.Parse(result);

            DateTime d = new DateTime(1970, 1, 1, 0, 0, 0);

            d = d.AddMilliseconds(millis);

            return d;
        }
        public static void EnviarRegistrosIgnorandoEstados()
        {
            List<Registro> regs = Registro.GetAllWithIgnore();

            EnviarRegistros(regs);
        }
        public static void EnviarRegistros()
        {
            List<Registro> regs = Registro.GetAll();

             EnviarRegistros(regs);
        }
        public static void EnviarRegistros(List<Registro> regs)
        {
            StringBuilder sb = new StringBuilder();

            int i = 1;
            int errores = 0;
            int exitos = 0;
            RespuestaEnvio res;

            foreach(Registro reg in regs)
            {
                Console.Clear();
                Console.WriteLine($"Enviando {i}/{regs.Count}");
                res = EnviarRegistro(reg);

                switch(res)
                {
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
                            $"Errores: {errores}\n" +
                            $"Total: {regs.Count}";
            Console.WriteLine(log);
            Console.WriteLine("Proceso Finalizado - Presione Enter para continuar.");

            if (Settings.PrintLogs)
            {
                sb.Append(log);
                if (!Directory.Exists("logs"))
                    Directory.CreateDirectory("logs");


                File.WriteAllText($"logs/log_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.txt", sb.ToString());
            }
        }
        public static RespuestaEnvio EnviarRegistro(Registro reg)
        {
            string uri = $"{Settings.APIURLReSend}serial={reg.NumeroSerial}&rut={reg.IDPersona}&i={reg.Tipo}&fecha={reg.Fecha}&hora={reg.Hora}";

            string result = HTTPRequest.Get(uri);

            Thread.Sleep(1000);

            string prevEstado = reg.Estado;
            reg.Estado = result;

            if (!(prevEstado.Equals(Settings.APISuccessStatus) && result.Equals(prevEstado)))
            {
                if (prevEstado != null && prevEstado != "" && result.Trim().Equals(Settings.APISuccessStatus))
                    Registro.UpdateStatus(reg, Settings.ReSendStatusText);
                else
                    Registro.UpdateStatus(reg, result);
            }

            if (result.Trim().Equals(Settings.APISuccessStatus))
            {
                return RespuestaEnvio.Exito;
            }
            return RespuestaEnvio.Error;
        }
    }
}
