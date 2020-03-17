// Point cloud geometry shader that adds planes

//
//
// Includes
#ifndef UNITY_SHADER_UTILITIES_INCLUDED
#define UNITY_SHADER_UTILITIES_INCLUDED

#include "UnityShaderVariables.cginc"

float3 ODSOffset(float3 worldPos, float ipd)
{
    //based on google's omni-directional stereo rendering thread
    const float EPSILON = 2.4414e-4;
    float3 worldUp = float3(0.0, 1.0, 0.0);
    float3 camOffset = worldPos.xyz - _WorldSpaceCameraPos.xyz;
    float4 direction = float4(camOffset.xyz, dot(camOffset.xyz, camOffset.xyz));
    direction.w = max(EPSILON, direction.w);
    direction *= rsqrt(direction.w);

    float3 tangent = cross(direction.xyz, worldUp.xyz);
    if (dot(tangent, tangent) < EPSILON)
        return float3(0, 0, 0);
    tangent = normalize(tangent);

    float directionMinusIPD = max(EPSILON, direction.w*direction.w - ipd*ipd);
    float a = ipd * ipd / direction.w;
    float b = ipd / direction.w * sqrt(directionMinusIPD);
    float3 offset = -a*direction + b*tangent;
    return offset;
}

inline float4 UnityObjectToClipPosODS(float3 inPos)
{
    float4 clipPos;
    float3 posWorld = mul(UNITY_MATRIX_M, float4(inPos, 1.0)).xyz;
#if defined(STEREO_CUBEMAP_RENDER_ON)
    float3 offset = ODSOffset(posWorld, unity_HalfStereoSeparation.x);
    clipPos = mul(UNITY_MATRIX_VP, float4(posWorld + offset, 1.0));
#else
    clipPos = mul(UNITY_MATRIX_VP, float4(posWorld, 1.0));
#endif
    return clipPos;
}

// Tranforms position from object to homogenous space
inline float4 UnityObjectToClipPos(in float3 pos)
{
#if defined(STEREO_CUBEMAP_RENDER_ON)
    return UnityObjectToClipPosODS(pos);
#else
    // More efficient than computing M*VP matrix product
    return mul(UNITY_MATRIX_VP, mul(UNITY_MATRIX_M, float4(pos, 1.0)));
#endif
}

inline float4 UnityObjectToClipPos(float4 pos) // overload for float4; avoids "implicit truncation" warning for existing shaders
{
    return UnityObjectToClipPos(pos.xyz);
}

#endif

//
//
// Data structures
struct v2g
{
    float4 pos : SV_POSITION;
    float3 norm : NORMAL;
    float2 uv : TEXCOORD0;

};

struct g2f
{
    float4 pos : SV_POSITION;
    float3 norm : NORMAL;
    float2 uv : TEXCOORD0;
};

//
//
// Parameters
//fixed4 _Color;
half _GrassHeight;
half _GrassWidth;

//
//
// Vertex output from geometry
PackedVaryingsType VertexOutput(AttributesMesh source, float3 position, float3 position_prev, half3 normal, half emission = 0, half random = 0, half2 qcoord = -1)
{
    half4 color = half4(qcoord, emission, random);
    return PackVertexData(source, position, position_prev, normal, color);
}

//
//
// Geometry shader
[maxvertexcount(4)]
//void FlattenerGeometry(uint pid : SV_PrimitiveID, triangle Attributes input[3], inout TriangleStream<PackedVaryingsType> outStream)
void PointCloudGeom(point Attributes input[3], uint pid : SV_PrimitiveID, inout TriangleStream<PackedVaryingsType> outStream)
{     
    //
    //  
    // HDRP way
    AttributesMesh inputVertex0 = ConvertToAttributesMesh(input[0]);
    outStream.Append(VertexOutput(inputVertex0, p0, p0_prev, n0));

    //float3 v0 = IN[0].positionOS;
    //float3 v1 = IN[0].pos.xyz + IN[0].norm * 1;

    //float3 color = float3(1, 1, 1);

    // g2f o;

    // o.pos = UnityObjectToClipPos(float4(0.5, 0, 0, 1));
    // o.norm = float3(1, 0, 1);
    // triStream.Append(o);

    // o.pos = UnityObjectToClipPos(float4(-0.5, 0, 0, 1));
    // o.norm = float3(1, 1, 1);
    // triStream.Append(o);

    // o.pos = UnityObjectToClipPos(float4(0, 1, 0, 1));
    // o.norm = float3(0, 0, 1);
    // triStream.Append(o);


    //AttributesMesh v0 = ConvertToAttributesMesh(IN[0]);
    //outStream.Append(VertexOutput(IN, float3(0.5, 0, 0), float3(0, 0, 1)));

    // adding vertecies to form a quad
    //g2f OUT;

    
    //float3 perpendicularAngle = float3(1, 0, 0);
    //float3 faceNormal;

    // one side
    //faceNormal = cross(perpendicularAngle, IN[0].norm);

    //OUT.pos = UnityObjectToClipPos(v0 + perpendicularAngle * 0.5 * 1.5);
    //OUT.norm  = faceNormal;
    //OUT.uv = float2(1, 0);
    //triStream.Append(OUT);

    //OUT.pos = UnityObjectToClipPos(v0 - perpendicularAngle * 0.5 * 1.5);
    //OUT.norm  = faceNormal;
    //OUT.uv = float2(0, 0);
    //triStream.Append(OUT);

    //OUT.pos = UnityObjectToClipPos(v1 + perpendicularAngle * 0.5 * 1.5);
    //OUT.norm = faceNormal;
    //OUT.uv = float2(1, 1);
    //triStream.Append(OUT);

    //OUT.pos = UnityObjectToClipPos(v1 - perpendicularAngle * 0.5 * 1.5);
    //OUT.norm = faceNormal;
    //OUT.uv = float2(0, 1);
    //triStream.Append(OUT);
}