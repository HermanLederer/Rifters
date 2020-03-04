Shader "Custom/DynamicGrassPointCloudShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _GrassWidth("Grass width", Float) = 0.25
        _GrassHeight("Grass height", Float) = 0.25
        _AlphaCutoff("Alpha cutoff", Float) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        CULL OFF

        Pass
        {
        	CGPROGRAM
        
	        #include "UnityCG.cginc"
	        #pragma vertex vert
	        #pragma fragment frag
	        #pragma geometry geom

	        #pragma target 4.0

	        sampler2D _MainTex;

	        //
	        //
	        // Data structures
			struct v2g
			{
				float4 pos : SV_POSITION;
				float3 norm : NORMAL;
				float2 uv : TEXCOORD0;
				float3 color : TEXCOORD1;
			};

			struct g2f
			{
				float4 pos : SV_POSITION;
				float3 norm : NORMAL;
				float2 uv : TEXCOORD0;
				float3 diffuseColor : TEXCOORD1;
			};

	        //
	        //
	        // Parameters
	        half _Glossiness;
	        half _Metallic;
	        fixed4 _Color;
	        half _GrassHeight;
	        half _GrassWidth;
	        half _AlphaCutoff;

	        //
	        //
	        // Vertex shader
	        v2g vert(appdata_full v)
	        {
	        	float3 v0 = v.vertex.xyz;

	        	v2g OUT;
	        	OUT.pos = v.vertex;
	        	OUT.norm = v.normal;
	        	OUT.uv = v.texcoord;
	        	//OUT.color = tex2Dlod(_MainTex, v.texcoord.rgba);
	        	return OUT;
	        }

	        //
	        //
	        // Geometry shader
	        [maxvertexcount(4)]
	        void geom(point v2g IN[1], inout TriangleStream<g2f> triStream)
	        {
	        	float3 lightPosition = _WorldSpaceLightPos0;

	        	float3 perpendicularAngle = float3(1, 0, 0);
	        	float3 faceNormal = cross(perpendicularAngle, IN[0].norm);

	        	float3 v0 = IN[0].pos.xyz;
	        	float3 v1 = IN[0].pos.xyz + IN[0].norm * _GrassHeight;

	        	float3 color = float3(1, 1, 1);

	        	// adding vertecies to form a quad
	        	g2f OUT;

	        	OUT.pos = UnityObjectToClipPos(v0 + perpendicularAngle * 0.5 * _GrassWidth);
	        	OUT.norm = faceNormal;
	        	OUT.diffuseColor = color;
	        	OUT.uv = float2(1, 0);
	        	triStream.Append(OUT);

	        	OUT.pos = UnityObjectToClipPos(v0 - perpendicularAngle * 0.5 * _GrassWidth);
	        	OUT.norm = faceNormal;
	        	OUT.diffuseColor = color;
	        	OUT.uv = float2(0, 0);
	        	triStream.Append(OUT);

	        	OUT.pos = UnityObjectToClipPos(v1 + perpendicularAngle * 0.5 * _GrassWidth);
	        	OUT.norm = faceNormal;
	        	OUT.diffuseColor = color;
	        	OUT.uv = float2(1, 1);
	        	triStream.Append(OUT);

	        	OUT.pos = UnityObjectToClipPos(v1 - perpendicularAngle * 0.5 * _GrassWidth);
	        	OUT.norm = faceNormal;
	        	OUT.diffuseColor = color;
	        	OUT.uv = float2(0, 1);
	        	triStream.Append(OUT);
	        }

			//
	        //
	        // Fragment shader
	        half4 frag (g2f IN) : COLOR
	        {
	        	fixed4 diffuse = tex2D(_MainTex, IN.uv) * _Color;
	        	clip(diffuse.a - _AlphaCutoff);
	            return diffuse;
	        }

	        ENDCG
        }
    }
}
