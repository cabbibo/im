// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Final/Water"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct varyings
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD3;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;


		sampler2D _HeightMap;
		float _MapSize;
		float _MapHeight;
		float3 _Player;
      

	float getHeight( float3 pos ){
    float4 c = tex2D( _HeightMap , pos.xz * _MapSize );
    return c.x  * _MapHeight;
	}

		float getHeightLOD( float3 pos ){
    float4 c = tex2Dlod( _HeightMap , float4(pos.xz * _MapSize,0,0) );
    return c.x  * _MapHeight;
	}

	#include "../Chunks/noise.cginc"


            
            varyings vert (appdata v)
            {
                varyings o;

                float3 wP = mul(unity_ObjectToWorld, v.vertex).xyz;
               	float h = getHeightLOD(wP);
                wP.y +=  .4*(1+noise(float3(wP.x, wP.y-_Time.y, wP.z) * .4 ));
                wP = mul( unity_WorldToObject , float4(wP,1)).xyz;
                o.vertex = UnityObjectToClipPos(float4(wP,1));
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
               	o.worldPos = mul(unity_ObjectToWorld, float4(wP,1)).xyz;
                return o;
            }
            
            fixed4 frag (varyings v) : SV_Target
            {
           			float h = getHeight(v.worldPos);

           			float n  = noise(float3(v.worldPos.x , v.worldPos.y ,v.worldPos.z) + float3(0,_Time.y,0) * 1.4);
           			float n2  = noise(float3(v.worldPos.x , v.worldPos.y ,v.worldPos.z) * 5+ float3(0,_Time.y,0) * 1.4);
           			float p = length( v.worldPos.xyz - _Player.xyz);

           			float dist = 20;

           			float l =saturate((dist-p)/dist);
           			float4 col;// = 4 *saturate(float4((h-v.worldPos.y)*1 + n*.3,.3,.3, 1))*l;// / p;
                col = (h* 1 + n  * h + n2 * .2-.2) + l*l*l*l;//,0,0,1);
                return col*l;
            }
            ENDCG
        }
    }
}













