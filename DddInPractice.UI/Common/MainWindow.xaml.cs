namespace DddInPractice.UI.Common
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();
            // Somewhat hacky approach where MainViewModel is showing a blocking dialog.
            // Preventing MainWindow showing after closing by calling Close once loaded.
            Loaded += (o, e) => Close();
        }
    }
}
