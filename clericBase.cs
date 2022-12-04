using Terraria;
using Terraria.ID;
using System.Collections.Generic;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using Terraria.Localization;
using Terraria.Utilities;
using excels.Prefixes;

namespace excels
{
    internal class ClericClass : DamageClass
    {
		public override void SetStaticDefaults()
		{
			// Make weapons with this damage type have a tooltip of 'X example damage'.
			ClassName.SetDefault("clerical damage");
		}

		public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
		{
			if (damageClass == DamageClass.Generic)
				return StatInheritanceData.Full;


			return new StatInheritanceData(
				damageInheritance: 0f,
				critChanceInheritance: 0f,
				attackSpeedInheritance: 0f,
				armorPenInheritance: 0f,
				knockbackInheritance: 0f
			);
		}

        public override bool ShowStatTooltipLine(Player player, string lineName)
        {
			if (lineName == "Speed")
				return true;

			return true;
        }

        public override bool UseStandardCritCalcs => true;

	}

	public class ClericClassPlayer : ModPlayer
    {
		public static ClericClassPlayer ModPlayer(Player player)
		{
			return player.GetModPlayer<ClericClassPlayer>();
		}

		public int clericRadiantAdd;
		public float clericRadiantMult = 1f;
		public int clericNecroticAdd;
		public float clericNecroticMult = 1f;

		public float clericKnockback;
		public int clericCrit;

		public int radianceStatCurrent;
		int radianceStatDefaultMax = 90;
		public int radianceStatMax;
		public int radianceStatMax2;
		int radianceRegenTimer = 0;
		public int radianceRegenRate = 4;
		public int radianceRegenTimeChange = 0;
		int timeSinceUse = 0;
		int lastRadianceCurrent;

		public override void Initialize()
		{
			radianceStatMax = radianceStatDefaultMax;
			lastRadianceCurrent = radianceStatCurrent;
		}

		public override void ResetEffects()
		{
			ResetVariables();
		}

		public override void UpdateDead()
		{
			ResetVariables();
		}

		private void ResetVariables()
		{
			clericRadiantAdd = 0;
			clericRadiantMult = 1f;
			clericNecroticAdd = 0;
			clericNecroticMult = 1f;

			clericKnockback = 0f;
			clericCrit = 0;

			radianceRegenRate = Math.Clamp((int)Math.Floor((double)timeSinceUse/40), 1, 4);
			radianceRegenTimeChange = 0;
			radianceStatMax2 = radianceStatMax;
			if (Player.GetModPlayer<Items.Misc.SnowFlowerPlayer>().SnowFlowerConsumed)
				radianceStatMax2 += 20;
		}

        public override void UpdateLifeRegen()
        {
			if (radianceStatCurrent < lastRadianceCurrent)
            {
				timeSinceUse = 0;
            }

			radianceRegenTimer += radianceRegenRate;

			while (radianceRegenTimer > 80)
            {
				radianceStatCurrent++;
				radianceRegenTimer -= 20;
            }

			if (Player.creativeGodMode)
				radianceStatCurrent += 33;

			radianceStatCurrent = Utils.Clamp(radianceStatCurrent, 0, radianceStatMax2);
			lastRadianceCurrent = radianceStatCurrent;
			timeSinceUse++;
		}

        public override void SaveData(TagCompound tag)
        {
			tag["Radiance"] = radianceStatCurrent;
		}

		public override void LoadData(TagCompound tag)
		{
			radianceStatCurrent = tag.GetInt("Radiance");
		}
	}

	public abstract class ClericDamageItem : ModItem
	{
		//public override bool i => true;
		public int clericBloodCost = 0;
		public int clericRadianceCost = 0;

		public bool clericEvil = false;
		public int healAmount = 0;
		public float healRate = 1;
		public bool skullPendantOverride = false;

		public float clericManaReduce = 0;

		public int clericBloodCostTrue = 0;

		// Custom items should override this to set their defaults
		public virtual void SafeSetDefaults()
		{
		}

		// By making the override sealed, we prevent derived classes from further overriding the method and enforcing the use of SafeSetDefaults()
		// We do this to ensure that the vanilla damage types are always set to false, which makes the custom damage type work
		public sealed override void SetDefaults()
		{
			Item.DamageType = ModContent.GetInstance<ClericClass>();
			SafeSetDefaults();
			//Item.DamageType = ModContent.GetInstance<ClericClass>();
		}

