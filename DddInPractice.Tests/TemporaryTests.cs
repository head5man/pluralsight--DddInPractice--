using DddInPractice.Logic;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Navigation;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DddInPractice.Tests
{
    public class TemporaryTests
    {
        [Fact(Skip = "Temporary testing")]
        public void Test()
        {
            SessionFactory.Init(@"Server=(localdb)\MSSQLLocalDB;Database=DddInPractice;Trusted_Connection=true");
            //SessionFactory.Init(@"Server=.;Database=DddInPractice;Trusted_Connection=true");

            var repository = new SnackMachineRepository();
            var snackMachine = repository.GetById(1);
            snackMachine.Id.Should().Be(1);
            snackMachine.InsertManyMoney(Money.Dollar * 3);
            var initial = snackMachine.GetSnackPile(1).Quantity;
            snackMachine.BuySnack(1);
            repository.Save(snackMachine);
            snackMachine = repository.GetById(1);
            snackMachine.GetSnackPile(1).Quantity.Should().Be(initial - 1);
        }
    }
}
