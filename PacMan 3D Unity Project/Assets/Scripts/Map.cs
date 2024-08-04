using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;

public class Map : MonoBehaviour
{
	[SerializeField]
	private LayerMask ignoreWallLayer;

	[SerializeField]
	private LayerMask ignoreNodeWallLayer;

	// A rectangular array can be initialized as follows:
	Vector3[,] map = new Vector3[29, 26];

	[SerializeField]
	private GameObject nodePrefab;

	public int nodeCount = 0;

	public static Map Instance;

	private void Awake()
	{
		Instance = this;
	}

	// Start is called before the first frame update
	private void Start()
    {
		InitVectorMap();
		buildGhostNodes();
		UnityEngine.Debug.Log("Node Count = " + nodeCount);
    }

	//x: -12.5 - 12.5
	//y: -13 - 15
	private void InitVectorMap()
	{
		for (int y = 0; y < 29; y++)
		{
			for (int x = 0; x < 26; x++)
			{
				map[y, x] = new Vector3(x - 12.5f, 0, y - 13);
			}
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	private void buildGhostNodes()
	{
		RaycastHit hit;

		for (int y = 0; y < 29; y++)
		{
			for (int x = 0; x < 26; x++)
			{
				if (isLocationWalkable(map[y, x]))
				{
					UnityEngine.Debug.DrawRay(map[y, x] + Vector3.up * 10, -Vector3.up * 15, Color.red, 5f);
					tryBuildGhostNode(new Vector3(map[y, x].x,1, map[y, x].z));
				}
			}
		}
	}

	public bool isLocationWalkable(Vector3 location)
	{
		RaycastHit hit;
		if (Physics.Raycast(location + Vector3.up * 10, -Vector3.up, out hit, Mathf.Infinity, ignoreNodeWallLayer))
		{
			//do a sphere overlap to check for obastacles
			Collider[] hitColliders = Physics.OverlapSphere(hit.point, .5f, ~ignoreWallLayer);
			if(hitColliders.Length > 0)
				return false;

			return true;
		}

		return false;
	}

	private void tryBuildGhostNode(Vector3 location)
	{
		bool locationValid = false;

		Vector3 left = new Vector3(location.x - 1, 0, location.z) ;
		Vector3 right = new Vector3(location.x + 1, 0, location.z);

		Vector3 up = new Vector3(location.x, 0, location.z+1);
		Vector3 down = new Vector3(location.x , 0, location.z-1);

		locationValid = (isLocationWalkable(left) && isLocationWalkable(up))
				|| (isLocationWalkable(right) && isLocationWalkable(up))
				|| (isLocationWalkable(left) && isLocationWalkable(down))
				|| (isLocationWalkable(right) && isLocationWalkable(down));

		if (locationValid)
		{
			GameObject node = Instantiate(nodePrefab, location, Quaternion.identity);
			//Node n = node.AddComponent(typeof(Node)) as Node;
			nodeCount++;
		}

	}

}
