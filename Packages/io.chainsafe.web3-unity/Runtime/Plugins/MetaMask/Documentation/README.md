# Metamask Unity SDK

The MetaMask SDK for Unity.

## How it works

As a developer you can import the MetaMask SDK into your Unity game to enable users to easily connect with their MetaMask Mobile wallet, The supported platforms are macOS, Windows, Linux, IOS, Android & WebGL

The SDK renders a QR code in the UI via a dedicated prefab which players can scan with their MetaMask Mobile app, we also support deep-linking on mobile platforms. Now you can use all the [`ethereum` methods available](guide/ethereum-provider.html) right from your game which you can learn more about in the above link.

## Build Settings

When building the SDK there is a couple of required changes be made to the unity editor when you would like to compile your build

### IOS
- Disable Enable Bitcode option in xcode from build settings
- Scripting backend - IL2CPP
- IL2CPP Code Generation - Faster Smaller Build

### WebGL
- Navigate to the player settings then navigate to the "Resolution & Presentation Tab" and then choose MetaMask
- Scripting backend - IL2CPP
- IL2CPP Code Generation - Faster Smaller Build

### Android
- Android Jar Resolver - Must be resolved before attempting build (Assets -> External Dependency Manager -> Android Resolver -> Resolve)
- Scripting backend - IL2CPP
- IL2CPP Code Generation - Faster Smaller Build
- Minimum API level - Android 7.0 'Nougat' (API level 24)

### Other Platforms
- Scripting backend - IL2CPP
- IL2CPP Code Generation - Faster Smaller Build

## Getting started

### 1. Install

To install the module, you first have to download the package via the Unity Asset Store which will make the asset available in your **Package Manager**.

Now, you need to import it via the **Package Manager**. To do that, go to the Window menu > Package Manager. Select "My Assets" then select the "MetaMask Unity SDK" and click "Install", You should see the `MetaMask SDK` package now listed in the project packages and be able to interface with it and its examples in the scene.

## Prerequisites

- TextMesh Pro(If you do not have TMP installed you will be prompted to install it by the unity editor automatically, N.B* If you choose this path text will not appear until your first repaint)

### 2. Initialization

The main class you will be interfacing with is called `MetaMaskWallet` it handles the connection to the users wallet as well as processing the requests to it via a socket.io implementation, in order to use it inside Unity, you must attach the component called `MetaMaskUnity` to a gameobject within the editor, this component is a singleton and you can use its `Instance` property to access the Wallet instance, but before doing any of those, you need to initialize it either manually by calling `Initialize();`:

```csharp
MetaMaskUnity.Instance.Initialize();
```

Or just making sure that `Initialize On Start` is checked on the component within the editor, and then this will let you to enable you to make calls to the users wallet using [`ethereum` methods available](guide/ethereum-provider.html) as you would expect in a traditional development environment.

This will initialize the Wallet instance and then it becomes accessible from `MetaMaskUnity.Instance.Wallet`.

### 3. Connection

Once the wallet is now prepared and initialized, now you need to connect to the MetaMask app, all you need to do is to call the `Connect` method on the wallet instance like so:

```csharp
var wallet = MetaMaskUnity.Instance.Wallet;
wallet.Connect();
```

Then you can also subscribe to the `OnWalletConnected` callback on the wallet instance to be notified once the wallet is connected:

```csharp
wallet.WalletConnected += OnWalletConnected;

void OnWalletConnected(object sender, EventArgs e) {
    Debug.Log("Wallet is connected");
}
```

You can also use the `Connect` method from `MetaMaskUnity` that just delegates the call to the Wallet instance:

```csharp
MetaMaskUnity.Instance.Connect();
```

There are a variety of sample buttons included inside the package that you can use that call this method when clicked, these are provided as a convenience to you to get a kickstart with your project.

Once the connection request is made, a QR code will be generated, and based on the transport you're using, which is `Unity UI` by default, either spawns a new Canvas that contains the QR code or the `MetaMaskUnityUIQRImage` generates the QR code when the connection is requested, so if you want to use the latter, make sure to add an instance of the `MetaMaskUnityUIQRImage` component to the scene with its fields provided, the transport field is required too if you want to use it isolated from the canvas that is spawned by the transport, then it'll generate the QR code for you.

### 4. Usage

Now you can make requests to the wallet once it is authorized, you'll notice that when the buttons become interactable or the `WalletAuthorized` event is fired:

```csharp
var wallet = MetaMaskUnity.Instance.Wallet;
wallet.WalletAuthorized += OnWalletAuthorized;

void OnWalletAuthorized(object sender, EventArgs e) {
    Deebug.Log("Wallet is authorized");
}
```

You can now call any Ethereum request on the wallet by calling `wallet.Request(myRequest);`, here is a sample transaction request:

