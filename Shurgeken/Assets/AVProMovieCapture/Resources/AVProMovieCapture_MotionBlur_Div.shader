Shader "Hidden/MotionBlur/Div" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}

	SubShader 
	{
		Pass 
		{
			ZTest Always Cull Off ZWrite Off
			Lighting Off
			Fog { Mode off }
			
			CGPROGRAM
			#pragma exclude_renderers flash xbox360 ps3 gles
			#pragma vertex vert
			#pragma fragment frag
			//#pragma target 3.0
			//#pragma fragmentoption ARB_precision_hint_fastest 
			#include "UnityCG.cginc"
			
			uniform float _NumSamples;
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_TexelSize;

			v2f_img vert(appdata_img v)
			{
				v2f_img o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.texcoord;

#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
				{
					o.uv.y = 1.0 - o.uv.y;
				}
#endif

				return o;
			}

			fixed4 frag (v2f_img i) : COLOR
			{
				float4 col = tex2D(_MainTex, i.uv);
				return col / _NumSamples;
			}
			ENDCG	
		}	
	}

Fallback off
}