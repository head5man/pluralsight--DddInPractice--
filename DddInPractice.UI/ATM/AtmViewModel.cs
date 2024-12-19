using DddInPractice.Logic;
using DddInPractice.Logic.ATM;
using DddInPractice.UI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DddInPractice.UI.ATM
{
    public class AtmViewModel : ViewModel
    {
        private Atm _atm;
        private readonly AtmRepository _repo;
        private string _message;

        public AtmViewModel(Atm atm, AtmRepository repo)
        {
            _atm = atm;
            _repo = repo;
            WithdrawCommand = new Command<decimal>(Withdraw, x => x > 0);
        }

        public override string Caption => "ATM";

        public string Message { get => _message; set => SetProperty(ref _message, value); }

        public Money MoneyInside => _atm.MoneyInside;

        public decimal MoneyCharged => _atm.MoneyCharged;

        public Command<decimal> WithdrawCommand { get; }

        private void Withdraw(decimal amount)
        {
            if (_atm.CanWithdraw(amount, out string error) is false)
            {
                Message = error;
                return;
            }

            _atm.Withdraw(amount);
            _repo.Save(_atm);
            NotifyClient($"Withdrew amount {amount:c2}");
        }

        private void NotifyClient(string message)
        {
            Message = message;
            Notify(nameof(MoneyInside));
            Notify(nameof(MoneyCharged));
        }
    }
}
