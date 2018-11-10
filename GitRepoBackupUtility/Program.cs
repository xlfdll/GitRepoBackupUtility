using System;
using System.ComponentModel;
using System.Diagnostics;

namespace GitRepoBackupUtility
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Git Repository Backup Tool");
            Console.WriteLine("(C) 2018 Xlfdll Workstation");
            Console.WriteLine();

            if (args.Length == 0)
            {
                Commands.ShowUsage();
            }
            else
            {
                switch (args[0])
                {
                    case "/?":
                        {
                            Commands.ShowUsage();

                            break;
                        }
                    case "/list":
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

                                break;
                            }

                            String fileName = args.Length > 1 && !String.IsNullOrEmpty(args[1])
                                ? args[1] : "repos.list";

                            Commands.HandleRepoList(fileName, "Backups");

                            break;
                        }
                    default:
                        {
                            Console.WriteLine("ERROR: Unknown arguments.");
                            Console.WriteLine();

                            Commands.ShowUsage();

                            break;
                        }
                }
            }
        }
    }
}