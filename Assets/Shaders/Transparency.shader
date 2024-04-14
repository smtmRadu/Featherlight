Shader "Custom/Transparency"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

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

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}

Material cop = new Material(Shader.Find("Custom/Transparency"));
cop.SetFloat("_Mode", 2); // 2 corresponds to fade mode
cop.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
cop.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
cop.DisableKeyword("_ALPHATEST_ON");
cop.DisableKeyword("_ALPHABLEND_ON");
cop.EnableKeyword("_ALPHAPREMULTIPLY_ON");
cop.renderQueue = 3000;
