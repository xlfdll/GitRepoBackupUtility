using System;

namespace GitRepoBackupUtility
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Git Repository Backup Utility");
            Console.WriteLine("(C) 2018-2026 Xlfdll Workstation");
            Console.WriteLine();

            if (args.Length == 0)
            {
                Commands.ShowUsage();

                return;
            }
            else
            {
                Boolean containsList = false;
                String listFileName = null;
                Boolean isMirror = false;

                for (Int32 i = 0; i < args.Length; i++)
                {
                    String arg = args[i];

                    switch (arg)
                    {
                        case "/?":
                            {
                                Commands.ShowUsage();

                                return;
                            }
                        case "/list":
                            {
                                containsList = true;

                                if (i + 1 < args.Length && !args[i + 1].StartsWith("/"))
                                {
                                    listFileName = args[i + 1];

                                    i++; // Skip filename
                                }

                                break;
                            }
                        case "/mirror":
                            {
                                isMirror = true;

                                break;
                            }
                        default:
                            {
                                Console.WriteLine("ERROR: Unknown arguments.");
                                Console.WriteLine();

                                Commands.ShowUsage();

                                return;
                            }
                    }
                }

                if (containsList)
                {
                    if (Commands.CheckGitInstallation())
                    {
                        String fileName = listFileName ?? "repos.list";

                        Commands.HandleRepoList(fileName, "Backups", isMirror);
                    }
                }
                else
                {
                    Console.WriteLine("ERROR: No valid arguments.");
                    Console.WriteLine();

                    Commands.ShowUsage();

                    return;
                }
            }
        }
    }
}