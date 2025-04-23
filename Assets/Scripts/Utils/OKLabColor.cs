
using UnityEngine;

public static class OKLabColor {

    // Convertir de RGB lineal a OKLab
    public static Vector3 RGBToOKLab(Color rgb) {
        // Asegurarse de usar espacio lineal
        float r = rgb.r;
        float g = rgb.g;
        float b = rgb.b;

        // Paso 1: RGB -> LMS
        float l = 0.4122214708f * r + 0.5363325363f * g + 0.0514459929f * b;
        float m = 0.2119034982f * r + 0.6806995451f * g + 0.1073969566f * b;
        float s = 0.0883024619f * r + 0.2817188376f * g + 0.6299787005f * b;

        // Paso 2: LMS ^ 1/3
        l = Mathf.Pow(l, 1f / 3f);
        m = Mathf.Pow(m, 1f / 3f);
        s = Mathf.Pow(s, 1f / 3f);

        // Paso 3: LMS -> OKLab
        float L = 0.2104542553f * l + 0.7936177850f * m - 0.0040720468f * s;
        float A = 1.9779984951f * l - 2.4285922050f * m + 0.4505937099f * s;
        float B = 0.0259040371f * l + 0.7827717662f * m - 0.8086757660f * s;

        return new Vector3(L, A, B);
    }

    // Convertir de OKLab a RGB lineal
    public static Color OKLabToRGB(Vector3 lab) {
        float L = lab.x;
        float A = lab.y;
        float B = lab.z;

        // OKLab -> LMS
        float l = L + 0.3963377774f * A + 0.2158037573f * B;
        float m = L - 0.1055613458f * A - 0.0638541728f * B;
        float s = L - 0.0894841775f * A - 1.2914855480f * B;

        // LMS ^ 3
        l = l * l * l;
        m = m * m * m;
        s = s * s * s;

        // LMS -> RGB
        float r = +4.0767416621f * l - 3.3077115913f * m + 0.2309699292f * s;
        float g = -1.2684380046f * l + 2.6097574011f * m - 0.3413193965f * s;
        float b = -0.0041960863f * l - 0.7034186147f * m + 1.7076147010f * s;

        return new Color(r, g, b, 1f);
    }

    // Aplica un cambio de luminosidad en espacio OKLab
    public static Color AdjustLightness(Color rgb, float deltaL) {
        Vector3 lab = RGBToOKLab(rgb);
        lab.x = Mathf.Clamp01(lab.x + deltaL);
        return OKLabToRGB(lab);
    }
}
