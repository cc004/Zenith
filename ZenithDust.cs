using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Zenith
{
    class ZenithDust : ModDust
    {
        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            Color result = new Color(lightColor.ToVector3() * dust.color.ToVector3());
            result.A = 25;
            return result;
        }

        public override bool Update(Dust dust)
		{
			float num103 = dust.scale;
			if (num103 > 1f)
			{
				num103 = 1f;
			}
			if (!dust.noLight)
			{
				Lighting.AddLight(dust.position, dust.color.ToVector3() * num103);
			}
			if (dust.noGravity)
			{
				dust.velocity *= 0.93f;
				if (dust.fadeIn == 0f)
				{
					dust.scale += 0.0025f;
				}
			}
			else
			{
				dust.velocity *= 0.95f;
				dust.scale -= 0.0025f;
			}
			if (WorldGen.SolidTile(Framing.GetTileSafely(dust.position)) && dust.fadeIn == 0f && !dust.noGravity)
			{
				dust.scale *= 0.9f;
				dust.velocity *= 0.25f;
			}
			return true;
		}
    }
}
