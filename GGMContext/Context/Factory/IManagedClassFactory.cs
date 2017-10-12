using System;

namespace GGMContext.Context.Factory
{
    public interface IManagedClassFactory
    {
        object GetManagedObject(Type targetType);
        object InstantiateManagedObject(Type managedObjectType);
    }
}