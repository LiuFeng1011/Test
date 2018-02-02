// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/MapObjShader" {
	Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Color ("Ground Color", Color) = (1,1,0,1) //底层颜色
        _BaseHigh ("Base High", Float) = 10.0 //基础高度
        _ChangeDis ("Change Dis", Float) = 1.0 //渐变速度
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert vertex:Myvert


        uniform fixed4 _Color;  
        uniform float _BaseHigh;  
        uniform float _ChangeDis;  
        sampler2D _CameraDepthTexture;
        sampler2D _MainTex;


        struct Input {
            float2 uv_MainTex;
            float3 worldSpacePos;
        };

        void Myvert(inout appdata_full v, out Input IN)
        {
            UNITY_INITIALIZE_OUTPUT(Input, IN);
            IN.worldSpacePos = mul(unity_ObjectToWorld, v.vertex);
        }

        void surf (Input IN, inout SurfaceOutput o) {
            fixed4 tint;

            half4 c = tex2D (_MainTex, IN.uv_MainTex);


            float y = IN.worldSpacePos.y - _BaseHigh;

            if(y < 0){

                float lp = 0.0;
                lp = - y / _ChangeDis;
                if(lp > 1) lp = 1;

                o.Albedo = lerp(c ,_Color,lp);
            }else{
                o.Albedo = c.rgb;
            }

            o.Alpha = c.a;
        }
        ENDCG
    } 
    FallBack "Diffuse"
}
