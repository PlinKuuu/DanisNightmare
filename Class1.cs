using BepInEx;
using HarmonyLib;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace MuckDebugMod
{
    [BepInPlugin("com.plinkuuu.danisnightmare", "Dani's Nightmare", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        // Debug state variables
        public static bool chestsFree = false;
        public static int forcedTier = 0; // 0 = Normal, 1 = White, 2 = Blue, 3 = Orange
        public static bool infiniteStamina = false;
        public static bool speedHackActive = false;
        public static bool godMode = false;
        public static bool infiniteHunger = false;
        public static int damageMultiplier = 1;
        public static bool forceRng = false;
        public static int dropMultiplier = 1;
        public static int powerupMultiplier = 1;
        public static float speedMultiplier = 1f;

        private void Awake()
        {
            Harmony harmony = new Harmony("com.plinkuuu.danisnightmare");
            harmony.PatchAll();

            MonoBehaviour.print("[DANIS_NIGHTMARE] ==============================================");
            MonoBehaviour.print("[DANIS_NIGHTMARE] DANI'S NIGHTMARE SUCCESSFULLY INITIALIZED!");
            MonoBehaviour.print("[DANIS_NIGHTMARE] ==============================================");
        }

        // Processes local debug chat commands starting with "/"
        public static void ProcessDebugCommand(string message)
        {
            string[] parts = message.Split(' ');
            string baseCommand = parts[0].ToLower();

            MonoBehaviour.print("[DANIS_NIGHTMARE] Chat command detected: " + message);

            // COMMAND 1: /free
            if (baseCommand == "/free")
            {
                chestsFree = !chestsFree;
                string state = chestsFree ? "<color=green>ENABLED<color=white>" : "<color=red>DISABLED<color=white>";
                ChatBox.Instance.SendMessage("[DANIS_NIGHTMARE] Free chests hack: " + state);
                MonoBehaviour.print("[DANIS_NIGHTMARE] Free chests hack set to: " + chestsFree);
                return;
            }

            // COMMAND 2: /tier <white/blue/orange/reset>
            if (baseCommand == "/tier" && parts.Length > 1)
            {
                string argument = parts[1].ToLower();
                if (argument == "white")
                {
                    forcedTier = 1;
                    ChatBox.Instance.SendMessage("[DANIS_NIGHTMARE] Chests forced to rarity: <color=white>COMMON (White)<color=white>");
                }
                else if (argument == "blue")
                {
                    forcedTier = 2;
                    ChatBox.Instance.SendMessage("[DANIS_NIGHTMARE] Chests forced to rarity: <color=cyan>RARE (Blue)<color=white>");
                }
                else if (argument == "orange")
                {
                    forcedTier = 3;
                    ChatBox.Instance.SendMessage("[DANIS_NIGHTMARE] Chests forced to rarity: <color=orange>LEGENDARY (Orange)<color=white>");
                }
                else if (argument == "reset")
                {
                    forcedTier = 0;
                    ChatBox.Instance.SendMessage("[DANIS_NIGHTMARE] Chest rarity restored to normal.");
                }
                MonoBehaviour.print("[DANIS_NIGHTMARE] Chest rarity (Forced Tier) changed to ID: " + forcedTier);
                return;
            }

            // COMMAND 3: /gold <amount>
            if (baseCommand == "/gold" && parts.Length > 1)
            {
                int amount;
                if (int.TryParse(parts[1], out amount))
                {
                    InventoryItem coins = (InventoryItem)ScriptableObject.CreateInstance(typeof(InventoryItem));
                    coins.Copy(ItemManager.Instance.GetItemByName("Coin"), amount);
                    InventoryUI.Instance.AddItemToInventory(coins);
                    ChatBox.Instance.SendMessage("[DANIS_NIGHTMARE] Added <color=yellow>" + amount + " gold<color=white> to your inventory.");
                    MonoBehaviour.print("[DANIS_NIGHTMARE] Spawned " + amount + " physical gold coins in inventory RAM.");
                }
                return;
            }

            // COMMAND 4: /stamina (Infinite Stamina)
            if (baseCommand == "/stamina")
            {
                infiniteStamina = !infiniteStamina;
                string state = infiniteStamina ? "<color=green>ENABLED<color=white>" : "<color=red>DISABLED<color=white>";
                ChatBox.Instance.SendMessage("[DANIS_NIGHTMARE] Infinite Stamina: " + state);
                MonoBehaviour.print("[DANIS_NIGHTMARE] Infinite Stamina set to: " + infiniteStamina);

                if (infiniteStamina && PlayerStatus.Instance != null)
                {
                    PlayerStatus.Instance.stamina = 100f;
                }
                return;
            }

            // COMMAND 5: /speed (Speedhack x3)
            if (baseCommand == "/speed" && parts.Length > 1)
            {
                float amount;
                if (float.TryParse(parts[1], out amount))
                {
                    speedMultiplier = amount;
                    ChatBox.Instance.SendMessage("[DANIS_NIGHTMARE] Speed multiplier set to: <color=orange>x" + amount + "<color=white>");
                    MonoBehaviour.print("[DANIS_NIGHTMARE] Speed multiplier changed to: x" + amount);

                    if (PlayerStatus.Instance != null)
                    {
                        PlayerStatus.Instance.UpdateStats();
                    }
                }
                else
                {
                    ChatBox.Instance.SendMessage("<color=yellow>[DANIS_NIGHTMARE] Correct usage: /speed <multiplier><color=white>");
                }
                return;
            }

            // COMMAND 6: /spawnchest <white/blue/orange>
            if (baseCommand == "/spawnchest" && parts.Length > 1)
            {
                string type = parts[1].ToLower();
                int chosenIndex = -1;

                if (type == "white") chosenIndex = 0;
                else if (type == "blue") chosenIndex = 1;
                else if (type == "orange") chosenIndex = 2;

                if (chosenIndex == -1)
                {
                    ChatBox.Instance.SendMessage("<color=yellow>[DANIS_NIGHTMARE] Correct usage: /spawnchest <white/blue/orange><color=white>");
                    return;
                }

                SpawnPowerupsInLocations[] spawners = Resources.FindObjectsOfTypeAll<SpawnPowerupsInLocations>();
                GameObject chestPrefab = null;

                foreach (SpawnPowerupsInLocations spawner in spawners)
                {
                    if (spawner.powerupChests != null && spawner.powerupChests.Length > chosenIndex)
                    {
                        chestPrefab = spawner.powerupChests[chosenIndex].prefab;
                        break;
                    }
                }

                if (chestPrefab == null)
                {
                    MonoBehaviour.print("[DANIS_NIGHTMARE] ERROR: Chest prefab not found in RAM. Index: " + chosenIndex);
                    return;
                }

                Vector3 playerPos = GameManager.players[LocalClient.instance.myId].transform.position;
                Vector3 lookForward = GameManager.players[LocalClient.instance.myId].transform.forward;

                Vector3 spawnPos = playerPos + (lookForward * 4.5f);
                spawnPos.y = playerPos.y; // Snap to ground

                Quaternion rotation = Quaternion.LookRotation(lookForward);
                GameObject newChest = UnityEngine.Object.Instantiate<GameObject>(chestPrefab, spawnPos, rotation);

                int uniqueId = ResourceManager.Instance.GetNextId();
                LootContainerInteract interactable = newChest.GetComponentInChildren<LootContainerInteract>();

                if (interactable != null)
                {
                    interactable.SetId(uniqueId);
                }

                ResourceManager.Instance.AddObject(uniqueId, newChest);

                ChatBox.Instance.SendMessage("[DANIS_NIGHTMARE] " + type.ToUpper() + " chest materialized at your feet!");
                MonoBehaviour.print("[DANIS_NIGHTMARE] Spawned chest: " + type.ToUpper() + " | Position: " + spawnPos + " | ID: " + uniqueId);
                return;
            }

            // COMMAND 7: /god (God Mode)
            if (baseCommand == "/god")
            {
                godMode = !godMode;
                string state = godMode ? "<color=green>ENABLED<color=white>" : "<color=red>DISABLED<color=white>";
                ChatBox.Instance.SendMessage("[DANIS_NIGHTMARE] God Mode: " + state);
                MonoBehaviour.print("[DANIS_NIGHTMARE] God Mode set to: " + godMode);
                return;
            }

            // COMMAND 8: /hunger (Infinite Hunger)
            if (baseCommand == "/hunger")
            {
                infiniteHunger = !infiniteHunger;
                string state = infiniteHunger ? "<color=green>ENABLED (Hunger Frozen)<color=white>" : "<color=red>DISABLED<color=white>";
                ChatBox.Instance.SendMessage("[DANIS_NIGHTMARE] Infinite Hunger: " + state);
                MonoBehaviour.print("[DANIS_NIGHTMARE] Infinite Hunger set to: " + infiniteHunger);

                if (infiniteHunger && PlayerStatus.Instance != null)
                {
                    PlayerStatus.Instance.hunger = 100f;
                }
                return;
            }

            // COMMAND 9: /dmg <amount>
            if (baseCommand == "/dmg" && parts.Length > 1)
            {
                int multiplier;
                if (int.TryParse(parts[1], out multiplier))
                {
                    damageMultiplier = multiplier;
                    ChatBox.Instance.SendMessage("[DANIS_NIGHTMARE] Damage multiplier set to: <color=orange>x" + multiplier + "<color=white>");
                    MonoBehaviour.print("[DANIS_NIGHTMARE] Damage multiplier changed to: x" + multiplier);
                }
                else
                {
                    ChatBox.Instance.SendMessage("<color=yellow>[DANIS_NIGHTMARE] Correct usage: /dmg <multiplier><color=white>");
                }
                return;
            }

            // COMMAND 10: /forcerng
            if (baseCommand == "/forcerng")
            {
                forceRng = !forceRng;
                string state = forceRng ? "<color=green>ENABLED (100% Max Drop)<color=white>" : "<color=red>DISABLED<color=white>";
                ChatBox.Instance.SendMessage("[DANIS_NIGHTMARE] Force RNG: " + state);
                MonoBehaviour.print("[DANIS_NIGHTMARE] Force RNG (Max Drops) set to: " + forceRng);
                return;
            }

            // COMMAND 11: /dropmult <amount>
            if (baseCommand == "/dropmult" && parts.Length > 1)
            {
                int amount;
                if (int.TryParse(parts[1], out amount))
                {
                    dropMultiplier = amount;
                    ChatBox.Instance.SendMessage("[DANIS_NIGHTMARE] Drop multiplier set to: <color=orange>x" + amount + "<color=white>");
                    MonoBehaviour.print("[DANIS_NIGHTMARE] Drop multiplier changed to: x" + amount);
                }
                else
                {
                    ChatBox.Instance.SendMessage("<color=yellow>[DANIS_NIGHTMARE] Correct usage: /dropmult <multiplier><color=white>");
                }
                return;
            }

            // COMMAND 12: /pmult <amount>
            if (baseCommand == "/pmult" && parts.Length > 1)
            {
                int amount;
                if (int.TryParse(parts[1], out amount))
                {
                    powerupMultiplier = amount;
                    ChatBox.Instance.SendMessage("[DANIS_NIGHTMARE] Powerup pickup multiplier: <color=orange>x" + amount + "<color=white>");
                    MonoBehaviour.print("[DANIS_NIGHTMARE] Powerup pickup multiplier changed to: x" + amount);
                }
                else
                {
                    ChatBox.Instance.SendMessage("<color=yellow>[DANIS_NIGHTMARE] Correct usage: /pmult <multiplier><color=white>");
                }
                return;
            }

            ChatBox.Instance.SendMessage("<color=red>[DANIS_NIGHTMARE] Unknown command. Commands: /free, /tier, /gold, /stamina, /dropmult, /pmult, /forcerng, /speed, /god, /hunger, /dmg, /spawnchest<color=white>");
        }
    }

    // PARCHE 1: Intercept chat messages for "/" commands and format pickup count
    [HarmonyPatch(typeof(ChatBox), "SendMessage", new Type[] { typeof(string) })]
    public class ParcheChatDebug
    {
        static bool Prefix(ref string message)
        {
            if (message.StartsWith("/") && !message.StartsWith("/give"))
            {
                Plugin.ProcessDebugCommand(message);
                return false;
            }
            if (message.StartsWith("Picked up ") && Plugin.powerupMultiplier > 1)
            {
                message = message.Replace("Picked up ", "Picked up " + Plugin.powerupMultiplier + "x ");
            }
            return true;
        }
    }

    // PARCHE 2: Forced drop quality hack
    [HarmonyPatch(typeof(ItemManager), "GetRandomPowerup")]
    public class ParcheSiempreNaranja
    {
        static bool Prefix(ItemManager __instance, ref Powerup __result)
        {
            if (Plugin.forcedTier == 1)
            {
                __result = __instance.powerupsWhite[UnityEngine.Random.Range(0, __instance.powerupsWhite.Length)];
                MonoBehaviour.print("[DANIS_NIGHTMARE] Forced drop intercepted: White");
                return false;
            }
            if (Plugin.forcedTier == 2)
            {
                __result = __instance.powerupsBlue[UnityEngine.Random.Range(0, __instance.powerupsBlue.Length)];
                MonoBehaviour.print("[DANIS_NIGHTMARE] Forced drop intercepted: Blue");
                return false;
            }
            if (Plugin.forcedTier == 3)
            {
                __result = __instance.powerupsOrange[UnityEngine.Random.Range(0, __instance.powerupsOrange.Length)];
                MonoBehaviour.print("[DANIS_NIGHTMARE] Forced drop intercepted: Orange");
                return false;
            }
            return true;
        }
    }

    // PARCHE 3: Chest text display reacts to /free
    [HarmonyPatch(typeof(LootContainerInteract), "GetName")]
    public class ParcheTextoCofre
    {
        static bool Prefix(LootContainerInteract __instance, ref string __result)
        {
            if (Plugin.chestsFree)
            {
                __result = "Open me, bitch!";
                return false;
            }
            return true;
        }
    }

    // PARCHE 4: Intercept chest price and temporarily set to 0 on click
    [HarmonyPatch(typeof(LootContainerInteract), "Interact")]
    public class ParcheInteractCofre
    {
        static void Prefix(LootContainerInteract __instance, ref int __state)
        {
            __state = __instance.price;
            if (Plugin.chestsFree)
            {
                __instance.price = 0;
                MonoBehaviour.print("[DANIS_NIGHTMARE] Chest clicked: Price temporarily forced to 0!");
            }
        }

        static void Postfix(LootContainerInteract __instance, int __state)
        {
            __instance.price = __state;
        }
    }

    // PARCHE 5: Infinite Stamina (Sprinting/Swimming - skips drain logic)
    [HarmonyPatch(typeof(PlayerStatus), "Stamina")]
    public class ParcheEstaminaGasto
    {
        static bool Prefix(PlayerStatus __instance, ref bool ___running, ref bool ___underwater, PlayerMovement ___player, ref float ___staminaRegenRate, ref float ___staminaDrainRate)
        {
            if (Plugin.infiniteStamina)
            {
                ___running = (___player.GetVelocity().magnitude > 5f && ___player.sprinting);
                ___underwater = ___player.IsUnderWater();

                if (!___running && !___underwater)
                {
                    if (__instance.stamina < 100f && ___player.grounded && __instance.hunger > 0f)
                    {
                        float num = 1f;
                        if (__instance.hunger <= 0f)
                        {
                            num *= 0.3f;
                        }
                        __instance.stamina += ___staminaRegenRate * Time.deltaTime * num;
                    }
                    return false;
                }
                if (__instance.stamina <= 0f)
                {
                    return false;
                }

                // Skip original subtraction
                return false;
            }
            return true;
        }
    }

    // PARCHE 6: Infinite Stamina (Jumping - skips jump drain)
    [HarmonyPatch(typeof(PlayerStatus), "Jump")]
    public class ParcheEstaminaSalto
    {
        static bool Prefix()
        {
            if (Plugin.infiniteStamina)
            {
                MonoBehaviour.print("[DANIS_NIGHTMARE] Jump detected: Stamina drain bypassed!");
                return false;
            }
            return true;
        }
    }

    // PARCHE 7: Stable customizable speedhack
    [HarmonyPatch(typeof(PlayerMovement), "Movement")]
    public class ParcheVelocidadReal
    {
        static void Prefix(PlayerMovement __instance, ref float ___moveSpeed, ref float ___maxSpeed)
        {
            ___moveSpeed = 3500f * Plugin.speedMultiplier;
            ___maxSpeed = 6.5f * Plugin.speedMultiplier;
        }
    }

    // PARCHE 8: God Mode (Skips raw damage processing)
    [HarmonyPatch(typeof(PlayerStatus), "HandleDamage")]
    public class ParcheModoDios
    {
        static bool Prefix()
        {
            if (Plugin.godMode)
            {
                return false;
            }
            return true;
        }
    }

    // PARCHE 9: Infinite Hunger (Skips hunger drain)
    [HarmonyPatch(typeof(PlayerStatus), "Hunger")]
    public class ParcheHambreInfinita
    {
        static bool Prefix()
        {
            if (Plugin.infiniteHunger)
            {
                return false;
            }
            return true;
        }
    }

    // PARCHE 10: Force 100% Drop (Overload 1)
    [HarmonyPatch(typeof(LootDrop), "GetLoot", new Type[] { })]
    public class ParcheRngForce1
    {
        static bool Prefix(LootDrop __instance, ref List<InventoryItem> __result)
        {
            if (Plugin.forceRng)
            {
                List<InventoryItem> list = new List<InventoryItem>();
                if (__instance.loot != null)
                {
                    foreach (LootDrop.LootItems lootItems in __instance.loot)
                    {
                        if (lootItems != null && lootItems.item != null)
                        {
                            int maxAmount = lootItems.amountMax;
                            InventoryItem inventoryItem = (InventoryItem)ScriptableObject.CreateInstance(typeof(InventoryItem));
                            inventoryItem.Copy(lootItems.item, maxAmount);
                            list.Add(inventoryItem);
                        }
                    }
                }
                __result = list;
                MonoBehaviour.print("[DANIS_NIGHTMARE] LootDrop ID " + __instance.id + " forced to 100% max drop!");
                return false;
            }
            return true;
        }
    }

    // PARCHE 11: Force 100% Drop (Overload 2 - ConsistentRandom)
    [HarmonyPatch(typeof(LootDrop), "GetLoot", new Type[] { typeof(ConsistentRandom) })]
    public class ParcheRngForce2
    {
        static bool Prefix(LootDrop __instance, ref List<InventoryItem> __result)
        {
            if (Plugin.forceRng)
            {
                List<InventoryItem> list = new List<InventoryItem>();
                if (__instance.loot != null)
                {
                    foreach (LootDrop.LootItems lootItems in __instance.loot)
                    {
                        if (lootItems != null && lootItems.item != null)
                        {
                            int maxAmount = lootItems.amountMax;
                            InventoryItem inventoryItem = (InventoryItem)ScriptableObject.CreateInstance(typeof(InventoryItem));
                            inventoryItem.Copy(lootItems.item, maxAmount);
                            list.Add(inventoryItem);
                        }
                    }
                }
                __result = list;
                MonoBehaviour.print("[DANIS_NIGHTMARE] LootDrop (Random) ID " + __instance.id + " forced to 100% max drop!");
                return false;
            }
            return true;
        }
    }

    // PARCHE 12: Drop quantity multiplier (Overload 1)
    [HarmonyPatch(typeof(LootDrop), "GetLoot", new Type[] { })]
    public class ParcheDropMultiplier1
    {
        static void Postfix(ref List<InventoryItem> __result)
        {
            if (Plugin.dropMultiplier > 1 && __result != null)
            {
                foreach (InventoryItem item in __result)
                {
                    if (item != null)
                    {
                        item.amount *= Plugin.dropMultiplier;
                    }
                }
                MonoBehaviour.print("[DANIS_NIGHTMARE] Drop amounts multiplied by: x" + Plugin.dropMultiplier);
            }
        }
    }

    // PARCHE 13: Drop quantity multiplier (Overload 2 - ConsistentRandom)
    [HarmonyPatch(typeof(LootDrop), "GetLoot", new Type[] { typeof(ConsistentRandom) })]
    public class ParcheDropMultiplier2
    {
        static void Postfix(ref List<InventoryItem> __result)
        {
            if (Plugin.dropMultiplier > 1 && __result != null)
            {
                foreach (InventoryItem item in __result)
                {
                    if (item != null)
                    {
                        item.amount *= Plugin.dropMultiplier;
                    }
                }
                MonoBehaviour.print("[DANIS_NIGHTMARE] Drop amounts (Random) multiplied by: x" + Plugin.dropMultiplier);
            }
        }
    }

    // PARCHE 14: Modify picked up powerup count in RAM
    [HarmonyPatch(typeof(PowerupInventory), "AddPowerup")]
    public class ParcheCantidadPowerups
    {
        static bool Prefix(ref int[] ___powerups, int powerupId)
        {
            if (Plugin.powerupMultiplier > 1)
            {
                ___powerups[powerupId] += (Plugin.powerupMultiplier - 1);
            }
            return true;
        }
    }

    // PARCHE 15: Sync powerup icon count in UI
    [HarmonyPatch(typeof(PowerupUI), "AddPowerup")]
    public class ParcheUI_PowerupAmount
    {
        static void Postfix(int powerupId, Dictionary<int, GameObject> ___powerups)
        {
            if (___powerups.ContainsKey(powerupId) && PowerupInventory.Instance != null)
            {
                string powerupName = ItemManager.Instance.allPowerups[powerupId].name;
                int realAmount = PowerupInventory.Instance.GetAmount(powerupName);

                TMPro.TextMeshProUGUI iconText = ___powerups[powerupId].GetComponentInChildren<TMPro.TextMeshProUGUI>();
                if (iconText != null)
                {
                    iconText.text = realAmount.ToString();
                }
            }
        }
    }

    // PARCHE 16: Outgoing damage multiplier
    [HarmonyPatch(typeof(PowerupInventory), "GetStrengthMultiplier")]
    public class ParcheDanoMultiplicador
    {
        static void Postfix(ref float __result)
        {
            __result *= (float)Plugin.damageMultiplier;
        }
    }
}