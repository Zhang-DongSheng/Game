Shader "UI/Effect"
{
	Properties 
	{
		_TintColor ("Tint Color", Color) = (0.5, 0.5, 0.5, 0.5)
		_MainTex ("Particle Texture (A = Transparency)", 2D) = "white" {}
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
		
		//新增 记录裁剪框的四个边界的值
		_Area ("Area", Vector) = (0, 0, 1, 1)
		//----end----
	}
 
	Category 
	{
		Tags 
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off Lighting Off ZWrite Off
		BindChannels 
		{
			Bind "Color", color
			Bind "Vertex", vertex
			Bind "TexCoord", texcoord
		}
		
		SubShader 
		{
			Pass 
			{
			
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma multi_compile_particles
	
				#include "UnityCG.cginc"
	
				sampler2D _MainTex;
				fixed4 _TintColor;
	
				//新增，对应上面的_Area
				float4 _Area;
				//----end----
				
				struct appdata_t 
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
				};
	
				struct v2f 
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
					//新增，记录顶点的世界坐标
					float2 worldPos : TEXCOORD1;
					//----end----
				};
				
				float4 _MainTex_ST;
	
				v2f vert (appdata_t v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.color = v.color;
					o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
					//新增，计算顶点的世界坐标
					o.worldPos = mul(unity_ObjectToWorld, v.vertex).xy;
					//----end----
					return o;
				}
	
				sampler2D _CameraDepthTexture;
				float _InvFade;
				
				fixed4 frag (v2f i) : COLOR
				{
					//新增，判断顶点坐标是否在裁剪框内
					bool inArea = i.worldPos.x >= _Area.x && i.worldPos.x <= _Area.z && i.worldPos.y >= _Area.y && i.worldPos.y <= _Area.w;
					//----end----
 
					//如果在裁剪框内return原本的效果，否则即隐藏
	                return inArea? 2.0f * i.color * _TintColor * tex2D(_MainTex, i.texcoord) : fixed4(0,0,0,0);
				}
				ENDCG 
			}
		} 
	}   
}