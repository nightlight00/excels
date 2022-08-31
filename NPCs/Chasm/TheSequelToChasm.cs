using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Localization;
using System.Collections.Generic;
using Terraria.Chat;

namespace excels.NPCs.Chasm
{
	public abstract class Chasm2Boss : Worm
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chasm 2");
			NPCID.Sets.MPAllowedEnemies[Type] = true;
		}

		public override void Init()
		{
			minLength = 30;
			maxLength = 30;
			tailType = ModContent.NPCType<Chasm2Tail>();
			bodyType = ModContent.NPCType<Chasm2Body>();
			headType = ModContent.NPCType<Chasm2Head>();
			speed = 15f;
			turnSpeed = 0.26f;
			flies = true;
		}

		public int resetTime = 300;
	}

	//[AutoloadBossHead]
	public class Chasm2Head : Chasm2Boss
	{
		public override string Texture => "excels/NPCs/Chasm/ChasmHead";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chasm, Host of the Infection");
			NPCID.Sets.DebuffImmunitySets.Add(Type, new NPCDebuffImmunityData
			{
				SpecificallyImmuneTo = new int[] {
					BuffID.Confused,
					ModContent.BuffType<Buffs.Debuffs.Mycosis>(),
					BuffID.BetsysCurse
				}
			});

			var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{ // Influences how the NPC looks in the Bestiary
				CustomTexturePath = "excels/NPCs/Chasm/Chasm_Bestiary", // If the NPC is multiple parts like a worm, a custom texture for the Bestiary is encouraged.
				Position = new Vector2(100f, 24f),
				PortraitPositionXOverride = 80f,
				PortraitPositionYOverride = 12f,
				Hide = true
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
			NPCID.Sets.MPAllowedEnemies[Type] = true;
		}

		public override void SetDefaults()
		{
			// Head is 10 defence, body 20, tail 30.
			NPC.CloneDefaults(NPCID.DiggerHead);
			NPC.aiStyle = -1;
			NPC.width = NPC.height = 116;
			NPC.lifeMax = 76000;
			NPC.defense = 30;
			NPC.damage = 80;
			NPC.HitSound = SoundID.NPCHit39;
			NPC.boss = true;
			NPC.npcSlots = 10;
			NPC.value = 150000;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life > 0)
			{
				return;
			}
			for (var l = 0; l < 4; l++)
			{
				for (var i = 0; i < 6; i++)
				{
					Vector2 shootvel = NPC.velocity.RotatedBy(MathHelper.ToRadians((360f / 6f) * (i + 1)) + (60 * l)) * (0.4f * (l + 1));
					Projectile p = Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center, shootvel, ModContent.ProjectileType<InfectionCloud>(), 18, 1);
					p.timeLeft = 360;
				}
			}

			for (var b = 0; b < 8; b++)
			{
				Dust d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, ModContent.DustType<Dusts.ChasmBodyDust>());
				d.velocity = (NPC.velocity * 0.5f).RotatedByRandom(MathHelper.ToRadians(20));
			}
			for (var h = 0; h < 28; h++)
			{
				Dust d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, ModContent.DustType<Dusts.ChasmHeadDust>());
				d.velocity = (NPC.velocity * 0.7f).RotatedByRandom(MathHelper.ToRadians(30));
			}

			if (Main.netMode == NetmodeID.Server)
			{
				// We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
				return;
			}
			int num675 = Gore.NewGore(NPC.GetSource_FromThis(), new Vector2(NPC.position.X, NPC.position.Y - 10f), new Vector2((float)hitDirection, 0f), 61, NPC.scale);
			Gore gore2 = Main.gore[num675];
			gore2.velocity *= 0.1f;

			Gore.NewGoreDirect(NPC.GetSource_FromThis(), NPC.Center, NPC.velocity * 0.4f, Mod.Find<ModGore>("ChasmGoreEye").Type);
			Gore.NewGoreDirect(NPC.GetSource_FromThis(), NPC.Center, (NPC.velocity * 0.4f).RotatedByRandom(MathHelper.ToRadians(20)), Mod.Find<ModGore>("ChasmGoreMandible").Type);
			Gore.NewGoreDirect(NPC.GetSource_FromThis(), NPC.Center, (NPC.velocity * 0.4f).RotatedByRandom(MathHelper.ToRadians(20)), Mod.Find<ModGore>("ChasmGoreMandible").Type);
		}

		public override void Init()
		{
			base.Init();
			head = true;
		}

        public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
			crit = true;
        }
        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			crit = true;
        }

        public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(armorActive);
			writer.Write(breathTimer);
			writer.Write(spinner);
			writer.Write(CurrentAttack);
			writer.Write(AttackVar);
			writer.Write(AttackVar2);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			armorActive = reader.ReadBoolean();
			breathTimer = reader.ReadInt32();
			spinner = reader.ReadInt32();
			CurrentAttack = reader.ReadInt32();
			AttackVar = reader.ReadInt32();
			AttackVar2 = reader.ReadInt32();
		}

		// Variables
		bool armorActive = false;
		int breathTimer = 0;
		int spinner = 0;
		
		int CurrentAttack = 0;
		int AttackVar = 0;
		int AttackVar2 = 0;

		Color chatC = new Color(63, 66, 207);

		public override void CustomBehavior()
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				if (Main.player[NPC.target].dead || !Main.player[NPC.target].active)
				{
					targetPos = Main.player[NPC.target].position + new Vector2(0, 5000);
					if (NPC.position.Y > Main.player[NPC.target].position.Y + 800)
					{
						NPC.active = false;
						NPC.life = -1;
					}
					return;
				}


				switch (CurrentAttack)
                {
					default:
						if (Main.rand.NextBool(200))
							CurrentAttack = 1;
						if (Main.rand.NextBool(200))
							CurrentAttack = 0;
						break;

					case 0:
						Vector2 tpos = Main.player[NPC.target].Center - new Vector2(0, 430).RotatedBy(MathHelper.ToRadians(spinner));

						if (AttackVar > 0)
						{
							turnSpeed = 0.26f;
							NPC.velocity = (tpos - NPC.Center).SafeNormalize(Vector2.Zero);
							NPC.rotation = NPC.velocity.ToRotation() + MathHelper.ToRadians(90);
							//NPC.rotation = ((tpos+NPC.Center-Main.player[NPC.target].Center) - NPC.Center).SafeNormalize(Vector2.Zero).ToRotation() + MathHelper.ToRadians(90);
							NPC.Center = tpos;
							spinner += 3 * AttackVar2;
							
							AttackVar++;
							if (AttackVar % 18 == 10)
							{
								Projectile p = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, (Main.player[NPC.target].Center - tpos).SafeNormalize(Vector2.Zero) * 6, ModContent.ProjectileType<InfectionWave>(), 20, 3);
								p.netUpdate = true;	
							}

							if (AttackVar > 260 && Main.rand.NextBool(50))
							{
								AttackVar = 0;
								AttackVar2 = 0;
								CurrentAttack = -1;
								targetPos = Main.player[NPC.target].Center;
								NPC.velocity = new Vector2(speed).RotatedBy(NPC.rotation);
								spinner = 0;
							}
						}
						else
						{
							targetPos = tpos;

							if (Vector2.Distance(NPC.Center, tpos) < 30)
							{
								AttackVar = 1;
								if (NPC.Center.X < tpos.X)
									AttackVar2 = 1;
								else
									AttackVar2 = -1;
							}

							turnSpeed = 1f;
						}
						break;

					case 1:
						//tpos = NPC.Center - new Vector2(0, 430).RotatedBy(MathHelper.ToRadians(spinner));
						NPC.velocity += new Vector2(3, 0).RotatedBy(MathHelper.ToRadians(spinner));
						spinner += 3;

						if (++AttackVar > 400)
                        {
							AttackVar = 0;
							spinner = 0;
							CurrentAttack = -1; 
                        }
						break;
                }


				if (--breathTimer <= 0)
				{
					Vector2 dir1 = new Vector2(1, 0).RotatedBy(NPC.velocity.ToRotation() + MathHelper.ToRadians(90));
					Projectile proj = Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center, dir1 * 4f, ModContent.ProjectileType<InfectionCloud>(), 16, 0);
					proj.netUpdate = true;
					Vector2 dir2 = new Vector2(1, 0).RotatedBy(NPC.velocity.ToRotation() + MathHelper.ToRadians(-90));
					Projectile proj2 = Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center, dir2 * 4f, ModContent.ProjectileType<InfectionCloud>(), 16, 0);
					proj2.netUpdate = true;

					breathTimer = 8 + (int)((NPC.life / NPC.lifeMax) * 12); // spawns more often the lower the health
					NPC.netUpdate = true;
				}

				if (NPC.life < NPC.lifeMax * 0.3f)
				{
					if (!armorActive)
                    {
						string TXT = "Chasm's armor has become reflective!";
						if (Main.netMode != NetmodeID.Server)
						{
							string text = Language.GetTextValue(TXT, Lang.GetNPCNameValue(NPC.type), TXT);
							Main.NewText(text, chatC.R, chatC.G, chatC.B);
						}
						else
						{
							NetworkText text = NetworkText.FromKey(TXT, Lang.GetNPCNameValue(NPC.type), TXT);
							Terraria.Chat.ChatHelper.BroadcastChatMessage(text, chatC);
						}
						armorActive = true;	
					}
				}
			}
		}
	}


	internal class Chasm2Body : Chasm2Boss
	{
		public override string Texture => "excels/NPCs/Chasm/ChasmBody";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chasm");
			NPCID.Sets.DebuffImmunitySets.Add(Type, new NPCDebuffImmunityData
			{
				ImmuneToWhips = true,
				ImmuneToAllBuffsThatAreNotWhips = true
			});
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
			NPCID.Sets.MPAllowedEnemies[Type] = true;
		}

		public override void SetDefaults()
		{
			NPC.CloneDefaults(NPCID.DiggerBody);
			NPC.aiStyle = -1;
			NPC.width = NPC.height = 60;
			NPC.lifeMax = 76000;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.defense = 40;
			NPC.damage = 50;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0 && !Main.player[NPC.target].dead)
			{
				for (var i = 0; i < 8; i++)
				{
					Dust d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, ModContent.DustType<Dusts.ChasmBodyDust>());
					d.velocity = (Vector2.One * 5f).RotatedByRandom(MathHelper.ToRadians(20));
				}

				if (Main.netMode == NetmodeID.Server)
				{
					// We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
					return;
				}
				int num675 = Gore.NewGore(NPC.GetSource_FromThis(), new Vector2(NPC.position.X, NPC.position.Y - 10f), new Vector2((float)hitDirection, 0f), 61, NPC.scale);
				Gore gore2 = Main.gore[num675];
				gore2.velocity *= 0.1f;

				Gore.NewGoreDirect(NPC.GetSource_FromThis(), NPC.Center, NPC.velocity * 0.2f, Mod.Find<ModGore>("ChasmGoreBody").Type);
				for (var i = 0; i < Main.rand.Next(2, 4); i++)
				{
					int gType = Mod.Find<ModGore>("ChasmGoreMush1").Type;
					switch (Main.rand.Next(3))
					{
						case 1: gType = Mod.Find<ModGore>("ChasmGoreMush2").Type; break;
						case 2: gType = Mod.Find<ModGore>("ChasmGoreMush3").Type; break;
					}
					Gore.NewGoreDirect(NPC.GetSource_FromThis(), NPC.Center, (NPC.velocity * Main.rand.NextFloat(0.2f, 0.4f)).RotatedByRandom(MathHelper.ToRadians(20)), gType);
				}
			}
		}

		public override void CustomBehavior()
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				if (NPC.life < NPC.lifeMax * 0.3f)
					NPC.reflectsProjectiles = true;
			}
		}
	}


	internal class Chasm2Tail : Chasm2Boss
	{
		public override string Texture => "excels/NPCs/Chasm/ChasmTail";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chasm");
			NPCID.Sets.DebuffImmunitySets.Add(Type, new NPCDebuffImmunityData
			{
				ImmuneToWhips = true,
				ImmuneToAllBuffsThatAreNotWhips = true
			});
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
			NPCID.Sets.MPAllowedEnemies[Type] = true;
		}

		public override void SetDefaults()
		{
			NPC.CloneDefaults(NPCID.DiggerTail);
			NPC.aiStyle = -1;
			NPC.width = NPC.height = 60;
			NPC.lifeMax = 72000;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.defense = 24;
			NPC.damage = 80;
		}

		public override void Init()
		{
			base.Init();
			tail = true;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode == NetmodeID.Server)
			{
				// We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
				return;
			}

			if (NPC.life <= 0 && !Main.player[NPC.target].dead)
			{
				int num675 = Gore.NewGore(NPC.GetSource_FromThis(), new Vector2(NPC.position.X, NPC.position.Y - 10f), new Vector2((float)hitDirection, 0f), 61, NPC.scale);
				Gore gore2 = Main.gore[num675];
				gore2.velocity *= 0.1f;
			}
		}
	}
}
