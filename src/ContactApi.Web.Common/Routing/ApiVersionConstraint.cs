using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Routing;

namespace ContactApi.Web.Common.Routing
{
    public class ApiVersionConstraint : IHttpRouteConstraint
    {
        private readonly string _allowedVersion;

        public ApiVersionConstraint(string allowedVersion)
        {
            _allowedVersion = allowedVersion.ToLowerInvariant();
        }

        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values,
            HttpRouteDirection routeDirection)
        {
            if (values.TryGetValue(parameterName, out var value) && value != null)
            {
                return _allowedVersion.Equals(value.ToString().ToLowerInvariant());
            }
            return false;
        }
    }
}
