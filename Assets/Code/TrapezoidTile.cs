using UnityEngine;
using System.Collections;

public class TrapezoidTile : BaseTile {

	public enum Alignment {
		Right,
		Left,
		Top
	}
	public Alignment Align;

	protected override int GetNumIndices () {
		return 9;
	}
	
	protected override int GetNumVerts () {
		return 5;
	}
	
	protected override void CreateMesh () {
		int vertOffset = 0;
		float x, y, angle;
		_vertices[vertOffset++] = new Vector3(0, 0, 0);

		int startIndex = 0;
		switch (Align) {
		case Alignment.Left:
			startIndex = 1;
			break;
		case Alignment.Right:
			startIndex = - 1;
			break;
		case Alignment.Top:
			startIndex = 3;
			break;
		}


		for(int i=startIndex,n=startIndex+4; i < n; i++) {
			angle = 2f * Mathf.PI / 6f * i;
			x = Size * Mathf.Cos(angle);
			y = Size * Mathf.Sin(angle);
			_vertices[vertOffset++] = new Vector3(x, y, 0);
		}
		vertOffset = 0;

		int indexOffset = 0;
		for(int i=0; i < 6; i++) {
			_indices[indexOffset++] = vertOffset;
			_indices[indexOffset++] = (i % 6) + 1 + vertOffset;
			_indices[indexOffset++] = ((i+1) % 6) + 1 + vertOffset;
		}
		
		vertOffset = 0;
		for (int i=0; i < 7; i++) {
			_normals[vertOffset++] = Vector3.up;
		}
		
		vertOffset = 0;
		_uvs [vertOffset++] = new Vector2 (0, 0);
		for (int i=1; i < 7; i++) {
			_uvs[vertOffset++] = new Vector2(1f, 0);
		}
	}
}
