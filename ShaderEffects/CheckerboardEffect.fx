//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- CheckerboardEffect
//
//--------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

float4 colorFilter : register(C0);

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including ImplicitInput)
//--------------------------------------------------------------------------------------

sampler2D implicitInputSampler : register(S0);


//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
   // This particular shader just multiplies the color at 
   // a point by the colorFilter shader constant.
   float4 color = tex2D(implicitInputSampler, uv);
   int xmod = (int)((uv.x) * 6.0f) % 2;
   int ymod = (int)((uv.y) * 6.0f) % 2;
   if (xmod == 1 && ymod == 1 || xmod == 0 && ymod == 0)
	 color = color * colorFilter;
   return color;
}


