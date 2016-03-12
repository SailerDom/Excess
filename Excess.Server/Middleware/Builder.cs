﻿using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware
{
    public static class BuilderExtensions
    {
        public static void UseConcurrent(this IAppBuilder app, Action<IConcurrentServer> initialize = null)
        {
            var server = new ConcurrentServer();
            if (initialize != null)
                initialize(server);

            app.Use<ConcurrentOwinMiddleware>(server);
        }
    }
}