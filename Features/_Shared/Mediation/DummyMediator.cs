using Cysharp.Threading.Tasks;
using DELTation.LeoEcsExtensions.Views;
using Features.Death.Mediation;
using NaughtyAttributes;
using UnityEngine;

namespace Features._Shared.Mediation
{
    public class DummyMediator : MonoBehaviour, IDeathMediator
    {
        [SerializeField] [Required] private EntityView _entityView = default!;

        public async void OnDied()
        {
            await UniTask.NextFrame();
            _entityView.Destroy();
        }
    }
}