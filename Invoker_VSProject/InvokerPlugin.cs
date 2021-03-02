using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using RoR2.Audio;
using RoR2.Skills;
using RoR2.Projectile;
using RoR2.Orbs;
using RoR2.CharacterAI;
using BepInEx;
using BepInEx.Configuration;
using R2API;
using R2API.Utils;
using R2API.Networking;
using R2API.Networking.Interfaces;
using EntityStates;
using KinematicCharacterController;


namespace Invoker
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin("com.Vodhr.Invoker", "Invoker Survivor", "0.0.1")]
    [R2APISubmoduleDependency(new string[]
{
        "PrefabAPI",
        "SurvivorAPI",
        "LoadoutAPI",
        "BuffAPI",
        "LanguageAPI",
        "SoundAPI",
        "EffectAPI",
        "OrbAPI",
        "UnlockablesAPI",
        "ResourcesAPI",
        "NetworkingAPI"
})]

    public class InvokerPlugin : BaseUnityPlugin
    {
        public static GameObject invokerBody;
        public static GameObject invokerDisplayBody;
        public static GameObject invokerDoppelgangerMaster;

        public static GameObject elementalBoltOrb;

        public static BepInEx.Logging.ManualLogSource logger;
        public static InvokerPlugin instance;

        public void Awake()
        {
            instance = this;

            Core.Assets.InitializeAssets();
            Core.Config.Read();
            CreateCharacter();
            InitializeCharacter();
            InitializeSkills();
            InitializeOrbs();
            InitializeProjectiles();
            CreateDoppelGanger();
        }

        private void CreateCharacter()
        {
            LanguageAPI.Add("INVOKER_NAME", "Invoker");
            LanguageAPI.Add("INVOKER_DESCRIPTION", "Carl." + Environment.NewLine);
            LanguageAPI.Add("INVOKER_SUBTITLE", "I Am Very Smart" + Environment.NewLine);
            LanguageAPI.Add("INVOKER_OUTRO_FLAVOR", "...and so he left, hungry for more knowledge.");
            //", with no new knowlege to show for it."
            //", without a shred of new knowledge."
            //", disgusted by the inconvenience."
            //I am bad at this.

            LanguageAPI.Add("INVOKER_SKIN_DEFAULT_NAME", "Default");

            invokerBody = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody"), "InvokerBody", true);

            invokerBody.GetComponent<NetworkIdentity>().localPlayerAuthority = true;

            GameObject model = CreateModel(invokerBody);

            GameObject gameObject = new GameObject("ModelBase");
            gameObject.transform.parent = invokerBody.transform;
            gameObject.transform.localPosition = new Vector3(0f, -0.81f, 0f);
            gameObject.transform.localRotation = Quaternion.identity;
            gameObject.transform.localScale = new Vector3(1f, 1f, 1f);

            GameObject gameObject2 = new GameObject("CameraPivot");
            gameObject2.transform.parent = gameObject.transform;
            gameObject2.transform.localPosition = new Vector3(0f, 1.6f, 0f);
            gameObject2.transform.localRotation = Quaternion.identity;
            gameObject2.transform.localScale = Vector3.one;

            GameObject gameObject3 = new GameObject("AimOrigin");
            gameObject3.transform.parent = gameObject.transform;
            gameObject3.transform.localPosition = new Vector3(0f, 1.4f, 0f);
            gameObject3.transform.localRotation = Quaternion.identity;
            gameObject3.transform.localScale = Vector3.one;

            Transform transform = model.transform;
            transform.parent = gameObject.transform;
            transform.localPosition = Vector3.zero;
            transform.localScale = new Vector3(1f, 1f, 1f);
            transform.localRotation = Quaternion.identity;

            CharacterDirection characterDirection = invokerBody.GetComponent<CharacterDirection>();
            characterDirection.moveVector = Vector3.zero;
            characterDirection.targetTransform = gameObject.transform;
            characterDirection.overrideAnimatorForwardTransform = null;
            characterDirection.rootMotionAccumulator = null;
            characterDirection.modelAnimator = model.GetComponentInChildren<Animator>();
            characterDirection.driveFromRootRotation = false;
            characterDirection.turnSpeed = 720f;

            CharacterBody bodyComponent = invokerBody.GetComponent<CharacterBody>();
            bodyComponent.bodyIndex = -1;
            bodyComponent.baseNameToken = "INVOKER_NAME"; 
            bodyComponent.subtitleNameToken = "INVOKER_SUBTITLE"; 
            bodyComponent.bodyFlags = CharacterBody.BodyFlags.ImmuneToExecutes;
            bodyComponent.rootMotionInMainState = false;
            bodyComponent.mainRootSpeed = 0;
            bodyComponent.baseMaxHealth = Core.Config.baseMaxHealth.Value;
            bodyComponent.levelMaxHealth = Core.Config.levelMaxHealth.Value;
            bodyComponent.baseRegen = Core.Config.baseRegen.Value;
            bodyComponent.levelRegen = Core.Config.levelRegen.Value;
            bodyComponent.baseMaxShield = Core.Config.baseMaxShield.Value;
            bodyComponent.levelMaxShield = Core.Config.levelMaxShield.Value;
            bodyComponent.baseMoveSpeed = Core.Config.baseMoveSpeed.Value;
            bodyComponent.levelMoveSpeed = Core.Config.levelMoveSpeed.Value;
            bodyComponent.baseAcceleration = Core.Config.baseAcceleration.Value;
            bodyComponent.baseJumpPower = Core.Config.baseJumpPower.Value;
            bodyComponent.levelJumpPower = Core.Config.levelJumpPower.Value;
            bodyComponent.baseJumpCount = Core.Config.baseJumpCount.Value;
            bodyComponent.baseDamage = Core.Config.baseDamage.Value;
            bodyComponent.levelDamage = Core.Config.levelDamage.Value;
            bodyComponent.baseAttackSpeed = Core.Config.baseAttackSpeed.Value;
            bodyComponent.levelAttackSpeed = Core.Config.levelAttackSpeed.Value;
            bodyComponent.baseCrit = Core.Config.baseCrit.Value;
            bodyComponent.levelCrit = Core.Config.levelCrit.Value;
            bodyComponent.baseArmor = Core.Config.baseArmor.Value;
            bodyComponent.levelArmor = Core.Config.levelArmor.Value;
            bodyComponent.sprintingSpeedMultiplier = Core.Config.sprintingSpeedMultiplier.Value;
            bodyComponent.wasLucky = false;
            bodyComponent.hideCrosshair = false;
            bodyComponent.aimOriginTransform = gameObject3.transform;
            bodyComponent.hullClassification = HullClassification.Human;
            bodyComponent.portraitIcon = Core.Assets.portrait.texture;
            bodyComponent.isChampion = false;
            bodyComponent.currentVehicle = null;
            bodyComponent.skinIndex = 0U;

            CharacterMotor characterMotor = invokerBody.GetComponent<CharacterMotor>();
            characterMotor.walkSpeedPenaltyCoefficient = 1f;
            characterMotor.characterDirection = characterDirection;
            characterMotor.muteWalkMotion = false;
            characterMotor.mass = 100f;
            characterMotor.airControl = 0.25f;
            characterMotor.disableAirControlUntilCollision = false;
            characterMotor.generateParametersOnAwake = true;
            characterMotor.useGravity = true;
            characterMotor.isFlying = false;

            InputBankTest inputBankTest = invokerBody.GetComponent<InputBankTest>();
            inputBankTest.moveVector = Vector3.zero;

            CameraTargetParams cameraTargetParams = invokerBody.GetComponent<CameraTargetParams>();
            cameraTargetParams.cameraParams = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponent<CameraTargetParams>().cameraParams;
            cameraTargetParams.cameraPivotTransform = null;
            cameraTargetParams.aimMode = CameraTargetParams.AimType.Standard;
            cameraTargetParams.recoil = Vector2.zero;
            cameraTargetParams.idealLocalCameraPos = Vector3.zero;
            cameraTargetParams.dontRaycastToPivot = false;

            ModelLocator modelLocator = invokerBody.GetComponent<ModelLocator>();
            modelLocator.modelTransform = transform;
            modelLocator.modelBaseTransform = gameObject.transform;
            modelLocator.dontReleaseModelOnDeath = false;
            modelLocator.autoUpdateModelTransform = true;
            modelLocator.dontDetatchFromParent = false;
            modelLocator.noCorpse = false;
            modelLocator.normalizeToFloor = false; 
            modelLocator.preserveModel = false;

            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            CharacterModel characterModel = model.AddComponent<CharacterModel>();
            characterModel.body = bodyComponent;
            List<CharacterModel.RendererInfo> rendererInfos = new List<CharacterModel.RendererInfo>();
            foreach (SkinnedMeshRenderer i in model.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                CharacterModel.RendererInfo info = new CharacterModel.RendererInfo
                {
                    defaultMaterial = i.material,
                    renderer = i,
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                };
                rendererInfos.Add(info);
            }
            characterModel.baseRendererInfos = rendererInfos.ToArray();

            characterModel.autoPopulateLightInfos = true;
            characterModel.invisibilityCount = 0;
            characterModel.temporaryOverlays = new List<TemporaryOverlay>();

            ModelSkinController skinController = model.AddComponent<ModelSkinController>();
            LoadoutAPI.SkinDefInfo skinDefInfo = new LoadoutAPI.SkinDefInfo
            {
                BaseSkins = Array.Empty<SkinDef>(),
                GameObjectActivations = new SkinDef.GameObjectActivation[0],
                Icon = Core.Assets.defaultSkin,
                MeshReplacements = new SkinDef.MeshReplacement[0],
                MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0],
                Name = "INVOKER_SKIN_DEFAULT_NAME",
                NameToken = "INVOKER_SKIN_DEFAULT_NAME",
                ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0],
                RendererInfos = rendererInfos.ToArray(),
                RootObject = model,
                UnlockableName = "",
            };
            SkinDef skin = LoadoutAPI.CreateNewSkinDef(skinDefInfo);
            skinController.skins = new SkinDef[] { skin };

            TeamComponent teamComponent = null;
            if (invokerBody.GetComponent<TeamComponent>() != null) teamComponent = invokerBody.GetComponent<TeamComponent>();
            else teamComponent = invokerBody.GetComponent<TeamComponent>();
            teamComponent.hideAllyCardDisplay = false;
            teamComponent.teamIndex = TeamIndex.None;

            HealthComponent healthComponent = invokerBody.GetComponent<HealthComponent>();
            healthComponent.health = 90f;
            healthComponent.shield = 0f;
            healthComponent.barrier = 0f;
            healthComponent.magnetiCharge = 0f;
            healthComponent.body = null;
            healthComponent.dontShowHealthbar = false;
            healthComponent.globalDeathEventChanceCoefficient = 1f;

            invokerBody.GetComponent<Interactor>().maxInteractionDistance = 3f;
            invokerBody.GetComponent<InteractionDriver>().highlightInteractor = true;

            CharacterDeathBehavior characterDeathBehavior = invokerBody.GetComponent<CharacterDeathBehavior>();
            characterDeathBehavior.deathStateMachine = invokerBody.GetComponent<EntityStateMachine>();
            characterDeathBehavior.deathState = new SerializableEntityStateType(typeof(GenericCharacterDeath));

            SfxLocator sfxLocator = invokerBody.GetComponent<SfxLocator>();
            sfxLocator.deathSound = "Play_ui_player_death";
            sfxLocator.barkSound = "";
            sfxLocator.openSound = "";
            sfxLocator.landingSound = "Play_char_land";
            sfxLocator.fallDamageSound = "Play_char_land_fall_damage";
            sfxLocator.aliveLoopStart = "";
            sfxLocator.aliveLoopStop = "";

            Rigidbody rigidbody = invokerBody.GetComponent<Rigidbody>();
            rigidbody.mass = 100f;
            rigidbody.drag = 0f;
            rigidbody.angularDrag = 0f;
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
            rigidbody.interpolation = RigidbodyInterpolation.None;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            rigidbody.constraints = RigidbodyConstraints.None;

            CapsuleCollider capsuleCollider = invokerBody.GetComponent<CapsuleCollider>();
            capsuleCollider.isTrigger = false;
            capsuleCollider.material = null;
            capsuleCollider.center = new Vector3(0f, 0f, 0f);
            capsuleCollider.radius = 0.5f;
            capsuleCollider.height = 1.82f;
            capsuleCollider.direction = 1;

            KinematicCharacterMotor kinematicCharacterMotor = invokerBody.GetComponent<KinematicCharacterMotor>();
            kinematicCharacterMotor.CharacterController = characterMotor;
            kinematicCharacterMotor.Capsule = capsuleCollider;
            kinematicCharacterMotor.Rigidbody = rigidbody;

            capsuleCollider.radius = 0.5f;
            capsuleCollider.height = 1.82f;
            capsuleCollider.center = new Vector3(0, 0, 0);
            capsuleCollider.material = null;

            kinematicCharacterMotor.DetectDiscreteCollisions = false;
            kinematicCharacterMotor.GroundDetectionExtraDistance = 0f;
            kinematicCharacterMotor.MaxStepHeight = 0.2f;
            kinematicCharacterMotor.MinRequiredStepDepth = 0.1f;
            kinematicCharacterMotor.MaxStableSlopeAngle = 55f;
            kinematicCharacterMotor.MaxStableDistanceFromLedge = 0.5f;
            kinematicCharacterMotor.PreventSnappingOnLedges = false;
            kinematicCharacterMotor.MaxStableDenivelationAngle = 55f;
            kinematicCharacterMotor.RigidbodyInteractionType = RigidbodyInteractionType.None;
            kinematicCharacterMotor.PreserveAttachedRigidbodyMomentum = true;
            kinematicCharacterMotor.HasPlanarConstraint = false;
            kinematicCharacterMotor.PlanarConstraintAxis = Vector3.up;
            kinematicCharacterMotor.StepHandling = StepHandlingMethod.None;
            kinematicCharacterMotor.LedgeHandling = true;
            kinematicCharacterMotor.InteractiveRigidbodyHandling = true;
            kinematicCharacterMotor.SafeMovement = false;

            HurtBoxGroup hurtBoxGroup = model.AddComponent<HurtBoxGroup>();

            HurtBox componentInChildren = model.GetComponentInChildren<CapsuleCollider>().gameObject.AddComponent<HurtBox>();
            componentInChildren.gameObject.layer = LayerIndex.entityPrecise.intVal;
            componentInChildren.healthComponent = healthComponent;
            componentInChildren.isBullseye = true;
            componentInChildren.damageModifier = HurtBox.DamageModifier.Normal;
            componentInChildren.hurtBoxGroup = hurtBoxGroup;
            componentInChildren.indexInGroup = 0;

            hurtBoxGroup.hurtBoxes = new HurtBox[]
            {
                componentInChildren
            };

            hurtBoxGroup.mainHurtBox = componentInChildren;
            hurtBoxGroup.bullseyeCount = 1;

            FootstepHandler footstepHandler = model.AddComponent<FootstepHandler>();
            footstepHandler.baseFootstepString = "Play_player_footstep";
            footstepHandler.sprintFootstepOverrideString = "";
            footstepHandler.enableFootstepDust = true;
            footstepHandler.footstepDustPrefab = Resources.Load<GameObject>("Prefabs/GenericFootstepDust");

            RagdollController ragdollController = model.AddComponent<RagdollController>();
            ragdollController.bones = null;
            ragdollController.componentsToDisableOnRagdoll = null;

            AimAnimator aimAnimator = model.AddComponent<AimAnimator>();
            aimAnimator.inputBank = inputBankTest;
            aimAnimator.directionComponent = characterDirection;
            aimAnimator.pitchRangeMax = 55f;
            aimAnimator.pitchRangeMin = -50f;
            aimAnimator.yawRangeMin = -44f;
            aimAnimator.yawRangeMax = 44f;
            aimAnimator.pitchGiveupRange = 30f;
            aimAnimator.yawGiveupRange = 10f;
            aimAnimator.giveupDuration = 8f;

            invokerBody.AddComponent<Miscellaneous.InvokerTracker>();
        }

        private void InitializeCharacter()
        {
            invokerDisplayBody = PrefabAPI.InstantiateClone(invokerBody.GetComponent<ModelLocator>().modelBaseTransform.gameObject, "InvokerDisplay", true);
            invokerDisplayBody.AddComponent<NetworkIdentity>();

            SurvivorDef survivorDef = new SurvivorDef
            {
                name = "INVOKER_NAME",
                unlockableName = "",
                descriptionToken = "INVOKER_DESCRIPTION",
                outroFlavorToken = "INVOKER_OUTRO_FLAVOR",
                primaryColor = Color.magenta,
                bodyPrefab = invokerBody,
                displayPrefab = invokerDisplayBody
            };

            SurvivorAPI.AddSurvivor(survivorDef);

            BodyCatalog.getAdditionalEntries += delegate (List<GameObject> list)
            {
                list.Add(invokerBody);
            };
        }

        private void InitializeSkills()
        {
            LanguageAPI.Add("INVOKER_PASSIVE_NAME", "");
            LanguageAPI.Add("INVOKER_PASSIVE_DESCRIPTION", "");
            LanguageAPI.Add("INVOKER_PRIMARY_NAME", "Elemental Bolt");
            LanguageAPI.Add("INVOKER_PRIMARY_DESCRIPTION", "Fires a homing magic projectile that deals <style=cIsDamage>" + Core.Config.primaryDamageCoefficient.Value * 100 + "% damage</style>.");
            LanguageAPI.Add("INVOKER_SECONDARY_NAME", "");
            LanguageAPI.Add("INVOKER_SECONDARY_DESCRIPTION", "");
            LanguageAPI.Add("INVOKER_UTILITY_NAME", "");
            LanguageAPI.Add("INVOKER_UTILITY_DESCRIPTION", "");
            LanguageAPI.Add("INVOKER_SPECIAL_NAME", "Invoke");
            LanguageAPI.Add("INVOKER_SPECIAL_DESCRIPTION", "Combines your current elemental orbs to <style=cIsUtility>create a new spell</style>.");

            #region SkillDefs

            //Primary
            SkillDef elementalBoltSkillDef = ScriptableObject.CreateInstance<SkillDef>();
            elementalBoltSkillDef.activationState = new SerializableEntityStateType(typeof(States.ElementalBolt));
            elementalBoltSkillDef.activationStateMachineName = "Weapon";
            elementalBoltSkillDef.baseMaxStock = 1;
            elementalBoltSkillDef.baseRechargeInterval = 0f;
            elementalBoltSkillDef.beginSkillCooldownOnSkillEnd = false;
            elementalBoltSkillDef.canceledFromSprinting = false;
            elementalBoltSkillDef.fullRestockOnAssign = true;
            elementalBoltSkillDef.interruptPriority = InterruptPriority.Any;
            elementalBoltSkillDef.isBullets = false;
            elementalBoltSkillDef.isCombatSkill = true;
            elementalBoltSkillDef.mustKeyPress = false;
            elementalBoltSkillDef.noSprint = true;
            elementalBoltSkillDef.rechargeStock = 1;
            elementalBoltSkillDef.requiredStock = 1;
            elementalBoltSkillDef.shootDelay = 0f;
            elementalBoltSkillDef.stockToConsume = 1;
            elementalBoltSkillDef.icon = null;
            elementalBoltSkillDef.skillDescriptionToken = "INVOKER_PRIMARY_DESCRIPTION";
            elementalBoltSkillDef.skillName = "INVOKER_PRIMARY_NAME";
            elementalBoltSkillDef.skillNameToken = "INVOKER_PRIMARY_NAME";

            //Special
            SkillDef invokeSkillDef = ScriptableObject.CreateInstance<SkillDef>();
            invokeSkillDef.activationState = new SerializableEntityStateType(typeof(States.ElementalBolt));
            invokeSkillDef.activationStateMachineName = "Weapon";
            invokeSkillDef.baseMaxStock = 1;
            invokeSkillDef.baseRechargeInterval = 3f;
            invokeSkillDef.beginSkillCooldownOnSkillEnd = false;
            invokeSkillDef.canceledFromSprinting = false;
            invokeSkillDef.fullRestockOnAssign = true;
            invokeSkillDef.interruptPriority = InterruptPriority.Any;
            invokeSkillDef.isBullets = false;
            invokeSkillDef.isCombatSkill = true;
            invokeSkillDef.mustKeyPress = false;
            invokeSkillDef.noSprint = true;
            invokeSkillDef.rechargeStock = 1;
            invokeSkillDef.requiredStock = 1;
            invokeSkillDef.shootDelay = 0f;
            invokeSkillDef.stockToConsume = 1;
            invokeSkillDef.icon = null;
            invokeSkillDef.skillDescriptionToken = "INVOKER_SPECIAL_DESCRIPTION";
            invokeSkillDef.skillName = "INVOKER_SPECIAL_NAME";
            invokeSkillDef.skillNameToken = "INVOKER_SPECIAL_NAME";

            LoadoutAPI.AddSkillDef(elementalBoltSkillDef);
            LoadoutAPI.AddSkillDef(invokeSkillDef);

            #endregion

            #region Creating new skill families and GenericSkill components.

            foreach (GenericSkill i in invokerBody.GetComponentsInChildren<GenericSkill>())
            {
                DestroyImmediate(i);
            }

            SkillLocator skillLocator = invokerBody.GetComponent<SkillLocator>();

            //Primary
            skillLocator.primary = invokerBody.AddComponent<GenericSkill>();
            SkillFamily newFamilyPrimary = ScriptableObject.CreateInstance<SkillFamily>();
            newFamilyPrimary.variants = new SkillFamily.Variant[1];
            LoadoutAPI.AddSkillFamily(newFamilyPrimary);
            skillLocator.primary.SetFieldValue("_skillFamily", newFamilyPrimary);
            SkillFamily skillFamilyPrimary = skillLocator.primary.skillFamily;
            skillFamilyPrimary.variants[0] = new SkillFamily.Variant
            {
                skillDef = elementalBoltSkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(elementalBoltSkillDef.skillNameToken, false, null)
            };

            //Special
            skillLocator.special = invokerBody.AddComponent<GenericSkill>();
            SkillFamily newFamilySpecial = ScriptableObject.CreateInstance<SkillFamily>();
            newFamilySpecial.variants = new SkillFamily.Variant[1];
            LoadoutAPI.AddSkillFamily(newFamilySpecial);
            skillLocator.special.SetFieldValue("_skillFamily", newFamilySpecial);
            SkillFamily skillFamilySpecial = skillLocator.special.skillFamily;
            skillFamilySpecial.variants[0] = new SkillFamily.Variant
            {
                skillDef = invokeSkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(invokeSkillDef.skillNameToken, false, null)
            };
            #endregion
        }

        private void InitializeOrbs()
        {
            elementalBoltOrb = Core.Assets.elementalBoltOrbEffectPrefab;
            foreach (Transform transform in elementalBoltOrb.GetComponentsInChildren<Transform>()) { transform.localScale = Vector3.one * 6f; }
            elementalBoltOrb.AddComponent<NetworkIdentity>();
            elementalBoltOrb.AddComponent<VFXAttributes>().vfxPriority = VFXAttributes.VFXPriority.Always;
            elementalBoltOrb.AddComponent<EffectComponent>();
            var orbEffect1 = elementalBoltOrb.AddComponent<OrbEffect>();
            orbEffect1.startVelocity1 = new Vector3(0, 0, 0);
            orbEffect1.startVelocity2 = new Vector3(0, 0, 0);
            orbEffect1.endVelocity1 = new Vector3(1, 1, 1);
            orbEffect1.endVelocity2 = new Vector3(1, 1, 1);
            orbEffect1.movementCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

            if (elementalBoltOrb) PrefabAPI.RegisterNetworkPrefab(elementalBoltOrb);
            EffectAPI.AddEffect(elementalBoltOrb);
            OrbAPI.AddOrb(typeof(Miscellaneous.ElementalBoltOrb));
        }

        private void InitializeProjectiles()
        {
        }

        private void CreateDoppelGanger()
        {
            invokerDoppelgangerMaster = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterMasters/CommandoMonsterMaster"), "InvokerDoppelgangerMaster", true);

            MasterCatalog.getAdditionalEntries += delegate (List<GameObject> list)
            {
                list.Add(invokerDoppelgangerMaster);
            };

            CharacterMaster component = invokerDoppelgangerMaster.GetComponent<CharacterMaster>();
            component.bodyPrefab = invokerBody;
        }

        private static GameObject CreateModel(GameObject main)
        {
            Destroy(main.transform.Find("ModelBase").gameObject);
            Destroy(main.transform.Find("CameraPivot").gameObject);
            Destroy(main.transform.Find("AimOrigin").gameObject);

            GameObject model = Core.Assets.MainAssetBundle.LoadAsset<GameObject>("mdlInvoker");

            return model;
        }
    }
}
