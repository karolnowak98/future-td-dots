using UnityEngine;

namespace GlassyCode.FutureTD.Utils.SO
{
    /// <summary>
    /// Container represent SO that store configs.
    /// </summary>
    public abstract class Container : ScriptableObject, IContainer
    {
        public int Id => GetInstanceID();

    }
}