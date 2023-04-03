using BepInEx;
using BepInEx.Configuration;
using Jotunn.Managers;
using Jotunn.Utils;
using System;

namespace PaddedArmourRequiresBlackmetal
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Patch)]
    internal class PaddedArmourRequiresBlackmetal : BaseUnityPlugin
    {
        public const string PluginGUID = "OhhLoz-PaddedArmorRequiresBlackmetal";
        public const string PluginName = "PaddedArmorRequiresBlackmetal";
        public const string PluginVersion = "1.0.3";
        private ConfigEntry<float> PercentageBlackmetalCraft;
        private ConfigEntry<float> PercentageBlackmetalUpgrade;

        private void Awake()
        {
            ItemManager.OnItemsRegistered += OnItemsRegistered;
            CreateConfigValues();
        }

        private void OnItemsRegistered()
        {
            try
            {
                ChangeRecipes();
            }
            catch (Exception e)
            {
                Jotunn.Logger.LogInfo($"Error OnItemsRegistered : {e.Message}");
            }
            finally
            {
                PrefabManager.OnPrefabsRegistered -= OnItemsRegistered;
            }
        }

        private void ChangeRecipes()
        {
            foreach (Recipe fetchedRecipe in ObjectDB.instance.m_recipes)
            {
                if (fetchedRecipe.name == "Recipe_ArmorPaddedCuirass")
                {
                    fetchedRecipe.m_resources = new Piece.Requirement[]
                    {
                        new Piece.Requirement() { m_resItem = PrefabManager.Cache.GetPrefab<ItemDrop>("Iron"), m_amount = (int) (10 * (1f-PercentageBlackmetalCraft.Value)), m_amountPerLevel = (int) (3 * (1f-PercentageBlackmetalUpgrade.Value))  },
                        new Piece.Requirement() { m_resItem = PrefabManager.Cache.GetPrefab<ItemDrop>("BlackMetal"), m_amount = (int) (10 * PercentageBlackmetalCraft.Value), m_amountPerLevel = (int) (3 * PercentageBlackmetalUpgrade.Value)},
                        new Piece.Requirement() { m_resItem = PrefabManager.Cache.GetPrefab<ItemDrop>("LinenThread"), m_amount = 20, m_amountPerLevel = 10 }
                    };
                }
                else if (fetchedRecipe.name == "Recipe_ArmorPaddedGreaves")
                {
                    fetchedRecipe.m_resources = new Piece.Requirement[]
                    {
                        new Piece.Requirement() { m_resItem = PrefabManager.Cache.GetPrefab<ItemDrop>("Iron"), m_amount = (int) (10 * (1f-PercentageBlackmetalCraft.Value)), m_amountPerLevel = (int) (3 * (1f-PercentageBlackmetalUpgrade.Value))  },
                        new Piece.Requirement() { m_resItem = PrefabManager.Cache.GetPrefab<ItemDrop>("BlackMetal"), m_amount = (int) (10 * PercentageBlackmetalCraft.Value), m_amountPerLevel = (int) (3 * PercentageBlackmetalUpgrade.Value)},
                        new Piece.Requirement() { m_resItem = PrefabManager.Cache.GetPrefab<ItemDrop>("LinenThread"), m_amount = 20, m_amountPerLevel = 10  }
                    };
                }
                else if (fetchedRecipe.name == "Recipe_HelmetPadded")
                {
                    fetchedRecipe.m_resources = new Piece.Requirement[]
                    {
                        new Piece.Requirement() { m_resItem = PrefabManager.Cache.GetPrefab<ItemDrop>("Iron"), m_amount = (int) (10 * (1f-PercentageBlackmetalCraft.Value)), m_amountPerLevel = (int) (5 * (1f-PercentageBlackmetalUpgrade.Value))  },
                        new Piece.Requirement() { m_resItem = PrefabManager.Cache.GetPrefab<ItemDrop>("BlackMetal"), m_amount = (int) (10 * PercentageBlackmetalCraft.Value), m_amountPerLevel = (int) (5 * PercentageBlackmetalUpgrade.Value)},
                        new Piece.Requirement() { m_resItem = PrefabManager.Cache.GetPrefab<ItemDrop>("LinenThread"), m_amount = 15, m_amountPerLevel = 10  }
                    };
                }
            }
        }

        private void CreateConfigValues()
        {
            Config.SaveOnConfigSet = true;

            PercentageBlackmetalCraft = Config.Bind("Client config", "BlackmetalCraft", 0.5f,
                new ConfigDescription("Percentage of crafting recipe that will be Blackmetal", new AcceptableValueRange<float>(0f, 1f)));

            PercentageBlackmetalUpgrade = Config.Bind("Client config", "BlackmetalUpgrade", 0.66f,
                new ConfigDescription("Percentage of upgrade recipe that will be Blackmetal", new AcceptableValueRange<float>(0f, 1f)));
        }

    }
}