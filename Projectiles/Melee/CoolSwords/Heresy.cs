using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Melee.CoolSwords
{
    public class Heresy : CoolSwordBase
    {
        public override string Texture => "GMR/Items/Weapons/Melee/Swords/Heresy";

        public override void SetStaticDefaults()
        {
            Projectile.AddElement(0);
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 68;
            Projectile.height = 68;
            Projectile.ownerHitCheck = true;
            hitboxOutwards = 68;
            rotationOffset = -MathHelper.PiOver4 * 3f;
            Projectile.extraUpdates = 4;
        }

        protected override void Initialize(Player player, GerdPlayer gerd)
        {
            base.Initialize(player, gerd);
            if (gerd.itemCombo > 0)
            {
                swingDirection *= -1;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void AI()
        {
            base.AI();
            Player player = Main.player[Projectile.owner];
            if (Main.player[Projectile.owner].itemAnimation <= 1)
            {
                player.GPlayer().itemCombo = (ushort)(combo == 0 ? 20 : 0);
            }
            if (!playedSound && AnimProgress > 0.4f)
            {
                playedSound = true;
                SoundEngine.PlaySound(GMR.GetSounds("Items/Melee/swordSwoosh", 7, 0.66f, 0f, 0.2f).WithPitchOffset(0.25f), Projectile.Center);
            }
        }

        public override void UpdateSwing(float progress, float interpolatedSwingProgress)
        {
            Player player = Main.player[Projectile.owner];
            GerdPlayer modPlayer = player.GetModPlayer<GerdPlayer>();
            if (progress == 0.5f && Main.myPlayer == Projectile.owner)
            {
                Vector2 speed = AngleVector * Projectile.velocity.Length();

                float numberProjectiles = 3;
                float rotation = MathHelper.ToRadians(15f);
                for (int i = 0; i < numberProjectiles; i++)
                {
                    Vector2 perturbedSpeed = speed.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 24f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.MountedCenter, perturbedSpeed, ModContent.ProjectileType<Projectiles.Melee.BlasphemyCut>(),
                        (int)(Projectile.damage * 0.3f), Projectile.knockBack / 2f, Main.myPlayer);
                }

                if (modPlayer.InfraRedSet != null)
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, AngleVector * Projectile.velocity.Length() * 9f, ModContent.ProjectileType<Projectiles.Melee.JackSwordThrow>(),
                        (int)(Projectile.damage * 0.75f), Projectile.knockBack / 4f, Projectile.owner);
            }
        }

        public override Vector2 GetOffsetVector(float progress)
        {
            Player player = Main.player[Projectile.owner];
            if (progress < 0.5f)
                return base.GetOffsetVector(progress);
            return BaseAngleVector.RotatedBy((progress * (MathHelper.Pi * 1.5f) - MathHelper.PiOver2 * 1.5f) * -swingDirection * (0.8f + 0.2f * Math.Min(player.GPlayer().itemUsage / 300f, 1f)));
        }

        public override float SwingProgress(float progress)
        {
            return GenericSwing2(progress);
        }
        public override float GetScale(float progress)
        {
            float scale = base.GetScale(progress);
            if (progress > 0.4f && progress < 0.6f)
            {
                return scale;
            }
            return scale;
        }
        public override float GetVisualOuter(float progress, float swingProgress)
        {
            if (progress > 0.8f)
            {
                float p = 1f - (1f - progress) / 0.2f;
                Projectile.alpha = (int)(p * 255);
                return -10f * p;
            }
            if (progress < 0.35f)
            {
                float p = 1f - (progress) / 0.35f;
                Projectile.alpha = (int)(p * 255);
                return -10f * p;
            }
            return 0f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.Devilish>(), 600);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            var texture = TextureAssets.Projectile[Type].Value;
            var center = Main.player[Projectile.owner].Center;
            var handPosition = Main.GetPlayerArmPosition(Projectile) + AngleVector * visualOutwards;
            var drawColor = Projectile.GetAlpha(lightColor) * Projectile.Opacity;
            var drawCoords = handPosition - Main.screenPosition;
            float size = texture.Size().Length();
            var effects = SpriteEffects.None;
            var origin = new Vector2(0f, texture.Height);

            foreach (var v in GerdHelper.CircularVector(4, Main.GlobalTimeWrappedHourly))
            {
                Main.EntitySpriteDraw(texture, drawCoords + v * 2f * Projectile.scale, null, new Color(125, 5, 55) * Projectile.Opacity, Projectile.rotation, origin, Projectile.scale, effects, 0);
            }

            Main.EntitySpriteDraw(texture, handPosition - Main.screenPosition, null, drawColor, Projectile.rotation, origin, Projectile.scale, effects, 0);

            if (AnimProgress > 0.45f && AnimProgress < 0.65f)
            {
                float intensity = (float)Math.Sin((AnimProgress - 0.25f) * 2f * MathHelper.Pi);
                Main.EntitySpriteDraw(texture, handPosition - Main.screenPosition, null, drawColor.UseA(0) * intensity, Projectile.rotation, origin, Projectile.scale, effects, 0);

                Main.instance.LoadProjectile(ProjectileID.RainbowCrystalExplosion);
                var shine = TextureAssets.Projectile[ProjectileID.RainbowCrystalExplosion].Value;
                var shineOrigin = shine.Size() / 2f;
                var shineColor = new Color(225, 105, 155) * intensity * intensity * Projectile.Opacity;
                var shineLocation = handPosition - Main.screenPosition + (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * ((size - 8f) * Projectile.scale);
                Main.EntitySpriteDraw(shine, shineLocation, null, shineColor, 0f, shineOrigin, new Vector2(Projectile.scale * 0.5f, Projectile.scale) * intensity, effects, 0);
                Main.EntitySpriteDraw(shine, shineLocation, null, shineColor, MathHelper.PiOver2, shineOrigin, new Vector2(Projectile.scale * 0.5f, Projectile.scale * 2f) * intensity, effects, 0);
            }

            if (AnimProgress > 0.10f && AnimProgress < 0.90f)
            {
                float intensity = (float)Math.Sin((AnimProgress - 0.35f) / 0.4f * MathHelper.Pi);
                Main.EntitySpriteDraw(texture, handPosition - Main.screenPosition, null, drawColor.UseA(0) * intensity * 0.5f, Projectile.rotation, origin, Projectile.scale, effects, 0);
                Player player = Main.player[Projectile.owner];
                effects = player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

                var swish = GMR.Instance.Assets.Request<Texture2D>("Projectiles/Melee/SwordSlash", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                var swishOrigin = swish.Size() / 2;
                var swishColor = new Color(125, 5, 55) * intensity * intensity * Projectile.Opacity * 0.5f;
                float r = BaseAngleVector.ToRotation() + ((AnimProgress - 0.45f) / 0.2f * 2f - 1f) * -swingDirection * 0.6f;
                var swishLocation = Main.player[Projectile.owner].Center - Main.screenPosition + r.ToRotationVector2() * (size - 20f) * scale;
                Main.EntitySpriteDraw(swish, swishLocation, null, swishColor, r + MathHelper.PiOver2, swishOrigin, Projectile.scale, effects, 0);
                Main.EntitySpriteDraw(swish, swishLocation, null, swishColor * 0.4f, r + MathHelper.PiOver2, swishOrigin, Projectile.scale * 1.1f, effects, 0);
            }
            return false;
        }
    }
}