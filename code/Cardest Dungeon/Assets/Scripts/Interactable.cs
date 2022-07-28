using UnityEngine;

public class Interactable : MonoBehaviour
{

	public float radius = 3f;               // How close do we need to be to interact?
	public Transform interactionTransform;  // The transform from where we interact in case you want to offset it

	/// <summary>
	/// <returns>true if the interaction was successfull. Else, false.</returns>
	/// </summary>
	public virtual bool Interact()
	{
		return false;
	}


	// Draw our radius in the editor
	void OnDrawGizmosSelected()
	{
		if (interactionTransform == null)
			interactionTransform = transform;

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(interactionTransform.position, radius);
	}

}