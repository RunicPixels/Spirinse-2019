
// Toony Colors Pro+Mobile 2
// (c) 2014,2015 Jean Moreno


Shader "Hidden/Toony Colors Pro 2/Variants/Desktop Specular Reflection"
{
	Properties
	{
		//TOONY COLORS
		_Color ("Color", Color) = (1.0,1.0,1.0,1.0)
		_HColor ("Highlight Color", Color) = (0.6,0.6,0.6,1.0)
		_SColor ("Shadow Color", Color) = (0.4,0.4,0.4,1.0)
		
		//DIFFUSE
		_MainTex ("Main Texture (RGB) Spec/Refl Mask (A) ", 2D) = "white" {}
		
		//TOONY COLORS RAMP
		_Ramp ("#RAMPT# Toon Ramp (RGB)", 2D) = "gray" {}
		_RampThreshold ("#RAMPF# Ramp Threshold", Range(0,1)) = 0.5
		_RampSmooth ("#RAMPF# Ramp Smoothing", Range(0.01,1)) = 0.1
		
		//BUMP
		_BumpMap ("#NORM# Normal map (RGB)", 2D) = "bump" {}
		
		//SPECULAR
		_SpecColor ("#SPEC# Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		_Shininess ("#SPEC# Shininess", Range(0.01,2)) = 0.1
		_SpecSmooth ("#SPECT# Smoothness", Range(0,1)) = 0.05
		
		//REFLECTION
		_Cube ("#REFL# Reflection Cubemap", Cube) = "_Skybox" {}
		_ReflectColor ("#REFL# Reflection Color (RGB) Strength (Alpha)", Color) = (1,1,1,0.5)
		
	}
	
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		
		#include "../Include/TCP2_Include.cginc"
		
		#pragma surface surf ToonyColorsSpec 
		#pragma target 3.0
		#pragma glsl
		
		#pragma multi_compile TCP2_RAMPTEXT_OFF TCP2_RAMPTEXT
		#pragma multi_compile TCP2_BUMP_OFF TCP2_BUMP
		#pragma multi_compile TCP2_SPEC TCP2_SPEC_TOON
		#pragma multi_compile TCP2_REFLECTION TCP2_REFLECTION_MASKED
		#pragma multi_compile TCP2_LIGHTMAP_OFF TCP2_LIGHTMAP
		
		//================================================================
		// VARIABLES
		
		fixed4 _Color;
		sampler2D _MainTex;
		
	#if TCP2_BUMP
		sampler2D _BumpMap;
	#endif
		fixed _Shininess;
	#if TCP2_REFLECTION || TCP2_REFLECTION_MASKED
		samplerCUBE _Cube;
		fixed4 _ReflectColor;
	#endif
		
		struct Input
		{
			half2 uv_MainTex : TEXCOORD0;
	#if TCP2_BUMP
			half2 uv_BumpMap : TEXCOORD1;
	#endif
	#if TCP2_REFLECTION || TCP2_REFLECTION_MASKED
			float3 worldRefl;
		#if TCP2_BUMP
			INTERNAL_DATA
		#endif
	#endif
		};
		
		//================================================================
		// SURFACE FUNCTION
		
		void surf (Input IN, inout SurfaceOutput o)
		{
			half4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb * _Color.rgb;
			o.Alpha = c.a * _Color.a;
			
			//Specular
			o.Gloss = c.a;
			o.Specular = _Shininess;
	#if TCP2_BUMP
			//Normal map
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
	#endif
	#if TCP2_REFLECTION || TCP2_REFLECTION_MASKED
			float3 worldRefl = WorldReflectionVector(IN, o.Normal);
			fixed4 reflColor = texCUBE(_Cube, worldRefl);
		#if TCP2_REFLECTION_MASKED
			reflColor.rgb *= c.a;
		#endif
			reflColor.rgb *= _ReflectColor.rgb * _ReflectColor.a;
			o.Emission += reflColor.rgb;
	#endif
		}
		
		ENDCG
		
	}
	
	Fallback "Diffuse"
	CustomEditor "TCP2_MaterialInspector"
}