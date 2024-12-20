using Xunit;
using DddInPractice.Logic.ATM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using DddInPractice.Logic.Common;

namespace DddInPractice.Logic.ATM.Tests
{
    public class AtmSpecs
    {
        [Fact]
        public void Withdraws_money_with_commission()
        {
            var atm = new Atm();
            atm.LoadMoney(Money.Dollar);

            atm.Withdraw(1m);

            atm.MoneyInside.Amount.Should().Be(0m);
            atm.MoneyCharged.Should().Be(1.01m);
        }

        [Fact]
        public void Withdrawing_more_than_inside_throws_exception()
        {
            var atm = new Atm();
            atm.LoadMoney(Money.Dollar);

            Action action = () => atm.Withdraw(2m);

            action.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void Commission_is_at_least_one_cent()
        {
            var atm = new Atm();
            atm.LoadMoney(Money.Cent);

            atm.Withdraw(Money.Cent.Amount);

            atm.MoneyCharged.ShouldBeEquivalentTo(0.02m);
        }

        [Fact]
        public void Commission_is_rounded_up_to_next_cent()
        {
            var atm = new Atm();
            atm.LoadMoney(Money.Dollar);
            atm.LoadMoney(Money.TenCent);

            atm.Withdraw(1.1m);

            atm.MoneyInside.Amount.Should().Be(0m);
            atm.MoneyCharged.Should().Be(1.12m);
        }

        [Fact]
        public void Withdrawal_raises_an_event()
        {
            var atm = new Atm();
            atm.LoadMoney(Money.Dollar);

            atm.Withdraw(1m);

            atm.ShouldContainBalanceChangedEvent(1.01m);
        }
    }

    internal static class AtmExtensions
    {
        public static void ShouldContainBalanceChangedEvent(this Atm self, decimal delta)
        {
            var domainEvent = self.DomainEvents
                .OfType<BalanceChangedEvent>()
                .SingleOrDefault();

            domainEvent.Should().NotBeNull();
            domainEvent.Delta.Should().Be(delta);
        }
    }
}