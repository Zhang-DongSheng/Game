// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.13 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.13;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,nrsp:0,limd:3,spmd:1,trmd:0,grmd:1,uamb:True,mssp:True,bkdf:False,rprd:True,enco:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,bsrc:0,bdst:1,culm:0,dpts:2,wrdp:True,dith:0,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:478,x:33029,y:32688,varname:node_478,prsc:2|diff-8487-OUT,spec-9143-B,gloss-1411-OUT,normal-9808-RGB,emission-1874-RGB,difocc-9143-R;n:type:ShaderForge.SFN_Tex2d,id:8615,x:31270,y:32166,ptovrint:False,ptlb:ColorMask,ptin:_ColorMask,varname:node_8615,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:9808,x:32377,y:33250,ptovrint:False,ptlb:Normal,ptin:_Normal,varname:node_9808,prsc:2,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Color,id:6805,x:31536,y:32300,ptovrint:False,ptlb:Color01,ptin:_Color01,varname:node_6805,prsc:2,glob:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Color,id:6054,x:31546,y:32585,ptovrint:False,ptlb:Color02,ptin:_Color02,varname:_node_6805_copy,prsc:2,glob:False,c1:1.014706,c2:1.014706,c3:1.014706,c4:1;n:type:ShaderForge.SFN_Color,id:1891,x:31546,y:32823,ptovrint:False,ptlb:Color03,ptin:_Color03,varname:_node_6805_copy_copy,prsc:2,glob:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:8100,x:31803,y:32299,varname:node_8100,prsc:2|A-6805-RGB,B-8615-R;n:type:ShaderForge.SFN_Multiply,id:7783,x:31815,y:32588,varname:node_7783,prsc:2|A-6054-RGB,B-8615-G;n:type:ShaderForge.SFN_Multiply,id:7942,x:31791,y:32823,varname:node_7942,prsc:2|A-1891-RGB,B-8615-B;n:type:ShaderForge.SFN_Add,id:9006,x:31997,y:32528,varname:node_9006,prsc:2|A-8100-OUT,B-7783-OUT;n:type:ShaderForge.SFN_Add,id:9946,x:32153,y:32679,varname:node_9946,prsc:2|A-9006-OUT,B-7942-OUT;n:type:ShaderForge.SFN_Lerp,id:8487,x:32576,y:32339,varname:node_8487,prsc:2|A-7921-RGB,B-9955-OUT,T-1386-RGB;n:type:ShaderForge.SFN_Tex2d,id:7921,x:32124,y:32301,ptovrint:False,ptlb:Albedo,ptin:_Albedo,varname:_node_4329_copy,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:1386,x:31974,y:32107,ptovrint:False,ptlb:WhiteMask,ptin:_WhiteMask,varname:node_1386,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Blend,id:9955,x:32321,y:32417,varname:node_9955,prsc:2,blmd:0,clmp:True|SRC-7921-RGB,DST-9946-OUT;n:type:ShaderForge.SFN_Tex2d,id:9143,x:32291,y:32801,ptovrint:False,ptlb:ORM,ptin:_ORM,varname:node_9143,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Blend,id:1411,x:32454,y:32905,varname:node_1411,prsc:2,blmd:10,clmp:True|SRC-9143-G,DST-3339-R;n:type:ShaderForge.SFN_Color,id:3339,x:32291,y:33014,ptovrint:False,ptlb:Roughness,ptin:_Roughness,varname:_Color04,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Tex2d,id:1874,x:32662,y:32996,ptovrint:False,ptlb:Emissive,ptin:_Emissive,varname:_ORM_copy,prsc:2,ntxv:2,isnm:False;proporder:6805-6054-1891-7921-9808-8615-1386-9143-3339-1874;pass:END;sub:END;*/

