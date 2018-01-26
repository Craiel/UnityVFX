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
        // Public
        // -------------------------------------------------------------------
        public override void DrawGUI()
        {
            this.DrawProperties();
        }

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        private void DrawProperties()
        {
            if (this.DrawFoldout("Properties", ref propertiesFoldout))
            {
                // TODO
            }
        }
    }
}