        public override bool AllowPrefix(int pre)
        {
			return (Item.damage > 0);
        }

        public override int ChoosePrefix(UnifiedRandom rand)
        {	
			switch (rand.Next(18))
            {
				case 0:
				case 1:
					return ModContent.PrefixType<AttunedPrefix>();
				case 2:
				case 3:
					return ModContent.PrefixType<UnattunedPrefix>();
				case 4:
					return ModContent.PrefixType<BlessedPrefix>();
				case 5:
				case 6:
				case 7:
					if (clericEvil)
						return ModContent.PrefixType<UnholyPrefix>();
					else
						return ModContent.PrefixType<HolyPrefix>();
				case 8:
					if (clericEvil)
						return ModContent.PrefixType<DivineNoManaPrefix>();
					else
						return ModContent.PrefixType<DivinePrefix>();
				case 9:
				case 10:
					if (Item.mana==0)
						return ModContent.PrefixType<CrazedNoManaPrefix>();
					else
						return ModContent.PrefixType<CrazedPrefix>();
				case 11:
					if (Item.mana == 0)
						return ModContent.PrefixType<ForgottenNoManaPrefix>();
					else
						return ModContent.PrefixType<ForgottenPrefix>();
				case 12:
					return PrefixID.Broken;
				case 13:
					return PrefixID.Damaged;
				case 14:
					return PrefixID.Zealous;
				case 15:
					return PrefixID.Keen;
				case 16:
					return PrefixID.Shoddy;
				case 17:
					return PrefixID.Godly;
			}

			return -1;
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
			// already gains these bonuses
		//	damage *= player.GetDamage(DamageClass.Generic).Multiplicative;
			if (!clericEvil)
			{
				// radient damage, plus 25% necrotic damage
				damage += ClericClassPlayer.ModPlayer(player).clericRadiantAdd;
				damage *= ClericClassPlayer.ModPlayer(player).clericRadiantMult + ((ClericClassPlayer.ModPlayer(player).clericNecroticMult - 1) * 0.25f);
			}
			else
			{
				// same as above but in reverse
				damage += ClericClassPlayer.ModPlayer(player).clericNecroticAdd;
				damage *= ClericClassPlayer.ModPlayer(player).clericNecroticMult + ((ClericClassPlayer.ModPlayer(player).clericRadiantMult - 1) * 0.25f);

				clericBloodCostTrue = (int)((clericBloodCost - player.GetModPlayer<excelPlayer>().bloodCostMinus) * player.GetModPlayer<excelPlayer>().bloodCostMult);
				if (clericBloodCostTrue <= 0)
                {
					clericBloodCostTrue = 1;
                }
			}
		}


        public override void ModifyWeaponKnockback(Player player, ref StatModifier knockback)
        {
			// Adds knockback bonuses
			knockback += ClericClassPlayer.ModPlayer(player).clericKnockback;
		}

        public override void ModifyWeaponCrit(Player player, ref float crit)
        {
		   crit += ClericClassPlayer.ModPlayer(player).clericCrit;
		}

		// Because we want the damage tooltip to show our custom damage, we need to modify it
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			var modPlayer = ClericClassPlayer.ModPlayer(Main.player[Item.whoAmI]);
			ExtraTooltipModifications(tooltips);

			// cool colour switching

			
		//	string s = $"{MathHelper.Lerp(Max.R, Min.R, timer).ToString("X2") + MathHelper.Lerp(Max.G, Min.G, timer).ToString("X2") + MathHelper.Lerp(Max.B, Min.B, timer).ToString("X2")}";
			string ClassText = $"[c/9b5ed4:~{Language.GetTextValue("Mods.excels.Common.DamageClass.ClericClass")}~]\n";
			// 9b5ed4

