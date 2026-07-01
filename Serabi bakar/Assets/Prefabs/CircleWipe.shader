Shader "UI/CircleWipe"
{
    // Shader ini menggambar lubang lingkaran (transparan) di tengah bidang hitam.
    // _Radius besar  -> lubang besar -> hampir semua layar terlihat (transparan)
    // _Radius = 0    -> lubang tertutup -> layar full hitam

    Properties
    {
        _Color ("Warna", Color) = (0,0,0,1)
        _Radius ("Radius Lubang", Range(0, 1.5)) = 1.2
        _Softness ("Kehalusan Tepi", Range(0.001, 0.5)) = 0.05
        _Center ("Titik Tengah (UV)", Vector) = (0.5, 0.5, 0, 0)
    }

    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Transparent" "IgnoreProjector"="True" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off
        ZTest Always

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

            fixed4 _Color;
            float _Radius;
            float _Softness;
            float4 _Center;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float aspect = _ScreenParams.x / _ScreenParams.y;
                float2 uv = i.uv - _Center.xy;
                uv.x *= aspect;
                float dist = length(uv);

                // dist < radius-softness => transparan (0), dist > radius => hitam (1)
                float alpha = smoothstep(_Radius - _Softness, _Radius, dist);

                return fixed4(_Color.rgb, alpha);
            }
            ENDCG
        }
    }
}
