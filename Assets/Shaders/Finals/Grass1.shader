﻿Shader "Finals/Grass1" {
  Properties {

    _Color ("Color", Color) = (1,1,1,1)
    _FalloffRadius ("Falloff", float) = 20

    _MainTex ("Texture", 2D) = "white" {}

       _ColorMap ("ColorMap", 2D) = "white" {}
    
    [Toggle(Enable16Struct)] _Struct16("16 Struct", Float) = 0
    [Toggle(Enable24Struct)] _Struct24("24 Struct", Float) = 0
    [Toggle(Enable36Struct)] _Struct36("36 Struct", Float) = 0
  }

	SubShader {
		// COLOR PASS

		Pass {
			Tags{ "LightMode" = "ForwardBase" }
			Cull Off

			CGPROGRAM
			#pragma target 4.5
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight

			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
			#include "../Chunks/StructIfDefs.cginc"
      #include "../Chunks/hsv.cginc"
			#include "../Chunks/hash.cginc"

      float3 _Color;
      float3 _Player;
      float _FalloffRadius;
      sampler2D _MainTex;
      sampler2D _ColorMap;


			struct varyings {
				float4 pos 		: SV_POSITION;
				float3 nor 		: TEXCOORD0;
				float2 uv  		: TEXCOORD1;
				float3 eye      : TEXCOORD5;
				float3 worldPos : TEXCOORD6;
        float3 debug    : TEXCOORD7;
				float3 vel    : TEXCOORD9;
				float3 closest    : TEXCOORD8;
				UNITY_SHADOW_COORDS(2)
			};

			varyings vert(uint id : SV_VertexID) {

				float3 fPos 	= _TransferBuffer[id].pos;
        float3 fNor   = _TransferBuffer[id].nor;
				float3 fVel 	= _TransferBuffer[id].vel;
        float2 fUV 		= _TransferBuffer[id].uv;
				float2 debug 	= _TransferBuffer[id].debug;

				varyings o;

				UNITY_INITIALIZE_OUTPUT(varyings, o);

				o.pos = mul(UNITY_MATRIX_VP, float4(fPos,1));
				o.worldPos = fPos;
				o.eye = _WorldSpaceCameraPos - fPos;
        o.nor = fNor;
				o.vel = fVel;

        float offset = floor(hash(debug.x) * 6) /6;
				o.uv =  fUV.yx * float2(1./6.,.999) + float2(offset,0);
				o.debug = float3(debug.x,debug.y,0);

				UNITY_TRANSFER_SHADOW(o,o.worldPos);

				return o;
			}

			float4 frag(varyings v) : COLOR {

        float4 color = float4(0,0,0,0);// = tex2D(_MainTex,v.uv );
				float4 tcol = tex2D(_MainTex,v.uv );
		
				fixed shadow = UNITY_SHADOW_ATTENUATION(v,v.worldPos  ) * .9 + .1 ;
float dif = saturate((_FalloffRadius -  length( v.worldPos - _Player ))/_FalloffRadius);
				
        float col =.2*pow(length(tcol.xyz) , 10);
        //color.xyz *= 1* hsv(color.a +v.uv.x *.3+.1+saturate(100*length(v.vel)) * .2 - .4,.3,dif);//col*hsv( v.uv.x * .4 + sin( v.debug.x) * .1 + sin(dif) * 1+ sin(_Time.y) * .1 , .7,dif);
       // color.xyz *= col;

       float vSat =  saturate(40*length(v.vel) + .3);
       float hue = -saturate(length( v.worldPos - _Player ) * .3) * .1 + .3 + tcol.r   * .1  +saturate(length(v.vel) * 20) * .1 ;
       color.xyz = tex2D(_ColorMap , float2( hue,0 ));;
        color.xyz *=  vSat;
        color.xyz *=  v.uv.y * 1.1;

			 	
        color *= (tcol+1);
				if( tcol.a < .8 ){ discard; }
color *= dif;
        if( v.debug.y < .3 ){ discard; }
        return float4( color.xyz * shadow, 1.);
			}

			ENDCG
		}


   // SHADOW PASS

    Pass
    {
      Tags{ "LightMode" = "ShadowCaster" }


      Fog{ Mode Off }
      ZWrite On
      ZTest LEqual
      Cull Off
      Offset 1, 1
      CGPROGRAM

      #pragma target 4.5
      #pragma vertex vert
      #pragma fragment frag
      #pragma multi_compile_shadowcaster
      #pragma fragmentoption ARB_precision_hint_fastest

  #include "UnityCG.cginc"
#include "../Chunks/StructIfDefs.cginc"

sampler2D _MainTex;
  struct v2f {
        V2F_SHADOW_CASTER;
        float2 uv : TEXCOORD1;
      };


      v2f vert(appdata_base v, uint id : SV_VertexID)
      {
        v2f o;
       
        o.uv = _TransferBuffer[id].uv.yx *float2(1./6.,1);;
        o.pos = mul(UNITY_MATRIX_VP, float4(_TransferBuffer[id].pos, 1));
        return o;
      }

      float4 frag(v2f i) : COLOR
      {
        float4 col = tex2D(_MainTex,i.uv);
        if( col.a < .8){discard;}
        SHADOW_CASTER_FRAGMENT(i)
      }
      ENDCG
    }
  


	}

}
