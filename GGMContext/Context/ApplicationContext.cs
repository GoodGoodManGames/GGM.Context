using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GGMContext.Context.Attribute;
using GGMContext.Context.Factory;

namespace GGMContext.Context
{
    public class ApplicationContext : ManagedClassFactory
    {
        public ApplicationContext(Assembly assembly) : base(assembly)
        {
            ApplicationAssembly = assembly;
        }

        public Assembly ApplicationAssembly { get; private set; }
    }



}