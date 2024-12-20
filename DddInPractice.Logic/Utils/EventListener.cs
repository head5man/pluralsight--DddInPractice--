using DddInPractice.Logic.Common;
using NHibernate.Event;
using NHibernate.Util;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DddInPractice.Logic.Utils
{
    public class EventListener
        :
        IPostInsertEventListener,
        IPostDeleteEventListener,
        IPostUpdateEventListener,
        IPostCollectionUpdateEventListener
    {
        public void OnPostDelete(PostDeleteEvent ev)
        {
            DispatchEvents(ev.Entity);
        }

        public Task OnPostDeleteAsync(PostDeleteEvent ev, CancellationToken token)
        {
            return DispatchEventsAsync(ev.Entity, token);
        }

        public void OnPostInsert(PostInsertEvent ev)
        {
            DispatchEvents(ev.Entity);
        }

        public Task OnPostInsertAsync(PostInsertEvent ev, CancellationToken token)
        {
            return DispatchEventsAsync(ev.Entity, token);
        }

        public void OnPostUpdate(PostUpdateEvent ev)
        {
            DispatchEvents(ev.Entity);
        }

        public Task OnPostUpdateAsync(PostUpdateEvent ev, CancellationToken token)
        {
            return DispatchEventsAsync(ev.Entity, token);
        }

        public void OnPostUpdateCollection(PostCollectionUpdateEvent ev)
        {
            DispatchEvents(ev.AffectedOwnerOrNull);
        }

        public Task OnPostUpdateCollectionAsync(PostCollectionUpdateEvent ev, CancellationToken token)
        {
            return DispatchEventsAsync(ev.AffectedOwnerOrNull, token);
        }

        private async Task DispatchEventsAsync(object entity, CancellationToken token)
        {
            if (entity is AggregateRoot root)
            {
                foreach (IDomainEvent @event in root.DomainEvents)
                {
                    await Task.Run(() => DomainEvents.Dispatch(@event), token);
                }

                root.ClearEvents();
            }
        }

        private void DispatchEvents(object entity)
        {
            if (entity is AggregateRoot root)
            {
                foreach (IDomainEvent @event in root.DomainEvents)
                {
                    DomainEvents.Dispatch(@event);
                }

                root.ClearEvents();
            }
        }
    }
}
