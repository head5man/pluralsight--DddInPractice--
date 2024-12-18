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
            MoneyInTransaction = 0;
            Slots = new List<Slot>
            {
                new Slot(this, 1),
                new Slot(this, 2),
                new Slot(this, 3)
            };
        }

        public virtual Money MoneyInside { get; protected set; }
        public virtual decimal MoneyInTransaction { get; protected set; }
        protected virtual IList<Slot> Slots { get; set; }

        public virtual void InsertMoney(Money money)
        {
            Money[] coinsAndNotes =
            {
                Cent, TenCent, Quarter, Dollar, FiveDollar, TwentyDollar
            };
            if (!coinsAndNotes.Contains(money))
                throw new InvalidOperationException();

            MoneyInside += money;
            MoneyInTransaction += money.Amount;
        }

        public virtual Money ReturnMoney()
        {
            Money money = MoneyInside.Allocate(MoneyInTransaction);
            MoneyInTransaction -= money.Amount;
            MoneyInside -= money;
            return money;
        }

        public virtual Money BuySnack(int position)
        {
            if (CanBuySnack(position, out Money change, out string reason) is false)
            {
                throw new InvalidOperationException(reason);
            }
            
            Slot slot = GetSlot(position);
            slot.SnackPile = slot.SnackPile.SubtractOne();
            MoneyInTransaction = 0;
            MoneyInside -= change;
            return change;
        }

        public virtual bool CanBuySnack(int position, out Money change, out string reason)
        {
            reason = string.Empty;
            change = Money.None;
            var slot = GetSlot(position);
            if (slot is null)
            {
                reason = "Invalid slot position";
                return false;
            }
            if (slot.SnackPile.Quantity == 0)
            {
                reason = "Slot empty";
                return false;
            }
            if (slot.SnackPile.Price > MoneyInTransaction)
            {
                reason = "Not enough money";
                return false;
            }
            change = MoneyInside.Allocate(MoneyInTransaction - slot.SnackPile.Price);
            if (change.Amount != MoneyInTransaction - slot.SnackPile.Price)
            {
                reason = "Not enough change";
                return false;
            }

            return true;
        }

        public virtual void LoadSnacks(int position, SnackPile snackPile)
        {
            Slot slot = GetSlot(position);
            slot.SnackPile = snackPile;
        }

        public virtual void LoadMoney(Money money)
        {
            MoneyInside += money;
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
