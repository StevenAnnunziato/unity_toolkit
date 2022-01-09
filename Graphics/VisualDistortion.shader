Shader "Custom/VisualDistortion"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Instensity("Intensity", Float) = 1.0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // hueshift function adapted from user mAlk:
            // https://www.shadertoy.com/view/MsjXRt
            fixed4 hueshift(in fixed3 Color, in float Shift)
            {
                fixed3 P = fixed3(0.55735, 0.55735, 0.55735) * dot(fixed3(0.55735, 0.55735, 0.55735), Color);
                fixed3 U = Color - P;
                fixed3 V = cross(fixed3(0.55735, 0.55735, 0.55735), U);

                Color = U * cos(Shift * 6.2832) + V * sin(Shift * 6.2832) + P;
                return fixed4(Color, 1.0);
            }

            sampler2D _MainTex;
            float _Intensity;

            fixed4 frag(v2f i) : SV_Target
            {
                fixed2 uv = i.uv + fixed2(sin(i.uv.x * 14.0 + _Time.y * 1.5) * 0.02, cos(i.uv.x * 12.0 + _Time.y * 1.5) * 0.02) * _Intensity;
                fixed4 col = tex2D(_MainTex, uv);
                fixed4 shift = hueshift(fixed3(col.x, col.y, col.z), frac(_Time.y * 0.1));
                return lerp(col, shift, _Intensity);
            }
            ENDCG
        }
    }
}
