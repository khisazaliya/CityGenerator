Shader "Custom/HouseShader" {
  Properties{
      _WallTexture("Wall Texture", 2D) = "white" {} _RoofTexture(
          "Roof Texture", 2D) = "white" {}  // Новое свойство для текстуры крыши
      _WindowAtlas("Window Atlas", 2D) = "white" {} _WindowPositionMap(
          "Window Position Map", 2D) = "white" {} _BoxSize("Box Size", Vector) =
          (1, 1, 1, 1)_WallTextureTiling("Wall Texture Tiling", Vector) =
              (1, 1, 0, 0)_WallTextureOffset("Wall Texture Offset", Vector) =
                  (0, 0, 0, 0)_RoofTextureTiling("Roof Texture Tiling",
                                                 Vector) =
                      (1, 1, 0, 0)  // Tiling для текстуры крыши
      _RoofTextureOffset("Roof Texture Offset", Vector) =
          (0, 0, 0, 0)  // Offset для текстуры крыши
      _WindowAtlasTiling("Window Atlas Tiling", Vector) =
          (1, 1, 0, 0)_WindowAtlasOffset("Window Atlas Offset", Vector) =
              (0, 0, 0, 0)_WindowSizeX("Window Size X", Float) =
                  0.2 _WindowSizeY("Window Size Y", Float) =
                      0.2 _DoorTexture("Door Texture", 2D) =
                          "white" {}  // Новое свойство для текстуры двери
      _DoorTextureTiling("Door Texture Tiling", Vector) =
          (1, 1, 0, 0)  // Tiling для текстуры двери
      _DoorTextureOffset("Door Texture Offset", Vector) =
          (0, 0, 0, 0)  // Offset для текстуры двери
  }

  SubShader {
    Tags{"RenderType" = "Opaque"} LOD 100

        Pass {
      CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

      struct appdata {
        float4 vertex : POSITION;
        float2 uv : TEXCOORD0;
        float3 normal : NORMAL;
        float2 windowPosition : TEXCOORD1;
        float windowID : TEXCOORD2;
        float doorType : TEXCOORD3;  // Добавляем канал для типа двери
      };

      struct v2f {
        float2 uv : TEXCOORD0;
        float4 vertex : SV_POSITION;
        float3 normal : TEXCOORD1;
        float2 windowPosition : TEXCOORD2;
        float windowID : TEXCOORD3;
        float doorType : TEXCOORD4;  // Передаем тип двери во фрагментный шейдер
      };

      sampler2D _WallTexture;
      sampler2D _RoofTexture;
      sampler2D _WindowAtlas;
      sampler2D _WindowPositionMap;
      sampler2D _DoorTexture;  // Добавленный сэмплер текстуры двери
      float4 _BoxSize;
      float4 _WallTextureTiling;
      float4 _WallTextureOffset;
      float4 _RoofTextureTiling;
      float4 _RoofTextureOffset;
      float4 _WindowAtlasTiling;
      float4 _WindowAtlasOffset;
      float4 _DoorTextureTiling;  // Tiling для текстуры двери
      float4 _DoorTextureOffset;  // Offset для текстуры двери
      float _WindowSizeX;
      float _WindowSizeY;

      v2f vert(appdata v) {
        v2f o;
        o.normal = v.normal;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.uv = v.uv * (_BoxSize.xy);
        o.windowPosition = v.windowPosition;
        o.windowID = v.windowID;
        o.doorType = v.doorType;  // Передаем тип двери в вершинный шейдер
        return o;
      }

      float4 frag(v2f i) : SV_Target {
        float windowType = tex2D(_WindowPositionMap, i.uv).r;
        float doorPixelValue =
            tex2D(_WindowPositionMap, i.uv).b;  // Значение пикселя в канале G

        float3 fragNormal = normalize(i.normal);
        float isRoof = step(0.99, fragNormal.y);

        if (isRoof == 1.0) {
          float2 roofTextureUV =
              i.uv * _RoofTextureTiling.xy + _RoofTextureOffset.xy;
          return tex2D(_RoofTexture, roofTextureUV);
        }

        if (doorPixelValue != 0.0) {  // Если значение в канале G не равно 0,
                                      // считаем, что это дверь
          float2 doorTextureUV =
              i.uv * _DoorTextureTiling.xy + _DoorTextureOffset.xy;
          return tex2D(_DoorTexture, doorTextureUV);
        }

        if (windowType == 0.0) {
          float2 wallTextureUV =
              i.uv * _WallTextureTiling.xy + _WallTextureOffset.xy;
          return tex2D(_WallTexture, wallTextureUV);
        } else {
          float2 tileSize = _BoxSize.xy / 2.0;
          float2 atlasTileSize = float2(_WindowSizeX, _WindowSizeY);
          float2 tileOffset;
          tileOffset.x = floor(fmod(i.windowID * 4.0, 2.0)) * atlasTileSize.x;
          tileOffset.y = floor(i.windowID * 4.0 / 2.0) * atlasTileSize.y;
          float2 atlasCoords = (i.windowPosition * atlasTileSize * tileSize);
          float2 windowAtlasUV =
              atlasCoords * _WindowAtlasTiling.xy + _WindowAtlasOffset.xy;
          float4 windowColor = tex2D(_WindowAtlas, windowAtlasUV);
          return windowColor;
        }
      }

      ENDCG
    }
  }
  FallBack "Diffuse"
}
