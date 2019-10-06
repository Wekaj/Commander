#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float2 Dimensions;
float2 Center0, Center1, Center2, Center3,
	Center4, Center5, Center6, Center7; // Not sure if the array bug is still present in monogame.
float ClearRadiusSqr, ClearBorderRadiusSqr;
float4 FogColor, BorderColor;

Texture2D SpriteTexture;
sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct PixelShaderInput
{
	float2 Position : TEXCOORD0;
	float4 Color : COLOR0;
};

float Dist(float2 p1, float2 p2) 
{
	float2 difference = p1 - p2;

	return difference.x * difference.x + difference.y * difference.y;
}

float4 MainPS(PixelShaderInput input) : COLOR
{
	float2 pixPos = input.Position * Dimensions;

	float minDistSqr = Dist(pixPos, Center0);
	minDistSqr = min(minDistSqr, Dist(pixPos, Center1));
	minDistSqr = min(minDistSqr, Dist(pixPos, Center2));
	minDistSqr = min(minDistSqr, Dist(pixPos, Center3));
	minDistSqr = min(minDistSqr, Dist(pixPos, Center4));
	minDistSqr = min(minDistSqr, Dist(pixPos, Center5));
	minDistSqr = min(minDistSqr, Dist(pixPos, Center6));
	minDistSqr = min(minDistSqr, Dist(pixPos, Center7));

	if (minDistSqr > ClearRadiusSqr) {
		return FogColor;
	}

	if (minDistSqr > ClearBorderRadiusSqr) {
		return tex2D(SpriteTextureSampler, input.Position) * 0.5;
	}

	return tex2D(SpriteTextureSampler, input.Position);
}

technique BasicColorDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};