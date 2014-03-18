using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
public class HexChunk : MonoBehaviour {

	public int rSize = 10;
	public int qSize = 10;

	// Use this for initialization
	void Start () {
		int totalNumVerts = rSize * qSize * HexGlobals.NUM_VERTS_PER_HEX;
		int totalIndices = rSize * qSize * HexGlobals.NUM_INDICES_PER_HEX;

		Vector3[] vertices = new Vector3[ totalNumVerts ];
		int[] indices = new int[ totalIndices ];
		Vector3[] normals = new Vector3[ totalNumVerts ];
		Vector2[] uvs = new Vector2[ totalNumVerts ];

		for( int q=0; q < qSize; q++) {
			for (int r=0; r < rSize; r++) {
				CreateHexMesh( r, q, ref vertices, ref indices, ref normals, ref uvs );
			}
		}

		MeshFilter meshComp = GetComponent<MeshFilter> ();
		Mesh mesh = new Mesh ();
		meshComp.mesh = mesh;
		mesh.vertices = vertices;
		mesh.triangles = indices;
		mesh.normals = normals;
		mesh.uv = uvs;
	}

	private void CreateHexMesh(int r, int q, ref Vector3[] vertices, ref int[] indices, ref Vector3[] normals, ref Vector2[] uvs ) {
		int axialOffset = (q * rSize + r);
		int vertOffset = axialOffset * HexGlobals.NUM_VERTS_PER_HEX;
		int indexOffset = axialOffset * HexGlobals.NUM_INDICES_PER_HEX;

		float x, y, angle;
		float centerX = r * HexGlobals.HorzDistance + q * HexGlobals.Width/2f;
		float centerY = q * HexGlobals.VertDistance;
		vertices[vertOffset] = new Vector3(centerX, centerY, 0);
		for(int i=0; i < 6; i++) {
			angle = 2f * Mathf.PI / 6f * (i + 0.5f);
			x = centerX + HexGlobals.Size * Mathf.Cos(angle);
			y = centerY + HexGlobals.Size * Mathf.Sin(angle);
			vertices[vertOffset+i+1] = new Vector3(x, y, 0);
		}


		int tempIndexOffset = indexOffset;
		for(int i=0; i < 6; i++) {
			indices[tempIndexOffset++] = vertOffset;
			indices[tempIndexOffset++] = ((i+1) % 6) + 1 + vertOffset;
			indices[tempIndexOffset++] = (i % 6) + 1 + vertOffset;
		}

		for (int i=0; i < 7; i++) {
			normals[vertOffset+i] = Vector3.up;
		}

		uvs [vertOffset] = new Vector2 (0, 0);
		for (int i=1; i < 7; i++) {
			uvs[vertOffset+i] = new Vector2(1f, 0);
		}
	}
}
