using UnityEngine;

/// <summary>
/// A behaviour which applies correction forces towards a target position.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class ForceTowardsTarget : MonoBehaviour
{
  private Rigidbody rb;

  [Header("Target")]

  [Tooltip("The target transform to move towards. If not specified, the target position is in world space.")]
  public Transform TargetTransform;

  [Tooltip("The target position in the target transform's local space (or world space if no target transform is specified).")]
  public Vector3 TargetPosition;

  [Header("Force")]

  public PIDCorrector xCorrector;
  public PIDCorrector yCorrector;
  public PIDCorrector zCorrector;

  [Header("Relative Force")]

  public PIDCorrector relativeXCorrector;
  public PIDCorrector relativeYCorrector;
  public PIDCorrector relativeZCorrector;

  protected virtual void Start()
  {
    rb = GetComponent<Rigidbody>();
  }

  protected virtual void FixedUpdate()
  {
    var target = TargetPosition;
    if (TargetTransform != null)
    {
      target = TargetTransform.TransformPoint(target);
    }

    var deltaTarget = target - transform.position;
    var x = xCorrector.Correct(deltaTarget.x);
    var y = yCorrector.Correct(deltaTarget.y);
    var z = zCorrector.Correct(deltaTarget.z);
    rb.AddForce(x, y, z);

    var relativeTarget = transform.InverseTransformPoint(target);
    var relativeX = relativeXCorrector.Correct(relativeTarget.x);
    var relativeY = relativeYCorrector.Correct(relativeTarget.y);
    var relativeZ = relativeZCorrector.Correct(relativeTarget.z);
    rb.AddRelativeForce(relativeX, relativeY, relativeZ);
  }
}
