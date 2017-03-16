using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
	private int tileId;
	private Unit occupant;
	private Vector3 position;
	private Renderer _renderer;

	void Awake() {
		position = new Vector3(transform.position.x, 0, transform.position.z);
		_renderer = GetComponent<Renderer>();
	}

	public int GetTileId() {
		return tileId;
	}

	public void SetTileId(int tileId) {
		this.tileId = tileId;
	}

	public bool IsAvailable() {
		return occupant == null; 
	}

	public Unit GetOccupant() {
		return occupant;
	}

	public void SetOccupant(Unit unit) {
		occupant = unit;
	}

	public void ClearOccupant() {
		occupant = null;
	}

	public Vector3 GetPosition() {
		return position;
	}

	public void SetMaterial(Material material) {
		_renderer.material = material;
	}
}
