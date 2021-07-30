using System.Collections.Generic;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using UnityEngine;
using System;

namespace BuildIt
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [NetworkCompatibility(CompatibilityLevel.OnlySyncWhenInstalled, VersionStrictness.Major)]
    internal class BuildIt : BaseUnityPlugin
    {
        public const string PluginGUID = "RockerKitten.BuildIt";
        public const string PluginName = "BuildIt";
        public const string PluginVersion = "1.3.1";
        public AssetBundle assetBundle;
        public EffectList buildStone;
        //public EffectList cookingSound;
        public EffectList breakStone;
        public EffectList hitStone;
        public EffectList breakWood;
        public EffectList hitWood;
        public EffectList buildMetal;
        public EffectList breakMetal;
        public EffectList hitMetal;
        public EffectList hearthAddFuel;
        public EffectList fireAddFuel;
        public EffectList buildRug;
        public EffectList doorOpen;
        public EffectList doorClose;
        ///public EffectList logHit;
        ///public EffectList logDestroy;
        public EffectList buildWood;
        public AudioSource fireVol;
        //public AudioSource fire2Vol;
        public GameObject repairFab;

        public CustomPieceTable improvedHammer;
        //public static ConfigEntry<float> placementOffsetIncrementConfig;
        //public static ConfigEntry<bool> placementOffsetEnabledConfig;
        public static ConfigEntry<bool> hidePlaceMarkerConfig;

        private void Awake()
        {
            Patches.SetupPlacementHooks();
            SetupConfig();
            LoadAssets();
            LoadHammerTable();
            ItemManager.OnVanillaItemsAvailable += LoadSounds;
            
        }

        private void SetupConfig()
        {
           /* placementOffsetIncrementConfig = Config.Bind(
                "Placement", "Placement change increment", 0.01f,
                new ConfigDescription("Placement change when holding Ctrl and/or Alt while scrolling using the Improved Hammer"));

            placementOffsetEnabledConfig = Config.Bind(
                 "Placement", "Enable placement change with Ctrl + Alt", true,
                 new ConfigDescription("Enable placement change when holding Ctrl and/or Alt while scrolling using the Improved Hammer"));*/
           
            hidePlaceMarkerConfig = Config.Bind(
                 "Placement", "Hide placement marker", true,
                 new ConfigDescription("Hide the yellow placement marker while using the Improved Hammer"));

        }

        public void LoadAssets()
        {
            assetBundle = AssetUtils.LoadAssetBundleFromResources("buildit", Assembly.GetExecutingAssembly());
        }
        private void LoadSounds()
        {
            //try
            //{
                var sfxstonebuild = PrefabManager.Cache.GetPrefab<GameObject>("sfx_build_hammer_stone");
                var vfxstonebuild = PrefabManager.Cache.GetPrefab<GameObject>("vfx_Place_stone_wall_2x1");
                var sfxwoodbuild = PrefabManager.Cache.GetPrefab<GameObject>("sfx_build_hammer_wood");
                var sfxbreakstone = PrefabManager.Cache.GetPrefab<GameObject>("sfx_rock_destroyed");
                var sfxwoodbreak = PrefabManager.Cache.GetPrefab<GameObject>("sfx_wood_break");
                var sfxmetalbuild = PrefabManager.Cache.GetPrefab<GameObject>("sfx_build_hammer_metal");
                var vfxmetalhit = PrefabManager.Cache.GetPrefab<GameObject>("vfx_HitSparks");
                var vfxadd = PrefabManager.Cache.GetPrefab<GameObject>("vfx_FireAddFuel");
                var sfxadd = PrefabManager.Cache.GetPrefab<GameObject>("sfx_FireAddFuel");
                var sfxstonehit = PrefabManager.Cache.GetPrefab<GameObject>("sfx_Rock_Hit");
                var vfxaddfuel = PrefabManager.Cache.GetPrefab<GameObject>("vfx_HearthAddFuel");
                var chestopen = PrefabManager.Cache.GetPrefab<GameObject>("sfx_chest_open");
                var sfxtreefall = PrefabManager.Cache.GetPrefab<GameObject>("sfx_tree_fall_hit");
                var vfxtreefallhit = PrefabManager.Cache.GetPrefab<GameObject>("vfx_tree_fall_hit");
                var sfxtreehit = PrefabManager.Cache.GetPrefab<GameObject>("sfx_tree_hit");
                var vfxbirch = PrefabManager.Cache.GetPrefab<GameObject>("vfx_birch1_cut");
                var sfxtfall = PrefabManager.Cache.GetPrefab<GameObject>("sfx_tree_fall");
                var vfxwoodhit = PrefabManager.Cache.GetPrefab<GameObject>("vfx_SawDust");
                var vfxdestroyloghalf = PrefabManager.Cache.GetPrefab<GameObject>("vfx_firlogdestroyed_half");
                var sfxbuildrug = PrefabManager.Cache.GetPrefab<GameObject>("sfx_build_hammer_default");
                var dooropensfs = PrefabManager.Cache.GetPrefab<GameObject>("sfx_door_open");
                var doorclosesfx = PrefabManager.Cache.GetPrefab<GameObject>("sfx_door_close");
                //repairFab = PrefabManager.Cache.GetPrefab<GameObject>("piece_repair");


                buildStone = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxstonebuild }, new EffectList.EffectData { m_prefab = vfxstonebuild } } };
                breakStone = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxbreakstone }, new EffectList.EffectData { m_prefab = vfxwoodhit } } };
                hitStone = new EffectList { m_effectPrefabs = new EffectList.EffectData[1] { new EffectList.EffectData { m_prefab = sfxstonehit } } };
                buildWood = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxwoodbuild }, new EffectList.EffectData { m_prefab = vfxstonebuild } } };
                breakWood = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxwoodbreak }, new EffectList.EffectData { m_prefab = vfxwoodhit } } };
                hitWood = new EffectList { m_effectPrefabs = new EffectList.EffectData[1] { new EffectList.EffectData { m_prefab = vfxwoodhit} } };
                buildMetal = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxmetalbuild }, new EffectList.EffectData {m_prefab =  vfxstonebuild} } };
                breakMetal = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxbreakstone }, new EffectList.EffectData { m_prefab = vfxmetalhit } } };
                hitMetal = new EffectList { m_effectPrefabs = new EffectList.EffectData[1] { new EffectList.EffectData { m_prefab = vfxmetalhit } } };
                hearthAddFuel = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = vfxaddfuel }, new EffectList.EffectData { m_prefab = sfxadd } } };
                fireAddFuel = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = vfxadd }, new EffectList.EffectData { m_prefab = sfxadd } } };
                buildRug = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxbuildrug }, new EffectList.EffectData { m_prefab = vfxstonebuild } } };
                doorOpen = new EffectList { m_effectPrefabs = new EffectList.EffectData[1] { new EffectList.EffectData { m_prefab = dooropensfs } } };
                doorClose = new EffectList { m_effectPrefabs = new EffectList.EffectData[1] { new EffectList.EffectData { m_prefab = doorclosesfx } } };

                

                //Repair();
                LoadBuild76();
                LoadBuild77();
                LoadBuild142();
                LoadBuild70();
                LoadBuild75();
                LoadBuild52();
                LoadBuild53();
                LoadBuild71();
                LoadBuild73();
                LoadBuild59();
                LoadBuild60();
                LoadBuild72();
                LoadBuild74();
                LoadBuild120();
                LoadBuild126();
                LoadBuild121();
                LoadBuild127();
                LoadBuild85();
                LoadBuild86();
                LoadBuild141();
                LoadBuild78();
                LoadBuild84();
                LoadBuild2();
                LoadBuild1();
                LoadBuild79();
                LoadBuild82();
                LoadBuild4();
                LoadBuild3();
                LoadBuild81();
                LoadBuild83();
                LoadBuild94();
                LoadBuild95();
                LoadBuild48();
                LoadBuild80();
                LoadBuild49();
                LoadBuild20();
                LoadBuild19();
                LoadBuild6();
                LoadBuild11();
                LoadBuild12();
                LoadBuild46();
                LoadBuild47();
                LoadBuild13();
                LoadBuild96();
                LoadBuild98();
                LoadBuild97();
                LoadBuild14();
                LoadBuild50();
                LoadBuild61();
                LoadBuild101();
                LoadBuild102();
                LoadBuild132();
                LoadBuild133();
                LoadBuild134();
                LoadBuild135();
                LoadBuild15();
                LoadBuild43();
                LoadBuild44();
                LoadBuild62();
                LoadBuild45();
                LoadBuild27();
                LoadBuild24();
                LoadBuild25();
                LoadBuild26();
                LoadBuild28();
                LoadBuild29();
                LoadBuild5();
                LoadBuild10();
                LoadBuild35();
                LoadBuild7();
                LoadBuild32();
                LoadBuild99();
                LoadBuild67();
                LoadBuild66();
                LoadBuild68();
                LoadBuild122();
                LoadBuild118();
                LoadBuild8();
                LoadBuild103();
                LoadBuild30();
                LoadBuild31();
                LoadBuild54();
                LoadBuild55();
                LoadBuild56();
                LoadBuild57();
                LoadBuild87();
                LoadBuild88();
                LoadBuild89();
                LoadBuild90();
                LoadBuild91();
                LoadBuild149();
                LoadBuild150();
                LoadBuild151();
                LoadBuild152();
                LoadBuild153();
                LoadBuild143();
                LoadBuild144();
                LoadBuild137();
                LoadBuild138();
                LoadBuild139();
                LoadBuild140();
                LoadBuild147();
                LoadBuild136();
                LoadBuild51();
                LoadBuild100();
                LoadBuild145();
                LoadBuild146();
                LoadBuild34();
                LoadBuild22();
                LoadBuild23();
                LoadBuild36();
                LoadBuild37();
                LoadBuild38();
                LoadBuild39();
                LoadBuild40();
                LoadBuild42();
                LoadBuild41();
                LoadBuild92();
                LoadBuild93();
                LoadBuild128();
                LoadBuild129();
                LoadBuild131();
                LoadBuild63();
                LoadBuild69();
                LoadBuild64();
                LoadBuild9();
                LoadBuild16();
                LoadBuild17();
                LoadBuild18();
                //greenhouse
                LoadBuild130();
                LoadBuild104();
                LoadBuild105();
                LoadBuild106();
                LoadBuild107();
                LoadBuild108();
                LoadBuild109();
                LoadBuild110();
                LoadBuild154();
                LoadBuild155();
                LoadBuild111();
                LoadBuild119();
                LoadBuild125();
                LoadBuild112();
                LoadBuild113();
                LoadBuild114();
                LoadBuild115();
                LoadBuild116();
                LoadBuild117();
                LoadBuild123();
                LoadBuild124();
                //fires
                LoadBuild148();
                LoadBuild21();
                LoadBuild65();
                LoadBuild58();
                LoadBuild33();



            fireVol.outputAudioMixerGroup = AudioMan.instance.m_ambientMixer;
            
            //fire2Vol.outputAudioMixerGroup = AudioMan.instance.m_ambientMixer;
            Jotunn.Logger.LogMessage("Loaded Game VFX and SFX");
           /* }
            catch (Exception ex)
            {
                Jotunn.Logger.LogError($"Error while running OnVanillaLoad: {ex.Message}");
            }
            finally
            {*/
                //Jotunn.Logger.LogMessage("Load Complete. Bone Appetit yall.");

                ItemManager.OnVanillaItemsAvailable -= LoadSounds;
            //}
        }
        private void LoadHammerTable()
        {
            var hammerTableFab = assetBundle.LoadAsset<GameObject>("_RKCustomTable");
            improvedHammer = new CustomPieceTable(hammerTableFab,
            new PieceTableConfig
            {
                CanRemovePieces = true,
                UseCategories = false,
                UseCustomCategories = true,
                CustomCategories = new string[]
                {
                    "Structure", "Furniture", "Roofs", "Outdoors", "GreenHouse"
                }
            });
            
            PieceManager.Instance.AddPieceTable(improvedHammer);
            LoadHammer();
        }

        /*private void Repair()
        {
            var repair = new CustomPiece(repairFab,
                new PieceConfig
                {
                    PieceTable = "_RKCustomTable",
                    Category = "Structure"
                });
            PieceManager.Instance.AddPiece(repair);
        }
        /*private void Repair2()
        {
            var repair = new CustomPiece(repairFab2,
                new PieceConfig
                {
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture"
                });
            PieceManager.Instance.AddPiece(repair);
        }
        private void Repair3()
        {
            var repair = new CustomPiece(repairFab3,
                new PieceConfig
                {
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs"
                });
            PieceManager.Instance.AddPiece(repair);
        }
        private void Repair4()
        {
            var repair = new CustomPiece(repairFab4,
                new PieceConfig
                {
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors"
                });
            PieceManager.Instance.AddPiece(repair);
        }
        private void Repair5()
        {
            var repair = new CustomPiece(repairFab5,
                new PieceConfig
                {
                    PieceTable = "_RKCustomTable",
                    Category = "GreenHouse"
                });
            PieceManager.Instance.AddPiece(repair);
        }*/
        private void LoadHammer()
        {
            var itemFab = assetBundle.LoadAsset<GameObject>("rk_hammer");
            var item = new CustomItem(itemFab, false,
                new ItemConfig
                {
                    Name = "$ImprovedHammer",
                    Amount = 1,
                    Enabled = true,
                    PieceTable = "_RKCustomTable",
                    //CraftingStation = "piece_workbench",
                    RepairStation = "piece_workbench",
                    //MinStationLevel = 1,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 4},
                        new RequirementConfig {Item = "Stone", Amount = 2}
                    }
                });
            ItemManager.Instance.AddItem(item);
        }
        private void LoadBuild1()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_45corner_roof");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "45 Corner Roof",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                    
                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild2()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_45roof");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "45 Roof",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild3()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_45corner_roof2");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "45 Wood Corner Roof",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild4()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_45roof2");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "45 Wood Roof",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild5()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_bed");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Bed",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "FineWood", Amount = 5, Recover = true},
                        new RequirementConfig {Item = "Feathers", Amount = 6, Recover = true},
                        new RequirementConfig {Item = "DeerHide", Amount = 4, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild6()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_blackpine");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Black Pine Tree",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 10, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild7()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_bucket");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Bucket",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild8()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_cabinet");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Cabinet",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "FineWood", Amount = 4, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild9()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_candle");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Candle",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Tin", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "Honey", Amount = 2, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildMetal;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakMetal;
            breakfx.m_hitEffect = hitMetal;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild10()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_chair");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Chair",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 10, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild11()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_cherryblossom");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Cherry Blossom Tree",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 10, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild12()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_cherryblossom2");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Cherry Blossom Tree 2",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 10, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild13()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_fence");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Fence",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild14()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_fence2");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Fence 2",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild15()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_fencehalf");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Fence Half",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild16()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_lamp");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Lamp - Table",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Tin", Amount = 2, Recover = true},
                        new RequirementConfig {Item = "Resin", Amount = 2, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildMetal;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakMetal;
            breakfx.m_hitEffect = hitMetal;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild17()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_lamphanging");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Lamp Hanging",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Tin", Amount = 2, Recover = true},
                        new RequirementConfig {Item = "Resin", Amount = 2, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildMetal;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakMetal;
            breakfx.m_hitEffect = hitMetal;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild18()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_lamppost");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Lamp Post",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Tin", Amount = 2, Recover = true},
                        new RequirementConfig {Item = "Resin", Amount = 2, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildMetal;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakMetal;
            breakfx.m_hitEffect = hitMetal;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild19()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_maple");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Maple Tree",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 10, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild20()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_crate2");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Crate",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 6, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild21()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_groundbrazier");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Ground Brazier",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Iron", Amount = 2, Recover = true},
                        new RequirementConfig {Item = "Coal", Amount = 2, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildMetal;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakMetal;
            breakfx.m_hitEffect = hitMetal;

            fireVol = buildFab.GetComponentInChildren<AudioSource>();
            

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild22()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_post");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Stone Post 4m",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    CraftingStation = "piece_stonecutter",
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Stone", Amount = 4, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildStone;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakStone;
            breakfx.m_hitEffect = hitStone;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild23()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_post2");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Stone Post 2m",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    CraftingStation = "piece_stonecutter",
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Stone", Amount = 2, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildStone;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakStone;
            breakfx.m_hitEffect = hitStone;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild24()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_rug");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Rug Large",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "TrollHide", Amount = 6, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildRug;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild25()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_rug2");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Rug 2 Small",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "LeatherScraps", Amount = 4, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildRug;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild26()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_rug2large");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Rug 2 Large",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "LeatherScraps", Amount = 6, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildRug;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild27()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_rugsmall");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Rug Small",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "TrollHide", Amount = 4, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildRug;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild28()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_screen");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Screen",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "TrollHide", Amount = 2, Recover = true},
                        new RequirementConfig {Item = "Wood", Amount = 4, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild29()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_screen2");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Screen 2",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "TrollHide", Amount = 2, Recover = true},
                        new RequirementConfig {Item = "Wood", Amount = 4, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild30()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_shelflong");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Shelf Long",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 4, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild31()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_shelfsmall");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Shelf Short",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 6, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild32()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_sidetable");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Side Table",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 6, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild33()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_stonehearth");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Stone Hearth",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Stone", Amount = 10, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildStone;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakStone;
            breakfx.m_hitEffect = hitStone;

            fireVol = buildFab.GetComponentInChildren<AudioSource>();

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild34()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_stoneslab");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Stone Slab",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    CraftingStation = "piece_stonecutter",
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Stone", Amount = 2, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildStone;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakStone;
            breakfx.m_hitEffect = hitStone;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild35()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_table");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Table",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 10, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild36()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_well");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Well",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    CraftingStation = "piece_stonecutter",
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Stone", Amount = 10, Recover = true},
                        new RequirementConfig {Item = "Wood", Amount = 4, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildStone;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakStone;
            breakfx.m_hitEffect = hitStone;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild37()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_window");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Window Tall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 6, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild38()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_windowshort");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Window Medium",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 4, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild39()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_windowshortest");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Window Short",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild40()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_windowplain");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Window Simple",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild41()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_windowstonesmall");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Window Small Stone",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    CraftingStation = "piece_stonecutter",
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Stone", Amount = 2, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildStone;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakStone;
            breakfx.m_hitEffect = hitStone;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild42()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_windowstone");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Window Long Stone",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    CraftingStation = "piece_stonecutter",
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Stone", Amount = 4, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildStone;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakStone;
            breakfx.m_hitEffect = hitStone;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild43()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_fenceheavy");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Fence Heavy",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "RoundLog", Amount = 4, Recover = true},
                        new RequirementConfig {Item = "Wood", Amount = 4, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild44()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_fenceheavy_corner");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Fence Heavy Corner",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 4, Recover = true},
                        new RequirementConfig {Item = "RoundLog", Amount = 4, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild45()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_fountain");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Fountain",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    CraftingStation = "piece_stonecutter",
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Stone", Amount = 10, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildStone;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakStone;
            breakfx.m_hitEffect = hitStone;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild46()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_cherryblossom3");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Cherry Blossom Tree 3",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 10, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild47()
        {
            
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_cherryblossom4");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Cherry Blossom Tree 4",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 10, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild48()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_barrel");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Barrel",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 6, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild49()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_crate");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Crate",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 6, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild50()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_fence3");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Fence Iron",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    CraftingStation = "forge",
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Iron", Amount = 1, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildMetal;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakMetal;
            breakfx.m_hitEffect = hitMetal;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild51()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_floor");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Floor",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "Stone", Amount = 1, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild52()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_roof");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "26 Roof",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild53()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_roof_corner");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "26 Roof Corner",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild54()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_wall");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Wall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    CraftingStation = "piece_stonecutter",
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Stone", Amount = 2, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildStone;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakStone;
            breakfx.m_hitEffect = hitStone;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild55()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_wall2");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Wall 2",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild56()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_Wall3");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Wall 3",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "Stone", Amount = 1, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildStone;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakStone;
            breakfx.m_hitEffect = hitStone;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild57()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_wall4");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Wall 4",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild58()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_hearthsmall");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Small Hearth",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    CraftingStation = "piece_stonecutter",
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Stone", Amount = 8, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildStone;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakStone;
            breakfx.m_hitEffect = hitStone;

            fireVol = buildFab.GetComponentInChildren<AudioSource>();
            //fire2Vol.outputAudioMixerGroup = AudioMan.instance.m_ambientMixer;
            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild59()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_roof2");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "26 Roof 2",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild60()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_roof_corner2");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "26 Roof Corner 2",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild61()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_fence4");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Fence 4",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
         private void LoadBuild62()
         {
             var buildFab = assetBundle.LoadAsset<GameObject>("piece_buildtower");
             var build = new CustomPiece(buildFab,
                 new PieceConfig
                 {
                     Name = "Tower",
                     AllowedInDungeons = false,
                     PieceTable = "_RKCustomTable",
                     Category = "Outdoors",
                     Enabled = true,
                     Requirements = new RequirementConfig[]
                     {
                         new RequirementConfig {Item = "Wood", Amount = 10, Recover = true},
                         new RequirementConfig {Item = "RoundLog", Amount = 6, Recover = true}
                     }

                 });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
         }
        private void LoadBuild63()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_woodrack");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Wood Rack",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                         new RequirementConfig {Item = "Wood", Amount = 20, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild64()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_outhouse");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Outhouse",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                         new RequirementConfig {Item = "Wood", Amount = 20, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild65()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_hearthdim");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Smokless Dimmer Hearth",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    CraftingStation = "piece_stonecutter",
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Stone", Amount = 15, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildStone;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakStone;
            breakfx.m_hitEffect = hitStone;

            fireVol = buildFab.GetComponentInChildren<AudioSource>();

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild66()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_stairstone2");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Stone Roof 2",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    CraftingStation = "piece_stonecutter",
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Stone", Amount = 6, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildStone;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakStone;
            breakfx.m_hitEffect = hitStone;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild67()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_stairstone");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Stone Roof",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    CraftingStation = "piece_stonecutter",
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Stone", Amount = 4, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildStone;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakStone;
            breakfx.m_hitEffect = hitStone;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild68()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_stairstone3");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Stone Stairs",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    CraftingStation = "piece_stonecutter",
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Stone", Amount = 4, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildStone;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakStone;
            breakfx.m_hitEffect = hitStone;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild69()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_stool");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Toilet Seat",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 4, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildStone;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakStone;
            breakfx.m_hitEffect = hitStone;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild70()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_26beam");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "26 Beam",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild71()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_26inner");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "26 Inner Roof Corner",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild72()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_26inner2");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "26 Inner Roof Corner",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild73()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_26ridge");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "26 Roof Ridge",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild74()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_26ridge2");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "26 Roof Ridge",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild75()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_26roof_x");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "26 Roof X",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild76()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_26wall");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "26 Wall Top",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild77()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_26wall2");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "26 Wall Top",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild78()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_45beam");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "45 Beam",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild79()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_45inner");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "45 Inner Roof Corner",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild80()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_barrel2");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Barrel",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 6, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild81()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_45inner2");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "45 Inner Roof Corner",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild82()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_45ridge");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "45 Roof Ridge",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild83()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_45ridge2");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "45 Roof Ridge",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild84()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_45roof_x");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "45 Roof X",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild85()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_45wall");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "45 Wall Top",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild86()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_45wall2");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "45 Wall Top",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild87()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_wall5");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Half Wall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild88()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_wall6");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Half Wall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild89()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_wall7");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Half Wall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild90()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_wall8");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Half Wall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    CraftingStation = "piece_stonecutter",
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Stone", Amount = 2, Recover = true}

                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild91()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_wall9");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Half Wall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 1, Recover = true},
                        new RequirementConfig {Item = "Stone", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild92()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_glassdoor");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Glass Door",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 6, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            var door = buildFab.GetComponent<Door>();
            door.m_closeEffects = doorClose;
            door.m_openEffects = doorOpen;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild93()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_glassdoor2");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Glass Door",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 6, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            var door = buildFab.GetComponent<Door>();
            door.m_closeEffects = doorClose;
            door.m_openEffects = doorOpen;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild94()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_1mbeam");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "1m Beam",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 1, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild95()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_2mbeam");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "2m Beam",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild96()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_fence5");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Darker Fence",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild97()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_fence7");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Short Fence",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild98()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_fence6");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "26 Fence",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild99()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_woodstairs");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Wood Stairs",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 4, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild100()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_floor2");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Floor",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild101()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_fence8");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Iron Fence",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Iron", Amount = 1, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild102()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_fence9");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "26 Iron Fence",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Iron", Amount = 1, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild103()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_cabinet2");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Cabinet",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "FineWood", Amount = 4, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild104()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_floor3");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Glass Floor",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "GreenHouse",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Stone", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild105()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_floor4");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Glass Floor",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "GreenHouse",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Stone", Amount = 1, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild106()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_wall10");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Glass Wall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "GreenHouse",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Stone", Amount = 2, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildStone;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakStone;
            breakfx.m_hitEffect = hitStone;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild107()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_wall11");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Glass Wall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "GreenHouse",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Stone", Amount = 1, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildStone;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakStone;
            breakfx.m_hitEffect = hitStone;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild108()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_45roof3");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "45 Glass Roof",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "GreenHouse",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Stone", Amount = 2, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildStone;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakStone;
            breakfx.m_hitEffect = hitStone;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild109()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_45wall3");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "45 Glass Wall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "GreenHouse",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Stone", Amount = 2, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildStone;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakStone;
            breakfx.m_hitEffect = hitStone;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild110()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_roof3");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Glass Wall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "GreenHouse",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Stone", Amount = 2, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildStone;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakStone;
            breakfx.m_hitEffect = hitStone;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild111()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_26wall3");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Glass Wall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "GreenHouse",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Stone", Amount = 2, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildStone;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakStone;
            breakfx.m_hitEffect = hitStone;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild112()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_45beam2");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Metal Beam",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "GreenHouse",
                    Enabled = true,
                    CraftingStation = "forge",
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Iron", Amount = 1, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildMetal;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakMetal;
            breakfx.m_hitEffect = hitMetal;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild113()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_26beam2");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Metal Beam",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "GreenHouse",
                    Enabled = true,
                    CraftingStation = "forge",
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Iron", Amount = 1, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildMetal;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakMetal;
            breakfx.m_hitEffect = hitMetal;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild114()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_2mvertbeam");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Metal Beam",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "GreenHouse",
                    Enabled = true,
                    CraftingStation = "forge",
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Iron", Amount = 1, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildMetal;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakMetal;
            breakfx.m_hitEffect = hitMetal;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild115()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_2mhorzbeam");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Metal Beam",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "GreenHouse",
                    Enabled = true,
                    CraftingStation = "forge",
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Iron", Amount = 1, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildMetal;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakMetal;
            breakfx.m_hitEffect = hitMetal;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild116()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_1mbeam2");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Metal Beam",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "GreenHouse",
                    Enabled = true,
                    CraftingStation = "forge",
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Iron", Amount = 1, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildMetal;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakMetal;
            breakfx.m_hitEffect = hitMetal;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild117()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_shelftable");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Greenhouse Counter",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "GreenHouse",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 4, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild118()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_shelftable2");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Counter",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 4, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
            private void LoadBuild119()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_26to45wall");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Glass Wall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "GreenHouse",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Stone", Amount = 1, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildStone;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakStone;
            breakfx.m_hitEffect = hitStone;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild120()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_26to45wall2");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Wall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 1, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildStone;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakStone;
            breakfx.m_hitEffect = hitStone;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild121()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_26to45wall3");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Wall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Stone", Amount = 1, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildStone;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakStone;
            breakfx.m_hitEffect = hitStone;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild122()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_sink");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Sink",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    CraftingStation = "forge",
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Copper", Amount = 2, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildMetal;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakMetal;
            breakfx.m_hitEffect = hitMetal;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild123()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_pot");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Pot",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "GreenHouse",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true},
                        new RequirementConfig {Item = "Stone", Amount = 1, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild124()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_bonzai");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Bonsai",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "GreenHouse",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild125()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_26to45wall6");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Glass Wall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "GreenHouse",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Stone", Amount = 1, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildStone;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakStone;
            breakfx.m_hitEffect = hitStone;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild126()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_26to45wall4");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Wall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 1, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildStone;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakStone;
            breakfx.m_hitEffect = hitStone;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild127()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_26to45wall5");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Wall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Stone", Amount = 1, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildStone;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakStone;
            breakfx.m_hitEffect = hitStone;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild128()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_door");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Door",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 6, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            var door = buildFab.GetComponent<Door>();
            door.m_closeEffects = doorClose;
            door.m_openEffects = doorOpen;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild129()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_door2");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Door",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 6, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            var door = buildFab.GetComponent<Door>();
            door.m_closeEffects = doorClose;
            door.m_openEffects = doorOpen;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild130()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_door3");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Door",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "GreenHouse",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 6, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            var door = buildFab.GetComponent<Door>();
            door.m_closeEffects = doorClose;
            door.m_openEffects = doorOpen;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild131()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_door4");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Door",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 6, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            var door = buildFab.GetComponent<Door>();
            door.m_closeEffects = doorClose;
            door.m_openEffects = doorOpen;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild132()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_gate");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Gate",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 6, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            var door = buildFab.GetComponent<Door>();
            door.m_closeEffects = doorClose;
            door.m_openEffects = doorOpen;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild133()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_gate2");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Gate",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 6, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            var door = buildFab.GetComponent<Door>();
            door.m_closeEffects = doorClose;
            door.m_openEffects = doorOpen;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild134()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_gate3");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Gate",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Iron", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            var door = buildFab.GetComponent<Door>();
            door.m_closeEffects = doorClose;
            door.m_openEffects = doorOpen;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild135()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_gate4");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Gate",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 6, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            var door = buildFab.GetComponent<Door>();
            door.m_closeEffects = doorClose;
            door.m_openEffects = doorOpen;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild136()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_drawbridge");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Door",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 6, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            var door = buildFab.GetComponent<Door>();
            door.m_closeEffects = doorClose;
            door.m_openEffects = doorOpen;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild137()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_door5");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Door",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 6, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            var door = buildFab.GetComponent<Door>();
            door.m_closeEffects = doorClose;
            door.m_openEffects = doorOpen;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild138()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_door6");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Door",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 6, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            var door = buildFab.GetComponent<Door>();
            door.m_closeEffects = doorClose;
            door.m_openEffects = doorOpen;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild139()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_door7");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Door",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 6, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            var door = buildFab.GetComponent<Door>();
            door.m_closeEffects = doorClose;
            door.m_openEffects = doorOpen;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild140()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_door8");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Door",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 6, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            var door = buildFab.GetComponent<Door>();
            door.m_closeEffects = doorClose;
            door.m_openEffects = doorOpen;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild141()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_45wall4");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "45 Wall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildStone;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakStone;
            breakfx.m_hitEffect = hitStone;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild142()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_26wall4");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "26 Wall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildStone;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakStone;
            breakfx.m_hitEffect = hitStone;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild143()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_curvecorner");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Arch Corner",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 1, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildStone;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakStone;
            breakfx.m_hitEffect = hitStone;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild144()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_curvecorner2");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Arch Corner",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 1, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildStone;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakStone;
            breakfx.m_hitEffect = hitStone;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild145()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_floor5");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Marble Floor",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Stone", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildStone;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakStone;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild146()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_floorquarter");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "1x1 Marble Floor",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Stone", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildStone;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakStone;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild147()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_door9");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Door",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 6, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            var door = buildFab.GetComponent<Door>();
            door.m_closeEffects = doorClose;
            door.m_openEffects = doorOpen;

            PieceManager.Instance.AddPiece(build);
        }
        
        private void LoadBuild148()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_brazier2");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Ground Brazier",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Copper", Amount = 5, Recover = true},
                        new RequirementConfig {Item = "Coal", Amount = 2, Recover = true},
                        new RequirementConfig {Item = "Chain", Amount = 1, Recover = true}
                    }

                });
            var buildfx = buildFab.GetComponent<Piece>();
            buildfx.m_placeEffect = buildMetal;

            var breakfx = buildFab.GetComponent<WearNTear>();
            breakfx.m_destroyedEffect = breakMetal;
            breakfx.m_hitEffect = hitMetal;

            fireVol = buildFab.GetComponentInChildren<AudioSource>();
            
            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild149()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_wall12");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Wall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild150()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_wall13");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Wall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild151()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_wall14");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Wall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild152()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_45wall5");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "45 Wall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 1, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild153()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_26wall5");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "26 Wall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 1, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild154()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_roof_corner3");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "26 Glass Roof Corner",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "GreenHouse",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Stone", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild155()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_45corner_roof3");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "45 Glass Roof Corner",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "GreenHouse",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Stone", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
    }
}
