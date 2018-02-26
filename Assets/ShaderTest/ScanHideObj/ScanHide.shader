Shader "Custom/ScanHide" {
	Properties {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    
    SubShader {
        Tags{"IgnoreProjector"="True" "LightMode" = "ForwardBase" "Queue"="Transparent" "RenderType"="Transparent"}  

        Pass  
        {  
            ZWrite On
            //ZTest Greater  

            CGPROGRAM  
    
            #pragma vertex vert  
            #pragma fragment frag 
            #include "Lighting.cginc" 

            sampler2D _MainTex;  
            float4 _MainTex_ST; 

            struct a2v{  
                float4 vertex : POSITION;  
                float3 normal : NORMAL;  
                float4 texcoord : TEXCOORD0;  
            };   

            struct v2f  
            {  
                float4 pos : SV_POSITION;  
                float2 uv : TEXCOORD0;  
                float3 worldPos : TEXCOORD1;  
            };  
          
            v2f vert (a2v v)  
            {  
                v2f o;  
                o.pos = UnityObjectToClipPos(v.vertex);   
                o.uv = TRANSFORM_TEX(v.texcoord,_MainTex);  
                //float3 wp = mul(unity_ObjectToWorld,v.vertex).xyz;  

                o.worldPos = mul(unity_ObjectToWorld,v.vertex).xyz;  

                //o.worldPos = mul(o.worldPos , normal).xyz;

                return o;  
            }  
          
            fixed4 frag(v2f i) : SV_Target  
            {  
                return tex2D(_MainTex,i.worldPos.xy);  
            }   
            ENDCG  
        }  
    }
    FallBack "Diffuse"
}
