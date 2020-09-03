using System.IO;

namespace EmployeeRecords.Loggers
{
    class Logger
    {
        private static readonly Logger _instance;
        private readonly string _filePath = @"..\Log.txt";

        //a private constructor provides that constructor can be called only inside this class
        private Logger()
        {
            if (!File.Exists(_filePath))
                File.Create(_filePath).Close();
        }
        //a static constructor is called only once, so we will have only one instance of this class
        static Logger()
        {
            _instance = new Logger();
        }
        public static Logger Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Log actions to a file 
        /// </summary>
        /// <param name="message"></param>
        public void Log(string message)
        {
            using (StreamWriter sw = File.AppendText(_filePath))
            {
                sw.WriteLine(message);
            }
        }
    }
}
