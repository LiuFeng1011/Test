// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "PengLu/ImageEffect/Unlit/MobileBloom" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Bloom ("Bloom (RGB)", 2D) = "black" {}
	}
	
	CGINCLUDE

		#include "UnityCG.cginc"

		sampler2D _MainTex;
		sampler2D _Bloom;	

		uniform fixed4 _ColorMix;	
		uniform half4 _MainTex_TexelSize;

		uniform fixed4 _Parameter;		
		#define ONE_MINUS_INTENSITY _Parameter.w
	
		// weight curves

		static const half curve[4] = { 0.0205, 0.0855, 0.232, 0.324};  
	

		struct v2f_simple {
			half4 pos : SV_POSITION;
			half4 uv : TEXCOORD0;
		};

		struct v2f_withBlurCoordsSGX 
		{
			float4 pos : SV_POSITION;
			half2 offs[7] : TEXCOORD0;
		};


		struct v2f_withMaxCoords {
			half4 pos : SV_POSITION;
			half2 uv2[5] : TEXCOORD0;
		};

		v2f_withMaxCoords vertMax (appdata_img v)
		{
			v2f_withMaxCoords o;
			o.pos = UnityObjectToClipPos (v.vertex);
        	o.uv2[0] = v.texcoord + _MainTex_TexelSize.xy * half2(1.5,1.5);					
			o.uv2[1] = v.texcoord + _MainTex_TexelSize.xy * half2(-1.5,1.5);
			o.uv2[2] = v.texcoord + _MainTex_TexelSize.xy * half2(-1.5,-1.5);
			o.uv2[3] = v.texcoord + _MainTex_TexelSize.xy * half2(1.5,-1.5);
			o.uv2[4] = v.texcoord ;
			return o; 
		}		

		v2f_withBlurCoordsSGX vertBlurHorizontalSGX (appdata_img v)
		{
			v2f_withBlurCoordsSGX o;
			o.pos = UnityObjectToClipPos (v.vertex);
			half2 netFilterWidth = _MainTex_TexelSize.xy * half2(1.0, 0.0) * _Parameter.x; 

			o.offs[0] = v.texcoord + netFilterWidth;
			o.offs[1] = v.texcoord + netFilterWidth*2.0;
			o.offs[2] = v.texcoord + netFilterWidth*3.0;
			o.offs[3] = v.texcoord - netFilterWidth;
			o.offs[4] = v.texcoord - netFilterWidth*2.0;
			o.offs[5] = v.texcoord - netFilterWidth*3.0;
			o.offs[6] = v.texcoord;

			return o; 
		}		
		
		v2f_withBlurCoordsSGX vertBlurVerticalSGX (appdata_img v)
		{
			v2f_withBlurCoordsSGX o;
			o.pos = UnityObjectToClipPos (v.vertex);	
			half2 netFilterWidth = _MainTex_TexelSize.xy * half2(0.0, 1.0) * _Parameter.x;
			
			o.offs[0] = v.texcoord + netFilterWidth;
			o.offs[1] = v.texcoord + netFilterWidth*2.0;
			o.offs[2] = v.texcoord + netFilterWidth*3.0;
			o.offs[3] = v.texcoord - netFilterWidth;
			o.offs[4] = v.texcoord - netFilterWidth*2.0;
			o.offs[5] = v.texcoord - netFilterWidth*3.0;
			o.offs[6] = v.texcoord;

			return o; 
		}	



		v2f_simple vertBloom (appdata_img v)
		{
			v2f_simple o;
			o.pos = UnityObjectToClipPos (v.vertex);
        	o.uv = v.texcoord.xyxy;			
        	 #if SHADER_API_D3D9
        		if (_MainTex_TexelSize.y < 0.0)
        			o.uv.w = 1.0 - o.uv.w;
        	#endif
			return o; 
		}

		fixed4 fragMax ( v2f_withMaxCoords i ) : COLOR
		{				
			fixed4 color = tex2D(_MainTex, i.uv2[4]);
			color = max(color, tex2D (_MainTex, i.uv2[0]));	
			color = max(color, tex2D (_MainTex, i.uv2[1]));	
			color = max(color, tex2D (_MainTex, i.uv2[2]));	
			color = max(color, tex2D (_MainTex, i.uv2[3]));	
			return saturate(color - ONE_MINUS_INTENSITY);
		} 



		fixed4 fragBlurSGX ( v2f_withBlurCoordsSGX i ) : COLOR
		{
			
			fixed4 color = tex2D(_MainTex, i.offs[6]) * curve[3];
			color += tex2D(_MainTex, i.offs[0])*curve[2];
			color += tex2D(_MainTex, i.offs[1])*curve[1];
			color += tex2D(_MainTex, i.offs[2])*curve[0];
			color += tex2D(_MainTex, i.offs[3])*curve[2];
			color += tex2D(_MainTex, i.offs[4])*curve[1];
			color += tex2D(_MainTex, i.offs[5])*curve[0];

			return color;

		}	
		

		fixed4 fragBloom ( v2f_simple i ) : COLOR
		{	
			fixed4 color = tex2D(_MainTex, i.uv.xy);
			color += tex2D(_Bloom, i.uv.zw)*_Parameter.z*_ColorMix;
			return color;
		}	

	ENDCG
	
	SubShader {
	  ZTest Always  ZWrite Off Cull Off Blend Off

	  Fog { Mode off } 
	//0  
	Pass { 
		CGPROGRAM
		
		#pragma vertex vertMax
		#pragma fragment fragMax
		#pragma fragmentoption ARB_precision_hint_fastest 
		
		ENDCG	 
		}	
	//1	
	Pass {
		
		
		
		CGPROGRAM 
		
		#pragma vertex vertBlurVerticalSGX
		#pragma fragment fragBlurSGX
		#pragma fragmentoption ARB_precision_hint_fastest 
		ENDCG
		}	
		
	//2
	Pass {		
		
		
				
		CGPROGRAM
		
		#pragma vertex vertBlurHorizontalSGX
		#pragma fragment fragBlurSGX
		#pragma fragmentoption ARB_precision_hint_fastest 
		ENDCG
		}
	//3	
	Pass {
		CGPROGRAM
		
		#pragma vertex vertBloom
		#pragma fragment fragBloom
		#pragma fragmentoption ARB_precision_hint_fastest 
		
		ENDCG
		}	
	}	

	FallBack Off
}
