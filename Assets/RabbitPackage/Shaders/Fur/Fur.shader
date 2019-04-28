Shader "Custom/FurShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_FurTex("Fur pattern", 2D) = "white" {}
		_Diffuse("Diffuse value", Range(0, 1)) = 1

		_FurLength("Fur length", Range(0.0, 1)) = 0.5
		_CutOff("Alpha cutoff", Range(0, 1)) = 0.5
		_Thickness("Thickness", Range(0, 0.5)) = 0
	}

	CGINCLUDE

		fixed _Diffuse;

		inline fixed4 LambertDiffuse(float3 worldNormal)
		{
			float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
			float NdotL = max(0, dot(worldNormal, lightDir));
			return NdotL * _Diffuse;
		}

	ENDCG

	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		CGPROGRAM
		#pragma surface surf Lambert addshadow
			struct Input {
				float2 uv_MainTex;
			};
		sampler2D _MainTex;
		void surf(Input IN, inout SurfaceOutput o) {
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
		}
		ENDCG

		Tags{ "RenderType" = "TransparentCutout" "IgnoreProjector" = "True" "Queue" = "Transparent" }
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma target 3.0
#pragma multi_compile LIGHTMAP_ON LIGHTMAP_OFF

#include "UnityCG.cginc"

	struct v2f
	{
		float4 pos : SV_POSITION;
		float2 uv0 : TEXCOORD0;
		float2 uv1 : TEXCOORD1;
		fixed4 dif : COLOR;
	};

	struct appdata_lightmap {
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
		float2 texcoord1 : TEXCOORD1;
		float3 normal : NORMAL;
	};

	sampler2D _MainTex;
	float4 _MainTex_ST;

	v2f vert(appdata_lightmap v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv0 = TRANSFORM_TEX(v.texcoord, _MainTex);
		o.uv1 = v.texcoord1 * unity_LightmapST.xy + unity_LightmapST.zw;
		o.dif = LambertDiffuse(v.normal);
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		fixed4 col = tex2D(_MainTex, i.uv0);
		col.rgb *= i.dif;
		col.rgb *= (DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv1)));
		return col;
	}
		ENDCG
	}

			Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#define FURSTEP 0
#include "FurHelper.cginc"
		ENDCG
	}
		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#define FURSTEP 0.05
#include "FurHelper.cginc"
		ENDCG
	}

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#define FURSTEP 0.10
#include "FurHelper.cginc"
		ENDCG
	}

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#define FURSTEP 0.15
#include "FurHelper.cginc"
		ENDCG
	}

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#define FURSTEP 0.20
#include "FurHelper.cginc"
		ENDCG
	}

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#define FURSTEP 0.25
#include "FurHelper.cginc"
		ENDCG
	}

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#define FURSTEP 0.30
#include "FurHelper.cginc"
		ENDCG
	}

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#define FURSTEP 0.35
#include "FurHelper.cginc"
		ENDCG
	}

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#define FURSTEP 0.40
#include "FurHelper.cginc"
		ENDCG
	}

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#define FURSTEP 0.45
#include "FurHelper.cginc"
		ENDCG
	}

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#define FURSTEP 0.50
#include "FurHelper.cginc"
		ENDCG
	}

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#define FURSTEP 0.55
#include "FurHelper.cginc"
		ENDCG
	}

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#define FURSTEP 0.60
#include "FurHelper.cginc"
		ENDCG
	}

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#define FURSTEP 0.65
#include "FurHelper.cginc"
		ENDCG
	}

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#define FURSTEP 0.70
#include "FurHelper.cginc"
		ENDCG
	}

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#define FURSTEP 0.75
#include "FurHelper.cginc"
		ENDCG
	}

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#define FURSTEP 0.80
#include "FurHelper.cginc"
		ENDCG
	}

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#define FURSTEP 0.85
#include "FurHelper.cginc"
		ENDCG
	}

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#define FURSTEP 0.90
#include "FurHelper.cginc"
		ENDCG
	}

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#define FURSTEP 0.95
#include "FurHelper.cginc"
		ENDCG
	}
	}
}