using System;
using System.Diagnostics;
using System.IO;

namespace GitRepoBackupUtility
{
    public static class Helper
    {
        public static void RemoveBackupFolder(String backupFolder)
        {
            if (Directory.Exists(backupFolder))
            {
                foreach (String file in Directory.GetFiles(backupFolder, "*.*", SearchOption.AllDirectories))
                {
                    File.SetAttributes(file, FileAttributes.Archive);
                }

                Directory.Delete(backupFolder, true);
            }
        }

        public static void CloneGitRepo(String address, String backupFolder)
        {
            Environment.CurrentDirectory = backupFolder;

            ProcessStartInfo startInfo = new ProcessStartInfo("git", $"clone {address}")
            {
                UseShellExecute = false,
                RedirectStandardOutput = true
            };

            using (Process p = new Process() { StartInfo = startInfo })
            {
                p.Start();
                p.WaitForExit();
            }
        }

        public static void CompressFolder(String folder, String backupFolder)
        {
            Environment.CurrentDirectory = backupFolder;

            String compressionUtilityPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"WinRAR\rar.exe");
            String compressionArguments = $"a -df -s -rr {Path.GetFileName(folder)}.rar {Path.GetFileName(folder)}";

            ProcessStartInfo startInfo = new ProcessStartInfo(compressionUtilityPath, compressionArguments)
            {
                UseShellExecute = false,
                RedirectStandardOutput = false
            };

            using (Process p = new Process() { StartInfo = startInfo })
            {
                p.Start();
                p.WaitForExit();
            }
        }
    }
}