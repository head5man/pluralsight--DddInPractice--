using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public virtual string Name { get; protected set; }

        protected Snack()
        {
        }

        public Snack(string name)
        {
            Name = name;
        }
    }
}
