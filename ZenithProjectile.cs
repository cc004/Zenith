using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;

namespace Zenith
{
    public class ZenithProjectile : ModProjectile
    {
		public override bool ShouldUpdatePosition()
		{
			return false;
		}

		public override void SetDefaults()
		{
			projectile.width = 32;
			projectile.height = 32;
			projectile.aiStyle = 182;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.alpha = 255;
			projectile.extraUpdates = 1;
			projectile.usesLocalNPCImmunity = true;
			projectile.manualDirectionChange = true;
			projectile.localNPCHitCooldown = 15;
			projectile.penetrate = -1;
			projectile.noEnchantments = true;

			ProjectileID.Sets.TrailingMode[projectile.type] = 4;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 60;
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
		}

		public Rectangle _lanceHitboxBounds;

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			Vector2 mountedCenter = player.MountedCenter;
			float lerpValue = Utils.GetLerpValue(900f, 0f, projectile.velocity.Length() * 2f, true);
			float num = MathHelper.Lerp(0.7f, 2f, lerpValue);
			projectile.localAI[0] += num;
			if (projectile.localAI[0] >= 120f)
			{
				projectile.Kill();
				return;
			}
			float lerpValue2 = Utils.GetLerpValue(0f, 1f, projectile.localAI[0] / 60f, true);
			float num2 = projectile.localAI[0] / 60f;
			float num3 = projectile.ai[0];
			float num4 = projectile.velocity.ToRotation();
			float num5 = 3.1415927f;
			float num6 = (float)((projectile.velocity.X > 0f) ? 1 : -1);
			float num7 = num5 + num6 * lerpValue2 * 6.2831855f;
			float num8 = projectile.velocity.Length() + Utils.GetLerpValue(0.5f, 1f, lerpValue2, true) * 40f;
			float num9 = 60f;
			if (num8 < num9)
			{
				num8 = num9;
			}
			Vector2 value = mountedCenter + projectile.velocity;
			Vector2 spinningpoint = new Vector2(1f, 0f).RotatedBy((double)num7, default(Vector2)) * new Vector2(num8, num3 * MathHelper.Lerp(2f, 1f, lerpValue));
			Vector2 value2 = value + spinningpoint.RotatedBy((double)num4, default(Vector2));
			Vector2 value3 = (1f - Utils.GetLerpValue(0f, 0.5f, lerpValue2, true)) * new Vector2((float)((projectile.velocity.X > 0f) ? 1 : -1) * -num8 * 0.1f, -projectile.ai[0] * 0.3f);
			float num10 = num7 + num4;
			projectile.rotation = num10 + 1.5707964f;
			projectile.Center = value2 + value3;
			projectile.spriteDirection = (projectile.direction = ((projectile.velocity.X > 0f) ? 1 : -1));
			if (num3 < 0f)
			{
				projectile.rotation = num5 + num6 * lerpValue2 * -6.2831855f + num4;
				projectile.rotation += 1.5707964f;
				projectile.spriteDirection = (projectile.direction = ((projectile.velocity.X > 0f) ? -1 : 1));
			}
			if (num2 < 1f)
			{
				FinalFractalHelper.FinalFractalProfile finalFractalProfile = FinalFractalHelper.GetFinalFractalProfile((int)projectile.ai[1]);
				Vector2 value4 = (projectile.rotation - 1.5707964f).ToRotationVector2();
				Vector2 center = projectile.Center;
				int num11 = 1 + (int)(projectile.velocity.Length() / 100f);
				num11 = (int)((float)num11 * Utils.GetLerpValue(0f, 0.5f, lerpValue2, true) * Utils.GetLerpValue(1f, 0.5f, lerpValue2, true));
				if (num11 < 1)
				{
					num11 = 1;
				}
				for (int i = 0; i < num11; i++)
				{
					finalFractalProfile.dustMethod(center + value4 * finalFractalProfile.trailWidth * MathHelper.Lerp(0.5f, 1f, Main.rand.NextFloat()), projectile.rotation - 1.5707964f + 1.5707964f * (float)projectile.spriteDirection, player.velocity);
				}
				Vector3 vector = finalFractalProfile.trailColor.ToVector3();
				Vector3 value5 = Vector3.Lerp(Vector3.One, vector, 0.7f);
				Lighting.AddLight(projectile.Center, vector * 0.5f * projectile.Opacity);
				Lighting.AddLight(mountedCenter, value5 * projectile.Opacity * 0.15f);
			}
			projectile.Opacity = Utils.GetLerpValue(0f, 5f, projectile.localAI[0], true) * Utils.GetLerpValue(120f, 115f, projectile.localAI[0], true);
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float collisionPoint = 0f;
			float scaleFactor = 40f;
			for (int i = 14; i < projectile.oldPos.Length; i += 15)
			{
				float num2 = projectile.localAI[0] - (float)i;
				if (!(num2 < 0f) && !(num2 > 60f))
				{
					Vector2 value2 = projectile.oldPos[i] + projectile.Size / 2f;
					Vector2 value3 = (projectile.oldRot[i] + (float)Math.PI / 2f).ToRotationVector2();
					_lanceHitboxBounds.X = (int)value2.X - _lanceHitboxBounds.Width / 2;
					_lanceHitboxBounds.Y = (int)value2.Y - _lanceHitboxBounds.Height / 2;
					if (_lanceHitboxBounds.Intersects(targetHitbox) && Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), value2 - value3 * scaleFactor, value2 + value3 * scaleFactor, 20f, ref collisionPoint))
					{
						return true;
					}
				}
			}
			Vector2 value4 = (projectile.rotation + (float)Math.PI / 2f).ToRotationVector2();
			_lanceHitboxBounds.X = (int)projectile.position.X - _lanceHitboxBounds.Width / 2;
			_lanceHitboxBounds.Y = (int)projectile.position.Y - _lanceHitboxBounds.Height / 2;
			if (_lanceHitboxBounds.Intersects(targetHitbox) && Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center - value4 * scaleFactor, projectile.Center + value4 * scaleFactor, 20f, ref collisionPoint))
			{
				return true;
			}
			return false;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White * projectile.Opacity;
		}

		private static void DrawPrettyStarSparkle(Projectile proj, SpriteEffects dir, Vector2 drawpos, Microsoft.Xna.Framework.Color drawColor, Microsoft.Xna.Framework.Color shineColor)
		{
			Texture2D value = ModContainer.Instance.GetTexture("Extra_98");
			Microsoft.Xna.Framework.Color color = shineColor * proj.Opacity * 0.5f;
			color.A = 0;
			Vector2 origin = value.Size() / 2f;
			Microsoft.Xna.Framework.Color color2 = drawColor * 0.5f;
			float num = Utils.GetLerpValue(15f, 30f, proj.localAI[0], clamped: true) * Utils.GetLerpValue(45f, 30f, proj.localAI[0], clamped: true);
			Vector2 vector = new Vector2(0.5f, 5f) * num;
			Vector2 vector2 = new Vector2(0.5f, 2f) * num;
			color *= num;
			color2 *= num;
			Main.spriteBatch.Draw(value, drawpos, null, color, (float)Math.PI / 2f, origin, vector, dir, 0);
			Main.spriteBatch.Draw(value, drawpos, null, color, 0f, origin, vector2, dir, 0);
			Main.spriteBatch.Draw(value, drawpos, null, color2, (float)Math.PI / 2f, origin, vector * 0.6f, dir, 0);
			Main.spriteBatch.Draw(value, drawpos, null, color2, 0f, origin, vector2 * 0.6f, dir, 0);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 vector3 = Main.player[projectile.owner].position - Main.player[projectile.owner].oldPosition;
			for (int num29 = projectile.oldPos.Length - 1; num29 > 0; num29--)
			{
				projectile.oldPos[num29] = projectile.oldPos[num29 - 1];
				projectile.oldRot[num29] = projectile.oldRot[num29 - 1];
				projectile.oldSpriteDirection[num29] = projectile.oldSpriteDirection[num29 - 1];
				if (projectile.numUpdates == 0 && projectile.oldPos[num29] != Vector2.Zero)
				{
					projectile.oldPos[num29] += vector3;
				}
			}
			projectile.oldPos[0] = projectile.position;
			projectile.oldRot[0] = projectile.rotation;
			projectile.oldSpriteDirection[0] = projectile.spriteDirection;


			default(FinalFractalHelper).Draw(projectile);


			Texture2D value13 = Main.projectileTexture[projectile.type];
			int num143 = value13.Height / Main.projFrames[projectile.type];
			int y12 = num143 * projectile.frame;
			Rectangle rectangle3 = new Rectangle(0, y12, value13.Width, num143);
			Vector2 origin4 = rectangle3.Size() / 2f;
			Vector2 zero = Vector2.Zero;
			float num144 = 0f;


			int num145 = (int)projectile.ai[1];
			if (Main.itemTexture.IndexInRange(num145))
			{
				value13 = Main.itemTexture[num145];
				rectangle3 = value13.Frame();
				origin4 = rectangle3.Size() / 2f;
				num144 = -(float)Math.PI / 4f * (float)projectile.spriteDirection;
			}

			float num150 = 0f;
			Rectangle value15 = rectangle3;

			int num148 = 60;
			float num149 = 60f;
			int num146 = 0;
			int num147 = -15;
			float value14 = 1f;


			Vector2 mountedCenter = Main.player[projectile.owner].MountedCenter;
			Color color29 = Lighting.GetColor((int)((double)projectile.position.X + (double)projectile.width * 0.5) / 16, (int)(((double)projectile.position.Y + (double)projectile.height * 0.5) / 16.0));
			if (projectile.hide && !ProjectileID.Sets.DontAttachHideToAlpha[projectile.type])
			{
				color29 = Lighting.GetColor((int)mountedCenter.X / 16, (int)(mountedCenter.Y / 16f));
			}
			if (projectile.type == 14)
			{
				color29 = Color.White;
			}


			SpriteEffects spriteEffects = SpriteEffects.None;
			if (projectile.spriteDirection == -1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
			if (projectile.type == 681 && projectile.velocity.X > 0f)
			{
				spriteEffects ^= SpriteEffects.FlipHorizontally;
			}

			for (int num151 = num148; (num147 > 0 && num151 < num146) || (num147 < 0 && num151 > num146); num151 += num147)
			{
				if (num151 >= projectile.oldPos.Length)
				{
					continue;
				}
				Color color30 = color29;
				color30 = projectile.GetAlpha(color30);
				float num156 = num146 - num151;
				if (num147 < 0)
				{
					num156 = num148 - num151;
				}
				color30 *= num156 / ((float)ProjectileID.Sets.TrailCacheLength[projectile.type] * 1.5f);
				Vector2 value17 = projectile.oldPos[num151];
				float num157 = projectile.rotation;
				SpriteEffects effects2 = spriteEffects;
				if (ProjectileID.Sets.TrailingMode[projectile.type] == 2 || ProjectileID.Sets.TrailingMode[projectile.type] == 3 || ProjectileID.Sets.TrailingMode[projectile.type] == 4)
				{
					num157 = projectile.oldRot[num151];
					effects2 = ((projectile.oldSpriteDirection[num151] == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
				}
				if (value17 == Vector2.Zero)
				{
					continue;
				}

				float t = projectile.localAI[0] - (float)num151;
				float scale3 = Utils.GetLerpValue(0f, 20f, t, clamped: true) * Utils.GetLerpValue(68f, 60f, t, clamped: true);
				float lerpValue3 = Utils.GetLerpValue(0f, ProjectileID.Sets.TrailCacheLength[projectile.type], num156, clamped: true);
				color30 = Color.White * lerpValue3 * projectile.Opacity * scale3;

				Vector2 position2 = value17 + zero + projectile.Size / 2f - Main.screenPosition + new Vector2(0f, projectile.gfxOffY);
				Main.spriteBatch.Draw(value13, position2, value15, color30, num157 + num144 + projectile.rotation * num150 * (float)(num151 - 1) * (float)(-spriteEffects.HasFlag(SpriteEffects.FlipHorizontally).ToDirectionInt()), origin4, MathHelper.Lerp(projectile.scale, value14, (float)num151 / num149), effects2, 0);
			}


			float num169 = projectile.scale;
			float rotation23 = projectile.rotation + num144;

			float t3 = projectile.localAI[0];
			float scale5 = Utils.GetLerpValue(0f, 20f, t3, clamped: true) * Utils.GetLerpValue(68f, 60f, t3, clamped: true);
			Main.spriteBatch.Draw(value13, projectile.Center + zero - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), rectangle3, new Microsoft.Xna.Framework.Color(255, 255, 255, 127) * projectile.Opacity * scale5, rotation23, origin4, num169 * 1.25f, spriteEffects, 0);
			FinalFractalHelper.FinalFractalProfile finalFractalProfile = FinalFractalHelper.GetFinalFractalProfile((int)projectile.ai[1]);
			Microsoft.Xna.Framework.Color trailColor = finalFractalProfile.trailColor;
			trailColor.A /= 2;
			DrawPrettyStarSparkle(projectile, spriteEffects, projectile.Center + zero - Main.screenPosition + new Vector2(0f, projectile.gfxOffY) + (projectile.rotation - (float)Math.PI / 2f).ToRotationVector2() * finalFractalProfile.trailWidth, Microsoft.Xna.Framework.Color.White * scale5, trailColor * scale5);

			return false;
		}
	}
}
