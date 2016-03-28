Shader "Custom/Shadow" 
{
    Properties 
    {
        _Color("Color", Color) = (1,1,1,1)
        // other properties like colors or vectors go here as well
    }
    SubShader 
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry"}
        Pass 
        {
            Stencil 
            {
                Ref 1
                Comp NotEqual
                Pass keep
            }
        
            CGPROGRAM
            half4 _MyColor;

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
                return _MyColor;
            }
            ENDCG
        }
    } 
}