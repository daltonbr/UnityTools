Shader "ColorChooser/HSLShader" {

    Properties {
        _Color("Main Color", Color) = (1.0, 0.0, 0.0, 1)
    }
    // End of Properties

    SubShader{
        Lighting Off
        Fog{ Mode Off }

        Tags {
            "Queue" = "Overlay"
            "RenderType" = "Opaque"
        }
        // End of Tags

        Pass {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            float4 _Color;

            struct vertInput {
                float4 pos : POSITION;
                float4 tex : TEXCOORD0;
            };

            struct vertOutput {
                float4 pos   : SV_POSITION;
                float4 color : TEXCOORD0;
            };

            vertOutput vert(vertInput input) {
                vertOutput o;
                o.pos = mul(UNITY_MATRIX_MVP, input.pos);
                o.color = float4(input.tex.x, input.tex.y, 0.0, 0.0);
                return o;
            }
            
            half4 frag(vertOutput output) : COLOR {
                half4 color = output.color.y + (_Color - 1) * output.color.x;
                return color;
            }

            ENDCG
        }
        // End of Pass
    
    }
    // End of SubShader

}
// End of Shader.