using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using GMR;

namespace GMR.Items.Accessories
{
	public class Halu : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 26;
			Item.value = Item.sellPrice(silver: 500);
			Item.rare = 10;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.aggro += 100;
			player.aggro *= 5;
			player.GPlayer().Halu = Item;
			player.GetDamage(DamageClass.Generic) += 0.20f;
			player.GetAttackSpeed(DamageClass.Generic) += 0.20f;
			player.GetKnockback(DamageClass.Generic) += 0.20f;
			player.GetCritChance(DamageClass.Generic) += 20f;
			player.GetArmorPenetration(DamageClass.Generic) += 20;
		}
	}
}