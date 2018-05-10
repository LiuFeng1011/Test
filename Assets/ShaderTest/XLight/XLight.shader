// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/XLight" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}
    
	SubShader {
		Tags{"IgnoreProjector"="True" "LightMode" = "ForwardBase" "Queue"="Transparent" "RenderType"="Transparent"}  

        Pass  
        {  
            Blend SrcAlpha OneMinusSrcAlpha  
            ZWrite Off  

            CGPROGRAM  

            #include "Lighting.cginc"  
            #pragma vertex vert  
            #pragma fragment frag 

            fixed4 _Color;  
            struct v2f  
            {  
                float4 pos : SV_POSITION;  
                float3 normal : normal;  
                float3 viewDir : TEXCOORD0;  
            };  
          
            v2f vert (appdata_base v)  
            {  
                v2f o;  
                o.pos = UnityObjectToClipPos(v.vertex);  
                o.viewDir = ObjSpaceViewDir(v.vertex);  
                o.normal = v.normal;  
                return o;  
            }  
          
            fixed4 frag(v2f i) : SV_Target  
            {  
                float3 normal = normalize(i.normal);  
                float3 viewDir = normalize(i.viewDir);  
                float rim = 1 - dot(normal, viewDir);  
                return _Color * rim;  
            }   
            ENDCG  
        }  
	}
	FallBack "Diffuse"
}
