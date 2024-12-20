using FluentNHibernate.Conventions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace DddInPractice.Logic
{
    public sealed class Money : ValueObject<Money>
    {
        public static readonly Money None = new Money(0, 0, 0, 0, 0, 0);
        public static readonly Money Cent = new Money(1, 0, 0, 0, 0, 0);
        public static readonly Money TenCent = new Money(0, 1, 0, 0, 0, 0);
        public static readonly Money Quarter = new Money(0, 0, 1, 0, 0, 0);
        public static readonly Money Dollar = new Money(0, 0, 0, 1, 0, 0);
        public static readonly Money FiveDollar = new Money(0, 0, 0, 0, 1, 0);
        public static readonly Money TwentyDollar = new Money(0, 0, 0, 0, 0, 1);

        public int OneCentCount { get; }
        public int TenCentCount { get; }
        public int QuarterCount { get; }
        public int OneDollarCount { get; }
        public int FiveDollarCount { get; }
        public int TwentyDollarCount { get; }

        public int AmountInCents =>
            OneDollarCount * 100 +
            FiveDollarCount * 500 +
            TwentyDollarCount * 2000 +
            OneCentCount + TenCentCount * 10 + QuarterCount * 25;

        public decimal Amount =>
            OneCentCount * 0.01m +
            TenCentCount * 0.10m +
            QuarterCount * 0.25m +
            OneDollarCount +
            FiveDollarCount * 5 +
            TwentyDollarCount * 20;

        private Money() { }

        public Money(
            int oneCentCount = 0,
            int tenCentCount = 0,
            int quarterCount = 0,
            int oneDollarCount = 0,
            int fiveDollarCount = 0,
            int twentyDollarCount = 0)
            : this()
        {
            if (oneCentCount < 0)
                throw new InvalidOperationException();
            if (tenCentCount < 0)
                throw new InvalidOperationException();
            if (quarterCount < 0)
                throw new InvalidOperationException();
            if (oneDollarCount < 0)
                throw new InvalidOperationException();
            if (fiveDollarCount < 0)
                throw new InvalidOperationException();
            if (twentyDollarCount < 0)
                throw new InvalidOperationException();

            OneCentCount = oneCentCount;
            TenCentCount = tenCentCount;
            QuarterCount = quarterCount;
            OneDollarCount = oneDollarCount;
            FiveDollarCount = fiveDollarCount;
            TwentyDollarCount = twentyDollarCount;
        }

        public static Money operator +(Money money1, Money money2)
        {
            Money sum = new Money(
                money1.OneCentCount + money2.OneCentCount,
                money1.TenCentCount + money2.TenCentCount,
                money1.QuarterCount + money2.QuarterCount,
                money1.OneDollarCount + money2.OneDollarCount,
                money1.FiveDollarCount + money2.FiveDollarCount,
                money1.TwentyDollarCount + money2.TwentyDollarCount);

            return sum;
        }

        public static Money operator *(Money money1, int multiplier)
        {
            Money sum = new Money(
                money1.OneCentCount * multiplier,
                money1.TenCentCount * multiplier,
                money1.QuarterCount * multiplier,
                money1.OneDollarCount * multiplier,
                money1.FiveDollarCount * multiplier,
                money1.TwentyDollarCount * multiplier);

            return sum;
        }

        public static Money operator -(Money money1, Money money2)
        {
            return new Money(
                money1.OneCentCount - money2.OneCentCount,
                money1.TenCentCount - money2.TenCentCount,
                money1.QuarterCount - money2.QuarterCount,
                money1.OneDollarCount - money2.OneDollarCount,
                money1.FiveDollarCount - money2.FiveDollarCount,
                money1.TwentyDollarCount - money2.TwentyDollarCount);
        }

        public override string ToString()
        {
            return Amount == 0 ? null : Amount.ToString("C2", CultureInfo.GetCultureInfo("en-US"));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return OneCentCount;
            yield return TenCentCount;
            yield return QuarterCount;
            yield return OneDollarCount;
            yield return FiveDollarCount;
            yield return TwentyDollarCount;
        }

        public bool CanAllocate(decimal amount)
        {
            Money money = AllocateCore(amount);
            return money.Amount == amount;
        }

        public Money Allocate(decimal amount)
        {
            if (!CanAllocate(amount))
                throw new InvalidOperationException();

            return AllocateCore(amount);
        }

        private Money AllocateCore(decimal allocate, Money money = null, int iteration = 0)
        {
            var amount = allocate;
            List<Money> iterations = null;
            if (money == null) 
            {
                money = Money.None;
                iterations = new List<Money>();
            }
            
            while (iteration <= 5 && allocate > 0)
            {
                switch (iteration)
                {
                    case 0:
                        if (TrySubAllocate(ref money, amount, iteration, TwentyDollar, TwentyDollarCount))
                        {
                            amount -= money.Amount;
                            break;
                        }
                        iteration++;
                        goto case 1;
                    case 1:
                        if (TrySubAllocate(ref money, amount, iteration, FiveDollar, FiveDollarCount))
                        {
                            amount -= money.Amount;
                            break;
                        }
                        iteration++;
                        goto case 2;
                    case 2:
                        if (TrySubAllocate(ref money, amount, iteration, Dollar, OneDollarCount))
                        {
                            amount -= money.Amount;
                            break;
                        }
                        iteration++;
                        goto case 3;
                    case 3:
                        if (TrySubAllocate(ref money, amount, iteration, Quarter, QuarterCount))
                        {
                            amount -= money.Amount;
                            break;
                        }
                        iteration++;
                        goto case 4;
                    case 4:
                        if (TrySubAllocate(ref money, amount, iteration, TenCent, TenCentCount))
                        {
                            amount -= money.Amount;
                            break;
                        }
                        iteration++;
                        goto case 5;
                    case 5:
                        if (TrySubAllocate(ref money, amount, iteration, Cent, OneCentCount))
                        {
                            amount -= money.Amount;
                        }
                        break;
                    default:
                        throw new Exception();
                }

                if (iterations is null)
                {
                    if (amount == 0)
                    {
                        allocate = 0;
                    }
                    return money;
                }
                else
                {
                    iterations?.Add(money);
                    if (amount == 0)
                    {
                        break;
                    }
                    amount = allocate;
                    iteration++;
                    money = Money.None;
                }
            }

            return iterations is null || iterations.IsEmpty() ?
                money :
                iterations.OrderByDescending(m => m.Amount).First();
        }

        private bool TrySubAllocate(ref Money money, decimal amountDue, int iteration, Money moneyKind, int moneyKindMax)
        {
            var subMoney = SubIteration(amountDue, iteration, moneyKind, moneyKindMax);
            if (subMoney != None)
            {
                money = subMoney;
                return true;
            }
            return false;
        }

        private Money SubIteration(decimal amount, int iteration, Money moneyKind, int max)
        {
            var count = Math.Min((int)(amount / moneyKind.Amount), max);
            for (int i = count; i > 0; i--)
            {
                Money money = moneyKind * i;
                var remaining = amount - money.Amount;
                money += AllocateCore(remaining, None, iteration + 1);
                if (money.Amount == amount)
                {
                    return money;
                }
            }
            return Money.None;
        }
    }
}
