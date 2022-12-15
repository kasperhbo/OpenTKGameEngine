// using OpenTK.Mathematics;

using MarioGabeKasper.Engine.Utils;
using Vector2 = System.Numerics.Vector2;

namespace MarioGabeKasper.Engine.Core
{
    public class Settings
    {
        /// <summary>
        /// Window Settings
        /// </summary>
        public static int Width = 1920;
        public static int Height = 1080;
        public static string Title = "Open TK Test Game";

        /// <summary>
        /// GRID
        /// </summary>
        // public int GridWidth = 32;
        // public int GridHeight = 32;
        public Vector2Int GridSize = new Vector2Int(32,32);
        
        
        /// <summary>
        /// Asset window
        /// </summary>
        public float AssetZoomSize = 1;
        
        /// <summary>
        /// Scene Camera
        /// </summary>
        public float SceneCameraSpeedMultiplier = 1;

        /// <summary>
        /// Scene
        /// </summary>
        public string lastOpenedScenePath = "Level01.scene";

    }
}