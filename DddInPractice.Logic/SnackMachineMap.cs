using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DddInPractice.Logic
{
    public class SnackMachineMap : ClassMap<SnackMachine>
    {
        public SnackMachineMap()
        {
            Id(x => x.Id);

            Component(x => x.MoneyInside, y =>
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
}
