using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using RoR2.Networking;
using RoR2.Audio;
using R2API.Networking;
using R2API.Networking.Interfaces;
using EntityStates;

namespace Invoker.States
{
    class InvokerMain : GenericCharacterMain
    {
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.healthComponent.combinedHealth <= base.healthComponent.fullCombinedHealth / 3)
            {
                this.GetModelAnimator().SetBool("isLowHealth", true);
            }
            else
            {
                this.GetModelAnimator().SetBool("isLowHealth", false);
            }
        }
    }
}
