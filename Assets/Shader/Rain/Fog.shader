Shader "Weather/Fog"
{
    Properties
    {
        _Color ("Base Color", Color) = (1, 1, 1, 1)
        _Density ("Density", Range(0, 10)) = 1
        _Near ("Near Distance", Float) = 0
        _Far ("Far Distance", Float) = 30
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Tags
            {
                "LightMode"="ForwardBase"
            }
            Fog { Mode Off}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase 

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            fixed4 _Color;
            float _Density;
            float _Near;
            float _Far;

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 depth : TEXCOORD0;
            };

            v2f vert (appdata_base v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.depth = mul(UNITY_MATRIX_MV, v.vertex);
                o.depth.z = o.depth.z * -1;
                o.depth.w = o.depth.w * (_Far - _Near);
                return o;
            }

            fixed4 frag (v2f i) : COLOR
            {
                float fog = 0;
               
                if(i.depth.z > _Far)
                {
                    fog = _Far / i.depth.w;
                }
                else if(i.depth.z > _Near)
                {
                    fog = i.depth.z / i.depth.w;
                }
                return fog * _Density * _Color;
            }
            ENDCG
        }
    }
}