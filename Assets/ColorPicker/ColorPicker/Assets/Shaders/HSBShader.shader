// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

/*
*   The MIT License (MIT) - Copyright (c) 2016 M Hillman
*
*   Permission is hereby granted, free of charge, to any person obtaining a copy
*   of this software and associated documentation files (the "Software"), to deal
*   in the Software without restriction, including without limitation the rights
*   to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
*   copies of the Software, and to permit persons to whom the Software is
*   furnished to do so, subject to the following conditions:
*
*   The above copyright notice and this permission notice shall be included in all
*   copies or substantial portions of the Software.
*
*   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
*   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
*   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
*   AUTHOR/COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
*   WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF
*   OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

/*
* Custom shader to render the HSB Swatch as a grid of colors with a set hue (based on the
* _Color variable) with varying saturation and brightness.
*/
Shader "ColorChooser/HSLShader" {

    Properties {
        _Color("Main Color", Color) = (1.0, 0.0, 0.0, 1)
    }
    // End of Properties

    SubShader{
        Lighting Off

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
                o.pos = UnityObjectToClipPos(input.pos);
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
// End of Shader