			//tooltips.Add(new TooltipLine(Mod, "Damage", "This weapon benefits from necrotic bonuses"));
			if (clericEvil)
            {
				float dmg = (float)Math.Round((modPlayer.clericNecroticMult - 1), 2) * 100;
				float dmgOp = (float)Math.Round((modPlayer.clericRadiantMult - 1) * 0.25f, 2) * 100;

				foreach (TooltipLine line2 in tooltips)
				{
					if (line2.Mod == "Terraria")
					{
						if (line2.Name == "Damage")
						{
							int wDmg = (int)Main.player[Item.whoAmI].GetWeaponDamage(Item, true);
							wDmg += ClericClassPlayer.ModPlayer(Main.player[Item.whoAmI]).clericNecroticAdd;
							wDmg = (int)(wDmg * (ClericClassPlayer.ModPlayer(Main.player[Item.whoAmI]).clericNecroticMult + ((ClericClassPlayer.ModPlayer(Main.player[Item.whoAmI]).clericRadiantMult - 1) * 0.25f)));

							string tip = $"{ClassText}{wDmg} {Language.GetTextValue("Mods.excels.Common.DamageClass.NecroticDamage")}";

							if (ModContent.GetInstance<excelConfig>().ClericAdvancedTooltip)
							{
								tip = $"{tip} ({dmg}% {Language.GetTextValue("Mods.excels.Common.Necrotic")} + {dmgOp}% {Language.GetTextValue("Mods.excels.Common.Radiant")})";
							}

							line2.Text = tip;
						}
						else if (line2.Name == "Knockback")
                        {
							string line = Language.GetTextValue("Mods.excels.Common.Tooltips.BloodCost", clericBloodCostTrue); 
							if (clericBloodCostTrue <= 1)
							{
								line = Language.GetTextValue("Mods.excels.Common.Tooltips.SingleBloodCost");
								clericBloodCostTrue = 1;
							}
							string orig = line2.Text;

							line2.Text = $"{orig}\n{line}";
						}
					}

				}
				//tooltips.Add(new TooltipLine(Mod, "ClericDamageType", "This weapon benefits from necrotic bonuses"));
			}
            else
            {
				float dmg = (float)Math.Round((modPlayer.clericRadiantMult - 1), 2) * 100;
				float dmgOp = (float)Math.Round((modPlayer.clericNecroticMult - 1) * 0.25f, 2) * 100;
				foreach (TooltipLine line2 in tooltips)
				{
					if (line2.Mod == "Terraria" && line2.Name == "Damage")
					{
						int wDmg = (int)(Main.player[Item.whoAmI].GetWeaponDamage(Item, true) * ExtraDamage());
						wDmg += ClericClassPlayer.ModPlayer(Main.player[Item.whoAmI]).clericRadiantAdd;
						wDmg = (int)(wDmg * (ClericClassPlayer.ModPlayer(Main.player[Item.whoAmI]).clericRadiantMult + ((ClericClassPlayer.ModPlayer(Main.player[Item.whoAmI]).clericNecroticMult - 1) * 0.25f)));

						string tip = $"{ClassText}{wDmg} {Language.GetTextValue("Mods.excels.Common.DamageClass.RadiantDamage")}";
						if (ModContent.GetInstance<excelConfig>().ClericAdvancedTooltip)
                        {
							tip = $"{tip} ({dmg}% {Language.GetTextValue("Mods.excels.Common.Radiant")} + {dmgOp}% {Language.GetTextValue("Mods.excels.Common.Necrotic")})";
						}

						line2.Text = tip;
					}
					else if (line2.Name == "UseMana")
                    {

						string l = "";
						if (Item.damage <= 0)
						{
							l = ClassText;
						}

						if (healAmount > 0)
						{
							int healExtra = Main.player[Item.whoAmI].GetModPlayer<excelPlayer>().healBonus;
							int trueHeal = (int)(healAmount + (healExtra * healRate));

							string tip = Language.GetTextValue("Mods.excels.Common.Tooltips.HealAmount", trueHeal);
							if (ModContent.GetInstance<excelConfig>().ClericHealTooltip)
								tip = $"{tip} ({healAmount} + {healExtra} x {healRate * 100}%)";

							line2.Text = $"{l}{line2.Text}\n{tip}";
						}
					}
					else if (line2.Name == "Knockback")
					{
						if (clericRadianceCost > 0)
						{
							line2.Text = $"{line2.Text}\n{Language.GetTextValue("Mods.excels.Common.Tooltips.RadianceCost", clericRadianceCost)}";
						}

						if (healAmount > 0)
						{
							int healExtra = Main.player[Item.whoAmI].GetModPlayer<excelPlayer>().healBonus;
							int trueHeal = (int)(healAmount + (healExtra * healRate));

							string tip = Language.GetTextValue("Mods.excels.Common.Tooltips.HealAmount", trueHeal);
							if (ModContent.GetInstance<excelConfig>().ClericHealTooltip)
								tip = $"{tip} ({healAmount} + {healExtra} x {healRate * 100}%)";

							line2.Text = $"{line2.Text}\n{tip}";
						}
					}
				}
				//tooltips.Add(new TooltipLine(Mod, "CerlicHealAMount", $"Restores {healAmount} health points"));
				//tooltips.Add(new TooltipLine(Mod, "ClericDamageType", "This weapon benefits from radiant bonuses"));
			}

			
		}

