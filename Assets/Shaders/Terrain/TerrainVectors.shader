Shader "Terrain/Vectors" {
	

	Properties {
    _Color ("Color", Color) = (1,1,1,1)
	}

  SubShader{
//        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
    Cull Back
    Pass{

      //Blend SrcAlpha OneMinusSrcAlpha // Alpha blending

      CGPROGRAM
      #pragma target 4.5

      #pragma vertex vert
      #pragma fragment frag

      #include "UnityCG.cginc"
      


		  uniform int _Count;

		int _Width;



    struct Vert{
      float3 pos;
      float3 nor;
      float3 tan;
      float2 uv;
      float debug;
    };
      StructuredBuffer<Vert> _VertBuffer;


      //uniform float4x4 worldMat;

      //A simple input struct for our pixel shader step containing a position.
      struct varyings {
          float4 pos      : SV_POSITION;
          float3 dir      : TEXCOORD2;
          float  grass    : TEXCOORD0;
          float3 nor      : TEXCOORD1;
      };


      //Our vertex function simply fetches a point from the buffer corresponding to the vertex index
      //which we transform with the view-projection matrix before passing to the pixel program.
      varyings vert (uint id : SV_VertexID){

        varyings o;

        int base = id/3;
        int alternate = id %3;


        Vert v = _VertBuffer[base];

        float3 dir = v.tan;
        float3 pos = v.pos;
        float3 c = cross( dir , float3(0,1,0));

        pos += float3(0,1,0);
        if( alternate == 0 ){
        	pos = pos + c * .2;
        }else if( alternate == 1 ){
        	pos = pos + dir * 1.4;
        }else{
        	pos = pos - c*.2;
        }

				         	

				 o.grass = v.debug;
				 o.dir = v.tan;
				 o.nor = v.nor;
	       o.pos = mul (UNITY_MATRIX_VP, float4(pos,1.0f));

       	
        return o;
      }




      //Pixel function returns a solid color for each point.
      float4 frag (varyings v) : COLOR {

      	float3 c = 1;//v.dir * .5 + .5;
          return float4( c , 1 );

      }

      ENDCG

    }
  }

  Fallback Off


}

