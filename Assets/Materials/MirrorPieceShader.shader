Shader "Unlit/NewUnlitShader"
{
	Properties{
		_IsActive("Active", Float) = 0.5
		_CubeLeft("Left Eye Map", Cube) = "" {}
		_CubeRight("Right Eye Map", Cube) = "" {}
		_Color("Inactive Color", Color) = (1,1,1,1)
		_ViewVector("View Vector",Vector ) = (1,1,1)
		_RightVector("Right Vector",Vector) = (1,1,1)
	}
		SubShader{
		   Pass {
			  CGPROGRAM

			  #pragma vertex vert  
			  #pragma fragment frag

			  #include "UnityCG.cginc"

			  // User-specified uniforms
			  uniform samplerCUBE _CubeLeft;
				uniform samplerCUBE _CubeRight;
				fixed4 _Color;
				float _IsActive;
				float3 _ViewVector;
				float3 _RightVector;

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
				 float3 right = UNITY_MATRIX_V[0].xyz;
				 float3 camPos = _WorldSpaceCameraPos;

				 //output.viewDir = _ViewVector;//mul(modelMatrix, input.vertex).xyz
					//- (camPos);

				 float3 rightOffset = float3(1,1,1);

				 if(unity_StereoEyeIndex == 0)
				  {
					  rightOffset = _RightVector*0.008;
				 
				  }
				  else
				  {
					 rightOffset = -_RightVector * 0.008;
				 }

				 output.viewDir = mul(modelMatrix, input.vertex).xyz
					- (_ViewVector+ rightOffset);
				 output.normalDir = normalize(
					mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
				 output.pos = UnityObjectToClipPos(input.vertex);
				 return output;
			  }

			  float4 frag(vertexOutput input) : COLOR
			  {
				  float4 cube;
				  //if(unity_StereoEyeIndex == 0)
				  //{
					  //float3 right = UNITY_MATRIX_V[0].xyz;
					  cube = texCUBE(_CubeLeft, input.viewDir);
				 // }
				  //else
				  //{
					  //float3 right = UNITY_MATRIX_V[0].xyz;
					  cube = texCUBE(_CubeLeft, input.viewDir);
				  //}
				  

				 float4 result = cube*_IsActive + _Color * (1 - _IsActive);
				 //float3 reflecteмdDir =
				 //	reflect(input.viewDir, normalize(input.normalDir));
				 return result;
			  }

			  ENDCG
		   }
	}
}