namespace PriestsAndDevils
{
    public interface ISceneController
    {
        void LoadResources();
    }

    public interface IUserAction
    {
        void ClickBoat();
        void ClickCharacter(CharacterController c);
        void Reset();
    }
}