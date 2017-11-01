namespace Assets.Scripts.VFX
{
    using System.Collections.Generic;
    using Data;
    using Data.Runtime;
    using Enums;
    using Logic;
    using NLog;
    using World;

    public class VFXInstance
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IList<VFXInstancePart> parts;

        private readonly RuntimeVfxData data;

        private EntityId abilityInstanceId;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public VFXInstance(VFXTicket ticket, RuntimeVfxData data)
        {
            this.parts = new List<VFXInstancePart>();

            this.Ticket = ticket;
            this.data = data;

            this.State = VFXState.Inactive;
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public VFXState State { get; private set; }

        public VFXTicket Ticket { get; private set; }

        public void Initialize(WorldTargetInfo source, IList<WorldTargetInfo> targets)
        {
            // Only initialize then normal sheets here
            foreach (RuntimeVfxSheetData sheet in this.data.CommonSheets)
            {
                var part = new VFXInstancePart(this, source, targets, sheet);
                this.parts.Add(part);
            }

            this.State = VFXState.Running;
        }

        public void Initialize(EntityId instanceId)
        {
            if (instanceId == EntityId.Invalid)
            {
                Logger.Warn("Initialize for VFXInstance called with invalid ability id!");
                return;
            }

            this.abilityInstanceId = instanceId;

            AbilityInstance instance = GameLogicCore.Instance.Get<AbilityInstance>(instanceId);

            IList<WorldTargetInfo> abilityTargetInfos = new List<WorldTargetInfo>();
            foreach (EntityId target in instance.Targets)
            {
                abilityTargetInfos.Add(new WorldTargetInfo(target));
            }

            // Include the normal sheets
            this.Initialize(new WorldTargetInfo(instance.Source), abilityTargetInfos);
            
            // Initialize the ability sheets
            foreach (RuntimeVfxSheetData sheet in this.data.AbilitySheets)
            {
                var part = new VFXInstancePart(this, new WorldTargetInfo(instance.Source), abilityTargetInfos, sheet, true);
                this.parts.Add(part);
            }
        }

        public void Update()
        {
            switch (this.State)
            {
                case VFXState.Running:
                {
                    this.UpdateRunning();
                    break;
                }
                
                case VFXState.Deactivating:
                {
                    this.UpdateDeactivating();
                    break;
                }
            }
        }

        public void End(bool immediate = false)
        {
            this.State = VFXState.Deactivating;

            foreach (VFXInstancePart part in this.parts)
            {
                part.End(immediate);
            }
        }

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        private void UpdateRunning()
        {
            AbilityInstance instanceData = null;
            if (this.abilityInstanceId != EntityId.Invalid)
            {
                instanceData = GameLogicCore.Instance.Get<AbilityInstance>(this.abilityInstanceId);
                if (instanceData == null || instanceData.State == AbilityInstanceState.Completed)
                {
                    this.End();
                    return;
                }
            }

            for (var i = 0; i < this.parts.Count; i++)
            {
                VFXInstancePart part = this.parts[i];
                if (part.IsAbilityPart)
                {
                    part.Update(instanceData);
                }
                else
                {
                    part.Update();
                }
            }
        }

        private void UpdateDeactivating()
        {
            // Wait for all parts to destroy
            foreach (VFXInstancePart part in this.parts)
            {
                switch (part.State)
                {
                    case VFXState.Destroyed:
                    {
                        break;
                    }

                    default:
                    {
                        return;
                    }
                }
            }

            this.State = VFXState.Destroyed;
        }
    }
}
