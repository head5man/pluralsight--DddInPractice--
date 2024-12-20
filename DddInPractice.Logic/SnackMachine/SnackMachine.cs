using NHibernate.Mapping;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using static DddInPractice.Logic.Money;

namespace DddInPractice.Logic.SnackMachine
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

        public virtual void BuySnack(int position)
        {
            if (CanBuySnack(position, out string reason) is false)
            {
                throw new InvalidOperationException(reason);
            }

            var change = CalculateChange(position);
            Slot slot = GetSlot(position);
            slot.SnackPile = slot.SnackPile.SubtractOne();
            MoneyInTransaction -= slot.SnackPile.Price;
            ReturnChange(change);
        }

        public virtual bool CanBuySnack(int position, out string reason)
        {
            reason = string.Empty;
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

            var amount = MoneyInTransaction - slot.SnackPile.Price;
            if (MoneyInside.CanAllocate(amount) is false)
            {
                reason = "Not enough change";
                return false;
            }

            return true;
        }

        public virtual Money CalculateChange(int position)
        {
            Money money = Money.None;
            var slot = GetSlot(position);
            var amount = MoneyInTransaction - slot.SnackPile.Price;
            if (MoneyInside.CanAllocate(amount))
            {
                money = MoneyInside.Allocate(amount);
            }
            return money;
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

        public virtual IReadOnlyList<SnackPile> GetAllSnackPiles()
        {
            return Slots.Select(s => s.SnackPile).ToList();
        }

        private Slot GetSlot(int position) 
        {
            return Slots.Single(s => s.Position == position);
        }

        private void ReturnChange(Money changeDue)
        {
            MoneyInTransaction -= changeDue.Amount;
            MoneyInside -= changeDue;
        }

        public virtual Money UnloadMoney()
        {
            var unloaded = MoneyInside;
            MoneyInside = Money.None;
            return unloaded;
        }
    }
}
