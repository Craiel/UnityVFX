namespace Assets.Scripts.Craiel.VFX
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Actors;
    using Audio;
    using CoreGame;
    using Craiel.Essentials.Resource;
    using Craiel.GameData;
    using Craiel.GDX.AI.Sharp.Mathematics;
    using Data;
    using Data.Runtime;
    using Enums;
    using NLog;
    using UnityEngine;
    using World;

    public class VFXInstancePart
    {
        private static readonly NLog.Logger Logger = LogManager.GetCurrentClassLogger();

        private static readonly AudioPlayParameters DefaultAudioParameters = new AudioPlayParameters {UseRandomClip = true};

        private readonly VFXInstance parent;
        private readonly RuntimeVfxSheetData data;

        private readonly IList<VFXController> controllers;
        private readonly IList<WorldTargetInfo> targets;

        private readonly WorldTargetInfo source;

        private readonly IList<AudioTicket> activeAudioTickets;

        private float stateTime;

        private float currentAbilityStateTimeMax;
        private float currentAbilityStateTime;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public VFXInstancePart(VFXInstance parent, WorldTargetInfo source, IList<WorldTargetInfo> targets, RuntimeVfxSheetData data, bool isAbilityPart = false)
        {
            this.parent = parent;
            this.data = data;
            this.source = source;
            this.targets = targets;
            this.IsAbilityPart = isAbilityPart;
            this.controllers = new List<VFXController>();
            this.activeAudioTickets = new List<AudioTicket>();
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public VFXState State { get; private set; }

        public bool IsAbilityPart { get; private set; }
        
        public void Update()
        {
            if (this.IsAbilityPart)
            {
                Logger.Error("Update without Ability instance called on ability part!");
                return;
            }

            if (this.State == VFXState.Inactive)
            {
                this.State = VFXState.Activating;
                this.stateTime = this.data.SpawnDelayValue;
            }

            this.UpdateState();
        }

        public void Update(AbilityInstance abilityInstance)
        {
            if (!this.IsAbilityPart)
            {
                Logger.Error("Update with Ability instance called on non-ability part!");
                return;
            }

            this.UpdateAbilityState(abilityInstance);
            this.UpdateState();
        }

        public void End(bool immediate = false)
        {
            if (this.data.DestroyFadeOut)
            {
                this.State = VFXState.Deactivating;
                this.stateTime = this.data.DestroyFadeOutTime;

                foreach (VFXController controller in this.controllers)
                {
                    controller.BeginFade(this.data.DestroyFadeOutTime);
                }
            }
            else
            {
                this.Destroy();
            }
        }

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        private void Destroy()
        {
            foreach (VFXController controller in this.controllers)
            {
                controller.End();
            }

            foreach (AudioTicket ticket in this.activeAudioTickets)
            {
                AudioSystem.Instance.Stop(ticket);
            }

            this.controllers.Clear();
            this.activeAudioTickets.Clear();

            this.State = VFXState.Destroyed;
        }

        private void UpdateAbilityState(AbilityInstance abilityInstance)
        {
            this.UpdateAbilityStateTime(abilityInstance);

            switch (this.State)
            {
                case VFXState.Inactive:
                {
                    if (this.data.AbilityState == abilityInstance.State)
                    {
                        this.State = VFXState.Activating;
                        this.stateTime = this.data.SpawnDelayValue;
                    }

                    return;
                }

                default:
                {
                    if (this.data.AbilityState != abilityInstance.State
                        && this.data.DestroyCondition == VFXDestroyCondition.AbilityStateChanged)
                    {
                        // State changed, destroy vfx
                        this.End();
                    }

                    return;
                }
            }
        }

        private void UpdateAbilityStateTime(AbilityInstance abilityInstance)
        {
            var abilityData = GameRuntimeData.Instance.Get<RuntimeAbilityData>(abilityInstance.DataId);
            switch (abilityInstance.State)
            {
                case AbilityInstanceState.Activation:
                {
                    this.currentAbilityStateTimeMax = abilityData.ActivationTime;
                    break;
                }

                case AbilityInstanceState.Delay:
                {
                    this.currentAbilityStateTimeMax = abilityData.DelayTime;
                    break;
                }

                case AbilityInstanceState.Running:
                {
                    this.currentAbilityStateTimeMax = abilityData.Duration;
                    break;
                }
            }

            this.currentAbilityStateTime = abilityInstance.StateTime;
        }

        private void UpdateState()
        {
            switch (this.State)
            {
                case VFXState.Activating:
                {
                    this.UpdateActivation();
                    break;
                }

                case VFXState.Running:
                {
                    this.UpdateRunning();
                    break;
                }

                case VFXState.Deactivating:
                {
                    this.UpdateDeactivation();
                    break;
                }
            }
        }

        private void UpdateActivation()
        {
            this.stateTime -= Time.deltaTime;

            if (this.stateTime > 0)
            {
                return;
            }

            this.State = VFXState.Running;
            this.stateTime = this.data.BehaviorDuration;

            this.ExecuteController();
            this.ExecuteSFX();
            this.ExecuteAnimations();
            this.ExecuteVisuals();
        }

        private void UpdateDeactivation()
        {
            this.stateTime -= Time.deltaTime;

            if (this.stateTime > 0)
            {
                return;
            }

            this.Destroy();
        }

        private void UpdateRunning()
        {
            if (this.CheckDestroy())
            {
                this.End();
                return;
            }

            // TODO
        }

        private bool CheckDestroy()
        {
            this.stateTime -= Time.deltaTime;

            switch (this.data.DestroyCondition)
            {
                case VFXDestroyCondition.DurationEnd:
                {
                    if (this.stateTime > 0)
                    {
                        return false;
                    }

                    break;
                }

                default:
                {
                    return false;
                }
            }

            return true;
        }

        private void ExecuteController()
        {
            if (!this.data.PrefabResourceKey.IsValid())
            {
                return;
            }

            switch (this.data.SpawnLocation)
            {
                case VFXSpawnLocation.OnSource:
                {
                    VFXController controller = this.ExecuteController(this.source);
                        
                    if (controller != null)
                    {
                        this.SetupControllerBehavior(controller, this.source, this.targets.First());
                    }

                    break;
                }

                case VFXSpawnLocation.OnTarget:
                {
                    foreach (WorldTargetInfo target in this.targets)
                    {
                        VFXController controller = ExecuteController(target);
                        if (controller != null)
                        {
                            SetupControllerBehavior(controller, this.source, target);
                        }
                    }

                    break;
                }

                case VFXSpawnLocation.OnArenaCenterFriendly:
                {
                    Logger.Error("TODO");
                    break;
                }

                case VFXSpawnLocation.OnArenaCenterHostile:
                {
                    Logger.Error("TODO");
                    break;
                }
            }
        }

        private VFXController ExecuteController(WorldTargetInfo target)
        {
            VFXController controller = VFXSystem.Instance.AcquireController(this.data.PrefabResourceKey);
            if (controller == null)
            {
                Logger.Warn("Failed to acquire controller for VFX");
                return null;
            }

            controller.SetPosition(WorldEntityCache.Instance.GetTargetPosition(target, this.data.SpawnAnchor));

            this.controllers.Add(controller);
            return controller;
        }

        private void SetupControllerBehavior(VFXController controller, WorldTargetInfo source, WorldTargetInfo target)
        {
            switch (this.data.Behavior)
            {
                case VFXBehavior.LerpToSource:
                {
                    Vector3 position = WorldEntityCache.Instance.GetTargetPosition(source, this.data.BehaviorAnchor);
                    RotateAndLerpController(controller, position);
                    break;
                }
                
                case VFXBehavior.LerpToTarget:
                {
                    if (this.data.Behavior == VFXBehavior.LerpToTarget && this.targets.Count > 1)
                    {
                        Logger.Warn("VFX is set LerpToTarget but VFX has multiple targets!");
                    }

                    Vector3 position = WorldEntityCache.Instance.GetTargetPosition(this.targets.First(), this.data.BehaviorAnchor);
                    RotateAndLerpController(controller, position);
                    break;
                }
            }
        }

        private void RotateAndLerpController(VFXController controller, Vector3 target)
        {
            float lerpTime = this.GetLerpTime();
            Vector3 direction = (target - controller.transform.forward).normalized;
            controller.transform.forward = direction;

            controller.LerpTo(target, lerpTime);
        }

        private float GetLerpTime()
        {
            if (this.data.BehaviorOverrideLerpTime)
            {
                return this.data.BehaviorLerpTime;
            }

            if (!this.IsAbilityPart)
            {
                Logger.Error("GetLerpPosition called on non-ability part, currently not supported!");
                return 0;
            }

            return this.currentAbilityStateTimeMax;
        }

        private void ExecuteSFX()
        {
            foreach (GameDataId id in this.data.Sfx)
            {
                AudioTicket ticket = AudioSystem.Instance.Play(id, DefaultAudioParameters);
                if (ticket == AudioTicket.Invalid)
                {
                    continue;
                }

                this.activeAudioTickets.Add(ticket);
            }
        }

        private void ExecuteAnimations()
        {
            if (this.data.AnimationSourceClipResourceKey.IsValid())
            {
                this.ApplyAnimationToTarget(this.source, this.data.AnimationSourceClipResourceKey, this.data.AnimationSourceScale, this.data.AnimationSourceScaleTime);
            }

            if(this.data.AnimationTargetClipResourceKey.IsValid())
            {
                foreach (WorldTargetInfo target in this.targets)
                {
                    this.ApplyAnimationToTarget(target, this.data.AnimationTargetClipResourceKey, this.data.AnimationTargetScale, this.data.AnimationTargetScaleTime);
                }
            }
        }

        private void ApplyAnimationToTarget(WorldTargetInfo target, ResourceKey resourceKey, bool scaleAnimation, float customScaleTime)
        {
            if (target.Entity == EntityId.Invalid)
            {
                return;
            }

            using (var resource = ResourceProvider.Instance.AcquireOrLoadResource<AnimationClip>(resourceKey))
            {
                if (resource == null || resource.Data == null)
                {
                    Logger.Warn("Failed to load animation clip: {0}", resourceKey);
                    return;
                }

                ActorAnimationMixer mixer = WorldEntityCache.Instance.GetAnimationMixer(target.Entity);
                if (mixer == null)
                {
                    Logger.Warn("Failed to get Target Animation Mixer for {0}", target);
                    return;
                }

                if (scaleAnimation)
                {
                    if (!this.IsAbilityPart)
                    {
                        Logger.Error("Scale animation not supported for non-ability VFX");
                        return;
                    }

                    float scaleTime = this.currentAbilityStateTimeMax;
                    if (Math.Abs(customScaleTime) > MathUtils.Epsilon)
                    {
                        scaleTime = customScaleTime;
                    }

                    mixer.QueueScaled(resource.Data, scaleTime);
                }
                else
                {
                    mixer.Queue(resource.Data);
                }
            }
        }

        private void ExecuteVisuals()
        {
            // TODO
        }
    }
}
