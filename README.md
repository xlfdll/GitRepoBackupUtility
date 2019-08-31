# Git Repository Backup Utility
A simple Git repository backup utility.

## System Requirements
* .NET Framework 4.8
* Git for Windows
* Git Credential Manager for Windows (to support OAuth-based authentication, such as GitHub and Azure DevOps)

[Runtime configuration](https://docs.microsoft.com/en-us/dotnet/framework/migration-guide/how-to-configure-an-app-to-support-net-framework-4-or-4-5) is needed for running on other versions of .NET Framework.

## Usage
```
GitRepoBackupUtility /? | /list [list file]
```
* **\<no arguments\>** or **/?** - Show usage
* **/list** - Backup repositories specified in list file
  * **\[list file\]** - List file path (optional)
  * If list file is not specified, **repos.list** in current working directory will be used

An example list file can be found in **Examples** directory.

## Development Prerequisites
* Visual Studio 2015+

Before the build, generate-build-number.sh needs to be executed in a Git / Bash shell to generate build information code file (BuildInfo.cs).
