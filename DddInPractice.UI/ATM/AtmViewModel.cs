using DddInPractice.Logic;
using DddInPractice.Logic.ATM;
using DddInPractice.UI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DddInPractice.UI.ATM
{
    public class AtmViewModel : ViewModel
    {
        private Atm _atm;

        public AtmViewModel(Atm atm)
        {
            _atm = atm;
        }

        public override string Caption => "ATM";

        public Money MoneyInside => _atm.MoneyInside;

        public decimal MoneyCharged => _atm.MoneyCharged;
    }
}
