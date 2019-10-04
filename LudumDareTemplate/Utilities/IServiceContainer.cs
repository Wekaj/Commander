using System;

namespace LudumDareTemplate.Utilities {
    public interface IServiceContainer : IServiceProvider {
        void SetService(object service);
    }
}
