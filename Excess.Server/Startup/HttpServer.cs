﻿using Excess.Concurrent.Runtime;
using Microsoft.Owin.Hosting;
using Middleware;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Startup
{
    public static class HttpServer
    {
        public static void Start<T>(string baseUrl)
        {
            using (WebApp.Start<T>(baseUrl))
            {
                Console.WriteLine("Press Enter to quit.");
                Console.ReadKey();
            }
        }

        public static void Start(string baseUrl, 
            IEnumerable<Type> classes = null,
            IEnumerable<KeyValuePair<Guid, ConcurrentObject>> instances = null,
            Action<IAppBuilder> onInit = null,
            bool useStaticFiles = true)
        {
            using (WebApp.Start(baseUrl, (app) =>
            {
                if (useStaticFiles)
                    app.UseStaticFiles();

                app.UseConcurrent(server =>
                {
                    if (classes != null)
                    {
                        foreach (var @class in classes)
                            server.RegisterClass(@class);
                    }

                    if (instances != null)
                    {
                        foreach (var instance in instances)
                            server.RegisterInstance(instance.Key, instance.Value);
                    }
                });

                if (onInit != null)
                    onInit(app);
            }))
            {
                Console.WriteLine("Press Enter to quit."); //td: lol
                Console.ReadKey();
            }
        }
    }
}
