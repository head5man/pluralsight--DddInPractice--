using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DddInPractice.Logic.Common
{
    public class DomainEvents
    {
        private static Dictionary<Type, List<Delegate>> _dynamicHandlers = new Dictionary<Type, List<Delegate>>();
        private static List<Type> _staticHandlers = new List<Type>();

        public static void Init()
        {
            _dynamicHandlers = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => typeof(IDomainEvent).IsAssignableFrom(x) && !x.IsInterface)
                .ToList()
                .ToDictionary(x => x, (_) => new List<Delegate>());

            _staticHandlers = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.GetInterfaces()
                    .Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IHandler<>)))
                .ToList();
        }

        public static void Register<T>(Action<T> eventHandler)
            where T : IDomainEvent
        {
            _dynamicHandlers[typeof(T)].Add(eventHandler);
        }

        public static void Raise<T>(T domainEvent)
            where T : IDomainEvent
        {
            if (_dynamicHandlers.ContainsKey(domainEvent.GetType()))
            {
                foreach (Delegate handler in _dynamicHandlers[domainEvent.GetType()])
                {
                    var action = (Action<T>)handler;
                    action(domainEvent);
                }
            }

            foreach (Type handler in _staticHandlers.Where(x => typeof(IHandler<T>).IsAssignableFrom(x)))
            {
                IHandler<T> instance = (IHandler<T>)Activator.CreateInstance(handler);
                instance.Handle(domainEvent);
            }
        }
    }
}
