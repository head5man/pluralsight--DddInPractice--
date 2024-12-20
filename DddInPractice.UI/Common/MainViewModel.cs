using DddInPractice.UI.Management;

namespace DddInPractice.UI.Common
{
    public class MainViewModel : ViewModel
    {
        public MainViewModel()
        {
            Dashboard = new DashboardViewModel();
        }

        public DashboardViewModel Dashboard { get; }
    }
}
