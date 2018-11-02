Shader "Face/Refract"{
	Properties {

      _MainTex ("Texture", 2D) = "white" {}
      _Metallic ("Metallic", Range(0,1)) = 0.5
      _Smooth ("Smooth", Range(0,1)) = 0.5
      _Depth ("Depth", Range(0,1)) = 0.03
      _Amount ("Amount", Range(0,1)) = 0.03
       _BumpMap ("Bumpmap", 2D) = "bump" {}

    }
    SubShader {
         

         Tags {"Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

  
        Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM

 
      #pragma target 4.5
            #include "UnityCG.cginc"

      			#pragma vertex vert
      			#pragma surface surf Standard addshadow

      sampler2D _textureY;
      sampler2D _textureCbCr;
      float4x4 _DisplayTransform;
      sampler2D _MainTex;
      sampler2D _BumpMap;
      float _Metallic;
      float _Smooth;
       float _Amount;
       float _Depth;

           struct appdata{
            float4 vertex : POSITION;
            float3 normal : NORMAL;
            float4 tangent : TANGENT;
            float4 texcoord : TEXCOORD0;
            float4 texcoord1 : TEXCOORD1;
            float4 texcoord2 : TEXCOORD2;
 
            uint id : SV_VertexID;
         };
 
  

       struct Input {
          float2 texcoord1;
          float3 tangent;
          float3 normal;
          float3 worldPos;
      };

 
       void vert (inout appdata v, out Input data ) {
      
        	UNITY_INITIALIZE_OUTPUT( Input , data );

          data.texcoord1 = v.texcoord;//float2(1,1);
          data.tangent = v.tangent;
          data.normal = v.normal;
          data.worldPos = mul( unity_ObjectToWorld , v.vertex ).xyz;
        
       }


     void surf (Input v, inout SurfaceOutputStandard o) {
         
          float3 mainCol = tex2D (_MainTex, v.texcoord1.xy).rgb;
           o.Albedo = mainCol;//*3* hsv(v.texcoord1.x * .2 + sin(v.debug.x*1000) * .04 -.1,1,1);
        
        	float3 nor = UnpackNormal(tex2D (_BumpMap, v.texcoord1.xy ));
        	o.Metallic = _Metallic;
        	o.Smoothness = _Smooth;
          o.Normal = nor;

          // Gets ray going from Camera to vert
        float3 dir = normalize((_WorldSpaceCameraPos - v.worldPos));


        // Here is where refraction happens!
        float3 refracted = normalize(refract( dir , nor , _Amount));
        
        //We take the refracted ray, and step it forward a tiny bit 
        //This is the point that we will use to sample from video texture
        float3 newPos = v.worldPos + refracted * _Depth;


        float4 mp = mul( UNITY_MATRIX_VP , float4( newPos, 1. ) );

        // Getting our screen position
        float4 sp = ComputeScreenPos( mp );
        float3 col = float3(0,0,0);


        // Getting The color from the ARCamera textures!
        float2 texcoord = float2(1,1) -  sp.yx/sp.w;//ComputeScreenPos( v.screenPos ); //v.uv;

        //clamping for when you get to the edge of the screen. 
        // This is really where you can see how the process is quite fake...
        float y = tex2D(_textureY, clamp( texcoord , float2(0.00001,0.000001) , float2(.9999999,.9999999))).r;
        float4 ycbcr = float4(y, tex2D(_textureCbCr, texcoord).rg, 1.0);

        const float4x4 ycbcrToRGBTransform = float4x4(
            float4(1.0, +0.0000, +1.4020, -0.7010),
            float4(1.0, -0.3441, -0.7141, +0.5291),
            float4(1.0, +1.7720, +0.0000, -0.8860),
            float4(0.0, +0.0000, +0.0000, +1.0000)
          );

        col = mul(ycbcrToRGBTransform, ycbcr).xyz;

        o.Albedo = 1.5 * col;


      }
		ENDCG
    }

     
    // 7.) To receive or cast a shadow, shaders must implement the appropriate "Shadow Collector" or "Shadow Caster" pass.
    // Although we haven't explicitly done so in this shader, if these passes are missing they will be read from a fallback
    // shader instead, so specify one here to import the collector/caster passes used in that fallback.
    Fallback "VertexLit"

}
