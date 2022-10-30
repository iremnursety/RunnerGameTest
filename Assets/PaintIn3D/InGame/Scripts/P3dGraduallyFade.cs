using UnityEngine;

namespace PaintIn3D
{
	/// <summary>This component allows you to fade the pixels of the specified P3dPaintableTexture.</summary>
	[HelpURL(P3dHelper.HelpUrlPrefix + "P3dGraduallyFade")]
	[AddComponentMenu(P3dHelper.ComponentMenuPrefix + "Gradually Fade")]
	public class P3dGraduallyFade : MonoBehaviour
	{
		/// <summary>This is the paintable texture whose pixels we will fade.</summary>
		public P3dPaintableTexture PaintableTexture { set { paintableTexture = value; } get { return paintableTexture; } } [SerializeField] private P3dPaintableTexture paintableTexture;

		/// <summary>If you want the gradually fade effect to be masked by a texture, then specify it here.</summary>
		public Texture MaskTexture { set { maskTexture = value; } get { return maskTexture; } } [SerializeField] private Texture maskTexture;

		/// <summary>If you want the gradually fade effect to be masked by a paintable texture, then specify it here.</summary>
		public P3dPaintableTexture MaskPaintableTexture { set { maskPaintableTexture = value; } get { return maskPaintableTexture; } } [SerializeField] private P3dPaintableTexture maskPaintableTexture;

		/// <summary>This allows you to specify the channel of the mask.</summary>
		public P3dChannel MaskChannel { set { maskChannel = value; } get { return maskChannel; } } [SerializeField] private P3dChannel maskChannel;

		/// <summary>This component will paint using this blending mode.
		/// NOTE: See <b>P3dBlendMode</b> documentation for more information.</summary>
		public P3dBlendMode BlendMode { set { blendMode = value; } get { return blendMode; } } [SerializeField] private P3dBlendMode blendMode = P3dBlendMode.ReplaceOriginal(Vector4.one);

		/// <summary>The texture that will be faded toward.</summary>
		public Texture Texture { set { texture = value; } get { return texture; } } [SerializeField] private Texture texture;

		/// <summary>The color that will be faded toward.</summary>
		public Color Color { set { color = value; } get { return color; } } [SerializeField] private Color color = Color.white;

		/// <summary>Once this component has accumulated this amount of fade, it will be applied to the <b>PaintableTexture</b>. The lower this value, the smoother the fading will appear, but also the higher the performance cost.</summary>
		public float Threshold { set { threshold = value; } get { return threshold; } } [Range(0.0f, 1.0f)] [SerializeField] private float threshold = 0.02f;

		/// <summary>The speed of the fading.
		/// 1 = 1 Second.
		/// 2 = 0.5 Seconds.</summary>
		public float Speed { set { speed = value; } get { return speed; } } [SerializeField] private float speed = 1.0f;

		[SerializeField]
		private float counter;

		protected virtual void Update()
		{
			if (paintableTexture != null && paintableTexture.Activated == true)
			{
				if (speed > 0.0f)
				{
					counter += speed * Time.deltaTime;
				}

				if (counter >= threshold)
				{
					var step = Mathf.FloorToInt(counter * 255.0f);

					if (step > 0)
					{
						var change = step / 255.0f;

						counter -= change;

						P3dCommandFill.Instance.SetState(false, 0);
						P3dCommandFill.Instance.SetMaterial(blendMode, texture, color, Mathf.Min(change, 1.0f), Mathf.Min(change, 1.0f));

						var command = P3dPaintableManager.Submit(P3dCommandFill.Instance, paintableTexture.Paintable, paintableTexture);

						if (maskPaintableTexture != null)
						{
							command.LocalMaskTexture = maskPaintableTexture.Current;
							command.LocalMaskChannel = P3dHelper.IndexToVector((int)maskChannel);
						}
						else if (maskTexture != null)
						{
							command.LocalMaskTexture = maskTexture;
							command.LocalMaskChannel = P3dHelper.IndexToVector((int)maskChannel);
						}
					}
				}
			}
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dGraduallyFade;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dGraduallyFade_Editor : P3dEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			BeginError(Any(tgts, t => t.PaintableTexture == null));
				Draw("paintableTexture", "This is the paintable texture whose pixels we will count.");
			EndError();
			Draw("blendMode", "This component will paint using this blending mode.\n\nNOTE: See P3dBlendMode documentation for more information.");
			Draw("texture", "The texture that will be faded toward.");
			Draw("color", "The color that will be faded toward.");

			Separator();

			EditorGUILayout.BeginHorizontal();
				Draw("maskTexture", "If you want the gradually fade effect to be masked by a texture, then specify it here.");
				EditorGUILayout.PropertyField(serializedObject.FindProperty("maskChannel"), GUIContent.none, GUILayout.Width(50));
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
				Draw("maskPaintableTexture", "If you want the gradually fade effect to be masked by a paintable texture, then specify it here.");
				EditorGUILayout.PropertyField(serializedObject.FindProperty("maskChannel"), GUIContent.none, GUILayout.Width(50));
			EditorGUILayout.EndHorizontal();

			Separator();

			BeginError(Any(tgts, t => t.Threshold <= 0.0f));
				Draw("threshold", "Once this component has accumulated this amount of fade, it will be applied to the PaintableTexture. The lower this value, the smoother the fading will appear, but also the higher the performance cost.");
			EndError();
			BeginError(Any(tgts, t => t.Speed <= 0.0f));
				Draw("speed", "The speed of the fading.\n\n1 = 1 Second.\n\n2 = 0.5 Seconds.");
			EndError();
		}
	}
}
#endif