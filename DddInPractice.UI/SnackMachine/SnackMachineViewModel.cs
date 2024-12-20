using DddInPractice.UI.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DddInPractice.Logic.SnackMachine;
using Money = DddInPractice.Logic.Money;

namespace DddInPractice.UI.SnackMachine
{
    public class SnackMachineViewModel : ViewModel
    {
        private readonly Logic.SnackMachine.SnackMachine _snackMachine;
        private readonly SnackMachineRepository _repository;
        private string _message = "";

        public SnackMachineViewModel(Logic.SnackMachine.SnackMachine snackMachine)
        {
            _snackMachine = snackMachine;
            _repository = new SnackMachineRepository();
            InsertCentCommand = new Command(() => InsertMoney(Money.Cent));
            InsertDimeCommand = new Command(() => InsertMoney(Money.TenCent));
            InsertQuarterCommand = new Command(() => InsertMoney(Money.Quarter));
            InsertDollarCommand = new Command(() => InsertMoney(Money.Dollar));
            InsertFiveDollarCommand = new Command(() => InsertMoney(Money.FiveDollar));
            InsertTwentyDollarCommand = new Command(() => InsertMoney(Money.TwentyDollar));
            ReturnMoneyCommand = new Command(() => ReturnMoney());
            BuySnackCommand = new Command<object>((pos) => BuySnack(pos));
        }

        public override string Caption => "Snack Machine";

        public string MoneyInTransaction => (_snackMachine.MoneyInTransaction is 0.00m) ? null : _snackMachine.MoneyInTransaction.ToString("C2", CultureInfo.GetCultureInfo("en-US"));

        public string Message { get => _message; set => SetProperty(ref _message, value); }

        public Money MoneyInside => _snackMachine.MoneyInside;

        public IReadOnlyList<SnackPileViewModel> Piles => _snackMachine.GetAllSnackPiles().Select(p => new SnackPileViewModel(p)).ToList();

        public Command InsertCentCommand { get; }

        public Command InsertDimeCommand { get; }

        public Command InsertQuarterCommand { get; }

        public Command InsertDollarCommand { get; }

        public Command InsertFiveDollarCommand { get; }

        public Command InsertTwentyDollarCommand { get; }

        public Command ReturnMoneyCommand { get; }

        public Command<object> BuySnackCommand { get; }

        private void InsertMoney(Money c)
        {
            _snackMachine.InsertMoney(c);
            NotifyClient($"You have inserted: {c}");
            SaveSnackMachine();
        }

        private void BuySnack(object obj)
        {
            if (obj is string str && int.TryParse(str, out int position))
            {
                if (_snackMachine.CanBuySnack(position, out var reason) is false)
                {
                    NotifyClient(reason);
                    return;
                }

                var change = _snackMachine.CalculateChange(position);
                _snackMachine.BuySnack(position);
                var changeMsg = change != Money.None ? Environment.NewLine + "Returned change " + change : string.Empty;
                var msg = "Bought a snack" + changeMsg;
                NotifyClient(msg);
                Notify(nameof(Piles));
                SaveSnackMachine();
            }
        }

        private void ReturnMoney()
        {
            if (_snackMachine.MoneyInTransaction <= 0)
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
