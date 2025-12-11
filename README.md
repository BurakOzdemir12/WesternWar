# Mobile Game- WesternWar
The development process continues. You can look for a demo link below
https://brk12.itch.io/western-war

## Design Patterns & Architecture Used

- Singleton Pattern

I implemented single-instance management with static instance fields in several core classes.

Usage:

GameManager: Global access to manage game states.

CrowdSystem: Global access to manage runners.

EnemyCounter: Global access for enemy count tracking.

PlayerWeaponController: Global access for the main player’s weapon (Note: not recommended as singleton for all runners, only the main player).

- Observer/Event Pattern

I used events to broadcast in-game actions (death, bonus, weapon change) to other systems.

Usage:

GameManager.OnGameStateChanged

PlayerHealth.OnAnyPlayerDeath, PlayerHealth.OnPlayerDeath

EnemyHealth.OnAnyDeath

Barrel.OnBreakBarrelMan, Barrel.OnBreakBarrelWeapon

CrowdSystem.OnRunnersChanged, CrowdSystem.OnWeaponChanged

This allows loosely coupled communication between systems.

- Component-Based Architecture (Unity Default)

Each functionality is implemented as a separate component.

Usage: Player (PlayerHealth, PlayerWeaponController, PlayerAnimator), Enemies (EnemyHealth), environment objects, etc.

- Coroutine (Asynchronous Programming)

Used for delayed and asynchronous operations.

Usage:

CrowdSystem.RemoveRunners, AnimationDelay, DelayedDestroy

EnemyHealth.HandleDeath

EnemySpawner.SpawnEnemiesCoroutine

PlayerHealth.SpawnFx

- ScriptableObject (Data-Driven Design)

I used ScriptableObjects to separate data from logic.

Usage:

WeaponsSo, PlayerSo, EnemySo for centralized, easily editable data for weapons, players, and enemies.

- Event Subscription/Unsubscription (Lifecycle Management)

Subscribing and unsubscribing to events inside OnEnable and OnDisable for proper memory and lifecycle management.

Usage: All components listening to events follow this pattern.

- MVC-like UI Management

UI logic is separated from game logic.

Usage: UIManager handles menu, in-game HUD, game-over, and win screens.

- Factory Pattern (Partial)

Used for spawning new objects via prefab instantiation.

Usage:

CrowdSystem.SpawnRunners

EnemySpawner.SpawnEnemiesCoroutine

## Summary

In my project, I actively applied Singleton, Observer/Event, Component-Based Architecture, Coroutine, ScriptableObject (Data-Driven), Factory (partial), and Unity’s default architectural patterns. Each pattern is used where it fits best to ensure scalability, modularity, and clean architecture.

