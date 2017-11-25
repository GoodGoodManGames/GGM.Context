using System;
using System.Reflection;
using GGM.Context;

namespace GGM
{
    public class GGMApplication
    {
        private GGMApplication(string[] args)
        {
            Arguments = args;
        }

        //TODO: 추후 객체화 될 예정
        public string[] Arguments { get; }

        public Assembly ApplicationAssembly { get; private set; }
        public ManagedContext Context { get; private set; }

        public static ManagedContext Run(Type applicationClass, string[] args)
        {
            return new GGMApplication(args).Run(applicationClass);
        }

        private ManagedContext Run(Type applicationClass)
        {
            ApplicationAssembly = applicationClass.Assembly;
            Context = new ApplicationContext(ApplicationAssembly);
            return Context;
        }
    }
}