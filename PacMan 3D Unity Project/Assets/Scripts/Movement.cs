using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    public float speed = 8f;
    public float speedMultiplier = 1f;
    public Vector3 initialDirection;
    public LayerMask obstacleLayer;
	public LayerMask nodeLayer;

	public Rigidbody rb { get; private set; }
    public Vector3 direction { get; private set; }
    public Vector3 nextDirection { get; private set; }
    public Vector3 startingPosition { get; private set; }

    private Vector3 exitDirection = Vector3.zero;

    private Pacman pacman;

    [SerializeField]
    private GameObject rays;

    private Vector3 exitLocation = Vector3.zero;
	private Vector3 finalPos = Vector3.zero;

    private bool calculateExitLocation = true;
    private bool _reachedExitLocation = false;
    private bool _reachedFinalLocation = false;

	private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pacman = GetComponent<Pacman>();
		startingPosition = transform.position;
    }

    private void Start()
    {
        ResetState();
    }

    public void ResetState()
    {
        speedMultiplier = 1f;
        //direction = initialDirection;
        direction = Vector3.zero;
        nextDirection = Vector3.zero;
        transform.position = startingPosition;
        transform.forward = initialDirection;
        rb.isKinematic = false;
        enabled = true;
    }

    public void calcClosestExitLocation(Vector3 direction)
    {
		exitLocation = getClosestExitLocation(nextDirection);
        UnityEngine.Debug.Log("exit lcoation = " + exitLocation);
	}

    private void Update()
    {
        // Try to move in the next direction while it's queued to make movements
        // more responsive
        if (nextDirection != Vector3.zero)
        {
            //if(calculateExitLocation)
            //exitLocation = getClosestExitLocation(nextDirection);

            UnityEngine.Debug.Log("exit location = " + exitLocation);

            SetDirection(nextDirection);
        }

        else if (exitDirection != Vector3.zero) 
        {
            SetDirection(exitDirection);
            UnityEngine.Debug.Log("setting exit direction?");
			//exitLocation = Vector3.zero;
		}

        else
        {
            UnityEngine.Debug.Log("next direction is v zero!");
        }

        _reachedExitLocation = reachedExitLocation();

		finalPos = getFinalPos();


		_reachedFinalLocation = reachedFinalLocation();

		UnityEngine.Debug.Log("final pos = " + finalPos);

		if (_reachedFinalLocation)
        {
			transform.position = finalPos;
			this.direction = Vector3.zero;
		}

		UnityEngine.Debug.Log("Direction = " + direction);

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

		Vector3 position = rb.position;
        Vector3 translation = speed * speedMultiplier * Time.fixedDeltaTime * direction;

        rb.MovePosition(position + translation);
    }

    public void SetDirection(Vector3 direction, bool forced = false)
    {
        if(direction == Vector3.zero)
        {
            //this.direction = Vector3.zero;
            //nextDirection = Vector3.zero;
            //return;
        }

        if (calculateExitLocation && exitLocation == Vector3.zero)
        {
			exitDirection = direction;

			exitLocation = getClosestExitLocation(exitDirection);

			UnityEngine.Debug.Log("calculating exit location!");
        }

        UnityEngine.Debug.Log("exit location = "  + exitLocation);
        //Time.timeScale = 0f;

		UnityEngine.Debug.Log("setting direction");

		rays.transform.forward = direction;
		// Only set the direction if the tile in that direction is available
		// otherwise we set it as the next direction so it'll automatically be
		// set when it does become available
		//if (forced || !Occupied(direction)) //Improvement: The occupied check is only needed for pacman and not for the ghosts
        
        //if physics.raycast(transform.position, transform.forward
		//if (forced || (!Occupied(direction) && (exitLocation == Vector3.zero || reachedExitLocation())))

		if (forced || (!Occupied(direction)))
		{
			//transform.forward = direction;
			if (_reachedExitLocation || _reachedFinalLocation || direction == -this.direction)
            {
				UnityEngine.Debug.Log("reached exit location!");
				this.direction = direction;

                nextDirection = Vector3.zero;

                transform.forward = direction;

                exitLocation = Vector3.zero;

                exitDirection = Vector3.zero;
            }

			//nextDirection = Vector3.zero;
			UnityEngine.Debug.Log("direction: " + direction + "NOT OCCUPIED!!!");
            //Time.timeScale = 0f;

		}

        //direction is blocked
        else if(Occupied(direction))
		{
            UnityEngine.Debug.Log("direction: " + direction + "is blocked???");
            nextDirection = direction;
        }
    }

	private bool reachedExitLocation()
    {
        UnityEngine.Debug.Log("dist = " + Vector3.Distance(exitLocation, transform.position));

		if (Vector3.Distance(exitLocation, transform.position) < 0.1f)
        {
            UnityEngine.Debug.Log("reached exit location!");
            transform.position = exitLocation;
            //StartCoroutine(pauseExitLocationCalculation(0.2f));
            return true;
        }
        return false;
    }

	private bool reachedFinalLocation()
	{
		return Vector3.Distance(finalPos, transform.position) < 0.1f;
	}

	private Vector3 getClosestExitLocation(Vector3 exitDir)
    {

        //if (exitLocation != Vector3.zero && !reachedExitLocation())
            //return exitLocation;

		RaycastHit[] hits;
        Ray ray = new Ray(transform.position, transform.forward);
        hits = Physics.RaycastAll(ray, 100f, nodeLayer);

        float minDistance = Mathf.Infinity;

		Vector3 result = Vector3.zero;

        for (int i = 0; i < hits.Length; ++i)
        {
            RaycastHit hit = hits[i];

            var pos = hit.collider.gameObject.transform.position;

            if (Map.Instance.isLocationWalkable(pos + exitDir))
            {
                var distanceToGO = Vector3.Distance(transform.position, pos);
                if (distanceToGO < minDistance)
                {
                    minDistance = distanceToGO;
                    result = pos;

                }
            }
		}

        return result;
	}

    private Vector3 getFinalPos()
    {
		Vector3 result = Vector3.zero;

        RaycastHit hit, hit2;

        float distToObstacle;
        //if (Physics.Raycast(location + Vector3.up * 10, -Vector3.up, out hit, Mathf.Infinity, ignoreNodeWallLayer))
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, obstacleLayer))
        {
            if (Physics.Raycast(hit.point, -transform.forward, out hit2, Mathf.Infinity, nodeLayer))
            {
                result = hit2.collider.gameObject.transform.position;
            }
		}
        return result;
	}

    public bool Occupied(Vector3 direction)
    {

        RaycastHit hit;

        //var rayOrigin = transform.forward.x > 0 ? pacman.leftRayOrigin.position : pacman.rightRayOrigin.position;
        var rayOrigin = Vector3.zero;

        if (direction == Vector3.right)
            rayOrigin = pacman.leftRayOrigin.position;

        else if (direction == Vector3.left)
			rayOrigin = pacman.rightRayOrigin.position;
        
        else
		    rayOrigin = transform.position;


		UnityEngine.Debug.DrawRay(rayOrigin, direction * 3f, Color.red);

        if (Physics.Raycast(rayOrigin, direction, out hit, 3f, obstacleLayer))
        {
			UnityEngine.Debug.Log("direction is blocked by: " + hit.collider.gameObject);
            UnityEngine.Debug.DrawRay(hit.point, Vector3.up * 1000, Color.blue);
			return true;
        }

		return false;
    }

	private void OnTriggerEnter(Collider other)
	{
        UnityEngine.Debug.Log("entered " + other.gameObject);
		//finalPos = new Vector3(other.transform.position.x,transform.position.y,other.transform.position.z);
	}

}
