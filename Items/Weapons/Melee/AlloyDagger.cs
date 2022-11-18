using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee
{
	public class AlloyDagger : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alloy Metal Dagger");
			Tooltip.SetDefault("Throws a dagger at enemies");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.rare = 2;
			Item.useTime = 16;
			Item.useAnimation = 16;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 50);
			Item.autoReuse = false;
			Item.UseSound = SoundID.Item7;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 18;
			Item.crit = 4;
			Item.knockBack = 1f;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.AlloyDagger>();
			Item.shootSpeed = 6f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "GerdDagger");
			recipe.AddIngredient(null, "AlloyBox");
			recipe.AddIngredient(ItemID.GoldBar, 8);
			recipe.AddIngredient(null, "UpgradeCrystal", 30);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(null, "GerdDagger");
			recipe2.AddIngredient(null, "AlloyBox");
			recipe2.AddIngredient(ItemID.PlatinumBar, 8);
			recipe2.AddIngredient(null, "UpgradeCrystal", 30);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}