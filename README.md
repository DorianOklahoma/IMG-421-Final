# IMG-421-Final: Skies of Ember

A 2D side-scrolling action RPG built in Unity. You play as Ember, a princess sent into the Skyheart Vault — a vast, ever-shifting skybound dungeon — to complete a sacred trial that will determine her worth as a future ruler.

---

## Story

Ember has never been seen as the obvious successor. Too soft, too gentle, too compassionate to lead — or so the court whispered. When the time comes for the sacred trial, it is not presented as an honor. It is a test. A radiant gem is placed in her hands with a quiet expectation: place it at the heart of the Skyheart Vault, or prove the doubts right.

The Vault is not merely a place of danger. It is alive, shaped by memory and emotion, and it does not yield easily. But what no one accounted for is this: Ember has spent her entire life being underestimated.

---

## Gameplay

Skies of Ember is a dungeon-crawling platformer. Players traverse interconnected skybound levels filled with enemies, environmental puzzles, and boss encounters. Weapons can be picked up and dropped throughout each level, encouraging players to adapt their playstyle on the fly.

- Progress through levels by solving puzzles and defeating the final boss of each area
- Death sends the player back to the start of the current level
- Earn currency from defeated enemies and spend it at Nimbus's shop
- Discover optional rooms and secret areas with rewards, traps, or secrets

---

## Characters

**Ember** — The protagonist. A princess whose quiet determination and empathy prove more powerful than anyone expected. Her special ability, Kindle Flame, releases a short-range burst of fire that damages enemies and can ignite torches, mechanisms, and hidden triggers in the environment.

