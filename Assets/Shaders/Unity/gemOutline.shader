// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

  Shader "Custom/GemOutline" {
    Properties {
      _Threshold ("Thresh", Range(0,1)) = .1
      _Fade("Fade", float) = .2
      _OutlineColor("OutlineColor", Color) = (1,1,1,1)
    }
    SubShader {
 
       Tags { "RenderType" = "Opaque" }


			

			CGPROGRAM
      
       float _Threshold;
       float _HueSize;
       float _HueStart;
       float _Saturation;
       float _Fade;

       float _OutlineOut;
       float _OutlineBack;
       float3 _OutlineColor;
       #pragma target 4.5

       #pragma vertex vert
      #pragma surface surf Standard
      struct Input {
          float2 uv_BumpMap : TEXCOORD0;
          float3 customColor;
          float3 normal;
          float3 eye;
      };
      
      
      #include "../Chunks/hsv.cginc"
      #include "UnityCG.cginc"


      float LightToonShading(float3 normal, float3 lightDir)
            {
                float NdotL = max(0.0, dot(normalize(normal), normalize(lightDir)));
                return floor(NdotL * _Threshold) / (_Threshold - 0.5);
            }

      void vert (inout appdata_full v, out Input o) {
          UNITY_INITIALIZE_OUTPUT(Input,o);
          o.normal = mul(unity_ObjectToWorld , float4( v.normal ,0 ));
          o.eye = normalize( _WorldSpaceCameraPos - mul(unity_ObjectToWorld,v.vertex).xyz);
      }
      void surf (Input v,  inout SurfaceOutputStandard o) {
      	float d = dot(v.eye, v.normal );

      	d = 1-d;
      	if( d < _Threshold ){ discard; }

          o.Emission = _OutlineColor * saturate((d-_Threshold) / _Fade );//float3(0,0,0);// -hsv(toon*_HueSize + _HueStart,_Saturation,1-_Fade *(1-toon));
      }


      ENDCG


      
    
  }
    Fallback "Diffuse"
  }