using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableMapPiece : Interactable
{
    [SerializeField]
	private Vector2Int mapPieceCoordinate;
	public override bool Interact()
	{
		MapPiece mapPiece = MapManager.Current.GetMapPiece(mapPieceCoordinate.x, mapPieceCoordinate.y);
		mapPiece.IsUnlocked = true;
		Destroy(gameObject);
		return true;
	}
}
