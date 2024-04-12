Shader "Custom/WindowAtlasShader" {
    Properties {
        _WallTexture ("Wall Texture", 2D) = "white" {}
        _RoofTexture ("Roof Texture", 2D) = "white" {} // ����� �������� ��� �������� �����
        _WindowAtlas ("Window Atlas", 2D) = "white" {}
        _WindowPositionMap ("Window Position Map", 2D) = "white" {}
        _BoxSize ("Box Size", Vector) = (1,1,1,1)
        _WallTextureTiling ("Wall Texture Tiling", Vector) = (1, 1, 0, 0)
        _WallTextureOffset ("Wall Texture Offset", Vector) = (0, 0, 0, 0)
        _RoofTextureTiling ("Roof Texture Tiling", Vector) = (1, 1, 0, 0) // Tiling ��� �������� �����
        _RoofTextureOffset ("Roof Texture Offset", Vector) = (0, 0, 0, 0) // Offset ��� �������� �����
        _WindowAtlasTiling ("Window Atlas Tiling", Vector) = (1, 1, 0, 0)
        _WindowAtlasOffset ("Window Atlas Offset", Vector) = (0, 0, 0, 0)
        _WindowSizeX ("Window Size X", Float) = 0.2
        _WindowSizeY ("Window Size Y", Float) = 0.2
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
                float3 normal : NORMAL;
                float2 windowPosition : TEXCOORD1; // �������������� UV ���������� ��� ������� ����
                float windowID : TEXCOORD2; // ���������� ������������� ����
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD1;
                float2 windowPosition : TEXCOORD2; // �������� UV ��������� ������� ���� �� ����������� ������
                float windowID : TEXCOORD3; // �������� ����������� �������������� ���� �� ����������� ������
            };

            sampler2D _WallTexture;
            sampler2D _RoofTexture; // ����������� ������� �������� �����
            sampler2D _WindowAtlas;
            sampler2D _WindowPositionMap;
            float4 _BoxSize;
            float4 _WallTextureTiling;
            float4 _WallTextureOffset;
            float4 _RoofTextureTiling;
            float4 _RoofTextureOffset;
            float4 _WindowAtlasTiling;
            float4 _WindowAtlasOffset;
            float _WindowSizeX; // ���������� �����
            float _WindowSizeY; // ���������� �����

            v2f vert (appdata v) {
                v2f o;
                o.normal = v.normal;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv * (_BoxSize.xy);
                o.windowPosition = v.uv; // �������� UV ��������� ������� ����
                o.windowID = v.windowID; // �������� ����������� �������������� ����
                return o;
            }
float4 frag (v2f i) : SV_Target {
    float3 fragNormal = normalize(i.normal);
    // ���������, �������� �� ����� �������
    float isRoof = step(0.99, fragNormal.y); // ��������� �������� ��� ����������� ������� �����

    if (isRoof == 1.0) { // ���� ��� ������� �����
        float2 roofTextureUV = i.uv * _RoofTextureTiling.xy + _RoofTextureOffset.xy;
        return tex2D(_RoofTexture, roofTextureUV);
    }

    float windowType = tex2D(_WindowPositionMap, i.uv).r;

    if (windowType == 0.0) {
        float2 wallTextureUV = i.uv * _WallTextureTiling.xy + _WallTextureOffset.xy;
        return tex2D(_WallTexture, wallTextureUV);
    }
    else {
        float tileSizeX = _BoxSize.x / 2.0;
        float tileSizeY = _BoxSize.y / 2.0;
        
        // ��������� ���������� �������� ���� ��� �������� �������
        float2 atlasTileSize = float2(_WindowSizeX, _WindowSizeY);
        float2 tileOffset;
        tileOffset.x = floor(fmod(windowType * 4.0, 2.0)) * atlasTileSize.x;
        tileOffset.y = floor(windowType * 4.0 / 2.0) * atlasTileSize.y;

        // ����������� ���������� �������� �������
        float2 normalizedUV = i.uv * float2(tileSizeX, tileSizeY);
        
        // ��������� ���������� �������� ���� ��� �������� �������
        float2 atlasCoords = (normalizedUV + tileOffset) / _BoxSize.xy;
        float2 windowAtlasUV = atlasCoords * _WindowAtlasTiling.xy + _WindowAtlasOffset.xy;

        // ������� �������� ����
        return tex2D(_WindowAtlas, windowAtlasUV);
    }

            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
