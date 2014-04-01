using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
public class IcoFace : MonoBehaviour {

	public Vector3 A;
	public Vector3 B;
	public Vector3 C;

	public GameObject HexChunk;

	public int NumSubdivisions; // max 300

	private float _sphereRadius;
	public float SphereExpansionFactor = 1f;

	Vector3[] _vertices;
	int[] _indices;
	Vector3[] _normals;
	Vector2[] _uvs;

	float _hexSize;
	Vector3 _faceNormal;
	Vector2 _faceUp;

	private List<Hex> _hexes = new List<Hex>();

	void Start() {
		RefreshFace ();
	}

	public void RefreshFace() {
		ClearMesh ();
		SubdivideHexes ();
		//CreateMesh ();
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

	private void ClearMesh() {
		for (int i=0, n=transform.childCount; i < n; i++) {
			GameObject.Destroy(transform.GetChild(i).gameObject);
		}
	}

	private void SubdivideHexes() {
		/*
		 * int numHexes = ((int)Math.Pow (NumSubdivisions + 1, 2) - 3) / 6;
		int numVerts = numHexes * 7;
		_vertices = new Vector3[numVerts];		
		_normals = new Vector3[numVerts];
		_uvs = new Vector2[numVerts];
		_indices = new int[3*6*numHexes];
		*/

		_sphereRadius = A.magnitude;

		Vector3 edge0 = B - A;
		Vector3 edge1 = C - A;
		Vector3 edge2 = C - B;

		Vector3 vertical = edge0 + (0.5f * edge2);
		vertical = vertical * 1f / (float)(NumSubdivisions+1);
		Vector3 horizontal = edge2 * 1f / (float)(NumSubdivisions+1);

		_hexSize = edge0.magnitude / (NumSubdivisions + 1);

		_faceNormal = Vector3.Cross (edge1, edge0).normalized;

		Vector3 vertAxis = edge0 * 1f / (float)(NumSubdivisions+1);
		Vector3 horAxis = edge2 * 1f / (float)(NumSubdivisions+1);
		for (int vertIter=2; vertIter < NumSubdivisions+1; vertIter++) {
			int hexCenterIter = (3 - (vertIter%3))%3;
			for(int horIter=1; horIter < vertIter; horIter++) {
				if( horIter % 3 == hexCenterIter && !(vertIter == NumSubdivisions+1 && horIter == NumSubdivisions+1)) {
					CreateHex( A + vertIter * vertAxis + horIter *horAxis, vertical);
				}
			}
		}
	}

	private void CreateHex(Vector3 center, Vector3 up) {
		GameObject hexGO = GameObject.Instantiate( HexChunk ) as GameObject;
		HexChunk hexChunk = hexGO.GetComponent<HexChunk> ();
		hexChunk.Size = _hexSize;
		hexChunk.Center = center;
		hexChunk.North = up;
		hexChunk.FaceNormal = _faceNormal;
		hexChunk.SphereRadius = _sphereRadius;
		hexChunk.SphereExpansionFactor = SphereExpansionFactor;
		hexChunk.transform.parent = this.transform;
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

	void OnDrawGizmos() {
		// Draw a yellow sphere at the transform's position
		Gizmos.color = Color.red;
		Gizmos.DrawLine (A, B);
		Gizmos.color = Color.green;
		Gizmos.DrawLine (A, C);
		Gizmos.color = Color.blue;
		Gizmos.DrawLine (B, C);
		Gizmos.color = Color.magenta;
		Gizmos.DrawLine (B + ((C - B) / 2), A);
		//Gizmos.color = Color.grey;
		//Gizmos.DrawCube (A, new Vector3 (1f, 1f, 1f));
	}
}
