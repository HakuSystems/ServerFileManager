using System.IO;
using System;
using System.Windows;

namespace ServerFileManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            progBar.ValueChanged += ProgBar_ValueChanged;
            statusTxt.Text = "Please Wait";
            var currentDir = Directory.GetCurrentDirectory();
            Console.WriteLine("-");
            Console.WriteLine(currentDir);
            Console.WriteLine("-");
            
            var audiPath = Path.GetFullPath(currentDir + "\\Audios");

            var imgPath = Path.GetFullPath(currentDir + "\\Images");

            var vidPath = Path.GetFullPath(currentDir + "\\Videos");

            var comprPath = Path.GetFullPath(currentDir + "\\Compressed");

            string[] allFiles = Directory.GetFiles(Directory.GetCurrentDirectory());
            progBar.Value = 0;
            foreach (var f in allFiles)
            {
                if (f.EndsWith(".png") || f.EndsWith(".jpg") || f.EndsWith(".jpeg") || f.EndsWith(".png") || f.EndsWith(".bmp") || f.EndsWith(".gif"))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    MoveFileToFolder(f, imgPath);
                }
                
                if (f.EndsWith(".mp4") || f.EndsWith(".mov") || f.EndsWith(".avi") || f.EndsWith(".mkv") || f.EndsWith(".webm"))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    MoveFileToFolder(f, vidPath);
                }

                if (f.EndsWith(".mp3") || f.EndsWith(".wav") || f.EndsWith(".ogg"))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    MoveFileToFolder(f, audiPath);
                }
                
                if (f.EndsWith(".rar") || f.EndsWith(".zip"))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    MoveFileToFolder(f, comprPath);
                }
            }
            progBar.Value = 100;
            statusTxt.Text = "Done";
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("---");
            Console.WriteLine("Done");
            Console.WriteLine("---");
        }

        private void ProgBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Console.Title = "Loading: " + e.NewValue + "%";
        }

        private void MoveFileToFolder(string file, string outputPath)
        {
            try
            {
                if (!Directory.Exists(outputPath))
                {
                    Console.WriteLine("Creating: " + outputPath);
                    Directory.CreateDirectory(outputPath);
                    Console.WriteLine("Created: " + outputPath);
                }
                var fileInfo = new FileInfo(file);
                if (File.Exists(outputPath +"\\"+ fileInfo.Name))
                {
                    Console.WriteLine(fileInfo.Name + " already exists in " + outputPath);
                    File.Delete(outputPath + "\\" + fileInfo.Name);
                    Console.WriteLine("Deleted: " + outputPath + "\\" + fileInfo.Name);
                }
                Console.WriteLine("Moving: " + file + "\nTo:" + outputPath);
                File.Move(file, outputPath + "\\" + fileInfo.Name);
                Console.WriteLine("Moved: " + file + "\nTo:" + outputPath + "\\" + fileInfo.Name);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR");
                Console.WriteLine("---");
                Console.WriteLine(ex.Message);
                Console.WriteLine("---");
                Console.WriteLine("ERROR");
            }
        }
    }
}
