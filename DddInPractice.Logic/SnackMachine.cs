using NHibernate.Mapping;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using static DddInPractice.Logic.Money;

namespace DddInPractice.Logic
{
    public class SnackMachine : AggregateRoot
    {
        public SnackMachine()
        {
            MoneyInside = None;
            MoneyInTransaction = None;
            Slots = new List<Slot>
            {
                new Slot(this, 1),
                new Slot(this, 2),
                new Slot(this, 3)
            };
        }

        public virtual Money MoneyInside { get; protected set; }
        public virtual Money MoneyInTransaction { get; protected set; }
        protected virtual IList<Slot> Slots { get; set; }

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
            Slot slot = GetSlot(position);
            if (slot.SnackPile.Quantity > 0 && slot.SnackPile.Price <= MoneyInTransaction.Amount)
            {
                slot.SnackPile = slot.SnackPile.SubtractOne();
                MoneyInside += MoneyInTransaction;
                MoneyInTransaction = None;
            }
        }

        public virtual void LoadSnacks(int position, SnackPile snackPile)
        {
            Slot slot = GetSlot(position);
            slot.SnackPile = snackPile;
        }

        public virtual SnackPile GetSnackPile(int position)
        {
            Slot slot = GetSlot(position);
            return slot.SnackPile;
        }

        private Slot GetSlot(int position) 
        {
            return Slots.Single(s => s.Position == position);
        }
    }
}
