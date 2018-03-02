﻿namespace Assets.Scripts.Craiel.VFX.Editor.Window
{
    using System;
    using Essentials.Editor.NodeEditor;
    using UnityEngine;

    public class VFXNodeEditor : ScriptableNodeEditor
    {
        private GameDataVFX activeVfx;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public VFXNodeEditor()
        {
            this.ContextMenu = new VFXNodeEditorContextMenu();
        }
        
        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public void Draw(Rect drawArea, GameDataVFX vfxData)
        {
            if (this.activeVfx != vfxData)
            {
                this.activeVfx = vfxData;
                this.Reload();
            }
            
            Draw(drawArea);
        }
        
        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        private void Reload()
        {
            this.Clear();
        }
    }
}