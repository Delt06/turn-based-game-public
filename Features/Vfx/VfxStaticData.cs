using Features._Shared.Data;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Vfx
{
    [CreateAssetMenu(menuName = AssetPath + "VFX")]
    public class VfxStaticData : StaticDataBase
    {
        [SerializeField] [Required] private GameObject _deathVfx = default!;

        public GameObject DeathVfx => _deathVfx;
    }
}