namespace Assets.Scripts.Craiel.VFX.Editor
{
    using System.Collections.Generic;
    using System.Linq;
    using Craiel.Editor.GameData;
    using GameData.Editor;
    public class GameDataVFXRef : GameDataRefBase
    {
        // -------------------------------------------------------------------
        // Protected
        // -------------------------------------------------------------------
        public static IList<GameDataVFX> GetAvailable()
        {
            return GameDataHelpers.FindGameDataList(typeof(GameDataVFX)).Cast<GameDataVFX>().ToList();
        }
    }
}