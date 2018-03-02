namespace Assets.Scripts.Craiel.VFX.Editor
{
    using System.Collections.Generic;
    using Contracts;
    using Data;
    using GameData.Editor.Builder;
    using GameData.Editor.Common;
    using UnityEngine;

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

            foreach (IGameDataVFXNode node in this.Nodes)
            {
                node.Build(this, context);
            }

            context.AddBuildResult(runtime);
        }
    }
}