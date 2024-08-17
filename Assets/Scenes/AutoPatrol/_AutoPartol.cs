using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _AutoPartol : MonoBehaviour
{
	public float moveSpeed = 5f;
	public float rotationSpeed = 2f;
	public float waypointThreshold = 0.1f;

	public List<Transform> waypoints;
	private int currentWaypointIndex = 0;

	private void Update()
	{
		if (waypoints.Count == 0)
			return;

		// Get the current waypoint
		Transform currentWaypoint = waypoints[currentWaypointIndex];

		// Move towards the waypoint
		transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime);

		// Rotate towards the waypoint
		Vector3 direction = (currentWaypoint.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

		// Check if we've reached the waypoint
		if (Vector3.Distance(transform.position, currentWaypoint.position) < waypointThreshold)
		{
			// Move to the next waypoint
			currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
		}
	}

	// Optional: Draw lines between waypoints in the Scene view
	private void OnDrawGizmos()
	{
		if (waypoints.Count == 0)
			return;

		for (int i = 0; i < waypoints.Count; i++)
		{
			int nextIndex = (i + 1) % waypoints.Count;
			Gizmos.DrawLine(waypoints[i].position, waypoints[nextIndex].position);
		}
	}
}
