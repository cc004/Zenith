using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Zenith
{
    public class Zenith : ModItem
    {
        public override void SetDefaults()
		{
			item.useStyle = 1;
			item.width = 24;
			item.height = 24;
			item.UseSound = mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Item, "Sounds/Item/Item_169");
			item.autoReuse = true;
			item.melee = true;
			item.shoot = ModContent.ProjectileType<ZenithProjectile>();
			item.useAnimation = 30;
			item.useTime = item.useAnimation / 3;
			item.shootSpeed = 16f;
			item.damage = 190;
			item.knockBack = 6.5f;
			item.value = Item.sellPrice(0, 20, 0, 0);
			item.crit = 10;
			item.rare = 10;
			item.noUseGraphic = true;
			item.noMelee = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zenith");
			Tooltip.SetDefault("123");
		}
		
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(255, 255, 255, lightColor.A - item.alpha);
		}
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 vector = player.RotatedRelativePoint(player.MountedCenter, true);
			float num2 = (float)Main.mouseX + Main.screenPosition.X - vector.X;
			float num3 = (float)Main.mouseY + Main.screenPosition.Y - vector.Y;
			int num161 = (player.itemAnimationMax - player.itemAnimation) / player.itemTime;
			Vector2 vector33 = new Vector2(num2, num3);
			int num162 = FinalFractalHelper.GetRandomProfileIndex();
			if (num161 == 0)
			{
				num162 = 4956;
			}
			Vector2 value14 = Main.MouseWorld - player.MountedCenter;
			if (num161 == 1 || num161 == 2)
			{
				int npcTargetIndex;
				bool zenithTarget = player.GetZenithTarget(Main.MouseWorld, 400f, out npcTargetIndex);
				if (zenithTarget)
				{
					value14 = Main.npc[npcTargetIndex].Center - player.MountedCenter;
				}
				bool flag5 = num161 == 2;
				if (num161 == 1 && !zenithTarget)
				{
					flag5 = true;
				}
				if (flag5)
				{
					value14 += Main.rand.NextVector2Circular(150f, 150f);
				}
			}
			vector33 = value14 / 2f;
			float ai5 = Main.rand.Next(-100, 101);
			var index = Projectile.NewProjectile(vector, vector33, item.shoot, damage, knockBack, player.whoAmI, ai5, num162);
			(Main.projectile[index].modProjectile as ZenithProjectile)._lanceHitboxBounds = new Rectangle(0, 0, 300, 300);
			return false;
		}
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
