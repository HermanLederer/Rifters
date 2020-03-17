// Point cloud fragment shader that attempts to recreate Unity's HDRP Lit framgent shader

//
//
// Fragment shader
float4 PointCloudFrag(g2f IN) : COLOR
{
    float4 diffuse = float4(1, 1, 1, 1);//tex2D(_MainTex, IN.uv) * _Color;
    //clip(diffuse.a - _AlphaCutoff);

    float4 result = diffuse;
    return float4(result);
}