// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

  Shader "Toon/Hue" {
    Properties {
      _Threshold ("Thresh", Range(0,20)) = 6
      _HueSize("HueSize", float) = .5
      _HueStart("HueStart", float) = .5
      _Saturation("Saturation", float) = .8
      _Fade("Fade", float) = .2
      _OutlineOut("OutlineOut", float) = .2
      _OutlineBack("OutlineBack", float) = .2
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
       #pragma target 4.5

       #pragma vertex vert
      #pragma surface surf Standard addshadow
      struct Input {
          float2 uv_BumpMap : TEXCOORD0;
          float3 customColor;
          float3 normal;
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
      }
      void surf (Input v,  inout SurfaceOutputStandard o) {
      	float toon = LightToonShading(v.normal,_WorldSpaceLightPos0.xyz);
          o.Albedo = hsv(toon*_HueSize + _HueStart,_Saturation,1-_Fade *(1-toon));
      }
      ENDCG

			

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
      #pragma surface surf NoLighting
      struct Input {
          float2 uv_BumpMap : TEXCOORD0;
          float3 customColor;
          float3 normal;
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
          float3 vDir = normalize(mul(unity_WorldToObject , float4( _WorldSpaceCameraPos ,0 )) - v.vertex.xyz);
          v.vertex.xyz += v.normal * _OutlineOut - vDir * _OutlineBack;
          o.normal = mul(unity_ObjectToWorld , float4( v.normal ,0 ));
      }
      void surf (Input v,  inout SurfaceOutput o) {
      	float toon = LightToonShading(v.normal,_WorldSpaceLightPos0.xyz);
          o.Albedo = float3(0,0,0);// -hsv(toon*_HueSize + _HueStart,_Saturation,1-_Fade *(1-toon));
      }

       fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten) {
         return fixed4(_OutlineColor,1);//half4(s.Albedo, s.Alpha);
     }

      ENDCG


      
    
  }
    Fallback "Diffuse"
  }