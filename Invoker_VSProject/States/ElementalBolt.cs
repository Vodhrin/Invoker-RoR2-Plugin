using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using RoR2.Audio;
using R2API.Networking;
using R2API.Networking.Interfaces;
using EntityStates;

namespace Invoker.States
{
    class ElementalBolt : BaseSkillState
    {
        public static float baseDuration = Core.Config.primaryBaseAttackDuration.Value;
        public static float damageCoefficient = Core.Config.primaryDamageCoefficient.Value;
        public static float procCoefficient = Core.Config.primaryProcCoefficient.Value;
    }
}
