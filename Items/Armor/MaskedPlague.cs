using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class MaskedPlagueHeadgear : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Masked Plague's Headgear");
			Tooltip.SetDefault("Increases movement speed by 3%\nIncreases magic and ranged crit chance by 5%");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.rare = 2;
			Item.value = Item.sellPrice(silver: 60);
			Item.defense = 5;
		}

		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += 0.03f;
			player.GetCritChance(DamageClass.Magic) += 5f;
			player.GetCritChance(DamageClass.Ranged) += 5f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<MaskedPlagueBreastplate>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Increases max minions by 1\nIncreases magic and ranged weapon speed by 5%\nIncreases minion damage by 5%";
			player.GetAttackSpeed(DamageClass.Magic) += 0.05f;
			player.GetDamage(DamageClass.Summon) += 0.05f;
			player.maxMinions++;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.IronBar, 12);
			recipe.AddIngredient(ItemID.Feather, 16);
			recipe.AddIngredient(ItemID.Silk, 15);
			recipe.AddRecipeGroup("GMR:AnyGem", 3);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 1);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.LeadBar, 12);
			recipe2.AddIngredient(ItemID.Feather, 16);
			recipe2.AddIngredient(ItemID.Silk, 15);
			recipe2.AddRecipeGroup("GMR:AnyGem", 3);
			recipe2.AddIngredient(null, "BossUpgradeCrystal", 1);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class MaskedPlagueBreastplate : ModItem
	{
		public override void Load()
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Legs}", EquipType.Legs, this);
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Masked Plague's Breastplate");
			Tooltip.SetDefault("Increases magic weapon speed by 5%\nIncreases magic and ranged weapon damage by 5%\nDecreases mana costs by 3%");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 22;
			Item.rare = 2;
			Item.value = Item.sellPrice(silver: 60);
			Item.defense = 7;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetAttackSpeed(DamageClass.Magic) += 0.05f;
			player.GetDamage(DamageClass.Magic) += 0.5f;
			player.GetDamage(DamageClass.Ranged) += 0.5f;
			player.manaCost -= 0.03f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.IronBar, 16);
			recipe.AddIngredient(ItemID.Feather, 15);
			recipe.AddIngredient(ItemID.Silk, 10);
			recipe.AddRecipeGroup("GMR:AnyGem", 4);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 1);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.LeadBar, 16);
			recipe2.AddIngredient(ItemID.Feather, 15);
			recipe2.AddIngredient(ItemID.Silk, 10);
			recipe2.AddRecipeGroup("GMR:AnyGem", 4);
			recipe2.AddIngredient(null, "BossUpgradeCrystal", 1);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}
