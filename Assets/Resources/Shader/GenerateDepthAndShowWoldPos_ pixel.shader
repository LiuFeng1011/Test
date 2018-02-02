Shader "Custom/GenerateDepthAndShowWoldPos" {
    
	Properties {  
        _MainTex ("Base (RGB)", 2D) = "white" {}  
    }
    SubShader {  
        Tags { "RenderType"="Opaque" }  
        LOD 200  
  
        Pass{  
            CGPROGRAM  
  
            #include "UnityCG.cginc"  
            #pragma vertex vert_img  
            #pragma fragment frag  

            uniform sampler2D _MainTex;  
  
            fixed4 Frosted(float2 uv){
                float depth= 0.015;
                half4 sum = tex2D(_MainTex, uv);

                sum += tex2D( _MainTex, float2(uv.x-5.0 * depth, uv.y+5.0 * depth)) * 0.025;    
                sum += tex2D( _MainTex, float2(uv.x+5.0 * depth, uv.y-5.0 * depth)) * 0.025;

                sum += tex2D( _MainTex, float2(uv.x-4.0 * depth, uv.y+4.0 * depth)) * 0.05;
                sum += tex2D( _MainTex, float2(uv.x+4.0 * depth, uv.y-4.0 * depth)) * 0.05;

                sum += tex2D( _MainTex, float2(uv.x-3.0 * depth, uv.y+3.0 * depth)) * 0.09;
                sum += tex2D( _MainTex, float2(uv.x+3.0 * depth, uv.y-3.0 * depth)) * 0.09;

                sum += tex2D( _MainTex, float2(uv.x-2.0 * depth, uv.y+2.0 * depth)) * 0.12;
                sum += tex2D( _MainTex, float2(uv.x+2.0 * depth, uv.y-2.0 * depth)) * 0.12;

                sum += tex2D( _MainTex, float2(uv.x-1.0 * depth, uv.y+1.0 * depth)) *  0.15;
                sum += tex2D( _MainTex, float2(uv.x+1.0 * depth, uv.y-1.0 * depth)) *  0.15;
                /*
                sum += tex2D( _MainTex, uv-5.0 * depth) * 0.025;    
                sum += tex2D( _MainTex, uv-4.0 * depth) * 0.05;
                sum += tex2D( _MainTex, uv-3.0 * depth) * 0.09;
                sum += tex2D( _MainTex, uv-2.0 * depth) * 0.12;
                sum += tex2D( _MainTex, uv-1.0 * depth) * 0.15;    
                sum += tex2D( _MainTex, uv) * 0.16; 
                sum += tex2D( _MainTex, uv+5.0 * depth) * 0.15;
                sum += tex2D( _MainTex, uv+4.0 * depth) * 0.12;
                sum += tex2D( _MainTex, uv+3.0 * depth) * 0.09;
                sum += tex2D( _MainTex, uv+2.0 * depth) * 0.05;
                sum += tex2D( _MainTex, uv+1.0 * depth) * 0.025;*/

                return sum / 2;
            }
  
            float4 frag( v2f_img o ) : COLOR  
            {  
                fixed4 renderTex = Frosted( o.uv); 
                return renderTex;
            }  
            ENDCG  
        }  
    }   
}
