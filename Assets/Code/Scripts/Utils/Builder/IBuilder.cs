/// <summary>
/// Builds an object of type T.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IBuilder<T>
{
    T Build();
}
