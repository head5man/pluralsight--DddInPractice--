using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DddInPractice.Logic
{
    public class SnackMap : ClassMap<Snack>
    {
        public SnackMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
        }
    }

    public class Snack : AggregateRoot
    {
        public static readonly Snack Chocolate = new Snack(1);
        public static readonly Snack Soda = new Snack(2);
        public static Snack Gum = new Snack(3);

        public virtual string Name { get; protected set; }

        protected Snack()
        {
        }

        private Snack(long id, [CallerMemberName]string name = null)
        {
            Id = id;
            Name = name;
        }
    }
}
