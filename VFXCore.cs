namespace Assets.Scripts.Craiel.VFX
{
    using Contracts;
    using Essentials.Component;

    public class VFXCore
    {
        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        static VFXCore()
        {
            new CraielComponentConfigurator<IVFXConfig>().Configure();
        }
    }
}