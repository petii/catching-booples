// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Sprites/Outline" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color ( "Color", Color ) = ( 1, 0, 0, 1 )
		_Width ( "Outline Width", Range(0.0, 0.05) ) = 0.0
		_AlphaLimit ( "Alpha Limit", Range(0.0, 1.0) ) = 0.0
	}
	SubShader {
		Tags { 
			"Queue" = "Geometry"
			"RenderType"="Transparent" 
		}
        Blend One OneMinusSrcAlpha
		Pass {
		CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			// #include "UnityCG.cginc"

			sampler2D _MainTex;
            float4 _MainTex_TexelSize;
			fixed4 _Color;
			float _Width;
			float _AlphaLimit;

			struct vertInput {
				float4 pos : POSITION;
				fixed4 c : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct vertOutput {
				float4 pos : SV_POSITION;
				fixed4 c : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			vertOutput vert ( vertInput input ) {
				vertOutput o;
				o.pos = UnityObjectToClipPos( input.pos );
				o.c = input.c; // * _Color;
				o.texcoord = input.texcoord;
                // o.pos = UnityPixelSnap (o.pos);
				return o;
			}

			//up to right
			fixed4 getPixel1 ( vertOutput v, float degree ) {
				fixed4 pixel = tex2D(_MainTex,
					v.texcoord + normalize(fixed2(_MainTex_TexelSize.x * degree ,
						_MainTex_TexelSize.y * (1-degree) )) * _Width );
				return pixel;
			}
			//down to left
			fixed4 getPixel2 ( vertOutput v, float degree ) {
				fixed4 pixel = tex2D(_MainTex,
					v.texcoord - normalize(fixed2(_MainTex_TexelSize.x * degree ,
						_MainTex_TexelSize.y * (1-degree) )) * _Width );
				return pixel;
			}
			//up to left
			fixed4 getPixel3 ( vertOutput v, float degree ) {
				fixed4 pixel = tex2D(_MainTex,
					v.texcoord + normalize(fixed2(_MainTex_TexelSize.x * (-1+degree) ,
						_MainTex_TexelSize.y * (degree) )) * _Width );
				return pixel;
			}
			//down to right
			fixed4 getPixel4 ( vertOutput v, float degree ) {
				fixed4 pixel = tex2D(_MainTex,
					v.texcoord - normalize(fixed2(_MainTex_TexelSize.x * (-1+degree) ,
						_MainTex_TexelSize.y * (degree) )) * _Width );
				return pixel;
			}

			bool HasPixelNear(vertOutput v) {
				bool has = false;
				
				float degree = 0;
				for (degree = 0; degree <= 1; degree+= 0.1) {
					if (getPixel1(v, degree).a > _AlphaLimit) return true; 
					if (getPixel2(v, degree).a > _AlphaLimit) return true; 
					if (getPixel3(v, degree).a > _AlphaLimit) return true; 
					if (getPixel4(v, degree).a > _AlphaLimit) return true; 
				}
				return false;
			}

			fixed4 frag ( vertOutput input ) : COLOR {
				fixed4 c = tex2D( _MainTex, input.texcoord ) * input.c;
				if (c.a <= _AlphaLimit && HasPixelNear(input)) {
					c = _Color;
					//c.rgb = _Color.rgb;
                    // fixed4 pixelUp = tex2D(_MainTex, input.texcoord + fixed2(0, _MainTex_TexelSize.y));
                    // fixed4 pixelDown = tex2D(_MainTex, input.texcoord - fixed2(0, _MainTex_TexelSize.y));
                    // fixed4 pixelRight = tex2D(_MainTex, input.texcoord + fixed2(_MainTex_TexelSize.x, 0));
                    // fixed4 pixelLeft = tex2D(_MainTex, input.texcoord - fixed2(_MainTex_TexelSize.x, 0));
					// if (pixelUp.a == 0
				}

				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	} 
	FallBack "Diffuse"
}
