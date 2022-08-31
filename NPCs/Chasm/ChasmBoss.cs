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

namespace excels.NPCs.Chasm
{ 
	[AutoloadBossHead]
	public class ChasmHead : ChasmBoss
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chasm");
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
				PortraitPositionYOverride = 12f
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);

			NPCID.Sets.MPAllowedEnemies[Type] = true;
		}

		public override void OnKill()
		{
			if (!excelWorld.downedChasm)
			{
				excelWorld.downedChasm = true;
				if (Main.netMode == NetmodeID.Server)
				{
					NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
				}
			}
			NPC.SetEventFlagCleared(ref excelWorld.downedChasm, -1);
		}

		public override void SetDefaults()
		{
			// Head is 10 defence, body 20, tail 30.
			NPC.CloneDefaults(NPCID.DiggerHead);
			NPC.aiStyle = -1;
			NPC.width = NPC.height = 60;
			NPC.lifeMax = 90000;
			NPC.defense = 30;
			NPC.damage = 70;
			NPC.HitSound = SoundID.NPCHit39;
			NPC.boss = true;
			NPC.npcSlots = 10;
			NPC.value = 150000;
		}

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
			bossLifeScale = 1;
			if (Main.expertMode)
            {
				bossLifeScale += .65f;
            }
			if (Main.masterMode)
            {
				bossLifeScale += .6f;
            }
			bossLifeScale += 0.16f * (numPlayers * 0.85f);

			//return bossLifeScale;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.SurfaceMushroom,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundMushroom,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("Attempting to seek refuge from his infection within a suit of armor, now mindlessly stalks the lands of his infection"),
