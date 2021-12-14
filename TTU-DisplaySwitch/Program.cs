using System;
using System.IO;
using TTU_DisplaySwitch.Class;
using TTU_DisplaySwitch.Cmd;

// handling command line arguments
cmdParser cmdP = new(args);

Options cmdFlags = cmdP.Parse();  

if (cmdFlags.versionFlag)
{
    Console.WriteLine(cmdFlags.versionText);
    return;
}

if (cmdFlags.helpFlag)
{
    Console.WriteLine(cmdFlags.helpText);
    return;
}

// running main function
HelperClass test = new HelperClass();

try
{
    test.Run();
}
catch(Exception e)
{
    log(e.Message);
    Environment.ExitCode = -1;
}


void log(string message)
{
    if (!(Directory.Exists(@"C:\ProgramData\TTU")))
    {
        Directory.CreateDirectory(@"C:\ProgramData\TTU");
    }

    if (!(File.Exists(@"C:\ProgramData\TTU\TTU-DisplaySwitch.txt")))
    {
        using (StreamWriter sw = File.CreateText(@"C:\ProgramData\TTU\TTU-DisplaySwitch.txt"))
        {
            sw.WriteLine($"{ DateTime.Now.ToString() } - { message }");
        }
    }
    else
    {
        using (StreamWriter sw = File.AppendText(@"C:\ProgramData\TTU\TTU-DisplaySwitch.txt"))
        {
            sw.WriteLine($"{ DateTime.Now.ToString() } - { message }");
        }
    }
}