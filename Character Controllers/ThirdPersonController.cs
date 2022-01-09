using UnityEngine;
using System.Collections;

public class ThirdPersonController : MonoBehaviour {

	[Header("Movement Settings")]
	public float mouseSensitivity = 1f;
	public float moveSpeed = 7f;
	public float jumpHeight = 4f;
	public int maxJumps = 2;
	public float viewRange = 89.9f;
	public float gravityFactor = 2f;
	public LayerMask groundMask;
	public float groundedTolerance = 0.04f;
	public float slopeCheckDist = 0.1f;
	public float slopeForce = 2f;

	[Header("Object References")]
	public GameObject cameraTurntable;
	private CharacterController cc;

	// private member variables
	private float xRot;
	private float yRot;
	private Vector2 keyInput;
	private float verticalVelocity;
	private int currentJumps;
	private bool isGrounded;
	private bool isOnSlope;

	// getters and setters
	public Vector2 getKeyInput() { return keyInput; }
	public bool getGrounded() { return isGrounded; }
	public bool getOnSlope() { return isOnSlope; }

	void Start () {

		// get references
		cc = gameObject.GetComponent<CharacterController>();

		// lock cursor
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

	}

	// assumes the player's origin is at the feet
	// BUG: When transitioning from a steep slope to a flat surface, this registers as falling.
	private bool Grounded(float tol)
	{
		const float MARGIN = 0.34f;
		Vector3 startPosition = cc.center + transform.position;
		tol += cc.height * 0.5f; // start at the center and shoot down
		//Debug.DrawRay(startPosition, Vector3.down * tol);
		return (Physics.Raycast(startPosition, Vector3.down, tol, groundMask)
			 || (Physics.Raycast(new Vector3(startPosition.x, startPosition.y, startPosition.z + MARGIN), Vector3.down, tol, groundMask))
			 || (Physics.Raycast(new Vector3(startPosition.x, startPosition.y, startPosition.z - MARGIN), Vector3.down, tol, groundMask))
			 || (Physics.Raycast(new Vector3(startPosition.x + MARGIN, startPosition.y, startPosition.z), Vector3.down, tol, groundMask))
			 || (Physics.Raycast(new Vector3(startPosition.x - MARGIN, startPosition.y, startPosition.z), Vector3.down, tol, groundMask)));
	}
	private bool CheckOnSlope()
	{
		isOnSlope = false;

		// if inputting a jump, return false
		if (Input.GetButton("Jump"))
		{
			return false;
		}

		// check ground normal
		Vector3 startPosition = cc.center + transform.position;
		float tol = slopeCheckDist;
		tol += cc.height * 0.5f; // start at the center and shoot down
								 //Debug.DrawRay(startPosition, Vector3.down * tol);
		RaycastHit hit;
		bool hitSomething = Physics.Raycast(startPosition, Vector3.down, out hit, tol, groundMask);

		// if there is no hit, we are not on a slope
		if (!hitSomething)
			return false;

		// we hit something, so it will be a slope if the normal is not (0, 1, 0)
		if (hit.normal != Vector3.up)
        {
			isOnSlope = true;
			return true;
        }

		return false;
	}

	void Update () {

		// get input
		xRot += Input.GetAxis ("Mouse X") * mouseSensitivity;
		yRot -= Input.GetAxis ("Mouse Y") * mouseSensitivity;

		keyInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

		// rotate and clamp camera
		yRot = Mathf.Clamp (yRot, -viewRange, viewRange);
		cameraTurntable.transform.localRotation = Quaternion.Euler(yRot, xRot, 0);

		// check if the plauer is groudned
		if (Grounded(groundedTolerance))
		{
			// land
			if (isGrounded == false)
            {
				currentJumps = 0;
				verticalVelocity = 0f;
				isGrounded = true;
			}

		}
		else
        {
			// fall
			isGrounded = false;
			verticalVelocity += (Physics.gravity.y * gravityFactor) * Time.deltaTime;

			// make sure the player can't use their first jump in the air
			if (currentJumps == 0)
				currentJumps++;
		}

		// check if we can jump
		if (currentJumps < maxJumps)
		{
			if (Input.GetButtonDown("Jump"))
			{
				Jump();
			}
		}

		// make horizontal movement vector
		Vector3 movement = new Vector3 (keyInput.x, 0f, keyInput.y);

		// normalize movement
		if (movement.sqrMagnitude > 1f)
		{
			movement = movement.normalized;
		}

		// apply move speed and vertical velocity
		movement *= moveSpeed;
		movement.y = verticalVelocity;
		// apply sloped movement when moving on a slope
		if ((keyInput.sqrMagnitude > 0) && CheckOnSlope())
        {
			//print("ON SLOPE");
			movement.y -= cc.height * 0.5f * slopeForce;
		}

		// move towards the camera's forward orientation
		Vector3 moveDir = cameraTurntable.transform.localRotation.eulerAngles;
		moveDir.x = 0f; // cancel vertical movement
		movement = Quaternion.Euler(moveDir) * movement;

		// apply to character controller
		cc.Move (movement * Time.deltaTime);
	}

	private void Jump()
    {
		verticalVelocity = jumpHeight;
		currentJumps++;
	}
}

