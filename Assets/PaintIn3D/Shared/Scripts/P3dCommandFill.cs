using System.Collections.Generic;
using UnityEngine;

namespace PaintIn3D
{
	/// <summary>This class manages the fill painting command.</summary>
	public class P3dCommandFill : P3dCommand
	{
		public P3dBlendMode Blend;
		public Texture      Texture;
		public Color        Color;
		public float        Opacity;
		public float        Minimum;

		public static P3dCommandFill Instance = new P3dCommandFill();

		private static Stack<P3dCommandFill> pool = new Stack<P3dCommandFill>();

		private static Material cachedMaterial;

		public override bool RequireMesh { get { return false; } }

		static P3dCommandFill()
		{
			cachedMaterial = P3dShader.BuildMaterial("Hidden/Paint in 3D/Fill");
		}

		public static RenderTexture Blit(RenderTexture main, P3dBlendMode blendMode, Texture texture, Color color, float opacity, float minimum)
		{
			var swap = P3dHelper.GetRenderTexture(main.descriptor, main);

			Blit(ref main, ref swap, blendMode, texture, color, opacity, minimum);

			P3dHelper.ReleaseRenderTexture(swap);

			return main;
		}

		public static void Blit(ref RenderTexture main, ref RenderTexture swap, P3dBlendMode blendMode, Texture texture, Color color, float opacity, float minimum)
		{
			Instance.SetMaterial(blendMode, texture, color, opacity, minimum);

			Instance.Apply();

			if (Instance.Double == true)
			{
				P3dCommandReplace.Blit(swap, main, Color.white);

				Instance.Material.SetTexture(P3dShader._Buffer, swap);
				Instance.Material.SetVector(P3dShader._BufferSize, new Vector2(swap.width, swap.height));
			}

			P3dHelper.Blit(main, Instance.Material, blendMode);
		}

		public override void Apply()
		{
			base.Apply();

			Blend.Apply(Material);

			Material.SetTexture(P3dShader._Texture, Texture);
			Material.SetColor(P3dShader._Color, P3dHelper.FromGamma(Color));
			Material.SetFloat(P3dShader._Opacity, Opacity);
			Material.SetVector(P3dShader._Minimum, new Vector4(Minimum, Minimum, Minimum, Minimum));
		}

		public override void Pool()
		{
			pool.Push(this);
		}

		public override void Transform(Matrix4x4 posMatrix, Matrix4x4 rotMatrix)
		{
		}

		public override P3dCommand SpawnCopy()
		{
			var command = SpawnCopy(pool);

			command.Blend   = Blend;
			command.Texture = Texture;
			command.Color   = Color;
			command.Opacity = Opacity;
			command.Minimum = Minimum;

			return command;
		}

		public override void Apply(P3dPaintableTexture paintableTexture)
		{
			base.Apply(paintableTexture);

			if (Blend.Index == P3dBlendMode.REPLACE_ORIGINAL)
			{
				Blend.Color   = paintableTexture.Color;
				Blend.Texture = paintableTexture.Texture;
			}
		}

		public void SetMaterial(P3dBlendMode blendMode, Texture texture, Color color, float opacity, float minimum)
		{
			Blend    = blendMode;
			Material = cachedMaterial;
			Pass     = blendMode;
			Texture  = texture;
			Color    = color;
			Opacity  = opacity;
			Minimum  = minimum;
		}
	}
}