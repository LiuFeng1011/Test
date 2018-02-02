Shader "Custom/FotTest" {
    
	Properties {  
        _MainTex ("Base (RGB)", 2D) = "white" {}  
        _Color ("Ground Color", Color) = (1,0,1,1) //颜色
        _ChangeDis ("Change Dis", float) = 3.0 //渐变速度

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
            uniform float _ChangeDis; 
            sampler2D _CameraDepthTexture;



            float4 GetWorldPositionFromDepthValue( float2 uv, float linearDepth )   
            {  
                
                float camPosZ = _ProjectionParams.y + (_ProjectionParams.z - _ProjectionParams.y) * linearDepth;  
  
                // unity_CameraProjection._m11 = near / t，其中t是视锥体near平面的高度的一半。  
                // 投影矩阵的推导见：http://www.songho.ca/opengl/gl_projectionmatrix.html。  
                // 这里求的height和width是坐标点所在的视锥体截面（与摄像机方向垂直）的高和宽，并且  
                // 假设相机投影区域的宽高比和屏幕一致。  
                float height = 2 * camPosZ / unity_CameraProjection._m11;  
                float width = _ScreenParams.x / _ScreenParams.y * height;  
  
                float camPosX = width * uv.x - width / 2;  
                float camPosY = height * uv.y - height / 2;  
                float4 camPos = float4(camPosX, camPosY, camPosZ, 1.0);  
                return mul(unity_CameraToWorld, camPos);  
            }  

            fixed4 GetWorldPos(float2 uv){
                float rawDepth =  SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, uv );  
                // 注意：经过投影变换之后的深度和相机空间里的z已经不是线性关系。所以要先将其转换为线性深度。  
                // 见：https://developer.nvidia.com/content/depth-precision-visualized  
                float linearDepth = Linear01Depth(rawDepth); 
                fixed4 worldpos = GetWorldPositionFromDepthValue( uv, linearDepth ); 
                return worldpos;
            }

            float4 frag( v2f_img o ) : COLOR  
            {  
                //获取世界坐标
                fixed4 worldpos = GetWorldPos( o.uv); 

                fixed4 renderTex = tex2D(_MainTex, o.uv);  

                fixed y = worldpos.y;

                //上下变色
                fixed tmp = step(0,y); 
                fixed lp1 = - y / _ChangeDis;

                fixed lp = lp1 * (1-tmp) ;

                fixed4 c = _Color * (1-tmp);

                return lerp(renderTex ,c,min(lp,1)) ;
            }  
            ENDCG  
        }  
    }   
}
