using System.Diagnostics;

namespace ServerFileManagerConsole;

static internal class Program
{
    private static void Main(string[] args)
    {
        Console.Title = "Server File Manager";
        CreateAllFolders();
        MoveFiles();
        CloseAppIn(5);
    }

    private static void CloseAppIn(int seconds)
    {
        SLogger.Log($"Closing app in {seconds} seconds.");
        Thread.Sleep(seconds * 1000);
    }

    private static void MoveFiles()
    {
        var folders = Enum.GetValues(typeof(FolderNames)).Cast<FolderNames>().ToList();
        var currentDirectory = Directory.GetCurrentDirectory();

        var fileEndingsList = folders.Select(folder => new FileEndings(folder)).ToList();

        var allFiles = Directory.GetFiles(currentDirectory)
            .Where(f => !IsCurrentExecutable(f))
            .ToList();

        var totalFiles = allFiles.Count;
        var processedFiles = 0;

        while (allFiles.Count > 0)
            foreach (var file in allFiles.ToList())
            {
                if (folders.Any(folder => file.StartsWith(Path.Combine(currentDirectory, folder.ToString()))))
                {
                    allFiles.Remove(file);
                    continue;
                }

                processedFiles++;
                var percentage = processedFiles * 100 / totalFiles;
                AnimateTitle($"Moving files... {percentage}%");

                var fileMoved = false;

                foreach (var fileEndings in fileEndingsList)
                    if (fileEndings.endings.Any(ending =>
                            file.EndsWith(ending, StringComparison.OrdinalIgnoreCase) ||
                            file.EndsWith("." + ending, StringComparison.OrdinalIgnoreCase)))
                    {
                        MoveFileToFolder(file, fileEndings.folder.ToString());
                        fileMoved = true;
                        allFiles.Remove(file);
                        break;
                    }

                if (!fileMoved)
                {
                    MoveFileToFolder(file, FolderNames.General.ToString());
                    allFiles.Remove(file);
                }
            }

        SLogger.Log("All files have been moved to their respective folders.");
    }

    private static bool IsCurrentExecutable(string filePath)
    {
        var currentExecutablePath = Process.GetCurrentProcess().MainModule.FileName;
        return string.Equals(
            Path.GetFullPath(filePath),
            Path.GetFullPath(currentExecutablePath),
            StringComparison.OrdinalIgnoreCase);
    }

    private static void AnimateTitle(string title)
    {
        Console.Title = title;
    }

    private static void MoveFileToFolder(string sourceFilePath, string destinationFolder)
    {
        try
        {
            if (IsCurrentExecutable(sourceFilePath))
            {
                SLogger.Log("This executable file will not be moved.");
                return;
            }

            var fileName = Path.GetFileName(sourceFilePath);
            var destinationPath = Path.Combine(destinationFolder, fileName);

            if (!Directory.Exists(destinationFolder))
                Directory.CreateDirectory(destinationFolder);

            if (File.Exists(destinationPath))
                File.Delete(destinationPath);

            File.Move(sourceFilePath, destinationPath);
        }
        catch (Exception ex) when (!(ex is FileNotFoundException))
        {
            SLogger.LogError(ex.Message);
        }
    }

    private static void CreateAllFolders()
    {
        foreach (FolderNames folder in Enum.GetValues(typeof(FolderNames)))
            CreateFolder(folder);
    }

    private static void CreateFolder(FolderNames folder)
    {
        try
        {
            if (!Directory.Exists(folder.ToString()))
            {
                Directory.CreateDirectory(folder.ToString());
                SLogger.Log($"Folder {folder} has been created.");
            }
        }
        catch (Exception ex) when (!(ex is FileNotFoundException))
        {
            SLogger.LogError(ex.Message);
        }
    }
}