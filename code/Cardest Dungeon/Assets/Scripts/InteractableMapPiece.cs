using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableMapPiece : Interactable
{
    [SerializeField]
	private Vector2Int unlockableMapPieceCoordinate;
	public override bool Interact()
	{
		MapPiece mapPiece = MapManager.Current.GetMapPiece(unlockableMapPieceCoordinate.x, unlockableMapPieceCoordinate.y);
		mapPiece.IsUnlocked = true;
		Destroy(gameObject);
		return true;
	}
}
