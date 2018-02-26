namespace Assets.Scripts.Craiel.VFX.Editor.Window
{
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
            // Might want to disable this for this case:
            //this.GridEnableMeasureSections = false;

            this.LayouterEnabled = true;
            this.Layouter = new ScriptableNodeGridLayouter
            {
                ColumnMargin = 8,
                RowMargin = 8
            };
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