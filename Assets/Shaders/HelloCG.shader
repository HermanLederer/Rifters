Shader "Unlit/HelloCG"
{
	Properties
	{
		//_MainTex("Texture", 2D) = "white" {}

	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		//LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"
			

			// Data structures			
			struct vertIn
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct fragIn
			{
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : TEXCOORD2;
			};

			// Variables
			//sampler2D _MainTex;
			float4 _MainTex_ST;

			// Vertex shader
			fragIn vert(vertIn v)
			{
				fragIn o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.normal = v.normal;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			// Fragment shader
			float4 frag(fragIn i) : SV_Target
			{
				float3 lightColor = float3(1, 0.9, 0.93);
				float3 lightDirection = normalize(float3(-1, -1, -1));

				float3 skyLightColor = float3(0, 0.06, 0.2);
				float3 skyLightDirection = normalize(float3(0, -1, 0));

				float3 goundLightColor = float3(0.01, 0.1, 0.02);
				float3 groundLightDirection = normalize(float3(0, 1, 0));

				float3 normal = i.normal;

				float3 lighting = max(0, -dot(lightDirection, normal));
				float3 skyLighting = max(0, -dot(skyLightDirection, normal));
				float3 groundLighting = max(0, -dot(groundLightDirection, normal));

				float3 light = 0;
				light += lightColor * lighting; // sun
				light += skyLightColor * skyLighting; // sky
				light += goundLightColor * groundLighting; // gound

				// apply fog
				float4 output = float4(light, 0);
				UNITY_APPLY_FOG(i.fogCoord, output);
				return output;
			}
			ENDCG
		}
	}
}
