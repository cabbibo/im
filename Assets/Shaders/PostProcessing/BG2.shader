Shader "BG/2"
{
	Properties
	{
    	_textureY ("TextureY", 2D) = "white" {}
      _textureCbCr ("TextureCbCr", 2D) = "black" {}
        _HueSize ("HueSize", float ) = 0.3
        _HueStart ("HueStart", float ) = 0.03
        _Saturation ("Saturation", float ) = 0.03
        _BlendVal("BlendVal", float ) = 0
	}
	SubShader
	{
		Cull Off
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
            ZWrite Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#include "../Chunks/hsv.cginc"

			float4x4 _DisplayTransform;
			float2 _ScreenSize;
      float _HueSize;
      float _HueStart;
      float _Saturation;
      float _BlendVal;


			struct Vertex
			{
				float4 position : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct TexCoordInOut
			{
				float4 position : SV_POSITION;
				float2 texcoord : TEXCOORD0;
			};

			TexCoordInOut vert (Vertex vertex)
			{
				TexCoordInOut o;
				o.position = UnityObjectToClipPos(vertex.position); 

				float texX = vertex.texcoord.x;
				float texY = vertex.texcoord.y;
				
				o.texcoord.x = (_DisplayTransform[0].x * texX + _DisplayTransform[1].x * (texY) + _DisplayTransform[2].x);
 			 	o.texcoord.y = (_DisplayTransform[0].y * texX + _DisplayTransform[1].y * (texY) + (_DisplayTransform[2].y));
	            
				return o;
			}
			
            // samplers
            sampler2D _textureY;
            sampler2D _textureCbCr;

			fixed4 frag (TexCoordInOut i) : SV_Target
			{
				// sample the texture
                float2 texcoord = i.texcoord;
                float y = tex2D(_textureY, texcoord).r;

                float2 size = _ScreenParams.xy;
                float yU = length(tex2D(_textureCbCr , texcoord + size * float2(1,0)).rg);
                float yD = length(tex2D(_textureCbCr , texcoord + size * float2(-1,0)).rg);
                float yL = length(tex2D(_textureCbCr , texcoord + size * float2(0,1)).rg);
                float yR = length(tex2D(_textureCbCr , texcoord + size * float2(0,-1)).rg);

                float mDif = abs(y-yU);
                mDif = max( mDif,abs(y-yD));
                mDif = max( mDif,abs(y-yL));
                mDif = max( mDif,abs(y-yR));

                float4 ycbcr = float4(y, tex2D(_textureCbCr, texcoord).rg, 1.0);

				const float4x4 ycbcrToRGBTransform = float4x4(
						float4(1.0, +0.0000, +1.4020, -0.7010),
						float4(1.0, -0.3441, -0.7141, +0.5291),
						float4(1.0, +1.7720, +0.0000, -0.8860),
						float4(0.0, +0.0000, +0.0000, +1.0000)
					);




				float4 col = mul(ycbcrToRGBTransform, ycbcr);
        float3 hCol = hsv( mDif * _HueSize + _HueStart , 1,1);
				col.xyz = lerp( hCol , hCol * col.xyz , _BlendVal );
        return float4(col.xyz,1);//mul(ycbcrToRGBTransform, ycbcr);
			}
			ENDCG
		}
	}
}
