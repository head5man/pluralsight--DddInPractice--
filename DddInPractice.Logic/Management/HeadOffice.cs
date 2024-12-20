using FluentNHibernate.Mapping;
using System;

namespace DddInPractice.Logic.Management
{
    public class HeadOfficeMap : ClassMap<HeadOffice>
    {
        public HeadOfficeMap()
        {
            Id(x => x.Id);

            Map(x => x.Balance);

            Component(x => x.Cash, y =>
            {
                y.Map(z => z.OneCentCount);
                y.Map(z => z.TenCentCount);
                y.Map(z => z.QuarterCount);
                y.Map(z => z.OneDollarCount);
                y.Map(z => z.FiveDollarCount);
                y.Map(z => z.TwentyDollarCount);
            });
        }
    }

    public class HeadOffice : AggregateRoot
    {
        public virtual decimal Balance { get; protected set; }
        public virtual Money Cash { get; protected set; } = Money.None;

        public virtual void ChangeBalance(decimal delta)
        {
            Balance += delta;
        }
    }
}