Shader "Shader Forge/Character" {
    Properties {
        _Color01 ("Color01", Color) = (1,1,1,1)
        _Color02 ("Color02", Color) = (1.014706,1.014706,1.014706,1)
        _Color03 ("Color03", Color) = (1,1,1,1)
        _Albedo ("Albedo", 2D) = "white" {}
        _Normal ("Normal", 2D) = "bump" {}
        _ColorMask ("ColorMask", 2D) = "white" {}
        _WhiteMask ("WhiteMask", 2D) = "white" {}
        _ORM ("ORM", 2D) = "white" {}
        _Roughness ("Roughness", Color) = (0.5,0.5,0.5,1)
        _Emissive ("Emissive", 2D) = "black" {}
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 xbox360 ps3 psp2 
            #pragma target 3.0
            uniform sampler2D _ColorMask; uniform float4 _ColorMask_ST;
            uniform sampler2D _Normal; uniform float4 _Normal_ST;
            uniform float4 _Color01;
            uniform float4 _Color02;
            uniform float4 _Color03;
            uniform sampler2D _Albedo; uniform float4 _Albedo_ST;
            uniform sampler2D _WhiteMask; uniform float4 _WhiteMask_ST;
            uniform sampler2D _ORM; uniform float4 _ORM_ST;
            uniform float4 _Roughness;
            uniform sampler2D _Emissive; uniform float4 _Emissive_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex);
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _Normal_var = UnpackNormal(tex2D(_Normal,TRANSFORM_TEX(i.uv0, _Normal)));
                float3 normalLocal = _Normal_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float4 _ORM_var = tex2D(_ORM,TRANSFORM_TEX(i.uv0, _ORM));
                float gloss = 1.0 - saturate(( _Roughness.r > 0.5 ? (1.0-(1.0-2.0*(_Roughness.r-0.5))*(1.0-_ORM_var.g)) : (2.0*_Roughness.r*_ORM_var.g) )); // Convert roughness to gloss
                float specPow = exp2( gloss * 10.0+1.0);
/////// GI Data:
                UnityLight light;
                #ifdef LIGHTMAP_OFF
                    light.color = lightColor;
                    light.dir = lightDirection;
                    light.ndotl = LambertTerm (normalDirection, light.dir);
                #else
                    light.color = half3(0.f, 0.f, 0.f);
                    light.ndotl = 0.0f;
                    light.dir = half3(0.f, 0.f, 0.f);
                #endif
                UnityGIInput d;
                UNITY_INITIALIZE_OUTPUT(UnityGIInput, d);
                d.light = light;
                d.worldPos = i.posWorld.xyz;
                d.worldViewDir = viewDirection;
                d.atten = attenuation;
#if UNITY_SPECCUBE_BOX_PROJECTION
                d.boxMax[0] = unity_SpecCube0_BoxMax;
                d.boxMin[0] = unity_SpecCube0_BoxMin;
                d.probePosition[0] = unity_SpecCube0_ProbePosition;
                d.probeHDR[0] = unity_SpecCube0_HDR;
                d.boxMax[1] = unity_SpecCube1_BoxMax;
                d.boxMin[1] = unity_SpecCube1_BoxMin;
                d.probePosition[1] = unity_SpecCube1_ProbePosition;
                d.probeHDR[1] = unity_SpecCube1_HDR;
#endif
                UnityGI gi = UnityGlobalIllumination (d, 1, gloss, normalDirection);
                lightDirection = gi.light.dir;
                lightColor = gi.light.color;
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float LdotH = max(0.0,dot(lightDirection, halfDirection));
                float4 _Albedo_var = tex2D(_Albedo,TRANSFORM_TEX(i.uv0, _Albedo));
                float4 _ColorMask_var = tex2D(_ColorMask,TRANSFORM_TEX(i.uv0, _ColorMask));
                float4 _WhiteMask_var = tex2D(_WhiteMask,TRANSFORM_TEX(i.uv0, _WhiteMask));
                float3 diffuseColor = lerp(_Albedo_var.rgb,saturate(min(_Albedo_var.rgb,(((_Color01.rgb*_ColorMask_var.r)+(_Color02.rgb*_ColorMask_var.g))+(_Color03.rgb*_ColorMask_var.b)))),_WhiteMask_var.rgb); // Need this for specular when using metallic
                float specularMonochrome;
                float3 specularColor;
                diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, _ORM_var.b, specularColor, specularMonochrome );
                specularMonochrome = 1-specularMonochrome;
                float NdotV = max(0.0,dot( normalDirection, viewDirection ));
                float NdotH = max(0.0,dot( normalDirection, halfDirection ));
                float VdotH = max(0.0,dot( viewDirection, halfDirection ));
                float visTerm = SmithBeckmannVisibilityTerm( NdotL, NdotV, 1.0-gloss );
                float normTerm = max(0.0, NDFBlinnPhongNormalizedTerm(NdotH, RoughnessToSpecPower(1.0-gloss)));
                float specularPBL = max(0, (NdotL*visTerm*normTerm) * unity_LightGammaCorrectionConsts_PIDiv4 );
                float3 directSpecular = (floor(attenuation) * _LightColor0.xyz) * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularPBL*lightColor*FresnelTerm(specularColor, LdotH);
                half grazingTerm = saturate( gloss + specularMonochrome );
                float3 indirectSpecular = (gi.indirect.specular);
                indirectSpecular *= FresnelLerp (specularColor, grazingTerm, NdotV);
                float3 specular = (directSpecular + indirectSpecular);
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float3 directDiffuse = ((1 +(fd90 - 1)*pow((1.00001-NdotL), 5)) * (1 + (fd90 - 1)*pow((1.00001-NdotV), 5)) * NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                indirectDiffuse *= _ORM_var.r; // Diffuse AO
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float4 _Emissive_var = tex2D(_Emissive,TRANSFORM_TEX(i.uv0, _Emissive));
                float3 emissive = _Emissive_var.rgb;
/// Final Color:
                float3 finalColor = diffuse + specular + emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 xbox360 ps3 psp2 
            #pragma target 3.0
            uniform sampler2D _ColorMask; uniform float4 _ColorMask_ST;
            uniform sampler2D _Normal; uniform float4 _Normal_ST;
            uniform float4 _Color01;
            uniform float4 _Color02;
            uniform float4 _Color03;
            uniform sampler2D _Albedo; uniform float4 _Albedo_ST;
            uniform sampler2D _WhiteMask; uniform float4 _WhiteMask_ST;
            uniform sampler2D _ORM; uniform float4 _ORM_ST;
            uniform float4 _Roughness;
            uniform sampler2D _Emissive; uniform float4 _Emissive_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _Normal_var = UnpackNormal(tex2D(_Normal,TRANSFORM_TEX(i.uv0, _Normal)));
                float3 normalLocal = _Normal_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float4 _ORM_var = tex2D(_ORM,TRANSFORM_TEX(i.uv0, _ORM));
                float gloss = 1.0 - saturate(( _Roughness.r > 0.5 ? (1.0-(1.0-2.0*(_Roughness.r-0.5))*(1.0-_ORM_var.g)) : (2.0*_Roughness.r*_ORM_var.g) )); // Convert roughness to gloss
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float LdotH = max(0.0,dot(lightDirection, halfDirection));
                float4 _Albedo_var = tex2D(_Albedo,TRANSFORM_TEX(i.uv0, _Albedo));
                float4 _ColorMask_var = tex2D(_ColorMask,TRANSFORM_TEX(i.uv0, _ColorMask));
                float4 _WhiteMask_var = tex2D(_WhiteMask,TRANSFORM_TEX(i.uv0, _WhiteMask));
                float3 diffuseColor = lerp(_Albedo_var.rgb,saturate(min(_Albedo_var.rgb,(((_Color01.rgb*_ColorMask_var.r)+(_Color02.rgb*_ColorMask_var.g))+(_Color03.rgb*_ColorMask_var.b)))),_WhiteMask_var.rgb); // Need this for specular when using metallic
                float specularMonochrome;
                float3 specularColor;
                diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, _ORM_var.b, specularColor, specularMonochrome );
                specularMonochrome = 1-specularMonochrome;
                float NdotV = max(0.0,dot( normalDirection, viewDirection ));
                float NdotH = max(0.0,dot( normalDirection, halfDirection ));
                float VdotH = max(0.0,dot( viewDirection, halfDirection ));
                float visTerm = SmithBeckmannVisibilityTerm( NdotL, NdotV, 1.0-gloss );
                float normTerm = max(0.0, NDFBlinnPhongNormalizedTerm(NdotH, RoughnessToSpecPower(1.0-gloss)));
                float specularPBL = max(0, (NdotL*visTerm*normTerm) * unity_LightGammaCorrectionConsts_PIDiv4 );
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularPBL*lightColor*FresnelTerm(specularColor, LdotH);
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float3 directDiffuse = ((1 +(fd90 - 1)*pow((1.00001-NdotL), 5)) * (1 + (fd90 - 1)*pow((1.00001-NdotV), 5)) * NdotL) * attenColor;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
