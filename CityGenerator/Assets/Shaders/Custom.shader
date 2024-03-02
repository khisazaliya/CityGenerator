Shader "Custom/LODShader" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Cutoff ("Alpha Cutoff", Range(0,1)) = 0.5
        _LODThreshold ("LOD Threshold", Range(0,1)) = 0.5
    }
    SubShader {
        Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100
        
        Cull Front
        ZWrite On
        ZTest LEqual

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float4 color : COLOR;
            };

            float4 _Color;
            float _Cutoff;
            float _LODThreshold;

            v2f vert(appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.color = _Color;
                return o;
            }

            half4 frag(v2f i) : SV_Target {
                // Определяем уровень детализации по расстоянию от камеры
                float dist = distance(_WorldSpaceCameraPos, mul(unity_ObjectToWorld, i.pos));
                float lodFactor = saturate(dist / _LODThreshold);

                // Если объект находится дальше заданного порога, делаем его менее прозрачным
                if (lodFactor > _Cutoff) {
                    return half4(i.color.rgb, 1 - (_Cutoff / lodFactor));
                }
                
                // Иначе рисуем объект с полной непрозрачностью
                return i.color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
