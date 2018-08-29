using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace TestRail_Searcher
{
    static class Program
    {
        public static void LogException(Exception e)
        {
            var workingDirectory = Path.GetDirectoryName(Application.ExecutablePath) ?? "";
            var fileName = "TestRail-Searcher_" + DateTime.Now.ToString("yyyy-MM-dd") + "_err.log";
            if (File.Exists(Path.Combine(workingDirectory, fileName)))
            {
                using (FileStream fs = new FileStream(Path.Combine(workingDirectory, fileName), FileMode.Append))
                {
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss\t") + e.Message);
                    sw.Close();
                }
            }
            else
            {
                using (FileStream fs = new FileStream(Path.Combine(workingDirectory, fileName), FileMode.OpenOrCreate))
                {
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss\t") + e.Message);
                    sw.Close();
                }
            }
        }

        public static string VersionLabel
        {
            get
            {
                if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
                {
                    Version ver = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion;
                    return string.Format("{4} v{0}.{1}.{2}.{3}", ver.Major, ver.Minor, ver.Build, ver.Revision, Assembly.GetEntryAssembly().GetName().Name);
                }
                else
                {
                    var ver = Assembly.GetExecutingAssembly().GetName().Version;
                    return string.Format("{4} v{0}.{1}.{2}.{3}", ver.Major, ver.Minor, ver.Build, ver.Revision, Assembly.GetEntryAssembly().GetName().Name);
                }
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TestRailSearcherForm());
        }
    }
}
