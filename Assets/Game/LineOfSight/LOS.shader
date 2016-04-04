Shader "Custom/LOS" 
{
    Properties 
    {
        // other properties like colors or vectors go here as well
    }
    SubShader 
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry-1"}

        ColorMask 0
        ZWrite off

        Stencil 
        {
            Ref 1
            Pass replace
            Comp always
        }

        Pass 
        {
            Cull back
            ZTest Less

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            struct appdata 
            {
                float4 vertex : POSITION;
            };
            struct v2f 
            {
                float4 pos : SV_POSITION;
            };
            v2f vert(appdata v) 
            {
                v2f o;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            half4 frag(v2f i) : SV_Target 
            {
                return (1,1,1,1);
            }
            ENDCG
        }
    } 
}