using System;
using System.Collections.Specialized;
using UnityEngine;

public class GhostScatter : MoveToTarget
{
	[SerializeField]
	public Transform homePosition;

	public void Start()
	{
		ghost.target = homePosition;
	}

	public override void Enable(float duration)
	{
		base.Enable(duration);
		ghost.target = homePosition;
		Debug.Log("SCATTER ENABLE!!");
		//updateBehaviour = true;
	}

	private void OnDisable()
	{
		if (!ghost.eaten.enabled)
			ghost.chase.Enable();
	}

	public override void Update()
	{
		if (!enabled || ghost.frightened.enabled)
			return;
		base.Update();
	}

	//Deprecated code:
	/*private void OnTriggerEnter(Collider other)
	{

		if (Node.allNodes.Count < 64) return;

		UnityEngine.Debug.Log("ghost entered trigger!");

		Node node = other.GetComponent<Node>();

		// Do nothing while the ghost is frightened
		//if (node != null && enabled && !ghost.frightened.enabled)
		if (node != null && enabled)
		{
			// Pick a random available direction
			//int index = UnityEngine.Random.Range(0, node.availableDirections.Count);

			// Pick a random neighbor node
			int index = UnityEngine.Random.Range(0, node.neighborNodes.Count);

			var newDir = (node.neighborNodes[index].transform.position - node.transform.position).normalized;


			if (node.neighborNodes.Count > 1 && newDir == -ghost.movement.direction)
			{
				index++;

				// Wrap the index back around if overflowed
				if (index >= node.neighborNodes.Count)
				{
					index = 0;
				}

				newDir = (node.neighborNodes[index].transform.position - node.transform.position).normalized;
			}

			//ghost.movement.SetDirection(newDir);
			ghost.movement.nextDirection = newDir;
			ghost.movement.SetExitLocation(node.transform.position);

			UnityEngine.Debug.Log("ghost newdir = " + newDir);

			UnityEngine.Debug.Log("ghost exit location = " + node.transform.position);

		}
	}*/

}
