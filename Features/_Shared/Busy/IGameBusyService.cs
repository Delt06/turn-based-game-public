namespace Features._Shared.Busy
{
    public interface IGameBusyService
    {
        bool IsBusy { get; }
        void MakeBusy();
        void MakeFree();
    }
}