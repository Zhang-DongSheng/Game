/* * 语法说明
#pragma multi_compile_instancing    使用该指令让Unity生成实例的变体，在表面着色器中是不需要的
UNITY_VERTEX_INPUT_INSTANCE_ID      在顶点着色器的输入输出结构中定义实例Id
UNITY_INSTANCING_BUFFER_START(name) / UNITY_INSTANCING_BUFFER_END(name)	 每个实例属性都要在一个常量缓冲区中定义，使用这一对宏命令将你希望在每个实例中值都不一样的属性包起来
UNITY_DEFINE_INSTANCED_PROP(propertyType, propertyName)	    使用类型和名称定义一个实例属性
UNITY_SETUP_INSTANCE_ID(v);         使shader函数可以访问实例Id ，在顶点着色器中必须使用，片元着色器可选
UNITY_TRANSFER_INSTANCE_ID(v, o);   在顶点着色器中将实例Id从输入结构复制到输出结构，只有在片元着色器需要访问实例数据时才需要
UNITY_ACCESS_INSTANCED_PROP(arrayName, propertyName)	    从缓冲区中获取实例的属性值，arrayName与UNITY_INSTANCING_BUFFER_START(name)对应
* */
Shader "Study/GPUInstance"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #include "UnityCG.cginc"

            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            sampler2D _MainTex;
            float4 _MainTex_ST;

            struct a2v
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(a2v v)
            {
                v2f o;

                UNITY_SETUP_INSTANCE_ID(v);

                o.vertex = UnityObjectToClipPos(v.vertex);

                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 color = tex2D(_MainTex, i.uv);

                return color;
            }
            ENDCG
        }
    }
}