using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;

namespace Zenith
{
	public class MiscShaderData14 : ShaderData
	{
		private static FieldInfo vpinfo = typeof(SpriteViewMatrix).GetField("_viewport", BindingFlags.NonPublic | BindingFlags.Instance);
		private static Matrix GetNormalizedTransformationmatrix(SpriteViewMatrix matrix)
		{
			var viewport = (Viewport) vpinfo.GetValue(matrix);
			Vector2 vector = new Vector2((float)viewport.Width, (float)viewport.Height);
			Matrix matrix2 = Matrix.CreateOrthographicOffCenter(0f, vector.X, vector.Y, 0f, 0f, 1f);
			return Matrix.Invert(matrix.EffectMatrix) * matrix.ZoomMatrix * matrix2;
		}

	// Token: 0x06001A9A RID: 6810 RVA: 0x004AAFFC File Offset: 0x004A91FC
	public MiscShaderData14(Ref<Effect> shader, string passName) : base(shader, passName)
		{
		}

		// Token: 0x06001A9B RID: 6811 RVA: 0x004AB048 File Offset: 0x004A9248
		public virtual void Apply(DrawData? drawData = null)
		{
			base.Shader.Parameters["uColor"].SetValue(this._uColor);
			base.Shader.Parameters["uSaturation"].SetValue(this._uSaturation);
			base.Shader.Parameters["uSecondaryColor"].SetValue(this._uSecondaryColor);
			base.Shader.Parameters["uTime"].SetValue((float)(Main._drawInterfaceGameTime.TotalGameTime.TotalSeconds % 3600.0));
			base.Shader.Parameters["uOpacity"].SetValue(this._uOpacity);
			base.Shader.Parameters["uShaderSpecificData"].SetValue(this._shaderSpecificData);
			if (drawData != null)
			{
				DrawData value = drawData.Value;
				Vector4 zero = Vector4.Zero;
				if (drawData.Value.sourceRect != null)
				{
					zero = new Vector4((float)value.sourceRect.Value.X, (float)value.sourceRect.Value.Y, (float)value.sourceRect.Value.Width, (float)value.sourceRect.Value.Height);
				}
				base.Shader.Parameters["uSourceRect"].SetValue(zero);
				base.Shader.Parameters["uWorldPosition"].SetValue(Main.screenPosition + value.position);
				base.Shader.Parameters["uImageSize0"].SetValue(new Vector2((float)value.texture.Width, (float)value.texture.Height));
			}
			else
			{
				base.Shader.Parameters["uSourceRect"].SetValue(new Vector4(0f, 0f, 4f, 4f));
			}
			if (this._uImage0 != null)
			{
				Main.graphics.GraphicsDevice.Textures[0] = this._uImage0;
				Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
				base.Shader.Parameters["uImageSize0"].SetValue(new Vector2((float)this._uImage0.Width, (float)this._uImage0.Height));
			}
			if (this._uImage1 != null)
			{
				Main.graphics.GraphicsDevice.Textures[1] = this._uImage1;
				Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.LinearWrap;
				base.Shader.Parameters["uImageSize1"].SetValue(new Vector2((float)this._uImage1.Width, (float)this._uImage1.Height));
			}
			if (this._uImage2 != null)
			{
				Main.graphics.GraphicsDevice.Textures[2] = this._uImage2;
				Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.LinearWrap;
				base.Shader.Parameters["uImageSize2"].SetValue(new Vector2((float)this._uImage2.Width, (float)this._uImage2.Height));
			}
			if (this._useProjectionMatrix)
			{
				base.Shader.Parameters["uMatrixTransform0"].SetValue(GetNormalizedTransformationmatrix(Main.GameViewMatrix));
			}
			base.Apply();
		}

		// Token: 0x06001A9C RID: 6812 RVA: 0x004AB3E4 File Offset: 0x004A95E4
		public MiscShaderData14 UseColor(float r, float g, float b)
		{
			return this.UseColor(new Vector3(r, g, b));
		}

