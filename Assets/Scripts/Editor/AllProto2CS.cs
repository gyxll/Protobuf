using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEditor;
using UnityEngine;

public class AllProto2CS
{
    [MenuItem("Tools/Proto2CS", priority = 2)]
    public static void Proto2CS()
    {
        ParseCS("Scripts/ProtoMessage/", "Proto/");
    }

    public static void ParseCS(string targetPath, string sourceDicPath)
    {
        string rootDir = Environment.CurrentDirectory;

        string protoc;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            protoc = Path.Combine(rootDir, "Proto/protoc.exe");
        }
        else
        {
            protoc = Path.Combine(rootDir, "Proto/protoc");
        }
        sourceDicPath = Path.Combine(rootDir, sourceDicPath);

        string hotfixMessageCodePath = Path.Combine(rootDir, "Assets", targetPath);
        DirectoryInfo protoDic = new DirectoryInfo(sourceDicPath);
        FileInfo[] protoFiles = protoDic.GetFiles("*.proto");
        StringBuilder protoFileNameStr = new StringBuilder();
        foreach (var protoFile in protoFiles)
        {
            protoFileNameStr.Append(" ");
            protoFileNameStr.Append(protoFile.Name);
        }
        string argument2 = $"--csharp_out=\"{hotfixMessageCodePath}\" --proto_path=\"{sourceDicPath}\"{protoFileNameStr}";
        //UnityEngine.Debug.Log(argument2);
        Run(protoc, argument2, waitExit: true);

        UnityEngine.Debug.Log("proto2cs succeed!");

        AssetDatabase.Refresh();
    }

    public static Process Run(string exe, string arguments, string workingDirectory = ".", bool waitExit = false)
    {
        try
        {
            bool redirectStandardOutput = true;
            bool redirectStandardError = true;
            bool useShellExecute = false;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                redirectStandardOutput = false;
                redirectStandardError = false;
                useShellExecute = true;
            }

            if (waitExit)
            {
                redirectStandardOutput = true;
                redirectStandardError = true;
                useShellExecute = false;
            }

            ProcessStartInfo info = new ProcessStartInfo
            {
                FileName = exe,
                Arguments = arguments,
                CreateNoWindow = true,
                UseShellExecute = useShellExecute,
                WorkingDirectory = workingDirectory,
                RedirectStandardOutput = redirectStandardOutput,
                RedirectStandardError = redirectStandardError,
            };

            Process process = Process.Start(info);

            if (waitExit)
            {
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    throw new Exception($"{process.StandardOutput.ReadToEnd()} {process.StandardError.ReadToEnd()}");
                }
            }

            return process;
        }
        catch (Exception e)
        {
            throw new Exception($"dir: {Path.GetFullPath(workingDirectory)}, command: {exe} {arguments}", e);
        }
    }
}
