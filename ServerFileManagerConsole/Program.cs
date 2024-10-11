using Serilog.Events;

namespace ServerFileManagerConsole;

static class Program
{
    static void Main(string[] args)
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
        var folders = (FolderNames[])Enum.GetValues(typeof(FolderNames));
        var fileEndings = new FileEndings[folders.Length];
        for (int i = 0; i < folders.Length; i++)
        {
            fileEndings[i] = new FileEndings(folders[i]);
        }

        var allFiles = Directory.GetFiles(Directory.GetCurrentDirectory());
        foreach (var f in allFiles)
        {
            var percentage = (Array.IndexOf(allFiles, f) + 1) * 100 / allFiles.Length;
            AnimateTitle($"Moving files... {percentage}%");
            foreach (var fileEnding in fileEndings)
            {
                if (fileEnding.endings.Contains(Path.GetExtension(f)))
                {
                    var percentage2 = (Array.IndexOf(fileEndings, fileEnding) + 1) * 100 / fileEndings.Length;
                    AnimateTitle($"Moving files... {percentage}% | {percentage2}%");
                    MoveFileToFolder(f, fileEnding.folder.ToString());
                }
            }
        }
        SLogger.Log("All files have been moved to their respective folders.");
    }

    private static void AnimateTitle(string s)
    {
        Console.Title = s;
        Console.Title = "Server File Manager";
    }

    private static void MoveFileToFolder(string s, string toString)
    {
        try
        {
            var fileInfo = new FileInfo(s);
            if (File.Exists($"{toString}\\{fileInfo.Name}"))
                File.Delete($"{toString}\\{fileInfo.Name}");

            File.Move(s, $"{toString}\\{fileInfo.Name}");
        }
        catch (Exception ex)
        {
            if (ex is FileNotFoundException)
                return;
            SLogger.LogError(ex.Message);
        }
    }

    private static void CreateAllFolders()
    {
        FolderNames[] folders = (FolderNames[])Enum.GetValues(typeof(FolderNames));
        foreach (var folder in folders)
        {
            CreateFolder(folder);
        }
    }

    private static void CreateFolder(FolderNames folder)
    {
        try
        {
            if (Directory.Exists(folder.ToString())) return;
            Directory.CreateDirectory(folder.ToString());
            SLogger.Log($"Folder {folder} has been created.");
        }
        catch (Exception ex)
        {
            if (ex is FileNotFoundException)
                return;
            SLogger.LogError(ex.Message);
        }
    }
}