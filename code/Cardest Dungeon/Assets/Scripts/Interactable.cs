using UnityEngine;

/*	
	This component is for all objects that the player can
	interact with such as enemies, items etc. It is meant
	to be used as a base class.
*/

public class Interactable : MonoBehaviour
{
	//public Item item;
	public float radius = 3f;               // How close do we need to be to interact?
	public Transform interactionTransform;  // The transform from where we interact in case you want to offset it

	/// <summary>
	/// 
	/// </summary>
	/// <returns>true if the interaction was successfull. Else, false.</returns>
	public virtual bool Interact()
	{
		return false;
	}


	// Draw the radius in the editor
	void OnDrawGizmosSelected()
	{
		if (interactionTransform == null)
			interactionTransform = transform;

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(interactionTransform.position, radius);
	}

}