using DddInPractice.Logic;
using DddInPractice.Logic.ATM;
using DddInPractice.UI.ATM;
using System.Windows;

namespace DddInPractice.UI.Common
{
    public class MainViewModel : ViewModel
    {
        public MainViewModel()
        {
            //RunSnackMachine();
            RunATM();
        }

        private static void RunATM()
        {
            var atmRepo = new AtmRepository();
            var atm = atmRepo.GetById(1);
            var atmViewModel = new AtmViewModel(atm, atmRepo);
            _dialogService.ShowDialog(atmViewModel);
        }

        private static void RunSnackMachine()
        {
            var repo = new SnackMachineRepository();
            SnackMachine sm = repo.GetById(1);
            var viewModel = new SnackMachineViewModel(sm, repo);
            _dialogService.ShowDialog(viewModel);
        }
    }
}
