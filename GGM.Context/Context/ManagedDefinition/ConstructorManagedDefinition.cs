using System;
using System.Reflection;
using System.Reflection.Emit;
using static System.Reflection.Emit.OpCodes;

namespace GGM.Context
{
    using Attribute;
    using Exception;
    using Util;


    internal class ConstructorManagedDefinition : BaseManagedDefinition
    {
        public ConstructorManagedDefinition(Type targetType, ConstructorInfo constructorInfo)
            : base(targetType, constructorInfo.GetParameterTypes())
        {
            var managedAttribute = TargetType.GetCustomAttribute<ManagedAttribute>();
            CreateManagedException.Check(managedAttribute != null, CreateManagedError.NotManagedClass);
            ManagedType = managedAttribute.ManagedType;


            var dynamicMethod = new DynamicMethod($"{TargetType.Name}Factory+{Guid.NewGuid()}", typeof(object), new[] {typeof(object[])});
            var il = dynamicMethod.GetILGenerator();
            for (int i = 0; i < ParameterTypes.Length; i++)
            {
                il.Emit(Ldarg_0);
                il.Emit(Ldc_I4, i);
                il.Emit(Ldelem_Ref);

                var parameterType = ParameterTypes[i];
                if (parameterType.IsValueType)
                    il.Emit(Unbox_Any, parameterType);
            }

            il.Emit(Newobj, constructorInfo);
            il.Emit(Ret);

            ManagedGenerator = dynamicMethod.CreateDelegate(typeof(Generator)) as Generator;
        }

        public override ManagedType ManagedType { get; }
        public override Type[] NeedParameterTypes => ParameterTypes;
        protected override Generator ManagedGenerator { get; }
    }
}