namespace Craiel.UnityVFX.Editor
{
    using System.Collections.Generic;
    using System.Linq;
    using Assets.Scripts.Craiel.Editor.GameData;
    using UnityGameData.Editor;

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