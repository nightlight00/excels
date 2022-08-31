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
using Terraria.GameContent;
using Terraria.Localization;
using System.Collections.Generic;
using Terraria.Chat;
using System.IO;
using Terraria.Audio;

namespace excels.NPCs.Stellar
{
    internal class StellarShip1 : ModNPC
    {
		public int ParentIndex
		{
			get => (int)NPC.ai[0] - 1;
			set => NPC.ai[0] = value + 1;
		}

		public bool HasParent => ParentIndex 
			> -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stellar Ship AF-004");
			// 004 of the Attack Fleet

			NPCID.Sets.DebuffImmunitySets.Add(Type, new NPCDebuffImmunityData
			{
				SpecificallyImmuneTo = new int[] {
					BuffID.Confused,
					BuffID.Poisoned,
					BuffID.OnFire,
					BuffID.OnFire3,
					BuffID.Ichor,
					BuffID.CursedInferno,
					BuffID.Frostburn,
					BuffID.Frostburn2,
					BuffID.Slow,
					BuffID.Bleeding
				}
			});
			NPCID.Sets.MPAllowedEnemies[Type] = true;


			NPCID.Sets.TrailCacheLength[NPC.type] = 10;
			NPCID.Sets.TrailingMode[NPC.type] = 1;

			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
		}

