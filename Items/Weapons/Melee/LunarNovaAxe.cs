using System;
using Terraria;
using System.IO;
using Terraria.ID;
using Terraria.Audio;
using ReLogic.Content;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using System.Collections.Generic;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework.Graphics;

namespace GMR.Items.Weapons.Melee
{
	public class LunarNovaAxe : ModItem
	{
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Hitting enemies causes a small burst that hits a second time\n[c/DD1166:--Special Melee Weapon--]");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1; //Count of items to research
		}

        public override void SetDefaults()
        {
            Item.DefaultToDopeSword<Projectiles.Melee.CoolSwords.LunarNovaAxe>(30);
            Item.useTime /= 2;
            Item.SetWeaponValues(40, 2f, 4);
            Item.width = 60;
            Item.height = 52;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.axe = 40;
            Item.tileBoost = 3;
            Item.value = Item.sellPrice(silver: 120);
            Item.rare = 4;
            Item.autoReuse = true;
            Item.reuseDelay = 2;
        }

		public override bool MeleePrefix()
        {
            return true;
        }

        public override bool CanShoot(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] <= 0;
        }
    }
}