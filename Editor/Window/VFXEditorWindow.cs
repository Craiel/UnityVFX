namespace Assets.Scripts.Craiel.VFX.Editor.Window
{
    using Essentials.Editor;
    using UnityEditor;
    using UnityEngine;

    public class VFXEditorWindow : EssentialEditorWindow<VFXEditorWindow>
    {
        private readonly VFXNodeEditor nodeEditor;

        private GameDataVFX activeVFX;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public VFXEditorWindow()
        {
            this.nodeEditor =  new VFXNodeEditor();
        }
        
        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public static void OpenWindow()
        {
            OpenWindow("VFX Editor");
        }

        public void OnGUI()
        {
            // Menu Tool Bar
            EditorGUILayout.BeginHorizontal("Toolbar");
            {
                if (EditorGUILayout.DropdownButton(new GUIContent("Edit"), FocusType.Passive, "ToolbarDropDown"))
                {
                    var menu = new GenericMenu();
                    //menu.AddItem(new GUIContent("Validate"), false, ValidateGameData);
                    //menu.AddItem(new GUIContent("Export"), false, ExportGameData);
                    menu.ShowAsContext();
                    Event.current.Use();
                }
                
                GUILayout.FlexibleSpace();
            }

            EditorGUILayout.EndHorizontal();

            if (this.activeVFX != null)
            {
                Rect contentRect = new Rect(10, 80, position.width - 20, position.height - 90);
                this.nodeEditor.Draw(contentRect, this.activeVFX);
            }

            ProcessEvents(Event.current);

            if (GUI.changed)
            {
                Repaint();
            }
        }

        public void SetActiveVFX(GameDataVFX vfxData)
        {
            this.activeVFX = vfxData;
        }

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        private void ProcessEvents(Event eventData)
        {
            this.nodeEditor.ProcessEvent(eventData);
        }
    }
}