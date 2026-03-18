using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace GitRepoBackupUtility
{
    public static class Commands
    {
        public static Boolean CheckGitInstallation()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("git")
            {
                UseShellExecute = false,
                RedirectStandardOutput = true
            };

            try
            {
                using (Process p = new Process() { StartInfo = startInfo })
                {
                    p.Start();
                    p.WaitForExit();
                }
            }
            catch (Win32Exception)
            {
                Console.WriteLine("ERROR: Git is not installed or not accessible!");

                return false;
            }

            return true;
        }

        public static void ShowUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine();
            Console.WriteLine("GitRepoBackupUtility /list [list file] [/mirror]");
            Console.WriteLine("- /list - Backup repositories specified in list file.");
            Console.WriteLine("- If list file is not specified, 'repos.list' in current working directory will be used");
            Console.WriteLine();
            Console.WriteLine("- /mirror - Backup a complete mirror of the original repositories.");
            Console.WriteLine();
        }

        public static void HandleRepoList(String fileName, String backupFolder, Boolean isMirror)
        {
            String prefix = String.Empty;
            String postfix = String.Empty;
            List<String> repos = new List<String>();

            String currentAssemblyLocation = Assembly.GetEntryAssembly().Location;
            backupFolder = Path.Combine(Path.GetDirectoryName(currentAssemblyLocation), backupFolder);

            if (Directory.Exists(backupFolder))
            {
                Helper.RemoveBackupFolder(backupFolder);
            }

            Directory.CreateDirectory(backupFolder);

            Console.WriteLine($"{backupFolder} created.");
            Console.WriteLine();

            using (StreamReader reader = new StreamReader(fileName, Encoding.UTF8))
            {
                while (!reader.EndOfStream)
                {
                    String line = reader.ReadLine();

                    if (!String.IsNullOrEmpty(line) && !line.StartsWith("//"))
                    {
                        switch (line[0])
                        {
                            case '<':
                                {
                                    prefix = line.Remove(0, 1).Trim();

                                    break;
                                }
                            case '>':
                                {
                                    postfix = line.Remove(0, 1).Trim();

                                    break;
                                }
                            case '#':
                                {
                                    repos.Add(line.Remove(0, 1).Trim());

                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                    }
                }
            }

            foreach (String repo in repos)
            {
                String repoAddress = prefix.Replace("#", repo) + repo + postfix.Replace("#", repo);

                Console.WriteLine($"Cloning {repoAddress}...");

                Helper.CloneGitRepo(repoAddress, backupFolder, isMirror);

                Console.WriteLine();
            }

            foreach (String folder in Directory.GetDirectories(backupFolder))
            {
                if (!isMirror)
                {
                    Console.WriteLine($"Fetching {Path.GetFileName(folder)}...");

                    Helper.FetchGitRemote(folder);

                    Console.WriteLine();
                }

                Console.WriteLine($"Compressing {Path.GetFileName(folder)}...");

                Helper.CompressFolder(folder, backupFolder);

                Console.WriteLine();
            }
        }
    }
}