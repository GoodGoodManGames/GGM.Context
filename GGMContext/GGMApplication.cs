using System;
using System.Reflection;
using GGMContext.Context;

namespace GGMContext
{
    public class GGMApplication
    {
        public static ApplicationContext Run(Type applicationClass, string[] args)
        {
            return new GGMApplication(args).Run(applicationClass);
        }

        private GGMApplication(string[] args)
        {
            Arguments = args;
        }
        
        //TODO: 추후 객체화 될 예정
        public string[] Arguments { get; private set; }
        public Assembly ApplicationAssembly { get; private set; }
        public ApplicationContext Context { get; private set; }

        private ApplicationContext Run(Type applicationClass)
        {
            ApplicationAssembly = applicationClass.Assembly;
            Context = new ApplicationContext(ApplicationAssembly);
            return Context;
        }
    }
}