using UnityEngine;
using GPC;
using System.Security.Cryptography;

[RequireComponent(typeof(Movement))]
public class Pacman : MonoBehaviour
{

    private BoxCollider boxCollider;

    private KeyboardInput keyboardInput;
    private Movement movement;

    public Transform leftRayOrigin;

	public Transform rightRayOrigin;

	public Transform topRayOrigin;

	public Transform bottomRayOrigin;

	public Transform fourTilesAway;

	public Transform twoTilesAway;

	public static Pacman Instance;

	private void Awake()
    {
		boxCollider = GetComponent<BoxCollider>();
        movement = GetComponent<Movement>();
        keyboardInput = GetComponent<KeyboardInput>();
        Instance = this;
	}

    private void Update()
    {
        // Set the new direction based on the current input
        //if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
		if (keyboardInput.Up){
			movement.SetDirection(Vector3.forward);
            //transform.forward = Vector3.forward;

		}

        //else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
        else if(keyboardInput.Down) {
			movement.SetDirection(-Vector3.forward);
			//transform.forward = -Vector3.forward;
		}


        //else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
		else if (keyboardInput.Left){
			movement.SetDirection(Vector3.left);
            //transform.forward = Vector3.left;
		}

        //else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
		else if (keyboardInput.Right){
			movement.SetDirection(Vector3.right);
			//transform.forward = Vector3.right;
		}

        //Debug.Log("Move Dir = " + keyboardInput.GetMovementDirectionVector());

		// Rotate pacman to face the movement direction
		//float angle = Mathf.Atan2(movement.direction.z, movement.direction.x);
        //transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }

    public void ResetState()
    {
        gameObject.SetActive(true);
        enabled = true;
		boxCollider.enabled = true;
        movement.enabled = true;
        //movement.ResetState();

        //movement.SetDirection(Vector3.zero);
    }

    public void DeathSequence()
    {
        
        gameObject.SetActive(false);
        enabled = false;
		boxCollider.enabled = false;
        movement.ResetState();
        movement.enabled = false;
    }

}
