struct ScharrOperators
{
    float3x3 x;
    float3x3 y;
};

//========================================================
//     Scharr X         ||         Scharr Y
//
//   -3,   0,   3       ||    -3,  -10,  -3
//  -10,   0,  10       ||     0,    0,   0
//   -3,   0,   3       ||     3,   10,   3
//
//========================================================

ScharrOperators GetEdgeDetectionKernels()
{
    ScharrOperators kernels;
    kernels.x = float3x3(-3, -10, -3, 0, 0, 0, 3, 10, 3);
    kernels.y = float3x3(-3, 0, 3, -10, 0, 10, -3, 0, 3);
    return kernels;
}

void DepthBasedOutlines_float(float2 ScreenUV, float2 px, float Step, out float outlines)
{
    outlines = 0;
    #if defined(UNITY_DECLARE_DEPTH_TEXTURE_INCLUDED)
    ScharrOperators kernels = GetEdgeDetectionKernels();
    float gx = 0;
    float gy = 0;
    for (int i = -1; i <= 1; i++)
    {
        for (int j = -1; j <= 1; j++)
        {
            if (i == 0 && j == 0)
                continue;
            float2 offset = float2(i, j) * px;
            float d = SampleSceneDepth(ScreenUV + offset);
            gx += d * kernels.x[i + 1][j + 1];
            gy += d * kernels.y[i + 1][j + 1];
        }

    }
    float g = sqrt(gx * gx + gy * gy);
    
    outlines = step(Step, g);

#endif
}

void NormalbasedOutlines_float(float2 ScreenUV, float2 px, float Step, out float outlines)
{
    outlines = 0;
    #if defined(UNITY_DECLARE_NORMALS_TEXTURE_INCLUDED)
    ScharrOperators kernels = GetEdgeDetectionKernels();
    float gx = 0;
    float gy = 0;
    
    float3 cn = SampleSceneNormals(ScreenUV);
    for (int i = -1; i <= 1; i++)
    {
        for (int j = -1; j <= 1; j++)
        {
            if (i == 0 && j == 0)
                continue;
            float2 offset = float2(i, j) * px;
            float n = SampleSceneNormals(ScreenUV + offset);
            float dp = dot(cn, n);
            gx += dp * kernels.x[i + 1][j + 1];
            gy += dp * kernels.y[i + 1][j + 1];
        }
    }
    
    float g = sqrt(gx * gx + gy * gy);
    outlines = step(Step, g);

#endif
}

void ColorBasedOutlines_float(float2 ScreenUV, float2 px, float3 ScreenColor, float Step, out float outlines)
{
    outlines = 0;

#if defined(UNITY_DECLARE_OPAQUE_TEXTURE_INCLUDED)
    ScharrOperators kernels = GetEdgeDetectionKernels();
    float gx = 0;
    float gy = 0;
    for (int i = -1; i <= 1; i++)
    {
        for (int j = -1; j <= 1; j++)
        {
            if (i == 0 && j == 0)
                continue;

            float2 offset = float2(i, j) * px;
            float3 c = SampleSceneColor(ScreenUV + offset);
            float l = dot(c, float3(0.299, 0.587, 0.114)); // luminance

            gx += l * kernels.x[i + 1][j + 1];
            gy += l * kernels.y[i + 1][j + 1];
        }
    }

    float g = sqrt(gx * gx + gy * gy);

    outlines = step(Step, g); // ПОРОГ МОЖНО НАСТРАИВАТЬ

#endif
}


