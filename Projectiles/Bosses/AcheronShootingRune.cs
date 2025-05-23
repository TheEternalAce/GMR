using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Bosses
{
	public class AcheronShootingRune : ModProjectile
	{
		public override string Texture => "GMR/Empty";

		public override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.aiStyle = -1;
			Projectile.hostile = false;
			Projectile.timeLeft = 360;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.scale = 0.25f;
		}

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, new Vector3(0.8f, 0.15f, 0.5f));

			Player player = Main.player[Projectile.owner];

			if (Projectile.scale < 0.5f)
				Projectile.scale += 0.01f;

			if (Projectile.timeLeft > 90)
				Projectile.Center = player.Center;
			else
				Projectile.Center = Projectile.Center;

			if (Projectile.timeLeft == 1)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<Projectiles.Bosses.AcheronBeam>(), Projectile.damage * 2, Projectile.knockBack, Main.myPlayer);
				SoundEngine.PlaySound(GMR.GetSounds("Items/Ranged/railgunVariant", 2, 1f, 0f, 0.75f), Projectile.Center);
			}
		}

		float runeRotate;
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = GMR.Instance.Assets.Request<Texture2D>("Assets/Images/JackRitual", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			var offset = Projectile.Size / 2f - Main.screenPosition;
			runeRotate += 0.05f;
			Color color26 = new Color(255, 55, 85, 5);

			Main.EntitySpriteDraw(texture, Projectile.position + offset, null, color26, runeRotate, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, Projectile.position + offset, null, color26, runeRotate, drawOrigin, Projectile.scale * 0.65f, SpriteEffects.None, 0);
			return false;
		}
	}
}