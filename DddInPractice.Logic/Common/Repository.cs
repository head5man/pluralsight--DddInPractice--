using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DddInPractice.Logic
{
    public abstract class Repository<T>
        where T : AggregateRoot
    {
        public T GetById(long id)
        {
            using (ISession session = SessionFactory.OpenSession())
            {
                return session.Get<T>(id);
            }
        }

        public void Save(T aggregateRoot)
        {
            using (var session = SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.SaveOrUpdate(aggregateRoot);
                transaction.Commit();
            }
        }
    }
}
