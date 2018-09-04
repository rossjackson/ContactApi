using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dependencies;

namespace ContactApi.Web.Common
{
    public static class WebContainerManager
    {
        public static IDependencyResolver GetDependencyResolver()
        {
            var dependencyResolver = GlobalConfiguration.Configuration.DependencyResolver;
            if (dependencyResolver != null)
            {
                return dependencyResolver;
            }

            throw new InvalidOperationException("The dependency resolver has not been set.");
        }

        public static T Get<T>()
        {
            var service = GetDependencyResolver().GetService(typeof(T));

            if (service == null)
                throw new NullReferenceException(
                    $"Requested service of type {typeof(T).FullName}, but null was found.");

            return (T) service;
        }

        public static IEnumerable<T> GetAll<T>()
        {
            var services = GetDependencyResolver().GetServices(typeof(T)).ToList();

            if (!services.Any())
                throw new NullReferenceException(
                    $"Requested services of type {typeof(T).FullName}, but none were found.");

            return services.Cast<T>();
        }
    }
}
