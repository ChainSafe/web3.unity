using System;
using System.Collections.Generic;
using System.Reflection;
using evm.net.Models;

namespace evm.net.Factory
{
    public class BackedTypeContractFactory : IContractFactory
    {
        private Dictionary<Type, Type> _interfaceMapping = new Dictionary<Type, Type>();

        public T BuildNewInstance<T>(IProvider provider, EvmAddress address) where T : class
        {
            var interfaceType = typeof(T);

            var backedTypeAttribute = interfaceType.GetCustomAttribute<BackedTypeAttribute>();

            var hasMapping = _interfaceMapping.ContainsKey(interfaceType);

            if (backedTypeAttribute == null)
            {
                if (!hasMapping)
                    throw new AggregateException($"No mapped class type for interface type {interfaceType.FullName}");
            }
            else if (!hasMapping)
            {
                var backedType = backedTypeAttribute.ImplementationType;
                
                RegisterType(interfaceType, backedType);
            }

            var classType = _interfaceMapping[interfaceType];

            var newInstance = Activator.CreateInstance(classType, provider, address, interfaceType);

            return newInstance as T;
        }

        public void RegisterType(Type interfaceType, Type classType)
        {
            _interfaceMapping.Add(interfaceType, classType);
        }

        public void RegisterType<TIt, TCt>() where TIt : class, IContract where TCt : class, IContract =>
            RegisterType(typeof(TIt), typeof(TCt));
    }
}