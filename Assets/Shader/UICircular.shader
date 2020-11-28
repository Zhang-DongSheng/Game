//圆角Shader
Shader "UI/Circular"
{
	Properties 
	{
		[PerRendererData] _MainTex ("Base (RGB)", 2D) = "white" {}
		_Radius("_Radius", Range(0, 0.5)) = 0.2

		//MASK SUPPORT ADD
        [HideInInspector]_StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector]_Stencil ("Stencil ID", Float) = 0
        [HideInInspector]_StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector]_StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector]_StencilReadMask ("Stencil Read Mask", Float) = 255
        [HideInInspector]_ColorMask ("Color Mask", Float) = 15
        //MASK SUPPORT END
	}
	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
		}

		//MASK SUPPORT ADD
        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp] 
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
        ColorMask [_ColorMask]
        //MASK SUPPORT END

		pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma exclude_renderers gles
			#pragma vertex vert
			#pragma fragment frag

			#include "unitycg.cginc"

			fixed _Radius;
			sampler2D _MainTex;

			struct a2v
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 texcoord: TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 color : COLOR;
				float2 uv: TEXCOORD0;
				float2 position : TEXCOORD1;
			};

			v2f vert(a2v v)
			{
				v2f o;

				o.pos = UnityObjectToClipPos(v.vertex);
				
				o.uv = v.texcoord;

				o.color = v.color;
				
				o.position = v.texcoord - fixed2(0.5, 0.5);
				
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 color = tex2D(_MainTex, i.uv) * i.color;

				fixed2 center = fixed2(0.5 - _Radius, 0.5 - _Radius);

				if(abs(i.position.x) > center.x && abs(i.position.y) > center.y)
				{
					if(length(abs(i.position) - center) > _Radius)
					{
						color.a = 0;
					}
				}
				return color;  
			}
			ENDCG
		}
	}
}