using System;
using UnityEngine;
using RoR2.Orbs;

namespace Invoker.Miscellaneous
{
    class ElementalBoltOrb : GenericDamageOrb
    {
        public override void Begin()
        {
            this.speed = 40f;
            base.Begin();
        }

        public override void OnArrival()
        {
            base.OnArrival();
            if (this.target)
            {
                RoR2.Util.PlaySound(Core.Assets.attackHitSound, this.target.gameObject);
            }
        }

        public override GameObject GetOrbEffect()
        {
            return InvokerPlugin.elementalBoltOrb;
        }
    }
}
