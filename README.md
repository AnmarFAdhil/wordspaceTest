# Unity 3D Survival Shooter Prototype

## Quick Start
1. Open this project in Unity 2022 LTS or later
2. Go to `Tools > Create Prototype Scene`
3. Press Play

## What's Included
- **Player**: Blue cube at bridge start, auto-fires projectiles forward
- **Bridge Arena**: Long gray platform with rails
- **Enemies**: Red cubes that spawn in waves and move toward player
- **Boss**: Orange cube spawns every 5 waves, much larger
- **UI**: Health, Wave counter, Score display

## Controls
- **WASD**: Move player left/right and forward/back
- **Auto-Fire**: Weapon fires automatically

## Gameplay
- Survive waves of enemies
- Every 5 waves spawns a powerful boss instead
- Higher waves = more enemies
- Boss is freezable (temporary cyan color when frozen)
- Game ends when player health reaches 0

## Architecture
- `Health.cs` - Damage and death system
- `PlayerController.cs` - Movement and input
- `Gun.cs` - Auto-fire mechanism
- `Projectile.cs` - Bullet physics and collision
- `EnemyAI.cs` - Enemy movement and damage
- `FreezeableBoss.cs` - Boss with freeze mechanic
- `WaveSpawner.cs` - Wave progression
- `SimpleGameUI.cs` - HUD updates
- `PrototypeSceneBuilder.cs` - Editor tool for scene auto-generation

All scripts compile and reference each other automatically via the editor builder.