using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles
{
    public class SmallExplosion : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_85";

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 7;
            Projectile.AddElement(0);
        }

        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 30;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.localNPCHitCooldown = 0;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.scale = 0.5f;
        }

        public override void AI()
        {
            Projectile.velocity = Vector2.Zero;

            for (int i = 0; i < 10; i++)
            {
                Dust dustId = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, Main.rand.NextFloat(-12f, 12f), Main.rand.NextFloat(-12f, 12f), 60, default(Color), 1f);
                dustId.noGravity = true;
            }

            Projectile.scale += 0.025f;
            Projectile.alpha += 8;
            Projectile.rotation += MathHelper.ToRadians(Main.rand.NextFloat(-1f, 1f));

            if (++Projectile.frameCounter > 2)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame--;
                    Projectile.Kill();
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire3, 600);
            target.immune[Projectile.owner] = 1;
            int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6, Projectile.velocity.X * 0.7f,
                Projectile.velocity.Y * 0.4f, 120, default(Color), 2f);
            Main.dust[dustId].noGravity = true;
            int dustId3 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6, Projectile.velocity.X * -0.7f,
                Projectile.velocity.Y * 0.4f, 120, default(Color), 2f);
            Main.dust[dustId3].noGravity = true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;

            Color color26 = new Color(255, 255, 85, 80);

            SpriteEffects spriteEffects = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Color color27 = color26;
            color27.G -= 130;
            Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                new Microsoft.Xna.Framework.Rectangle?(rectangle), color27 * Projectile.Opacity, Projectile.rotation, origin2, Projectile.scale * 1.05f, spriteEffects, 0);

            Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                new Microsoft.Xna.Framework.Rectangle?(rectangle), color26 * 0.8f * Projectile.Opacity, Projectile.rotation + MathHelper.ToRadians(15f), origin2, Projectile.scale, spriteEffects, 0);

            Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                new Microsoft.Xna.Framework.Rectangle?(rectangle), color26 * Projectile.Opacity, Projectile.rotation - MathHelper.ToRadians(15f), origin2, Projectile.scale * 0.8f, spriteEffects, 0);
            return false;
        }
    }
}