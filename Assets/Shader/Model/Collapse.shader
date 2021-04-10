//Sahder: ËúÏÝ
Shader "Model/Collapse"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Progress ("Progress", Range(0, 1)) = 0
        _Collapsar ("Collapsar Position", Vector) = (0.5, 0.5, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            fixed4 _Collapsar;
            fixed _Progress;

            fixed4 frag (v2f_img  i) : SV_Target
            {
                float2 uv = i.uv - _Collapsar.xy;

                uv = i.uv + normalize(uv) * (1 - length(uv)) * _Progress;

                fixed4 col = tex2D(_MainTex, uv);

                return col * (1 - _Progress);
            }
            ENDCG
        }
    }
}