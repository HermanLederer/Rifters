// Regenerator effect geometry shader
// https://github.com/keijiro/TestbedHDRP

// Vertex output from geometry
PackedVaryingsType VertexOutput(
    AttributesMesh source,
    float3 position, float3 position_prev, half3 normal, float2 uv,
    half emission = 0, half random = 0, half2 qcoord = -1
)
{
    half4 color = half4(qcoord, emission, random);
    return PackVertexDataFull(source, position, position_prev, normal, uv, color);
}

// Geometry shader function body
[maxvertexcount(8)]
void RegeneratorGeometry(
    uint primitiveID : SV_PrimitiveID,
    point Attributes input[1],
    inout TriangleStream<PackedVaryingsType> outStream
)
{
    // Input vertices
    AttributesMesh v0 = ConvertToAttributesMesh(input[0]);
    //AttributesMesh v1 = ConvertToAttributesMesh(input[1]);
    //AttributesMesh v2 = ConvertToAttributesMesh(input[2]);

    float3 p0 = v0.positionOS;
    //float3 p1 = v1.positionOS;
    //float3 p2 = v2.positionOS;

    #if SHADERPASS == SHADERPASS_VELOCITY
        bool hasDeformation = unity_MotionVectorsParams.x > 0.0;
        float3 p0_prev = hasDeformation ? input[0].previousPositionOS : p0;
        //float3 p1_prev = hasDeformation ? input[1].previousPositionOS : p1;
        //float3 p2_prev = hasDeformation ? input[2].previousPositionOS : p2;
    #else
        float3 p0_prev = p0;
        //float3 p1_prev = p1;
        //float3 p2_prev = p2;
    #endif

    #ifdef ATTRIBUTES_NEED_NORMAL
        float3 n0 = v0.normalOS;
        //float3 n1 = v1.normalOS;
        //float3 n2 = v2.normalOS;
    #else
        float3 n0 = 0;
        //float3 n1 = 0;
        //float3 n2 = 0;
    #endif

    float3 p;
    float n;

    // x plane
    n = float3(1, 0, 0);

    p = float3(-0.1, 0, 0);
    outStream.Append(VertexOutput(v0, p0 + p, p0_prev + p, n, float2(0, 0)));

    p = float3(0.1, 0, 0);
    outStream.Append(VertexOutput(v0, p0 + p, p0_prev + p, n, float2(1, 0)));

    p = float3(-0.1, 0.2, 0);
    outStream.Append(VertexOutput(v0, p0 + p, p0_prev + p, n, float2(0, 1)));

    p = float3(0.1, 0.2, 0);
    outStream.Append(VertexOutput(v0, p0 + p, p0_prev + p, n, float2(1, 1)));

    outStream.RestartStrip();

    // z plane
    n = float3(-1, 0, 0);

    p = float3(0, 0, -0.1);
    outStream.Append(VertexOutput(v0, p0 + p, p0_prev + p, n, float2(0, 0)));

    p = float3(0, 0, 0.1);
    outStream.Append(VertexOutput(v0, p0 + p, p0_prev + p, n, float2(1, 0)));

    p = float3(0, 0.2, -0.1);
    outStream.Append(VertexOutput(v0, p0 + p, p0_prev + p, n, float2(0, 1)));

    p = float3(0, 0.2, 0.1);
    outStream.Append(VertexOutput(v0, p0 + p, p0_prev + p, n, float2(1, 1)));

    outStream.RestartStrip();
}
