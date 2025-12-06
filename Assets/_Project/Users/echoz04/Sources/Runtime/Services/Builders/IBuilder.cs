using UnityEngine;

namespace Sources.Runtime.Services.Builders
{
    public interface IBuilder<T> where T : Component
    {
        T Build(T prefab, Vector3 spawnPosition, Transform parent = null);
    }
}