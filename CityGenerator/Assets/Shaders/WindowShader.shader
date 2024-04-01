Shader "Custom/WindowAtlasShader" {
    Properties {
        _WallTexture ("Wall Texture", 2D) = "white" {}
        _WindowAtlas ("Window Atlas", 2D) = "white" {}
        _WindowPositionMap ("Window Position Map", 2D) = "white" {}
        _BoxSize ("Box Size", Vector) = (1,1,1,1)
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _WallTexture;
            sampler2D _WindowAtlas;
            sampler2D _WindowPositionMap;
            float4 _BoxSize;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv * (_BoxSize.xy);
                return o;
            }

            float4 frag (v2f i) : SV_Target {
                float windowType = tex2D(_WindowPositionMap, i.uv).r;

                // Если участок на карте позиций окон черный, применяем текстуру стены
                if (windowType == 0.0) {
                    return tex2D(_WallTexture, i.uv);
                }
                else {
                    // Рассчитываем размер тайла в соответствии с размером грани куба
                    float2 tileSize = _BoxSize.xy / 2.0;

                    // Рассчитываем UV-координаты для текущего тайла, учитывая его размер
                    float2 atlasTileSize = float2(0.5, 0.5);
                    float2 tileOffset;
                    tileOffset.x = floor(fmod(windowType * 4.0, 2.0)) * atlasTileSize.x;
                    tileOffset.y = floor(windowType * 4.0 / 2.0) * atlasTileSize.y;

                    float2 atlasCoords = (i.uv * atlasTileSize * tileSize) + tileOffset;

                    // Сэмплируем текстуру окна с использованием рассчитанных координат
                    float4 windowColor = tex2D(_WindowAtlas, atlasCoords);

                    return windowColor;
                }
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
