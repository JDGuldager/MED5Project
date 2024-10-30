Shader "Custom/TransparentHeadShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,0) // Default color is fully transparent
    }
    SubShader
    {
        Tags { "Queue" = "Overlay" "RenderType" = "Transparent" }
        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha // Enable transparency blending
        Cull Off                        // Render both sides if necessary
        ZWrite Off                      // Do not write to depth buffer

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return _Color; // Use color with alpha for transparency
            }
            ENDCG
        }
    }
    FallBack "Transparent/VertexLit"
}
