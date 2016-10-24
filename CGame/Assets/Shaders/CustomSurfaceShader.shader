Shader "Custom/CustomSurfaceShader2" {
	Properties { //'public' variables
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Amount ("Amount", Range(-0.05,0.1)) = 0 
		_Reflection ("Reflection", Range(0,1)) = 1
		_Gloss ("Gloss", Range(0,1)) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		
		CGPROGRAM
		// The shader uses a Lambertian lighting model, which is a very typical way of 
		// modelling how light reflects onto an object
		#pragma surface surf Lambert vertex:vert

		sampler2D _MainTex; //declaration of texture

		struct Input {
			float2 uv_MainTex; //UV data of the current pixel
		};

		//declaration of variables
		fixed4 _Color;
		float _Amount;
		half _Reflection;
		float _Gloss;

		// to make the object look chubbier we have to expand the triangles along the direction they’re facing

		// change vertices before sending them to surf
	    void vert (inout appdata_full v) {
	    	//newVertex = vertex + normal * amount to extend its vertices in the direction of its normal
	        v.vertex.xyz += v.normal * _Amount;
	    }

	    // surf: takes data from the 3D model as input, and outputs its rendering properties
	    // Surface shaders hide the calculations of how light is reflected and allows to specify “intuitive” properties
		// such as the albedo, the normals, the reflectivity and so on in a function called surf. 
		//These values are then plugged into a lighting model which will output the final RGB values for each pixel.
		void surf (Input IN, inout SurfaceOutput o) {
			//The struct SurfaceOutput has several other properties which can be used to determine the final aspect of a material:
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color; //text2d: given a texture and some UV coordinate, it returns the RGBA colour
			o.Specular = _Reflection;
			o.Albedo = c.rgb; //the base colour / texture of an object
			o.Gloss = _Gloss; //how diffuse the specular reflection is
			//o.Emission = _Emission; //how much light this object is generating by itself
			o.Alpha = c.a; //how transparent the material is
		}

		ENDCG
	}
	FallBack "Diffuse"
}
