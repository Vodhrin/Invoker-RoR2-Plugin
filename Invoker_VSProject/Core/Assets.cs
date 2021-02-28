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

        public static void InitializeAssets()
        {
            if (MainAssetBundle == null)
            {
                using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Invoker.invokerbundle"))
                {
                    MainAssetBundle = AssetBundle.LoadFromStream(assetStream);
                    Provider = new AssetBundleResourcesProvider("@Invoker", MainAssetBundle);
                }
            }

            portrait = MainAssetBundle.LoadAsset<Sprite>("Portrait");
            defaultSkin = MainAssetBundle.LoadAsset<Sprite>("DefaultSkin");
        }
    }
}
