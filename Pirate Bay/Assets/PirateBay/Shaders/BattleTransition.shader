Shader "Custom/BattleTransition" {
	Properties {
		_Color("Colour", Color) = (1, 1, 1, 1)
		_MainTex("Main Texture", 2D) = "white" {}
		_DirTex("Direction Texture", 2D) = "white" {}
		_TransitionTex("Transition Texture", 2D) = "white" {}
		_Cutoff("Cutoff", Range(0, 1)) = 0
		_Fade("Fade", Range(0,1)) = 0
		[MaterialToggle] _Distort("Distort", Float) = 0
	}
	SubShader {
			// No culling or depth
			Cull Off ZWrite Off ZTest Always

			Pass {

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				sampler2D _MainTex;
				sampler2D _DirTex;
				sampler2D _TransitionTex;
				float _Cutoff;
				float4 _Color;
				float _Fade;
				float _Distort;

				struct appdata {
					float4 pos : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f {
					float4 pos : SV_POSITION;
					float2 uv : TEXCOORD0;
				};


				v2f vert(appdata i) {
					v2f o;
					o.pos = UnityObjectToClipPos(i.pos);
					o.uv = i.uv;
					return o;
				}

				fixed4 frag(v2f i) : COLOR{
					// Fade the screen to colour by fade.
					if (_Fade > 0) {
						return lerp(tex2D(_MainTex, i.uv), _Color, _Fade);
					}

					// Distort the screen.
					if (_Distort > 0) {
						float3 col = tex2D(_DirTex, i.uv);
						float2 dir = normalize(float2((col.r - 0.5) * 2, (col.g - 0.5) * 2));

						if (col.b < _Cutoff) {
							return _Color;
						}

						return fixed4(tex2D(_MainTex, (i.uv + _Cutoff * dir)).rgba);
					}

					float3 color = tex2D(_TransitionTex, i.uv);
					float3 tex = tex2D(_MainTex, i.uv);

					return (color.b < _Cutoff) ? _Color : fixed4(tex, 0.0);
				}
				ENDCG
		}
		
	}
}
