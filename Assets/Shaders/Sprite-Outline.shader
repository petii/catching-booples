Shader "Sprites/Outline" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color ( "Color", Color ) = ( 1, 0, 0, 1 )
		_Width ( "Outline Width", Range(0.0, 20.0) ) = 0.0
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
				o.pos = mul( UNITY_MATRIX_MVP, input.pos );
				o.c = input.c; // * _Color;
				o.texcoord = input.texcoord;
                // o.pos = UnityPixelSnap (o.pos);
				return o;
			}

			bool HasPixelNear(vertOutput v, fixed4 c) {
				fixed4 pixelUp = tex2D(_MainTex, v.texcoord + fixed2(0, _MainTex_TexelSize.y * _Width));
                fixed4 pixelDown = tex2D(_MainTex, v.texcoord - fixed2(0, _MainTex_TexelSize.y * _Width));
                fixed4 pixelRight = tex2D(_MainTex, v.texcoord + fixed2(_MainTex_TexelSize.x * _Width, 0));
                fixed4 pixelLeft = tex2D(_MainTex, v.texcoord - fixed2(_MainTex_TexelSize.x * _Width, 0));				
				if (pixelUp.a != 0 || pixelDown.a != 0 || pixelRight.a != 0 || pixelLeft.a != 0 ) {
					return true;
				}
				else {
					return false;
				}
				
			}

			fixed4 frag ( vertOutput input ) : COLOR {
				fixed4 c = tex2D( _MainTex, input.texcoord ) * input.c;
				if (c.a == 0 && HasPixelNear(input, c)) {
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
