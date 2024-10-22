![ChainSafeGaming](https://user-images.githubusercontent.com/681817/218129249-850c4be0-b64f-4215-a780-7766db8cd75e.png)


[<img alt="Discord" src="https://img.shields.io/discord/593655374469660673.svg?style=for-the-badge&label=Discord&logo=discord" height="20">](https://discord.gg/Q6A3YA2)
[<img alt="Twitter" src="https://img.shields.io/twitter/follow/espadrine.svg?style=for-the-badge&label=Twitter&color=1DA1F2" height="20">](https://twitter.com/chainsafeth)

## Documentation
You can access the full docs at [docs.gaming.chainsafe.io](https://docs.gaming.chainsafe.io).

Our codebase is quite easy to use. To immediately start with reading from the blockchain, once you've installed our core package,you can simply drag and drop our Web3Unity prefab to the scene and do the following

```csharp
async void Awake()
{
    await Web3Unity.Instance.Initialize(false);
    var balance = await Web3Unity.Web3.Erc20.GetBalanceOf(contractAddress, accountAddress);   
}
```

Additional prefab scripts can be found here [docs.gaming.chainsafe.io/current/sample-scripts](https://docs.gaming.chainsafe.io/current/sample-scripts).

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
