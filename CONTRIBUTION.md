# Contribution Guide for web3.unity

Welcome to the web3.unity project! We appreciate your interest in contributing to this open-source SDK, designed to facilitate the integration of web3 features into Unity-based video games. This guide will provide you with all the essential information you need to make meaningful contributions to our project.

## Getting Started

1. **Fork the Repository**: Start by forking the [web3.unity repository](https://github.com/ChainSafe/web3.unity) on GitHub.
2. **Clone the Fork**: Next, clone your forked repository to your local machine.

### Forking the Repository

If you're unfamiliar with Forking, here is a quick guide on how to fork our repo:

1. Go to the [web3.unity repository](https://github.com/ChainSafe/web3.unity) on GitHub.
2. Click the "Fork" button in the top-right corner of the page.
3. Select your GitHub account as the destination to fork the repository.

## Setting Up the Development Environment

### macOS
**Note: If you're building the project for iOS, you have to install Xcode.**

1. **Install Unity**: Install Unity Hub by downloading it from Unity's official website.

2. **Install Homebrew**: Install Homebrew using the following Terminal command:

    ```
    /bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"
    ```

3. **Install .NET SDK**: Install .NET through Homebrew:

    ```
    brew install --cask dotnet-sdk
    ```
    This will install the latest .NET SDK to your machine.

### Linux

1. **Install Unity**: Follow the instructions for your Linux distribution on the Unity website.

2. **Install .NET SDK**: If you're using a Debian-based Linux distro (i.e., Debian, Ubuntu, Linux Mint, etc.), use the following Terminal command to install the .NET SDK:

    ```
    sudo apt-get install dotnet-sdk-7.0
    ```
    For non-Debian Linux flavors, please follow this [link](https://learn.microsoft.com/en-us/dotnet/core/install/linux) to learn more about how to install the .NET SDK into your operating system.

### Windows

1. **Download Unity**: Download and install Unity Hub from the Unity website.
2. **Download .NET SDK**: Download and install the .NET SDK from the Microsoft website.

## Opening The Project
The project is divided into two solutions. The first solution, Chainsafe.Gaming, is the core of our SDK. Here is located our famous Web3 class, which is the core of the communication with blockchain.
The second solution is the one inside the UnitySampleProject. 
This project contains a lot of Unity-specific code that helps facilitate our Chainsafe.Gaming in the Unity environment.
UnitySampleProject also contains a lot of different packages that are dependent on the core web3.unity one, like web3auth, lootboxes, and ramp.

## Deciding Where to Write the Code
Because we have two projects, developers can often be confused about in which project they should start writing the code. 
General rule of thumb:
1. If your code will directly use some of the Unity features, it should probably live inside the UnitySampleProject. 
Note: If you want to add a new module (like web3auth, ramp, or lootboxes), you should create a new package inside the Root of the repository's Packages folder.
2. If your code doesn't care much about Unity but is doing a lot of communication with the blockchain itself, you should definitely create a new csproj inside our Chainsafe.Gaming solution. 

If you're unsure how you would actually extend Chainsafe's SDK, take a look at this lengthy [video](https://youtu.be/D6_786zPva8). It shows in depth how you can add your own code and injectors to the Chainsafe.Gaming project.

## Contributing Code

1. **Create a New Branch**: Create a new branch for your contribution using the command `git checkout -b my-contribution`.
2. **Make Improvements**: Implement the necessary changes and improvements in the codebase.
3. **Adhere to Guidelines**: Ensure that your code follows the project's coding conventions and guidelines.
4. **Write Tests**: Write unit tests for your code and ensure that all existing tests pass.
5. **Commit Changes**: Commit your changes with a descriptive commit message using `git commit -m "Add feature X"`.
6. **Push to Your Fork**: Push your changes to your forked repository with `git push origin my-contribution`.
7. **Submit a Pull Request**: Finally, submit a pull request targeting the `dev` branch of web3.unity repository and label it with `ready-to-merge` so required checks can initialize once your changes are ready for review.

## Reporting Issues

If you come across any bugs, have suggestions, or want to request new features, please submit an issue on the [GitHub issue tracker](https://github.com/ChainSafe/web3.unity/issues). When submitting an issue, provide comprehensive details, including steps to reproduce the problem and relevant code snippets.

## Collaboration and Communication

We encourage collaboration and discussion among contributors. Join our community on Discord to connect with fellow developers, ask questions, and share ideas. Additionally, we have various communication channels, both synchronous and asynchronous, to coordinate project-related discussions.

## Proposing New Changes

If you have an idea for a feature but don't know how to implement it, follow these steps:

1. **Create an Issue**: Create a new issue on the [GitHub issue tracker](https://github.com/ChainSafe/web3.unity/issues).
2. **Provide Details**: Provide a detailed description of your proposed changes, including any design or implementation considerations.
3. **Engage in Discussion**: Engage with the community and maintainers to refine and validate your proposal.

Thank you for contributing to web3.unity. Together, we'll make it an even better SDK for Unity-based video games, empowering developers to seamlessly integrate web3 features into their projects!
