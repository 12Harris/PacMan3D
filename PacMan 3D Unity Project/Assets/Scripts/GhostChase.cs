using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;
using UnityEngine;

public class GhostChase : MoveToTarget
{

	public void Start()
	{
		ghost.target = Pacman.Instance.transform;
	}

	public override void Enable(float duration)
	{
		base.Enable(duration);
		ghost.target = Pacman.Instance.transform;
		//updateBehaviour = true;
	}

	private void OnEnable()
	{
		ghost.movement.direction *= -1;
	}


	private void OnDisable()
	{
		if(!ghost.eaten.enabled) 
			ghost.scatter.Enable();
	}

	public override void Update()
	{
		if (!enabled || ghost.frightened.enabled)
			return;
		base.Update();
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
