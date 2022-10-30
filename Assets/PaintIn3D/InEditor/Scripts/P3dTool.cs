using System.Collections.Generic;
using UnityEngine;

namespace PaintIn3D
{
	/// <summary>This component can be used to create tool prefabs for in-editor painting. These will automatically appear in the Paint tab's Tool list.</summary>
	public class P3dTool : MonoBehaviour, IBrowsable
	{
		public string Category { set { category = value; } get { return category; } } [SerializeField] private string category;

		public Texture2D Icon { set { icon = value; } get { return icon; } } [SerializeField] private Texture2D icon;

		private static List<P3dTool> cachedTools;

		private bool       cameraExpectedSet;
		private float      cameraExpectedMismatch;
		private Vector3    cameraExpectedPosition;
		private Quaternion cameraExpectedRotation = Quaternion.identity;

		public static List<P3dTool> CachedTools
		{
			get
			{
				if (cachedTools == null)
				{
					cachedTools = new List<P3dTool>();
#if UNITY_EDITOR
					var scriptGuid  = P3dHelper.FindScriptGUID<P3dTool>();

					if (scriptGuid != null)
					{
						foreach (var prefabGuid in UnityEditor.AssetDatabase.FindAssets("t:prefab"))
						{
							var tool = P3dHelper.LoadPrefabIfItContainsScriptGUID<P3dTool>(prefabGuid, scriptGuid);

							if (tool != null)
							{
								cachedTools.Add(tool);
							}
						}
					}
#endif
				}

				return cachedTools;
			}
		}

		public bool CameraMovedUnexpectedly
		{
			get
			{
				return cameraExpectedMismatch > 0.0f;
			}
		}

		public static void ClearCache()
		{
			cachedTools = null;
		}

		public string GetCategory()
		{
			return category;
		}

		public string GetTitle()
		{
			return name;
		}

		public Texture2D GetIcon()
		{
			return icon;
		}

		public Object GetObject()
		{
			return this;
		}

		protected virtual void Update()
		{
			cameraExpectedMismatch -= Time.deltaTime;
		}

#if UNITY_EDITOR
		protected virtual void OnGUI()
		{
			if (P3dWindow.Settings.OverrideCamera == true && P3dWindow.Settings.Observer != null)
			{
				var target   = P3dWindow.Settings.Observer;
				var delta    = Event.current.delta;
				var distance = P3dWindow.Settings.Distance;

				if (cameraExpectedSet == true)
				{
					if (cameraExpectedPosition != target.position || cameraExpectedRotation != target.rotation)
					{
						cameraExpectedMismatch = 1.0f;
					}
				}

				if (Event.current.type == EventType.ScrollWheel)
				{
					var newDistance = delta.y > 0.0f ? distance / 0.9f : distance * 0.9f;

					target.position += target.forward * (distance - newDistance);

					distance = newDistance;
				}
				else if (Event.current.type == EventType.MouseDown)
				{
					if (Event.current.clickCount == 2 && Event.current.button != 0 && Camera.main != null)
					{
						UnityEditor.Handles.SetCamera(Camera.main);

						var ray = UnityEditor.HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
						var hit = default(RaycastHit);

						if (Physics.Raycast(ray, out hit, float.PositiveInfinity) == true)
						{
							target.position = hit.point - target.forward * distance;
						}
					}
				}

				if (Event.current.type == EventType.MouseDrag || Event.current.type == EventType.MouseMove)
				{
					if (IsDown(P3dWindow.Settings.Rotate) == true)
					{
						var point = target.TransformPoint(0.0f, 0.0f, distance);

						target.RotateAround(point, target.up   , delta.x * 0.5f);
						target.RotateAround(point, target.right, delta.y * 0.5f);

						target.rotation = Quaternion.LookRotation(target.forward, Vector3.up);
					}

					if (IsDown(P3dWindow.Settings.Pan) == true)
					{
						target.position -= target.right * delta.x * distance * 0.001f;
						target.position += target.up    * delta.y * distance * 0.001f;
					}
				}

				if (IsDown(P3dWindow.Settings.MoveForward) == true)
				{
					target.position += target.forward * Time.deltaTime * distance * P3dWindow.Settings.MoveSpeed;
				}

				if (IsDown(P3dWindow.Settings.MoveBackward) == true)
				{
					target.position -= target.forward * Time.deltaTime * distance * P3dWindow.Settings.MoveSpeed;
				}

				if (IsDown(P3dWindow.Settings.MoveLeft) == true)
				{
					target.position -= target.right * Time.deltaTime * distance * P3dWindow.Settings.MoveSpeed;
				}

				if (IsDown(P3dWindow.Settings.MoveRight) == true)
				{
					target.position += target.right * Time.deltaTime * distance * P3dWindow.Settings.MoveSpeed;
				}

				cameraExpectedSet      = true;
				cameraExpectedPosition = target.position;
				cameraExpectedRotation = target.rotation;

				P3dWindow.Settings.Distance = distance;
			}
			else
			{
				cameraExpectedSet = false;
			}
		}

		private bool IsDown(KeyCode code)
		{
			if (code == KeyCode.Mouse0) return Event.current.button == 0;
			if (code == KeyCode.Mouse1) return Event.current.button == 1;
			if (code == KeyCode.Mouse2) return Event.current.button == 2;
			if (code == KeyCode.Mouse3) return Event.current.button == 3;
			if (code == KeyCode.Mouse4) return Event.current.button == 4;
			if (code == KeyCode.Mouse5) return Event.current.button == 5;
			if (code == KeyCode.Mouse6) return Event.current.button == 6;

			return Event.current.keyCode == code;
		}
#endif
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dTool;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dTool_Editor : P3dEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			if (P3dTool.CachedTools.Contains(tgt) == false && P3dHelper.IsAsset(tgt) == true)
			{
				P3dTool.CachedTools.Add(tgt);
			}

			Draw("category");
			Draw("icon");
		}

		[MenuItem("Assets/Create/Paint in 3D/Tool")]
		private static void CreateAsset()
		{
			var tool  = new GameObject("Tool").AddComponent<P3dTool>();
			var guids = Selection.assetGUIDs;
			var path  = guids.Length > 0 ? AssetDatabase.GUIDToAssetPath(guids[0]) : null;

			if (string.IsNullOrEmpty(path) == true)
			{
				path = "Assets";
			}
			else if (AssetDatabase.IsValidFolder(path) == false)
			{
				path = System.IO.Path.GetDirectoryName(path);
			}

			var assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/NewTool.prefab");

			var asset = PrefabUtility.SaveAsPrefabAsset(tool.gameObject, assetPathAndName);

			DestroyImmediate(tool.gameObject);

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			EditorUtility.FocusProjectWindow();

			Selection.activeObject = asset; EditorGUIUtility.PingObject(asset);
		}
	}
}
#endif