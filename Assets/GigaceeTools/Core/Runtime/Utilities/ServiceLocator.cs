using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GigaceeTools.Service
{
    public static class ServiceLocator
    {
        /// <summary>
        /// The dictionary to register services.
        /// </summary>
        private static readonly Dictionary<Type, IService> s_services = new();

        /// <summary>
        /// Register a service. If a service of the same type is already registered, you should unregister it first.
        /// </summary>
        /// <param name="service">The service to register.</param>
        /// <typeparam name="TService">The type of the service.</typeparam>
        public static void Register<TService>(TService service) where TService : class, IService
        {
            if (service is null)
            {
                Debug.LogError("It does not allow to register null.");
                return;
            }

            Type type = typeof(TService);

            if (!s_services.TryAdd(type, service))
            {
                Debug.LogWarning($"A service of the same type is already registered: {type.Name}");
            }
        }

        /// <summary>
        /// Unregister a service.
        /// </summary>
        /// <param name="service">The service to unregister.</param>
        /// <typeparam name="TService">The type of the service.</typeparam>
        public static void Unregister<TService>(TService service) where TService : class, IService
        {
            (Type type, IService registeredService) = s_services.SingleOrDefault(pair => Equals(pair.Value, service));

            if (type == null)
            {
                Debug.LogWarning($"The passed service is not registered: {service.GetType()}");
                return;
            }

            if (registeredService is IDisposable disposable)
            {
                disposable.Dispose();
            }

            s_services.Remove(type);
        }

        /// <summary>
        /// Checks if a service of the specified type is registered.
        /// </summary>
        /// <typeparam name="TService">The type of a service to check.</typeparam>
        /// <returns>Returns true if a service of the given type is registered.</returns>
        public static bool IsRegistered<TService>() where TService : class, IService
        {
            return s_services.ContainsKey(typeof(TService));
        }

        /// <summary>
        /// Checks if the specified service is registered.
        /// </summary>
        /// <param name="service">The service to check.</param>
        /// <typeparam name="TService">The type of a service to check.</typeparam>
        /// <returns>Returns true if the given service is registered.</returns>
        public static bool IsRegistered<TService>(TService service) where TService : class, IService
        {
            Type type = typeof(TService);

            return s_services.ContainsKey(type) && Equals(s_services[type], service);
        }

        /// <summary>
        /// Get a service.
        /// </summary>
        /// <typeparam name="TService">The type of a service.</typeparam>
        /// <returns>The service of the requested type.</returns>
        public static TService Get<TService>() where TService : class, IService
        {
            Type type = typeof(TService);

            if (s_services.ContainsKey(type))
            {
                return s_services[type] as TService;
            }

            Debug.LogError($"A service of the given type is not registered: {type.Name}");
            return null;
        }

        /// <summary>
        /// Try to get a service.
        /// </summary>
        /// <param name="service">The service of the requested type.</param>
        /// <typeparam name="TService">The type of a service.</typeparam>
        /// <returns>Returns true if the get was successful.</returns>
        public static bool TryGet<TService>(out TService service) where TService : class, IService
        {
            Type type = typeof(TService);
            service = s_services.ContainsKey(type) ? s_services[type] as TService : null;

            return service != null;
        }

        /// <summary>
        /// Unregister all services.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ClearServices()
        {
            s_services.Clear();
        }
    }
}
