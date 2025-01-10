using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
//[RequireComponent(typeof(Rigidbody))]
public class GhostMovement : MonoBehaviour
{
    public float speed = 8f;
    public float speedMultiplier = 1f;
    public Vector3 initialDirection;

	public Rigidbody rb { get; private set; }
    public Vector3 direction { get; set; }
    public Vector3 nextDirection { get; set; }
	public Vector3 startingPosition { get; private set; }
	public Vector3 initialPosition { get; private set; }

	private Vector3 exitLocation = Vector3.zero;

	public bool _reachedExitLocation = false;

	private Ghost ghost;


	private void Awake()
    {
        rb = GetComponentInChildren<Rigidbody>();
		startingPosition = initialPosition = transform.position;
		ghost = GetComponent<Ghost>();
    }

    private void Start()
    {
        ResetState();
    }


    public void ResetState()
    {
        speedMultiplier = 1f;
        direction = nextDirection = initialDirection;
		transform.position = startingPosition;
        //rb.isKinematic = false;
        enabled = true;
    }

    private void Update()
    {
		//rb.velocity = direction * speed * speedMultiplier;

		if (nextDirection != Vector3.zero)
		{
			SetDirection(nextDirection);
		}

		_reachedExitLocation = reachedExitLocation();

		UnityEngine.Debug.Log("cur ghost dir = " + direction);

		//rb.velocity = direction * speed * speedMultiplier;

		Vector3 translation = speed * speedMultiplier * Time.deltaTime * direction;

		transform.position += translation;

		if(!ghost.home.enabled)
			CorrectMovement();
	}

	private void CorrectMovement()
	{
		var posX = transform.position.x;
		var posZ = transform.position.z;

		if (Mathf.Abs(direction.z) > 0.1f)
		{
			var posXInt = (int)posX;

			if (posX > posXInt)
				posX = posXInt + 0.5f;
			else
				posX = posXInt - 0.5f;
		}
		else
		{
			posZ = Mathf.Round(posZ);
		}
		transform.position = new Vector3(posX, transform.position.y, posZ);
	}


	private void FixedUpdate()
    {

		/*Vector3 position = rb.position;
        Vector3 translation = speed * speedMultiplier * Time.fixedDeltaTime * direction;

        rb.MovePosition(position + translation);*/

	}


	public void SetExitLocation(Vector3 v)
	{
		exitLocation = v;
	}

	private bool reachedExitLocation()
	{
		UnityEngine.Debug.Log("ghost exit location = " + exitLocation);

		UnityEngine.Debug.Log("ghost dist to exit loc = " + Vector3.Distance(exitLocation, transform.position));

		if (Vector3.Distance(exitLocation, transform.position) < 0.1f && !_reachedExitLocation)
		{
			//if (exitLocation.x < -3.0f)
				//Time.timeScale = 0f;

			UnityEngine.Debug.Log("reached exit location: " + exitLocation);
			transform.position = exitLocation;
			rb.velocity = rb.velocity = Vector3.zero;
			return true;
		}

		return _reachedExitLocation;
		//return false;
	}

	public void SetDirection(Vector3 direction, bool forced = false)
    {
		//transform.forward = direction;
		UnityEngine.Debug.Log("ghost setting direction");

		UnityEngine.Debug.Log("new ghost dir = " + direction);
		//transform.forward = direction;
		if (_reachedExitLocation || forced)
		{
			//UnityEngine.Debug.Log("ghost reached exit location!");
			this.direction = direction;

			nextDirection = Vector3.zero;

			transform.forward = direction;

			//exitLocation = Vector3.zero;
		}
		
    }

}
