using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class GhostEaten : MoveToTarget
{
	[SerializeField]
	private Transform eyeHomePosition;

	[SerializeField]
	private Transform body;

	public Transform inside;
	public Transform outside;

	private bool reachedDestination = false;

	public override void Enable(float duration)
    {
		enabled = true;
		UnityEngine.Debug.Log("ghost eaten enabled!");
		ghost.target = eyeHomePosition;
		//body.gameObject.SetActive(false);
		GetComponentInChildren<MeshRenderer>().enabled = false;
		ghost.movement.speedMultiplier = 2f;
	}

	private void OnEnable()
	{
		ghost.chase.Disable();
		ghost.scatter.Disable();
		ghost.frightened.Disable();
		reachedDestination = false;
	}


	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    public override void Update()
    {
		base.Update();
		if (Vector3.Distance(eyeHomePosition.position, transform.position) < 0.1f &&  !reachedDestination)
		{
			StartCoroutine(ExitTransition());
			reachedDestination = true;
		}
    }

	private IEnumerator ExitTransition()
	{

		UnityEngine.Debug.Log("exiting ghost eaten state!");
		// Turn off movement while we manually animate the position
		//ghost.movement.SetDirection(Vector2.up, true);
		//ghost.movement.rb.isKinematic = true;
		ghost.movement.enabled = false;

		Vector3 position = transform.position;

		float duration = 0.5f;
		float elapsed = 0f;

		// Animate to the starting point
		while (elapsed < duration)
		{
			ghost.SetPosition(Vector3.Lerp(position, outside.position, elapsed / duration));
			elapsed += Time.deltaTime;
			yield return null;
		}

		elapsed = 0f;

		// Animate exiting the ghost home
		//The ghost will move faster towards the outside node than it will move towards the inside node
		while (elapsed < duration)
		{
			ghost.SetPosition(Vector3.Lerp(outside.position, inside.position, elapsed / duration));
			elapsed += Time.deltaTime;
			yield return null;
		}

		GetComponentInChildren<MeshRenderer>().enabled = true;
		ghost.home.Enable(ghost.home.duration);
		Disable();
		ghost.ResetState();

	}
}
