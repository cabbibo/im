Shader "Custom/Golden" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_BumpMap ("Normal", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
		#include "../Chunks/hsv.cginc"
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard addshadow

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _BumpMap;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;

			     float3 mainCol = tex2D (_MainTex, IN.uv_MainTex.xy * .8).rgb;
           o.Albedo =mainCol* 3* hsv(abs( ((IN.uv_MainTex.x *  .4 + IN.uv_MainTex.y + _Time.y  * .04)%.2) -.1),1,1);
           o.Albedo = 1* hsv(abs( (length(mainCol) * .3 + _Time.y *.1) %.2-.1),1,1);
        	float3 nor = UnpackNormal(tex2D (_BumpMap, IN.uv_MainTex.xy * .8));

          o.Normal = nor;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
