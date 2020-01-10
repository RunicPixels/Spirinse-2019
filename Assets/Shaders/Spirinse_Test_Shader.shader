// Toony Colors Pro+Mobile 2
// (c) 2014-2019 Jean Moreno

Shader "Spirinse_Test_Shader"
{
	Properties
	{
	[TCP2HeaderHelp(BASE, Base Properties)]
		//TOONY COLORS
		_Color ("Color", Color) = (1,1,1,1)
		_HColor ("Highlight Color", Color) = (0.785,0.785,0.785,1.0)
		_SColor ("Shadow Color", Color) = (0.195,0.195,0.195,1.0)

	[Header(Shadow HSV)]
		_Shadow_HSV_H ("Hue", Range(-360,360)) = 0
		_Shadow_HSV_S ("Saturation", Range(-1,1)) = 0
		_Shadow_HSV_V ("Value", Range(-1,1)) = 0
	[TCP2Separator]
		_WrapFactor ("Light Wrapping", Range(-1,3)) = 1.0

		//DIFFUSE
		_MainTex ("Main Texture", 2D) = "white" {}
		_Detail ("Detail (RGB)", 2D) = "gray" {}
		_DiffTint ("Diffuse Tint", Color) = (0.7,0.8,1,1)
	[TCP2Separator]

		//TOONY COLORS RAMP
		[TCP2Header(RAMP SETTINGS)]

		[Header(Main Directional Light)]
		_RampThreshold ("Ramp Threshold", Range(0,1)) = 0.5
		_RampSmooth ("Ramp Smoothing", Range(0.001,1)) = 0.1
		[Header(Other Lights)]
		_RampThresholdOtherLights ("Threshold", Range(0,1)) = 0.5
		_RampSmoothOtherLights ("Smoothing", Range(0.001,1)) = 0.5
		[Space]
		_ThresholdTex ("Threshold Texture (Alpha)", 2D) = "gray" {}
	[TCP2Separator]
	[Header(HSV Controls)]
		[Header(Main Texture)]
		_HSV_H ("Hue", Range(-360,360)) = 0
		_HSV_S ("Saturation", Range(-1,1)) = 0
		_HSV_V ("Value", Range(-1,1)) = 0
		[Header(Final Color)]
		_Final_HSV_H ("Hue", Range(-360,360)) = 0
		_Final_HSV_S ("Saturation", Range(-4,4)) = 0
		_Final_HSV_V ("Value", Range(-1,1)) = 0
	[TCP2Separator]

	[TCP2HeaderHelp(EMISSION, Emission)]
		[NoScaleOffset] _EmissionMap ("Emission (RGB)", 2D) = "black" {}
		[HDR] _EmissionColor ("Emission Color", Color) = (1,1,1,1.0)
	[TCP2Separator]

	[TCP2HeaderHelp(NORMAL MAPPING, Normal Bump Map)]
		//BUMP
		_BumpMap ("Normal map (RGB)", 2D) = "bump" {}
	[TCP2Separator]

	[TCP2HeaderHelp(SPECULAR, Specular)]
		//SPECULAR
		_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		_Smoothness ("Size", Float) = 0.2
	[TCP2Separator]

	[TCP2HeaderHelp(RIM, Rim)]
		//RIM LIGHT
		_RimColor ("Rim Color", Color) = (0.8,0.8,0.8,0.6)
		_RimMin ("Rim Min", Range(0,2)) = 0.5
		_RimMax ("Rim Max", Range(0,2)) = 1.0
	[TCP2Separator]

	[TCP2HeaderHelp(SKETCH, Sketch)]
		//SKETCH
		_SketchTex ("Sketch (Alpha)", 2D) = "white" {}
		_SketchSpeed ("Sketch Anim Speed", Range(1.1, 10)) = 6
		_SketchColor ("Sketch Color (RGB)", Color) = (0,0,0,1)
		_SketchHalftoneMin ("Sketch Halftone Min", Range(0,1)) = 0.2
		_SketchHalftoneMax ("Sketch Halftone Max", Range(0,1)) = 1.0
	[TCP2Separator]

	[TCP2HeaderHelp(DISSOLVE)]
		[NoScaleOffset]
		_DissolveMap ("Dissolve Map", 2D) = "white" {}
		_DissolveValue ("Dissolve Value", Range(0,1)) = 0.5
	[TCP2Separator]

	[TCP2HeaderHelp(OUTLINE, Outline)]
		//OUTLINE
		_OutlineColor ("Outline Color", Color) = (0.2, 0.2, 0.2, 1.0)
		_Outline ("Outline Width", Float) = 1

		//Outline Textured
		[Toggle(TCP2_OUTLINE_TEXTURED)] _EnableTexturedOutline ("Color from Texture", Float) = 0
		[TCP2KeywordFilter(TCP2_OUTLINE_TEXTURED)] _TexLod ("Texture LOD", Range(0,10)) = 5

		//Constant-size outline
		[Toggle(TCP2_OUTLINE_CONST_SIZE)] _EnableConstSizeOutline ("Constant Size Outline", Float) = 0

		//ZSmooth
		[Toggle(TCP2_ZSMOOTH_ON)] _EnableZSmooth ("Correct Z Artefacts", Float) = 0
		//Z Correction & Offset
		[TCP2KeywordFilter(TCP2_ZSMOOTH_ON)] _ZSmooth ("Z Correction", Range(-3.0,3.0)) = -0.5
		[TCP2KeywordFilter(TCP2_ZSMOOTH_ON)] _Offset1 ("Z Offset 1", Float) = 0
		[TCP2KeywordFilter(TCP2_ZSMOOTH_ON)] _Offset2 ("Z Offset 2", Float) = 0

		//This property will be ignored and will draw the custom normals GUI instead
		[TCP2OutlineNormalsGUI] __outline_gui_dummy__ ("_unused_", Float) = 0
	[TCP2Separator]


		//Avoid compile error if the properties are ending with a drawer
		[HideInInspector] __dummy__ ("unused", Float) = 0
	}

	SubShader
	{
		//================================================================
		// OUTLINE INCLUDE

		CGINCLUDE

		#include "UnityCG.cginc"

		struct a2v
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
	#if TCP2_OUTLINE_TEXTURED
			float3 texcoord : TEXCOORD0;
	#endif
		#if TCP2_COLORS_AS_NORMALS
			float4 color : COLOR;
		#endif
	#if TCP2_UV2_AS_NORMALS
			float2 uv2 : TEXCOORD1;
	#endif
	#if TCP2_TANGENT_AS_NORMALS
			float4 tangent : TANGENT;
	#endif
	#if UNITY_VERSION >= 550
			UNITY_VERTEX_INPUT_INSTANCE_ID
	#endif
		};

		struct v2f
		{
			float4 pos : SV_POSITION;
	#if TCP2_OUTLINE_TEXTURED
			float3 texlod : TEXCOORD1;
	#endif
			UNITY_VERTEX_OUTPUT_STEREO
		};

		float _Outline;
		float _ZSmooth;
		fixed4 _OutlineColor;

	#if TCP2_OUTLINE_TEXTURED
		sampler2D _MainTex;
		float4 _MainTex_ST;
		float _TexLod;
	#endif

		#define OUTLINE_WIDTH _Outline

		v2f TCP2_Outline_Vert(a2v v)
		{
			v2f o;
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

	#if UNITY_VERSION >= 550
			//GPU instancing support
			UNITY_SETUP_INSTANCE_ID(v);
	#endif


	#if TCP2_ZSMOOTH_ON
			float4 pos = float4(UnityObjectToViewPos(v.vertex), 1.0);
	#endif

	#ifdef TCP2_COLORS_AS_NORMALS
			//Vertex Color for Normals
			float3 normal = (v.color.xyz*2) - 1;
	#elif TCP2_TANGENT_AS_NORMALS
			//Tangent for Normals
			float3 normal = v.tangent.xyz;
	#elif TCP2_UV2_AS_NORMALS
			//UV2 for Normals
			float3 n;
			//unpack uv2
			v.uv2.x = v.uv2.x * 255.0/16.0;
			n.x = floor(v.uv2.x) / 15.0;
			n.y = frac(v.uv2.x) * 16.0 / 15.0;
			//get z
			n.z = v.uv2.y;
			//transform
			n = n*2 - 1;
			float3 normal = n;
	#else
			float3 normal = v.normal;
	#endif

	#if TCP2_ZSMOOTH_ON
			//Correct Z artefacts
			normal = UnityObjectToViewPos(normal);
			normal.z = -_ZSmooth;
	#endif

	#ifdef TCP2_OUTLINE_CONST_SIZE
			//Camera-independent outline size
			float dist = distance(_WorldSpaceCameraPos, mul(unity_ObjectToWorld, v.vertex));
			#define SIZE	dist
	#else
			#define SIZE	1.0
	#endif

	#if TCP2_ZSMOOTH_ON
			o.pos = mul(UNITY_MATRIX_P, pos + float4(normalize(normal),0) * OUTLINE_WIDTH * 0.01 * SIZE);
	#else
			o.pos = UnityObjectToClipPos(v.vertex + float4(normal,0) * OUTLINE_WIDTH * 0.01 * SIZE);
	#endif

	#if TCP2_OUTLINE_TEXTURED
			half2 uv = TRANSFORM_TEX(v.texcoord, _MainTex);
			o.texlod = tex2Dlod(_MainTex, float4(uv, 0, _TexLod)).rgb;
	#endif

			return o;
		}

		#define OUTLINE_COLOR _OutlineColor

		float4 TCP2_Outline_Frag (v2f IN) : SV_Target
		{
	#if TCP2_OUTLINE_TEXTURED
			return float4(IN.texlod, 1) * OUTLINE_COLOR;
	#else
			return OUTLINE_COLOR;
	#endif
		}

		ENDCG

		// OUTLINE INCLUDE END
		//================================================================

		Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}

		CGPROGRAM

		#pragma surface surf ToonyColorsCustom finalcolor:fcolor vertex:vert exclude_path:deferred exclude_path:prepass
		#pragma target 3.0

		//================================================================
		// VARIABLES

		fixed4 _Color;
		sampler2D _MainTex;
		float _Shadow_HSV_H;
		float _Shadow_HSV_S;
		float _Shadow_HSV_V;
		float _Final_HSV_H;
		float _Final_HSV_S;
		float _Final_HSV_V;
		float _HSV_H;
		float _HSV_S;
		float _HSV_V;
		sampler2D _Detail;
		sampler2D _ThresholdTex;
		sampler2D _DissolveMap;
		half _DissolveValue;
		half4 _EmissionColor;
		sampler2D _EmissionMap;
		fixed _SketchSpeed;
		sampler2D _BumpMap;
		fixed _Smoothness;
		fixed4 _RimColor;
		fixed _RimMin;
		fixed _RimMax;
		float4 _RimDir;
		fixed4 _Random;

		#define UV_MAINTEX uv_MainTex

		struct Input
		{
			half2 uv_MainTex;
			half2 uv2_Detail;
			half2 uv_BumpMap;
			float3 viewDir;
			half4 sketchUv;
			half2 uv_ThresholdTex;
		};

		//================================================================
		// HSV HELPERS
		// source: http://lolengine.net/blog/2013/07/27/rgb-to-hsv-in-glsl

		float3 rgb2hsv(float3 c)
		{
			float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
			float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
			float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));

			float d = q.x - min(q.w, q.y);
			float e = 1.0e-10;
			return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
		}

		float3 hsv2rgb(float3 c)
		{
			c.g = max(c.g, 0.0); //make sure that saturation value is positive
			float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
			float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
			return c.z * lerp(K.xxx, saturate(p - K.xxx), c.y);
		}

		//================================================================
		// CUSTOM LIGHTING

		//Lighting-related variables
		fixed4 _HColor;
		fixed4 _SColor;
		half _WrapFactor;
		half _RampThreshold;
		half _RampSmooth;
		half _RampThresholdOtherLights;
		half _RampSmoothOtherLights;
		sampler2D _SketchTex;
		float4 _SketchTex_ST;
		fixed4 _SketchColor;
		fixed _SketchHalftoneMin;
		fixed _SketchHalftoneMax;
		fixed4 _DiffTint;

		// Instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		//Custom SurfaceOutput
		struct SurfaceOutputCustom
		{
			half atten;
			fixed3 Albedo;
			fixed3 Normal;
			fixed3 Emission;
			half Specular;
			fixed Gloss;
			fixed Alpha;
			half2 ScreenUVs;
			fixed TexThreshold;
		};

		inline half4 LightingToonyColorsCustom (inout SurfaceOutputCustom s, half3 viewDir, UnityGI gi)
		{
		#define IN_NORMAL s.Normal
	
			half3 lightDir = gi.light.dir;
		#if defined(UNITY_PASS_FORWARDBASE)
			half3 lightColor = _LightColor0.rgb;
			half atten = s.atten;
		#else
			half3 lightColor = gi.light.color.rgb;
			half atten = 1;
		#endif

			IN_NORMAL = normalize(IN_NORMAL);
			fixed ndl = max(0, (dot(IN_NORMAL, lightDir) + _WrapFactor) / (1+_WrapFactor));
			#define NDL ndl
			NDL += s.TexThreshold;

		#if defined(UNITY_PASS_FORWARDBASE)
			#define		RAMP_THRESHOLD	_RampThreshold
			#define		RAMP_SMOOTH		_RampSmooth
		#else
			#define		RAMP_THRESHOLD	_RampThresholdOtherLights
			#define		RAMP_SMOOTH		_RampSmoothOtherLights
		#endif

			fixed3 ramp = smoothstep(RAMP_THRESHOLD - RAMP_SMOOTH*0.5, RAMP_THRESHOLD + RAMP_SMOOTH*0.5, NDL);
		#if !(POINT) && !(SPOT)
			ramp *= atten;
		#endif
			//Sketch
			#define SKETCH_RGB	sketch
			fixed sketch = tex2D(_SketchTex, s.ScreenUVs).a;
			sketch = smoothstep(sketch - 0.2, sketch, clamp(ramp, _SketchHalftoneMin, _SketchHalftoneMax));	//Gradient halftone

			//Shadow Hsv
			float3 albedoHsv = rgb2hsv(s.Albedo.rgb);
			albedoHsv += float3(_Shadow_HSV_H/360,_Shadow_HSV_S,_Shadow_HSV_V);
			s.Albedo = lerp(hsv2rgb(albedoHsv), s.Albedo, ramp);
			_SColor = lerp(_HColor, _SColor, _SColor.a);	//Shadows intensity through alpha
			ramp = lerp(_SColor.rgb, _HColor.rgb, ramp);
			fixed3 wrappedLight = saturate(_DiffTint.rgb + saturate(dot(IN_NORMAL, lightDir)));
			ramp *= wrappedLight;
			//Blinn-Phong Specular (legacy)
			half3 h = normalize(lightDir + viewDir);
			float ndh = max(0, dot (IN_NORMAL, h));
			float spec = pow(ndh, s.Specular*128.0) * s.Gloss * 2.0;
			spec *= atten;
			fixed4 c;
			c.rgb = s.Albedo * lightColor.rgb * ramp;
		#if (POINT || SPOT)
			c.rgb *= atten;
		#endif

			#define SPEC_COLOR	_SpecColor.rgb
			c.rgb += lightColor.rgb * SPEC_COLOR * spec;
			c.a = s.Alpha;
			c.rgb *= lerp(_SketchColor.rgb, fixed3(1,1,1), sketch);

		#ifdef UNITY_LIGHT_FUNCTION_APPLY_INDIRECT
			c.rgb += s.Albedo * gi.indirect.diffuse;
		#endif

			return c;
		}

		void LightingToonyColorsCustom_GI(inout SurfaceOutputCustom s, UnityGIInput data, inout UnityGI gi)
		{
			gi = UnityGlobalIllumination(data, 1.0, IN_NORMAL);

			s.atten = data.atten;	//transfer attenuation to lighting function
			gi.light.color = _LightColor0.rgb;	//remove attenuation
		}

		//Adjust screen UVs relative to object to prevent screen door effect
		inline void ObjSpaceUVOffset(inout float2 screenUV, in float screenRatio)
		{
			// UNITY_MATRIX_P._m11 = Camera FOV
			float4 objPos = float4(-UNITY_MATRIX_T_MV[3].x * screenRatio * UNITY_MATRIX_P._m11, -UNITY_MATRIX_T_MV[3].y * UNITY_MATRIX_P._m11, UNITY_MATRIX_T_MV[3].z, UNITY_MATRIX_T_MV[3].w);

			float offsetFactorX = 0.5;
			float offsetFactorY = offsetFactorX * screenRatio;
			offsetFactorX *= _SketchTex_ST.x;
			offsetFactorY *= _SketchTex_ST.y;

			if (unity_OrthoParams.w < 1)	//don't scale with orthographic camera
			{
				//adjust uv scale
				screenUV -= float2(offsetFactorX, offsetFactorY);
				screenUV *= objPos.z;	//scale with cam distance
				screenUV += float2(offsetFactorX, offsetFactorY);

				// sign(UNITY_MATRIX_P[1].y) is different in Scene and Game views
				screenUV.x -= objPos.x * offsetFactorX * sign(UNITY_MATRIX_P[1].y);
				screenUV.y -= objPos.y * offsetFactorY * sign(UNITY_MATRIX_P[1].y);
			}
			else
			{
				// sign(UNITY_MATRIX_P[1].y) is different in Scene and Game views
				screenUV.x += objPos.x * offsetFactorX * sign(UNITY_MATRIX_P[1].y);
				screenUV.y += objPos.y * offsetFactorY * sign(UNITY_MATRIX_P[1].y);
			}
		}

		//Vertex input
		struct appdata_tcp2
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float4 texcoord : TEXCOORD0;
			float4 texcoord1 : TEXCOORD1;
			float4 texcoord2 : TEXCOORD2;
			float4 tangent : TANGENT;
	#if UNITY_VERSION >= 550
			UNITY_VERTEX_INPUT_INSTANCE_ID
	#endif
		};

		//================================================================
		// VERTEX FUNCTION

		void vert(inout appdata_tcp2 v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);

			//Sketch
			float4 pos = UnityObjectToClipPos(v.vertex);
			o.sketchUv = ComputeScreenPos(pos);
			o.sketchUv.xy = TRANSFORM_TEX(o.sketchUv, _SketchTex);
		}

		//================================================================
		// SURFACE FUNCTION

		void surf(Input IN, inout SurfaceOutputCustom o)
		{
			fixed4 mainTex = tex2D(_MainTex, IN.UV_MAINTEX);

			//Hsv
			float3 mainTexHSV = rgb2hsv(mainTex.rgb);
			mainTexHSV += float3(_HSV_H/360,_HSV_S,_HSV_V);
			mainTex.rgb = hsv2rgb(mainTexHSV);

			//Detail Tex
			fixed4 detail = tex2D(_Detail, IN.uv2_Detail);
			mainTex.rgb *= (detail.rgb * 2.0);
			o.Albedo = mainTex.rgb * _Color.rgb;
			o.Alpha = mainTex.a * _Color.a;

			//Dissolve
			fixed4 dslv = tex2D(_DissolveMap, IN.UV_MAINTEX.xy);
			#define DSLV dslv.a
			float dissValue = _DissolveValue;
			o.Alpha *= DSLV - dissValue;

			//Sketch
			float2 screenUV = IN.sketchUv.xy / IN.sketchUv.w;
			float screenRatio = _ScreenParams.y / _ScreenParams.x;
			screenUV.y *= screenRatio;
			ObjSpaceUVOffset(screenUV, screenRatio);
			_Random.x = round(_Time.z * _SketchSpeed) / _SketchSpeed;
			_Random.y = -round(_Time.z * _SketchSpeed) / _SketchSpeed;
			screenUV.xy += frac(_Random.xy);
			o.ScreenUVs = screenUV;

			//Specular
			o.Gloss = 1;
			o.Specular = _Smoothness;

			//Normal map
			half4 normalMap = tex2D(_BumpMap, IN.uv_BumpMap.xy);
			o.Normal = UnpackNormal(normalMap);

			//Rim
			float3 viewDir = normalize(IN.viewDir);
			half rim = 1.0f - saturate( dot(viewDir, o.Normal) );
			rim = smoothstep(_RimMin, _RimMax, rim);
			o.Emission += (_RimColor.rgb * rim) * _RimColor.a;
			o.Albedo = lerp(o.Albedo.rgb, _RimColor.rgb, rim * _RimColor.a);

			//Emission
			half3 emissiveColor = half3(1,1,1);
			emissiveColor *= tex2D(_EmissionMap, IN.UV_MAINTEX);
			emissiveColor *= _EmissionColor.rgb * _EmissionColor.a;
			o.Emission += emissiveColor;

			//Textured Threshold
			o.TexThreshold = tex2D(_ThresholdTex, IN.uv_ThresholdTex).a - 0.5;
		}

		//================================================================
		// FINAL COLOR MODIFIER

		void fcolor(Input IN, SurfaceOutputCustom o, inout fixed4 color)
		{

			//Hsv
			float3 colorHsv = rgb2hsv(color.rgb);
			colorHsv += float3(_Final_HSV_H/360,_Final_HSV_S,_Final_HSV_V);
			color.rgb = hsv2rgb(colorHsv);
		}

		ENDCG

		//Outline
		Pass
		{
			Cull Front
			Offset [_Offset1],[_Offset2]

			Tags { "LightMode"="ForwardBase" "IgnoreProjectors"="True" }

			CGPROGRAM

			#pragma vertex TCP2_Outline_Vert
			#pragma fragment TCP2_Outline_Frag

			#pragma multi_compile TCP2_NONE TCP2_ZSMOOTH_ON
			#pragma multi_compile TCP2_NONE TCP2_OUTLINE_CONST_SIZE
			#pragma multi_compile TCP2_NONE TCP2_COLORS_AS_NORMALS TCP2_TANGENT_AS_NORMALS TCP2_UV2_AS_NORMALS
			#pragma multi_compile TCP2_NONE TCP2_OUTLINE_TEXTURED			
			#pragma multi_compile_instancing


			#pragma target 3.0

			ENDCG
		}
	}

	Fallback "Diffuse"
	CustomEditor "TCP2_MaterialInspector_SG"
}
