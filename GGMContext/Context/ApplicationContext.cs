using GGM.Context.Attribute;
using System.Linq;
using System.Reflection;

namespace GGM.Context
{
    public class ApplicationContext : ManagedContext
    {
        public ApplicationContext(Assembly assembly)
        {
            Assembly = assembly;

            var allTypes = Assembly.GetTypes();
            var managedTypes = allTypes
                .Where(type => type.IsDefined(typeof(ManagedAttribute)))
                .ToDictionary(type => type, type => type.GetCustomAttribute<ManagedAttribute>());

            // Singleton들은 미리 생성.
            foreach(var managedType in managedTypes)
            {
                // Key : type
                // Value : ManagedAttribute
                if (managedType.Value.ManagedType == ManagedType.Singleton)
                    GetManaged(managedType.Key);
            }
        }

        public Assembly Assembly { get; }
    }
}
