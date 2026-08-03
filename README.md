# Dani's Nightmare (Muck Debug/Cheats Tools)

A powerful, stable, and highly modular in-game debug and cheat command suite for Muck. This mod completely bypasses vanilla limitations to give you absolute control over the game's mechanics, player stats, chest economy, and item spawning.

Developed as a pure runtime Harmony suite, it integrates seamlessly with other mods (like ItemChoice or /give) without breaking the game's native physics or memory layout.

---

## Installation

1. Make sure you have **BepInEx** installed in your Muck directory.
2. Download the mod and extract the `MuckDebugMod.dll` file.
3. Paste the `.dll` file inside your `Muck/BepInEx/plugins/` directory.
4. Launch the game through Steam and enjoy your absolute power.

---

## Available Chat Commands (Cheats Suite)

Simply open the in-game chat and type any of these commands. All commands are processed locally and won't spam your multiplayer lobby.

### Player Stats & Movement
- `/god` - Toggle God Mode (Bypasses all incoming damage, drowning, and fall damage - *yes, I know Muck doesn't have fall damage, but now you are extra safe from it*).
- `/speed <multiplier>` - Set a custom movement speed multiplier (e.g., `/speed 3` or `/speed 0.5` for slow-motion).
- `/stamina` - Toggle Infinite Stamina (Bypasses stamina drain completely for running, swimming, and jumping).
- `/hunger` - Toggle Infinite Hunger (Freezes hunger at 100%).
- `/gold <amount>` - Instantly spawn and add a custom amount of gold coins directly into your inventory.
- `/dmg <multiplier>` - Set a custom outgoing damage multiplier for all weapons and tools (e.g., `/dmg 100`).

### Chests & Loot Manipulation
- `/free` - Toggle free chests. All interactive chest prices are temporarily bypassed to 0, and display a custom "Open me, bitch!" text.
- `/tier <white/blue/orange/reset>` - Force chest drops to a specific rarity tier, or reset to vanilla random drops.
- `/forcerng` - Toggle Force RNG. Guarantees a 100% drop chance for all possible loot table items in their maximum base quantities.
- `/dropmult <multiplier>` - Multiply all resource, mob, and chest drop quantities (e.g., `/dropmult 10` for massive resource piles).
- `/pmult <multiplier>` - Set a custom multiplier for picked-up powerups (e.g., `/pmult 50` to gain 50 stacks of any picked-up powerup).
- `/spawnchest <white/blue/orange>` - Materialize a fully interactive chest of the chosen quality right at your feet.

---

## Developer's Corner

- **AI-Assisted Code:** This mod contains AI-assisted code (yes, I vibe-coded some parts, deal with it).
- **No English, Sorry:** All logs and in-game messages were translated by AI, mostly because I don't speak this shit language.
- **Do Whatever You Want:** Licensed under the MIT License. The source code is hosted on GitHub, so feel free to do whatever the fuck you want with it.
- **Born out of necessity:** I made this mod because I wanted to develop a serious gameplay overhaul, but I needed a proper debugging suite first. Thus, this mod was born. Soon, I will release a content overhaul featuring some... *PERFECTLY* balanced mechanics (wink wink).
- **Written in 6 hours:** If you find a bug, keep in mind I wrote this entire thing in about 6 hours. Before complaining, make sure you didn't do any stupidly extreme shit (like typing `/dmg 99999999999`) or running obviously incompatible mods. If it's actually the mod's fault, feel free to open a GitHub issue. I might fix it... or I might not.
- **The 69 Stack Limit (Not my fault):** The `/dropmult` command has a physical limit built into Muck's core engine. Enemies cannot drop item stacks larger than 69 (except coins). This is NOT a bug in this mod. However, the mod "BetterStackLimits" by "YaBoiAlex" (available on Thunderstore) should fix this. I didn't fix this limit on purpose because doing so could break the game, and I am absolutely not spending 20 hours of my life on that crap.
- **WARNING (No Multiplayer):** This mod is absolutely NOT designed for multiplayer play. Use it online at your own risk.
- **WARNING (No Powerup Over-Scaling):** Obviously, this mod does NOT change how powerups scale natively. The difference between having 300 and 500 Sniper Scopes is null due to the game's mathematical formulas. The mod "MuckConfigurePowerupDrops" by "MichMcb" changes this, but keep in mind that it is NOT compatible with "ItemChoice" (or "/give" commands).
- **CRITICAL EXPLICIT LANGUAGE AND LEXICAL TRANSGRESSION WARNING:** This software product contains highly uncensored, explicit, and potentially hazardous vocabulary that may cause severe moral distress, linguistic contamination, or emotional damage to sensitive individuals, puritans, or Microsoft executives. The development team has conducted a thorough, multi-layered risk assessment regarding the psychological impact of this lexical choice on the end-user. After long, exhausting hours of ethical deliberation, we must warn you: there is exactly ONE (1) highly offensive swear word embedded in the runtime code. Specifically, a chest will tell you *"Open me, bitch!"* when the free-chest hack is enabled. If this single, isolated string of text ruins your day or causes you to call your therapist, we highly suggest uninstalling this mod, turning off your PC, and going outside to touch some fucking grass... bitch.