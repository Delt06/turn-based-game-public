using JetBrains.Annotations;

namespace Features._Shared.TimeManagement
{
    [UsedImplicitly]
    public class UnityTime : ITime
    {
        public float Time => UnityEngine.Time.time;

        public float DeltaTime => UnityEngine.Time.deltaTime;
    }
}