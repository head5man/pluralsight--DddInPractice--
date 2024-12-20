using System;
using System.Collections.Generic;

namespace DddInPractice.Logic.SnackMachine
{
    public sealed class SnackPile : ValueObject<SnackPile>
    {
        public static readonly SnackPile None = new SnackPile(Snack.None, 0, 0m);
        public Snack Snack { get; }
        public int Quantity { get; }
        public decimal Price { get; }

        private SnackPile()
        {
        }

        public SnackPile(
            Snack snack, int quantity, decimal price)
            : this()
        {
            Snack = snack;
            Quantity = quantity;
            Price = price;
            Validate();
        }

        public SnackPile SubtractOne()
        {
            return new SnackPile(Snack, Quantity - 1, Price);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Snack;
            yield return Quantity;
            yield return Price;
        }

        private void Validate()
        {
            if (Quantity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Quantity));
            }
            if (Price < 0 || Price % 0.01m > 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Price));
            }
        }
    }
}