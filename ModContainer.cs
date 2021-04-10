using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace Zenith
{
    public class ModContainer : Mod
    {
        public static ModContainer Instance;
        public MiscShaderData14 finalFractal;
        public Effect pixelShader;

        public override void Load()
        {
            Instance = this;
            pixelShader = GetEffect("Effects/PixelShader");
            finalFractal = new MiscShaderData14(new Ref<Effect>(pixelShader), "FinalFractalVertex").UseProjectionMatrix(true);
        }
    }
}