
# TCS Dependency Injection for Unity

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Build Status](https://img.shields.io/badge/build-passing-brightgreen)](https://github.com/Ddemon26)
[![Unity Version](https://img.shields.io/badge/unity-2021.1+-blue.svg)](https://unity.com/)

## Overview

This repository implements a lightweight **Dependency Injection (DI) framework** designed for Unity, inspired by the popular DI containers used in larger .NET applications. It simplifies the management of dependencies between different components in a Unity project by allowing you to register, resolve, and inject services easily.

### Key Features
- **Service Collection & Provider**: A flexible `ServiceCollection` and `ServiceProvider` architecture that enables the registration and resolution of services.
- **Custom Attributes**: Simplify the injection process using attributes and extensions.
- **Extensible**: Easily extendable to support more complex scenarios and service lifetimes.
- **Unity-Compatible**: Designed with Unity's lifecycle in mind, ensuring that services can be injected and managed efficiently within a Unity project.

## Installation

To install this dependency injection system into your Unity project:

1. Clone or download this repository.
2. Copy the `DependencyInjection-Net` folder into your Unity project's `Assets` directory.
3. Add the necessary namespaces in your scripts to start using the DI system:
    ```csharp
    using TCS.DependencyInjection;
    ```

## How It Works

### 1. Register Services
In the `GameManager.cs` example provided, services are registered using a `ServiceCollection`. Services can be registered as singletons or with different lifetimes.

```csharp
void Awake() {
    IServiceCollection services = new ServiceCollection();
    
    // Register a service (Singleton in this case)
    services.AddSingleton<IWeapon, Sword>();

    // Build the service provider
    ServiceProvider serviceProvider = services.BuildServiceProvider();
}
```

### 2. Resolve Services
Once services are registered, you can resolve them through the `ServiceProvider`:

```csharp
IWeapon weapon = serviceProvider.GetService<IWeapon>();
weapon.Use();
```

### Example
In the `Tests/` folder, there is an example where a `Player` depends on an `IWeapon` service, which is injected at runtime:

- **IWeapon.cs**: Interface for the weapon.
- **Sword.cs**: Concrete implementation of the `IWeapon` interface.
- **Player.cs**: The `Player` class that requires an `IWeapon` to function.

```csharp
public class Player {
    private readonly IWeapon _weapon;

    public Player(IWeapon weapon) {
        _weapon = weapon;
    }

    public void Attack() {
        _weapon.Use();
    }
}
```

In the `GameManager`, the dependency injection framework automatically injects the `Sword` into the `Player` class:

```csharp
Player player = new Player(serviceProvider.GetService<IWeapon>());
player.Attack();
```

## Service Lifetimes

The framework supports different lifetimes for services:
- **Singleton**: A single instance is created and shared across the entire application.
- **Scoped** (future support): Instances are shared within specific scopes.
- **Transient** (future support): A new instance is created every time the service is requested.

## Contributing

Contributions are welcome! If you'd like to add features or report issues, feel free to create a pull request or open an issue on the repository.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.
