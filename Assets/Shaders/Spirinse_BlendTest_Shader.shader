// Toony Colors Pro+Mobile 2
// (c) 2014-2019 Jean Moreno

Shader "Spirinse_BlendTest_Shader"
{
	Properties
	{
		[TCP2HeaderHelp(Base)]
		_Color ("Color", Color) = (1,1,1,1)
		[TCP2ColorNoAlpha] _HColor ("Highlight Color", Color) = (0.75,0.75,0.75,1)
		[TCP2ColorNoAlpha] _SColor ("Shadow Color", Color) = (0.2,0.2,0.2,1)
		[HideInInspector] __BeginGroup_ShadowHSV ("Shadow HSV", Float) = 0
		_Shadow_HSV_H ("Hue", Range(-180,180)) = 0
		_Shadow_HSV_S ("Saturation", Range(-1,1)) = 0
		_Shadow_HSV_V ("Value", Range(-1,1)) = 0
		[HideInInspector] __EndGroup ("Shadow HSV", Float) = 0
		_MainTex ("Albedo", 2D) = "white" {}
		[TCP2Separator]

		[TCP2Header(Ramp Shading)]
		_RampThreshold ("Threshold", Range(0.01,1)) = 0.5
		_RampSmoothing ("Smoothing", Range(0.001,1)) = 0.1
		[TCP2Separator]
		
		[TCP2HeaderHelp(Specular)]
		[Toggle(TCP2_SPECULAR)] _UseSpecular ("Enable Specular", Float) = 0
		[TCP2ColorNoAlpha] _SpecularColor ("Specular Color", Color) = (0.5,0.5,0.5,1)
		_SpecularToonSize ("Toon Size", Range(0,1)) = 0.25
		_SpecularToonSmoothness ("Toon Smoothness", Range(0.001,0.5)) = 0.05
		[TCP2Separator]

		[TCP2HeaderHelp(Emission)]
		[TCP2ColorNoAlpha] [HDR] _Emission ("Emission Color", Color) = (0,0,0,1)
		[TCP2Separator]
		
		[TCP2HeaderHelp(Rim Lighting)]
		[Toggle(TCP2_RIM_LIGHTING)] _UseRim ("Enable Rim Lighting", Float) = 0
		[TCP2ColorNoAlpha] _RimColor ("Rim Color", Color) = (0.8,0.8,0.8,0.5)
		_RimMinVert ("Rim Min", Range(0,2)) = 0.5
		_RimMaxVert ("Rim Max", Range(0,2)) = 1
		//Rim Direction
		_RimDirVert ("Rim Direction", Vector) = (0,0,1,1)
		[TCP2Separator]
		
		[TCP2HeaderHelp(MatCap)]
		[Toggle(TCP2_MATCAP)] _UseMatCap ("Enable MatCap", Float) = 0
		[NoScaleOffset] _MatCapTex ("MatCap (RGB)", 2D) = "white" {}
		[TCP2ColorNoAlpha] _MatCapColor ("MatCap Color", Color) = (1,1,1,1)
		[TCP2Separator]
		[Header(Ambient Lighting)]
		[Toggle(TCP2_AMBIENT)] _UseAmbient ("Enable Ambient/Indirect Diffuse", Float) = 0
		_TCP2_AMBIENT_RIGHT ("+X (Right)", Color) = (0,0,0,1)
		_TCP2_AMBIENT_LEFT ("-X (Left)", Color) = (0,0,0,1)
		_TCP2_AMBIENT_TOP ("+Y (Top)", Color) = (0,0,0,1)
		_TCP2_AMBIENT_BOTTOM ("-Y (Bottom)", Color) = (0,0,0,1)
		_TCP2_AMBIENT_FRONT ("+Z (Front)", Color) = (0,0,0,1)
		_TCP2_AMBIENT_BACK ("-Z (Back)", Color) = (0,0,0,1)
		[TCP2Separator]
		
		[TCP2HeaderHelp(Texture Blending)]
		[NoScaleOffset] _BlendingSource ("Blending Source", 2D) = "black" {}
		_BlendTex1 ("Texture 1", 2D) = "white" {}
		_BlendTex2 ("Texture 2", 2D) = "white" {}
		_BlendingContrast ("Blending Contrast", Vector) = (1,1,1,0)
		[TCP2Separator]
		
		[Toggle(TCP2_TEXTURED_THRESHOLD)] _UseTexturedThreshold ("Enable Textured Threshold", Float) = 0
		_StylizedThreshold ("Stylized Threshold", 2D) = "gray" {}
		[TCP2Separator]
		
		[TCP2ColorNoAlpha] _DiffuseTint ("Diffuse Tint", Color) = (1,0.5,0,1)
		[NoScaleOffset] _DiffuseTintMask ("Diffuse Tint Mask", 2D) = "white" {}
		[TCP2Separator]
		
		[TCP2HeaderHelp(Sketch)]
		[Toggle(TCP2_SKETCH)] _UseSketch ("Enable Sketch Effect", Float) = 0
		_SketchTexture ("Sketch Texture", 2D) = "black" {}
		_SketchTexture_OffsetSpeed ("Sketch Texture UV Offset Speed", Float) = 120
		[TCP2Separator]
		
		[TCP2HeaderHelp(Dissolve)]
		[Toggle(TCP2_DISSOLVE)] _UseDissolve ("Enable Dissolve", Float) = 0
		[NoScaleOffset] _DissolveMap ("Map", 2D) = "gray" {}
		_DissolveValue ("Value", Range(0,1)) = 0.5
		[NoScaleOffset] _DissolveGradientTexture ("Gradient Texture", 2D) = "gray" {}
		_DissolveGradientWidth ("Ramp Width", Range(0,1)) = 0.2
		[TCP2Separator]
		
		[TCP2HeaderHelp(Outline)]
		_OutlineWidth ("Width", Range(0.1,4)) = 1
		_OutlineColorVertex ("Color", Color) = (0,0,0,1)
		//This property will be ignored and will draw the custom normals GUI instead
		[TCP2OutlineNormalsGUI] __outline_gui_dummy__ ("_unused_", Float) = 0
		[TCP2Separator]

		//Avoid compile error if the properties are ending with a drawer
		[HideInInspector] __dummy__ ("unused", Float) = 0
	}

	SubShader
	{
		Tags
		{
			"RenderType"="Opaque"
			"Queue"="AlphaTest+25"
		}

		CGINCLUDE
		
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
		
		float3 ApplyHSV_3(float3 color, float h, float s, float v)
		{
			float3 hsv = rgb2hsv(color.rgb);
			hsv += float3(h/360,s,v);
			return hsv2rgb(hsv);
		}
		float3 ApplyHSV_3(float color, float h, float s, float v) { return ApplyHSV_3(color.xxx, h, s ,v); }
		
		float4 ApplyHSV_4(float4 color, float h, float s, float v)
		{
			float3 hsv = rgb2hsv(color.rgb);
			hsv += float3(h/360,s,v);
			return float4(hsv2rgb(hsv), color.a);
		}
		float4 ApplyHSV_4(float color, float h, float s, float v) { return ApplyHSV_4(color.xxxx, h, s, v); }
		
		// Hash without sin and uniform across platforms
		// Adapted from: https://www.shadertoy.com/view/4djSRW (c) 2014 - Dave Hoskins - CC BY-SA 4.0 License
		float2 hash22(float2 p)
		{
			float3 p3 = frac(p.xyx * float3(443.897, 441.423, 437.195));
			p3 += dot(p3, p3.yzx + 19.19);
			return frac((p3.xx+p3.yz)*p3.zy);
		}
		
		ENDCG

		// Outline Include
		CGINCLUDE

		#include "UnityCG.cginc"
		#include "UnityLightingCommon.cginc"	// needed for LightColor

		// Shader Properties
		float _OutlineWidth;
		fixed4 _OutlineColorVertex;

		struct appdata_outline
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
		#if TCP2_COLORS_AS_NORMALS
			float4 vertexColor : COLOR;
		#endif
		// TODO: need a way to know if texcoord1 is used in the Shader Properties
		#if TCP2_UV2_AS_NORMALS
			float2 uv2 : TEXCOORD1;
		#endif
		#if TCP2_TANGENT_AS_NORMALS
			float4 tangent : TANGENT;
		#endif
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		struct v2f_outline
		{
			float4 vertex : SV_POSITION;
			float4 vcolor : TEXCOORD0;
			float pack1 : TEXCOORD1; /* pack1.x = ndl */
			UNITY_VERTEX_OUTPUT_STEREO
		};

		v2f_outline vertex_outline (appdata_outline v)
		{
			v2f_outline output;
			UNITY_INITIALIZE_OUTPUT(v2f_outline, output);
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

			// Shader Properties Sampling
			float __outlineLightingWrapFactorVertex = ( 1.0 );
			float __outlineWidth = ( _OutlineWidth );
			float4 __outlineColorVertex = ( _OutlineColorVertex.rgba );

			float3 objSpaceLight = normalize(mul(unity_WorldToObject, _WorldSpaceLightPos0).xyz);
			float3 normal = objSpaceLight.xyz;
			half lightWrap = __outlineLightingWrapFactorVertex;
			half ndl = max(0, (dot(v.normal.xyz, objSpaceLight.xyz) + lightWrap) / (1 + lightWrap));
			output.pack1.x = ndl;
		
			//Camera-independent outline size
			float dist = distance(_WorldSpaceCameraPos.xyz, mul(unity_ObjectToWorld, v.vertex).xyz);
			float size = dist;
		
		#if !defined(SHADOWCASTER_PASS)
			output.vertex = UnityObjectToClipPos(v.vertex + float4(normal,0) * __outlineWidth * size * 0.01);
		#else
			v.vertex = v.vertex + float4(normal,0) * __outlineWidth * size * 0.01;
		#endif
		
			output.vcolor.xyzw = __outlineColorVertex;
			float4 clipPos = output.vertex;

			//Screen Position
			float4 screenPos = ComputeScreenPos(clipPos);
			return output;
		}

		float4 fragment_outline (v2f_outline input) : SV_Target
		{
			// Shader Properties Sampling
			float4 __outlineColor = ( float4(1,1,1,1) );

			half4 outlineColor = __outlineColor * input.vcolor.xyzw;
			outlineColor *= input.pack1.x;
			return outlineColor;
		}

		ENDCG
		// Outline Include End

		//Outline
		Pass
		{
			Name "Outline"
			Tags { "LightMode"="ForwardBase" }
			Cull Off
			ZWrite Off

			CGPROGRAM
			#pragma vertex vertex_outline
			#pragma fragment fragment_outline
			#pragma multi_compile TCP2_NONE TCP2_COLORS_AS_NORMALS TCP2_TANGENT_AS_NORMALS TCP2_UV2_AS_NORMALS
			#pragma multi_compile_instancing
			#pragma target 3.0
			ENDCG
		}
		// Main Surface Shader

		CGPROGRAM

		#pragma surface surf ToonyColorsCustom vertex:vertex_surface exclude_path:deferred exclude_path:prepass keepalpha fullforwardshadows nofog nolppv
		#pragma target 3.0

		//================================================================
		// SHADER KEYWORDS

		#pragma shader_feature TCP2_SPECULAR
		#pragma shader_feature TCP2_RIM_LIGHTING
		#pragma shader_feature TCP2_AMBIENT
		#pragma shader_feature TCP2_MATCAP
		#pragma shader_feature TCP2_SKETCH
		#pragma shader_feature TCP2_TEXTURED_THRESHOLD
		#pragma shader_feature TCP2_DISSOLVE

		//================================================================
		// VARIABLES

		// Shader Properties
		float3 _RimDirVert;
		float _RimMinVert;
		float _RimMaxVert;
		sampler2D _MainTex;
		float4 _MainTex_ST;
		sampler2D _DissolveMap;
		float _DissolveValue;
		float _DissolveGradientWidth;
		sampler2D _DissolveGradientTexture;
		sampler2D _BlendingSource;
		float4 _BlendingContrast;
		sampler2D _BlendTex1;
		float4 _BlendTex1_ST;
		sampler2D _BlendTex2;
		float4 _BlendTex2_ST;
		fixed4 _Color;
		half3 _Emission;
		fixed3 _MatCapColor;
		sampler2D _StylizedThreshold;
		float4 _StylizedThreshold_ST;
		float _RampThreshold;
		float _RampSmoothing;
		float _Shadow_HSV_H;
		float _Shadow_HSV_S;
		float _Shadow_HSV_V;
		sampler2D _SketchTexture;
		float4 _SketchTexture_ST;
		float _SketchTexture_OffsetSpeed;
		fixed3 _HColor;
		fixed3 _SColor;
		fixed3 _DiffuseTint;
		sampler2D _DiffuseTintMask;
		float _SpecularToonSize;
		float _SpecularToonSmoothness;
		fixed3 _SpecularColor;
		fixed3 _RimColor;
		sampler2D _MatCapTex;
		fixed4 _TCP2_AMBIENT_RIGHT;
		fixed4 _TCP2_AMBIENT_LEFT;
		fixed4 _TCP2_AMBIENT_TOP;
		fixed4 _TCP2_AMBIENT_BOTTOM;
		fixed4 _TCP2_AMBIENT_FRONT;
		fixed4 _TCP2_AMBIENT_BACK;
			
		half3 DirAmbient (half3 normal)
		{
			fixed3 retColor =
				saturate( normal.x * _TCP2_AMBIENT_RIGHT) +
				saturate(-normal.x * _TCP2_AMBIENT_LEFT) +
				saturate( normal.y * _TCP2_AMBIENT_TOP) +
				saturate(-normal.y * _TCP2_AMBIENT_BOTTOM) +
				saturate( normal.z * _TCP2_AMBIENT_FRONT) +
				saturate(-normal.z * _TCP2_AMBIENT_BACK);
			return retColor * UNITY_LIGHTMODEL_AMBIENT.a;
		}

		//Vertex input
		struct appdata_tcp2
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float4 texcoord0 : TEXCOORD0;
			float4 texcoord1 : TEXCOORD1;
			float4 texcoord2 : TEXCOORD2;
		#if defined(LIGHTMAP_ON) && defined(DIRLIGHTMAP_COMBINED)
			half4 tangent : TANGENT;
		#endif
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		struct Input
		{
			half3 viewDir;
			float4 screenPosition;
			half rim;
			half2 matcap;
			float2 texcoord0;
		};

		//================================================================
		// VERTEX FUNCTION

		void vertex_surface(inout appdata_tcp2 v, out Input output)
		{
			UNITY_INITIALIZE_OUTPUT(Input, output);

			// Texture Coordinates
			output.texcoord0.xy = (v.texcoord0.xy) * _MainTex_ST.xy + _MainTex_ST.zw;
			// Shader Properties Sampling
			float3 __rimDirVert = ( _RimDirVert.xyz );
			float __rimMinVert = ( _RimMinVert );
			float __rimMaxVert = ( _RimMaxVert );

			float4 clipPos = UnityObjectToClipPos(v.vertex);

			//Screen Position
			float4 screenPos = ComputeScreenPos(clipPos);
			output.screenPosition = screenPos;
			float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			half3 viewDir = normalize(UnityWorldSpaceViewDir(worldPos));

			#if defined(TCP2_RIM_LIGHTING)
			half3 rViewDir = viewDir;
			half3 rimDir = __rimDirVert;
			rViewDir = normalize(UNITY_MATRIX_V[0].xyz * rimDir.x + UNITY_MATRIX_V[1].xyz * rimDir.y + UNITY_MATRIX_V[2].xyz * rimDir.z);
			half rim = 1.0f - saturate(dot(rViewDir, v.normal.xyz));
			rim = smoothstep(__rimMinVert, __rimMaxVert, rim);
			output.rim = rim;
			#endif
			#if defined(TCP2_MATCAP)
			//MatCap
			float3 worldNorm = normalize(unity_WorldToObject[0].xyz * v.normal.x + unity_WorldToObject[1].xyz * v.normal.y + unity_WorldToObject[2].xyz * v.normal.z);
			worldNorm = mul((float3x3)UNITY_MATRIX_V, worldNorm);
			float3 perspectiveOffset = (screenPos.xyz / screenPos.w) - 0.5;
			worldNorm.xy -= (perspectiveOffset.xy * perspectiveOffset.z) * 0.5;
			output.matcap = worldNorm.xy * 0.5 + 0.5;
			#endif
		}

		//================================================================

		//Custom SurfaceOutput
		struct SurfaceOutputCustom
		{
			half atten;
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Specular;
			half Gloss;
			half Alpha;

			Input input;
			
			// Shader Properties
			float __stylizedThreshold;
			float __stylizedThresholdScale;
			float __rampThreshold;
			float __rampSmoothing;
			float __shadowHue;
			float __shadowSaturation;
			float __shadowValue;
			float __shadowHsvMask;
			float3 __sketchColor;
			float3 __sketchTexture;
			float __sketchThresholdScale;
			float3 __highlightColor;
			float3 __shadowColor;
			float3 __diffuseTint;
			float3 __diffuseTintMask;
			float __occlusion;
			float __ambientIntensity;
			float __specularToonSize;
			float __specularToonSmoothness;
			float3 __specularColor;
			float3 __rimColor;
			float __rimStrength;
		};

		//================================================================
		// SURFACE FUNCTION

		void surf(Input input, inout SurfaceOutputCustom output)
		{
			//Screen Space UV
			float2 screenUV = input.screenPosition.xy / input.screenPosition.w;
			
			// Shader Properties Sampling
			float4 __albedo = ( tex2D(_MainTex, (input.texcoord0.xy)).rgba );
			float4 __mainColor = ( _Color.rgba );
			float __alpha = ( __albedo.a * __mainColor.a );
			float __dissolveMap = ( tex2D(_DissolveMap, (input.texcoord0.xy)).r );
			float __dissolveValue = ( _DissolveValue );
			float __dissolveGradientWidth = ( _DissolveGradientWidth );
			float __dissolveGradientStrength = ( 2.0 );
			float4 __blendingSource = ( tex2D(_BlendingSource, (input.texcoord0.xy)).rgba );
			float4 __blendingContrast = ( _BlendingContrast.xyzw );
			float4 __blendTexture1 = ( tex2D(_BlendTex1, (input.texcoord0.xy) * _BlendTex1_ST.xy + _BlendTex1_ST.zw).rgba );
			float4 __blendTexture2 = ( tex2D(_BlendTex2, (input.texcoord0.xy) * _BlendTex2_ST.xy + _BlendTex2_ST.zw).rgba );
			float3 __emission = ( _Emission.rgb );
			float3 __matcapColor = ( _MatCapColor.rgb );
			output.__stylizedThreshold = ( tex2D(_StylizedThreshold, (input.texcoord0.xy) * _StylizedThreshold_ST.xy + _StylizedThreshold_ST.zw).a );
			output.__stylizedThresholdScale = ( 1.0 );
			output.__rampThreshold = ( _RampThreshold );
			output.__rampSmoothing = ( _RampSmoothing );
			output.__shadowHue = ( _Shadow_HSV_H );
			output.__shadowSaturation = ( _Shadow_HSV_S );
			output.__shadowValue = ( _Shadow_HSV_V );
			output.__shadowHsvMask = ( __albedo.a );
			output.__sketchColor = ( float3(0,0,0) );
			output.__sketchTexture = ( tex2D(_SketchTexture, (screenUV) * _ScreenParams.zw * _SketchTexture_ST.xy + _SketchTexture_ST.zw + hash22(floor(_Time.xx * _SketchTexture_OffsetSpeed.xx) / _SketchTexture_OffsetSpeed.xx)).aaa );
			output.__sketchThresholdScale = ( 1.0 );
			output.__highlightColor = ( _HColor.rgb );
			output.__shadowColor = ( _SColor.rgb );
			output.__diffuseTint = ( _DiffuseTint.rgb );
			output.__diffuseTintMask = ( tex2D(_DiffuseTintMask, (input.texcoord0.xy)).rgb );
			output.__occlusion = ( __albedo.a );
			output.__ambientIntensity = ( 1.0 );
			output.__specularToonSize = ( _SpecularToonSize );
			output.__specularToonSmoothness = ( _SpecularToonSmoothness );
			output.__specularColor = ( _SpecularColor.rgb );
			output.__rimColor = ( _RimColor.rgb );
			output.__rimStrength = ( 1.0 );

			output.input = input;

			output.Albedo = __albedo.rgb;
			output.Alpha = __alpha;
			
			//Dissolve
			#if defined(TCP2_DISSOLVE)
			half dissolveMap = __dissolveMap;
			half dissolveValue = __dissolveValue;
			half gradientWidth = __dissolveGradientWidth;
			float dissValue = dissolveValue*(1+2*gradientWidth) - gradientWidth;
			float dissolveUV = smoothstep(dissolveMap - gradientWidth, dissolveMap + gradientWidth, dissValue);
			clip((1-dissolveUV) - 0.001);
			half4 dissolveColor = ( tex2D(_DissolveGradientTexture, (dissolveUV.xx)).rgba );
			dissolveColor *= __dissolveGradientStrength * dissolveUV;
			output.Emission += dissolveColor.rgb;
			#endif
			half4 albedoAlpha = half4(output.Albedo, output.Alpha);
			
			//Texture Blending
			fixed4 blendingSource = __blendingSource;
			blendingSource.rgba = saturate(normalize(blendingSource.rgba) * dot(__blendingContrast, blendingSource.rgba));
			
			fixed4 tex1 = __blendTexture1;
			albedoAlpha = lerp(albedoAlpha, tex1, blendingSource.r);
			fixed4 tex2 = __blendTexture2;
			albedoAlpha = lerp(albedoAlpha, tex2, blendingSource.g);
			output.Albedo = albedoAlpha.rgb;
			
			output.Albedo *= __mainColor.rgb;
			output.Emission += __emission;
			
			//MatCap
			#if defined(TCP2_MATCAP)
			fixed3 matcap = tex2D(_MatCapTex, input.matcap).rgb * __matcapColor;
			output.Albedo *= matcap;
			#endif
		}

		//================================================================
		// LIGHTING FUNCTION

		inline half4 LightingToonyColorsCustom(inout SurfaceOutputCustom surface, half3 viewDir, UnityGI gi)
		{
			half3 lightDir = gi.light.dir;
			#if defined(UNITY_PASS_FORWARDBASE)
				half3 lightColor = _LightColor0.rgb;
				half atten = surface.atten;
			#else
				//extract attenuation from point/spot lights
				half3 lightColor = _LightColor0.rgb;
				half atten = max(gi.light.color.r, max(gi.light.color.g, gi.light.color.b)) / max(_LightColor0.r, max(_LightColor0.g, _LightColor0.b));
			#endif

			half3 normal = normalize(surface.Normal);
			half ndl = dot(normal, lightDir);
			#if defined(TCP2_TEXTURED_THRESHOLD)
			float stylizedThreshold = surface.__stylizedThreshold;
			stylizedThreshold -= 0.5;
			stylizedThreshold *= surface.__stylizedThresholdScale;
			ndl += stylizedThreshold;
			#endif
			half3 ramp;
			#define		RAMP_THRESHOLD	surface.__rampThreshold
			#define		RAMP_SMOOTH		surface.__rampSmoothing
			ndl = saturate(ndl);
			ramp = smoothstep(RAMP_THRESHOLD - RAMP_SMOOTH*0.5, RAMP_THRESHOLD + RAMP_SMOOTH*0.5, ndl);
			half3 rampGrayscale = ramp;

			//Apply attenuation (shadowmaps & point/spot lights attenuation)
			ramp *= atten;
			
			//Shadow HSV
			float3 albedoShadowHSV = ApplyHSV_3(surface.Albedo, surface.__shadowHue, surface.__shadowSaturation, surface.__shadowValue);
			surface.Albedo = lerp(albedoShadowHSV, surface.Albedo, ramp + surface.__shadowHsvMask * (1 - ramp));
			
			// Sketch
			#if defined(TCP2_SKETCH)
			half3 sketchColor = lerp(surface.__sketchColor, half3(1,1,1), surface.__sketchTexture);
			half3 sketch = lerp(sketchColor, half3(1,1,1), saturate(ramp * surface.__sketchThresholdScale));
			#endif

			//Highlight/Shadow Colors
			#if !defined(UNITY_PASS_FORWARDBASE)
				ramp = lerp(half3(0,0,0), surface.__highlightColor, ramp);
			#else
				ramp = lerp(surface.__shadowColor, surface.__highlightColor, ramp);
			#endif
			half3 diffuseTint = saturate(surface.__diffuseTint + ndl);
				ramp = lerp(ramp, ramp * diffuseTint, surface.__diffuseTintMask);

			//Output color
			half4 color;
			color.rgb = surface.Albedo * lightColor.rgb * ramp;
			color.a = surface.Alpha;
			#if defined(TCP2_SKETCH)
			color.rgb *= sketch;
			#endif

			// Apply indirect lighting (ambient)
			half occlusion = surface.__occlusion;
			#ifdef UNITY_LIGHT_FUNCTION_APPLY_INDIRECT
			#if defined(TCP2_AMBIENT)
				half3 ambient = gi.indirect.diffuse;
				
				//Directional Ambient
				ambient.rgb += DirAmbient(normal);
				ambient *= surface.Albedo * occlusion * surface.__ambientIntensity;

				#if defined(TCP2_SKETCH)
				#endif
				color.rgb += ambient;
			#endif
			#endif
			
			#if defined(TCP2_SPECULAR)
			//Blinn-Phong Specular
			half3 h = normalize(lightDir + viewDir);
			float ndh = max(0, dot (normal, h));
			float spec = smoothstep(surface.__specularToonSize + surface.__specularToonSmoothness, surface.__specularToonSize - surface.__specularToonSmoothness,1 - (ndh / (1+surface.__specularToonSmoothness)));
			spec *= ndl;
			spec *= atten;
			
			//Apply specular
			color.rgb += spec * lightColor.rgb * surface.__specularColor;
			#endif
			// Rim Lighting
			#if defined(TCP2_RIM_LIGHTING)
			#if !defined(UNITY_PASS_FORWARDADD)
			half rim = surface.input.rim;
			half3 rimColor = surface.__rimColor;
			half rimStrength = surface.__rimStrength;
			color.rgb += rim * rimColor * rimStrength;
			#endif
			#endif

			return color;
		}

		void LightingToonyColorsCustom_GI(inout SurfaceOutputCustom surface, UnityGIInput data, inout UnityGI gi)
		{
			half3 normal = surface.Normal;

			//GI without reflection probes
			gi = UnityGlobalIllumination(data, 1.0, normal); // occlusion is applied in the lighting function, if necessary

			surface.atten = data.atten; // transfer attenuation to lighting function
			gi.light.color = _LightColor0.rgb; // remove attenuation
		}

		ENDCG

	}

	Fallback "Diffuse"
	CustomEditor "ToonyColorsPro.ShaderGenerator.MaterialInspector_SG2"
}

