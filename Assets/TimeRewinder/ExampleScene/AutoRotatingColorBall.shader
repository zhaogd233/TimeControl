Shader "Custom/AutoRotatingColorBall"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _RotateSpeed ("Rotation Speed (deg/sec)", Range(-360, 360)) = 30
        _Color ("Tint Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Back

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _RotateSpeed;
            float4 _Color;

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

                // 根据时间计算旋转角度
                float rotDeg = _Time.y * _RotateSpeed; // _Time.y 是 t/20
                float rad = radians(rotDeg);
                float s = sin(rad);
                float c = cos(rad);

                // UV 旋转
                float2 uv = TRANSFORM_TEX(v.uv, _MainTex);
                float2 center = float2(0.5, 0.5);
                uv -= center;
                uv = float2(
                    uv.x * c - uv.y * s,
                    uv.x * s + uv.y * c
                );
                uv += center;

                o.uv = uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texCol = tex2D(_MainTex, i.uv);
                texCol *= _Color; // 颜色 + 透明度
                return texCol;
            }
            ENDCG
        }
    }
}
