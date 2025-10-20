// 溶解
Shader "UI/Dissolve"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}                        // 主纹理
        _NoiseTex("Noise Texture", 2D) = "white" {}                 // 噪声纹理
        _DissolveThreshold("Dissolve Threshold", Range(0, 1)) = 0.5 // 溶解阈值
        _EdgeColor("Edge Color", Color) = (1, 0, 0, 1)              // 边缘颜色
        _EdgeWidth("Edge Width", Range(0, 0.2)) = 0.1               // 边缘宽度

        _StencilComp("Stencil Comparison", Float) = 8
        _Stencil("Stencil ID", Float) = 0
        _StencilOp("Stencil Operation", Float) = 0
        _StencilWriteMask("Stencil Write Mask", Float) = 255
        _StencilReadMask("Stencil Read Mask", Float) = 255
        _ColorMask("Color Mask", Float) = 15
    }
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "CanUseSpriteAtlas" = "True"
        }
        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask[_ColorMask]

        Stencil
        {
            Ref[_Stencil]
            Comp[_StencilComp]
            Pass[_StencilOp]
            ReadMask[_StencilReadMask]
            WriteMask[_StencilWriteMask]
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
    
            #include "UnityCG.cginc"
    
            // 定义输入结构
            struct appdata
            {
                float4 vertex : POSITION;  // 顶点位置
                float2 uv : TEXCOORD0;     // 纹理坐标
            };
    
            // 定义输出结构
            struct v2f
            {
                float2 uv : TEXCOORD0;      // 主纹理坐标
                float2 uvNoise : TEXCOORD1; // 噪声纹理坐标
                float4 vertex : SV_POSITION;// 裁剪空间顶点位置
            };
    
            // 属性变量
            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _NoiseTex;
            float4 _NoiseTex_ST;
            float _DissolveThreshold;
            float4 _EdgeColor;
            float _EdgeWidth;
    
            // 顶点 Shader
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);  // 顶点变换到裁剪空间
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);       // 主纹理坐标变换
                o.uvNoise = TRANSFORM_TEX(v.uv, _NoiseTex); // 噪声纹理坐标变换
                return o;
            }
    
            // 片段 Shader
            fixed4 frag(v2f i) : SV_Target
            {
                // 采样主纹理
                fixed4 main = tex2D(_MainTex, i.uv);
                // 采样噪声纹理
                float noise = tex2D(_NoiseTex, i.uvNoise).r;
                // 溶解效果 如果噪声值小于阈值，丢弃像素
                if (noise < _DissolveThreshold)
                    discard;
                // 计算边缘透明度
                float edgeAlpha = smoothstep(_DissolveThreshold, _DissolveThreshold + _EdgeWidth, noise);
                // 计算边缘颜色
                fixed4 edgeColor = _EdgeColor * (1 - edgeAlpha);
                // 最终颜色 = 主纹理颜色 + 边缘颜色
                fixed4 col = main * edgeAlpha + edgeColor;
                // 设置透明度
                col.a = min(main.a, edgeAlpha);

                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}