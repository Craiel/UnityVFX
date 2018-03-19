using RuntimeGameData = Craiel.UnityGameData.RuntimeGameData;

namespace Craiel.UnityVFX.Data
{
    using System;
    using System.Collections.Generic;
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
