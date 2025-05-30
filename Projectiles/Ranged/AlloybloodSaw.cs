using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Ranged
{
    public class AlloybloodSaw : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            Projectile.AddElement(0);
            Projectile.AddElement(2);
        }

        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 40;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 900;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            if (Projectile.extraUpdates > 0 || Projectile.extraUpdates < 0)
                Projectile.extraUpdates = 0;
            Projectile.scale = 0.5f;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, new Vector3(0.85f, 0.25f, 0.35f));

            Projectile.rotation += Projectile.direction * 0.02f * Projectile.velocity.Length();
            Projectile.width = (int)(48 * Projectile.scale);
            Projectile.height = (int)(48 * Projectile.scale);

            if (Projectile.scale < 1f)
                Projectile.scale *= 1.05f;

            var target = Projectile.FindTargetWithLineOfSight(500f);
            if (++Projectile.ai[1] % 60 == 0)
            {
                if (Projectile.ai[0] < 5f && target != -1)
                {
                    int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Normalize(Projectile.Center - Main.npc[target].Center) * 2f, Projectile.type, (int)(Projectile.damage / 2f), Projectile.knockBack, Main.myPlayer, 5f);
                    Main.projectile[p].timeLeft = 300;
                }
            }

            if (Projectile.ai[0] == 5f && target != -1)
            {
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Normalize(Main.npc[target].Center - Projectile.Center) * 12f, 0.09f);
                Projectile.tileCollide = false;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 2;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;
            float opacity = Projectile.Opacity / 2;
            var drawOffset = new Vector2(Projectile.width / 2f, Projectile.height / 2f) - Main.screenPosition;

            SpriteEffects spriteEffects = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
            {
                Color color26 = new Color(255, 105, 125, 55) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);

                Color color27 = color26;
                color27.A = 0;
                color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
                Vector2 value4 = Projectile.oldPos[i];
                float num165 = Projectile.oldRot[i];
                Main.EntitySpriteDraw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color27, num165, origin2, Projectile.scale * 1.1f, spriteEffects, 0);
            }

            Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), lightColor, Projectile.rotation, origin2, Projectile.scale, spriteEffects, 0);
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            // Limits the bounce number to 3 times
            if (++Projectile.ai[2] >= 3)
                return true;
            else
                Projectile.velocity *= -1.5f;
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            return false;
        }

        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item110, Projectile.position);
            Projectile.width = Projectile.height = 10;
            Projectile.position.X -= (float)(Projectile.width / 2);
            Projectile.position.Y -= (float)(Projectile.height / 2);
        }
    }
}