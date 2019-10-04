using System;
using System.Collections.Generic;

namespace LD45.Utilities {
    public sealed class ServiceContainer : IServiceContainer {
        private readonly IServiceProvider _parent;

        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public ServiceContainer(IServiceProvider parent = null) {
            _parent = parent;
        }

        public object GetService(Type type) {
            if (_services.TryGetValue(type, out object service)) {
                return service;
            }
            else if (_parent != null) {
                return _parent.GetService(type);
            }
            else {
                return null;
            }
        }

        public void SetService(object service) {
            Type type = service.GetType();

            if (_services.ContainsKey(type)) {
                _services[type] = service;
            }
            else {
                _services.Add(type, service);
            }
        }
    }
}
