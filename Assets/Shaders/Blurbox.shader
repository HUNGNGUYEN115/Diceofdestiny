Shader "Custom/BoxBlur"
{
    Properties
    {
        _Blur ("Blur strength (size of filter (2n+1)^2)", Integer) = 1
        _Scale ("Scale (texel offset)", Range(1, 5)) = 1
        _ColorTint ("Color Tint (A = strength)", Color) = (0,0,0,0.5)
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }

        Pass
        {
            Name "BoxBlur"
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float4 screenPos   : TEXCOORD0;
            };

            TEXTURE2D(_CameraOpaqueTexture);
            SAMPLER(sampler_CameraOpaqueTexture);

            CBUFFER_START(UnityPerMaterial)
                int _Blur;
                float _Scale;
                float4 _ColorTint; // RGB = color, A = strength
            CBUFFER_END

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.screenPos = ComputeScreenPos(OUT.positionHCS);
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                float4 blurred = 0.0;

                float2 uv = IN.screenPos.xy / IN.screenPos.w;
                float2 texel = _Scale * (1.0 / _ScreenParams.xy);

                int blurSize = max(_Blur, 1);
                int sampleCount = (2 * blurSize + 1) * (2 * blurSize + 1);

                for (int x = -blurSize; x <= blurSize; x++)
                {
                    for (int y = -blurSize; y <= blurSize; y++)
                    {
                        blurred += SAMPLE_TEXTURE2D(
                            _CameraOpaqueTexture,
                            sampler_CameraOpaqueTexture,
                            uv + float2(x, y) * texel
                        );
                    }
                }

                blurred /= sampleCount;

                // Apply tint using alpha as strength
                float3 tinted = blurred.rgb * _ColorTint.rgb;
                float strength = saturate(_ColorTint.a);

                float3 finalColor = lerp(blurred.rgb, tinted, strength);

                return half4(finalColor, 1.0);
            }
            ENDHLSL
        }
    }
}
