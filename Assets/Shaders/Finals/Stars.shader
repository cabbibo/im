Shader "Final/Stars" {
  Properties {

    _Color ("Color", Color) = (1,1,1,1)
    
       _MainTex ("Texture", 2D) = "white" {}
    
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
			
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"    

			struct Vert{
    	  float3 pos;
    	  float3 vel;
    	  float3 nor;
    	  float3 tan;
    	  float2 uv;
    	  float2 debug;
    	};

    	StructuredBuffer<Vert> _TransferBuffer;
      uniform sampler2D _MainTex;

      float3 _Color;

      float3 _Player;


			struct varyings {
				float4 pos 		: SV_POSITION;
				float3 nor 		: TEXCOORD0;
				float2 uv  		: TEXCOORD1;
				float3 eye      : TEXCOORD5;
				float3 worldPos : TEXCOORD6;
				float3 debug    : TEXCOORD7;
				float3 closest    : TEXCOORD8;
				UNITY_SHADOW_COORDS(2)
			};

			#include "../Chunks/hsv.cginc"

			varyings vert(uint id : SV_VertexID) {

				float3 fPos 	= _TransferBuffer[id].pos;
				float3 fNor 	= _TransferBuffer[id].nor;
        float2 fUV 		= _TransferBuffer[id].uv;
				float2 debug 	= _TransferBuffer[id].debug;

				varyings o;

				UNITY_INITIALIZE_OUTPUT(varyings, o);

				o.pos = mul(UNITY_MATRIX_VP, float4(fPos,1));
				o.worldPos = fPos;
				o.eye = _WorldSpaceCameraPos - fPos;
				o.nor = fNor;
				o.uv =  fUV;
				o.debug = float3(debug.x,debug.y,0);

				UNITY_TRANSFER_SHADOW(o,o.worldPos);

				return o;
			}

			float4 frag(varyings v) : COLOR {
		
				fixed shadow = UNITY_SHADOW_ATTENUATION(v,v.worldPos -v.nor ) * .9 + .1 ;
				float4 d = tex2D(_MainTex,v.uv);
        if( d.a < .9 ){discard;}
        

        float3 lDir = normalize(_Player - v.worldPos);
        float3 refl = reflect( lDir , v.nor );
        float rM  = dot( normalize( v.eye) , refl );

        float3 col = normalize(lDir) * .5 + .5;

        rM = rM*rM*rM*rM*rM*rM;
        col = hsv(rM * .1,1,rM);// + normalize(refl) * .5+.5;
        return float4(  col * shadow, 1.);
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
       
        o.uv =  float2(.9,1)- _TransferBuffer[id].uv;
        o.pos = mul(UNITY_MATRIX_VP, float4(_TransferBuffer[id].pos, 1));
        return o;
      }

      float4 frag(v2f i) : COLOR
      {
        float4 col = tex2D(_MainTex,i.uv);
        if( col.a < .4){discard;}
        SHADOW_CASTER_FRAGMENT(i)
      }
      ENDCG
    }
  


	}

}
