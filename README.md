# Dani's Nightmare (Muck Debug & Command Suite)

[![BepInEx](https://img.shields.io/badge/BepInEx-5.4.1100-blue.svg)](https://github.com/BepInEx/BepInEx)
[![Game](https://img.shields.io/badge/Game-Muck-orange.svg)](https://store.steampowered.com/app/1625450/Muck/)
[![Version](https://img.shields.io/badge/Version-2.1.2-green.svg)](https://github.com/PlinKuuu/DanisNightmare)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

Take complete control of Muck with an all-in-one debugging and cheat command suite. Spawn any item or enemy, multiply physical powerup drops, freeze time, adjust player stats, or turn on peaceful mode with zero lag. Built with Harmony runtime patches and packed with a full Tab-autocomplete chat console.

> **CRITICAL VERSION 2.1.0 AUTOCOMPLETE UPDATE:**
> This update brings a massive, game-changing upgrade to the chat console's usability. We have developed and implemented a fully **Context-Aware Autocomplete System using the `Tab` key**.
> - Typing `/` and pressing `Tab` automatically suggests all main commands.
> - Typing a main command followed by a space (e.g., `/player `) and pressing `Tab` suggests only relevant sub-commands.
> - Typing `/powerup spawn `, `/powerup give `, `/enemy spawn `, or `/items give ` and pressing `Tab` will **dynamically read Muck's active RAM database to autocomplete the exact names of powerups and items**! No more typing complex names blindly.
> - **Other 2.1.0 Fixes:** We integrated **Free Villager Trades** into the `/free` command ($0 cost), made the `/items give` command automatically default to `1` item if the amount argument is omitted (even with trailing spaces), and patched `/kill` with a safe `/kill help` screen to prevent you from accidentally committing suicide while checking command options (yes, I know you did it, you're welcome).

> **CRITICAL VERSION 2.0.0 OVERHAUL:**
> This update completely rewrote the core command architecture. Old standalone commands (`/pmult`, `/mobmult`, `/pdrop`, `/speed`, `/dmg`) are DEPRECATED. 
> The entire suite has been unified into clean, organized sub-command categories: `/player`, `/enemy`, `/powerup`, and `/items`. Check the updated reference tables below before typing commands blind!

> **Developer Note:**
> Born out of necessity because I wanted to develop a serious gameplay overhaul for Muck, but needed a proper runtime debugging tool first. Entirely vibe-coded in about 10 hours. It works, it fixes vanilla engine quirks, and it gives you absolute power. Deal with it.

> **Future Outlook:**
> This will potentially be the final update for this mod. I am currently rebuilding "ItemChoice" by *ThatWugg* from scratch due to its inherent UI and logic limitations when adding custom powerups. Soon, I will release a modified, fully compatible version of it along with a dedicated helper API library so I can finally begin development on my upcoming, serious content overhaul mod.
---

## In-Game Chat Commands

Open the in-game chat (`Enter`) and type any command. Commands process locally and won't spam multiplayer logs. You can also type `/player help`, `/enemy help`, `/powerup help`, `/items help`, or `/kill help` at any time for quick in-game reference.

### Player & Survival Suite
| Command | Description |
| :--- | :--- |
| `/god` | Toggle God Mode (Invincibility + Auto-activates infinite stamina and hunger). |
| `/player stamina` | Toggle Infinite Stamina (Bypasses drain for sprinting, swimming, and jumping). |
| `/player hunger` | Toggle Infinite Hunger (Freezes hunger stat at 100%). |
| `/player defense <percent>` | Reduce incoming damage by X% (0 to 100% damage reduction). |
| `/player speed <multiplier>` | Set movement speed multiplier with smooth physics dampening. |
| `/player dmg <multiplier>` | Set outgoing damage multiplier for weapons and tools. |

### Enemies & Combat Suite
| Command | Description |
| :--- | :--- |
| `/enemy hp <multiplier>` | Set custom global enemy health scaling multiplier. |
| `/enemy dmg <multiplier>` | Set custom global enemy damage scaling multiplier. |
| `/enemy amount <multiplier>` | Multiply nightly wave spawn sizes & world mob cap (supports decimals like `2.5`). |
| `/enemy spawn <name> [amount]` | Materialize any mob or boss directly in front of you. |
| `/peaceful` | Toggle true Peaceful Mode (Blocks hostile mobs & night bosses; keeps cows & Woodmen). |
| `/kill [target]` | Purge targets (`enemies`, `bosses`, `hostiles`, `peaceful`, `all`, `self`) & drops their real loot tables. |

### Powerups Suite
| Command | Description |
| :--- | :--- |
| `/powerup tier <white/blue/orange/reset>` | Force chest drops to a specific rarity tier or reset to vanilla drops. |
| `/powerup drop <multiplier>` | Multiply physical powerups exploded out of chests, shrines, and bosses on open/kill. |
| `/powerup pickup <multiplier>` | Multiply powerup stack gains when picking up an item from the ground. |
| `/powerup give <name> <amount>` | Inject powerups directly into inventory with native UI and stats sync. |
| `/powerup spawn <name> [amount]` | Materialize physical powerup items on the ground in front of you. |

### Items & Chests Suite
| Command | Description |
| :--- | :--- |
| `/items forcerng` | Toggle Force RNG (Guarantees 100% max drop chance for all loot tables). |
| `/items drop <multiplier>` | Multiply resource, tree, ore, and mob dropped item stack quantities. |
| `/items give <id_or_name> [amount]` | Give any item (Supports full item names with spaces; auto-handles inventory slots). |
| `/free` | Toggle Free Chests & Trades ($0 cost for chests & Villager trades + custom interact text). |
| `/spawnchest <white/blue/orange>` | Materialize a fully interactive chest of chosen quality right at your feet. |

### World & Time Control
| Command | Description |
| :--- | :--- |
| `/setday <number>` | Travel to a specific game day while preserving sky hour & triggering native difficulty. |
| `/setnight <number>` | Travel to the night of a specific game day, force night sky, and trigger night spawns/music natively. |
| `/structmult <multiplier>` | Multiply total spawned world structures (shrines, camps, ruins) for the next run. |

---

## Installation & Mod Compatibility

1. Make sure you have **[BepInEx Pack for Muck](https://thunderstore.io/c/muck/p/BepInEx/BepInExPack_Muck/)** installed.
2. Download the latest release (`DanisNightmare.dll`).
3. Place the `.dll` file inside your `Muck/BepInEx/plugins/` directory.
4. Launch the game through Steam, open the chat (`Enter`), and enjoy your absolute power.

### Mod Compatibility Notes
- **Replaces / Redundant:** This mod makes `giveitems` by *Davidud* obsolete, as Dani's Nightmare provides a superior item injection system (`/items give` supports natural spaces without underscores, Tab autocompletion, optional amounts, and powerup injection).
- **Compatible:** Remains fully compatible with `ItemChoice` by *ThatWugg*.

---

## Technical Engine Quirks & Hardware Warnings

- **Hardware & Performance Warning:**
  Be careful when spawning hundreds of enemies at once (`/enemy spawn` / `/enemy amount`) or setting extreme structure multipliers (`/structmult`). Your poor Intel Celeron might explode trying to handle thousands of active entity physics.
- **The 69 Stack Item Cap:**
  Muck's core engine hardcaps enemy item drops to a maximum of **69 units per stack** (except coins and arrows, which bypass this cap). This is a native engine constraint built into Muck, not a bug in this mod. If you want higher stack limits on drops, install the mod `BetterStackLimits` by *YaBoiAlex*.
- **Powerup Formula Diminishing Returns:**
  Powerup scaling relies on Muck's native mathematical formulas. For certain items (like *Sniper Scope*), having 300 vs 500 stacks yields virtually the same gameplay effect due to built-in mathematical diminishing returns curves.
- **Multiplayer Notice:**
  This mod directly manipulates local client and host memory via runtime Harmony patches. Using this in public multiplayer lobbies is done entirely at your own risk.

---

## Developer's Corner

- **AI-Assisted Code:**
  This mod contains AI-assisted code (yes, I vibe-coded some parts, deal with it).
- **Language & Translation:**
  Log messages and documentation were refined using AI assistance because English is not my primary writing language (I can read it perfectly fine, though).
- **Open Source & License:**
  Licensed under the MIT License. Source code is hosted on GitHub. Do whatever the fuck you want with it.
- **Written in 10 Hours:**
  Keep in mind this entire suite was written in about 10 hours. Before complaining about a bug, make sure you didn't do anything stupidly extreme (like typing `/player dmg 999999999999`) or running broken setups. If you find a legitimate issue, feel free to open a GitHub Issue.

---

## Critical Explicit Language & Lexical Warning

This software product contains highly uncensored, explicit, and potentially hazardous vocabulary that may cause severe moral distress, linguistic contamination, or emotional damage to sensitive individuals, puritans, GNU purists, or Microsoft executives.

After long, exhausting hours of ethical deliberation, we must warn you: there is exactly **ONE (1) highly offensive swear word** embedded in the runtime code. Specifically, a chest will tell you *"Open me, bitch!"* when the free chest hack (`/free`) is enabled.

If this single, isolated string of text ruins your day or causes you to call your therapist, we highly suggest uninstalling this mod, turning off your PC, and going outside to touch some fucking grass... bitch.