using UnityEngine;
using System.Collections;

public class FirstPersonController : MonoBehaviour {

	[Header("Object References")]
	public GameObject cam;

	[Header("Movement Settings")]
	public float mouseSensitivity = 1f;
	public float moveSpeed = 10f;
	public float jumpHeight = 5f;
	public int maxJumps = 2;
	public float viewRange = 89.9f;
	public float gravityFactor = 2f;

	// private member variables
	private CharacterController cc;
	private Collider col;
	private float xRot;
	private float yRot;
	private float moveX;
	private float moveZ;
	private float verticalVelocity;
	private int currentJumps;
	private Vector3 pos;

	void Start () {

		// get references
		cc = gameObject.GetComponent<CharacterController> ();
		col = gameObject.GetComponent<Collider>();

		// lock cursor
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

	}

	// shoots five rays around the base of the character
	private bool Grounded (float tol) {
		const float MARGIN = 0.4f;
		float extents = gameObject.GetComponent<Collider>().bounds.extents.y + tol;
		return (Physics.Raycast(transform.position, Vector3.down, extents))
			|| (Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z + MARGIN), Vector3.down, extents))
			|| (Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z - MARGIN), Vector3.down, extents))
			|| (Physics.Raycast(new Vector3(transform.position.x + MARGIN, transform.position.y, transform.position.z), Vector3.down, extents))
			|| (Physics.Raycast(new Vector3(transform.position.x - MARGIN, transform.position.y, transform.position.z), Vector3.down, extents));
	}

	void Update () {

		// get input
		xRot = Input.GetAxis ("Mouse X") * mouseSensitivity;
		yRot -= Input.GetAxis ("Mouse Y") * mouseSensitivity;

		moveX = Input.GetAxis("Horizontal");
		moveZ = Input.GetAxis("Vertical");

		// rotate and clamp camera
		transform.Rotate (new Vector3(0, xRot, 0));
		yRot = Mathf.Clamp (yRot, -viewRange, viewRange);
		cam.transform.localRotation = Quaternion.Euler(yRot, 0, 0);

		// check if the plauer is groudned
		if (Grounded(0.1f))
		{
			if (!Input.GetButton("Jump"))
			{
				currentJumps = 0;
				verticalVelocity = 0f;
			}
		}
		else
        {
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
		Vector3 movement = new Vector3 (moveX, 0f, moveZ);

		// normalize movement
		if (movement.sqrMagnitude > 1f)
		{
			movement = movement.normalized;
		}

		// apply move speed and vertical velocity
		movement *= moveSpeed;
		movement.y = verticalVelocity;

		// move towards the camera's forward orientation
		movement = transform.rotation * movement;

		// apply to character controller
		cc.Move (movement * Time.deltaTime);
	}

	private void Jump()
    {
		verticalVelocity = jumpHeight;
		currentJumps++;
	}
}

