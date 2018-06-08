namespace Craiel.UnityVFX.Editor
{
    using System.Collections.Generic;
    using Runtime.Data;
    using UnityEngine;
    using UnityGameData.Editor.Builder;
    using UnityGameData.Editor.Common;
    using UnityGameData.Editor.Contracts.VFXShared;

    public class GameDataVFX : GameDataObject
    {
        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        [SerializeField]
        public List<IGameDataVFXNode> Nodes;

        public override void Validate(GameDataBuildValidationContext context)
        {
            base.Validate(context);

            if (this.Nodes == null || this.Nodes.Count == 0)
            {
                context.Error(this, this, null, "VFX does not contain any data!");
                return;
            }
            
            foreach (IGameDataVFXNode node in this.Nodes)
            {
                node.Validate(this, context);
            }
        }

        public override void Build(GameDataBuildContext context)
        {
            var runtime = new RuntimeVFXData();

            this.BuildBase(context, runtime);

            if (this.Nodes != null)
            {
                foreach (IGameDataVFXNode node in this.Nodes)
                {
                    node.Build(this, context);
                }
            }

            context.AddBuildResult(runtime);
        }
    }
}