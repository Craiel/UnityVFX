namespace Assets.Scripts.Craiel.VFX.Editor
{
    using System;
    using System.Collections.Generic;
    using Components;
    using Contracts;
    using Essentials.Component;
    using Essentials.Event.Editor;
    using Events;

    public class VFXEditorCore
    {
        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        static VFXEditorCore()
        {
            Components = new List<Type>
            {
                typeof(VFXIntegratedEditorComponentParticle),
                typeof(VFXIntegratedEditorComponentPrefab)
            };
        }
        
        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public static bool IsInitialized { get; private set; }

        public static VFXEditorConfig Config { get; private set; }
        
        public static IList<Type> Components { get; private set; }

        public static void Initialize()
        {
            if (IsInitialized)
            {
                return;
            }

            Config = new VFXEditorConfig();
            Config.Load(); 

            new CraielComponentConfigurator<IVFXEditorConfig>().Configure();
            
            IsInitialized = true;
        }

        public static void AddComponent<T>()
        {
            if (Components.Contains(typeof(T)))
            {
                throw new InvalidOperationException("VFX Component was already registered!");
            }
            
            Components.Add(typeof(T));
            EditorEvents.Send(new EditorEventVFXComponentsChanged());
        }
    }
}