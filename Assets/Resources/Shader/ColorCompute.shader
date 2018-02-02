Shader "Custom/ColorCompute" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
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
            uniform fixed4 _Color;  

            //去色
            fixed4 DelColor(fixed4 renderTex){
                fixed c = renderTex.x + renderTex.y + renderTex.z;
                c /= 3;
                return fixed4(c,c,c,1.0f);
            }

            //曝光
            fixed4 Exposure(fixed4 renderTex,fixed force){
                fixed x = min(1,max(0,renderTex.x * pow(2,force)));
                fixed y = min(1,max(0,renderTex.y * pow(2,force)));
                fixed z = min(1,max(0,renderTex.z * pow(2,force)));


                return fixed4(x,y,z,1.0f);
            }
            //颜色加深
            fixed4 ColorPlus(fixed4 renderTex){
                fixed x = 1-(1-renderTex.x)/renderTex.x;
                fixed y = 1-(1-renderTex.y)/renderTex.y;
                fixed z = 1-(1-renderTex.z)/renderTex.z;

                return fixed4(x,y,z,1.0f);
            }

            //颜色减淡
            fixed4 ColorMinus(fixed4 renderTex){
                fixed x = renderTex.x + pow(renderTex.x,2)/(1-renderTex.x);
                fixed y = renderTex.y + pow(renderTex.y,2)/(1-renderTex.y);
                fixed z = renderTex.z + pow(renderTex.z,2)/(1-renderTex.z);

                return fixed4(x,y,z,1.0f);
            }


            //滤色
            fixed4 Screen(fixed4 renderTex){
                fixed x = 1-(pow((1-renderTex.x),2));
                fixed y = 1-(pow((1-renderTex.y),2));
                fixed z = 1-(pow((1-renderTex.z),2));
                return fixed4(x,y,z,1.0f);
            }

            //正片叠底
            fixed4 Muitiply(fixed4 renderTex){
                fixed x = pow(renderTex.x,2);
                fixed y = pow(renderTex.y,2);
                fixed z = pow(renderTex.z,2);
                return fixed4(x,y,z,1.0f);
            }
            //强光
            fixed4 ForceLight(fixed4 renderTex){
                fixed x = renderTex.x;
                fixed y = renderTex.y;
                fixed z = renderTex.z;
                if(x < 0.5f) x = pow(x,2)/0.5f;
                if(y < 0.5f) y = pow(y,2)/0.5f;
                if(z < 0.5f) z = pow(z,2)/0.5f;
                return fixed4(x,y,z,1.0f);
            }

            float4 frag( v2f_img o ) : COLOR  
            {  
                fixed4 renderTex = tex2D(_MainTex, o.uv); 

                renderTex = ForceLight(renderTex);
                    

                return renderTex;
            }  
            ENDCG  
        }  
	}
	FallBack "Diffuse"
}
