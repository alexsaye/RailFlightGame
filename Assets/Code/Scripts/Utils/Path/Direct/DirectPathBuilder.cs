
/// <summary>
/// A behaviour which builds a direct path from a collection of nodes which represent point trees.
/// </summary>
public class DirectPathBuilder : PathBuilder
{
  public override IPath Build()
  {
    return new DirectPath(Points);
  }
}
