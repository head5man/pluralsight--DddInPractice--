using DddInPractice.Logic;
using DddInPractice.UI.Common;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace DddInPractice.UI
{
    public class SnackMachineViewModel : ViewModel
    {
        private readonly SnackMachine _snackMachine;
        private readonly SnackMachineRepository _repository;
        private string _message = "";

        public SnackMachineViewModel(SnackMachine snackMachine, SnackMachineRepository repository)
        {
            _snackMachine = snackMachine;
            _repository = repository;
            InsertCentCommand = new Command(() => InsertMoney(Money.Cent));
            InsertDimeCommand = new Command(() => InsertMoney(Money.TenCent));
            InsertQuarterCommand = new Command(() => InsertMoney(Money.Quarter));
            InsertDollarCommand = new Command(() => InsertMoney(Money.Dollar));
            InsertFiveDollarCommand = new Command(() => InsertMoney(Money.FiveDollar));
            InsertTwentyDollarCommand = new Command(() => InsertMoney(Money.TwentyDollar));
            ReturnMoneyCommand = new Command(() => ReturnMoney());
            BuySnackCommand = new Command(() => BuySnack(1));
        }

        public override string Caption => "Snack Machine";

        public decimal MoneyInTransaction => _snackMachine.MoneyInTransaction;

        public string Message { get => _message; set => SetProperty(ref _message, value); }

        public Money MoneyInside => _snackMachine.MoneyInside;

        public Command InsertCentCommand { get; }

        public Command InsertDimeCommand { get; }

        public Command InsertQuarterCommand { get; }

        public Command InsertDollarCommand { get; }

        public Command InsertFiveDollarCommand { get; }

        public Command InsertTwentyDollarCommand { get; }

        public Command ReturnMoneyCommand { get; }

        public Command BuySnackCommand { get; }

        private void InsertMoney(Money c)
        {
            _snackMachine.InsertMoney(c);
            NotifyClient($"You have inserted: {c}");
            SaveSnackMachine();
        }

        private void BuySnack(int position)
        {
            if (_snackMachine.CanBuySnack(position, out var change, out var reason) is false)
            {
                NotifyClient(reason);
                return;
            }

            _snackMachine.BuySnack(position);
            NotifyClient($"Bought a snack{(change != Money.None ? $"{Environment.NewLine}Returned {change}" : string.Empty)}");
            SaveSnackMachine();
        }

        private void ReturnMoney()
        {
            if (MoneyInTransaction <= 0)
            {
                NotifyClient("No money inserted");
                return;
            }

            var returned = _snackMachine.ReturnMoney();
            NotifyClient($"Money returned: {returned}");
            SaveSnackMachine();
        }

        private void NotifyClient(string message)
        {
            Notify(nameof(MoneyInTransaction));
            Notify(nameof(MoneyInside));
            Message = message;
        }

        private void SaveSnackMachine()
        {
            _repository.Save(_snackMachine);
        }
    }
}
