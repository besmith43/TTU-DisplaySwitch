using System;
using System.Text;

#nullable enable

namespace TTU_DisplaySwitch.Cmd
{
    public class Options
    {
        public bool helpFlag { get; set; }
        public string helpText { get; set; }
        public bool versionFlag { get; set; }
        public string versionText { get; set; }
        public bool verbose { get; set; }

        public Options()
        {
            helpFlag = false;
            versionFlag = false;
            versionText = "TTU-DisplaySwitch 1.0.0";
            verbose = false;
            StringBuilder helpTextBuilder = new StringBuilder();
            helpTextBuilder.AppendLine($"{ versionText }");
            helpTextBuilder.AppendLine("Usage:  TTU-DisplaySwitch [OPTION]  ");
            helpTextBuilder.AppendLine("This application checks if the monitor configuration is duplicated, and if not sets it to be duplicated");
            helpTextBuilder.AppendLine("Creates a log file at C:\\ProgramData\\TTU\\TTU-DisplaySwitch.txt");
            helpTextBuilder.AppendLine("");
            helpTextBuilder.AppendLine("    -v | --version          Display version message");
            helpTextBuilder.AppendLine("    -h | --help             Display this help message");
            helpTextBuilder.AppendLine("");
            helpText = helpTextBuilder.ToString();
        }
    }
}