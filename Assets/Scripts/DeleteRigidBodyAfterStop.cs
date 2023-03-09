using UnityEngine;

public class DeleteRigidBodyAfterStop : MonoBehaviour
{
	private Rigidbody _itemRB;
	private void Start()
	{
		_itemRB = gameObject.GetComponent<Rigidbody>();
	}
	private void FixedUpdate()
	{
		if (_itemRB.IsSleeping())
			Destroy(_itemRB);
	}
}
