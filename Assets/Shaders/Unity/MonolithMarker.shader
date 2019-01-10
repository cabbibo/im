
  Shader "Monolith/Marker" {
    Properties {
    }
    SubShader {
 
       Tags { "RenderType" = "Opaque" }

       Pass{

      CGPROGRAM
      
       float _Threshold;
       float _HueSize;
       float _HueStart;
       float _Saturation;
       float _Fade;
       #pragma target 4.5

       #pragma vertex vert
       #pragma fragment frag


            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
       struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
      
      
      #include "../Chunks/hsv.cginc"
      #include "UnityCG.cginc"

      float _CanSelect;
      float _Hovered;
      float _Selected;
      float _HoverTime;
      float _Current;




      v2f vert (appdata v){

          v2f o;
          o.vertex = UnityObjectToClipPos(v.vertex);
          o.uv = v.uv;//TRANSFORM_TEX(v.uv, _MainTex);
          return o;
      
      }

       fixed4 frag (v2f i) : SV_Target{



            		float3 col = float3( 0, 1, 0);

            		col = hsv( _HoverTime * .3 + _Selected * .5 , _CanSelect * .7 + .3, .5 + _Hovered * .5 );

            		if( _Current > .5 ){ col *= 2; }
                // sample the texture
                fixed4 color = float4( col , 1 );//tex2D(_MainTex, i.uv);
                return color;
            }
      ENDCG

			
}

      
    
  }
  }
