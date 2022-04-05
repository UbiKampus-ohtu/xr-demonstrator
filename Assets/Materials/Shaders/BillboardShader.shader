Shader "GUI/BillboardShader" {
  Properties {
      _Color("Color", Color) = (1,1,1,1)
      _MainTex ("Texture", 2D) = "white" {}
      [HDR] _EmissionColor ("_EmissionColor", Color) = (0,0,0)
      [Enum(Off,0,On,2)] _ZTestSetter ("ZTest", Int) = 1
      _VertexColor ("Vertex Color", Range(0,1)) = 0
  }
  SubShader {
    Tags { "RenderType"="Transparent" "Queue" = "Transparent+500" }
    LOD 100

    Pass {
      ZTest [_ZTestSetter]
      Blend SrcAlpha OneMinusSrcAlpha
      Cull Off

      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag
      
      // make fog work
      #pragma multi_compile_fog

      #include "UnityCG.cginc"

      struct appdata {
          float4 vertex : POSITION;
          float2 uv : TEXCOORD0;
          float4 color : COLOR;
      };

      struct v2f {
          float2 uv : TEXCOORD0;
          UNITY_FOG_COORDS(1)
          float4 vertex : SV_POSITION;
          float4 color : COLOR0;
      };

      fixed4 _Color;
      fixed4 _EmissionColor;
      float _VertexColor;
      sampler2D _MainTex;
      float4 _MainTex_ST;

      v2f vert (appdata v) {
          v2f o;

          float4 worldOrigin = mul(UNITY_MATRIX_M, float4(0, 0, 0, 1));
          float4 viewOrigin = float4(UnityObjectToViewPos(float3(0,0,0)), 1);

          float4 worldPosition = mul(UNITY_MATRIX_M, v.vertex);
          float4 flippedWorldPosition = float4(-1, 1, -1, 1) * (worldPosition - worldOrigin) + worldOrigin;
          
          float4 viewPosition = flippedWorldPosition - worldOrigin + viewOrigin;
          float4 clipPosition = mul(UNITY_MATRIX_P, viewPosition);

          o.vertex = clipPosition;

          o.color = v.color;

          o.uv = TRANSFORM_TEX(v.uv, _MainTex);
          UNITY_TRANSFER_FOG(o,o.vertex);
          return o;
      }

      fixed4 frag (v2f i) : SV_Target {
        fixed4 col;
        fixed4 tex = tex2D(_MainTex, i.uv);

        col.rgb = (tex.rgb * (1 - _VertexColor) * _Color) + (i.color.rgb * _VertexColor);
        col.a = tex.a;
        col *= _EmissionColor;
        UNITY_APPLY_FOG(i.fogCoord, col);
        return col;
      }
      ENDCG
    }
  }
}
