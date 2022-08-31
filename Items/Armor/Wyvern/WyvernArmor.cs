using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace excels.Items.Armor.Wyvern
{
    [AutoloadEquip(EquipType.Head)]
    internal class WyvernHood : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wyvern Scale Hood");
			Tooltip.SetDefault("Increases your max number of minions by 1 \nIncreases minion damage by 5%");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 22;
			Item.sellPrice(gold: 1);
			Item.rare = 4;
			Item.defense = 8;
		}

        public override void UpdateEquip(Player player)
        {
			player.maxMinions += 1;
			player.GetDamage(DamageClass.Summon) += 0.05f;
        }

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Avian.AvianHead>())
				.AddIngredient(ModContent.ItemType<Materials.WyvernScale>(), 10)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<WyvernScalemail>() && legs.type == ModContent.ItemType<WyvernBoots>();
		}

		public override void UpdateArmorSet(Player player)
		{
			// taking damage temporarily surrounds player with ice shards
			player.setBonus = "Summons 3 orbiting Wind Pixies to fight for you"; // \nIncreases your max number of minions and sentries by 1";

			player.GetModPlayer<excelPlayer>().WyvernSet = true;
			if (player.ownedProjectileCounts[ModContent.ProjectileType<WindPixie>()] == 0)
			{
				for (var i = 0; i < 3; i++)
				{
					Projectile p = Projectile.NewProjectileDirect(player.GetSource_FromThis(), player.Center, new Vector2(0, -3), ModContent.ProjectileType<WindPixie>(), 0, 0, player.whoAmI);
					p.ai[0] = i * 120;
					p.ai[1] = -30 * i;
					p.frame = i;
				}
			}
			//	player.maxTurrets += 1;
			//	player.maxMinions += 1;
		}
	}

		[AutoloadEquip(EquipType.Body)]
	internal class WyvernScalemail : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wyvern Scalemail");
			Tooltip.SetDefault("Increases spirit attack speed and whip speed by 15% \nIncreases minion damage by 6%");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 22;
			Item.sellPrice(gold: 1);
			Item.rare = 4;
			Item.defense = 12;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetModPlayer<excelPlayer>().SpiritAttackSpeed += 0.15f;
			player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.15f;
			player.GetDamage(DamageClass.Summon) += 0.06f;
			//	ArmorIDs.Wing.Sets.Stats[Item.wingSlot].FlyTime += 120;
			//player.wingTimeMax += 120;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Avian.AvianChest>())
				.AddIngredient(ModContent.ItemType<Materials.WyvernScale>(), 12)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}


	[AutoloadEquip(EquipType.Legs)]
	internal class WyvernBoots : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wyvern Scale Boots");
			Tooltip.SetDefault("Increases your max number of sentries by 1 \nIncreases minion damage by 6%");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 22;
			Item.sellPrice(gold: 1);
			Item.rare = 4;
			Item.defense = 9;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Summon) += 0.06f;
			player.maxTurrets += 1;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Avian.AvianSkirt>())
				.AddIngredient(ModContent.ItemType<Materials.WyvernScale>(), 10)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}

	public class WindPixie : ModProjectile
    {
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			Projectile.DamageType = DamageClass.Summon;
			Projectile.timeLeft = 10;
			Projectile.width = 32;
			Projectile.height = 24;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.netImportant = true;
		}

        public override void AI()
        {
			Player player = Main.player[Projectile.owner];

			if (!player.GetModPlayer<excelPlayer>().WyvernSet || player.dead)
            {
				Projectile.Kill();
            }
			Projectile.timeLeft = 2;

			Projectile.ai[0] += 1.5f;
			Projectile.Center = player.Center + new Vector2(0, -46) + new Vector2(25, 5).RotatedBy(MathHelper.ToRadians(Projectile.ai[0]));

			Projectile.ai[1]++;
			if (Projectile.ai[1] >= 30)
            {
				Vector2 targetPos = Projectile.position;
				float targetDist = 600;
				bool target = false;
				if (player.HasMinionAttackTargetNPC)
				{
					NPC npc = Main.npc[player.MinionAttackTargetNPC];
					// changed so that it uses checks if player has line of sight with npc, not the actual summon
					if (Collision.CanHitLine(player.position, player.width, player.height, npc.position, npc.width, npc.height))
					{
						if (Vector2.Distance(Projectile.Center, targetPos) < targetDist)
						{
							targetPos = npc.Center;
							target = true;
						}
					}
				}
				if (!target)
				{
					for (int k = 0; k < 200; k++)
					{
						NPC npc = Main.npc[k];
						if (npc.CanBeChasedBy(this, false))
						{
							float distance = Vector2.Distance(npc.Center, Projectile.Center);
							if ((distance < targetDist || !target) && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height))
							{
								targetDist = distance;
								targetPos = npc.Center;
								target = true;
							}
						}
					}
				}

				if (target)
                {
					Vector2 vel = (targetPos - Projectile.Center).SafeNormalize(Vector2.Zero) * 3;
					Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, vel, ModContent.ProjectileType<Items.Weapons.Skyline.SkylineCaneProj>(), 35, 2, player.whoAmI);
					p.netUpdate = true;
					p.extraUpdates = 15;
					p.timeLeft = 500;
					Projectile.netUpdate = true;
                }

				Projectile.ai[1] = 0;
            }

			if (++Projectile.frameCounter % 6 == 0)
            {
				if (++Projectile.frame > 3)
                {
					Projectile.frame = 0;
                }
            }
		}
    }
}
