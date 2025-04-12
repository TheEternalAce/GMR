using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Melee
{
	public class StarMadnessThrow : ModProjectile
	{
		public override string Texture => "GMR/Items/Weapons/Melee/Swords/StarMadness";

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Projectile.AddElement(1);
		}

		public override void SetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 600;
			Projectile.penetrate = 2;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.extraUpdates = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 15;
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
			Projectile.velocity *= 0.95f;
			Projectile.alpha += 8;
			if (Projectile.alpha >= 255)
            {
				Projectile.Kill();
			}

			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 86, Projectile.velocity.X * 0.75f,
				Projectile.velocity.Y * 0.75f, 60, default(Color), 0.5f);
			Main.dust[dustId].noGravity = true;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			SoundEngine.PlaySound(GMR.GetSounds("Items/Melee/SwordImpact", 3, 0.1f, 0.25f, 0.15f), Projectile.Center);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 origin2 = texture2D13.Size() / 2f;

			Color color26 = new Color(200, 125, 255, 20) * Projectile.Opacity;

			SpriteEffects effects = SpriteEffects.None;

			for (float i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i += 0.5f)
			{
				Color color27 = color26;
				color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
				int max0 = (int)i - 1;//Math.Max((int)i - 1, 0);
				if (max0 < 0)
					continue;
				Vector2 value4 = Vector2.Lerp(Projectile.oldPos[(int)i], Projectile.oldPos[max0], 1 - i % 1);
				Main.EntitySpriteDraw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), null, color27, Projectile.rotation, origin2, Projectile.scale, effects, 0);
			}

			Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, color26, Projectile.rotation, origin2, Projectile.scale, effects, 0);
			return false;
		}

		public override void Kill(int timeLeft)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);

			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 86, Projectile.velocity.X * 0.75f,
				Projectile.velocity.Y * 0.75f, 60, default(Color), 0.5f);
			Main.dust[dustId].noGravity = true;
			int dustId3 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 86, Projectile.velocity.X * 0.75f,
				Projectile.velocity.Y * 0.75f, 60, default(Color), 1f);
			Main.dust[dustId3].noGravity = true;
		}
	}
}