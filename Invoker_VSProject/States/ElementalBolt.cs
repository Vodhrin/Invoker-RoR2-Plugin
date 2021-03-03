using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using RoR2.Orbs;
using RoR2.Audio;
using R2API.Networking;
using R2API.Networking.Interfaces;
using EntityStates;

namespace Invoker.States
{
    class ElementalBolt : BaseSkillState
    {
        public float baseDuration = Core.Config.primaryBaseAttackDuration.Value;
        public float damageCoefficient = Core.Config.primaryDamageCoefficient.Value;
        public float procCoefficient = Core.Config.primaryProcCoefficient.Value;
		public string muzzleString;

		private float duration;
		private bool hasFired;
		protected bool isCrit;
		private HurtBox initialOrbTarget;
		private ChildLocator childLocator;
		private Miscellaneous.InvokerAimTracker invokerTracker;
		private Animator animator;

		public override void OnEnter()
		{
			base.OnEnter();
			this.invokerTracker = base.GetComponent<Miscellaneous.InvokerAimTracker>();
			this.duration = this.baseDuration / this.attackSpeedStat;
			this.hasFired = false;
			this.isCrit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);

			Transform modelTransform = base.GetModelTransform();

			if (modelTransform)
			{
				this.childLocator = modelTransform.GetComponent<ChildLocator>();
				this.animator = modelTransform.GetComponent<Animator>();
			}

			if (this.invokerTracker && base.isAuthority)
			{
				this.initialOrbTarget = this.invokerTracker.GetTrackingTarget();
			}
			if (base.characterBody)
			{
				base.characterBody.SetAimTimer(this.duration + 1f);
			}

            if (this.initialOrbTarget && this.animator)
            {
				Util.PlayScaledSound(Core.Assets.preAttackSound, base.gameObject, base.attackSpeedStat);

				bool attackSwitch = this.animator.GetBool("attackSwitch");
				if (attackSwitch)
				{
					this.muzzleString = "HandR";
					this.PlayAnimation("Gesture, Override", "Attack");
					this.animator.SetBool("attackSwitch", !attackSwitch);
				}
				else
				{
					this.muzzleString = "HandL";
					this.PlayAnimation("Gesture, Override", "Attack");
					this.animator.SetBool("attackSwitch", !attackSwitch);
				}
			}
		}

		public override void OnExit()
		{
			if(base.isAuthority && !hasFired)
            {
				this.FireOrbBolt();
            }

			base.OnExit();
		}

		protected virtual GenericDamageOrb CreateArrowBolt()
		{
			return new Miscellaneous.ElementalBoltOrb();
		}

		private void FireOrbBolt()
		{
			GenericDamageOrb genericDamageOrb = this.CreateArrowBolt();
			genericDamageOrb.damageValue = base.characterBody.damage * this.damageCoefficient;
			genericDamageOrb.isCrit = this.isCrit;
			genericDamageOrb.teamIndex = TeamComponent.GetObjectTeam(base.gameObject);
			genericDamageOrb.attacker = base.gameObject;
			genericDamageOrb.procCoefficient = this.procCoefficient;
			HurtBox hurtBox = this.initialOrbTarget;
			if (hurtBox)
			{
				Transform transform = this.childLocator.FindChild(this.muzzleString);
				Util.PlayScaledSound(Core.Assets.attackLaunchSound, base.gameObject, base.attackSpeedStat);
				genericDamageOrb.origin = transform.position;
				genericDamageOrb.target = hurtBox;
				OrbManager.instance.AddOrb(genericDamageOrb);
			}
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			
			if(base.isAuthority && base.fixedAge >= this.duration / 4 && !this.hasFired)
            {
				this.FireOrbBolt();
				this.hasFired = true;
            }

			if (base.fixedAge > this.duration && base.isAuthority)
			{
				this.outer.SetNextStateToMain();
				return;
			}
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Skill;
		}

		public override void OnSerialize(NetworkWriter writer)
		{
			writer.Write(HurtBoxReference.FromHurtBox(this.initialOrbTarget));
		}

		public override void OnDeserialize(NetworkReader reader)
		{
			this.initialOrbTarget = reader.ReadHurtBoxReference().ResolveHurtBox();
		}
	}
}
