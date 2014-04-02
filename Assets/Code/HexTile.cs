using UnityEngine;
using System.Collections;

public class HexTile : BaseTile {

	protected override int GetNumIndices () {
		return 18;
	}

	protected override int GetNumVerts () {
		return 7;
	}

	protected override void CreateMesh () {

		int vertOffset = 0;
		float x, y, angle;
		_vertices[vertOffset++] = new Vector3(0, 0, 0);
		for(int i=0; i < 6; i++) {
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

		Expand ();
	}

	private void Expand() {
		
		
		Vector3[] worldVerts = new Vector3[_vertices.Length];
		for (int i=0, n=_vertices.Length; i < n; i++) {
			worldVerts[i] = this.transform.TransformPoint( _vertices[i] );
			worldVerts[i] = Vector3.Slerp(worldVerts[i], worldVerts[i].normalized * SphereRadius, SphereExpansionFactor);
		}
		
		this.transform.position = Vector3.Slerp(Center, Center.normalized * SphereRadius, SphereExpansionFactor);
		Vector3 sphereNormal = (Center - transform.parent.position).normalized;
		_normal = Vector3.Slerp (FaceNormal, sphereNormal, SphereExpansionFactor);
		this.transform.rotation = Quaternion.LookRotation (_normal, North);
		
		
		for (int i=0, n=_vertices.Length; i < n; i++) {
			_vertices[i] = this.transform.InverseTransformPoint( worldVerts[i] ); 
		}
		
	}
}
