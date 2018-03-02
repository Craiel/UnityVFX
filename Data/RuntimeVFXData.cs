namespace Assets.Scripts.Craiel.VFX.Data
{
    using System;
    using System.Collections.Generic;
    using Craiel.Essentials.Resource;
    using Craiel.GameData;
    using UnityEngine;

    [Serializable]
    public class RuntimeVFXData : RuntimeGameData
    {
        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public RuntimeVFXData()
        {
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        [SerializeField]
        public List<RuntimeVFXNodeData> Nodes;
        // TODO

        public override void PostLoad()
        {
            base.PostLoad();
        }
    }
}
