﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace excels.Buffs.Whips
{
	public class SkylineWhipDebuff : ModBuff
	{
		public override string Texture => "excels/Buffs/Whips/ShroomyBuff";
        public override void SetStaticDefaults()
		{
			// This allows the debuff to be inflicted on NPCs that would otherwise be immune to all debuffs.
			// Other mods may check it for different purposes.
			BuffID.Sets.IsAnNPCWhipDebuff[Type] = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<WhipDebuffNPC>().markedBySkylineWhip = true;
		}
	}
	public class PerrenialWhipDebuff : ModBuff
	{
		public override string Texture => "excels/Buffs/Whips/ShroomyBuff";
		public override void SetStaticDefaults()
		{
			// This allows the debuff to be inflicted on NPCs that would otherwise be immune to all debuffs.
			// Other mods may check it for different purposes.
			BuffID.Sets.IsAnNPCWhipDebuff[Type] = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<WhipDebuffNPC>().markedByFlowerWhip = true;
		}
	}


	public class WhipDebuffNPC : GlobalNPC
	{
		// This is required to store information on entities that isn't shared between them.
		public override bool InstancePerEntity => true;

		public bool markedBySkylineWhip;
		public bool markedByFlowerWhip;

		public override void ResetEffects(NPC npc)
		{
			markedBySkylineWhip = false;
			markedByFlowerWhip = false;
		}

		// TODO: Inconsistent with vanilla, increasing damage AFTER it is randomised, not before. Change to a different hook in the future.
		public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			// Only player attacks should benefit from this buff, hence the NPC and trap checks.
			if (markedBySkylineWhip && !projectile.npcProj && !projectile.trap && (projectile.minion || ProjectileID.Sets.MinionShot[projectile.type]))
			{
				damage = (int)(damage * 1.08f);
			}
			if (markedByFlowerWhip && !projectile.npcProj && !projectile.trap && (projectile.minion || ProjectileID.Sets.MinionShot[projectile.type]))
			{
				damage = (int)(damage * 1.15f);
			}
		}
	}
}