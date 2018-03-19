using System;
using System.Reflection;
using System.Reflection.Emit;
using static System.Reflection.Emit.OpCodes;

namespace GGM.Context
{
    using Attribute;
    using Exception;
    using Util;

    /// <summary>
    /// 생성자를 이용한 ManagedDefinition입니다.
    /// </summary>
    internal sealed class ConstructorManagedDefinition : BaseManagedDefinition
    {
        public ConstructorManagedDefinition(ConstructorInfo constructorInfo)
            : base(constructorInfo.DeclaringType)
        {
            if(constructorInfo == null)
                throw new ArgumentNullException(nameof(constructorInfo));

            var managedAttribute = TargetType.GetCustomAttribute<ManagedAttribute>();
            CreateManagedException.Check(managedAttribute != null, CreateManagedError.NotManagedClass);
            ManagedType = managedAttribute.ManagedType;
            
            NeedParameterTypes = constructorInfo.GetParameterTypes();

            var dynamicMethod = new DynamicMethod(GeneratorName, typeof(object), new[] {typeof(object[])}, TargetType);
            var il = dynamicMethod.GetILGenerator();
            for (int i = 0; i < NeedParameterTypes.Length; i++)
            {
                il.Emit(Ldarg_0);
                il.Emit(Ldc_I4, i);
                il.Emit(Ldelem_Ref);

                var parameterType = NeedParameterTypes[i];
                if (parameterType.IsValueType)
                    il.Emit(Unbox_Any, parameterType);
                else
                    il.Emit(Castclass, parameterType);
            }

            il.Emit(Newobj, constructorInfo);
            il.Emit(Ret);

            ManagedGenerator = dynamicMethod.CreateDelegate(typeof(Generator)) as Generator;
        }

        public override ManagedType ManagedType { get; }
        public override Type[] NeedParameterTypes { get; }
        protected override Generator ManagedGenerator { get; }
    }
}