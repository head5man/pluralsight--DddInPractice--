using DddInPractice.Logic;

namespace DddInPractice.UI.Common
{
    public class MainViewModel : ViewModel
    {
        public MainViewModel()
        {
            SnackMachine sm;
            sm = GetSnackMachine();

            var viewModel = new SnackMachineViewModel(sm);
            _dialogService.ShowDialog(viewModel);
        }

        private static SnackMachine GetSnackMachine()
        {
            SnackMachine sm;
            using (var session = SessionFactory.OpenSession())
            {
                sm = session.Get<SnackMachine>(1L);
            }
            return sm;
        }
    }
}
