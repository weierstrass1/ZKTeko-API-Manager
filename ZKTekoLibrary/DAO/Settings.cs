using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace ZKTekoLibrary.DAO
{
    public class Settings
    {
        private static Settings _instance;

        public static string DatabaseServerName { get => instance.DBServerName; }
        public static string DatabaseServerIP { get => instance.DBServerIP; }
        public static string DatabaseUsername { get => instance.DBUsername; }
        public static string DatabasePassword { get => instance.DBPassword; }
        public static int QueryDays { get => instance.Days; }
        public static List<string> QueryDateRange { get => instance.DateRange; }
        public static string APIURLReSend { get => instance.APIUrlReSend; }
        public static string APIURLHour { get => instance.APIUrlHour; }
        public static string ReSendStatusText { get => instance.ReSendStatus; }
        public static string APISuccessStatus { get => instance.APISuccessMessage; }
        public static string DBIgnoreStatus { get => instance.DBIgnoreMessages; }
        public static string SerialsNumberList { get => instance.SerialsNumber; }
        public static bool PrintLogs { get => instance.SendLog; }
        public static bool UseSerialNumberList { get => instance.OnlySerialsInList; }
        public static bool ReSend { get; set; } = false;
        public static bool OnlyNewRegisters { get => instance.OnlyNewRegs; }
        public static bool IgnoreMessages { get => instance.Ignore; }
        public static long MaxDiffTimeAllowed { get => instance.MaxDiffTime*60000; }
        public static bool OnlyNotNull { get => instance.OnlyNotNewReg; }

        public string DBServerName;
        public string DBServerIP;
        public string DBUsername;
        public string DBPassword;
        public string APIUrlReSend;
        public string APIUrlHour;
        public int Days;
        public string ReSendStatus;
        public string APISuccessMessage;
        public bool SendLog;
        public string DBIgnoreMessages;
        public List<string> DateRange;
        public string SerialsNumber;
        public long MaxDiffTime;
        public bool Ignore;
        public bool OnlySerialsInList;
        public bool OnlyNewRegs;
        public bool OnlyNotNewReg;

        private static Settings instance
        {
            get
            {
                if (_instance == null)
                    _instance = JsonConvert.DeserializeObject<Settings>(File.ReadAllText("settings.json"));

                return _instance;
            }
        }
        private Settings()
        {
        }
    }
}
