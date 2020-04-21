    Shader "Unlit/Invisible"
    {
        SubShader
        {
            ZWrite Off
            ColorMask 0
            Pass
            {
                ZWrite On
                Color (1,1,1,1)
            }  
        }
    }
