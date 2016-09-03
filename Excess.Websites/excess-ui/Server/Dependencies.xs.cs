﻿using System;
using System.Collections.Generic;
using System.Linq;
using Excess.Runtime;
using Ninject;

namespace excess_ui
{
    [AutoInit]
    public class NinjectExcessModule : Ninject.Modules.NinjectModule
    {
        private class NinjectInstantiator : IInstantiator
        {
            IKernel _kernel = new StandardKernel(new NinjectExcessModule());
            public object Create(Type type)
            {
                return _kernel.GetService(type);
            }
        }

        public static void __init(__Scope scope)
        {
            scope.set<IInstantiator>(new NinjectInstantiator());
        }

        public override void Load()
        {
        }
    }
}