```csharp
var wallet = MetaMaskUnity.Instance.Wallet;
var transactionParams = new MetaMaskTranscation
{
    To = "0xd0059fB234f15dFA9371a7B45c09d451a2dd2B5a",
    From = MetaMaskUnity.Instance.Wallet.SelectedAddress,
    Value = "0x0"
};

var request = new MetaMaskEthereumRequest
{
    Method = "eth_sendTransaction",
    Parameters = new MetaMaskTranscation[] { transactionParams }
};
await wallet.Request(request);
```

### 5. Config

You can customize the default configuration or creating your own config:

#### Edit Default Config

To edit the default config you can do so by either opening the setup window through the `Window > MetaMask > Setup` menu item, or by opening the `MetaMaskConfig` asset in the project window, then you can edit the fields and save the changes.

#### Create New Config

To create a new config, simply right-click on the project window and go to `MetaMask > Config` menu to create a new config, give it a name and then use it when initializing the `MetaMaskUnity` instance.

## API

Here is a quick overview of the APIs from the most important classes.

### MetaMaskUnity

This is a singleton class that you can use to access the `MetaMaskWallet` instance which is specific to Unity.

#### Instance

This is the singleton instance of the `MetaMaskUnity` class that is lazy-loaded when you access it for the first time.

#### Initialize

This method initializes the `MetaMaskWallet` instance and makes it accessible via the `Wallet` property.

You can also pass extra options and parameters to it to customize the wallet instance:

```csharp
// Initialize using default settings
MetaMaskUnity.Instance.Initialize();

// Initialize using custom transport and socket provider
var transport = new MyCustomTransport();
var socketProvider = new MyCustomSocketProvider();
MetaMaskUnity.Instance.Initialize(transport, socketProvider);

// Initialize using custom config, transport and socket provider
var config = myMetaMaskConfig;
var transport = new MyCustomTransport();
var socketProvider = new MyCustomSocketProvider();
MetaMaskUnity.Instance.Initialize(config, transport, socketProvider);
```

#### SaveSession

This method saves the current session to the persistent storage, this is useful when you want to save the session and restore it later on, this is automatically called when the application quits, but you can manually call it too.

#### LoadSession

This method loads the session from the persistent storage, this is useful when you want to restore the session after the application quits, this is automatically called when the application starts, but you can manually call it too.

### MetaMaskWallet

#### Connect

This method connects to the MetaMask app, it will render a generated QRCode in the UI for your users to scan with the MetaMask Mobile app. After the user scan this QR code, a connect screen will appear in MetaMask Mobile where the user can now approve the connection with your game application.

#### Disconnect

This method disconnects the user that is connected from the MetaMask app session.

#### Request

This method is used to send a request to the MetaMask app, you can use it to call any `ethereum` method listed on the [the Ethereum Provider API](guide/ethereum-provider.html).

## Package Structure

- **Documentation**: contains the documentation and link to online documentation
- **Editor**: contains the editor-only code such as Setup GUI windows, data persistence for SDK settings.
- **Plugins**: contains the plugins needed by the package. In particular the ECIES Platform runtime libraries as well as the core SDK Codebase.
- **Runtime**: contains the main scripts for the SDK that are environment agnostic, so they work fine in .NET, The `Runtime` folder contains the C# scripts that you need to import or use in your script for your project, they provide the base implementation of the SDK, you can create your own implementation of Unity components on top of these without ever toching the premade Unity component to have total control.
- **Samples**: contains a test application scene that can be used as a referral for your project including modal popups and dynamic UI scaling.
- **LICENSE.md**: the package license
- **Third Party Notices.md**: third party notices

## License

Check the `LICENSE` file for more information.

## FAQS

#### When I download the tool from the asset store i can’t find where to install it?

The first stage of the installation process is to navigate to the “tools” then “metamask” menu and you will see install options from there, if you do not see this options make sure you are on the latest unity version and that you have no “red “ errors printed in your console, A case of this menu not appearing is typically associated with incorrect editor initialization which can generally be resolved by restarting the editor or updating your unity version.

#### On IOS why does a popup appear when utilizing a deeplink?

When deeplinking a background service is created to facilitate the communication layer between the  game application and the metamask app, On IOS there is a strict policy on how long a background service may life for before expiring hence why a notification is popped to let you know the socket connection has expired.

#### What does the external dependency manager do?
The Unity Jar Resolver is a specific external dependency manager for Unity projects that are using external  libraries in their projects.
It helps manage the dependencies between Unity and external libraries, which can sometimes be complicated due to differences between the two environments.
This tool is particularly useful for the metamask SDK as on Android and IOS a variety of native libraries are needed to facilitate deeplinking and the persistent socket connection.

#### Does the SDK increase my compilation time?
No it doesnt , if you are noticing an increased compilation time it may be related to the ILL2CP pipeline which can take longer to build at compile time but the SDK is filled with precompiled libraries to save runtime compilation.
