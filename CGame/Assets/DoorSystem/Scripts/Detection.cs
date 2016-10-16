////////////////////////////////////
//Last edited by: Alexander Ameye //
//on: Wednesday, 11/11/2015          //
////////////////////////////////////

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Detection : MonoBehaviour
{
	// INSPECTOR SETTINGS
	[Header("Detection Settings")]
	[Tooltip("Within this radius the player is able to open/close the door")]
	public float Reach = 4F;
	[Tooltip("The tag that triggers the object to be openable")]
	public string TriggerTag = "Door";

	// PRIVATE SETTINGS
	[HideInInspector] public bool InReach;

	//UPDATE FUNCTION
	void Update()
	{
		// Set origin of ray to 'center of screen' and direction of ray to 'cameraview'.
		Ray ray = Camera.main.ViewportPointToRay (new Vector3 (0.5F, 0.5F, 0F));

		RaycastHit hit; // Variable reading information about the collider hit.

		// Cast a ray from the center of screen towards where the player is looking.
		if (Physics.Raycast (ray, out hit, Reach))
		{
			if(hit.collider.tag == TriggerTag)
			{
				InReach = true;
				// Give the object that was hit the name 'Door'.
				GameObject Door = hit.transform.gameObject;

				// Get access to the 'DoorOpening' script attached to the door that was hit.
				Door dooropening = Door.GetComponent<Door>();

				// Check whether the door is opening/closing or not.
				if (dooropening.Running == false)
				{
					// Open/close the door by running the 'Open' function in the 'DoorOpening' script.
					StartCoroutine (hit.collider.GetComponent<Door>().Open());
				}
			}
			else InReach = false;
		}

		else {
			InReach = false;
		}
	}
}
