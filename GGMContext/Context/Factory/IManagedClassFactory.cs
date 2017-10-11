using System;

namespace GGMContext.Context.Factory
{
    interface IManagedClassFactory
    {
        object GetManagedObject(Type targetType);
        object InstantiateManagedObject(Type managedObjectType);
    }
}