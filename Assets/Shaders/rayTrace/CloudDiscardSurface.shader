  Shader "Surface/CloudDiscard" {
    	
  Properties {
  
    _NumberSteps( "Number Steps", Int ) = 3
    _TotalDepth( "Total Depth", Float ) = 0.16
    _NoiseSize( "Noise Size" , Float ) = 0.6
    _HueSize( "Hue Size" , Float ) = 2.45
    _BaseHue( "Base Hue" , Float ) = 1.67
    _Saturation( "Saturation" , Float ) = 1
    _NoiseSpeed( "Noise Speed" , Float ) = 0.8
    _Cutoff("Cutoff" , Float ) = .5

  }
    SubShader {
      Tags { "RenderType" = "Opaque" }
      CGPROGRAM
      #pragma surface surf Lambert vertex:vert
      #pragma target 4.5

      #include "UnityCG.cginc"



      uniform int _NumberSteps;
      uniform float _TotalDepth;

      uniform float _NoiseSize;
      uniform float _NoiseSpeed;

      uniform float _Saturation;

      uniform float _HueSize;
      uniform float _BaseHue;

      uniform float _Cutoff;
 
 			float3 hsv(float h, float s, float v){
        return lerp( float3( 1.0,1,1 ), clamp(( abs( frac(h + float3( 3.0, 2.0, 1.0 ) / 3.0 )
        					 * 6.0 - 3.0 ) - 1.0 ), 0.0, 1.0 ), s ) * v;
      }


			// Taken from https://www.shadertoy.com/view/4ts3z2
			// By NIMITZ  (twitter: @stormoid)
			// good god that dudes a genius...

			float tri( float x ){ 
			  return abs( frac(x) - .5 );
			}

			float3 tri3( float3 p ){
			 
			  return float3( 
			      tri( p.z + tri( p.y * 1. ) ), 
			      tri( p.z + tri( p.x * 1. ) ), 
			      tri( p.y + tri( p.x * 1. ) )
			  );

			}
			                                 
			float triNoise3D( float3 p, float spd , float time){
			  
			  float z  = 1.4;
				float rz =  0.;
			  float3  bp =   p;

				for( float i = 0.; i <= 3.; i++ ){
			   
			    float3 dg = tri3( bp * 2. );
			    p += ( dg + time * .1 * spd );

			    bp *= 1.8;
					z  *= 1.5;
					p  *= 1.2; 
			      
			    float t = tri( p.z + tri( p.x + tri( p.y )));
			    rz += t / z;
			    bp += 0.14;

				}

				return rz;

			}


      struct Input {
          float3 ro    ;
          float3 rd    ;
          float3 camPos;
          float3 world ;
          float3 nor ;
      };


      void vert (inout appdata_full v, out Input o) {
          UNITY_INITIALIZE_OUTPUT(Input,o);


        o.camPos = mul( unity_WorldToObject , float4( _WorldSpaceCameraPos , 1. )).xyz;
        //.lightPos = mul( unity_WorldToObject , _WorldSpaceLightPos0 ).xyz ;
  
     
        float3 mPos = mul( unity_ObjectToWorld , v.vertex.xyz );
        o.world = mPos;
        o.nor = v.normal;

        o.ro = v.vertex.xyz;

        o.rd = normalize( o.camPos - v.vertex.xyz );
      
      }



      sampler2D _MainTex;
      void surf (Input v, inout SurfaceOutput o) {
      float3 ro 			= v.ro;
        float3 rd 			= v.rd; 
         
        float3 col = v.nor * .5 + .5;//float3( 0.0 , 0.0 , 0.0 );

        float3 p;



        float acc = 0.;

        //float3 col = float3( 0, 0,0);


        float hit = 0;
        float wasHit = 0;

        for( int i = 0; i < _NumberSteps; i++ ){

        	float stepVal = float( i ) / _NumberSteps;

          p = ro + -rd * _TotalDepth * stepVal;

          float v = float( i );
          float val = triNoise3D( p * _NoiseSize, _NoiseSpeed , _Time.y );

          

        	if( val > _Cutoff ){
        		hit = stepVal;
        		wasHit = 1;
        		//hit = float(i);
        		break;
        	}

        }
if(wasHit!= 1){
	col = hsv(_BaseHue+_HueSize,_Saturation,.2);//.1;//discard;
}else{
	col = hsv(_BaseHue,_Saturation,1);//.1;//discard;
}
        if( wasHit == 1 && hit != 0){
        	col = hsv( hit * _HueSize + _BaseHue, _Saturation,(1-hit*.8));
				}

				float3 refl = reflect( v.nor , v.rd );
				col *= refl * .5 + .5;
				col *= hsv( pow(dot(refl,float3(0,1,0 )) , 5) * .8,.5,1);

        //float3 bw = float3( hit,hit,hit);

        //col = lerp( bw * bw * 4,col,_Saturation);


          o.Albedo = col;//float3(1,1,1);
      }
      ENDCG
    } 
    Fallback "Diffuse"
  }