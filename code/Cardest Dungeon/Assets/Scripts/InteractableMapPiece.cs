using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableMapPiece : Interactable
{
    [SerializeField]
	private Vector2Int unlockableMapPieceCoordinate;
	[SerializeField]
	private AudioSource pickupSound;
	[SerializeField]
	private GameObject audioPlayerPrefab;
	public override bool Interact()
	{
		MapPiece mapPiece = MapManager.Current.GetMapPiece(unlockableMapPieceCoordinate.x, unlockableMapPieceCoordinate.y);
		mapPiece.IsUnlocked = true;
		GameObject audioPlayer = Instantiate(audioPlayerPrefab);
		audioPlayer.GetComponent<AudioSource>().clip = pickupSound.clip;
		Destroy(gameObject);
		return true;
	}
}