//				new FlavorTextBestiaryInfoElement("Seeking shelter from his infection withen a suit of armor, Chasm is no longer the one in control and now mindlessly hunts for new prey")
			});
		}

        public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
			damage = (int)(damage * 1.2f);
			return;
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			damage = (int)(damage * 1.2f);
			return;
		}

        public override void BossLoot(ref string name, ref int potionType)
        {
			potionType = ItemID.GreaterHealingPotion;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<Items.Bags.BagChasm>()));

			LeadingConditionRule leadingConditionRule = new LeadingConditionRule(new Conditions.NotExpert());
			int[] WeapDrops = {
				ModContent.ItemType<Items.Weapons.Chasm.Skewer>(),
				ModContent.ItemType<Items.Weapons.Chasm.V90>()
			};
			leadingConditionRule.OnSuccess(ItemDropRule.OneFromOptions(1, WeapDrops));
			leadingConditionRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Vanity.ChasmMask>(), 7));
			npcLoot.Add(leadingConditionRule);
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

		private int attackCounter;
		private int breathTimer = 0;
		private int waitTimer = 0;
		private int attackType = 0;
		private float spinTimer = 0;

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(attackCounter);
			writer.Write(breathTimer);
			writer.Write(waitTimer);
			writer.Write(attackType);
			writer.Write(spinTimer);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			attackCounter = reader.ReadInt32();
			breathTimer = reader.ReadInt32();
			waitTimer = reader.ReadInt32();
			attackType = reader.ReadInt32();
			spinTimer = reader.ReadInt32();
		}

		public override void CustomBehavior()
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{

				// spins around player, kinda gives worm motion when chasing player
				//	spinTimer += 0.08f;
				//	targetPos = Main.player[NPC.target].position + new Vector2(MathF.Cos(spinTimer) * 200, MathF.Sin(spinTimer) * 200);
				targetPos = Main.player[NPC.target].position;

				// burrows into the ground
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


				if (waitTimer < 0)
                {
					attackType = Main.rand.Next(7);
					switch (attackType)
                    {
						default: // infectious breath
							attackCounter = 45;
							waitTimer = 40;
							break;
						case 2: // missiles
						case 3:
							attackCounter = 90;
							waitTimer = 80;
							break;
						case 4: // speed up / charge at player
							SoundEngine.PlaySound(SoundID.ScaryScream, NPC.Center);
							attackCounter = 200;
							waitTimer = 50;
							break;
						case 5: // infection waves
						case 6:
							attackCounter = 20;
							waitTimer = 160;
							break;

                    }	
                }

				if (attackCounter > 0)
                {
					attackCounter--;
					switch (attackType)
                    {
						default:
							if (attackCounter % 30 == 25)
                            {
								SoundEngine.PlaySound(SoundID.NPCDeath6, NPC.Center);
								Vector2 direction = NPC.velocity.SafeNormalize(Vector2.UnitX) * 5;
								Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, direction * 1, ModContent.ProjectileType<InfectionBreath>(), 45, 0);
							}
							break;
						case 2:
						case 3:
							if (NPC.life <= NPC.lifeMax / 2)
							{
								if (attackCounter % 15 == 0)
								{
									SoundEngine.PlaySound(SoundID.Item92, NPC.Center);
									Vector2 direction = NPC.velocity.SafeNormalize(Vector2.UnitX); //.RotatedBy(MathHelper.ToRadians(10));
									Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, direction * 1, ModContent.ProjectileType<InfectionMissile>(), 32, 0);
								}
							}
							else
							{
								if (attackCounter % 30 == 0)
								{
									SoundEngine.PlaySound(SoundID.Item92, NPC.Center);
									Vector2 direction = NPC.velocity.SafeNormalize(Vector2.UnitX); //.RotatedBy(MathHelper.ToRadians(10));
									Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, direction * 1, ModContent.ProjectileType<InfectionMissile>(), 32, 0);
								}
							}
							break;
						case 4:
							if (NPC.life <= NPC.lifeMax / 2)
							{
								if (attackCounter == 199)
								{
									NPC.damage = (int)(NPC.damage * 1.5f);
									speed = 24f;
									turnSpeed = 0.29f;
								}
								if (attackCounter == 2)
								{
									speed = 14.5f;
									turnSpeed = 0.115f;
									NPC.damage = (int)(NPC.damage - (NPC.damage / 3));
								}
							}
							else
							{
								if (attackCounter == 199)
								{
									NPC.damage = (int)(NPC.damage * 1.5f);
									speed = 20f;
									turnSpeed = 0.23f;
								}
								if (attackCounter == 2)
								{
									speed = 14.5f;
									turnSpeed = 0.115f;
									NPC.damage = (int)(NPC.damage - (NPC.damage / 3));
								}
							}
							break;
						case 5:
						case 6:
							if (attackCounter == 15)
							{
								SoundEngine.PlaySound(SoundID.NPCDeath6, NPC.Center);
								if (NPC.life <= NPC.lifeMax / 2)
								{
									for (var i = 0; i < 15; i++)
									{
										Vector2 vel = new Vector2(5, 5).SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToDegrees(24 * i));
										Projectile p = Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center, vel, ModContent.ProjectileType<InfectionWave>(), 35, 1);
									}
								}
                                else
                                {
									for (var i = 0; i < 10; i++)
									{
										Vector2 vel = new Vector2(5, 5).SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToDegrees(36 * i));
										Projectile p = Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center, vel, ModContent.ProjectileType<InfectionWave>(), 20, 1);
									}
								}
							}
							break;
                    }
                }
				else
                {
					waitTimer--;
                }

				// breath 'attack'
				if (breathTimer > 0)
				{
					breathTimer--; // tick down the attack counter.
					if (attackType == 4) // spawns more often when charging
                    {
						breathTimer--;
                    }
				}

				Player target = Main.player[NPC.target];
				// If the attack counter is 0, this NPC is less than 12.5 tiles away from its target, and has a path to the target unobstructed by blocks, summon a projectile.
				if (breathTimer <= 0 && Collision.CanHit(NPC.Center, 1, 1, target.Center, 1, 1))
				{
					Vector2 direction = NPC.velocity.SafeNormalize(Vector2.UnitX) * (4f + Main.rand.NextFloat());

					Vector2 dir1 = direction.RotatedBy(MathHelper.ToRadians(-90));
					int projectile = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, dir1 * 1, ModContent.ProjectileType<InfectionCloud>(), 16, 0);
					Vector2 dir2 = direction.RotatedBy(MathHelper.ToRadians(90));
					int projectile2 = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, dir2 * 1, ModContent.ProjectileType<InfectionCloud>(), 16, 0);

					breathTimer = 8 + (int)((NPC.life / NPC.lifeMax) * 12); // spawns more often the lower the health
					NPC.netUpdate = true;
				}
			}
		}
		
	}

	internal class ChasmBody : ChasmBoss
	{

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
			NPC.lifeMax = 90000;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.defense = 75;
			NPC.damage = 40;
		}

		int HitTimer = 120;

		public override bool? CanBeHitByProjectile(Projectile projectile)
		{
			if (HitTimer > 0)
			{
				return false;
			}
			return true;
		}

		public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
		{
			if (projectile.penetrate > 1) 
			{
				HitTimer = 45 + (projectile.penetrate * 4);
			}
			else if (projectile.penetrate <= -1)
			{
				HitTimer = (int)(60 + (projectile.damage * 0.4f));
			}
		}

		public override void CustomBehavior()
		{
			HitTimer--;
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
    }



	internal class ChasmTail : ChasmBoss
	{

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
			NPC.width = NPC.height = 50;
			NPC.lifeMax = 90000;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.defense = 30;
			NPC.damage = 32;
		}

		int HitTimer = 120;

		public override bool? CanBeHitByProjectile(Projectile projectile)
		{
			if (HitTimer > 0)
			{
				return false;
			}
			return true;
		}

        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
			if (projectile.penetrate > 1)
			{
				HitTimer = 30 + (projectile.penetrate * 3);
			}
			else if (projectile.penetrate <= -1)
			{
				HitTimer = 45 + (projectile.damage / 3);
			}
		}

        public override void CustomBehavior()
        {
			HitTimer--;
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



	// I made this 2nd base class to limit code repetition.
	public abstract class ChasmBoss : Worm
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chasm");
			NPCID.Sets.MPAllowedEnemies[Type] = true;
		}

		public override void Init()
		{
			minLength = 50;
			maxLength = 50;
			tailType = ModContent.NPCType<ChasmTail>();
			bodyType = ModContent.NPCType<ChasmBody>();
			headType = ModContent.NPCType<ChasmHead>();
			speed = 14.5f;
			turnSpeed = 0.115f;
			flies = true;
		}
	}

	// copy pasted from git hub becuase no way in hell id be able to make this myself

	// ported from my tAPI mod because I'm lazy
	// This abstract class can be used for non splitting worm type NPC.
	public abstract class Worm : ModNPC
	{
		/* ai[0] = follower
		* ai[1] = following
		* ai[2] = distanceFromTail
		* ai[3] = head
		*/
		public bool head;
		public bool tail;
		public int minLength;
		public int maxLength;
		public int headType;
		public int bodyType;
		public int tailType;
		public bool flies = false;
		public bool directional = false;
		public float speed;
		public float turnSpeed;

		public Vector2 targetPos = Vector2.Zero;

		public override void AI()
		{
			if (NPC.localAI[1] == 0f)
			{
				NPC.localAI[1] = 1f;
				Init();
			}
			if (NPC.ai[3] > 0f)
			{
				NPC.realLife = (int)NPC.ai[3];
			}
			if (!head && NPC.timeLeft < 300)
			{
				NPC.timeLeft = 300;
			}
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead)
			{
				NPC.TargetClosest(true);
				targetPos = Main.player[NPC.target].position;
			}

			// put this here so i can try and modify desired position to move to
			CustomBehavior();
			
			if (Main.player[NPC.target].dead && NPC.timeLeft > 300)
			{
				NPC.timeLeft = 300;
			}
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				if (!tail && NPC.ai[0] == 0f)
				{
					if (head)
					{
						NPC.ai[3] = (float)NPC.whoAmI;
						NPC.realLife = NPC.whoAmI;
						NPC.ai[2] = (float)Main.rand.Next(minLength, maxLength + 1);
						NPC.ai[0] = (float)NPC.NewNPC(NPC.GetSource_FromThis(), (int)(NPC.position.X + (float)(NPC.width / 2)), (int)(NPC.position.Y + (float)NPC.height), bodyType, NPC.whoAmI);
					}
					else if (NPC.ai[2] > 0f)
					{
						NPC.ai[0] = (float)NPC.NewNPC(NPC.GetSource_FromThis(), (int)(NPC.position.X + (float)(NPC.width / 2)), (int)(NPC.position.Y + (float)NPC.height), NPC.type, NPC.whoAmI);
					}
					else
					{
						NPC.ai[0] = (float)NPC.NewNPC(NPC.GetSource_FromThis(), (int)(NPC.position.X + (float)(NPC.width / 2)), (int)(NPC.position.Y + (float)NPC.height), tailType, NPC.whoAmI);
					}
					Main.npc[(int)NPC.ai[0]].ai[3] = NPC.ai[3];
					Main.npc[(int)NPC.ai[0]].realLife = NPC.realLife;
					Main.npc[(int)NPC.ai[0]].ai[1] = (float)NPC.whoAmI;
					Main.npc[(int)NPC.ai[0]].ai[2] = NPC.ai[2] - 1f;
					NPC.netUpdate = true;
				}
				if (!head && (!Main.npc[(int)NPC.ai[1]].active || Main.npc[(int)NPC.ai[1]].type != headType && Main.npc[(int)NPC.ai[1]].type != bodyType))
				{
					NPC.life = 0;
					NPC.HitEffect(0, 10.0);
					NPC.active = false;
				}
				if (!tail && (!Main.npc[(int)NPC.ai[0]].active || Main.npc[(int)NPC.ai[0]].type != bodyType && Main.npc[(int)NPC.ai[0]].type != tailType))
				{
					NPC.life = 0;
					NPC.HitEffect(0, 10.0);
					NPC.active = false;
				}
				if (!NPC.active && Main.netMode == NetmodeID.Server)
				{
					NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, NPC.whoAmI, -1f, 0f, 0f, 0, 0, 0);
				}
			}
			int num180 = (int)(NPC.position.X / 16f) - 1;
			int num181 = (int)((NPC.position.X + (float)NPC.width) / 16f) + 2;
			int num182 = (int)(NPC.position.Y / 16f) - 1;
			int num183 = (int)((NPC.position.Y + (float)NPC.height) / 16f) + 2;
			if (num180 < 0)
			{
				num180 = 0;
			}
			if (num181 > Main.maxTilesX)
			{
				num181 = Main.maxTilesX;
			}
			if (num182 < 0)
			{
				num182 = 0;
			}
			if (num183 > Main.maxTilesY)
			{
				num183 = Main.maxTilesY;
			}
			bool flag18 = flies;
			if (!flag18)
			{
				for (int num184 = num180; num184 < num181; num184++)
				{
					for (int num185 = num182; num185 < num183; num185++)
					{
						if (Main.tile[num184, num185] != null && (Main.tile[num184, num185].HasUnactuatedTile && (Main.tileSolid[(int)Main.tile[num184, num185].TileType] || Main.tileSolidTop[(int)Main.tile[num184, num185].TileType] && Main.tile[num184, num185].TileFrameY == 0) || Main.tile[num184, num185].LiquidAmount > 64))
						{
							Vector2 vector17;
							vector17.X = (float)(num184 * 16);
							vector17.Y = (float)(num185 * 16);
							if (NPC.position.X + (float)NPC.width > vector17.X && NPC.position.X < vector17.X + 16f && NPC.position.Y + (float)NPC.height > vector17.Y && NPC.position.Y < vector17.Y + 16f)
							{
								flag18 = true;
								if (Main.rand.NextBool(100) && NPC.behindTiles && Main.tile[num184, num185].HasUnactuatedTile)
								{
									WorldGen.KillTile(num184, num185, true, true, false);
								}
								if (Main.netMode != NetmodeID.MultiplayerClient && Main.tile[num184, num185].TileType == 2)
								{
									ushort arg_BFCA_0 = Main.tile[num184, num185 - 1].TileType;
								}
							}
						}
					}
				}
			}
			if (!flag18 && head)
			{
				Rectangle rectangle = new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height);
				int num186 = 1000;
				bool flag19 = true;
				for (int num187 = 0; num187 < 255; num187++)
				{
					if (Main.player[num187].active)
					{
						Rectangle rectangle2 = new Rectangle((int)targetPos.X - num186, (int)targetPos.Y - num186, num186 * 2, num186 * 2);
						if (rectangle.Intersects(rectangle2))
						{
							flag19 = false;
							break;
						}
					}
				}
				if (flag19)
				{
					flag18 = true;
				}
			}
			if (directional)
			{
				if (NPC.velocity.X < 0f)
				{
					NPC.spriteDirection = 1;
				}
				else if (NPC.velocity.X > 0f)
				{
					NPC.spriteDirection = -1;
				}
			}
			float num188 = speed;
			float num189 = turnSpeed;
			Vector2 vector18 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
			float num191 = targetPos.X + (float)(Main.player[NPC.target].width / 2);
			float num192 = targetPos.Y + (float)(Main.player[NPC.target].height / 2);
			num191 = (float)((int)(num191 / 16f) * 16);
			num192 = (float)((int)(num192 / 16f) * 16);
			vector18.X = (float)((int)(vector18.X / 16f) * 16);
			vector18.Y = (float)((int)(vector18.Y / 16f) * 16);
			num191 -= vector18.X;
			num192 -= vector18.Y;
			float num193 = (float)System.Math.Sqrt((double)(num191 * num191 + num192 * num192));
			if (NPC.ai[1] > 0f && NPC.ai[1] < (float)Main.npc.Length)
			{
				try
				{
					vector18 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
					num191 = Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - vector18.X;
					num192 = Main.npc[(int)NPC.ai[1]].position.Y + (float)(Main.npc[(int)NPC.ai[1]].height / 2) - vector18.Y;
				}
				catch
				{
				}
				NPC.rotation = (float)System.Math.Atan2((double)num192, (double)num191) + 1.57f;
				num193 = (float)System.Math.Sqrt((double)(num191 * num191 + num192 * num192));
				int num194 = NPC.width;
				num193 = (num193 - (float)num194) / num193;
				num191 *= num193;
				num192 *= num193;
				NPC.velocity = Vector2.Zero;
				NPC.position.X = NPC.position.X + num191;
				NPC.position.Y = NPC.position.Y + num192;
				if (directional)
				{
					if (num191 < 0f)
					{
						NPC.spriteDirection = 1;
					}
					if (num191 > 0f)
					{
						NPC.spriteDirection = -1;
					}
				}
			}
			else
			{
				if (!flag18)
				{
					NPC.TargetClosest(true);
					NPC.velocity.Y = NPC.velocity.Y + 0.11f;
					if (NPC.velocity.Y > num188)
					{
						NPC.velocity.Y = num188;
					}
					if ((double)(System.Math.Abs(NPC.velocity.X) + System.Math.Abs(NPC.velocity.Y)) < (double)num188 * 0.4)
					{
						if (NPC.velocity.X < 0f)
						{
							NPC.velocity.X = NPC.velocity.X - num189 * 1.1f;
						}
						else
						{
							NPC.velocity.X = NPC.velocity.X + num189 * 1.1f;
						}
					}
					else if (NPC.velocity.Y == num188)
					{
						if (NPC.velocity.X < num191)
						{
							NPC.velocity.X = NPC.velocity.X + num189;
						}
						else if (NPC.velocity.X > num191)
						{
							NPC.velocity.X = NPC.velocity.X - num189;
						}
					}
					else if (NPC.velocity.Y > 4f)
					{
						if (NPC.velocity.X < 0f)
						{
							NPC.velocity.X = NPC.velocity.X + num189 * 0.9f;
						}
						else
						{
							NPC.velocity.X = NPC.velocity.X - num189 * 0.9f;
						}
					}
				}
				else
				{
					if (!flies && NPC.behindTiles && NPC.soundDelay == 0)
					{
						float num195 = num193 / 40f;
						if (num195 < 10f)
						{
							num195 = 10f;
						}
						if (num195 > 20f)
						{
							num195 = 20f;
						}
						NPC.soundDelay = (int)num195;
						SoundEngine.PlaySound(SoundID.Roar, NPC.position); // 1
					}
					num193 = (float)System.Math.Sqrt((double)(num191 * num191 + num192 * num192));
					float num196 = System.Math.Abs(num191);
					float num197 = System.Math.Abs(num192);
					float num198 = num188 / num193;
					num191 *= num198;
					num192 *= num198;
					if (ShouldRun())
					{
						bool flag20 = true;
						for (int num199 = 0; num199 < 255; num199++)
						{
							if (Main.player[num199].active && !Main.player[num199].dead && Main.player[num199].ZoneCorrupt)
							{
								flag20 = false;
							}
						}
						if (flag20)
						{
							if (Main.netMode != NetmodeID.MultiplayerClient && (double)(NPC.position.Y / 16f) > (Main.rockLayer + (double)Main.maxTilesY) / 2.0)
							{
								NPC.active = false;
								int num200 = (int)NPC.ai[0];
								while (num200 > 0 && num200 < 200 && Main.npc[num200].active && Main.npc[num200].aiStyle == NPC.aiStyle)
								{
									int num201 = (int)Main.npc[num200].ai[0];
									Main.npc[num200].active = false;
									NPC.life = 0;
									if (Main.netMode == NetmodeID.Server)
									{
										NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num200, 0f, 0f, 0f, 0, 0, 0);
									}
									num200 = num201;
								}
								if (Main.netMode == NetmodeID.Server)
								{
									NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, NPC.whoAmI, 0f, 0f, 0f, 0, 0, 0);
								}
							}
							num191 = 0f;
							num192 = num188;
						}
					}

					bool flag21 = false;

					if (!flag21)
					{
						if (NPC.velocity.X > 0f && num191 > 0f || NPC.velocity.X < 0f && num191 < 0f || NPC.velocity.Y > 0f && num192 > 0f || NPC.velocity.Y < 0f && num192 < 0f)
						{
							if (NPC.velocity.X < num191)
							{
								NPC.velocity.X = NPC.velocity.X + num189;
							}
							else
							{
								if (NPC.velocity.X > num191)
								{
									NPC.velocity.X = NPC.velocity.X - num189;
								}
							}
							if (NPC.velocity.Y < num192)
							{
								NPC.velocity.Y = NPC.velocity.Y + num189;
							}
							else
							{
								if (NPC.velocity.Y > num192)
								{
									NPC.velocity.Y = NPC.velocity.Y - num189;
								}
							}
							if ((double)System.Math.Abs(num192) < (double)num188 * 0.2 && (NPC.velocity.X > 0f && num191 < 0f || NPC.velocity.X < 0f && num191 > 0f))
							{
								if (NPC.velocity.Y > 0f)
								{
									NPC.velocity.Y = NPC.velocity.Y + num189 * 2f;
								}
								else
								{
									NPC.velocity.Y = NPC.velocity.Y - num189 * 2f;
								}
							}
							if ((double)System.Math.Abs(num191) < (double)num188 * 0.2 && (NPC.velocity.Y > 0f && num192 < 0f || NPC.velocity.Y < 0f && num192 > 0f))
							{
								if (NPC.velocity.X > 0f)
								{
									NPC.velocity.X = NPC.velocity.X + num189 * 2f;
								}
								else
								{
									NPC.velocity.X = NPC.velocity.X - num189 * 2f;
								}
							}
						}
						else
						{
							if (num196 > num197)
							{
								if (NPC.velocity.X < num191)
								{
									NPC.velocity.X = NPC.velocity.X + num189 * 1.1f;
								}
								else if (NPC.velocity.X > num191)
								{
									NPC.velocity.X = NPC.velocity.X - num189 * 1.1f;
								}
								if ((double)(System.Math.Abs(NPC.velocity.X) + System.Math.Abs(NPC.velocity.Y)) < (double)num188 * 0.5)
								{
									if (NPC.velocity.Y > 0f)
									{
										NPC.velocity.Y = NPC.velocity.Y + num189;
									}
									else
									{
										NPC.velocity.Y = NPC.velocity.Y - num189;
									}
								}
							}
							else
							{
								if (NPC.velocity.Y < num192)
								{
									NPC.velocity.Y = NPC.velocity.Y + num189 * 1.1f;
								}
								else if (NPC.velocity.Y > num192)
								{
									NPC.velocity.Y = NPC.velocity.Y - num189 * 1.1f;
								}
								if ((double)(System.Math.Abs(NPC.velocity.X) + System.Math.Abs(NPC.velocity.Y)) < (double)num188 * 0.5)
								{
									if (NPC.velocity.X > 0f)
									{
										NPC.velocity.X = NPC.velocity.X + num189;
									}
									else
									{
										NPC.velocity.X = NPC.velocity.X - num189;
									}
								}
							}
						}
					}
				}
				NPC.rotation = (float)System.Math.Atan2((double)NPC.velocity.Y, (double)NPC.velocity.X) + 1.57f;
				if (head)
				{
					if (flag18)
					{
						if (NPC.localAI[0] != 1f)
						{
							NPC.netUpdate = true;
						}
						NPC.localAI[0] = 1f;
					}
					else
					{
						if (NPC.localAI[0] != 0f)
						{
							NPC.netUpdate = true;
						}
						NPC.localAI[0] = 0f;
					}
					if ((NPC.velocity.X > 0f && NPC.oldVelocity.X < 0f || NPC.velocity.X < 0f && NPC.oldVelocity.X > 0f || NPC.velocity.Y > 0f && NPC.oldVelocity.Y < 0f || NPC.velocity.Y < 0f && NPC.oldVelocity.Y > 0f) && !NPC.justHit)
					{
						NPC.netUpdate = true;
						return;
					}
				}
			}
			
		}

		public virtual void Init()
		{
		}

		public virtual bool ShouldRun()
		{
			return false;
		}

		public virtual void CustomBehavior()
		{
		}

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			return head ? (bool?)null : false;
		}
	}
}