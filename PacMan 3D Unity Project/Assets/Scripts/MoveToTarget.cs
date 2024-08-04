using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;
using UnityEngine;

public class MoveToTarget : GhostBehavior
{
	[SerializeField]
	private LayerMask nodeLayer;

	private Node hitNode;

	private void OnEnable()
	{
		ghost.movement.direction *= -1;
	}

	private void calculatePinkyTarget()
	{
		UnityEngine.Debug.Log("Ghost is Pinky!");
		if ((Pacman.Instance.transform.position.x - Pacman.Instance.fourTilesAway.position.x < -12.5f)
			|| (Pacman.Instance.transform.position.x + Pacman.Instance.fourTilesAway.position.x > 12.5f)
			|| (Pacman.Instance.transform.position.z - Pacman.Instance.fourTilesAway.position.z < -13f)
			|| (Pacman.Instance.transform.position.z + Pacman.Instance.fourTilesAway.position.z > 15f))

			ghost.target = Pacman.Instance.transform;

		else
		{
			UnityEngine.Debug.Log("Target is 4 tiles away");
			ghost.target = Pacman.Instance.fourTilesAway;
		}
	}

	private void calculateInkyTarget()
	{
		UnityEngine.Debug.Log("Ghost is Inky!");

		Transform intermediateTarget;

		//first calculate intermediate target tile
		if ((Pacman.Instance.transform.position.x - Pacman.Instance.twoTilesAway.position.x < -12.5f)
			|| (Pacman.Instance.transform.position.x + Pacman.Instance.twoTilesAway.position.x > 12.5f)
			|| (Pacman.Instance.transform.position.z - Pacman.Instance.twoTilesAway.position.z < -13f)
			|| (Pacman.Instance.transform.position.z + Pacman.Instance.twoTilesAway.position.z > 15f))
		{
			UnityEngine.Debug.Log("Inky => Target = Pacman");
			intermediateTarget = Pacman.Instance.transform;
		}

		else
		{
			UnityEngine.Debug.Log("Inky => Target = Pm 2 tiles away");
			intermediateTarget = Pacman.Instance.twoTilesAway;
		}

		//Get vector to blinky and rotate 180 degrees, this causes inky to flank pacman when blinky and inky are far apart
		Vector3 v = Blinky.Instance.transform.position - intermediateTarget.position;
		v.x *= -1;
		v.z *= -1;

		Inky.Instance.InkyTarget.position = intermediateTarget.position + v;

		ghost.target = Inky.Instance.InkyTarget;
	}

	public virtual void Update()
	{

		RaycastHit hit;
		UnityEngine.Debug.DrawRay(transform.position, ghost.movement.direction, Color.red);
		if (Physics.Raycast(transform.position, ghost.movement.direction, out hit, 1f, nodeLayer))
		{
			Node node = hit.collider.gameObject.GetComponent<Node>();
			// Do nothing while the ghost is frightened
			if (node != null && node != hitNode)
			{

				//...do calculations specific to the ghost chase state
				if (this is GhostChase)
				{
					//...do calculations specific to ghost Pinky
					//Pinky targets the tile four tiles in front of Pacman, this tile must be within the map otherwise it targets pacman directly
					if (ghost is Pinky)
					{
						calculatePinkyTarget();

					}

					//...do calculations specific to ghost Inky
					//Inky's target position depends on both pacmans position and blinky's position
					else if (ghost is Inky)
					{
						calculateInkyTarget();
					}

				}

				UnityEngine.Debug.Log("ray hit node");

				hitNode = node;

				Vector3 direction = Vector3.zero;
				float minDistance = float.MaxValue;

				// Find the available direction that moves closet to the target, which is not the inverse of the current direction
				foreach (Node nnode in node.neighborNodes)
				{
					var nodeDir = (nnode.transform.position - node.transform.position).normalized;

					UnityEngine.Debug.Log("nodeDir = " + nodeDir);

					if (nodeDir == -ghost.movement.direction)
						continue;

					Vector3 newPosition = transform.position + nodeDir;

					float distance = (ghost.target.position - newPosition).sqrMagnitude;

					if (distance < minDistance)
					{
						direction = nodeDir;
						minDistance = distance;
					}
				}

				ghost.movement.nextDirection = direction;
				ghost.movement.SetExitLocation(node.transform.position);
				ghost.movement._reachedExitLocation = false;

			}
		}
	}

	/*private void OnTriggerEnter(Collider other)
	{

		//ghost.movement.rb.isKinematic = true;

		UnityEngine.Debug.Log("collided with node at location:  " + other.gameObject.transform.position);

		Node node = other.GetComponent<Node>();

		// Do nothing while the ghost is frightened
		if (node != null && enabled && (!ghost.frightened.enabled || ghost.eaten.enabled))
		{
			//isColliding = true;

			Vector3 direction = Vector3.zero;
			float minDistance = float.MaxValue;

			// Find the available direction that moves closet to the target, which is not the inverse of the current direction
			foreach (Node nnode in node.neighborNodes)
			{
				var nodeDir = (nnode.transform.position - node.transform.position).normalized;

				UnityEngine.Debug.Log("nodeDir = " + nodeDir);

				if (nodeDir == -ghost.movement.direction)
					continue;

				Vector3 newPosition = transform.position + nodeDir;

				float distance = (ghost.target.position - newPosition).sqrMagnitude;

				if (distance < minDistance)
				{
					direction = nodeDir;
					minDistance = distance;
				}
			}

			ghost.movement.nextDirection = direction;
			ghost.movement.SetExitLocation(node.transform.position);
			ghost.movement._reachedExitLocation = false;

			UnityEngine.Debug.Log("next dir = " + direction);
		}
	}*/

}
