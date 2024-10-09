# Modular approach

As we're trying to keep our codebase maintainable and easily scalable Modular Approach has been introduced.
The philosophy here is to keep interconnected code as loosely coupled as possible.

# SDK Configuration

Every component of the SDK is swappable because it follows the rules 
established by its interface. So we can have `Web3AuthSigner`, `MetaMaskSigner` & `JsonRpcSinger` all available to a
user to configure his application however he wants.

All configuration happens during the construction of Web3 object.

```csharp
var web3 = new Web3Builder().Configure(services =>
{
    services.UseJsonRpcProvider();
    services.UseWeb3AuthSigner();
    services.UseSomething();
})
.Build();

var balance = await web3.Provider.GetBalance();
```

# Module integration

`services.UseJsonRpcProvider()` is an extension method. When you add a
new module you have to provide the extension methods that register components provided by the module.

It looks like this:
```csharp
public static IWeb3ServiceCollection UseJsonRpcProvider(this IWeb3ServiceCollection serviceCollection)
{
    // assert no Provider yet bound
    serviceCollection.AssertServiceNotBound<IEvmProvider>();
    
    // register Service with IEvmProvider as a Contract Type
    // and JsonRpcProvider as Implementation Type
    serviceCollection.AddSingleton<IEvmProvider, JsonRpcProvider>();
    return serviceCollection;
}
```

Each module should ideally be a separate project referencing Core library. For now JsonRpc module is a part of Core project,
but don't let that stop you from creating your own project for a new module.

# Dependency injection

Later when user requests `web3.Provider`, DI framework handles this requests and creates an 
instance of `JsonRpcProvider` with all it's dependencies injected using its constructor.

```csharp
public JsonRpcProvider(JsonRpcProviderConfiguration configuration,
            Web3Environment environment,
            ChainProvider chainProvider)
{
    _chainProvider = chainProvider;
    _environment = environment;
    _configuration = configuration;
}
```

You can see here that `JsonRpcProvider` requires a Configuration object, `ChainProvider` and `Web3Environment`
as dependencies.

# Component configuration

`JsonRpcProviderConfiguration` is a simple data object used to configure `JsonRpcProvider`. It look like this:
```csharp
[Serializable]
public class JsonRpcProviderConfiguration
{
    public string RpcNodeUrl;
    public Network Network;
}
```
You provide it during Web3 construction using `Configure*COMPONENT_NAME*` extension method.
```csharp
public static IWeb3ServiceCollection ConfigureJsonRpcProvider(this IWeb3ServiceCollection serviceCollection, JsonRpcProviderConfiguration configuration)
{
    serviceCollection.AssertConfigurationNotBound<JsonRpcProviderConfiguration>();
    serviceCollection.AddSingleton(configuration);
    return serviceCollection;
}
```
This is one more extension method you have to provide with your module if it supports configuration.

Yet in most cases user won't need a separate `Configure` method. That's why we should create `UseJsonRpcProvider` method
that handles both service and configuration binding:
```csharp
public static IWeb3ServiceCollection UseJsonRpcProvider(this IWeb3ServiceCollection serviceCollection, JsonRpcProviderConfiguration configuration)
{
    serviceCollection.ConfigureJsonRpcProvider(configuration);
    serviceCollection.UseJsonRpcProvider();
    return serviceCollection;
}
```

# Default services

`ChainProvider` is a Default Service registered automatically in `Web3Builder` during construction.
```csharp
public Web3Builder()
{
    _serviceCollection = new Web3ServiceCollection();

    // Bind default services
    _serviceCollection.AddSingleton<ChainProvider>();
}
```
If you ever need to bind a service, that's' not swappable and can be used by many other components 
as some sort of helper, bind it here. No interface required this time.

If your service is platform dependent you'd have to bind it as an Environment service.

```csharp
public static IWeb3ServiceCollection UseUnityEnvironment(this IWeb3ServiceCollection services)
{
    services.AddSingleton<Web3Environment>();
    services.AddSingleton<IMainThreadRunner, UnityDispatcherAdapter>(); // <- this one
    ...
    return services;
}
```
You can see I used `IMainThreadRunner` (not just `UnityDispatcherAdapter`) as a contract type. You don't need that
if you think you'll only have one implementation of `IMainThreadRunner`.

# Environment

`Web3Environment` represents the Environment the SDK is being used in. Currently there are `UnityEnvironment` and `NetCoreEnvironment`.
There is also going to be `UnrealEnvironment` when we start supporting Unreal Engine.

Right now `Web3Environment` consists of these required services:

```csharp
public class Web3Environment
{
    public ISettingsProvider SettingsProvider { get; }
    public IHttpClient HttpClient { get; }
    public ILogWriter LogWriter { get; }
    public IAnalyticsClient AnalyticsClient { get; }
}
```
You can reference any of this Services directly using their interface type in the constructor of your component:
```csharp
public JsonRpcProvider(IHttpClient httpClient)
{
    _httpClient = httpClient; // ready to use
}
```
As well as any component or default service:
```csharp
public JsonRpcSigner(IEvmProvider provider, ChainProvider chainProvider)
{
    _chainProvider = chainProvider;
    _provider = provider;
}
```

# Backward-compatibility

As I'm writing this we are working on a seamless technique for backward-compatibility. 
But for now there are 3 classes:
* `MigrationHelper`
* `ProviderMigration`
* `SignerMigration`

Most of code is compatible by default, but for Provider and Singer we have to use 
this for object instantiation:
```csharp
var provider = ProviderMigration.NewJsonRpcProvider("http://127.0.0.1:7545");
var signer = SignerMigration.NewJsonRpcSigner(provider);
```
_Migration might not be the best word for describing this process. Ping me (@oleksandrchainsafe) if you have better ideas._

# Useful links

* [Dependency Injection Framework](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection)
* [Design Patterns by Gang of Four](https://ru.wikipedia.org/wiki/Design_Patterns)
