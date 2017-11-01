namespace Assets.Scripts.Craiel.VFX
{
    using System.Collections.Generic;
    using CoreGame;
    using CoreGame.Events;
    using Craiel.Essentials;
    using Craiel.Essentials.Enums;
    using Craiel.Essentials.Event;
    using Craiel.Essentials.Resource;
    using Craiel.Essentials.Scene;
    using Craiel.GameData;
    using Data;
    using Data.Runtime;
    using Enums;
    using Logic;
    using NLog;
    using UnityEngine;

    public class VFXSystem : UnitySingletonBehavior<VFXSystem>
    {
        private static readonly NLog.Logger Logger = LogManager.GetCurrentClassLogger();

        private static uint nextTicketId = 1;

        private readonly IList<VFXInstance> instances;
        private readonly IDictionary<VFXTicket, VFXInstance> instanceTicketLookup;
        private readonly IDictionary<ResourceKey, VFXPool> vfxPools;
        private readonly IList<VFXTicket> ticketCache;

        private SceneObjectRoot controllerRoot;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public VFXSystem()
        {
            this.instances = new List<VFXInstance>();
            this.instanceTicketLookup = new Dictionary<VFXTicket, VFXInstance>();
            this.vfxPools = new Dictionary<ResourceKey, VFXPool>();
            this.ticketCache = new List<VFXTicket>();
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public override void Awake()
        {
            this.RegisterInController(SceneObjectController.Instance, SceneRootCategory.System, true);

            this.controllerRoot = SceneObjectController.Instance.AcquireRoot(SceneRootCategory.Dynamic, "VFX Controllers", true);

            base.Awake();

            GameEvents.Instance.Subscribe<EventGameDataLoaded>(this.OnGameDataReloaded);
            GameEvents.Instance.Subscribe<EventAbilityExecuted>(this.OnAbilityExecuted);
        }

        public void Update()
        {
            this.ticketCache.Clear();
            for (var i = 0; i < this.instances.Count; i++)
            {
                this.instances[i].Update();
                if (this.instances[i].State == VFXState.Destroyed)
                {
                    this.ticketCache.Add(this.instances[i].Ticket);
                }
            }

            for (var i = 0; i < this.ticketCache.Count; i++)
            {
                this.instances.Remove(this.instanceTicketLookup[this.ticketCache[i]]);
                this.ticketCache.Remove(this.ticketCache[i]);
            }

            foreach (ResourceKey key in this.vfxPools.Keys)
            {
                this.vfxPools[key].Update();
            }
        }

        public VFXInstance Create(GameDataId id)
        {
            var data = GameRuntimeData.Instance.Get<RuntimeVfxData>(id);
            if (data == null)
            {
                Logger.Error("Could not Create VFX {0}, data not found", id);
                return null;
            }

            var ticket = new VFXTicket(nextTicketId++);
            var instance = new VFXInstance(ticket, data);

            this.instances.Add(instance);
            this.instanceTicketLookup.Add(ticket, instance);

            return instance;
        }

        public VFXController AcquireController(ResourceKey key)
        {
            VFXPool pool;
            if (this.vfxPools.TryGetValue(key, out pool))
            {
                VFXController controller = pool.Obtain();
                if (controller != null)
                {
                    controller.Begin();
                    return controller;
                }
            }

            return null;
        }

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        private void OnGameDataReloaded(EventGameDataLoaded eventdata)
        {
            this.ClearInstances();
            this.ClearPools();

            IList<RuntimeVfxData> vfxData = new List<RuntimeVfxData>();
            if (GameRuntimeData.Instance.GetAll(vfxData))
            {
                foreach (RuntimeVfxData data in vfxData)
                {
                    foreach (RuntimeVfxSheetData sheet in data.CommonSheets)
                    {
                        InitializePool(sheet);
                    }

                    foreach (RuntimeVfxSheetData sheet in data.AbilitySheets)
                    {
                        InitializePool(sheet);
                    }
                }
            }
        }

        private void OnAbilityExecuted(EventAbilityExecuted eventdata)
        {
            var instance = GameLogicCore.Instance.Get<AbilityInstance>(eventdata.InstanceId);
            var abilityData = GameRuntimeData.Instance.Get<RuntimeAbilityData>(instance.DataId);
            if (abilityData.VFX != GameDataId.Invalid)
            {
                VFXInstance vfxInstance = this.Create(abilityData.VFX);
                vfxInstance.Initialize(eventdata.InstanceId);
            }
        }

        private void InitializePool(RuntimeVfxSheetData data)
        {
            if (!data.PrefabResourceKey.IsValid())
            {
                return;
            }

            if (this.vfxPools.ContainsKey(data.PrefabResourceKey))
            {
                return;
            }

            // TODO initialize new pool for this key
            using (var resource = ResourceProvider.Instance.AcquireOrLoadResource<GameObject>(data.PrefabResourceKey))
            {
                if (resource != null && resource.Data != null)
                {
                    var pool = new VFXPool();
                    pool.Initialize(resource.Data, this.OnVFXControllerUpdate, this.controllerRoot.GetTransform());
                    this.vfxPools.Add(data.PrefabResourceKey, pool);
                }
            }
        }

        private void ClearPools()
        {
            foreach (ResourceKey key in this.vfxPools.Keys)
            {
                this.vfxPools[key].Clear();
            }

            this.vfxPools.Clear();
        }

        private void ClearInstances()
        {
            foreach (VFXTicket ticket in this.instanceTicketLookup.Keys)
            {
                this.instanceTicketLookup[ticket].End(true);
            }

            this.instanceTicketLookup.Clear();
        }

        private bool OnVFXControllerUpdate(VFXController controller)
        {
            return controller.IsActive;
        }
    }
}
