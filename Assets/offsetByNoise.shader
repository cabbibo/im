// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/offsetByNoise" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}

	SubShader{
	Tags { "RenderType"="Opaque" }
		LOD 200


		CGPROGRAM


		#pragma target 4.5

		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard
		#pragma vertex vert 


		#include "UnityCG.cginc"


		struct Input {
			float2 uv_MainTex;
			float3 worldPosition;
			float4 color : TEXCOORD3;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;


		sampler2D _HeightMap;
		float _MapSize;
		float _MapHeight;

		float3 _Player;

		float3 worldPos( float4 pos ){
			float3 wp = mul( unity_ObjectToWorld, pos ).xyz;
 				float4 c = tex2Dlod(_HeightMap , float4(wp.xz * _MapSize,0,0) );
 				wp.xyz += float3(0,1,0) * c.r * _MapHeight;
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


	float4 sampleColor( float4 pos ){
				float3 wp = mul( unity_ObjectToWorld, pos ).xyz;
 				return tex2Dlod(_HeightMap , float4(wp.xz * _MapSize,0,0) );
		}

 		void vert (inout appdata_full v,out Input o) {
				UNITY_INITIALIZE_OUTPUT(Input,o);
 				v.normal = getNormal( v.vertex );
 				o.worldPosition = worldPos( v.vertex );
 				v.vertex = newPos( v.vertex );//mul( unity_WorldToObject, worldPos);
 				o.color = sampleColor( v.vertex );
      }



		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			//fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Emission = IN.color.w * 1;//float3(1,1,1);//c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
	
		}

			ENDCG


	}

	FallBack "Diffuse"
}
