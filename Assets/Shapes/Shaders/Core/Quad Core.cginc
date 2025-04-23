
// Shapes © Freya Holmér - modificado para usar OKLab
#include "UnityCG.cginc"
#include "../Shapes.cginc"
#pragma target 3.0

UNITY_INSTANCING_BUFFER_START(Props)
PROP_DEF( half4, _Color)
PROP_DEF( half4, _ColorB)
PROP_DEF( half4, _ColorC)
PROP_DEF( half4, _ColorD)
PROP_DEF( float3, _A)
PROP_DEF( float3, _B)
PROP_DEF( float3, _C)
PROP_DEF( float3, _D)
UNITY_INSTANCING_BUFFER_END(Props)

float3 RGBToOKLab(float3 c) {
    float l = 0.4122214708 * c.r + 0.5363325363 * c.g + 0.0514459929 * c.b;
    float m = 0.2119034982 * c.r + 0.6806995451 * c.g + 0.1073969566 * c.b;
    float s = 0.0883024619 * c.r + 0.2817188376 * c.g + 0.6299787005 * c.b;
    l = pow(l, 1.0 / 3.0);
    m = pow(m, 1.0 / 3.0);
    s = pow(s, 1.0 / 3.0);
    return float3(
        0.2104542553 * l + 0.7936177850 * m - 0.0040720468 * s,
        1.9779984951 * l - 2.4285922050 * m + 0.4505937099 * s,
        0.0259040371 * l + 0.7827717662 * m - 0.8086757660 * s
    );
}

float3 OKLabToRGB(float3 lab) {
    float l = lab.x + 0.3963377774 * lab.y + 0.2158037573 * lab.z;
    float m = lab.x - 0.1055613458 * lab.y - 0.0638541728 * lab.z;
    float s = lab.x - 0.0894841775 * lab.y - 1.2914855480 * lab.z;
    l = l * l * l;
    m = m * m * m;
    s = s * s * s;
    return float3(
        +4.0767416621 * l - 3.3077115913 * m + 0.2309699292 * s,
        -1.2684380046 * l + 2.6097574011 * m - 0.3413193965 * s,
        -0.0041960863 * l - 0.7034186147 * m + 1.7076147010 * s
    );
}

struct VertexInput {
    float4 vertex : POSITION;
    float4 color : COLOR;
    float2 uv : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};
struct VertexOutput {
    float4 pos : SV_POSITION;
    half2 uv : TEXCOORD0;
    UNITY_FOG_COORDS(1)
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};

VertexOutput vert (VertexInput v) {
    UNITY_SETUP_INSTANCE_ID(v);
    VertexOutput o = (VertexOutput)0;
    UNITY_TRANSFER_INSTANCE_ID(v, o);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

    float3 A = PROP(_A);
    float3 B = PROP(_B);
    float3 C = PROP(_C);
    float3 D = PROP(_D);
    v.vertex.xyz = A * v.color.r + B * v.color.g + C * v.color.b + D * v.color.a;
    o.uv = v.uv * 0.5 + 0.5;

    o.pos = UnityObjectToClipPos(v.vertex);
    UNITY_TRANSFER_FOG(o, o.pos);
    return o;
}

FRAG_OUTPUT_V4 frag(VertexOutput i) : SV_Target {
    UNITY_SETUP_INSTANCE_ID(i);

    half2 uv = i.uv;
    half4 colorA = PROP(_Color);
    half4 colorB = PROP(_ColorB);
    half4 colorC = PROP(_ColorC);
    half4 colorD = PROP(_ColorD);

    float3 labA = RGBToOKLab(colorA.rgb);
    float3 labB = RGBToOKLab(colorB.rgb);
    float3 labC = RGBToOKLab(colorC.rgb);
    float3 labD = RGBToOKLab(colorD.rgb);

    float3 left = lerp(labA, labB, uv.y);
    float3 right = lerp(labD, labC, uv.y);
    float3 finalLab = lerp(left, right, uv.x);
    float3 rgb = OKLabToRGB(finalLab);

    float alpha = lerp(lerp(colorA.a, colorB.a, uv.y), lerp(colorD.a, colorC.a, uv.y), uv.x);
    return half4(rgb, alpha);
}