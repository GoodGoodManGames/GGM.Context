using System;
using System.Reflection;
using System.Reflection.Emit;
using static System.Reflection.Emit.OpCodes;

namespace GGM.Context
{
    using Attribute;
    using Exception;
    using Util;

    internal class ConfigurationManagedDefinition : BaseManagedDefinition
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="configurationType"></param>
        /// <param name="methodInfo"></param>
        /// <param name="parameterTypes"></param>
        public ConfigurationManagedDefinition(Type configurationType, MethodInfo methodInfo)
            : base(methodInfo.ReturnType, methodInfo.GetParameterTypes())
        {
            _configurationType = configurationType;
            _methodInfo = methodInfo;
            var managedAttribute = _methodInfo.GetCustomAttribute<ManagedAttribute>();
            CreateManagedException.Check(managedAttribute != null, CreateManagedError.NotManagedClass);

            ManagedType = managedAttribute.ManagedType;

            var dynamicMethod = new DynamicMethod($"{TargetType.Name}Factory+{Guid.NewGuid()}", typeof(object), new[] {typeof(object[])});
            var il = dynamicMethod.GetILGenerator();
            il.Emit(Ldarg_0);
            il.Emit(Ldc_I4_0);
            il.Emit(Ldelem_Ref);

            for (int i = 0; i < ParameterTypes.Length; i++)
            {
                il.Emit(Ldarg_0);
                il.Emit(Ldc_I4, i + 1);
                il.Emit(Ldelem_Ref);

                var parameterType = ParameterTypes[i];
                if (parameterType.IsValueType)
                    il.Emit(Unbox_Any, parameterType);
            }

            il.Emit(Call, methodInfo);
            il.Emit(Ret);

            ManagedGenerator = dynamicMethod.CreateDelegate(typeof(Generator)) as Generator;
        }

        private readonly Type _configurationType;
        private readonly MethodInfo _methodInfo;

        private Type[] _needParameterTypes;
        public override ManagedType ManagedType { get; }

        public override Type[] NeedParameterTypes
        {
            get
            {
                if (_needParameterTypes == null)
                {
                    _needParameterTypes = new Type[1 + ParameterTypes.Length];
                    _needParameterTypes[0] = _configurationType;
                    for (int i = 0; i < ParameterTypes.Length; i++)
                        _needParameterTypes[i + 1] = ParameterTypes[i];
                }

                return _needParameterTypes;
            }
        }

        protected override Generator ManagedGenerator { get; }
    }
}