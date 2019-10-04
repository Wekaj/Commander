using System;

namespace LD45.Utilities {
    public interface IServiceContainer : IServiceProvider {
        void SetService(object service);
    }
}
