// adapted from here:
// https://www.youtube.com/watch?v=owwnUcmO3Lw&list=PLLEoQZOmM_baTRHWkGk1qhrpVntqgKXua&index=15&t=179s
// and here:
// https://alastaira.wordpress.com/2014/12/30/adding-shadows-to-a-unity-vertexfragment-shader-in-7-easy-steps/

Shader "Custom/CelShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BaseColor("Base Color", COLOR) = (1, 1, 1, 1)
        _LightColor("Light Color", COLOR) = (1, 1, 1, 1)
        _Brightness("Brightness", Range(0, 1)) = 0.3
        _Strength("Strength", Range(0, 1)) = 0.5
        _Detail("Detail", Range(0, 1)) = 0.3
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            // for shadows
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"

            #pragma multi_compile_fwdbase

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                half3 worldNormal : NORMAL;
                LIGHTING_COORDS(0, 1)
            };

            sampler2D _MainTex;
            float4 _BaseColor;
            float4 _LightColor;
            float4 _MainTex_ST;
            float _Brightness;
            float _Strength;
            float _Detail;

            float Cel(float3 normal, float3 lightDir)
            {
                float NdotL = dot(normalize(normal), normalize(lightDir));
                NdotL = max(0.0, NdotL);
                return floor(NdotL / _Detail);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);

                TRANSFER_VERTEX_TO_FRAGMENT(o);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                col *= _BaseColor;

                col *= Cel(i.worldNormal, _WorldSpaceLightPos0.xyz) * _LightColor * _Strength + _Brightness;

                // apply shadows
                float attenuation = LIGHT_ATTENUATION(i);
                col *= attenuation;

                return col;
            }
            ENDCG
        }
    }

    Fallback "VertexLit"
}
