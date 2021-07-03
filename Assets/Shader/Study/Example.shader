Shader "Study/Example"
{
    Properties
    {
        [PerRendererData] _MainTex ("Texture", 2D) = "white" {}
        [Enum(None, 0, First, 1, Second, 2)] _Enum ("Enum", Int) = 1
    }
    SubShader
    {
        Tags 
        {
            /*
            "RenderType"="Opaque"               //用于大多数着色器[法线着色器、自发光着色器、反射着色器以及地形的着色器]
            "RenderType"="Transparent"          //用于半透明着色器[透明着色器、粒子着色器、字体着色器、地形额外通道的着色器]
            "RenderType"="TransparentCutout"    //蒙皮透明着色器[Transparent Cutout，两个通道的植被着色器]
            "RenderType"="Background"           //天空盒着色器。
            "RenderType"="Overlay"              //光晕着色器、闪光着色器
            "RenderType"="TreeOpaque"           //地形引擎中的树皮
            "RenderType"="TreeTransparentCutout"//地形引擎中的树叶
            "RenderType"="TreeBillboard"        //地形引擎中的广告牌树
            "RenderType"="Grass"                //地形引擎中的草
            "RenderType"="GrassBillboard"       //地形引擎何中的广告牌草
            */
        }
        LOD 100

        /* Blend {code for SrcFactor} {code for DstFactor}
        //One                 float4(1, 1, 1, 1)
        //Zero                float4(0, 0, 0, 0)
        //SrcColor            fragment_output[片元产生的颜色]
        //SrcAlpha            fragment_output.aaaa
        //DstColor            pixel_color[缓存区的颜色]
        //DstAlpha            pixel_color.aaaa
        //OneMinusSrcColor    float4(1, 1, 1, 1) - fragment_output
        //OneMinusSrcAlpha    float4(1, 1, 1, 1) - fragment_output.aaaa
        //OneMinusDstColor    float4(1, 1, 1, 1) - pixel_color
        //OneMinusDstAlpha    float4(1, 1, 1, 1) - pixel_color.aaaa
        
        Blend SrcAlpha OneMinusSrcAlpha         //传统透明度
        Blend One OneMinusSrcAlpha              //预乘透明度
        Blend One One                           //叠加
        Blend OneMinusDstColor One              //柔和叠加
        Blend DstColor Zero                     //相乘——正片叠底
        Blend DstColor SrcColor                 //两倍相乘
        Blend Off                               //不混合
        */

        Pass
        {
            Tags
            {
                /* 
                "LightMode"="Normal"            //不与光照交互的规则着色器通道
                "LightMode"="Vertex"            //旧的顶点照明着色器通道
                "LightMode"="VertexLM"          //旧的顶点照明着色器通道，并且带有移动端光照图
                "LightMode"="ForwardBase"       //前向渲染基本通道
                "LightMode"="ForwardAdd"        //前向渲染加像素光通道
                "LightMode"="LightPrePassBase"  //旧延迟光照基础通道
                "LightMode"="LightPrePassFinal" //旧延迟光照最终通道
                "LightMode"="ShadowCaster"      //阴影投射和深度纹理着色器通道
                "LightMode"="Deffered"          //延迟着色器通道
                "LightMode"="Meta"              //用于产生反照率和发射值的着色器通道，用作光映射的输入
                "LightMode"="MotionVectors"     //运动矢量渲染通道
                "LightMode"="ScriptableRenderPipeline"              //自定义脚本通道
                "LightMode"="ScriptableRenderPipelineDefaultUnlit"  //当光模式被设置为默认未亮或没有光模式时，设置自定义脚本管道
                */
            }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Enum;

            v2f vert (appdata_base v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);

                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 color = tex2D(_MainTex, i.uv);

                switch(_Enum)
                {
                    case 1:
                        color = color * 0.5;
                        break;
                    case 2:
                        color = color * 0.75;
                        break;
                    default:
                        break;
                }

                return color;
            }
            ENDCG
        }
    }
}