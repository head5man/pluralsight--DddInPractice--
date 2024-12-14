using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DddInPractice.Logic
{
    public class Slot : Entity
    {
        protected Slot()
        {
        }

        public Slot(SnackMachine snackMachine, int position, Snack snack, int quantity, decimal price)
            : this()
        {
            Snack = snack;
            Quantity = quantity;
            Price = price;
            SnackMachine = snackMachine;
            Position = position;
        }

        public virtual Snack Snack { get; set; }
        public virtual int Quantity { get; set; }
        public virtual decimal Price { get; set; }
        public virtual SnackMachine SnackMachine { get; protected set; }
        public virtual int Position { get; protected set; }
    }
}