		// Token: 0x06001A9D RID: 6813 RVA: 0x004AB3F4 File Offset: 0x004A95F4
		public MiscShaderData14 UseColor(Color color)
		{
			return this.UseColor(color.ToVector3());
		}

		// Token: 0x06001A9E RID: 6814 RVA: 0x004AB404 File Offset: 0x004A9604
		public MiscShaderData14 UseColor(Vector3 color)
		{
			this._uColor = color;
			return this;
		}

		// Token: 0x06001A9F RID: 6815 RVA: 0x004AB410 File Offset: 0x004A9610
		public MiscShaderData14 UseImage0(Texture2D texture)
		{
			this._uImage0 = texture;
			return this;
		}

		// Token: 0x06001AA0 RID: 6816 RVA: 0x004AB428 File Offset: 0x004A9628
		public MiscShaderData14 UseImage1(Texture2D texture)
		{
			this._uImage1 = texture;
			return this;
		}

		// Token: 0x06001AA1 RID: 6817 RVA: 0x004AB440 File Offset: 0x004A9640
		public MiscShaderData14 UseImage2(Texture2D texture)
		{
			this._uImage2 = texture;
			return this;
		}

		// Token: 0x06001AA2 RID: 6818 RVA: 0x004AB458 File Offset: 0x004A9658
		public MiscShaderData14 UseOpacity(float alpha)
		{
			this._uOpacity = alpha;
			return this;
		}

		// Token: 0x06001AA3 RID: 6819 RVA: 0x004AB464 File Offset: 0x004A9664
		public MiscShaderData14 UseSecondaryColor(float r, float g, float b)
		{
			return this.UseSecondaryColor(new Vector3(r, g, b));
		}

		// Token: 0x06001AA4 RID: 6820 RVA: 0x004AB474 File Offset: 0x004A9674
		public MiscShaderData14 UseSecondaryColor(Color color)
		{
			return this.UseSecondaryColor(color.ToVector3());
		}

		// Token: 0x06001AA5 RID: 6821 RVA: 0x004AB484 File Offset: 0x004A9684
		public MiscShaderData14 UseSecondaryColor(Vector3 color)
		{
			this._uSecondaryColor = color;
			return this;
		}

		// Token: 0x06001AA6 RID: 6822 RVA: 0x004AB490 File Offset: 0x004A9690
		public MiscShaderData14 UseProjectionMatrix(bool doUse)
		{
			this._useProjectionMatrix = doUse;
			return this;
		}

		// Token: 0x06001AA7 RID: 6823 RVA: 0x004AB49C File Offset: 0x004A969C
		public MiscShaderData14 UseSaturation(float saturation)
		{
			this._uSaturation = saturation;
			return this;
		}

		// Token: 0x06001AA8 RID: 6824 RVA: 0x004AB4A8 File Offset: 0x004A96A8
		public virtual MiscShaderData14 GetSecondaryShader(Entity entity)
		{
			return this;
		}

		// Token: 0x06001AA9 RID: 6825 RVA: 0x004AB4AC File Offset: 0x004A96AC
		public MiscShaderData14 UseShaderSpecificData(Vector4 specificData)
		{
			this._shaderSpecificData = specificData;
			return this;
		}

		// Token: 0x040044BE RID: 17598
		public Vector3 _uColor = Vector3.One;

		// Token: 0x040044BF RID: 17599
		public Vector3 _uSecondaryColor = Vector3.One;

		// Token: 0x040044C0 RID: 17600
		public float _uSaturation = 1f;

		// Token: 0x040044C1 RID: 17601
		public float _uOpacity = 1f;

		// Token: 0x040044C2 RID: 17602
		public Texture2D _uImage0;

		// Token: 0x040044C3 RID: 17603
		public Texture2D _uImage1;

		// Token: 0x040044C4 RID: 17604
		public Texture2D _uImage2;

		// Token: 0x040044C5 RID: 17605
		public bool _useProjectionMatrix;

		// Token: 0x040044C6 RID: 17606
		public Vector4 _shaderSpecificData = Vector4.Zero;
	}
}
