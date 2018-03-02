namespace Assets.Scripts.Craiel.VFX.Editor
{
    using Contracts;
    using Essentials.Component;

    public class VFXEditorCore
    {
        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public static bool IsInitialized { get; private set; }

        public static VFXEditorConfig Config { get; private set; }

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
    }
}