**The Queen (Ember's Mother)** — Ember's measuring stick. She completed the Skyheart trial herself and emerged believing that ruling requires sacrifice and emotional restraint. Her high expectations are the source of much of Ember's self-doubt.

**The King (Ember's Father)** — Ember's quiet supporter. Kind, patient, and emotionally open, he believes in Ember's potential even when others don't. He represents compassion as a form of strength.

**Shade (Ember's Shadow)** — A manifestation of Ember's insecurities given form by the Vault itself. She mirrors Ember's appearance but appears as a dark, ethereal silhouette. She does not threaten — she undermines, voicing the exact doubts Ember tries to suppress. As Ember grows stronger, so does her ability to confront Shade.

**Nimbus (The Cloud Shopkeep)** — A chaotic, cloud-like merchant who somehow exists inside the Skyheart Vault. Nobody knows how they got there. They don't seem concerned about it. Provides levity, items, and the occasional cryptic hint.

---

## Controls

| Action | Key / Button |
|---|---|
| Move | A / D |
| Jump | W |
| Interact / Pick Up | E |
| Primary Attack | Left Mouse Button |
| Secondary Attack | Right Mouse Button (hold to draw bow) |
| Drop Weapon | Q |

The player aims with the mouse — the weapon rotates to follow the cursor direction in world space.

---

## Scenes

**MainMenu** — The opening screen. Features a procedurally generated sunset sky with drifting clouds. Contains Play and Quit buttons wired to `MainMenu.cs`. Loads first in the build (index 0).

**Level1** — The main gameplay scene.

---

## Weapons

All weapons extend the base `Weapon` class and share a cooldown system. Weapons are picked up from the world and dropped with Q. The default weapon is never dropped.

**Fire Sword** — Melee. Primary slashes all enemies inside the hit collider. Uses `SwordAnimator` controller with `idle` and `slash` states.

**Fireball** — Ranged. Primary throws a fireball in the aim direction. Spawns a `FireballProjectile` that damages the first enemy it hits and plays an impact animation on contact. Uses `FireballAnimator` controller with `hover`, `throw`, and `impact` states driven by triggers `throw` and `impact`.

**Bow** — Ranged. Primary fires a quick arrow at base damage. Secondary hold-and-release draws the bow — the longer you hold (up to `maxDrawTime`), the more damage the arrow deals (up to 2.5× at full draw). Releasing too quickly cancels the shot. Uses `BowAnimator` controller with `idle`, `quickShot`, `drawing`, and `release` states driven by triggers `quickShot`, `draw`, `release`, `idle` and float `drawProgress`.

**Fists** — The fallback unarmed weapon.

---

## Enemies

**Shattered** — A grounded melee enemy. Chases the player and attacks at close range. Uses `ShatteredAnimator` controller.

**Skywisp** — A flying enemy that drifts through the air toward the player. Uses `SkywispAnimator` controller.

Both enemy types use an idle → chase state machine and extend the base `Enemy` class. Enemies are on the **Objects** layer; projectiles are on the **Default** layer. The Physics Layer Collision Matrix must have Objects ↔ Default enabled for projectile damage to register.

---

## Architecture

```
Character (abstract)
├── Player
└── Enemy (abstract)
    ├── GroundedEnemy
    │   └── Shattered
    └── FlyingEnemy
        └── Skywisp

Weapon (abstract)
├── Fists
├── Sword
├── Fireball
│   └── [spawns] FireballProjectile
└── Bow
    └── [spawns] ArrowProjectile

WeaponController   — manages equip/drop, routes input, rotates weapon point
Object             — base class for interactable world objects
```

### Scripts

| Script | Purpose |
|---|---|
| `Character.cs` | Base class for all living entities. Health, death, animation, facing. |
| `Player.cs` | Player movement, mouse-aim direction, interaction raycasting. |
| `Enemy.cs` | Idle/chase state machine, attack routing. |
| `GroundedEnemy.cs` | Ground-based enemy locomotion. |
| `FlyingEnemy.cs` | Aerial enemy locomotion. |
| `Shattered.cs` | Grounded melee enemy. |
| `Skywisp.cs` | Flying enemy. |
| `WeaponController.cs` | Equip/drop logic, player and enemy input routing, weapon-point rotation. |
| `Weapon.cs` | Abstract base for all weapons. Cooldown system, equip/drop callbacks. |
| `Sword.cs` | Melee weapon using trigger colliders. |
| `Fireball.cs` | Ranged weapon; throws a `FireballProjectile` on primary attack. |
| `FireballProjectile.cs` | Travelling fireball. Damages first enemy hit, plays impact animation. |
| `Bow.cs` | Ranged weapon; quick-shot primary, draw-and-release secondary. |
| `ArrowProjectile.cs` | Arrow projectile. Sticks on impact. |
| `Fists.cs` | Fallback unarmed weapon. |
| `Object.cs` | Interactable world object base. |
| `HazardSpike.cs` | Environmental spike hazard. |
| `Trapdoor.cs` | Trapdoor mechanic. |
| `TriggerArea.cs` | Generic trigger zone. |
| `FollowCamera.cs` | Camera that follows the player. |
| `MainMenu.cs` | Start/quit logic for the main menu scene. |
| `CloudSpawner.cs` | Procedurally spawns and drifts clouds across the main menu background. |

---

## Animation Controllers

| Controller | Used By | States |
|---|---|---|
| `PlayerAnimator` | Ember (Player) | idle, walk, jump, death, hit, special |
| `ShatteredAnimator` | Shattered (Enemy) | idle, attack, death, dodge, hit |
| `SkywispAnimator` | Skywisp (Enemy) | float, drift, attack, death, hit |
| `SwordAnimator` | Fire Sword | idle, slash |
| `FireballAnimator` | Fireball weapon + FireballProjectile | hover, throw, impact |
| `BowAnimator` | Bow weapon | idle, quickShot, drawing, release |

---

## Project Structure

```
Assets/
├── Animations/
│   ├── AnimationControllers/
│   │   ├── PlayerAnimator.controller
│   │   ├── ShatteredAnimator.controller
│   │   ├── SkywispAnimator.controller
│   │   ├── SwordAnimator.controller
│   │   ├── FireballAnimator.controller
│   │   └── BowAnimator.controller
│   └── Animations/
├── Prefabs/
│   ├── Fireball.prefab
│   ├── FireballProjectile.prefab
│   ├── Bow.prefab
│   └── ArrowProjectile.prefab
├── Scenes/
│   ├── MainMenu.unity
│   └── Level1.unity
├── Scripts/
│   ├── Character.cs
│   ├── Player.cs
│   ├── Enemy.cs
│   ├── GroundedEnemy.cs
│   ├── FlyingEnemy.cs
│   ├── Shattered.cs
│   ├── Skywisp.cs
│   ├── WeaponController.cs
│   ├── Weapon.cs
│   ├── Fists.cs
│   ├── Sword.cs
│   ├── Fireball.cs
│   ├── FireballProjectile.cs
│   ├── Bow.cs
│   ├── ArrowProjectile.cs
│   ├── Object.cs
│   ├── HazardSpike.cs
│   ├── Trapdoor.cs
│   ├── TriggerArea.cs
│   ├── FollowCamera.cs
│   ├── MainMenu.cs
│   └── CloudSpawner.cs
└── ...
```

---

## Built With

- [Unity](https://unity.com/) — Game engine (3D project, 2D gameplay plane)
- [Aseprite](https://www.aseprite.org/) — Pixel art and animation
- C# — Scripting
- GitHub — Version control

---

## Team

Alexandra Curry, Hilbert Lee, Dorian Sanchez

*IMG-421 Final Project*
