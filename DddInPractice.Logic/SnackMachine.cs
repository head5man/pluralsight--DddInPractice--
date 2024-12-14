using NHibernate.Mapping;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using static DddInPractice.Logic.Money;

namespace DddInPractice.Logic
{
    public class SnackMachine : Entity
    {
        public SnackMachine()
        {
            MoneyInside = None;
            MoneyInTransaction = None;
            Slots = new List<Slot>
            {
                new Slot(this, 1, null, 0, 0m),
                new Slot(this, 2, null, 0, 0m),
                new Slot(this, 3, null, 0, 0m)
            };
        }

        public virtual Money MoneyInside { get; protected set; }
        public virtual Money MoneyInTransaction { get; protected set; }
        public virtual IList<Slot> Slots { get; protected set; }

        public virtual void InsertMoney(Money money)
        {
            Money[] coinsAndNotes =
            {
                Cent, TenCent, Quarter, Dollar, FiveDollar, TwentyDollar
            };
            if (!coinsAndNotes.Contains(money))
                throw new InvalidOperationException();

            MoneyInTransaction += money;
        }

        public virtual Money ReturnMoney()
        {
            Money money = MoneyInTransaction;
            MoneyInTransaction = None;
            return money;
        }

        public virtual void BuySnack(int position)
        {
            Slot slot = Slots.Single(p => p.Position == position);
            if (slot.Quantity > 0 && slot.Price <= MoneyInTransaction.Amount)
            {
                slot.Quantity--;
                MoneyInside += MoneyInTransaction;
                MoneyInTransaction = None;
            }
        }

        public virtual void LoadSnacks(int position, Snack snack, int quantity, decimal price)
        {
            Slot slot = Slots.Single(s => s.Position == position);
            slot.Snack = snack;
            slot.Quantity = quantity;
            slot.Price = price;
        }
    }
}
