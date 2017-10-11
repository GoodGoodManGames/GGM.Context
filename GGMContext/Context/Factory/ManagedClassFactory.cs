using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using GGMContext.Context.Attribute;

namespace GGMContext.Context.Factory
{
    public class ManagedClassFactory : IManagedClassFactory
    {
        delegate object GetInstanceDelegate();

        public ManagedClassFactory(Assembly assembly)
        {
            var managedClasses = assembly.GetTypes().Where(type => type.IsDefined(typeof(ManagedAttribute), true));
            foreach (var managedClass in managedClasses)
            {
                if(_managedClassGetter.ContainsKey(managedClass))
                    continue;

                _managedClassGetter[managedClass] = CreateGetter(managedClass);
            }
        }

        private GetInstanceDelegate CreateGetter(Type targetType)
        {
            if (_managedClassGetter.ContainsKey(targetType))
                return _managedClassGetter[targetType];
            
            var managedAttribute = targetType.GetCustomAttribute<ManagedAttribute>(true);
            if (managedAttribute.ClassType == ManagedClassType.Singleton)
            {
                _managedClassLookUp[targetType] = InstantiateManagedObject(targetType);
                return _managedClassGetter[targetType] = () => _managedClassLookUp[targetType];
            }
            else
            {
                return _managedClassGetter[targetType] = MakeConstructorDelegate(targetType);
            }
        }
        

        private readonly Dictionary<Type, object> _managedClassLookUp = new Dictionary<Type, object>();
        private readonly Dictionary<Type, GetInstanceDelegate> _managedClassGetter = new Dictionary<Type, GetInstanceDelegate>();

        public T GetManagedObject<T>() where T : class
        {
            return GetManagedObject(typeof(T)) as T;
        }

        public object GetManagedObject(Type targetType)
        {
            return GetManagedObjectImpl(targetType);
        }

        private object GetManagedObjectImpl(Type targetType)
        {
            return _managedClassGetter[targetType]();
        }

        public object InstantiateManagedObject(Type managedObjectType)
        {
            //TODO: Application Context를 요청하는 경우는 없게 해보자.
            // if (managedObjectType == typeof(ApplicationContext))
            //    return this;

            // ManagedClassType.Proto와 다르게 싱글턴이기 때문에 성능을 따로 고려하지 않음.
            var autoWiredConstructor = managedObjectType.GetConstructors()
                .FirstOrDefault(info => info.IsDefined(typeof(AutoWiredAttribute), true));
            
            if (autoWiredConstructor == null)
                return Activator.CreateInstance(managedObjectType);

            var parameterInfos = autoWiredConstructor.GetParameters();
            var parameters = parameterInfos.Select(info => CreateGetter(info.ParameterType)() ).ToArray();
            return autoWiredConstructor.Invoke(parameters);
        }
        
        private GetInstanceDelegate MakeConstructorDelegate(Type managedClass)
        {
            var constructorInfos = managedClass.GetConstructors();
            ConstructorInfo constructor = null;

            var autoWiredConstructor =
                constructorInfos.FirstOrDefault(info => info.IsDefined(typeof(AutoWiredAttribute)));

            constructor = autoWiredConstructor ?? constructorInfos.First();
            if (constructor == null)
                throw new Exception("Factory에서 사용할 수 있는 생성자가 존재하지 않습니다.");

            var parameterTypes = constructor.GetParameters().Select(info => info.ParameterType).ToArray();
            var parameterExpressions = parameterTypes.Select(Expression.Parameter).ToArray();
            NewExpression newExp = Expression.New(constructor, parameterExpressions);

            var parameterBindExpressions = new List<Expression>(parameterTypes.Length + 1);
            for (int i = 0; i < parameterTypes.Length; i++)
            {
                Type parameterType = parameterTypes[i];
                Expression assignExpression = _managedClassLookUp.ContainsKey(parameterType)
                    ? (Expression) Expression.Constant(_managedClassGetter[parameterType] ())
                    : Expression.Call(MakeConstructorDelegate(parameterType).Method);

                parameterBindExpressions.Add(Expression.Assign(parameterExpressions[i], assignExpression));
            }
            parameterBindExpressions.Add(newExp);
            var block = Expression.Block(parameterExpressions, parameterBindExpressions);

            return Expression.Lambda<GetInstanceDelegate>(block).Compile();
        }
    }
}