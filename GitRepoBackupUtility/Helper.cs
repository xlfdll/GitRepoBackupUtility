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
            ProcessStartInfo startInfo = new ProcessStartInfo("git", $"clone {address}")
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                WorkingDirectory = backupFolder
            };

            using (Process p = new Process() { StartInfo = startInfo })
            {
                p.Start();
                p.WaitForExit();
            }
        }

        public static void FetchGitRemote(String folder)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("git", "fetch --all --tags")
            {
                UseShellExecute = false,
                RedirectStandardOutput = false,
                WorkingDirectory = folder
            };

            using (Process p = new Process() { StartInfo = startInfo })
            {
                p.Start();
                p.WaitForExit();
            }
        }

        public static void CompressFolder(String folder, String backupFolder)
        {
            String compressionUtilityPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"WinRAR\rar.exe");
            String compressionArguments = $"a -df -s -rr {Path.GetFileName(folder)}.rar {Path.GetFileName(folder)}";

            ProcessStartInfo startInfo = new ProcessStartInfo(compressionUtilityPath, compressionArguments)
            {
                UseShellExecute = false,
                RedirectStandardOutput = false,
                WorkingDirectory = backupFolder
            };

            using (Process p = new Process() { StartInfo = startInfo })
            {
                p.Start();
                p.WaitForExit();
            }
        }
    }
}