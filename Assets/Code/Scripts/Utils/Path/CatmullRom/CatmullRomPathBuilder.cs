using UnityEngine;

/// <summary>
/// A behaviour which builds Catmull-Rom splines from a collection of nodes which represent point trees.
/// </summary>
public class CatmullRomPathBuilder : PathBuilder
{
  [Header("Tuning")]

  [Tooltip("The tightness of the curve.")]
  [Range(0f, 1f)]
  public float Tension = 0.5f;

  [Tooltip("The style of the curve, where 0 is uniform, 0.5 is centripetal, and 1 is chordal.")]
  [Range(0f, 1f)]
  public float Alpha = 0.5f;

  [Tooltip("Whether the spline is a closed loop.")]
  public bool Closed = false;

  public override IPath Build()
  {
    return new CatmullRomPath(Points, Tension, Alpha, Closed);
  }
}
