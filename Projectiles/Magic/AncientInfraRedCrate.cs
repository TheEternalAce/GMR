using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Magic
{
	public class AncientInfraRedCrate : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Projectile.AddElement(0);
			Projectile.AddElement(2);
		}

		public override void SetDefaults()
		{
			Projectile.width = 34;
			Projectile.height = 34;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 2400;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.extraUpdates = 4;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
		}

		float vScale;
		public override void AI()
		{
			if (++Projectile.localAI[0] > 60)
			{
				Projectile.velocity.Y = 0f;
				Projectile.velocity.X = 0f;
				Projectile.rotation = 0f + MathHelper.ToRadians(-90f);
			}

			Projectile.rotation = Projectile.velocity.ToRotation();
			Projectile.scale = 1f + vScale;

			if (++Projectile.ai[0] % 300 == 0)
			{
				for (int y = 0; y < 4; y++)
				{
					Vector2 projDirection = (1f * Vector2.UnitX).RotatedBy(MathHelper.ToRadians(90f * y)) * 6f;
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, projDirection, ModContent.ProjectileType<JackBlast>(), Projectile.damage, 0f, Main.myPlayer);
				}
				vScale = 0.5f;

				int dustType = 60;
				for (int i = 0; i < 5; i++)
				{
					Vector2 velocity = Projectile.velocity + new Vector2(Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f));
					Dust dust = Dust.NewDustPerfect(Projectile.Center, dustType, velocity, 120, Color.White, Main.rand.NextFloat(1.6f, 3.8f));

					dust.noLight = false;
					dust.noGravity = true;
					dust.fadeIn = Main.rand.NextFloat(0.1f, 0.5f);
				}
			}
			if (vScale > 0f)
				vScale -= 0.005f;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity * 0.75f, ModContent.ProjectileType<Projectiles.InfraRedExplosion>(), damageDone / 4, 0f, Main.myPlayer);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale * vScale, SpriteEffects.None, 0);
			}
			return true;
		}

		public override void Kill(int timeLeft)
		{
			// This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item62, Projectile.position);

			int dustType = 60;
			for (int i = 0; i < 5; i++)
			{
				Vector2 velocity = Projectile.velocity + new Vector2(Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f));
				Dust dust = Dust.NewDustPerfect(Projectile.Center, dustType, velocity, 120, Color.White, Main.rand.NextFloat(1.6f, 3.8f));

				dust.noLight = false;
				dust.noGravity = true;
				dust.fadeIn = Main.rand.NextFloat(0.1f, 0.5f);
			}
		}
	}

	public class JackBlast : Bosses.JackBlastBad
	{
		public override string Texture => "GMR/Projectiles/Bosses/JackBlastBad";

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
		}
	}
}