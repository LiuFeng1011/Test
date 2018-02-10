Shader "Custom/meshshader" {
	Properties {
		
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

        Pass{  
            CGPROGRAM  
  
            #include "UnityCG.cginc"  
            #pragma vertex vert_img  
            #pragma fragment frag  

            float4 frag( v2f_img o ) : COLOR  
            {  
                fixed4 color = fixed4(1,1,1,1); 
                   color.x = sin(o.uv.x*2);
                   color.y = 1-o.uv.x;
                   color.z = 0.5+o.uv.x*0.5;

                return color;

            }  
            ENDCG
        }  
	}
	FallBack "Diffuse"
}
