using GGMContext.Context.Attribute;
using GGMContext.Context.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using static System.Reflection.Emit.OpCodes;

namespace GGMContext.Context.Factory
{
    /// <summary>
    ///     Managed객체들을 런타임에 생성하는 ManagedFactory입니다.
    ///     들어온 인자들에 매칭되는 생성자를 찾아 실행합니다.
    ///     만약 일치하는 생성자가 없다면 기본생성자를 실행합니다.
    /// </summary>
    public class ManagedFactory : IFactory
    {
        private delegate object Ganerator(object[] parameters);
        private Dictionary<Type, Ganerator> mGenerators = new Dictionary<Type, Ganerator>();

        /// <summary>
        ///     객체를 생성합니다.
        /// </summary>
        /// <param name="type">생성할 클래스타입</param>
        /// <param name="parameters">생성시 사용될 인자</param>
        /// <returns>생성된 객체</returns>
        public object Create(Type type, object[] parameters = null)
        {
            var generator = GetCachedGeneratorInternal(type, parameters);
            return generator(parameters);
        }

        /// <summary>
        ///     객체를 생성합니다.
        /// </summary>
        /// <typeparam name="T">생성할 클래스 다입</typeparam>
        /// <param name="parameters">생성시 사용될 인자</param>
        /// <returns>생성된 객체</returns>
        public T Create<T>(object[] parameters = null) where T : class => Create(typeof(T), parameters) as T;

        private Ganerator GetCachedGeneratorInternal(Type type, object[] parameters)
        {
            if (mGenerators.ContainsKey(type))
                return mGenerators[type];

            var parameterTypes = parameters != null ? parameters.Select(param => param.GetType()).ToArray() : Type.EmptyTypes;

            var constructor = type.GetConstructor(parameterTypes);
            if (constructor == null) throw new CreateManagedException(CreateManagedError.NotExistMatchedConstructor);

            var parameterInfos = constructor.GetParameters();

            var dm = new DynamicMethod($"{type.Name}Factory+{Guid.NewGuid()}", typeof(object), new[] { typeof(object[]) });
            var il = dm.GetILGenerator();
            for (int i = 0; i < parameterInfos.Length; i++)
            {
                il.Emit(Ldarg_0); // [params]
                il.Emit(Ldc_I4, i); // [params] [index]
                il.Emit(Ldelem_Ref); // [params[index]]

                var parameterType = parameterInfos[i].ParameterType;
                if (parameterType.IsValueType)
                    il.Emit(Unbox_Any, parameterType);
            }
            il.Emit(Newobj, constructor);
            il.Emit(Ret);

            return mGenerators[type] = dm.CreateDelegate(typeof(Ganerator)) as Ganerator;
        }
    }
}
