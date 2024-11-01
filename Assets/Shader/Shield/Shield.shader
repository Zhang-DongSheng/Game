Shader "Model/Shield"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Range ("Range", Float) = 10
        _Alpha ("Alpha", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }
        LOD 100

        Pass
        {
            Tags
            {
                "LightMode"="ForwardBase"
            }
            ZWrite Off Cull Back

            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
// Upgrade NOTE: excluded shader from DX11, OpenGL ES 2.0 because it uses unsized arrays
#pragma exclude_renderers d3d11 gles
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float3 _Points[16];
            float _Values[16];
            fixed _PointCount;
            fixed _Range;
            fixed _Alpha;

            

            v2f vert(appdata v)
            {
                v2f o;

                o.pos = UnityObjectToClipPos(v.vertex);

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                o.worldPos = mul(unity_ObjectToWorld, v.vertex);

                return o;
            }

            fixed alpha(float3 pos)
            {
                fixed alpha = _Alpha;

                fixed _count = min(_PointCount, 16);

                for(int i = 0; i < _count; i++)
                {
                    if (_Values[i] < 0.01) continue;

                    fixed dis = distance(pos, _Points[i].xyz);

                    if(dis > _Range) continue;

                    alpha += saturate(lerp(1, 0, dis / _Range) * _Values[i]);
                }
                return alpha;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 _color = tex2D(_MainTex, i.uv);

                _color.a = alpha(i.worldPos);

                return _color;
            }
            ENDCG
        }
    }
}
