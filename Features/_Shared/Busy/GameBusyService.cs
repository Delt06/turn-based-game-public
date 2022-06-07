using JetBrains.Annotations;

namespace Features._Shared.Busy
{
    [UsedImplicitly]
    public class GameBusyService : IGameBusyService
    {
        public bool IsBusy { get; private set; }

        public void MakeBusy() => IsBusy = true;
        public void MakeFree() => IsBusy = false;
    }
}