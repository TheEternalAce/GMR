using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace GMR.NPCs.Bosses.Jack
{
    public class JackArmGun : ModNPC
    {
        public int ParentIndex
        {
            get => (int)NPC.ai[1] - 1;
            set => NPC.ai[1] = value + 1;
        }

        public bool HasParent => ParentIndex > -1;

        // Helper method to determine the body type
        public static int BodyType()
        {
                return ModContent.NPCType<Jack>();
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Jack Cannon");
            NPCID.Sets.DontDoHardmodeScaling[Type] = true;
            NPCID.Sets.CantTakeLunchMoney[Type] = true;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.width = 34;
            NPC.height = 74;
            NPC.lifeMax = 500;
            NPC.defense = 2;
            NPC.HitSound = SoundID.NPCHit42;
            NPC.DeathSound = SoundID.NPCDeath37;
            NPC.knockBackResist = 0.5f;
            NPC.damage = 15;
            NPC.aiStyle = -1;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.value = Item.buyPrice(gold: 0);
            NPC.npcSlots = 1f;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = ImmunityCooldownID.Bosses; // use the boss immunity cooldown counter, to prevent ignoring boss attacks by taking damage from other sources
            return false; // Set to false because fuck contact damage
        }

        public override void AI()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (!NPC.AnyNPCs(ModContent.NPCType<Jack>()))
                {
                    NPC.life += -50;

                    if (NPC.life <= 0)
                    {
                        int dustType = 60;
                        for (int i = 0; i < 20; i++)
                        {
                            Vector2 velocity = NPC.velocity + new Vector2(Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f));
                            Dust dust = Dust.NewDustPerfect(NPC.Center, dustType, velocity, 30, Color.White, Main.rand.NextFloat(1.6f, 3.8f));

                            dust.noLight = false;
                            dust.noGravity = true;
                            dust.fadeIn = Main.rand.NextFloat(0.1f, 0.5f);
                        }
                        SoundEngine.PlaySound(SoundID.Item62, NPC.Center);
                    }
                    NPC.netUpdate = true;
                    return;
                }

                if (NPC.damage > 30)
                    NPC.damage = 30;

                Player player = Main.player[NPC.target];
                if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
                {
                    NPC.TargetClosest();
                    NPC.netUpdate = true;
                }

                Vector2 toPlayer = NPC.Center - player.Center;
                NPC.rotation = toPlayer.ToRotation() + MathHelper.ToRadians(90f);

                if (!Main.dayTime || player.dead)
                {
                    NPC.netUpdate = true;
                    NPC.velocity.Y += 0.5f;
                    NPC.EncourageDespawn(300);
                    return;
                }
                else if (NPC.ai[2] > 0)
                {
                    NPC.netUpdate = true;
                    NPC.spriteDirection = 1;

                    if (NPC.ai[0] < 0)
                    {
                        Vector2 bossToPlayer = Main.player[NPC.target].Center + 350 * Vector2.UnitX + 300 * Vector2.UnitY;
                        NPC.velocity = (bossToPlayer - NPC.Center) * 0.075f;
                    }
                    else
                    {
                        Vector2 bossToPlayer = Main.player[NPC.target].Center + 350 * Vector2.UnitX - 300 * Vector2.UnitY;
                        NPC.velocity = (bossToPlayer - NPC.Center) * 0.075f;
                    }
                }
                else if (NPC.ai[2] < 0)
                {
                    NPC.netUpdate = true;
                    NPC.spriteDirection = -1;

                    if (NPC.ai[0] < 0)
                    {
                        Vector2 bossToPlayer = Main.player[NPC.target].Center - 350 * Vector2.UnitX + 300 * Vector2.UnitY;
                        NPC.velocity = (bossToPlayer - NPC.Center) * 0.075f;
                    }
                    else
                    {
                        Vector2 bossToPlayer = Main.player[NPC.target].Center - 350 * Vector2.UnitX - 300 * Vector2.UnitY;
                        NPC.velocity = (bossToPlayer - NPC.Center) * 0.075f;
                    }
                }

                if (Main.netMode != NetmodeID.MultiplayerClient)
                    if (++NPC.ai[1] > 241) // After 4 seconds plus one tick, reset
                    {
                        NPC.ai[3] = 0;
                        NPC.ai[1] = 0;
                        NPC.ai[0]++;
                        NPC.ai[0] *= -1;
                        NPC.netUpdate = true;
                    }
                    else if (++NPC.ai[1] > 240 && !Main.getGoodWorld) // After 4 seconds
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(player.Center) * 7f, ModContent.ProjectileType<Projectiles.Bosses.AlloyCrate>(), NPC.damage, 1f, Main.myPlayer, NPC.whoAmI);
                        SoundEngine.PlaySound(SoundID.Research, NPC.Center);
                        NPC.netUpdate = true;
                    }
                    else if (++NPC.ai[1] > 240) // After 4 seconds and FTW
                    {
                        float numberProjectiles = 2;
                        float rotation = MathHelper.ToRadians(25);
                        Vector2 velocity = NPC.DirectionTo(player.Center) * 6f;
                        for (int i = 0; i < numberProjectiles; i++)
                        {
                            Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 1f;
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, perturbedSpeed, ModContent.ProjectileType<Projectiles.Bosses.JackBlastBad>(), NPC.damage, 1f, Main.myPlayer, NPC.whoAmI);
                        }
                        SoundEngine.PlaySound(SoundID.Research, NPC.Center);
                        NPC.netUpdate = true;
                    }
            }
        }
    }
}