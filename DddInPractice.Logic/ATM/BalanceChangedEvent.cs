﻿using DddInPractice.Logic.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DddInPractice.Logic.ATM
{
    public class BalanceChangedEvent : IDomainEvent
    {
        public decimal Delta { get; private set; }

        public BalanceChangedEvent(decimal delta)
        {
            Delta = delta;
        }
    }
}
