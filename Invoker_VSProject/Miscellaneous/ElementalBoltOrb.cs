using System;
using UnityEngine;
using RoR2.Orbs;

namespace Invoker.Miscellaneous
{
    class ElementalBoltOrb : GenericDamageOrb
    {
        public override void Begin()
        {
            this.speed = 35f;
            base.Begin();
        }
        public override GameObject GetOrbEffect()
        {
            return InvokerPlugin.elementalBoltOrb;
        }
    }
}
