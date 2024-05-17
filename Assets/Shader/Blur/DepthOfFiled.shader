 Shader "Custom/DepthOfFiled"
 {
      Properties
      {
          _MainTex ("Texture", 2D) = "white" {}
      }
      SubShader
      {
          CGINCLUDE
 
         #include "UnityCG.cginc"
 
         sampler2D _MainTex;
         sampler2D _CameraDepthTexture;
         half4 _MainTex_TexelSize;
         fixed _FocusDistance;
         float _FocusRange;
         sampler2D _BlurTex;
		float _BlurSize;
		  
		struct v2f_blur {
			float4 pos : SV_POSITION;
			half2 uv[5]: TEXCOORD0;
		};
		  
         struct appdata
         {
             float4 vertex : POSITION;
             float2 uv : TEXCOORD0;
        };
 
         struct v2f
         {
             half4 uv:TEXCOORD0;
             half2 uv_depth:TEXCOORD1;
             float4 vertex:SV_POSITION;
         };
		           //https://www.desmos.com/calculator
        fixed4 getRange(float x, float n){
            x = clamp(abs(x), 0, 1/n);
            float y = -(cos(n*x*3.1415926)-1);
            return clamp(y, 0.0, 1.0);
        }
        float getDepthInterpolation(float2 uv){
             float depth=SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv);
             depth=Linear01Depth(depth);
             return depth-_FocusDistance;
        }
		 //模糊相关
		v2f_blur vertBlurVertical(appdata_img v) {
			v2f_blur o;
			o.pos = UnityObjectToClipPos(v.vertex);
			
			half2 uv = v.texcoord;
			
			o.uv[0] = uv;
			o.uv[1] = uv + float2(0.0, _MainTex_TexelSize.y * 1.0) * _BlurSize;
			o.uv[2] = uv - float2(0.0, _MainTex_TexelSize.y * 1.0) * _BlurSize;
			o.uv[3] = uv + float2(0.0, _MainTex_TexelSize.y * 2.0) * _BlurSize;
			o.uv[4] = uv - float2(0.0, _MainTex_TexelSize.y * 2.0) * _BlurSize;
					 
			return o;
		}
		
		v2f_blur vertBlurHorizontal(appdata_img v) {
			v2f_blur o;
			o.pos = UnityObjectToClipPos(v.vertex);
			
			half2 uv = v.texcoord;
			
			o.uv[0] = uv;
			o.uv[1] = uv + float2(_MainTex_TexelSize.x * 1.0, 0.0) * _BlurSize;
			o.uv[2] = uv - float2(_MainTex_TexelSize.x * 1.0, 0.0) * _BlurSize;
			o.uv[3] = uv + float2(_MainTex_TexelSize.x * 2.0, 0.0) * _BlurSize;
			o.uv[4] = uv - float2(_MainTex_TexelSize.x * 2.0, 0.0) * _BlurSize;
					 
			return o;
		}

		fixed4 fragBlur(v2f_blur i) : SV_Target {
			float weight[3] = {0.4026, 0.2442, 0.0545};
			
			fixed3 sum = tex2D(_MainTex, i.uv[0]).rgb * weight[0];
            float d = getDepthInterpolation(i.uv[0]);
			for (int it = 1; it < 3; it++) {

                float2 uv1 = i.uv[it*2-1];
                float2 uv2 = i.uv[it*2];

                float iter1 = getDepthInterpolation(uv1);
                float iter2 = getDepthInterpolation(uv2);

                //对于在焦点后面的，不进入焦点区域取样颜色，保持聚焦区域轮廓清晰。
                iter1 = getRange(iter1, _FocusRange);
                iter2 = getRange(iter2, _FocusRange);
                //纯粹是为了替代if(d>0)逻辑运算
                iter1 -= clamp(d*100, -1, 0);
                iter2 -= clamp(d*100, -1, 0);
                uv1 = lerp(i.uv[0], uv1, clamp(iter1, 0, 1));
                uv2 = lerp(i.uv[0], uv2, clamp(iter2, 0, 1));
                //}
                
				sum += tex2D(_MainTex, uv1).rgb * weight[it];
				sum += tex2D(_MainTex, uv2).rgb * weight[it];
			}
			
			return fixed4(sum, 1.0);
		}
         v2f vert(appdata v)
        {
             v2f o;
             o.vertex=UnityObjectToClipPos(v.vertex);
 
             o.uv.xy=v.uv;
             o.uv.zw=v.uv;
             o.uv_depth=v.uv;
 
             #if UNITY_UV_STARTS_AT_TOP
             if(_MainTex_TexelSize.y<0){
                 o.uv.w=1.0-o.uv.w;
                 o.uv_depth.y=1.0-o.uv_depth.y;
             }
             #endif
 
             return o;
         }

         fixed4 frag(v2f i):SV_Target
         {
             fixed4 color = tex2D(_MainTex, i.uv.xy);
             fixed4 blur=tex2D(_BlurTex,i.uv.zw);

             fixed iter= getDepthInterpolation(i.uv_depth);
             fixed cositer = getRange(iter, _FocusRange);
             //return fixed4(bVa, bVa, bVa, 1);
             return lerp(color,blur,cositer);
         }
 
         ENDCG
 
		Pass {
			
			CGPROGRAM
			#pragma vertex vertBlurVertical  
			#pragma fragment fragBlur
			ENDCG  
		}
		
		Pass {  
			
			CGPROGRAM  
			#pragma vertex vertBlurHorizontal  
			#pragma fragment fragBlur
			ENDCG
		}
         Pass
         {
             CGPROGRAM
             #pragma vertex vert
             #pragma fragment frag
             ENDCG
         }
     }
     FallBack Off
 }
