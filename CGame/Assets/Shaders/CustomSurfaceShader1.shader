Shader "Custom/CustomSurfaceShader" {

	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_Ramp ("Shading Ramp", 2D) = "gray" {}
	}

	SubShader {

		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}

		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM

		#pragma surface surf Lambert finalcolor:mycolor

		struct Input {
			float2 uv_MainTex;
			float3 viewDir;
		};

		sampler2D _MainTex;
		sampler2D _Ramp;
		float _RampOffset;

		void mycolor (Input IN, SurfaceOutput o, inout fixed4 color) {
			half rim = 1 - saturate(dot (normalize(IN.viewDir),o.Normal));
			half4 rimCol = tex2D (_Ramp, float2(rim,0));
			color.rgb = color.rgb * (1-rimCol.a) + rimCol.rgb * (rimCol.a);
			color.a = rimCol.a;
		}

		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
		}

		ENDCG

	}

}
