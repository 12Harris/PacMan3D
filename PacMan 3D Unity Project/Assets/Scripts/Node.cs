using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;

public class Node : MonoBehaviour
{
    public LayerMask nodeWallLayer;
    public readonly List<Vector3> availableDirections = new();

	public List<Node> neighborNodes = new();

    public bool didInit = false;

    public static List<Node> allNodes = new();

	private void Start()
    {
        availableDirections.Clear();
        neighborNodes.Clear();
        allNodes.Add(this);

        // We determine if the direction is available by box casting to see if
        // we hit a wall. The direction is added to list if available.
        /*CheckAvailableDirection(Vector2.up);
        CheckAvailableDirection(Vector2.down);
        CheckAvailableDirection(Vector2.left);
        CheckAvailableDirection(Vector2.right);*/

	}

	private void CheckAvailableNodes(Vector3 direction)
	{
        RaycastHit hit;

		UnityEngine.Debug.DrawRay(transform.position, direction * 3, Color.blue, 5f);

		if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity, nodeWallLayer))
        {

            UnityEngine.Debug.DrawRay(transform.position, direction, Color.green, 5f);

            if (hit.collider != null && hit.collider.gameObject.TryGetComponent<Node>(out Node node))
            {
                if (node != this)
                {
                    UnityEngine.Debug.Log("Node added to neighbour nodes");
                    neighborNodes.Add(node);
                }
            }
        }

        else
        {
			UnityEngine.Debug.Log("Node has no neighbors???");
		}
	}

    public void Update()
    {
        UnityEngine.Debug.Log("Map.Instance.nodeCount = " + Map.Instance.nodeCount);

		if (!didInit && Map.Instance.nodeCount == 64)
		{
			CheckAvailableNodes(Vector3.forward);
			CheckAvailableNodes(-Vector3.forward);
			CheckAvailableNodes(Vector3.left);
			CheckAvailableNodes(Vector3.right);
            didInit = true;
		
		}
    }

}
