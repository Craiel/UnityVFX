namespace Assets.Scripts.Craiel.VFX.Editor
{
    using GameData.Editor;
    using GameData.Editor.Common;
    using GameData.Editor.Enums;
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
            switch (GameDataEditorCore.Config.GetViewMode())
            {
                case GameDataEditorViewMode.Compact:
                {
                    this.DrawCompact();
                    break;
                }
                    
                case GameDataEditorViewMode.Full:
                {
                    this.DrawFull();
                    break;
                }
            }
        }

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        private void DrawCompact()
        {
            
        }
        
        private void DrawFull()
        {
            this.DrawProperties();
        }
        
        private void DrawProperties()
        {
            if (this.DrawFoldout("Properties", ref propertiesFoldout))
            {
                // TODO
            }
        }
    }
}