using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
public static class command
{
    //var output = command.run(ffprobe,
    //      String.Format("-v quiet -print_format json -show_format -show_streams \"{0}\"", filePath));


    public static string run(string filePath, string args, string workingDir = "")
    {
        Process proc = new Process();
        proc.StartInfo.WorkingDirectory = workingDir;
        proc.StartInfo.FileName = filePath;
        proc.StartInfo.Arguments = args;
        proc.StartInfo.RedirectStandardError = true;
        proc.StartInfo.RedirectStandardOutput = true;
        proc.StartInfo.UseShellExecute = false;
        if (!proc.Start())
        {
            Console.WriteLine("Error starting");
            return string.Empty;
        }

        StreamReader reader = proc.StandardOutput;
        string line;
        StringBuilder sb = new StringBuilder();
        while ((line = reader.ReadLine()) != null)
        {
            sb.AppendLine(line);
        }
        proc.Close();
        return sb.ToString();
    }

    /*var workingDir = @"G:\temp\sql2json\";
            var op = command.runNodeAppJs(workingDir);*/
    public static string runNodeAppJs(string workingDir)
    {
        return command.run("node.exe", "app.js", workingDir);
    }

    public static string runNodeFile(string workingDir, string jsFileName)
    {
        return command.run("node.exe", jsFileName, workingDir);
    }


}