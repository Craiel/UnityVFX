namespace Assets.Scripts.Craiel.VFX.Editor
{
    using Data;
    using GameData.Editor.Builder;
    using GameData.Editor.Common;

    public class GameDataVFX : GameDataObject
    {
        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        
        // TODO

        public override void Validate(GameDataBuildValidationContext context)
        {
            base.Validate(context);
        }

        public override void Build(GameDataBuildContext context)
        {
            var runtime = new RuntimeVFXData
            {
            };

            this.BuildBase(context, runtime);

            context.AddBuildResult(runtime);
        }
    }
}