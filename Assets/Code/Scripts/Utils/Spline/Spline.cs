using UnityEngine;

/// <summary>
/// A sampleable spline.
/// </summary>
public abstract class Spline : MonoBehaviour
{
  public abstract Vector3 Sample(float progress);

  protected virtual void OnDrawGizmos()
  {
    var previous = this.Sample(0f);
    for (var i = 1; i <= 100; i++)
    {
      var t = i / 100f;
      var current = this.Sample(t);
      Gizmos.DrawLine(previous, current);
      previous = current;
    }
  }
}