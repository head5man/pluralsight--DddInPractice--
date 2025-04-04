using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using DddInPractice.Logic.SnackMachine;

namespace DddInPractice.UI.SnackMachine
{
    public class SnackPileViewModel
    {
        private readonly SnackPile _snackPile;

        public string Price => _snackPile.Price.ToString("C2", CultureInfo.GetCultureInfo("en-US"));
        public int Amount => _snackPile.Quantity;
        public int ImageWidth => GetImageWidth(_snackPile.Snack);
        public ImageSource Image => 
            (ImageSource)Application.Current.FindResource("img" + _snackPile.Snack.Name);

        public SnackPileViewModel(SnackPile snackPile)
        {
            _snackPile = snackPile;
        }

        private int GetImageWidth(Snack snack)
        {
            if (snack == Snack.Chocolate)
                return 120;

            if (snack == Snack.Soda)
                return 70;

            if (snack == Snack.Gum)
                return 70;

            throw new ArgumentException();
        }
    }
}
