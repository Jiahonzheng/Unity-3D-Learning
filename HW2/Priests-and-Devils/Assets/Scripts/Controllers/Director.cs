namespace PriestsAndDevils
{
    public class Director : System.Object
    {
        private static Director instance;
        public ISceneController CurrentSceneController { get; set; }

        public static Director GetInstance()
        {
            return instance ?? (instance = new Director());
        }
    }
}