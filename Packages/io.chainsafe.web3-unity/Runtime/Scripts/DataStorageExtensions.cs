using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChainSafe.Gaming.LocalStorage;
using ChainSafe.Gaming.Unity;
using ChainSafe.Gaming.Web3.Environment;
using ChainSafe.Gaming.Web3.Unity;
using Microsoft.Extensions.DependencyInjection;
using UnityEngine;

namespace ChainSafe.Gaming.UnityPackage
{
    public static class DataStorageExtensions
    {
        public static Task<TStorable> LoadOneTime<TStorable>(this TStorable storable)
            where TStorable : class, IStorable
        {
            IServiceCollection services = new ServiceCollection()
                .AddSingleton<DataStorage>()
                .AddSingleton<IStorable>(storable)
                .AddSingleton<ILogWriter, UnityLogWriter>()
                .AddSingleton<IOperatingSystemMediator, UnityOperatingSystemMediator>()
                .AddSingleton<IMainThreadRunner, UnityDispatcherAdapter>();
            
            return storable.LoadOneTime(services);
        }
        
        public static Task SaveOneTime<TStorable>(this TStorable storable)
            where TStorable : class, IStorable
        {
            IServiceCollection services = new ServiceCollection()
                .AddSingleton<DataStorage>()
                .AddSingleton<IStorable>(storable)
                .AddSingleton<ILogWriter, UnityLogWriter>()
                .AddSingleton<IOperatingSystemMediator, UnityOperatingSystemMediator>()
                .AddSingleton<IMainThreadRunner, UnityDispatcherAdapter>();
            
            return storable.SaveOneTime(services);
        }
    }
}
