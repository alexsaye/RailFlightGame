using UnityEngine;

/// <summary>
/// A behaviour which builds an object of type T
/// </summary>
public abstract class BuilderBehaviour<T> : MonoBehaviour, IBuilder<T>
{
    public abstract T Build();
}
