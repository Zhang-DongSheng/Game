Shader "UI/CircularPro"
{
	Properties 
	{
		[PerRendererData] _MainTex ("Base (RGB)", 2D) = "white" {}
		_Radius("Radius", Range(0, 0.5)) = 0.2
		_Width("Width", Float) = 1
		_Height("Height", Float) = 1
	}
	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
		}
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
			float _Width;
			float _Height;

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
			};

			v2f vert(a2v v)
			{
				v2f o;

				o.pos = UnityObjectToClipPos(v.vertex);
				
				o.uv = v.texcoord;

				o.color = v.color;
				
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 color = tex2D(_MainTex, i.uv) * i.color;

				float ratio = _Height / _Width;

                float2 center = float2(abs(round(i.uv.x) - _Radius * ratio), abs(round(i.uv.y) - _Radius));
                
				fixed alpha = color.a * step(distance(fixed2(i.uv.x * _Width, i.uv.y * _Height), fixed2(center.x * _Width, center.y * _Height)), _Radius * _Height);

                fixed y = max(step(i.uv.y, _Radius), step((1 - _Radius), i.uv.y));

                fixed x = max(step(i.uv.x, _Radius * ratio), step((1 - _Radius * ratio), i.uv.x));

                color.a = x * (y * alpha + (1 - y) * color.a) + (1 - x) * color.a;

				return color;  
			}
			ENDCG
		}
	}
}