using DddInPractice.Logic;

namespace DddInPractice.UI.Common
{
    public class MainViewModel : ViewModel
    {
        public MainViewModel()
        {
            var repo = new SnackMachineRepository();
            SnackMachine sm = repo.GetById(1);
            
            var viewModel = new SnackMachineViewModel(sm, repo);
            _dialogService.ShowDialog(viewModel);
        }
    }
}
