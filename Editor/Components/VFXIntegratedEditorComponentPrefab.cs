using IVFXEditorComponent = Craiel.GameData.Editor.Contracts.VFXShared.IVFXEditorComponent;

namespace Assets.Scripts.Craiel.VFX.Editor.Components
{
    public class VFXIntegratedEditorComponentPrefab : IVFXEditorComponent
    {
        public string Category
        {
            get { return VFXEditorConstants.IntegratedComponentCategory; }
        }

        public virtual string Name
        {
            get { return "Prefab"; }
        }
    }
}