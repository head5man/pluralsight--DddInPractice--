using FluentNHibernate.Mapping;
using System;

namespace DddInPractice.Logic.ATM
{
    public class AtmMap : ClassMap<Atm>
    {
        public AtmMap()
        {
            Id(x => x.Id);

            Map(x => x.MoneyCharged);

            Component(x => x.MoneyInside, y =>
            {
                y.Map(z => z.OneCentCount);
                y.Map(z => z.TenCentCount);
                y.Map(z => z.QuarterCount);
                y.Map(z => z.OneDollarCount);
                y.Map(z => z.FiveDollarCount);
                y.Map(z => z.TwentyDollarCount);
            });
        }
    }

    public class Atm : AggregateRoot
    {
        private const decimal CommissionRate = 0.01m;
        private const decimal CommissionMin = 0.01m;

        public virtual Money MoneyInside { get; protected set; } = Money.None;
        public virtual decimal MoneyCharged { get; protected set; }

        public virtual void LoadMoney(Money money)
        {
            MoneyInside += money;
        }

        public virtual bool CanWithdrawMoney(decimal amount, out string error)
        {
            error = string.Empty;
            if (amount <= 0m)
            {
                error = "Invalid amount";
                return false;
            }
            if (MoneyInside.Amount < amount)
            {
                error = "Not enough money";
                return false;
            }

            if (MoneyInside.CanAllocate(amount) is false)
            {
                error = "Cannot allocate amount";
                return false;
            }

            return true;
        }

        public virtual void WithdrawMoney(decimal amount)
        {
            if (CanWithdrawMoney(amount, out string error) is false)
            {
                throw new InvalidOperationException(error);
            }

            var withdrawal = MoneyInside.Allocate(amount);
            MoneyInside -= withdrawal;

            decimal amountWithCommission = amount + CalculateCommission(amount);
            MoneyCharged += amountWithCommission;
        }

        private decimal CalculateCommission(decimal amount)
        {
            var roundup = Math.Ceiling(amount * 100m * CommissionRate) / 100m;
            var commission = Math.Round(roundup, 2);
            return Math.Max(commission, CommissionMin);
        }
    }
}
