using UnityEngine;
using System.Collections;

public class FollowMouse : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {

		Vector3 mouseLocation = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
		
		// Generate a plane that intersects the transform's position with an upwards normal.
		Plane playerPlane = new Plane(Vector3.up, transform.position);
		
		// Generate a ray from the cursor position
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		
		// Determine the point where the cursor ray intersects the plane.
		// This will be the point that the object must look towards to be looking at the mouse.
		// Raycasting to a Plane object only gives us a distance, so we'll have to take the distance,
		//   then find the point along that ray that meets that distance.  This will be the point
		//   to look at.
		float hitdist = 0.0f;
		// If the ray is parallel to the plane, Raycast will return false.
		if (playerPlane.Raycast (ray, out hitdist)) {
			// Get the point along the ray that hits the calculated distance.
			Vector3 targetPoint = ray.GetPoint(hitdist);
			
			// Determine the target rotation.  This is the rotation if the transform looks at the target point.
			Vector3 relativePos = targetPoint - transform.position;
			Quaternion targetRotation = Quaternion.LookRotation(relativePos);
			
			// rotate towards the target point.
			if ( targetPoint.z > 0.5f)
			transform.LookAt(targetPoint);

		}
	}

}
