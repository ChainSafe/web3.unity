![ChainSafeGaming](https://user-images.githubusercontent.com/681817/218129249-850c4be0-b64f-4215-a780-7766db8cd75e.png)


[<img alt="Discord" src="https://img.shields.io/discord/593655374469660673.svg?style=for-the-badge&label=Discord&logo=discord" height="20">](https://discord.gg/Q6A3YA2)
[<img alt="Twitter" src="https://img.shields.io/twitter/follow/espadrine.svg?style=for-the-badge&label=Twitter&color=1DA1F2" height="20">](https://twitter.com/chainsafeth)

## Documentation
You can access the full docs at [here](https://docs.gaming.chainsafe.io).

Our codebase is quite easy to use. To immediately start with reading from the blockchain, once you've installed our core
package, you can simply add a new Web3 client to your scene by right clicking in the hierarchy and selecting Web3 -> Web3 client.

If you want to see our SDK in action, don't forget to import our Samples from the package manager -> web3.unity SDK -> Samples tab on the right.

## Initializing the web3 client
We recommend that you use the c# class generator when interacting with custom contracts [here](https://docs.gaming.chainsafe.io/current/abi-to-csharp-converter/)
This will allow event subscriptions as well as figure out all the datatypes needed to avoid any pesky errors.

## Sample balance of ERC20 call
```csharp
async void Awake()
{
    // Initialize the Web3 Client
    await Web3Unity.Instance.Initialize(false);
    
    // Read the balance of a custom token for the specified account address
    var balance = await Web3Unity.Web3.Erc20.GetBalanceOf(contractAddress, accountAddress);   
}
```

## ERC20 sample contract
Here we're subscribing to the Web3Unity.Web3Initialized event which executes once a user has logged in. We're then calling Web3Initialized which
builds the contract and subscribes to the chosen events.

```csharp
public class CustomContractSample : MonoBehaviour
{
    [SerializeField] private string contractAddress;

    Erc20Contract _erc20Contract;

    void Awake()
    {
        //Make sure to subscribe to the Web3Initialized event in awake so that you don't miss out the event invocation.
        Web3Unity.Web3Initialized += Web3Initialized;
    }

    public async void Start()
    {
        //You should call the Initialize only from a single place.
        //That way there is no potential race condition and the behaviour of the app can become unpredictable.
        if(Web3Unity.Web3 == null)
            await Web3Unity.Instance.Initialize(false);

    }

    //Always create your custom contract inside of the event handler. That way you always have the up-to-date data inside of it. 
    private async void Web3Initialized((Web3 web3, bool isLightweight) obj)
    {
        //Since Web3Initialized event can be invoked multiple times during the app lifecycle (once you open the app and don't have a wallet, then when there is a wallet etc.)
        //You need to properly dispose the previously created contract to remove any potential memory leaks. 
        if (_erc20Contract != null)
        {
            _erc20Contract.OnTransfer -= Erc20Transfer;
            await _erc20Contract.DisposeAsync();
        }

        _erc20Contract = await web3.ContractBuilder.Build<Erc20Contract>(contractAddress);
        _erc20Contract.OnTransfer += Erc20Transfer;
    }

    public async string BalanceOf(string address)
    {
        return await _erc20Contract.BalanceOf(address);
    }

    public async void Transfer(string destinationAddress, BigInteger amount)
    {
        await _erc20.Transfer(destinationAddress, amount);
    }

    public async void OnDestroy()
    {
        Web3Unity.Web3Initialized -= Web3Initialized;
        await _erc20Contract.DisposeAsync();
    }
    

}
```

## Custom read/write calls
If you'd like a more direct way of testing methods similar to the old EVM.ContractCall/Send, you can use the custom read & write functions below.

```csharp
public class CustomContractSample : MonoBehaviour
{
    [SerializeField] private string contractAddress;
    [SerializeField] private string contractAbi;
    [SerializeField] private string contractMethod;

    public async void Start()
    {
        if(Web3Unity.Web3 == null)
            await Web3Unity.Instance.Initialize(false);

        //Used for writing to the chain
        await Web3Unity.Instance.ContractSend(contractMethod, contractAbi, contractAddress);

        
        //Used for reading from the chain
        await Web3Unity.Instance.ContractRead(contractMethod, contractAbi, contractAddress);
    }

}
```

Additional sample scripts can be found [here](https://docs.gaming.chainsafe.io/current/sample-scripts).

## Support
- Need help with web3.unity or found a bug? Be sure to read the documentation above, then review existing issues or create a new one [here](https://github.com/ChainSafe/web3.unity/issues). This is the best way to get help from the ChainSafe Gaming team.
- Need help from the community, including questions not related to web3.unity? Ask in #community-code-support on [Discord](https://discord.gg/Q6A3YA2).

## Contributing
- Have an idea for a new feature that would improve web3.unity? Create a feature request [here](https://github.com/ChainSafe/web3.unity/issues/new?assignees=&labels=Type%3A+Feature&template=feature_request.md&title=).
- Have a code contribution you'd like to make directly? Make a pull request to this repo!
- Join our community! Say hi on [Discord](https://discord.gg/Q6A3YA2) in #web3unity-gaming-general and share your project built with web3.unity in #gaming-showcase.

## Building the code
- Clone the repository and run the `setup` script.
- You can now open `src/UnitySampleProject` in Unity and start hacking!
- Whenever you make changes to the code in the libraries, run the `src/ChainSafe.Gaming.Unity/publish-to-unity-package` script from within the folder it is in. This will build and publish the libraries to the UPM package's folder, thus making any new code accessible inside Unity.

## ChainSafe Security Policy

### Reporting a Security Bug
We take all security issues seriously, if you believe you have found a security issue within a ChainSafe
project please notify us immediately. If an issue is confirmed, we will take all necessary precautions 
to ensure a statement and patch release is made in a timely manner.

Please email us a description of the flaw and any related information (e.g. reproduction steps, version) to
[security at chainsafe dot io](mailto:security@chainsafe.io).
