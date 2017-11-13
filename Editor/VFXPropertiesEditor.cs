namespace Assets.Scripts.Craiel.VFX.Editor
{
    using Essentials.Editor.ReorderableList;
    using Rotorz.ReorderableList;
    using Scripts.Editor;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(VFXProperties), true)]
    public class VFXPropertiesEditor : BaseControllerPropertyEditor<VFXProperties>
    {
        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public VFXPropertiesEditor()
        {
            this.Title = "VFX Poperties";
            this.ObjectTitle = "VFX";

            this.RegisterTab("Visuals", this.DrawVisuals);
            this.RegisterTab("ParticleSystems", this.DrawParticleSystems);
        }
        
        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        private void DrawVisuals()
        {
            ReorderableListGUI.Title("Renderers");
            ReorderableListGUI.ListField(this.GetProp(x => x.Renderers));

            if (GUILayout.Button("Auto Find Renderers"))
            {
                this.AutoFindComponents<Renderer>(this.GetProp(x => x.Renderers));
            }
        }

        private void DrawParticleSystems()
        {
            ReorderableListGUI.Title("Partile Systems");
            ReorderableListGUI.ListField(this.GetProp(x => x.ParticleSystems));

            if (GUILayout.Button("Auto Find Particle Systems"))
            {
                this.AutoFindComponents<ParticleSystem>(this.GetProp(x => x.ParticleSystems));
            }
        }
    }
}
