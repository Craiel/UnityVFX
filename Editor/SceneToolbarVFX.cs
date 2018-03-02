﻿namespace Assets.Scripts.Craiel.VFX.Editor
{
    using Essentials.Editor;
    using UnityEditor;
    using UnityEngine;
    using Window;

    public class SceneToolbarVFX : SceneToolbarWidget
    {
        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public override void OnGUi()
        {
            base.OnGUi();
            if (EditorGUILayout.DropdownButton(new GUIContent("VFX"), FocusType.Passive, "ToolbarDropDown"))
            {
                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("VFX Editor"), false, VFXEditorWindow.OpenWindow);
                menu.AddSeparator("");
                menu.ShowAsContext();
                Event.current.Use();
            }
        }
    }
}