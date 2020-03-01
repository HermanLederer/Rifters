Shader "Unlit/HelloCG"
{
	Properties
	{
		_Color("Color", Color) = (0.5, 0.5, 0.5, 1)
		_Metalness("Metalness", Range(0, 1)) = 0.5
		_Smoothness("Smoothness", Range(1, 64)) = 0.5
	}
	
	SubShader
	{
		Tags
		{
			"RenderType" = "Opaque"
			"Queue" = "Transparent"
		}
		//LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"
			#include "AutoLight.cginc"

			// Data structures			
			struct vertIn
			{
				float4 vertex : POSITION; // for vertext position
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct fragIn
			{
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION; // for clip space position
				float3 worldPos : TEXCOORD3;
				float2 uv : TEXCOORD0;
				float3 normal : TEXCOORD2;
			};

			// Variables
			//sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;
			float _Metalness;
			float _Smoothness;

			// Vertex shader
			fragIn vert(vertIn v)
			{
				fragIn o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.uv = v.uv;
				o.normal = v.normal;
				UNITY_TRANSFER_FOG(o, o.vertex);
				return o;
			}

			// Fragment shader
			float4 frag(fragIn i) : SV_Target
			{
				i.normal = normalize(i.normal);

				// Specular lighting
				float3 camPos = _WorldSpaceCameraPos;
				float3 viewDir = normalize(camPos - i.worldPos);

				float3 reflectedViewDir = reflect(-viewDir, i.normal);
				float specularLighting = max(0, dot(_WorldSpaceLightPos0.xyz, reflectedViewDir));
				specularLighting = pow(specularLighting, _Smoothness); // changing smoothness

				// Direct lighting
				float3 lightColor = float3(1, 1, 1);
				float3 lightDirection = _WorldSpaceLightPos0.xyz;

				float3 skyLightColor = float3(0, 0.06, 0.2);
				float3 skyLightDirection = normalize(float3(0, -1, 0));

				float3 goundLightColor = float3(0.01, 0.1, 0.02);
				float3 groundLightDirection = normalize(float3(0, 1, 0));

				float3 normal = i.normal;

				float3 lighting = max(0, dot(lightDirection, normal));
				float3 skyLighting = max(0, dot(skyLightDirection, normal));
				float3 groundLighting = max(0, dot(groundLightDirection, normal));

				float3 light = 0;
				light += lightColor * lerp(lighting, specularLighting, _Metalness); // sun
				light += skyLightColor * skyLighting; // sky
				light += goundLightColor * groundLighting; // gound
				light *= _Color; // diffuse

				

				// apply fog
				float4 output = float4(light, 1);
				UNITY_APPLY_FOG(i.fogCoord, output);
				return output;
			}
			ENDCG
		}
	}
}
