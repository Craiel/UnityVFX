using IVFXEditorComponentFactory = Craiel.GameData.Editor.Contracts.VFXShared.IVFXEditorComponentFactory;

namespace Assets.Scripts.Craiel.VFX.Editor.Components
{
    using System;

    public struct VFXEditorComponentDescriptor
    {
        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public VFXEditorComponentDescriptor(IVFXEditorComponentFactory factory)
            : this()
        {
            this.Factory = factory;
        }
        
        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public readonly IVFXEditorComponentFactory Factory;
        
        public Type Type;

        public string Name;

        public string Category;
    }
}