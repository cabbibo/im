Shader "Face/NoiseDiscard" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_CutoffMap ("Cutoff Map", 2D) = "white" {}
		_PatternSize ("Pattern Size" , Vector) = (1,1,0,03)
		_NoiseVec ("NoiseVec" , Vector) = (1,1,0,03)
		_NoiseSpeed ("NoiseSpeed" , float) = 0
		_CutoffNoiseSize ("Cutoff Noise Size" , float) = 10.5
		_CutoffNoisePower ("Cutoff Noise Power" , float) = .2
		_BumpMap ("Normal", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Cutoff("Cutoff" , Range(0,4)) = .5
		_CutoffFade("CutoffFade" , float) = .5
	}
	SubShader {
		Tags {"Queue" = "Transparent" "RenderType"="Transparent" }
		LOD 200

		 ZWrite Off

		Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM
		#include "../Chunks/hsv.cginc"
		// Physically based Standard lighting model, and enable shadows on all light types

      			#pragma vertex vert
		#pragma surface surf Standard addshadow alpha:fade

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 4.5

		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _CutoffMap;

		float2 _PatternSize;
		float3 _NoiseVec;

		float _CutoffNoisePower;
		float _CutoffNoiseSize;
		float _Cutoff;
		float _CutoffFade;
		float _NoiseSpeed;




		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		#include "../Chunks/noise.cginc"


           struct appdata{
            float4 vertex : POSITION;
            float3 normal : NORMAL;
            float4 tangent : TANGENT;
            float4 texcoord : TEXCOORD0;
            float4 texcoord1 : TEXCOORD1;
            float4 texcoord2 : TEXCOORD2;
 
            uint id : SV_VertexID;
         };
 
  

       struct Input {
          float2 texcoord1;
          float3 tangent;
          float3 normal;
          float3 worldPos;
      };

 
       void vert (inout appdata v, out Input data ) {
      
        	UNITY_INITIALIZE_OUTPUT( Input , data );

          data.texcoord1 = v.texcoord;//float2(1,1);
          data.tangent = v.tangent;
          data.normal = v.normal;
          data.worldPos = mul( unity_ObjectToWorld , v.vertex ).xyz;
        
       }
		
     void surf (Input v, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;

	    float3 mainCol = tex2D (_MainTex, v.texcoord1.xy *_PatternSize ).rgb;
	    float3 cutoffCol = tex2D (_CutoffMap, v.texcoord1.xy*_PatternSize).rgb;

	    float n = noise(v.worldPos * _CutoffNoiseSize + _NoiseVec  * (1+_Time.y*_NoiseSpeed)) * _CutoffNoisePower;

	    if( length( cutoffCol) + n  < _Cutoff){ discard; }else{
				float v =  length( cutoffCol) + n  - _Cutoff;

				//mainCol *= saturate( v*v * _CutoffFade );

      o.Alpha = saturate( v*v * _CutoffFade );

	    }
      o.Albedo = mainCol;
    	float3 nor = UnpackNormal(tex2D (_BumpMap, v.texcoord1.xy*_PatternSize));
      o.Normal = nor;
		}
		ENDCG
	}

	/*SubShader {
		// Render the mask after regular geometry, but before masked geometry and
		// transparent things.
 
		Tags {"Queue" = "Geometry+10" }
 
		// Don't draw in the RGBA channels; just the depth buffer
 
		ColorMask 0
		ZWrite On
 
		// Do nothing specific in the pass:
 
		Pass {}
	}*/

	FallBack "Diffuse"
}
