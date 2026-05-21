using MelonLoader;
using UnityEngine;
using HarmonyLib;
using Il2CppQuantum.Core;
using Il2CppQuantum;
using System.Configuration;

[assembly: MelonInfo(typeof(DismountVisualizer.Core), "DismountVisualizer", "1.0.0", "taldo", null)]
[assembly: MelonGame("Videocult", "Airframe")]

namespace DismountVisualizer
{
    public class Core : MelonMod
    {
        internal static MelonLogger.Instance Log => Melon<Core>.Instance.LoggerInstance;
        internal static DismountVisualizer.Core Inst => Melon<Core>.Instance;
 
        /*
        public static Texture2D CreateTexture()
        {
            Texture2D texture = new Texture2D(128, 128);
            for (int y = 0; y < texture.height; y++)
            {
                for (int x = 0; x < texture.width; x++)
                {
                    texture.SetPixel(x, y, Color.white);
                }
            }
            texture.Apply();
            return texture;
        }
        */
        public static int progress = 0;
        [HarmonyPatch(typeof(FrameContext), "OnFrameSimulationBegin")]
        private class FrameContext__OnFrameSimulationBegin_Patch
        {
            public static void DrawProgBar()
            {
                Resolution res = Screen.currentResolution;
                
                GUI.Box(new Rect
                (
                    res.width / 2 * 1.3F, 
                    res.height / 2 * 1.2F,
                    20, 
                    progress * 5 * -1
                ), 
                    new Texture()
                );

                GUI.Box(new Rect
                (
                    res.width / 2 * 1.3F, 
                    res.height / 2 * 1.2F - 270,
                    20, 
                    15
                ), 
                    new Texture()
                );
            }
            public static void Postfix(FrameBase f)
            {
                Il2CppSystem.Collections.Generic.List<EntityRef> all_Erefs = new();
                f.GetAllEntityRefs(all_Erefs);

                unsafe {
                foreach (EntityRef eref in all_Erefs)
                    if (f.Has<Player>(eref))
                {
                    Player* player = f.GetPointer<Player>(eref);
                    if (!f.Exists(player->controlledEntity)) continue;
                    progress = player->holdBuy;
                    
                    if (progress > 14)
                    {
                        MelonEvents.OnGUI.Subscribe(DrawProgBar);
                    }
                    else
                    {
                        try { MelonEvents.OnGUI.Unsubscribe(DrawProgBar); }
                        catch{ }
                    }
                    
                }
            }
            }
        }
    }
}