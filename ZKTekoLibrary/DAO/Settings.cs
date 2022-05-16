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
        public static string APIURL { get => instance.APIUrl; }
        public static string ReSendStatusText { get => instance.ReSendStatus; }
        public static string APISuccessStatus { get => instance.APISuccessMessage; }
        public static string APIDuplicateStatus { get => instance.APIDuplicateMessage; }
        public static string DBIgnoreStatus { get => instance.DBIgnoreMessages; }
        public static bool PrintLogs { get => instance.SendLog; }

        public string DBServerName;
        public string DBServerIP;
        public string DBUsername;
        public string DBPassword;
        public string APIUrl;
        public int Days;
        public string ReSendStatus;
        public string APISuccessMessage;
        public string APIDuplicateMessage;
        public bool SendLog;
        public string DBIgnoreMessages;

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
