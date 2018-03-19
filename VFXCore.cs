using IVFXConfig = Craiel.UnityVFX.Contracts.IVFXConfig;

namespace Craiel.UnityVFX
{
    using UnityEssentials.Component;

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