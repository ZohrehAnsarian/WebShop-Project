using System;
using System.Globalization;
using System.IO;
using System.Threading;
 

namespace BusinessLayer
{
    public static class BusinessMessage
    {
        private static string _Message = "";
        public static string Message
        {
            get { return _Message; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) == false)
                {
                    _Message += "\n" + value;
                    LogError(_Message);
                }
            }
        }

        public static bool HasError { get; set; }

        public static void Clear()
        {
            HasError = false;
            _Message = "";
        }
        public static void LogError(string message)
        {
            if (Message.Contains("The server was not found or was not accessible"))
            {
                using (var log = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "ErrorLog.txt", true))
                {
                    log.WriteLine(DateTime.Now + ": " + message);
                    log.WriteLine("___________________________________________________________________________________________________");
                    log.Close();
                }

                throw new Exception(Message);
            }

        }

    }
}
