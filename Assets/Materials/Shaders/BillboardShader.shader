Shader "GUI/BillboardShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent+500" }
        LOD 100

        Pass
        {
            ZTest Off
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;

                float4 worldOrigin = mul(UNITY_MATRIX_M, float4(0, 0, 0, 1));
                float4 viewOrigin = float4(UnityObjectToViewPos(float3(0,0,0)), 1);

                float4 worldPosition = mul(UNITY_MATRIX_M, v.vertex);
                float4 flippedWorldPosition = float4(-1, 1, -1, 1) * (worldPosition - worldOrigin) + worldOrigin;
                
                float4 viewPosition = flippedWorldPosition - worldOrigin + viewOrigin;
                float4 clipPosition = mul(UNITY_MATRIX_P, viewPosition);

                o.vertex = clipPosition;

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
