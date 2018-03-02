using EditorEventGameDataSelectionChanged = Craiel.GameData.Editor.Events.EditorEventGameDataSelectionChanged;

namespace Assets.Scripts.Craiel.VFX.Editor.Window
{
    using Craiel.Editor.GameData;
    using Essentials.Editor;
    using Essentials.Event;
    using Essentials.Event.Editor;
    using UnityEditor;
    using UnityEngine;

    public class VFXEditorWindow : EssentialEditorWindow<VFXEditorWindow>
    {
        private readonly VFXNodeEditor nodeEditor;

        private GameDataVFX activeVFX;

        private BaseEventSubscriptionTicket eventGameDataChangedTicket;

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

        public override void OnEnable()
        {
            base.OnEnable();

            this.eventGameDataChangedTicket = EditorEvents.Subscribe<EditorEventGameDataSelectionChanged>(this.OnGameDataSelectionChanged);
        }

        public override void OnDestroy()
        {
            this.eventGameDataChangedTicket.Dispose();
            this.eventGameDataChangedTicket = null;
            
            base.OnDestroy();
        }

        public override void OnSelectionChange()
        {
            base.OnSelectionChange();
            
            if (UnityEditor.Selection.objects == null
                || UnityEditor.Selection.objects.Length != 1)
            {
                this.activeVFX = null;
                return;
            }
            
            this.activeVFX = UnityEditor.Selection.objects[0] as GameDataVFX;
            this.Repaint();
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

                string selectionTitle = this.activeVFX == null ? "<None>" : this.activeVFX.Name;
                if (EditorGUILayout.DropdownButton(new GUIContent("Selected: " + selectionTitle), FocusType.Passive, "ToolbarDropDown"))
                {
                    var menu = new GenericMenu();
                    foreach (GameDataVFX vfx in GameDataVFXRef.GetAvailable())
                    {
                        GameDataVFX closure = vfx;
                        menu.AddItem(new GUIContent(vfx.Name), false, () => this.SelectActiveVFX(closure));
                    }
                    
                    menu.ShowAsContext();
                    Event.current.Use();
                }

                GUILayout.FlexibleSpace();
            }

            EditorGUILayout.EndHorizontal();

            if (this.activeVFX != null)
            {
                Rect contentRect = new Rect(10, 40, position.width - 20, position.height - 50);
                this.nodeEditor.Draw(contentRect, this.activeVFX);
            }

            ProcessEvents(Event.current);

            if (GUI.changed)
            {
                Repaint();
            }
        }

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        private void ProcessEvents(Event eventData)
        {
            this.nodeEditor.ProcessEvent(eventData);
        }
        
        private void OnGameDataSelectionChanged(EditorEventGameDataSelectionChanged eventData)
        {
            if (eventData.SelectedObjects == null
                || eventData.SelectedObjects.Length != 1)
            {
                this.activeVFX = null;
                return;
            }

            this.activeVFX = eventData.SelectedObjects[0] as GameDataVFX;
            this.Repaint();
        }
        
        private void SelectActiveVFX(GameDataVFX vfxData)
        {
            this.activeVFX = vfxData;
            UnityEditor.Selection.objects = new[] {vfxData};
            this.Repaint();
        }
    }
}