/* TCP_DATA u config(ver:"2.4.1";tmplt:"SG2_Template_Default";features:list["UNITY_5_4","UNITY_5_5","UNITY_5_6","UNITY_2017_1","UNITY_2018_1","UNITY_2018_2","UNITY_2018_3","UNITY_2019_1","UNITY_2019_2","SHADOW_HSV","ENABLE_LIGHTMAPS","SKETCH","OUTLINE","OUTLINE_BEHIND_DEPTH","OUTLINE_CONSTANT_SIZE","OUTLINE_LIGHTING_WRAP","OUTLINE_LIGHTING_VERT","OUTLINE_LIGHTING","OUTLINE_FAKE_RIM_DIRLIGHT","SKETCH_SHADER_FEATURE","TEXTURED_THRESHOLD","TT_SHADER_FEATURE","DIFFUSE_TINT","DIFFUSE_TINT_MASK","DISSOLVE","DISSOLVE_CLIP","DISSOLVE_GRADIENT","DISSOLVE_SHADER_FEATURE","TEXTURE_BLENDING","TEXBLEND_LINEAR","TEXBLEND_NORMALIZE","BLEND_TEX1","BLEND_TEX2","AMBIENT_SHADER_FEATURE","OCCLUSION","DIRAMBIENT","MATCAP_MULT","MATCAP","MATCAP_PERSPECTIVE_CORRECTION","MATCAP_SHADER_FEATURE","RIM","RIM_VERTEX","RIM_DIR","RIM_SHADER_FEATURE","SPEC_LEGACY","SPECULAR","SPECULAR_TOON","SPECULAR_SHADER_FEATURE","SHADOW_HSV_MASK","EMISSION"];flags:list["fullforwardshadows"];keywords:dict[RENDER_TYPE="Opaque",RampTextureDrawer="[TCP2Gradient]",RampTextureLabel="Ramp Texture",SHADER_TARGET="3.0",BLEND_TEX1_CHNL="r",BLEND_TEX2_CHNL="g",RIM_LABEL="Rim Lighting"];shaderProperties:list[];customTextures:list[]) */
/* TCP_HASH 8f5b96e5fddae4129cb2cd78102219b2 */
