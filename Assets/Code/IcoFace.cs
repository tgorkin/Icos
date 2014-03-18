using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
public class IcoFace : MonoBehaviour {

	public Vector3 A;
	public Vector3 B;
	public Vector3 C;

	public int NumSubdivisions = 5; // max 300

	public float SphereExpansionFactor = 1f;

	Vector3[] _vertices;
	int[] _indices;
	Vector3[] _normals;
	Vector2[] _uvs;

	private Hex[][] _tiles;

	void Start() {
		RefreshFace ();
	}

	public void RefreshFace() {
		Subdivide ();
		CreateMesh ();
	}

	private void CreateMesh() {
		MeshFilter meshComp = GetComponent<MeshFilter> ();
		Mesh mesh = new Mesh ();
		meshComp.mesh = mesh;
		mesh.vertices = _vertices;
		mesh.triangles = _indices;
		mesh.normals = _normals;
		mesh.uv = _uvs;
	}

	private void Subdivide() {
	
		int numTris = (int)Math.Pow (NumSubdivisions + 1, 2); // 2^2 = 4
		int numVerts = SeriesSum( NumSubdivisions+2 ); 	// 1+2+3 = 6
		_vertices = new Vector3[numVerts];			
		_normals = new Vector3[numVerts];
		_uvs = new Vector2[numVerts];
		_indices = new int[3*numTris];

		Vector3 edge0 = B - A;
		Vector3 edge1 = C - A;
		Vector3 edge2 = C - B;

		_vertices [0] = A;
		int pointsPerSubdivision = 0;
		int vertCounter = 1;
		for (int subdivision=0; subdivision < NumSubdivisions+1; subdivision++) {
			float edgeDistance = ((float)subdivision+1)/(NumSubdivisions+1);
			Vector3 leftVert = A + (edge0 * edgeDistance);
			Vector3 rightVert = A + (edge1 * edgeDistance);
			_vertices[vertCounter++] = leftVert;
			Vector3 horizontal = rightVert - leftVert;
			Vector3 horDelta = (pointsPerSubdivision > 0) ? horizontal * 1/(float)(pointsPerSubdivision+1) : horizontal;
			for(int j=0; j < pointsPerSubdivision; j++) {
				_vertices[vertCounter++] = leftVert + ((j+1) * horDelta);
			}
			_vertices[vertCounter++] = rightVert;
			pointsPerSubdivision++;
		}

		float size = A.magnitude;
		for (int i=0; i < numVerts; i++) {
			Vector3 vert = _vertices[i];
			_vertices[i] = Vector3.Slerp(vert, vert.normalized * size, SphereExpansionFactor );
			_normals[i] = vert.normalized;
		}

		int indexCounter = 0;
		int numTrisPerSubdivision = 0;
		for (int subdivision=0; subdivision < NumSubdivisions+1; subdivision++) {
			numTrisPerSubdivision = (2* subdivision) +1;
			int topLeftIndex = SeriesSum( subdivision );
			int bottomLeftIndex = SeriesSum( subdivision +1 );
			bool up = true;
			for(int i=0; i < numTrisPerSubdivision; i++) {
				if( up ) {
					_indices[indexCounter++] =  topLeftIndex;
					_indices[indexCounter++] =  bottomLeftIndex +1;
					_indices[indexCounter++] =  bottomLeftIndex;
				} else {
					_indices[indexCounter++] =  topLeftIndex;
					_indices[indexCounter++] =  topLeftIndex + 1;
					_indices[indexCounter++] =  bottomLeftIndex +1;
					topLeftIndex++;
					bottomLeftIndex++;
				}
				up = !up;
			}
		}

		int uvCounter = 0;
		for (int i=0; i < numVerts; i++) {

			int uvMultiple = i % 4;
			switch(uvMultiple) {
			case 0:
				_uvs[uvCounter++] = new Vector2 (0, 0);
				break;
			case 1:
				_uvs[uvCounter++] = new Vector2 (1f, 0);
				break;
			case 2:
				_uvs[uvCounter++] = new Vector2 (1f, 1f);
				break;
			case 3:
				_uvs[uvCounter++] = new Vector2 (0, 1f);
				break;
			default:
				break;
			}
		}
	}

	private int SeriesSum(int n) {
		return n * (n + 1) / 2;
	}
}
