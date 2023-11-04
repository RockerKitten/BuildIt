using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace BuildIt
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [NetworkCompatibility(CompatibilityLevel.VersionCheckOnly, VersionStrictness.Minor)]
    internal class BuildIt : BaseUnityPlugin
    {
        public const string PluginGUID = "RockerKitten.BuildIt";
        public const string PluginName = "BuildIt";
        public const string PluginVersion = "1.5.0";
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
        public EffectList buildWood;
        public AudioSource fireVol;
        //public GameObject repairFab;

        public CustomPieceTable ImprovedHammer;
        //public static ConfigEntry<float> placementOffsetIncrementConfig;
        //public static ConfigEntry<bool> placementOffsetEnabledConfig;
        //public static ConfigEntry<bool> hidePlaceMarkerConfig;

        private void Awake()
        {
            //Patches.SetupPlacementHooks();
            //SetupConfig();
            LoadAssets();
            AddLocalizations();
            //LoadHammerTable();
            PrefabManager.OnVanillaPrefabsAvailable += LoadSounds;

        }

        /*private void SetupConfig()
        {
           /* placementOffsetIncrementConfig = Config.Bind(
                "Placement", "Placement change increment", 0.01f,
                new ConfigDescription("Placement change when holding Ctrl and/or Alt while scrolling using the Improved Hammer"));

            placementOffsetEnabledConfig = Config.Bind(
                 "Placement", "Enable placement change with Ctrl + Alt", true,
                 new ConfigDescription("Enable placement change when holding Ctrl and/or Alt while scrolling using the Improved Hammer"));
           
            hidePlaceMarkerConfig = Config.Bind(
                 "Placement", "Hide placement marker", true,
                 new ConfigDescription("Hide the yellow placement marker while using the Improved Hammer"));

        }*/

        public void LoadAssets()
        {
            assetBundle = AssetUtils.LoadAssetBundleFromResources("buildit", Assembly.GetExecutingAssembly());
        }
        private void LoadSounds()
        {
            // try
            // {
                var sfxStoneBuild = PrefabManager.Cache.GetPrefab<GameObject>("sfx_build_hammer_stone");
                var vfxStoneBuild = PrefabManager.Cache.GetPrefab<GameObject>("vfx_Place_stone_wall_2x1");
                var sfxWoodBuild = PrefabManager.Cache.GetPrefab<GameObject>("sfx_build_hammer_wood");
                var sfxBreakStone = PrefabManager.Cache.GetPrefab<GameObject>("sfx_rock_destroyed");
                var sfxWoodBreak = PrefabManager.Cache.GetPrefab<GameObject>("sfx_wood_break");
                var sfxMetalBuild = PrefabManager.Cache.GetPrefab<GameObject>("sfx_build_hammer_metal");
                var vfxMetalHit = PrefabManager.Cache.GetPrefab<GameObject>("vfx_HitSparks");
                var vfxAdd = PrefabManager.Cache.GetPrefab<GameObject>("vfx_FireAddFuel");
                var sfxAdd = PrefabManager.Cache.GetPrefab<GameObject>("sfx_FireAddFuel");
                var sfxStoneHit = PrefabManager.Cache.GetPrefab<GameObject>("sfx_Rock_Hit");
                var vfxAddFuel = PrefabManager.Cache.GetPrefab<GameObject>("vfx_HearthAddFuel");
                var chestOpen = PrefabManager.Cache.GetPrefab<GameObject>("sfx_chest_open");
                var sfxTreeFall = PrefabManager.Cache.GetPrefab<GameObject>("sfx_tree_fall_hit");
                var vfxTreeFallHit = PrefabManager.Cache.GetPrefab<GameObject>("vfx_tree_fall_hit");
                var sfxTreeHit = PrefabManager.Cache.GetPrefab<GameObject>("sfx_tree_hit");
                var vfxBirch = PrefabManager.Cache.GetPrefab<GameObject>("vfx_birch1_cut");
                var sfxFall = PrefabManager.Cache.GetPrefab<GameObject>("sfx_tree_fall");
                var vfxWoodHit = PrefabManager.Cache.GetPrefab<GameObject>("vfx_SawDust");
                var vfxDestroyLogHalf = PrefabManager.Cache.GetPrefab<GameObject>("vfx_firlogdestroyed_half");
                var sfxBuildRug = PrefabManager.Cache.GetPrefab<GameObject>("sfx_build_hammer_default");
                var sfxDoorOpen = PrefabManager.Cache.GetPrefab<GameObject>("sfx_door_open");
                var sfxDoorClose = PrefabManager.Cache.GetPrefab<GameObject>("sfx_door_close");


                buildStone = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxStoneBuild }, new EffectList.EffectData { m_prefab = vfxStoneBuild } } };
                breakStone = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxBreakStone }, new EffectList.EffectData { m_prefab = vfxWoodHit } } };
                hitStone = new EffectList { m_effectPrefabs = new EffectList.EffectData[1] { new EffectList.EffectData { m_prefab = sfxStoneHit } } };
                buildWood = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxWoodBuild }, new EffectList.EffectData { m_prefab = vfxStoneBuild } } };
                breakWood = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxWoodBreak }, new EffectList.EffectData { m_prefab = vfxWoodHit } } };
                hitWood = new EffectList { m_effectPrefabs = new EffectList.EffectData[1] { new EffectList.EffectData { m_prefab = vfxWoodHit} } };
                buildMetal = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxMetalBuild }, new EffectList.EffectData {m_prefab =  vfxStoneBuild} } };
                breakMetal = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxBreakStone }, new EffectList.EffectData { m_prefab = vfxMetalHit } } };
                hitMetal = new EffectList { m_effectPrefabs = new EffectList.EffectData[1] { new EffectList.EffectData { m_prefab = vfxMetalHit } } };
                hearthAddFuel = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = vfxAddFuel }, new EffectList.EffectData { m_prefab = sfxAdd } } };
                fireAddFuel = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = vfxAdd }, new EffectList.EffectData { m_prefab = sfxAdd } } };
                buildRug = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxBuildRug }, new EffectList.EffectData { m_prefab = vfxStoneBuild } } };
                doorOpen = new EffectList { m_effectPrefabs = new EffectList.EffectData[1] { new EffectList.EffectData { m_prefab = sfxDoorOpen } } };
                doorClose = new EffectList { m_effectPrefabs = new EffectList.EffectData[1] { new EffectList.EffectData { m_prefab = sfxDoorClose } } };


                LoadHammerTable();
            //beams
                LoadBuild94();
                LoadBuild95();
            LoadBuild158();
            LoadBuild161();
            LoadBuild70();
            LoadBuild78();
            LoadBuild75();
            LoadBuild84();
            LoadBuild157();
            LoadBuild160();
            LoadBuild159();
            LoadBuild162();
            LoadBuild163();
            LoadBuild171();
            LoadBuild166();
            LoadBuild176();
            LoadBuild22();
            LoadBuild23();
            //dark wall
            LoadBuild120();
            LoadBuild121();
            LoadBuild126();
            LoadBuild156();
            LoadBuild77();
            LoadBuild142();
            LoadBuild153();
            LoadBuild86();
            LoadBuild141();
            LoadBuild152();
            LoadBuild143();
            LoadBuild144();
            //light wall
            LoadBuild168();
            LoadBuild169();
            LoadBuild170();
            LoadBuild177();
            LoadBuild178();
            //stone wall
            LoadBuild199(); 
            LoadBuild127();
            LoadBuild76();
            LoadBuild85();
            //roofs
            LoadBuild52();
            LoadBuild53();
            LoadBuild71();
            LoadBuild73();
            LoadBuild188();
            LoadBuild189();
            LoadBuild164();
            LoadBuild165();
            LoadBuild59();
            LoadBuild60();
            LoadBuild72();
            LoadBuild74();

            LoadBuild2();
            LoadBuild1();
            LoadBuild79();
            LoadBuild82();
            LoadBuild175();
            LoadBuild172();
            LoadBuild173();
            LoadBuild174();
            LoadBuild4();
            LoadBuild3();
            LoadBuild81();
            LoadBuild83();
            //stuff
            LoadBuild48();
            LoadBuild80();
            LoadBuild49();
            LoadBuild20();
            LoadBuild19();
            //trees
            LoadBuild6();
            LoadBuild11();
            LoadBuild12();
            LoadBuild46();
            LoadBuild47();
            //fences and gates
            LoadBuild132();
            LoadBuild97();
            LoadBuild98();
            LoadBuild96();
            LoadBuild185();
            LoadBuild183();
            LoadBuild182();
            LoadBuild13();
            LoadBuild133();
            LoadBuild14();
            LoadBuild134();
            LoadBuild50();
            LoadBuild135();
            LoadBuild61();
            LoadBuild101();
            LoadBuild102();
            LoadBuild15();
            LoadBuild62();
            LoadBuild43();
            LoadBuild44();
            //outside
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
            //inside
            LoadBuild122();
            LoadBuild118();
            LoadBuild8();
            LoadBuild103();
            LoadBuild30();
            LoadBuild31();
            //wall
            LoadBuild55();
            LoadBuild89();
            LoadBuild200();
            LoadBuild88();
            LoadBuild57();
            LoadBuild87();
            LoadBuild205();
            LoadBuild149();
            LoadBuild150();
            LoadBuild151();
            LoadBuild190();
            LoadBuild191();
            LoadBuild192();
            LoadBuild54();
            LoadBuild90();
            LoadBuild167();
            LoadBuild201();
            LoadBuild56();
            LoadBuild91();
            //floor
            LoadBuild100();
            LoadBuild184();
            LoadBuild198();
            LoadBuild51();
            LoadBuild145();
            LoadBuild146();
            LoadBuild34();
            LoadBuild99();
            LoadBuild197();
            LoadBuild67();
            LoadBuild66();
            LoadBuild68();
           // LoadBuild202();
            //doors
            LoadBuild128();
            LoadBuild179();
            LoadBuild129();
            LoadBuild131();
            LoadBuild180();
            LoadBuild137();
            LoadBuild181();
            LoadBuild138();
            LoadBuild139();
            LoadBuild140();
            LoadBuild147();
            LoadBuild136();
            LoadBuild203();
            //windows
            LoadBuild40();
            LoadBuild39();
            LoadBuild38();
            LoadBuild37();
            LoadBuild92();
            LoadBuild93();
            LoadBuild194();
            LoadBuild196();
            LoadBuild195();
            LoadBuild193();
            LoadBuild186();
            LoadBuild187();
            LoadBuild41();
            LoadBuild42();
            LoadBuild36();
            LoadBuild63();
            LoadBuild69();
            LoadBuild64();
            //light
            LoadBuild148();
            LoadBuild9();
            LoadBuild16();
            LoadBuild17();
            LoadBuild18();
            LoadBuild21();
            LoadBuild65();
            LoadBuild58();
            LoadBuild33();
            //greenhouse
            //glass
            LoadBuild104();
            LoadBuild105();
            LoadBuild130();
            LoadBuild106();
            LoadBuild107();
            LoadBuild111();
            LoadBuild109();
            LoadBuild119();
            LoadBuild125();
            LoadBuild108();
            LoadBuild155();
            LoadBuild110();
            LoadBuild154();
            //metal and greenhouse
            LoadBuild116();
            LoadBuild115();
            LoadBuild204();
            LoadBuild114();
            LoadBuild113();
            LoadBuild112();
            LoadBuild117();
            LoadBuild123();
            LoadBuild124();

                /*LoadBuild76();
                LoadBuild77();
                LoadBuild142();
                LoadBuild70();
                LoadBuild75();
                LoadBuild52();
                LoadBuild53();
                LoadBuild188();
                LoadBuild189();
                LoadBuild71();
                LoadBuild73();
                LoadBuild59();
                LoadBuild60();
                LoadBuild72();
                LoadBuild164();
                LoadBuild165();
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
                LoadBuild172();
                LoadBuild173();
                LoadBuild174();
                LoadBuild175();
                LoadBuild176();
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
                LoadBuild190();
                LoadBuild191();
                LoadBuild192();
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
                LoadBuild193();
                LoadBuild194();
                LoadBuild195();
                LoadBuild196();
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
                LoadBuild186();
                LoadBuild187();
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
                //end greenhouse
                LoadBuild156();
                LoadBuild157();
                LoadBuild158();
                LoadBuild159();
                LoadBuild160();
                LoadBuild161();
                LoadBuild162();
                LoadBuild163();
                LoadBuild166();
                //LoadBuild167();
                LoadBuild168();
                LoadBuild169();
                LoadBuild170();
                LoadBuild171();
                LoadBuild177();
                LoadBuild178();
                LoadBuild179();
                LoadBuild180();
                LoadBuild181();
                LoadBuild182();
                LoadBuild183();
                LoadBuild184();
                LoadBuild185();
                LoadBuild197();
                //fires
                LoadBuild148();
                LoadBuild21();
                LoadBuild65();
                LoadBuild58();
                LoadBuild33();*/


            fireVol.outputAudioMixerGroup = AudioMan.instance.m_ambientMixer;
            
            Jotunn.Logger.LogMessage("Loaded Game VFX and SFX");
            /*}
            catch (Exception ex)
            {
                Jotunn.Logger.LogError($"Error while running OnVanillaLoad: {ex.Message}");
            }
            finally
            {*/
                Jotunn.Logger.LogMessage("Load Complete.");

                PrefabManager.OnVanillaPrefabsAvailable -= LoadSounds;
            //}
        }

        private void AddLocalizations()
        {
            CustomLocalization customLocalization = new CustomLocalization();
            customLocalization.AddTranslation("English", new Dictionary<String, String> {
                // Add translations for the custom piece in AddPieceCategories
                //LocalizationManager.Instance.AddLocalization(new CustomLocalization

                {"piece_cabinet", "Cabinet"},
                //{"tab_structure", "Structure"},{"tab_roofs", "Roofs"},
                //{"tab_outdoors", "Outdoors"},{"tab_greenhouse", "Greenhouse"},
                {"ImprovedHammer", "Improved Hammer"},
                {"piece_45cornderroof", "45 Corner Roof"},{"piece_45roof", "45 Roof"},{"piece_bedrk", "Bed"},
                {"piece_blackpinetree", "Black Pine Tree"},{"piece_bucket", "Bucket"},{"piece_candle", "Candle"},
                {"piece_45roofridge", "45 Roof Ridge"},{"piece_chair","Chair"},{"piece_cherryblossom","Cherry Blossom Tree"},
                {"piece_fencerk","Fence"},{"piece_halffence","Half Fence"},{"piece_tablelamp","Table Lamp"},{"piece_hanginglamp","Hanging Lamp"},
                {"piece_lamppost","Lamp Post"},{"piece_mapletree","Maple Tree"},{"piece_smallcrate","Small Crate"},
                {"piece_groundbrazier","Ground Brazier"},{"piece_4stonepost","Stone Post 4m"},{"piece_2stonepost","Stone Post 2m"},
                {"piece_lrug","Large Rug"},{"piece_srug","Small Rug"},{"piece_screen","Screen"},{"piece_lshelf","Long Shelf"},
                {"piece_sshelf","Short Shelf"},{"piece_sidetable","Side Table"},{"piece_stonehearth","Stone Hearth"},{"piece_stoneslab","Stone Slab"},
                {"piece_tablerk","Table"},{"piece_well","Well"},{"piece_twindow","Window Tall"},{"piece_mwindow","Window Medium"},
                {"piece_swindow","Window Short"},{"piece_swindows","Window Simple"},{"piece_stonewindows","Stone Window Small"},
                {"piece_stonewindowl","Stone Window Long"},{"piece_hfence","Heavy Fence"},{"piece_hfencecorner","Heavy Fence Corner"},
                {"piece_fountain","Fountain"},{"piece_barrel","Barrel"},{"piece_crate","Crate"},{"piece_ironfence","Iron Fence"},{"piece_floorrk","Floor"},
                {"piece_26roofrk","26 Roof"},{"piece_26roofcorner","26 Roof Corner"},{"piece_wallrk","Wall"},{"piece_smallhearth","Small Hearth"},
                {"piece_tower","Tower"},{"pieceroofrk","Roof"},{"piece_45cornerroof","45 Roof Corner"},
                {"piece_woodrack","Wood Rack"},{"piece_outhouse","Outhouse"},{"piece_hearthdim","Dim Smokless Hearth"},
                {"piece_stoneroof","Stone Roof"},{"piece_stonestairs","Stone Stairs"},{"piece_rkstool","Toilet Seat"},
                {"piece_26beam","26 Beam"},{"piece_26inner","26 Inner Corner Roof"},
                {"piece_26ridge","26 Roof Ridge"},{"piece_26roofx","26 Roof X"},{"piece_26wall","26 Wall"},{"piece_45beam","45 Beam"},
                {"piece_45innerc","45 Inner Corner Roof"},{"piece_45roofx","45 Roof X"},{"piece_45wall","45 Wall"},
                {"piece_halfwall","Half Wall"},{"piece_fencehalf","Half Fence"},
                {"piece_glassdoor","Glass Door"},{"piece_rkbeam","1m Beam"},{"piece_rkbeam2","2m Beam"},{"piece_rkpole","1m Pole"},
                {"piece_rkpole2","2m Pole"},{"piece_rkstairs","Wood Stairs"},{"piece_26fence","26 Fence"},
                {"piece_gfloor","Glass Floor"},{"piece_gwall","Glass Wall"},{"piece_45groof","45 Glass Roof"},
                {"piece_45gwall","45 Glass Wall"},{"piece_metbeam","Metal Beam"},{"piece_ghousecounter","Greenhouse Counter"},
                {"piece_counter","Counter"},{"piece_rksink","Sink"},{"piece_potrk","Pot"},{"piece_rkbonsai","Bonsai"},
                {"piece_rkdoor","Door"},{"piece_rkgate","Gate"},{"piece_curvecorner","Arch Corner"},
                {"piece_26gcroof","26 Glass Corner Roof"},{"piece_45gcroof","45 Glass Corner Roof"},
                {"piece_26to45","26 to 45 Wall"},{"piece_rkbridge","Drawbridge"},{"piece_26groof","26 Glass Roof"},{"piece_metpole1","1m Metal Pole"}
                ,{"piece_metpole2","2m Metal Pole"},{"piece_metbeam1","1m Metal Beam"},{"piece_metbeam2","2m Metal Beam"},{"piece_hangbrazier","Hanging Brazier"}

            });
            LocalizationManager.Instance.AddLocalization(customLocalization);
        }

        private void LoadHammerTable()
        {
            var hammerTableFab = assetBundle.LoadAsset<GameObject>("_RKCustomTable");
            ImprovedHammer = new CustomPieceTable(hammerTableFab,
            new PieceTableConfig
            {
                CanRemovePieces = true,
                UseCategories = false,
                UseCustomCategories = true,
                CustomCategories = new string[]
                {
                    "Structure", "Furniture", "Roofs", "Outdoors", "Greenhouse"
                }
            });
            
            PieceManager.Instance.AddPieceTable(ImprovedHammer);
            LoadHammer();
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
                    PieceTable = "_RKCustomTable",
                    RepairStation = "piece_workbench",
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_45cornerroof",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_45roof",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_45cornerroof",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_45roof",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_bedrk",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_blackpinetree",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_bucket",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_cabinet",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_candle",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_chair",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_cherryblossom",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_cherryblossom",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_fencerk",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_fencerk",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_fencehalf",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_tablelamp",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_hanginglamp",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_lamppost",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_mapletree",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_crate",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "FineWood", Amount = 10, Recover = true},
                        new RequirementConfig {Item = "Iron", Amount = 2, Recover = true}
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_groundbrazier",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_4stonepost",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    CraftingStation = "piece_stonecutter",
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_2stonepost",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    CraftingStation = "piece_stonecutter",
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_lrug",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_srug",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_lrug",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_srug",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_screen",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_screen",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_lshelf",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_sshelf",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_sidetable",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_stonehearth",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_stoneslab",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    CraftingStation = "piece_stonecutter",
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_table",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_well",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    CraftingStation = "piece_stonecutter",
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_twindow",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_mwindow",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_swindow",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_swindows",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_stonewindows",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    CraftingStation = "piece_stonecutter",
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_stonewindowl",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    CraftingStation = "piece_stonecutter",
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_hfence",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_hfencecorner",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_fountain",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    CraftingStation = "piece_stonecutter",
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_cherryblossom",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_cherryblossom",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_barrel",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "FineWood", Amount = 10, Recover = true},
                        new RequirementConfig {Item = "Iron", Amount = 2, Recover = true}
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_crate",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "FineWood", Amount = 10, Recover = true},
                        new RequirementConfig {Item = "Iron", Amount = 2, Recover = true}
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_ironfence",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    CraftingStation = "forge",
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_floorrk",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_26roofrk",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_26roofcorner",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_wallrk",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    CraftingStation = "piece_stonecutter",
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_wallrk",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_wallrk",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_wallrk",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_smallhearth",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    CraftingStation = "piece_stonecutter",
                    Requirements = new[]
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
            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild59()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_roof2");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_26roofrk",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_26roofcorner",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_fencerk",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new[]
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
             var build = new CustomPiece(buildFab, false,
                 new PieceConfig
                 {
                     Name = "$piece_tower",
                     AllowedInDungeons = false,
                     PieceTable = "_RKCustomTable",
                     Category = "Outdoors",
                     Enabled = true,
                     Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_woodrack",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_outhouse",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_hearthdim",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    CraftingStation = "piece_stonecutter",
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_stoneroof",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    CraftingStation = "piece_stonecutter",
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_stoneroof",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    CraftingStation = "piece_stonecutter",
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_stonestairs",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    CraftingStation = "piece_stonecutter",
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_rkstool",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_26beam",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_26inner",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_26inner",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_26ridge",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_26ridge",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_26roofx",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_26wall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_26wall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_45beam",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_45innerc",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_barrel",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "FineWood", Amount = 10, Recover = true},
                        new RequirementConfig {Item = "Iron", Amount = 2, Recover = true}
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_45innerc",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_45roofridge",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_45roofridge",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_45roofx",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_45wall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_45wall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_halfwall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_halfwall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_halfwall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_halfwall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    CraftingStation = "piece_stonecutter",
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_halfwall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_glassdoor",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_glassdoor",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_rkbeam",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_rkbeam2",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_fencerk",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_fencerk",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_26fence",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_rkstairs",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_floorrk",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_fencerk",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_26fence",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_cabinet",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_gfloor",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Greenhouse",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_gfloor",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Greenhouse",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_gwall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Greenhouse",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_gwall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Greenhouse",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_45groof",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Greenhouse",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_45gwall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Greenhouse",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_26groof",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Greenhouse",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_gwall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Greenhouse",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_metbeam",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Greenhouse",
                    Enabled = true,
                    CraftingStation = "forge",
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_metbeam",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Greenhouse",
                    Enabled = true,
                    CraftingStation = "forge",
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_metpole2",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Greenhouse",
                    Enabled = true,
                    CraftingStation = "forge",
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_metbeam2",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Greenhouse",
                    Enabled = true,
                    CraftingStation = "forge",
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_metbeam1",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Greenhouse",
                    Enabled = true,
                    CraftingStation = "forge",
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_ghousecounter",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Greenhouse",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_counter",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_gwall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Greenhouse",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_26to45",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_26to45",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_rksink",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    CraftingStation = "forge",
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_potrk",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Greenhouse",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_rkbonsai",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Greenhouse",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_gwall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Greenhouse",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_26to45",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_26to45",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_rkdoor",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_rkdoor",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_rkdoor",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Greenhouse",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_rkdoor",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_rkgate",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_rkgate",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_rkgate",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_rkgate",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_rkbridge",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_rkdoor",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_rkdoor",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_rkdoor",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_rkdoor",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_45wall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_26wall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_curvecorner",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_curvecorner",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_floorrk",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_floorrk",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_rkdoor",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$c",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_wallrk",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_wallrk",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_wallrk",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_45wall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_26wall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_26gcroof",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Greenhouse",
                    Enabled = true,
                    Requirements = new[]
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
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_45gcroof",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Greenhouse",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild156()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_26to45wall7");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_wallrk",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild157()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_1mbeam1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_rkbeam",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild158()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_1mpole");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_rkpole",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild159()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_1mpole1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_rkpole",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild160()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_2mbeam1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_rkbeam2",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild161()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_2mpole");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_rkpole2",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild162()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_2mpole1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_rkpole2",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild163()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_26beam1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_26beam",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild164()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_26inner1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_26inner",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild165()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_26ridge1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_26ridge",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild166()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_26roof_x1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_26roofx",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild167()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_wall15");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_wallrk",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild168()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_26to45wall8");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_26to45",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild169()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_26to45wall9");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_26to45",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild170()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_26wall5_1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_26wall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild171()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_45beam1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_45beam",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild172()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_45corner_roof1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_45cornerroof",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild173()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_45inner1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_45innerc",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild174()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_45ridge1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_45roofridge",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild175()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_45roof1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_45roof",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild176()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_45roof_x1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_45roofx",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild177()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_45wall5_1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_45wall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild178()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_curvecorner1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_curvecorner",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild179()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_door1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_rkdoor",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild180()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_door4_1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_rkdoor",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild181()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_door5_1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_rkdoor",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild182()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_fence6_1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_fencerk",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild183()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_fence7_1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_fencerk",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild184()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_floor6");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_floorrk",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild185()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_gate1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_rkgate",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 3, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild186()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_glassdoor1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_glassdoor",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true},
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
        private void LoadBuild187()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_glassdoor2_1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_glassdoor",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true},
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
        private void LoadBuild188()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_roof1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_roofrk",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild189()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_roof_corner1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_26roofcorner",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Roofs",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild190()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_wall12_1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_wallrk",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild191()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_wall13_1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_wallrk",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild192()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_wall14_1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_wallrk",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild193()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_window1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_twindow",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 4, Recover = true},
                        new RequirementConfig {Item = "Stone", Amount=2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild194()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_windowplain1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_swindows",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true},
                        new RequirementConfig {Item = "Stone", Amount=1, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild195()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_windowshort1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_mwindow",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 3, Recover = true},
                        new RequirementConfig {Item = "Stone", Amount=1, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild196()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_windowshortest1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_swindow",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true},
                        new RequirementConfig {Item = "Stone", Amount=1, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild197()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_woodstairs1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_rkstairs",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild198()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_floor7");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_floorrk",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild199()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_26to45wall2_1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_26to45",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild200()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_wall17_1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_wallrk",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild201()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_wall16");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_wallrk",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
        /*private void LoadBuild202()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_stairstone4");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_stonestairs",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    CraftingStation = "piece_stonecutter",
                    Requirements = new[]
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
        }*/
        private void LoadBuild203()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rk_drawbridge2");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_rkbridge",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild204()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_1mpole2");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_metpole1",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Greenhouse",
                    Enabled = true,
                    Requirements = new[]
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
        private void LoadBuild205()
        {

            var buildFab = assetBundle.LoadAsset<GameObject>("rk_wall15_1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_wallrk",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
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

    }
}