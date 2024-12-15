using System;

namespace DddInPractice.Logic
{
    public sealed class SnackPile : ValueObject<SnackPile>
    {
        public static readonly SnackPile None = new SnackPile(null, 0, 0m);
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

        protected override bool EqualsCore(SnackPile other)
        {
            return Snack == other.Snack &&
                Quantity == other.Quantity &&
                Price == other.Quantity;
        }

        protected override int GetHashCodeCore()
        {
            unchecked
            {
                int hashCode = Snack.GetHashCode();
                hashCode = (hashCode * 397) ^ Quantity;
                hashCode = (hashCode * 397) ^ Price.GetHashCode();
                return hashCode;
            }
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