// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/MyDiamondShader" {
	Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("BaseTex", 2D) = "white" {}
        _BumpMap ("Normalmap", 2D) = "bump" {}
        _BumpAmt ("Distortion", range (0,1)) = 0.12
    }

    SubShader {
        Tags { "Queue"="Transparent" "RenderType"="Opaque" }
         ZWrite off
         Lighting off

        GrabPass {                          
                Name "BASE"
                Tags { "LightMode" = "Always" }
            }

        CGPROGRAM
        #pragma surface surf Lambert nolightmap nodirlightmap
        #pragma target 3.0
        #pragma debug

        float4 _Color;
        sampler2D _MainTex;
        sampler2D _BumpMap;
        sampler2D _GrabTexture;
        float _BumpAmt;



        struct Input {
            float2 uv_MainTex;
            float2 uv_BumpMap;
            float4 screenPos;
        };


        void surf (Input IN, inout SurfaceOutput o) {
            fixed3 nor = UnpackNormal (tex2D(_BumpMap, IN.uv_BumpMap));
            fixed4 col = tex2D(_MainTex,IN.uv_MainTex);
            float4 screenUV2 = IN.screenPos;
            screenUV2.xy = screenUV2.xy / screenUV2.w;
            screenUV2.xy += nor.xy * _BumpAmt;

            fixed4 trans = tex2D(_GrabTexture,screenUV2.xy)*_Color;
            trans*=col; 
            o.Albedo = trans.rgb;
            o.Emission = trans.rgb;
        ;   
        }                                                                                                                                                                                                                                                                                                   
        ENDCG
    }

    FallBack "Transparent/VertexLit"

}
