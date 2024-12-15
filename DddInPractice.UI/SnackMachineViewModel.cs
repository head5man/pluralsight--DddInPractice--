using DddInPractice.Logic;
using DddInPractice.UI.Common;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace DddInPractice.UI
{
    public class SnackMachineViewModel : ViewModel
    {
        private readonly SnackMachine _snackMachine;

        private string _message = "";

        public SnackMachineViewModel(SnackMachine snackMachine)
        {
            _snackMachine = snackMachine;
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

        public Money MoneyInTransaction => _snackMachine.MoneyInTransaction;

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
        }

        private void BuySnack(int position)
        {
            if (MoneyInTransaction == Money.None)
            {
                NotifyClient("No money inserted");
                return;
            }
            if (_snackMachine.CanBuySnack(position, MoneyInTransaction, out var reason) is false)
            {
                NotifyClient(reason);
                return;
            }

            _snackMachine.BuySnack(position);
            Notify(nameof(MoneyInside));
            NotifyClient($"Bought a snack");
            SaveSnackMachine();
        }

        private void ReturnMoney()
        {
            if (MoneyInTransaction == Money.None)
            {
                NotifyClient("No money inserted");
                return;
            }

            var returned = _snackMachine.ReturnMoney();
            NotifyClient($"Money returned: {returned}");
        }

        private void NotifyClient(string message)
        {
            Notify(nameof(MoneyInTransaction));
            Message = message;
        }

        private void SaveSnackMachine()
        {
            using (var session = SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.SaveOrUpdate(_snackMachine);
                transaction.Commit();
            }
        }
    }
}
