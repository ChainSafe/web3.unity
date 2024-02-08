# Contribution Guide for web3.unity

Welcome to the web3.unity project! We appreciate your interest in contributing to this open-source SDK, designed to facilitate the integration of web3 features into Unity-based video games. This guide will provide you with all the essential information you need to make meaningful contributions to our project.

## Getting Started

1. **Fork the Repository**: Start by forking the [web3.unity repository](https://github.com/ChainSafe/web3.unity) on GitHub.
2. **Clone the Fork**: Next, clone your forked repository to your local machine.
3. **Setup Development Environment**: Follow the instructions in the README file to set up your development environment.

## Contributing Code

1. **Create a New Branch**: Create a new branch for your contribution using the command `git checkout -b my-contribution`.
2. **Make Improvements**: Implement the necessary changes and improvements in the codebase.
3. **Adhere to Guidelines**: Ensure that your code follows the project's coding conventions and guidelines.
4. **Write Tests**: Write unit tests for your code and ensure that all existing tests pass.
5. **Commit Changes**: Commit your changes with a descriptive commit message using `git commit -m "Add feature X"`.
6. **Push to Your Fork**: Push your changes to your forked repository with `git push origin my-contribution`.
7. **Submit a Pull Request**: Finally, submit a pull request to the main web3.unity repository.

## Reporting Issues

If you come across any bugs, have suggestions, or want to request new features, please submit an issue on the [GitHub issue tracker](https://github.com/ChainSafe/web3.unity/issues). When submitting an issue, provide comprehensive details, including steps to reproduce the problem and relevant code snippets.

## Collaboration and Communication

We encourage collaboration and discussion among contributors. Join our community on Discord to connect with fellow developers, ask questions, and share ideas. Additionally, we have various communication channels, both synchronous and asynchronous, to coordinate project-related discussions.

## Setting Up the Development Environment

### macOS

1. **Install Xcode**: Get Xcode from the App Store.
2. **Install Homebrew**: Install Homebrew using the following Terminal command:

    ```
    /bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"
    ```

3. **Install Unity**: Install Unity by running this command in the Terminal:

    ```
    brew install --cask unity-hub
    ```

4. **Install Dependencies**: Install the necessary dependencies with this command:

    ```
    brew install --cask dotnet-sdk
    ```

### Linux

1. **Install Unity**: Follow the instructions for your Linux distribution on the Unity website.
2. **Install .NET SDK**: Use the following Terminal command to install the .NET SDK:

    ```
    sudo apt-get install dotnet-sdk-5.0
    ```

### Windows

1. **Download Unity**: Download and install Unity from the Unity website.
2. **Download .NET SDK**: Download and install the .NET SDK from the Microsoft website.

## Dealing with Forks

When contributing to web3.unity, it's advisable to fork the main repository and make your changes in your forked repository. This allows you to work independently on your changes and submit a pull request when you're ready.

To fork the web3.unity repository:

1. Go to the [web3.unity repository](https://github.com/ChainSafe/web3.unity) on GitHub.
2. Click the "Fork" button in the top-right corner of the page.
3. Select your GitHub account as the destination to fork the repository.

## Proposing New Changes

If you have ideas for new features or improvements, follow these steps:

1. **Create an Issue**: Create a new issue on the [GitHub issue tracker](https://github.com/ChainSafe/web3.unity/issues).
2. **Provide Details**: Provide a detailed description of your proposed changes, including any design or implementation considerations.
3. **Engage in Discussion**: Engage with the community and maintainers to refine and validate your proposal.

## Working with the Local Codebase

To work with the local codebase of web3.unity:

1. **Clone Your Fork**: Clone your forked repository to your local machine using the command:

    ```
    git clone <forked-repository-url>
    ```

2. **Open in Unity**: Open the project in Unity by selecting "Open" and navigating to the cloned repository directory.

## Installing Local Copy to Unity

To install a local copy of web3.unity in Unity:

1. Open Unity and create a new project or open an existing one.
2. Import the web3.unity package by selecting "Assets" -> "Import Package" -> "Custom Package" and navigate to the location of the web3.unity package in your local repository.

Thank you for contributing to web3.unity. Together, we'll make it an even better SDK for Unity-based video games, empowering developers to seamlessly integrate web3 features into their projects!