		public override void SetDefaults()
		{
			NPC.width = 96;
			NPC.height = 54;
			NPC.damage = 20;
			NPC.defense = 12;
			NPC.lifeMax = 3400;
			NPC.HitSound = SoundID.NPCHit9;
			NPC.DeathSound = SoundID.NPCDeath11;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.knockBackResist = 0f;
			NPC.netAlways = true;

			NPC.aiStyle = -1;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("NPCs/Stellar/StellarShip1").Value;
			var offset = new Vector2(NPC.width / 2f, NPC.height / 2f);
			Vector2 origin = NPC.frame.Size() / 2f;
			for (int k = 0; k < NPC.oldPos.Length; k++)
			{ // only every second one is drawn
				if (k % 2 == 0)
				{
					//NPC.oldPos[k]
					Vector2 drawPos = NPC.oldPos[k] + new Vector2(0, 4) - Main.screenPosition;
					var trailColor = NPC.GetAlpha(new Color(120, 120, 120, 30) * (1f - (float)NPC.alpha / 255f)) * ((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length); //drawColor * 0.1f;
					Main.spriteBatch.Draw(texture, drawPos + offset, null, trailColor, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0);
				}
			}

			return true;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life < 0)
			{
				for (var i = 0; i < 25; i++)
				{
					Dust d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 31);
					d.noGravity = true;
					d.velocity *= Main.rand.NextFloat(0.8f, 1.4f);
					d.scale *= 1.4f;
				}

				if (Main.netMode == NetmodeID.Server)
				{
					// We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
					return;
				}
				Gore.NewGoreDirect(NPC.GetSource_FromThis(), NPC.Top, NPC.velocity.RotatedByRandom(MathHelper.ToRadians(8)) * Main.rand.NextFloat(0.8f, 1.4f) + new Vector2(0, -2), Mod.Find<ModGore>("StellarGoreSmallGlass1").Type);
				Gore.NewGoreDirect(NPC.GetSource_FromThis(), NPC.Top, NPC.velocity.RotatedByRandom(MathHelper.ToRadians(8)) * Main.rand.NextFloat(0.8f, 1.4f) + new Vector2(0, -2), Mod.Find<ModGore>("StellarGoreSmallGlass2").Type);

				Gore.NewGoreDirect(NPC.GetSource_FromThis(), NPC.Left, NPC.velocity.RotatedByRandom(MathHelper.ToRadians(8)) * Main.rand.NextFloat(0.8f, 1.4f), Mod.Find<ModGore>("StellarGoreSmallLightR").Type);
				Gore.NewGoreDirect(NPC.GetSource_FromThis(), NPC.Bottom, NPC.velocity.RotatedByRandom(MathHelper.ToRadians(8)) * Main.rand.NextFloat(0.8f, 1.4f), Mod.Find<ModGore>("StellarGoreSmallLightR").Type);
				Gore.NewGoreDirect(NPC.GetSource_FromThis(), NPC.Right, NPC.velocity.RotatedByRandom(MathHelper.ToRadians(8)) * Main.rand.NextFloat(0.8f, 1.4f), Mod.Find<ModGore>("StellarGoreSmallLightR").Type);

				for (var a = 0; a < 2; a++)
				{
					Gore.NewGoreDirect(NPC.GetSource_FromThis(), NPC.Center, NPC.velocity.RotatedByRandom(MathHelper.ToRadians(8)) * Main.rand.NextFloat(0.8f, 1.4f), Mod.Find<ModGore>("StellarGorePlating").Type);
				}
				for (var i = 0; i < 3; i++)
				{
					Gore.NewGoreDirect(NPC.GetSource_FromThis(), NPC.Center, NPC.velocity.RotatedByRandom(MathHelper.ToRadians(8)) * Main.rand.NextFloat(0.5f, 0.9f), Mod.Find<ModGore>("StellarGoreRim").Type);
				}
			}
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			// draw 'glowmask'
			Texture2D texture = Mod.Assets.Request<Texture2D>("NPCs/Stellar/StellarShip1_Glow").Value;
			var offset = new Vector2(NPC.width / 2f, NPC.height / 2f);
			Vector2 origin = NPC.frame.Size() / 2f;
			Main.spriteBatch.Draw(texture, (NPC.position + new Vector2(0, 4) - Main.screenPosition) + offset, null, NPC.GetAlpha(new Color(155, 155, 155, 155)), NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0);
		}

		private bool Despawn()
		{
			if (Main.netMode != NetmodeID.MultiplayerClient &&
				(!HasParent || !Main.npc[ParentIndex].active || Main.npc[ParentIndex].type != ModContent.NPCType<StellarStarship>()))
			{
				// * Not spawned by the boss body (didn't assign a position and parent) or
				// * Parent isn't active or
				// * Parent isn't the body
				// => invalid, kill itself without dropping any items
				NPC.active = false;
				NPC.life = 0;
				NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
				return true;
			}
			return false;
		}

		Vector2 TargetPos = Vector2.Zero;
		float speedScale = 9;

		public override void AI()
		{
			if (Despawn())
			{
				return;
			}

			NPC.TargetClosest(true);

			switch (NPC.ai[1])
            {
				default: // top left
					TargetPos = Main.player[Main.npc[ParentIndex].target].Center - new Vector2(500, 225);
					break;

				case 1: // left
					TargetPos = Main.player[Main.npc[ParentIndex].target].Center - new Vector2(600, 0);
					break;

				case 2: // bottom left
					TargetPos = Main.player[Main.npc[ParentIndex].target].Center - new Vector2(500, -225);
					break;

				case 3: // bottom
					TargetPos = Main.player[Main.npc[ParentIndex].target].Center - new Vector2(0, -300);
					break;

				case 4: // bottom right
					TargetPos = Main.player[Main.npc[ParentIndex].target].Center - new Vector2(-500, -225);
					break;

				case 5: // right
					TargetPos = Main.player[Main.npc[ParentIndex].target].Center - new Vector2(-600, 0);
					break;

				case 6: // top right
					TargetPos = Main.player[Main.npc[ParentIndex].target].Center - new Vector2(-500, 225);
					break;

				case 7: // top
					TargetPos = Main.player[Main.npc[ParentIndex].target].Center - new Vector2(0, 300);
					break;
			} 

			NPC.velocity = (TargetPos - NPC.Center).SafeNormalize(Vector2.Zero) * speedScale;
			speedScale += 0.07f;
			if (Vector2.Distance(TargetPos, NPC.Center) < 50)
            {
				//NPC.Center = TargetPos;
				//NPC.velocity = Vector2.Zero;

				if (Main.masterMode || (NPC.ai[1] == 0 || NPC.ai[1] == 2 || NPC.ai[1] == 4 || NPC.ai[1] == 6))
				{
					Vector2 vel = (Main.player[Main.npc[ParentIndex].target].Center - NPC.Center).SafeNormalize(Vector2.Zero) * 4;
					Projectile p = Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center, vel, ModContent.ProjectileType<StellarRocket>(), 20, 0);
					p.netUpdate = true;
					SoundEngine.PlaySound(SoundID.Item11, NPC.Center);
					if (!Main.expertMode)
                    {
						p.ai[1] = 1;
                    }
				}
				speedScale = 9;

				NPC.netUpdate = true;

				NPC.ai[1]++;
				if (NPC.ai[1] == 8)
                {
					NPC.ai[1] = 0;
                }
			}
		}
	}

	internal class StellarShip2 : ModNPC
	{
		public int ParentIndex
		{
			get => (int)NPC.ai[0] - 1;
			set => NPC.ai[0] = value + 1;
		}

		public bool HasParent => ParentIndex > -1;

        public override void SetStaticDefaults()
        {
			DisplayName.SetDefault("Stellar Ship SD-027");
			// #027 of the Support Divison
			NPCID.Sets.TrailCacheLength[NPC.type] = 10;
			NPCID.Sets.TrailingMode[NPC.type] = 1;
			NPCID.Sets.DebuffImmunitySets.Add(Type, new NPCDebuffImmunityData
			{
				SpecificallyImmuneTo = new int[] {
					BuffID.Confused,
					BuffID.Poisoned,
					BuffID.OnFire,
					BuffID.OnFire3,
					BuffID.Ichor,
					BuffID.CursedInferno,
					BuffID.Frostburn,
					BuffID.Frostburn2,
					BuffID.Slow,
					BuffID.Bleeding
				}
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
			NPC.width = 96;
			NPC.height = 54;
			NPC.damage = 20;
			NPC.defense = 18;
			NPC.lifeMax = 2600;
			NPC.HitSound = SoundID.NPCHit9;
			NPC.DeathSound = SoundID.NPCDeath11;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.knockBackResist = 0f;
			NPC.netAlways = true;

			NPC.aiStyle = -1;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("NPCs/Stellar/StellarShip2").Value;
			var offset = new Vector2(NPC.width / 2f, NPC.height / 2f);
			Vector2 origin = NPC.frame.Size() / 2f;
			for (int k = 0; k < NPC.oldPos.Length; k++)
			{ // only every second one is drawn
				if (k % 2 == 0)
				{
					//NPC.oldPos[k]
					Vector2 drawPos = NPC.oldPos[k] + new Vector2(0, 4) - Main.screenPosition;
					var trailColor = NPC.GetAlpha(new Color(120, 120, 120, 30) * (1f - (float)NPC.alpha / 255f)) * ((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length); //drawColor * 0.1f;
					Main.spriteBatch.Draw(texture, drawPos + offset, null, trailColor, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0);
				}
			}

			return true;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life < 0)
			{
				for (var i = 0; i < 25; i++)
				{
					Dust d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 31);
					d.noGravity = true;
					d.velocity *= Main.rand.NextFloat(0.8f, 1.4f);
					d.scale *= 1.4f;
				}

				if (Main.netMode == NetmodeID.Server)
				{
					// We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
					return;
				}
				Gore.NewGoreDirect(NPC.GetSource_FromThis(), NPC.Top, NPC.velocity.RotatedByRandom(MathHelper.ToRadians(8)) * Main.rand.NextFloat(0.8f, 1.4f) + new Vector2(0, -2), Mod.Find<ModGore>("StellarGoreSmallGlass1").Type);
				Gore.NewGoreDirect(NPC.GetSource_FromThis(), NPC.Top, NPC.velocity.RotatedByRandom(MathHelper.ToRadians(8)) * Main.rand.NextFloat(0.8f, 1.4f) + new Vector2(0, -2), Mod.Find<ModGore>("StellarGoreSmallGlass2").Type);

				Gore.NewGoreDirect(NPC.GetSource_FromThis(), NPC.Left, NPC.velocity.RotatedByRandom(MathHelper.ToRadians(8)) * Main.rand.NextFloat(0.8f, 1.4f), Mod.Find<ModGore>("StellarGoreSmallLightG").Type);
				Gore.NewGoreDirect(NPC.GetSource_FromThis(), NPC.Bottom, NPC.velocity.RotatedByRandom(MathHelper.ToRadians(8)) * Main.rand.NextFloat(0.8f, 1.4f), Mod.Find<ModGore>("StellarGoreSmallLightG").Type);
				Gore.NewGoreDirect(NPC.GetSource_FromThis(), NPC.Right, NPC.velocity.RotatedByRandom(MathHelper.ToRadians(8)) * Main.rand.NextFloat(0.8f, 1.4f), Mod.Find<ModGore>("StellarGoreSmallLightG").Type);

				for (var a = 0; a < 2; a++)
				{
					Gore.NewGoreDirect(NPC.GetSource_FromThis(), NPC.Center, NPC.velocity.RotatedByRandom(MathHelper.ToRadians(8)) * Main.rand.NextFloat(0.8f, 1.4f), Mod.Find<ModGore>("StellarGorePlating").Type);
				}
				for (var i = 0; i < 3; i++)
				{
					Gore.NewGoreDirect(NPC.GetSource_FromThis(), NPC.Center, NPC.velocity.RotatedByRandom(MathHelper.ToRadians(8)) * Main.rand.NextFloat(0.5f, 0.9f), Mod.Find<ModGore>("StellarGoreRim").Type);
				}
			}
		}
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			// draw 'glowmask'
			Texture2D texture = Mod.Assets.Request<Texture2D>("NPCs/Stellar/StellarShip2_Glow").Value;
			var offset = new Vector2(NPC.width / 2f, NPC.height / 2f);
			Vector2 origin = NPC.frame.Size() / 2f;
			Main.spriteBatch.Draw(texture, (NPC.position + new Vector2(0, 4) - Main.screenPosition) + offset, null, NPC.GetAlpha(new Color(155, 155, 155, 155)), NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0);
		}

		private bool Despawn()
		{
			if (Main.netMode != NetmodeID.MultiplayerClient &&
				(!HasParent || !Main.npc[ParentIndex].active || Main.npc[ParentIndex].type != ModContent.NPCType<StellarStarship>()))
			{
				// * Not spawned by the boss body (didn't assign a position and parent) or
				// * Parent isn't active or
				// * Parent isn't the body
				// => invalid, kill itself without dropping any items
				NPC.active = false;
				NPC.life = 0;
				NPC.HitEffect();
				NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
				return true;
			}
			return false;
		}

		Vector2 TargetPos = Vector2.Zero;

		int Speed = 150;
		int Heal = 150;

		public override void AI()
		{
			if (Despawn())
			{
				return;
			}

			if (Main.expertMode)
            {
				Speed = 127;
            }
			if (Main.masterMode)
            {
				Heal = 175;
            }

			NPC.ai[1]++;
			if (Main.rand.Next(7) == 0)
            {
				NPC.ai[1]--;
            }
			if (NPC.ai[1] > Speed)
            {
				if (Main.npc[ParentIndex].life < Main.npc[ParentIndex].lifeMax * 0.35f && Main.npc[ParentIndex].type == ModContent.NPCType<StellarStarship>())
                {
					Main.npc[ParentIndex].life += Heal;
					if (Main.npc[ParentIndex].life > Main.npc[ParentIndex].lifeMax * 0.35f)
                    {
						Main.npc[ParentIndex].life = (int)(Main.npc[ParentIndex].lifeMax * 0.35f);
					}
					Main.npc[ParentIndex].netUpdate = true;

					for (var i = 0; i < 30; i++)
                    {
						//Vector2 vel = (Main.npc[ParentIndex].Center - NPC.Center).SafeNormalize(Vector2.Zero) * (1.1f * (i + 1));
						Vector2 pos = NPC.Center + ((Main.npc[ParentIndex].Center - NPC.Center) / 30) * i;
						Dust d = Dust.NewDustDirect(pos, 0, 0, 110);
						d.velocity = Vector2.Zero;
						d.noGravity = true;
						d.scale = 2f - (i * 0.025f);
                    }

					SoundEngine.PlaySound(SoundID.Item91, NPC.Center);
					CombatText.NewText(Main.npc[ParentIndex].getRect(), CombatText.HealLife, Heal);
				}
				NPC.ai[1] = 0;
			}

			/*
			if (++NPC.ai[2] > 5)
            {
				Projectile p = Projectile.NewProjectileDirect(NPC.GetSpawnSource_ForProjectile(), NPC.Center, new Vector2(-8, 0), ProjectileID.GreenLaser, 10, 0);
				p.friendly = false;
				p.hostile = true;
				p.netUpdate = true;

				Projectile p2 = Projectile.NewProjectileDirect(NPC.GetSpawnSource_ForProjectile(), NPC.Center, new Vector2(0, -8), ProjectileID.GreenLaser, 10, 0);
				p2.friendly = false;
				p2.hostile = true;
				p2.netUpdate = true;

				NPC.netUpdate = true;
				NPC.ai[2] = 0;
			}
			*/

			NPC.TargetClosest(true);

			TargetPos = Main.player[Main.npc[ParentIndex].target].Center - new Vector2(0, 300);

			NPC.velocity = (TargetPos - NPC.Center).SafeNormalize(Vector2.Zero) * 5.3f;
			if (Vector2.Distance(TargetPos, NPC.Center) < 5)
			{
				NPC.Center = TargetPos;
				NPC.velocity = Vector2.Zero;
			}
		}
	}
}
