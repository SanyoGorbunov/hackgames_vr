Shader "Unlit/NewUnlitShader"
{
	Properties{
		_IsActive("Active", Range(0,1)) = 0.5
	   _Cube("World Map", Cube) = "" {}
		_Color("Inactive Color", Color) = (1,1,1,1)
	}
		SubShader{
		   Pass {
			  CGPROGRAM

			  #pragma vertex vert  
			  #pragma fragment frag

			  #include "UnityCG.cginc"

			  // User-specified uniforms
			  uniform samplerCUBE _Cube;
				fixed4 _Color;
				float _IsActive;

			  struct vertexInput {
				 float4 vertex : POSITION;
				 float3 normal : NORMAL;
			  };
			  struct vertexOutput {
				 float4 pos : SV_POSITION;
				 float3 normalDir : TEXCOORD0;
				 float3 viewDir : TEXCOORD1;
			  };

			  vertexOutput vert(vertexInput input)
			  {
				 vertexOutput output;

				 float4x4 modelMatrix = unity_ObjectToWorld;
				 float4x4 modelMatrixInverse = unity_WorldToObject;

				 output.viewDir = mul(modelMatrix, input.vertex).xyz
					- _WorldSpaceCameraPos;
				 output.normalDir = normalize(
					mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
				 output.pos = UnityObjectToClipPos(input.vertex);
				 return output;
			  }

			  float4 frag(vertexOutput input) : COLOR
			  {
				 float4 cube = texCUBE(_Cube, input.viewDir);

				 float4 result = cube* (1- _IsActive) + _Color * _IsActive;
				 //float3 reflecteмdDir =
				 //	reflect(input.viewDir, normalize(input.normalDir));
				 return result;
			  }

			  ENDCG
		   }
	}
}