using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace Zenith
{
    public static class Utils
	{
		public static Vector2 DirectionTo(this Vector2 Origin, Vector2 Target)
		{
			return Vector2.Normalize(Target - Origin);
		}

		public static float GetLerpValue(float from, float to, float t, bool clamped = false)
		{
			if (clamped)
			{
				if (from < to)
				{
					if (t < from)
					{
						return 0f;
					}
					if (t > to)
					{
						return 1f;
					}
				}
				else
				{
					if (t < to)
					{
						return 1f;
					}
					if (t > from)
					{
						return 0f;
					}
				}
			}
			return (t - from) / (to - from);
		}

		public static float Distance(this Vector2 Origin, Vector2 Target)
		{
			return Vector2.Distance(Origin, Target);
		}

		public static bool GetZenithTarget(this Player player, Vector2 searchCenter, float maxDistance, out int npcTargetIndex)
		{
			npcTargetIndex = 0;
			int? num = null;
			float num2 = maxDistance;
			for (int i = 0; i < 200; i++)
			{
				NPC nPC = Main.npc[i];
				if (nPC.CanBeChasedBy(player))
				{
					float num3 = searchCenter.Distance(nPC.Center);
					if (!(num2 <= num3))
					{
						num = i;
						num2 = num3;
					}
				}
			}
			if (!num.HasValue)
			{
				return false;
			}
			npcTargetIndex = num.Value;
			return true;
		}

	}
}
