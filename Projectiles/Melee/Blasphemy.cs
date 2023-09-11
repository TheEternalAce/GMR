using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Melee
{
	public class Blasphemy : ModProjectile
	{
		public override string Texture => "GMR/Projectiles/Melee/SwordSlash";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blasphemy");
		}

		public override void SetDefaults()
		{
			Projectile.width = 124;
			Projectile.height = 124;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 60;
			Projectile.extraUpdates = 1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.localNPCHitCooldown = 30;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.ownerHitCheck = true;
			Projectile.usesOwnerMeleeHitCD = true;
			Projectile.scale = 1.15f;
			Projectile.alpha = 125;
		}

		private float Angle;
		private bool DirectionOnSpawn;
		private int Direction;
		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, new Vector3(0.95f, 0.25f, 0.65f));

			Player player = Main.player[Projectile.owner];

			// Get direction on spawn so it's not changed
			if (!DirectionOnSpawn)
			{
				Direction = player.direction;
				DirectionOnSpawn = true;
			}
			Projectile.direction = Direction;
			player.direction = Direction;

			float attackBuffs = (player.GetAttackSpeed(DamageClass.Melee) + player.GetAttackSpeed(DamageClass.Generic)) * 0.75f;

			int duration = (int)(29 * 2 / attackBuffs); // Define the duration the projectile will exist in frames

			// Reset projectile time left if necessary
			if (Projectile.timeLeft > duration)
			{
				Projectile.timeLeft = duration;
			}

			if (player.gravDir == -1)
				Angle += -0.005f * 8 * attackBuffs;
			else
				Angle += 0.005f * 8 * attackBuffs;

			// ONLY CHANGE THE 40 TO FIT YOUR SPRITE, ANYTHING ELSE WILL BREAK THE ROTATION
			Projectile.Center = player.MountedCenter + Vector2.One.RotatedBy(Direction * Angle + (Direction == 1 ? MathHelper.ToRadians(135f) : MathHelper.ToRadians(-45f))) * 45f;

			Vector2 toPlayer = player.Center - Projectile.Center;
			Projectile.rotation = toPlayer.ToRotation() - MathHelper.ToRadians(90f);

			Projectile.scale += 0.001f * attackBuffs;

			if (Projectile.alpha > 0 && Projectile.timeLeft > duration * 0.5)
				Projectile.alpha -= 16;

			if (Projectile.timeLeft < duration * 0.25)
				Projectile.alpha += 16;

			if (player.dead || player.ItemAnimationActive && player.HeldItem.type != ModContent.ItemType<Items.Weapons.Melee.Blasphemy>()
				&& player.ownedProjectileCounts[Projectile.type] == 1)
			{
				Projectile.Kill();
				return;
			}
			player.heldProj = Projectile.whoAmI;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.Devilish>(), 120);
		}

		public override void Kill(int timeleft)
		{
			Player player = Main.player[Projectile.owner];
			Vector2 spawnPos = player.MountedCenter;
			Vector2 speed = Main.MouseWorld - player.Center;
			if (speed.Length() < 800)
				speed = Vector2.Normalize(speed) * 800;
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.MountedCenter, Vector2.Normalize(speed) * 6f, ModContent.ProjectileType<BlasphemyCut>(),
				Projectile.damage, Projectile.knockBack, Main.myPlayer);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Player player = Main.player[Projectile.owner];
			Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
			int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
			Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
			Vector2 origin2 = rectangle.Size() / 2f;
			var opacity = Projectile.Opacity;

			Color color26 = new Color(125, 5, 55) * opacity;

            SpriteEffects spriteEffects = Projectile.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			if (player.gravDir == -1)
				spriteEffects = Projectile.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color26,
				Projectile.rotation, origin2, Projectile.scale, spriteEffects, 0);
			Main.EntitySpriteDraw(ModContent.Request<Texture2D>($"GMR/Projectiles/Melee/SwordSlashLight", AssetRequestMode.ImmediateLoad).Value, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
				new Microsoft.Xna.Framework.Rectangle?(rectangle), new Color(225, 105, 155) * opacity, Projectile.rotation, origin2, Projectile.scale, spriteEffects, 0);
			return false;
		}
	}
}