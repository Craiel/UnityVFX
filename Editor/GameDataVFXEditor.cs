namespace Assets.Scripts.Craiel.VFX.Editor
{
    using GameData.Editor.Common;
    using UnityEditor;

    [CustomEditor(typeof(GameDataVFX))]
    [CanEditMultipleObjects]
    public class GameDataVFXEditor : GameDataObjectEditor
    {
        private static bool propertiesFoldout = true;

        // -------------------------------------------------------------------
        // Protected
        // -------------------------------------------------------------------
        protected override void DrawCompact()
        {
            base.DrawCompact();
        }
        
        protected override void DrawFull()
        {
            base.DrawFull();

            this.DrawProperties();
        }
        
        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        private void DrawProperties()
        {
            if (this.DrawFoldout("VFX Properties", ref propertiesFoldout))
            {
                // TODO
            }
        }
    }
}