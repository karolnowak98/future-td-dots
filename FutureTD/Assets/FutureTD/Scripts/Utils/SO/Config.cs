using UnityEngine;

namespace GlassyCode.FutureTD.Utils.SO
{
    /// <summary>
    /// Config store data about an instance of real time game object.
    /// </summary>
    public abstract class Config : ScriptableObject, IConfig
    {
        [field: SerializeField]
        public GameObject Prefab { get; private set; }
        
        [field: SerializeField]
        public Sprite IconSprite { get; private set; }
        
        public int Id => GetInstanceID();
    }
}