// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/FogOfSanity"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        
        // Custom Properties
        _FogRadius ("FogRadius", Range(1,500)) = 1.0
        _FogMaxRadius ("FogMaxRadius", Range(0.1,10)) = 0.5
        _PlayerPosition ("PlayerPostion", Vector) = (0,0,0,1)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200
        Cull Off

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert alpha:blend

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        
        // Custom Properties
        float _FogRadius;
        float _FogMaxRadius;
        float4 _PlayerPosition;

        struct Input
        {
            float2 uv_MainTex;
            float2 location;
        };
        
        float powerForPos(float4 position, float2 nearVertex);

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)
        
        void vert(inout appdata_full vertexData, out Input outData)
        {
            float4 position = UnityObjectToClipPos(vertexData.vertex);
            float4 positionWorld = mul(unity_ObjectToWorld, vertexData.vertex);
            outData.uv_MainTex = vertexData.texcoord;
            outData.location = positionWorld.xz;
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            float alpha = (1.0 - (c.a + powerForPos(_PlayerPosition, IN.location)));
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = alpha;
        }
        
        // return 0 if (position - nearVertex > _FogRadius)
        float powerForPos(float4 position, float2 nearVertex)
        {
            float atten = clamp(_FogRadius - length(position.xz - nearVertex.xy), 0.0, _FogRadius);
            return (1.0/_FogMaxRadius) * atten / _FogRadius;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
