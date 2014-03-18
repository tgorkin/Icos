using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Icosahedron))]
public class IcoEditor : Editor {

	public override void OnInspectorGUI () {
		bool dirty = false;

		Icosahedron ico = target as Icosahedron;
		int currentSubdivisions = ico.NumSubdivisions;
		ico.NumSubdivisions = EditorGUILayout.IntSlider ("Subdivisions", ico.NumSubdivisions, 0, 20); 
		if (ico.NumSubdivisions != currentSubdivisions) {
			dirty = true;
		}
		
		float sphereFactor = ico.SphereExpansionFactor;
		ico.SphereExpansionFactor = EditorGUILayout.Slider ("Sphere Factor", ico.SphereExpansionFactor, 0, 1f);
		if (ico.SphereExpansionFactor != sphereFactor) {
			dirty = true;
		}
		if (dirty) {
			ico.RefreshFaces();
			dirty = false;
		}
	}
}
