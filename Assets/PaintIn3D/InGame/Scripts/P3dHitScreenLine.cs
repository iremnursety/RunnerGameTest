using UnityEngine;
using System.Collections.Generic;

namespace PaintIn3D
{
	/// <summary>This component will perform a raycast under the mouse or finger as it moves across the screen. It will then send hit events to components like <b>P3dPaintDecal</b>, allowing you to paint the scene.</summary>
	[HelpURL(P3dHelper.HelpUrlPrefix + "P3dHitScreenLine")]
	[AddComponentMenu(P3dHelper.ComponentHitMenuPrefix + "Hit Screen Line")]
	public class P3dHitScreenLine : P3dHitScreenBase
	{
		public enum FrequencyType
		{
			StartAndEnd,
			PixelInterval,
			ScaledPixelInterval,
			StretchedPixelInterval,
			StretchedScaledPixelInterval,
			Start,
			End
		}

		/// <summary>This allows you to control how many hit points will be generated along the drawn line.
		/// StartAndEnd = Once at the start, and once at the end.
		/// PixelInterval = Once at the start, and then every <b>Interval</b> pixels.
		/// ScaledPixelInterval = Once at the start, and then every <b>Interval</b> scaled pixels.
		/// StretchedPixelInterval = Like <b>ScaledPixelInterval</b>, but the hits are stretched to reach the end.
		/// StretchedScaledPixelInterval = Like <b>ScaledPixelInterval</b>, but the hits are stretched to reach the end.</summary>
		public FrequencyType Frequency { set { frequency = value; } get { return frequency; } } [SerializeField] private FrequencyType frequency = FrequencyType.PixelInterval;

		/// <summary>This allows you to set the pixels between each hit point based on the current <b>Frequency</b> setting.</summary>
		public float Interval { set { interval = value; } get { return interval; } } [SerializeField] private float interval = 10.0f;

		/// <summary>This allows you to connect the hit points together to form lines.</summary>
		public P3dPointConnector Connector { get { if (connector == null) connector = new P3dPointConnector(); return connector; } } [SerializeField] private P3dPointConnector connector;

		protected override void OnEnable()
		{
			base.OnEnable();

			Connector.ResetConnections();
		}

		protected virtual void Update()
		{
			connector.Update();
		}

		protected void LateUpdate()
		{
			for (var i = fingers.Count - 1; i >= 0; i--)
			{
				var finger = fingers[i];

				if (finger.Index == P3dInputManager.HOVER_FINGER_INDEX)
				{
					continue;
				}

				if (finger.Up == true)
				{
					if (storeStates == true)
					{
						P3dStateManager.StoreAllStates();
					}
				}

				switch (frequency)
				{
					case FrequencyType.StartAndEnd: PaintStartEnd(finger); break;
					case FrequencyType.PixelInterval: PaintStartInterval(finger, interval, false); break;
					case FrequencyType.ScaledPixelInterval: PaintStartInterval(finger, interval / P3dInputManager.ScaleFactor, false); break;
					case FrequencyType.StretchedPixelInterval: PaintStartInterval(finger, interval, true); break;
					case FrequencyType.StretchedScaledPixelInterval: PaintStartInterval(finger, interval / P3dInputManager.ScaleFactor, true); break;
					case FrequencyType.Start: PaintOne(finger, 0.0f); break;
					case FrequencyType.End: PaintOne(finger, 1.0f); break;
				}

				connector.BreakHits(finger);

				if (finger.Up == true)
				{
					fingers.Remove(finger);
				}
			}
		}

		private void PaintStartEnd(P3dInputManager.Finger finger)
		{
			var preview = finger.Up == false;
			var pointS  = finger.StartScreenPosition;
			var pointE  = finger.ScreenPosition;
			var pointV  = pointE - pointS;

			PaintAt(connector, connector.HitCache, pointS, pointS - pointV, preview, finger.Pressure, finger);
			PaintAt(connector, connector.HitCache, pointE, pointE - pointV, preview, finger.Pressure, finger);
		}

		private void PaintStartInterval(P3dInputManager.Finger finger, float pixelSpacing, bool stretch)
		{
			var preview = finger.Up == false;
			var pointS  = finger.StartScreenPosition;
			var pointE  = finger.ScreenPosition;
			var pointV  = pointE - pointS;
			var pointM  = pointV.magnitude;
			var steps   = 0;

			if (pixelSpacing > 0.0f)
			{
				steps = Mathf.FloorToInt(pointM / pixelSpacing);

				if (stretch == true && steps > 0)
				{
					pixelSpacing = pointM / steps;
				}
			}

			for (var i = 0; i <= steps; i++)
			{
				PaintAt(connector, connector.HitCache, pointS, pointS - pointV, preview, finger.Pressure, finger);

				pointS = Vector2.MoveTowards(pointS, pointE, pixelSpacing);
			}
		}

		private void PaintOne(P3dInputManager.Finger finger, float frac)
		{
			var preview = finger.Up == false;
			var pointS  = finger.StartScreenPosition;
			var pointE  = finger.ScreenPosition;
			var pointV  = pointE - pointS;

			pointS += pointV * frac;

			PaintAt(connector, connector.HitCache, pointS, pointS - pointV, preview, finger.Pressure, finger);
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dHitScreenLine;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dHitScreenLine_Editor : P3dHitScreenBase_Editor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			base.OnInspector();

			Separator();

			DrawBasic();

			Separator();

			DrawAdvancedFoldout();

			Separator();

			var point    = tgt.Emit == P3dHitScreenBase.EmitType.PointsIn3D;
			var line     = tgt.Emit == P3dHitScreenBase.EmitType.PointsIn3D && tgt.Connector.ConnectHits == true;
			var triangle = tgt.Emit == P3dHitScreenBase.EmitType.TrianglesIn3D;
			var coord    = tgt.Emit == P3dHitScreenBase.EmitType.PointsOnUV;

			tgt.Connector.HitCache.Inspector(tgt.gameObject, point: point, line: line, triangle: triangle, coord: coord);
		}

		protected override void DrawBasic()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			base.DrawBasic();

			Draw("frequency", "This allows you to control how many hit points will be generated along the drawn line.\n\nStartAndEnd = Once at the start, and once at the end.\n\nPixelInterval = Once at the start, and then every Interval pixels.\n\nScaledPixelInterval = Once at the start, and then every Interval scaled pixels.\n\nStretchedPixelInterval = Like PixelInterval, but the hits are stretched to reach the end.\n\nStretchedScaledPixelInterval = Like ScaledPixelInterval, but the hits are stretched to reach the end.");
			if (Any(tgts, t => t.Frequency == P3dHitScreenLine.FrequencyType.PixelInterval || t.Frequency == P3dHitScreenLine.FrequencyType.ScaledPixelInterval || t.Frequency == P3dHitScreenLine.FrequencyType.StretchedPixelInterval || t.Frequency == P3dHitScreenLine.FrequencyType.StretchedScaledPixelInterval))
			{
				BeginIndent();
					BeginError(Any(tgts, t => t.Interval <= 0.0f));
						Draw("interval", "This allows you to set the pixels/seconds between each hit point based on the current Frequency setting.");
					EndError();
				EndIndent();
			}
		}

		protected override void DrawAdvanced()
		{
			base.DrawAdvanced();

			Separator();

			P3dPointConnector_Editor.Draw(serializedObject);
		}
	}
}
#endif