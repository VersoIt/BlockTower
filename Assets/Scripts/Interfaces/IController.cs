namespace Assets.Scripts.Interfaces
{
    interface IController : IDisable, IEnable, IIsEnable
    {
        void Switch();
        void SetState(bool targetState);
    }
}
