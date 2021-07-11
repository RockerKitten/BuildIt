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
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class BuildIt : BaseUnityPlugin
    {
        public const string PluginGUID = "RockerKitten.BuildIt";
        public const string PluginName = "BuildIt";
        public const string PluginVersion = "1.0.0";
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
        ///public EffectList treeDestroyed;
        ///public EffectList treeHit;
        ///public EffectList logHit;
        ///public EffectList logDestroy;
        public EffectList buildWood;
        public AudioSource fireVol;
        //public AudioSource fire2Vol;

        public CustomPieceTable improvedHammer;

        private void Awake()
        {
            LoadAssets();
            LoadHammerTable();
            LoadHammer();
            ItemManager.OnVanillaItemsAvailable += LoadSounds;
            
        }

        public void LoadAssets()
        {
            assetBundle = AssetUtils.LoadAssetBundleFromResources("buildit", Assembly.GetExecutingAssembly());
        }
        private void LoadSounds()
        {
            try
            {
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
                ///treeDestroyed = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = vfxadd }, new EffectList.EffectData { m_prefab = sfxadd } } };
                ///treeHit = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = vfxwoodhit }, new EffectList.EffectData { m_prefab = sfxtreehit} } };
                ///logHit = new EffectList { m_effectPrefabs = new EffectList.EffectData[3] { new EffectList.EffectData { m_prefab = sfxtreefall }, new EffectList.EffectData { m_prefab = vfxtreefallhit }, new EffectList.EffectData { m_prefab = sfxtreehit } } };
                ///logDestroy = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = vfxdestroyloghalf }, new EffectList.EffectData { m_prefab = sfxwoodbreak } } };
                LoadBuild1();
                LoadBuild2();
                LoadBuild3();
                LoadBuild4();
                LoadBuild5();
                LoadBuild6();
                LoadBuild7();
                LoadBuild8();
                LoadBuild9();
                LoadBuild10();
                LoadBuild11();
                LoadBuild12();
                LoadBuild13();
                LoadBuild14();
                LoadBuild15();
                LoadBuild16();
                LoadBuild17();
                LoadBuild18();
                LoadBuild19();
                //LoadBuild20();
                LoadBuild22();
                LoadBuild23();
                LoadBuild24();
                LoadBuild25();
                LoadBuild26();
                LoadBuild27();
                LoadBuild28();
                LoadBuild29();
                LoadBuild30();
                LoadBuild31();
                LoadBuild32();
                LoadBuild33();
                LoadBuild34();
                LoadBuild35();
                LoadBuild36();
                LoadBuild37();
                LoadBuild38();
                LoadBuild39();
                LoadBuild40();
                LoadBuild41();
                LoadBuild42();
                LoadBuild43();
                LoadBuild44();
                LoadBuild45();
                LoadBuild46();
                LoadBuild47();
                LoadBuild48();
                LoadBuild49();
                LoadBuild50();
                LoadBuild51();
                LoadBuild52();
                LoadBuild53();
                LoadBuild54();
                LoadBuild55();
                LoadBuild56();
                LoadBuild57();
                LoadBuild59();
                LoadBuild60();
                LoadBuild61();
                LoadBuild62();
                LoadBuild63();
                LoadBuild64();
                LoadBuild65();
                LoadBuild66();
                LoadBuild67();
                LoadBuild68();
                LoadBuild69();
                LoadBuild58();
                LoadBuild21();

            fireVol.outputAudioMixerGroup = AudioMan.instance.m_ambientMixer;
            
            //fire2Vol.outputAudioMixerGroup = AudioMan.instance.m_ambientMixer;
            Jotunn.Logger.LogMessage("Loaded Game VFX and SFX");
            }
            catch (Exception ex)
            {
                Jotunn.Logger.LogError($"Error while running OnVanillaLoad: {ex.Message}");
            }
            finally
            {
                Jotunn.Logger.LogMessage("Load Complete. Bone Appetit yall.");

                ItemManager.OnVanillaItemsAvailable -= LoadSounds;
            }
        }
        private void LoadHammerTable()
        {
            var hammerTableFab = assetBundle.LoadAsset<GameObject>("_RK_HammerPieceTable");
            improvedHammer = new CustomPieceTable(hammerTableFab,
            new PieceTableConfig
            {
                CanRemovePieces = true,
                UseCategories = false,
                UseCustomCategories = true,
                CustomCategories = new string[]
                {
                    "Build", "Furnish", "Decorate", "Other"
                }
            });
            PieceManager.Instance.AddPieceTable(improvedHammer);
            
        }
        private void LoadHammer()
        {
            var itemFab = assetBundle.LoadAsset<GameObject>("rk_hammer");
            var item = new CustomItem(itemFab, false,
                new ItemConfig
                {
                    Name = "$ImprovedHammer",
                    Amount = 1,
                    Enabled = true,
                    PieceTable = "_RK_HammerPieceTable",
                    CraftingStation = "piece_workbench",
                    RepairStation = "piece_workbench",
                    MinStationLevel = 1,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 4, Recover = true, AmountPerLevel =1}
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Build",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Build",
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
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_45coner_roof2");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "45 Wood Corner Roof",
                    AllowedInDungeons = false,
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Build",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Build",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Furnish",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Other",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Decorate",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Furnish",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Decorate",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Furnish",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Other",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Other",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Other",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Other",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Other",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Decorate",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Decorate",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Decorate",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Other",
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
        /*private void LoadBuild20()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_outhouse");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "",
                    AllowedInDungeons = false,
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "", Amount = 1, Recover = true}
                    }

                });
            PieceManager.Instance.AddPiece(build);
        }*/
        private void LoadBuild21()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_groundbrazier");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "Ground Brazier",
                    AllowedInDungeons = false,
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Decorate",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Build",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Build",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Decorate",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Decorate",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Decorate",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Decorate",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Decorate",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Decorate",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Furnish",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Furnish",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Furnish",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Decorate",
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

            //fireVol = buildFab.GetComponentInChildren<AudioSource>();

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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Build",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Furnish",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Other",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Build",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Build",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Build",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Build",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Build",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Build",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Other",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Other",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Other",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Other",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Other",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Decorate",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Decorate",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Other",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Build",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Build",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Build",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Build",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Build",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Build",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Build",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Decorate",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Build",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Build",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Other",
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
                     PieceTable = "_RK_HammerPieceTable",
                     Category = "Other",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Decorate",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Other",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Decorate",
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
                    Name = "Stone Roof",
                    AllowedInDungeons = false,
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Build",
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
                    Name = "Stone Roof Long",
                    AllowedInDungeons = false,
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Build",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Build",
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
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Build",
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

        /*private void LoadBuild62()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_45roof2");
            var build = new CustomPiece(buildFab,
                new PieceConfig
                {
                    Name = "45 Roof 2",
                    AllowedInDungeons = false,
                    PieceTable = "_RK_HammerPieceTable",
                    Category = "Build",
                    Enabled = true,
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            PieceManager.Instance.AddPiece(build);
        }
        /*private void LoadTree()
           {
               var tree = assetBundle.LoadAsset<GameObject>("rk_cherryblossom3");

           }*/
    }
}