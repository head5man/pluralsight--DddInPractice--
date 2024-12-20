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
            DispatchEvents(ev.Entity as AggregateRoot);
        }

        public Task OnPostDeleteAsync(PostDeleteEvent ev, CancellationToken token)
        {
            return DispatchEventsAsync(ev.Entity as AggregateRoot, token);
        }

        public void OnPostInsert(PostInsertEvent ev)
        {
            DispatchEvents(ev.Entity as AggregateRoot);
        }

        public Task OnPostInsertAsync(PostInsertEvent ev, CancellationToken token)
        {
            return DispatchEventsAsync(ev.Entity as AggregateRoot, token);
        }

        public void OnPostUpdate(PostUpdateEvent ev)
        {
            DispatchEvents(ev.Entity as AggregateRoot);
        }

        public Task OnPostUpdateAsync(PostUpdateEvent ev, CancellationToken token)
        {
            return DispatchEventsAsync(ev.Entity as AggregateRoot, token);
        }

        public void OnPostUpdateCollection(PostCollectionUpdateEvent ev)
        {
            DispatchEvents(ev.AffectedOwnerOrNull as AggregateRoot);
        }

        public Task OnPostUpdateCollectionAsync(PostCollectionUpdateEvent ev, CancellationToken token)
        {
            return DispatchEventsAsync(ev.AffectedOwnerOrNull as AggregateRoot, token);
        }

        private async Task DispatchEventsAsync(AggregateRoot aggregateRoot, CancellationToken token)
        {
            foreach (IDomainEvent @event in aggregateRoot?.DomainEvents)
            {
                await Task.Run(() => DomainEvents.Dispatch(@event), token);
            }

            aggregateRoot?.ClearEvents();
        }

        private void DispatchEvents(AggregateRoot aggregateRoot)
        {
            foreach (IDomainEvent @event in aggregateRoot?.DomainEvents)
            {
                DomainEvents.Dispatch(@event);
            }

            aggregateRoot?.ClearEvents();
        }
    }
}
