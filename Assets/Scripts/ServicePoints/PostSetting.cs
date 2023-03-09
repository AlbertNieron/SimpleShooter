using UnityEngine;

public enum PointType
{
	InspectionPoint,
	WorkPoint,
	RoutePoint
}

public class PostSetting : MonoBehaviour
{
	[SerializeField] private PointType _typeOfPoint;
	[SerializeField] private float _howLongToWork;

	public string Name { get => gameObject.name; }
	public Vector3 Position { get => transform.position; }
	public PointType TypeOfPoint { get => _typeOfPoint; }
	public float HowLongToWork { get => _howLongToWork; }
}
public struct ServicePost
{
	#region Debug
	private readonly string _name;
	#endregion

	public Vector3 Position { get; private set; }
	public PointType TypeofPoint { get; private set; }
	public float HowLongToWork { get; private set; }
	public bool NeedToBeServicing { get; set; }
	public string Name => _name;

	public ServicePost(string name, Vector3 position, PointType typeofPoint, float howLongToWork)
	{
		_name = name;
		Position = position;
		HowLongToWork = howLongToWork;
		TypeofPoint = typeofPoint;
		NeedToBeServicing = true;
	}


}
