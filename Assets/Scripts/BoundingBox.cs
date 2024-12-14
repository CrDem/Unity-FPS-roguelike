using UnityEngine;

public class BoundingBox : MonoBehaviour
{
    private BoxCollider boxCollider;

	void Start() {
		boxCollider = GetComponent<BoxCollider>();
		boxCollider.enabled = false;
	}

    public bool IsInside(Vector3 position)
    {
        Bounds bounds = boxCollider.bounds;
        return bounds.Contains(position);
    }
}
