using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DddInPractice.Logic;

using FluentAssertions;

using Xunit;

using static DddInPractice.Logic.Money;

namespace DddInPractice.Tests
{
    public class ReturnsHighestDenominationData : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            // [Loaded, Inserted, Return]
            new object[] {Dollar, new Money(quarterCount: 4), Dollar},
            new object[] {Dollar, new Money(quarterCount: 5), new Money(oneDollarCount: 1, quarterCount: 1)},
            new object[] {FiveDollar, new Money(oneDollarCount: 3, quarterCount: 8), FiveDollar}
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class ReturnsChangeData : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            // [Loaded, Inserted, Price, Return]
            new object[] {
                Dollar,
                new Money(quarterCount: 4),
                Dollar,
                None},
            new object[] {
                Dollar,
                Dollar,
                new Money(quarterCount: 2),
                None},
            new object[] {
                new Money(fiveDollarCount: 1, oneDollarCount: 4, quarterCount: 3, tenCentCount: 3),
                new Money(fiveDollarCount: 1, oneDollarCount: 1),
                new Money(quarterCount: 2, tenCentCount: 4, oneCentCount: 5),
                new Money(oneDollarCount: 4, quarterCount: 3, tenCentCount: 3)
            },
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    internal static class SnackMachineExtensions
    {
        public static void InsertManyMoney(this SnackMachine machine, Money inserted)
        {
            machine.InsertSingles(inserted.TwentyDollarCount, TwentyDollar);
            machine.InsertSingles(inserted.FiveDollarCount, FiveDollar);
            machine.InsertSingles(inserted.OneDollarCount, Dollar);
            machine.InsertSingles(inserted.QuarterCount, Quarter);
            machine.InsertSingles(inserted.TenCentCount, TenCent);
            machine.InsertSingles(inserted.OneCentCount, Cent);
        }

        public static Money InsertSingles(this SnackMachine machine, int count, Money moneyKind)
        {
            Money inserted = None;
            while (count > 0)
            {
                machine.InsertMoney(moneyKind);
                inserted += moneyKind;
                count--;
            }
            return inserted;
        }
    }

    public class SnackMachineSpecs
    {
        [Fact]
        public void Return_money_empties_money_in_transaction()
        {
            var snackMachine = new SnackMachine();
            snackMachine.InsertMoney(Dollar);

            snackMachine.ReturnMoney();

            snackMachine.MoneyInTransaction.Should().Be(0m);
        }

        [Fact]
        public void Inserted_money_goes_to_money_in_transaction()
        {
            var snackMachine = new SnackMachine();

            snackMachine.InsertMoney(Cent);
            snackMachine.InsertMoney(Dollar);

            snackMachine.MoneyInTransaction.Should().Be(1.01m);
        }

        [Fact]
        public void Cannot_insert_more_than_one_coin_or_note_at_a_time()
        {
            var snackMachine = new SnackMachine();
            var twoCent = Cent + Cent;

            Action action = () => snackMachine.InsertMoney(twoCent);

            action.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void BuySnack_trades_inserted_money_for_a_snack()
        {
            var snackMachine = new SnackMachine();
            snackMachine.LoadSnacks(1, new SnackPile(Snack.Chocolate, 10, 1m));
            snackMachine.InsertMoney(Dollar);
            
            snackMachine.BuySnack(1);

            snackMachine.MoneyInTransaction.Should().Be(0);
            snackMachine.MoneyInside.Amount.Should().Be(1m);
            snackMachine.GetSnackPile(1).Quantity.Should().Be(9);
        }

        [Fact]
        public void Cannot_make_purchase_when_there_is_no_snacks()
        {
            var snackMachine = new SnackMachine();
            
            Action action = () => snackMachine.BuySnack(1);

            action.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void Cannot_make_purchase_when_there_is_not_enough_money()
        {
            var snackMachine = new SnackMachine();
            snackMachine.LoadSnacks(1, new SnackPile(Snack.Soda, 10, 2m));
            snackMachine.InsertMoney(Dollar);

            Action action = () => snackMachine.BuySnack(1);

            action.ShouldThrow<InvalidOperationException>();
        }

        [Theory]
        [ClassData(typeof(ReturnsHighestDenominationData))]
        public void Returns_money_with_highest_denomination_first(
            Money loaded,
            Money inserted,
            Money expected
            )
        {
            var snackMachine = new SnackMachine();
            snackMachine.LoadMoney(loaded);
            snackMachine.InsertManyMoney(inserted);
            
            var returned = snackMachine.ReturnMoney();

            returned.Should().Be(expected);
        }

        
        [Theory]
        [ClassData(typeof(ReturnsChangeData))]
        public void After_purchase_change_is_returned(
            Money loaded,
            Money inserted,
            Money price,
            Money expected)
        {
            var snackMachine = new SnackMachine();
            snackMachine.LoadSnacks(1, new SnackPile(Snack.Gum, 10, price.Amount));
            snackMachine.LoadMoney(loaded);
            snackMachine.InsertManyMoney(inserted);

            var change = snackMachine.BuySnack(1);

            change.Should().Be(expected);
            snackMachine.MoneyInTransaction.Should().Be(inserted.Amount - price.Amount - change.Amount);
        }
    }
}
