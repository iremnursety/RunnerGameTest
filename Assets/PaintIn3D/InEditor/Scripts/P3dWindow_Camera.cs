#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PaintIn3D
{
	public partial class P3dWindow
	{
		private Vector2 cameraScrollPosition;

		private void DrawCameraTab()
		{
			cameraScrollPosition = GUILayout.BeginScrollView(cameraScrollPosition, GUILayout.ExpandHeight(true));
				EditorGUILayout.HelpBox("This allows you to control the Game view camera. Keep in mind this will not work if your camera already has controls from another source.", MessageType.Info);

				EditorGUILayout.Separator();

				P3dEditor.BeginLabelWidth(100);
					EditorGUILayout.LabelField("Controls", EditorStyles.boldLabel);
					Settings.MoveSpeed    = EditorGUILayout.FloatField("Speed", Settings.MoveSpeed);
					Settings.MoveForward  = (KeyCode)EditorGUILayout.EnumPopup("Forward", Settings.MoveForward);
					Settings.MoveBackward = (KeyCode)EditorGUILayout.EnumPopup("Backward", Settings.MoveBackward);
					Settings.MoveLeft     = (KeyCode)EditorGUILayout.EnumPopup("Left", Settings.MoveLeft);
					Settings.MoveRight    = (KeyCode)EditorGUILayout.EnumPopup("Right", Settings.MoveRight);
					Settings.Rotate       = (KeyCode)EditorGUILayout.EnumPopup("Rotate", Settings.Rotate);
					Settings.Pan          = (KeyCode)EditorGUILayout.EnumPopup("Pan", Settings.Pan);
					EditorGUI.BeginDisabledGroup(true);
						EditorGUILayout.TextField("Paint", "Mouse 0", EditorStyles.popup);
						EditorGUILayout.TextField("Zoom", "Mouse Wheel", EditorStyles.popup);
						EditorGUILayout.TextField("Move Pivot", "Double Click (Right / Middle)", EditorStyles.popup);
					EditorGUI.EndDisabledGroup();

					EditorGUILayout.Separator();

					Settings.OverrideCamera = EditorGUILayout.Toggle("Override Camera", Settings.OverrideCamera);

					if (Settings.OverrideCamera == true)
					{
						EditorGUI.indentLevel++;
							Settings.Distance  = LogSlider("Distance", Settings.Distance, -4, 4);
							Settings.Observer  = (Transform)EditorGUILayout.ObjectField("Root", Settings.Observer, typeof(Transform), true);
							Settings.ShowPivot = EditorGUILayout.Toggle("Show Pivot", Settings.ShowPivot);

							if (GUI.Button(EditorGUI.IndentedRect(P3dHelper.Reserve()), "Snap To Scene View", EditorStyles.miniButton) == true)
							{
								var camA = Camera.main;

								if (camA != null && SceneView.lastActiveSceneView != null && SceneView.lastActiveSceneView.camera != null)
								{
									var camB = SceneView.lastActiveSceneView.camera;

									camA.transform.position = camB.transform.position;
									camA.transform.rotation = camB.transform.rotation;
								}
							}
						EditorGUI.indentLevel--;

						if (toolInstance != null && toolInstance.CameraMovedUnexpectedly == true)
						{
							EditorGUILayout.HelpBox("The camera moved unexpectedly. Mabe your scene already has camera controls?", MessageType.Warning);
						}
					}
				P3dEditor.EndLabelWidth();
			GUILayout.EndScrollView();

			GUILayout.FlexibleSpace();

			if (Application.isPlaying == false)
			{
				EditorGUILayout.HelpBox("You must enter play mode to move the Game camera.", MessageType.Warning);
			}
		}
	}
}
#endif