﻿using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(IcoFace))]
public class IcoFaceEditor : Editor {

	public override void OnInspectorGUI () {
		bool dirty = false;

		IcoFace face = target as IcoFace;

		face.HexTile = EditorGUILayout.ObjectField (face.HexTile, typeof(GameObject), false, null) as GameObject;
		face.TrapezoidTile = EditorGUILayout.ObjectField (face.TrapezoidTile, typeof(GameObject), false, null) as GameObject;

		int currentSubdivisions = face.NumSubdivisions;
		face.NumSubdivisions = EditorGUILayout.IntSlider ("Subdivisions", face.NumSubdivisions, 0, 20); 
		if (face.NumSubdivisions != currentSubdivisions) {
			dirty = true;
		}

		float sphereFactor = face.SphereExpansionFactor;
		face.SphereExpansionFactor = EditorGUILayout.Slider ("Sphere Factor", face.SphereExpansionFactor, 0, 1f);
		if (face.SphereExpansionFactor != sphereFactor) {
			dirty = true;
		}

		if (dirty) {
			face.RefreshFace();
			dirty = false;
		}
	}
}
