using System;
using System.Collections;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;

public class GhostHome : GhostBehavior
{
	public Transform inside;
	public Transform outside;
	public Transform homeGate;

	private void OnEnable()
	{
		StopAllCoroutines();
	}

	private void OnDisable()
	{
		// Check for active self to prevent error when object is destroyed
		if (gameObject.activeInHierarchy)
		{
			StartCoroutine(ExitTransition());
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		// Reverse direction everytime the ghost hits a wall to create the
		// effect of the ghost bouncing around the home
		if (enabled && collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
		{
			ghost.movement.SetDirection(-ghost.movement.direction, true);
			UnityEngine.Debug.Log("bouncing off wall");
		}
	}

	private IEnumerator ExitTransition()
	{
		homeGate.gameObject.SetActive(false);
		UnityEngine.Debug.Log("exiting ghost home state!");
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
			ghost.SetPosition(Vector3.Lerp(position, inside.position, elapsed / duration));
			elapsed += Time.deltaTime;
			yield return null;
		}

		elapsed = 0f;

		// Animate exiting the ghost home
		//The ghost will move faster towards the outside node than it will move towards the inside node
		while (elapsed < duration)
		{
			ghost.SetPosition(Vector3.Lerp(inside.position, outside.position, elapsed / duration));
			elapsed += Time.deltaTime;
			yield return null;
		}

		// Pick a random direction left or right and re-enable movement
		//ghost.movement.SetDirection(new Vector3(UnityEngine.Random.value < 0.5f ? -1f : 1f, 0f, 0f), true);
		ghost.movement.SetDirection(new Vector3(-1f, 0f, 0f), true);

		//ghost.movement.rb.isKinematic = false;
		ghost.movement.enabled = true;

		//Reactivate the home gate
		homeGate.gameObject.SetActive(true);
	}

}
