using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace excels.Items.Bags
{
	public abstract class BossBag : ModItem
    {

		public virtual void ModdedDevItems(Player player)
		{
			if (Main.rand.NextBool(7))
			{
				switch (Main.rand.Next(2)) {
					case 0:
						player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<Weapons.Sword1.CatsClaw>());
						player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<Vanity.DevCoat>());
						player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<Items.Placeable.Decorations.Misc.NightLightLamp>());
						break;
					case 1:
						player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<Vanity.AcesGoldFoxMask>());
						//Mod atheria = ModLoader.GetMod("ShardsOfAtheria");
						if (ModLoader.TryGetMod("ShardsOfAtheria", out Mod atheria))
                        {
							if (atheria.TryFind<ModItem>("AcesJacket", out ModItem AceJack))
								player.QuickSpawnItem(player.GetSource_FromThis(), AceJack.Type);
							if (atheria.TryFind<ModItem>("AcesPants", out ModItem AcePant))
								player.QuickSpawnItem(player.GetSource_FromThis(), AcePant.Type);

							if (atheria.TryFind<ModItem>("AceOfSpades", out ModItem AceOfSpades))
								player.QuickSpawnItem(player.GetSource_FromThis(), AceOfSpades.Type);
						}
						break;
				}
			}
		}

		public override Color? GetAlpha(Color lightColor)
		{
			// Makes sure the dropped bag is always visible
			return Color.Lerp(lightColor, Color.White, 0.4f);
		}

		public override void PostUpdate()
		{
			// Spawn some light and dust when dropped in the world
			Lighting.AddLight(Item.Center, Color.White.ToVector3() * 0.4f);

			if (Item.timeSinceItemSpawned % 12 == 0)
			{
				Vector2 center = Item.Center + new Vector2(0f, Item.height * -0.1f);

				// This creates a randomly rotated vector of length 1, which gets it's components multiplied by the parameters
				Vector2 direction = Main.rand.NextVector2CircularEdge(Item.width * 0.6f, Item.height * 0.6f);
				float distance = 0.3f + Main.rand.NextFloat() * 0.5f;
				Vector2 velocity = new Vector2(0f, -Main.rand.NextFloat() * 0.3f - 1.5f);

				Dust dust = Dust.NewDustPerfect(center + direction * distance, DustID.SilverFlame, velocity);
				dust.scale = 0.5f;
				dust.fadeIn = 1.1f;
				dust.noGravity = true;
				dust.noLight = true;
				dust.alpha = 0;
			}
		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			// Draw the periodic glow effect behind the item when dropped in the world (hence PreDrawInWorld)
			Texture2D texture = TextureAssets.Item[Item.type].Value;

			Rectangle frame;

			if (Main.itemAnimations[Item.type] != null)
			{
				// In case this item is animated, this picks the correct frame
				frame = Main.itemAnimations[Item.type].GetFrame(texture, Main.itemFrameCounter[whoAmI]);
			}
			else
			{
				frame = texture.Frame();
			}

			Vector2 frameOrigin = frame.Size() / 2f;
			Vector2 offset = new Vector2(Item.width / 2 - frameOrigin.X, Item.height - frame.Height);
			Vector2 drawPos = Item.position - Main.screenPosition + frameOrigin + offset;

			float time = Main.GlobalTimeWrappedHourly;
			float timer = Item.timeSinceItemSpawned / 240f + time * 0.04f;

			time %= 4f;
			time /= 2f;

			if (time >= 1f)
			{
				time = 2f - time;
			}

			time = time * 0.5f + 0.5f;

			for (float i = 0f; i < 1f; i += 0.25f)
			{
				float radians = (i + timer) * MathHelper.TwoPi;

				spriteBatch.Draw(texture, drawPos + new Vector2(0f, 8f).RotatedBy(radians) * time, frame, new Color(90, 70, 255, 50), rotation, frameOrigin, scale, SpriteEffects.None, 0);
			}

			for (float i = 0f; i < 1f; i += 0.34f)
			{
				float radians = (i + timer) * MathHelper.TwoPi;

				spriteBatch.Draw(texture, drawPos + new Vector2(0f, 4f).RotatedBy(radians) * time, frame, new Color(140, 120, 255, 77), rotation, frameOrigin, scale, SpriteEffects.None, 0);
			}

			return true;
		}
	}

    internal class BagNiflheim : BossBag
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag (Niflheim)");
			Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
			ItemID.Sets.BossBag[Type] = true; // This set is one that every boss bag should have, it, for example, lets our boss bag drop dev armor..
			ItemID.Sets.PreHardmodeLikeBossBag[Type] = true; // ..But this set ensures that dev armor will only be dropped on special world seeds, since that's the behavior of pre-hardmode boss bags.

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.EyeOfCthulhuBossBag);
			Item.maxStack = 999;
			Item.consumable = true;
			Item.width = 24;
			Item.height = 24;
			Item.rare = ItemRarityID.Cyan;
			Item.expert = true;
		}

		public override bool CanRightClick()
		{
			return true;
		}

		public override void OpenBossBag(Player player)
		{
			ModdedDevItems(player);

			player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<Accessories.Expert.NiflheimExpertAcc>());
			
			player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<Items.Materials.GlacialOre>(), Main.rand.Next(66, 100));
			switch (Main.rand.Next(4))
			{
				case 0:
					player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<Weapons.Glacial.Boss.DarkEye > ());
					break;
				case 1:
					player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<Weapons.Glacial.Boss.Nastrond>());
					break;
				case 2:
					player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<Weapons.Glacial.Boss.FrozenChainblade>());
					break;
				case 3:
					player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<Weapons.ThrowPotions.SnowSpellPot>());
					break;
			}

			if (Main.rand.NextBool())
				player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<Items.Accessories.Random.SnowflakeAmulet>());

			player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<Misc.SnowFlower>());

			//player.QuickSpawnItem(player.GetItemSource_OpenItem(ModContent.ItemType<BagNiflheim>()), ModContent.ItemType<Items.Weapons.Glacial.Boss.DarkEye>());
			////player.QuickSpawnItem(ItemType<PurityTotem>());
			//player.QuickSpawnItem(ItemType<SixColorShield>());
		}

		public override int BossBagNPC => ModContent.NPCType<NPCs.Glacial.GlacialQueen>();
	}

	internal class BagChasm : BossBag
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag (Chasm)");
			Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
			ItemID.Sets.BossBag[Type] = true; // This set is one that every boss bag should have, it, for example, lets our boss bag drop dev armor..
			ItemID.Sets.PreHardmodeLikeBossBag[Type] = true;

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.EyeOfCthulhuBossBag);
			Item.maxStack = 999;
			Item.consumable = true;
			Item.width = 24;
			Item.height = 24;
			Item.rare = ItemRarityID.Cyan;
			Item.expert = true;
		}

		public override bool CanRightClick()
		{
			return true;
		}

		public override void OpenBossBag(Player player)
		{
			ModdedDevItems(player);

			player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<Accessories.Expert.ChasmExpertAcc>());
			switch (Main.rand.Next(3)) 
			{
				case 0:
					player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<Weapons.Chasm.Skewer>());
					break;
				case 1:
					player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<Weapons.Chasm.V90>());
					break;
				case 2:
					player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<Weapons.Chasm.GraspofDisease>());
					break;
			}
			if (Main.rand.NextBool(7))
				player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<Vanity.ChasmMask>());

			////player.QuickSpawnItem(ItemType<PurityTotem>());
			//player.QuickSpawnItem(ItemType<SixColorShield>());
		}
		public override int BossBagNPC => ModContent.NPCType<NPCs.Glacial.GlacialQueen>();
	}

	internal class BagStarship : BossBag
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag (Stellar Starship)");
			Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
			ItemID.Sets.BossBag[Type] = true; // This set is one that every boss bag should have, it, for example, lets our boss bag drop dev armor..
			ItemID.Sets.PreHardmodeLikeBossBag[Type] = true; // ..But this set ensures that dev armor will only be dropped on special world seeds, since that's the behavior of pre-hardmode boss bags.

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.EyeOfCthulhuBossBag);
			Item.maxStack = 999;
			Item.consumable = true;
			Item.width = 24;
			Item.height = 24;
			Item.rare = ItemRarityID.Cyan;
			Item.expert = true;
		}

		public override bool CanRightClick()
		{
			return true;
		}

		public override void OpenBossBag(Player player)
		{
			ModdedDevItems(player);

			player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<Accessories.Expert.StellarExpertAcc>());
			if (Main.rand.NextBool(7))
				player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<Vanity.StellarStarshipMask>());

			player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<Items.Materials.StellarPlating>(), Main.rand.Next(50, 61));
		}
		public override int BossBagNPC => ModContent.NPCType<NPCs.Glacial.GlacialQueen>();
	}
}
