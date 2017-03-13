using System;
using System.ComponentModel.Design;

namespace GuruComponents.Netrix
{
    /// <summary>
    /// Provides internal services.
    /// </summary>
    public class NetrixServiceProvider : IServiceProvider, IServiceContainer
    {

        IServiceContainer services;

        /// <summary>
        /// ctor
        /// </summary>
        public NetrixServiceProvider()
        {
            this.services = new ServiceContainer();
        }

        IServiceContainer ServiceContainer
        {
            get { return services; }
        }

        #region IServiceProvider Member

        /// <summary>
        /// Returns a service object.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public object GetService(Type serviceType)
        {
            return services.GetService(serviceType);
        }

        #endregion

        /// <summary>
        /// Adds a service to the container.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="callback"></param>
        /// <param name="promote"></param>
        public void AddService(Type serviceType, ServiceCreatorCallback callback, bool promote)
        {
            ServiceContainer.AddService(serviceType, callback, promote);
        }

        /// <summary>
        /// Adds a service to the container.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="callback"></param>
        public void AddService(Type serviceType, ServiceCreatorCallback callback)
        {
            ServiceContainer.AddService(serviceType, callback);
        }
        /// <summary>
        /// Adds a service to the container.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceInstance"></param>
        /// <param name="promote"></param>
        public void AddService(Type serviceType, object serviceInstance, bool promote)
        {
            ServiceContainer.AddService(serviceType, serviceInstance, promote);
        }
        /// <summary>
        /// Adds a service to the container.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceInstance"></param>
        public void AddService(Type serviceType, object serviceInstance)
        {
            ServiceContainer.AddService(serviceType, serviceInstance);
        }
        /// <summary>
        /// Removes a service from the container.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="promote"></param>
        public void RemoveService(Type serviceType, bool promote)
        {
            ServiceContainer.RemoveService(serviceType, promote);
        }
        /// <summary>
        /// Removes a service from the container.
        /// </summary>
        /// <param name="serviceType"></param>
        public void RemoveService(Type serviceType)
        {
            ServiceContainer.RemoveService(serviceType);
        }
    }
}