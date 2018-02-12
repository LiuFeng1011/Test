Shader "Custom/CollapseShader" {
	Properties {
		_MainTex ("Maintex", 2D) = "white" {}
        _Progress ("Progress" , Float) = 1  
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

        Pass{  
		CGPROGRAM
		    #include "UnityCG.cginc"    
            #pragma vertex vert_img    
            #pragma fragment frag  
 
            sampler2D _MainTex;
            float _Progress;

            fixed4 frag(v2f_img i) : SV_Target {  

                float2 uv = (i.uv - float2(0.5f,0.5f));

                float dis = length(uv);

                //uv = i.uv + normalize(uv)*(1/(dis + 0.01f) * 0.07f)*(_Progress);
                uv = i.uv + normalize(uv)*(1-length(uv))*(_Progress);
                
                fixed4 c = tex2D(_MainTex,uv);  
 
                return c*(1-_Progress);  
            }    
        ENDCG 
        }     
	}
	FallBack "Diffuse"
}
