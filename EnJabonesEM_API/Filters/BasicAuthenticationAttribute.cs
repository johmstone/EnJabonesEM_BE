﻿using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using BL;
using ET;

namespace EnJabonesEM_API.Filters
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        private UserBL UBL = new UserBL();

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var authHeader = actionContext.Request.Headers.Authorization;

            if (authHeader != null)
            {
                var authenticationToken = actionContext.Request.Headers.Authorization.Parameter;
                var decodedAuthenticationToken = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));
                var usernamePasswordArray = decodedAuthenticationToken.Split(':');
                var userName = usernamePasswordArray[0];
                var password = usernamePasswordArray[1];

                // Replace this with your own system of security / means of validating credentials
                UserLogin login = new UserLogin()
                {
                    Email = userName,
                    PasswordHash = password
                };

                User LoginUser = UBL.Login(login);

                //var isValid = userName == "andy" && password == "password";

                if (LoginUser.UserID > 0)
                {
                    var principal = new GenericPrincipal(new GenericIdentity(userName), null);
                    Thread.CurrentPrincipal = principal;

                    //actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK);
                    return;
                }
                else
                {
                    if (LoginUser.UserID == -1)
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                        return;
                    }
                    else
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.NotFound);
                        return;
                    }
                }
            }

            HandleUnathorized(actionContext);
        }

        private static void HandleUnathorized(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            //actionContext.Response.Headers.Add("WWW-Authenticate", "Basic Scheme='Data' location = 'http://localhost:");
        }
    }
}