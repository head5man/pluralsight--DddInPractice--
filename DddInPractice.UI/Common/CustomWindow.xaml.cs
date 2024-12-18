using System.Windows;

namespace DddInPractice.UI.Common
{
    public partial class CustomWindow
    {
        public CustomWindow(ViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