		public virtual Projectile CreateHealProjectile(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, float ai0 = 0, float ai1 = 0)
		{
			Projectile p = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
			p.GetGlobalProjectile<excelProjectile>().healStrength = healAmount;
			p.GetGlobalProjectile<excelProjectile>().healRate = healRate;
			p.ai[0] = ai0;
			p.ai[1] = ai1;
			p.netUpdate = true;

			return Main.projectile[p.whoAmI];
		}

		public virtual int CalculateHeal()
		{
			return (int)(healAmount + (Main.player[Item.whoAmI].GetModPlayer<excelPlayer>().healBonus * healRate));
		}


		public virtual void ExtraTooltipModifications(List<TooltipLine> tooltips)
        {

        }
		public virtual float ExtraDamage()
        {
			return 1;
        }


		public override bool CanUseItem(Player player)
		{
			if (clericBloodCostTrue > 0)
            {
				if ((player.statLife > clericBloodCostTrue) && (player.statMana > Item.mana))
                {
					CombatText.NewText(player.getRect(), Color.Red, clericBloodCostTrue);
					player.statLife -= clericBloodCostTrue;

					int bTime = (Item.damage * (40 - (Item.damage / 10))) - (player.lifeRegen * 5);
					bTime = (Item.damage / clericBloodCost * 30) + (clericBloodCostTrue * 45);
					player.AddBuff(ModContent.BuffType<Buffs.ClericCld.AnguishedSoul>(), bTime);
					return true;
                }
				else
                {
					return false;
                }
            }
			if (clericRadianceCost > 0)
            {
				var clericDamagePlayer = player.GetModPlayer<ClericClassPlayer>();

				if (clericDamagePlayer.radianceStatCurrent >= clericRadianceCost)
				{
					clericDamagePlayer.radianceStatCurrent -= clericRadianceCost;
					return true;
				}

				return false;
			}
			return true;
		}

        public override bool? UseItem(Player player)
        {
			if (player.GetModPlayer<excelPlayer>().skullPendant && !skullPendantOverride)
            {
				CheckSkullPendant(player, clericBloodCostTrue);
            }
            return base.UseItem(player);
        }

        public void CheckSkullPendant(Player player, int increase)
        {
			if (!player.GetModPlayer<excelPlayer>().skullPendant)
				return;

			player.GetModPlayer<excelPlayer>().skullPendantBlood += increase;
			if (player.GetModPlayer<excelPlayer>().skullPendantBlood >= 60)
			{
				player.GetModPlayer<excelPlayer>().skullPendantBlood -= 60;

				player.statLife += 10;
				if (player.statLife > player.statLifeMax2)
					player.statLife = player.statLifeMax2;
				player.HealEffect(10);

				if (player.GetModPlayer<excelPlayer>().skullPendant2)
                {
					player.immune = true;
					player.immuneTime = 45;
                }
				if (player.GetModPlayer<excelPlayer>().skullPendantFrost)
					player.GetModPlayer<excelPlayer>().SnowflakeAmulet();

				for (var i = 0; i < 20; i++)
                {
					Dust d = Dust.NewDustPerfect(player.Center, 90);
					d.velocity = new Vector2(2).RotatedBy(MathHelper.ToRadians((360 / 20) * i));
					d.noGravity = true;
					d.scale = 1.3f;
                }

				NetMessage.SendData(66, -1, -1, null, player.whoAmI, 10, 0f, 0f, 0, 0, 0);
			}

		}
    }

    public abstract class ClericHolyWeap : ClericDamageItem
	{
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

        public override bool CanUseItem(Player player)
        {
			if (player.altFunctionUse == 2)
            {
				if (player.HasBuff(ModContent.BuffType<Buffs.ClericCld.BlessingCooldown>()))
				{
					return false;
                }
				return true;
            }
			return true;
        }
    }
}
