using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using ContactApi.Web.Common.Security;
using Microsoft.IdentityModel.Tokens;

namespace ContactApi.Web.Common
{
    public class TokenValidationHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpStatusCode statusCode;
            //determine whether a jwt exists or not
            if (!TryRetrieveToken(request, out var token))
            {
                request.CreateResponse(HttpStatusCode.Unauthorized);
                return base.SendAsync(request, cancellationToken);
            }

            try
            {
                var domain = request?.RequestUri.AbsoluteUri.Replace(request.RequestUri.PathAndQuery, string.Empty) ??
                             "http://localhost:50602/";
                var jwtService = new JwtService(domain);
                var claimsPrincipal = jwtService.GetClaimsFromToken(token, out _);
                Thread.CurrentPrincipal = HttpContext.Current.User = claimsPrincipal;
                return base.SendAsync(request, cancellationToken);
            }
            catch (SecurityTokenValidationException e)
            {
                statusCode = HttpStatusCode.Unauthorized;
            }
            catch (Exception ex)
            {
                statusCode = HttpStatusCode.InternalServerError;
            }

            request.CreateResponse(statusCode);
            return base.SendAsync(request, cancellationToken);
        }

        private bool TryRetrieveToken(HttpRequestMessage request, out string token)
        {
            token = null;
            if (!request.Headers.TryGetValues("Authorization", out var authorizationHeader) ||
                authorizationHeader.ToList().Count > 1)
            {
                return false;
            }

            var bearerToken = authorizationHeader.ElementAt(0);
            var bearerPrefix = "Bearer ";
            token = bearerToken.StartsWith(bearerPrefix) ? bearerToken.Substring(bearerPrefix.Length) : bearerToken;
            return true;
        }
    }
}
