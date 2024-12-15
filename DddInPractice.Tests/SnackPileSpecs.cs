using Xunit;
using DddInPractice.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace DddInPractice.Logic.Tests
{
    public class SnackPileSpecs
    {
        [Fact]
        public void Negative_quantity_throws_exception()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new SnackPile(null, -1, 1));
        }

        [Fact]
        public void Negative_price_throws_exception()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new SnackPile(null, 1, -1));
        }

        [Theory]
        [InlineData(0.001)]
        [InlineData(0.111)]
        [InlineData(20.0001)]
        public void Price_with_cent_fractions_throws_exception(decimal price)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new SnackPile(null, 1, price));
        }

        [Fact]
        public void Subtracts_quantity_by_one()
        {
            SnackPile pile = new SnackPile(null, 1, 1m);
            pile.SubtractOne().Quantity.Should().Be(0);
        }

        [Fact]
        public void Subracting_empty_throws_exception()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>SnackPile.None.SubtractOne());
        }
    }
}