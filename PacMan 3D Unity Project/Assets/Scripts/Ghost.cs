using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public enum State {Home, Scatter, Chase, Frightened}

[DefaultExecutionOrder(-10)]
[RequireComponent(typeof(GhostMovement))]
public class Ghost : MonoBehaviour
{
	public GhostMovement movement { get; private set; }
	public GhostHome home { get; private set; }

	//Ghost behaviours
	public GhostScatter scatter { get; private set; }
	public GhostChase chase { get; private set; }
	public GhostFrightened frightened { get; private set; }
	public GhostEaten eaten { get; private set; }
	public GhostBehavior initialBehavior;

	public State state;
	//target is pacman
	public Transform target;
	public int points = 200;

	public BoxCollider ghostCollider;

	public virtual void Awake()
	{
		movement = GetComponent<GhostMovement>();

		home = GetComponent<GhostHome>();
		scatter = GetComponent<GhostScatter>();
		chase = GetComponent<GhostChase>();
		frightened = GetComponent<GhostFrightened>();
		eaten = GetComponent<GhostEaten>();
	}

	private void Start()
	{
		ResetState();
	}

	public void ResetState()
	{
		gameObject.SetActive(true);
		movement.ResetState();

		frightened.Disable();
		//chase.enabled = false;
		//scatter.enabled = false;
		eaten.Disable();
		home.Disable();

		if (initialBehavior != null)
		{
			Debug.Log("enabling initial behaviour");
			initialBehavior.Enable();
		}
	
		//if home is the initial behaviour for this ghost, then scatter will also be enabled, because scatter is the next behaviour to transition to

		//the transitions are: home/scatter => chase
	}

	public void SetPosition(Vector3 position)
	{
		// Keep the y-position the same since it determines draw depth
		position.y = transform.position.y;
		transform.position = position;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
		{
			if (frightened.enabled)
			{
				UnityEngine.Debug.Log("pacman touched ghost!");
				GameManager.Instance.GhostEaten(this);
				ghostCollider.isTrigger = true;
			}
			else
			{
				GameManager.Instance.PacmanEaten();
			}
		}
	}

}

