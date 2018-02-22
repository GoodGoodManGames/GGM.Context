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
    /// Configuration의 Factory메소드를 이용한 ManagedDefinition입니다.
    /// </summary>
    internal sealed class ConfigurationManagedDefinition : BaseManagedDefinition
    {
        
        public ConfigurationManagedDefinition(MethodInfo methodInfo)
            : base(methodInfo.ReturnType)
        {
            if(methodInfo == null)
                throw new ArgumentNullException(nameof(methodInfo));
            
            var managedAttribute = methodInfo.GetCustomAttribute<ManagedAttribute>();
            CreateManagedException.Check(managedAttribute != null, CreateManagedError.NotManagedClass);
            ManagedType = managedAttribute.ManagedType;
            
            var configurationType = methodInfo.DeclaringType;

            // FactoryMethod 등록시에는 NeedParameterTypes의 첫번째에 해당 메소드의 DeclaringType이 들어간다.
            var parameterTypes = methodInfo.GetParameterTypes();
            NeedParameterTypes = new Type[1 + parameterTypes.Length];
            NeedParameterTypes[0] = configurationType;
            for (int i = 0; i < parameterTypes.Length; i++)
                NeedParameterTypes[i + 1] = parameterTypes[i];

            var dynamicMethod = new DynamicMethod(GeneratorName, typeof(object), new[] {typeof(object[])});
            var il = dynamicMethod.GetILGenerator();
            il.Emit(Ldarg_0);
            il.Emit(Ldc_I4_0);
            il.Emit(Ldelem_Ref);

            for (int i = 0; i < parameterTypes.Length; i++)
            {
                il.Emit(Ldarg_0);
                il.Emit(Ldc_I4, i + 1);
                il.Emit(Ldelem_Ref);

                var parameterType = parameterTypes[i];
                if (parameterType.IsValueType)
                    il.Emit(Unbox_Any, parameterType);
            }

            il.Emit(Call, methodInfo);
            il.Emit(Ret);

            ManagedGenerator = dynamicMethod.CreateDelegate(typeof(Generator)) as Generator;
        }

        public override ManagedType ManagedType { get; }
        public override Type[] NeedParameterTypes { get; }
        protected override Generator ManagedGenerator { get; }
    }
}