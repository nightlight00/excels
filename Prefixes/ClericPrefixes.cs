using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace excels.Prefixes
{
    internal class RadiantPrefix : ModPrefix
    {
        public virtual float damage => 1;
        public virtual float useSpeed => 1;
        public virtual int critChance => 0;
        public virtual float shootSpeed => 1;
        public virtual float manaCost => 1;
        public virtual float knockback => 1;

        public override PrefixCategory Category => PrefixCategory.Custom;

        public override string Name => "R A AD IANT PREFIX";

        public override bool CanRoll(Item item)
        {
            return true; // (item.DamageType==ClericClass.Generic);
        }

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            damageMult *= damage;
            knockbackMult *= knockback;
            critBonus += critChance;
            useTimeMult *= useSpeed;
            shootSpeedMult *= shootSpeed;
            manaMult *= manaCost;
            
        }
    }

    internal class DivinePrefix : RadiantPrefix
    {
        public override string Name => "Divine";

        public override float damage => 1.14f;
        public override float useSpeed => .91f;
        public override int critChance => 8;
        public override float shootSpeed => 1.09f;
        public override float manaCost => 0.9f;
    }
    internal class DivineNoManaPrefix : RadiantPrefix
    {
        public override string Name => "Satanic";

        public override float damage => 1.18f;
        public override float useSpeed => .88f;
        public override int critChance => 6;
        public override float shootSpeed => 1.12f;
        public override float knockback => 1.1f;
    }

    internal class BlessedPrefix : RadiantPrefix
    {
        public override string Name => "Blessed";

        public override float damage => 1.06f;
        public override float useSpeed => .92f;
        public override float shootSpeed => 1.08f;
    }

    internal class SacredPrefix : RadiantPrefix
    {
        public override string Name => "Sacred";

        public override float useSpeed => .95f;
        public override int critChance => 2;
        public override float shootSpeed => 1.05f;
        public override float manaCost => 0.92f;
    }

    internal class HolyPrefix : RadiantPrefix
    {
        public override string Name => "Holy";

        public override float useSpeed => .93f;
        public override float shootSpeed => 1.15f;
        public override float manaCost => 0.95f;
    }

    internal class UnholyPrefix : RadiantPrefix
    {
        public override string Name => "Unholy";

        public override float damage => 1.12f;
        public override int critChance => 4;
        public override float knockback => 1.08f;
    }

    internal class AttunedPrefix : RadiantPrefix
    {
        public override string Name => "Attuned";

        public override float damage => 1.08f;
        public override float useSpeed => .95f;
    }

    internal class UnattunedPrefix : RadiantPrefix
    {
        public override string Name => "Unattuned";

        public override float damage => 0.95f;
        public override float useSpeed => 1.05f;
    }

    internal class CrazedPrefix : RadiantPrefix
    {
        public override string Name => "Crazed";

        public override float damage => 1.05f;
        public override float useSpeed => 1.05f;
        public override int critChance => -2;
        public override float manaCost => 1.12f;
    }
    internal class CrazedNoManaPrefix : RadiantPrefix
    {
        public override string Name => "Murderous";

        public override float damage => 1.05f;
        public override float useSpeed => 1.05f;
        public override int critChance => -2;
    }

    internal class ForgottenPrefix : RadiantPrefix
    {
        public override string Name => "Forgotten";

        public override float damage => 0.86f;
        public override float useSpeed => 1.12f;
        public override int critChance => -3;
        public override float shootSpeed => 0.9f;
        public override float manaCost => 1.19f;
    }
    internal class ForgottenNoManaPrefix : RadiantPrefix
    {
        public override string Name => "Forbidden";

        public override float damage => 0.83f;
        public override float useSpeed => 1.12f;
        public override int critChance => -3;
        public override float shootSpeed => 0.9f;
    }
}
