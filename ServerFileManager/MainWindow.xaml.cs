using System.IO;
using System.Windows;
using System.Windows.Shapes;

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
            statusTxt.Text = "Please Wait";
            //get current application path
            var currentDir = Directory.GetCurrentDirectory();
        }
    }
}
