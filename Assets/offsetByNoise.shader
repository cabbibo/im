// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/offsetByNoise" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_HeightMap ("height map", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque"  "DisableBatching" = "True"}
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert addshadow

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _HeightMap;
		float _TerrainSize;
		float _TerrainHeight;

		struct Input {
			float2 uv_MainTex;
			float3 worldPosition;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		float3 _Player;

		float3 worldPos( float4 pos ){
			float3 wp = mul( unity_ObjectToWorld, pos ).xyz;
 				float4 c = tex2Dlod(_HeightMap , float4(wp.xz * _TerrainSize,0,0) );
 				wp.xyz += float3(0,1,0) * c.r * _TerrainHeight;
 				return wp;
		}

		float4 newPos( float4 pos ){
				float4 wp = float4(worldPos( pos ) ,1 );
 				return mul( unity_WorldToObject, wp);
		}

		float3 getNormal( float4 pos ){

			float delta = 1;
			float4 dU = newPos( pos + float4(delta,0,0,0) );
			float4 dD = newPos( pos + float4(-delta,0,0,0) );
			float4 dL = newPos( pos + float4(0,0,delta,0) );
			float4 dR = newPos( pos + float4(0,0,-delta,0) );

			return -normalize(cross(dU.xyz-dD.xyz , dL.xyz-dR.xyz));

		}
 		void vert (inout appdata_full v,out Input o) {
				UNITY_INITIALIZE_OUTPUT(Input,o);
 				v.normal = getNormal( v.vertex );
 				o.worldPosition = worldPos( v.vertex );
 				v.vertex = newPos( v.vertex );//mul( unity_WorldToObject, worldPos);
      }

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;

			o.Albedo =	saturate(100/pow(length(IN.worldPosition.xyz - _Player.xyz),3));

			float2 dif = abs(IN.uv_MainTex - float2(.5,.5));

			if( dif.x > .49 ){ o.Albedo = float3(0,0,0); }
			if( dif.y > .49 ){ o.Albedo = float3(0,0,0); }
		}
		ENDCG
	}
	FallBack "Diffuse"
}
