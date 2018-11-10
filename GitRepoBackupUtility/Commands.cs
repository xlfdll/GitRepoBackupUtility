using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GitRepoBackupUtility
{
    public static class Commands
    {
        public static void ShowUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine();
            Console.WriteLine("GitRepoBackupUtility /list [list file]");
            Console.WriteLine("- Backup repositories specified in list file.");
            Console.WriteLine("- If list file is not specified, 'repos.list' in current working directory will be used");
            Console.WriteLine();
        }

        public static void HandleRepoList(String fileName, String backupFolder)
        {
            String prefix = String.Empty;
            String postfix = String.Empty;
            List<String> repos = new List<String>();

            backupFolder = Path.GetFullPath(backupFolder);

            if (Directory.Exists(backupFolder))
            {
                Helper.RemoveBackupFolder(backupFolder);
            }

            Directory.CreateDirectory(backupFolder);

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
                Helper.CloneGitRepo(prefix.Replace("#", repo) + repo + postfix.Replace("#", repo), backupFolder);

                Console.WriteLine();
            }

            foreach (String folder in Directory.GetDirectories(backupFolder))
            {
                Helper.CompressFolder(folder, backupFolder);

                Console.WriteLine();
            }
        }
    }
}