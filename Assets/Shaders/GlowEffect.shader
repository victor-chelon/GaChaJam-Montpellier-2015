// Shader created with Shader Forge Beta 0.34 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.34;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,blpr:0,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:True,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32564,y:32631|emission-2-OUT;n:type:ShaderForge.SFN_Lerp,id:2,x:32843,y:32715|A-3-RGB,B-5-OUT,T-10-OUT;n:type:ShaderForge.SFN_Color,id:3,x:33185,y:32568,ptlb:baseColor,ptin:_baseColor,glob:False,c1:0.5661765,c2:0.5661765,c3:0.5661765,c4:0;n:type:ShaderForge.SFN_Color,id:4,x:33340,y:32756,ptlb:glowColor,ptin:_glowColor,glob:False,c1:0.9034482,c2:1,c3:0,c4:1;n:type:ShaderForge.SFN_Multiply,id:5,x:33051,y:32781|A-4-RGB,B-6-OUT;n:type:ShaderForge.SFN_ValueProperty,id:6,x:33352,y:32936,ptlb:glowIntensity,ptin:_glowIntensity,glob:False,v1:2.33;n:type:ShaderForge.SFN_Multiply,id:10,x:33018,y:33059|A-15-OUT,B-16-OUT;n:type:ShaderForge.SFN_Fresnel,id:11,x:33383,y:33065|NRM-12-OUT,EXP-14-OUT;n:type:ShaderForge.SFN_NormalVector,id:12,x:33608,y:33055,pt:False;n:type:ShaderForge.SFN_ValueProperty,id:14,x:33593,y:33246,ptlb:lightReflection,ptin:_lightReflection,glob:False,v1:1;n:type:ShaderForge.SFN_OneMinus,id:15,x:33201,y:33065|IN-11-OUT;n:type:ShaderForge.SFN_Slider,id:16,x:33123,y:33288,ptlb:Slider,ptin:_Slider,min:0,cur:0.9705178,max:1;proporder:3-4-6-14-16;pass:END;sub:END;*/

Shader "Shader Forge/GlowEffect" {
    Properties {
        _baseColor ("baseColor", Color) = (0.5661765,0.5661765,0.5661765,0)
        _glowColor ("glowColor", Color) = (0.9034482,1,0,1)
        _glowIntensity ("glowIntensity", Float ) = 2.33
        _lightReflection ("lightReflection", Float ) = 1
        _Slider ("Slider", Range(0, 1)) = 0.9705178
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _baseColor;
            uniform float4 _glowColor;
            uniform float _glowIntensity;
            uniform float _lightReflection;
            uniform float _Slider;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.normalDir = mul(float4(v.normal,0), _World2Object).xyz;
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
/////// Normals:
                float3 normalDirection =  i.normalDir;
////// Lighting:
////// Emissive:
                float3 emissive = lerp(_baseColor.rgb,(_glowColor.rgb*_glowIntensity),((1.0 - pow(1.0-max(0,dot(i.normalDir, viewDirection)),_lightReflection))*_Slider));
                float3 finalColor = emissive;
/// Final Color:
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
