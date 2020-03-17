// Point cloud geometry shader that makes planes out of points for our grass

//
//
// Includes


//
//
// Data structures

//
//
// Parameters
//fixed4 _Color;
half _GrassHeight;
half _GrassWidth;

//
//
// Vertex output from geometry
PackedVaryingsType VertexOutput(AttributesMesh source, float3 position, float3 position_prev, float3 normal, half emission = 0, half random = 0, half2 qcoord = -1)
{
    half4 color = half4(qcoord, emission, random);
    return PackVertexData(source, position, position_prev, normal, color);
}

//
//
// Geometry shader
[maxvertexcount(4)]
//void FlattenerGeometry(uint pid : SV_PrimitiveID, triangle Attributes input[3], inout TriangleStream<PackedVaryingsType> outStream)
void PointCloudGeom(uint primitiveID : SV_PrimitiveID, triangle Attributes input[3], inout TriangleStream<PackedVaryingsType> outStream)
{
    //
    //
    // Input

    //
    // Input vertecies
    AttributesMesh inputVertex0 = ConvertToAttributesMesh(input[0]);
    AttributesMesh inputVertex1 = ConvertToAttributesMesh(input[1]);
    AttributesMesh inputVertex2 = ConvertToAttributesMesh(input[2]);

    //
    // Attributes

    // Position
    float3 p0 = inputVertex0.positionOS;
    float3 p1 = inputVertex1.positionOS;
    float3 p2 = inputVertex2.positionOS;

    // Pevious position
    #if SHADERPASS == SHADERPASS_VELOCITY
        bool hasDeformation = unity_MotionVectorsParams.x > 0.0;
        float3 p0_prev = hasDeformation ? input[0].previousPositionOS : p0;
        float3 p1_prev = hasDeformation ? input[1].previousPositionOS : p1;
        float3 p2_prev = hasDeformation ? input[2].previousPositionOS : p2;
    #else
        float3 p0_prev = p0;
        float3 p1_prev = p1;
        float3 p2_prev = p2;
    #endif

    // Normal
    #ifdef ATTRIBUTES_NEED_NORMAL
        float3 n0 = inputVertex0.normalOS;
        float3 n1 = inputVertex1.normalOS;
        float3 n2 = inputVertex2.normalOS;
    #else
        float3 n0 = 0;
        float3 n1 = 0;
        float3 n2 = 0;
    #endif

    //
    //
    // Output
    float3 offset0 = float3(0, 1, 0);
    float3 offset1 = float3(0.5, 0, 0);
    float3 offset2 = float3(0, 100, 0);

    //p0 += offset0;
    //p0_prev += offset0;
    outStream.Append(VertexOutput(inputVertex0, offset0, offset0, n0));

    //p1 += offset1;
    //p1_prev += offset1;
    outStream.Append(VertexOutput(inputVertex1, offset1, offset1, n1));

    //p2 += offset2;
    //p2_prev += offset2;
    outStream.Append(VertexOutput(inputVertex2, offset2, offset2, n2));

    outStream.RestartStrip();
}