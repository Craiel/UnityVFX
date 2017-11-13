namespace Assets.Scripts.Craiel.VFX
{
    using Contracts;
    using Essentials.UnityComponent;

    public class VFXCore
    {
        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        static VFXCore()
        {
            new UnityComponentConfigurator<IVFXConfig>().Configure();
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
    }
}