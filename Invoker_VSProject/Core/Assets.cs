using System.IO;
using System.Reflection;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using R2API;

namespace Invoker.Core
{
   public static class Assets
    {
        public static AssetBundle MainAssetBundle = null;
        public static AssetBundleResourcesProvider Provider;

        public static Sprite portrait;
        public static Sprite defaultSkin;

        public static GameObject elementalBoltOrbEffectPrefab;
        public static GameObject quasElementPrefab;
        public static GameObject wexElementPrefab;
        public static GameObject exortElementPrefab;

        public const string castMeteorSound = "cast_meteor";
        public const string castInvokeSound = "cast_invoke";
        public const string miniTornadoAmbientSound = "mini_tornado_ambient";
        public const string iceWallImpactSound = "ice_wall_slow";
        public const string castTornadoNormalSound = "cast_tornado1";
        public const string castTornadoSpecialSound = "cast_tornado2";
        public const string meteorImpactSound = "meteor_impact";
        public const string attackLaunchSound = "attack_launch";
        public const string castColdSnapSound = "cast_cold_snap";
        public const string castDeafBlastNormalSound = "cast_deafening_blast1";
        public const string castDeafBlastSpecialSound = "cast_deafening_blast2";
        public const string tornadoAmbientSound = "tornado_ambient";
        public const string meteorAmbientSound = "meteor_ambient";
        public const string castEMPSound = "cast_emp";
        public const string EMPChargeSound = "emp_charge";
        public const string castGhostWalkSound = "cast_ghost_walk";
        public const string EMPExplodeSound = "emp_explode";
        public const string preAttackSound = "attack_pre";
        public const string sunstrikeChargeNormalSound = "sunstrike_charge1";
        public const string sunstrikeChargeSpecialSound = "sunstrike_charge2";
        public const string attackHitSound = "attack_hit";
        public const string sunstrikeExplodeNormalSound = "sunstrike_explode1";
        public const string sunstrikeExplodeSpecialSound = "sunstrike_explode2";
        public const string elementSwitchSound = "elementswitch_click";

        public static void InitializeAssets()
        {
            if (MainAssetBundle == null)
            {
                using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Invoker.invokerbundle"))
                {
                    MainAssetBundle = AssetBundle.LoadFromStream(assetStream);
                    Provider = new AssetBundleResourcesProvider("@Invoker", MainAssetBundle);
                    ResourcesAPI.AddProvider(Provider);
                }
            }

            using (Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream("Invoker.invokersoundbank.bnk"))
            {
                byte[] array = new byte[manifestResourceStream2.Length];
                manifestResourceStream2.Read(array, 0, array.Length);
                SoundAPI.SoundBanks.Add(array);
            }

            portrait = MainAssetBundle.LoadAsset<Sprite>("Portrait");
            defaultSkin = MainAssetBundle.LoadAsset<Sprite>("DefaultSkin");

            elementalBoltOrbEffectPrefab = MainAssetBundle.LoadAsset<GameObject>("ElementalBoltOrbEffect");
            quasElementPrefab = MainAssetBundle.LoadAsset<GameObject>("QuasElement");
            wexElementPrefab = MainAssetBundle.LoadAsset<GameObject>("WexElement");
            exortElementPrefab = MainAssetBundle.LoadAsset<GameObject>("ExortElement");
        }
    }
}
