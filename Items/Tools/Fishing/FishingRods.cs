using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace excels.Items.Tools.Fishing
{
    internal class DualHookingPole : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dual Hooker");
			Tooltip.SetDefault("Casts out two lines at once!");

			// Allows the pole to fish in lava
			ItemID.Sets.CanFishInLava[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.WoodFishingPole);

			Item.rare = 4;
			Item.fishingPole = 30; // Sets the poles fishing power
			Item.shootSpeed = 13.5f; // Sets the speed in which the bobbers are launched. Wooden Fishing Pole is 9f and Golden Fishing Rod is 17f.
			Item.shoot = ModContent.ProjectileType<DualBobbers>(); // The Bobber projectile.
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			//Projectile.NewProjectile(source, position, velocity.RotatedByRandom(MathHelper.ToRadians(12)) * Main.rand.NextFloat(0.92f, 1.08f), ModContent.ProjectileType<DualBobberBlue>(), 0, 0, player.whoAmI);
			for (var i = 0; i < 2; i++)
			{
				Projectile p = Projectile.NewProjectileDirect(source, position, velocity.RotatedByRandom(MathHelper.ToRadians(12)) * Main.rand.NextFloat(0.9f, 1.11f), type, 0, 0, player.whoAmI);
				p.frame = i;
			}
			return false;
        }

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.DualHook)
				.AddIngredient(ItemID.ReinforcedFishingPole)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	internal class DualBobbers : ModProjectile
    {
		public override void SetStaticDefaults()
		{
			// Total count animation frames
			Main.projFrames[Projectile.type] = 2;
		}

		public static readonly Color[] PossibleLineColors = new Color[] {
			new Color(255, 255, 255)
		};

		private Color FishingLineColor => PossibleLineColors[0];

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.BobberWooden);

			//DrawOriginOffsetY = -8; // Adjusts the draw position
		}

        public override void ModifyFishingLine(ref Vector2 lineOriginOffset, ref Color lineColor)
		{
			// Change these two values in order to change the origin of where the line is being drawn.
			// This will make it draw 47 pixels right and 31 pixels up from the player's center, while they are looking right and in normal gravity.
			lineOriginOffset = new Vector2(47, -31);
			// Sets the fishing line's color. Note that this will be overridden by the colored string accessories.
			lineColor = FishingLineColor;
		}
	}
}
