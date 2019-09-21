using UnityEngine;

namespace PriestsAndDevils
{
    public class Director : System.Object
    {
        // Singlton instance.
        private static Director instance;
        public ISceneController currentSceneController { get; set; }
        public bool running { get; set; }
        public int fps
        {
            get
            {
                return Application.targetFrameRate;
            }
            set
            {
                Application.targetFrameRate = value;
            }
        }

        public static Director GetInstance()
        {
            return instance ?? (instance = new Director());
        }
    }
}