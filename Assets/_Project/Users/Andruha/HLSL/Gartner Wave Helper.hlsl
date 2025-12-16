void SimpleGerstner_float
(
float4 direction, // Направление волны (нормализованный вектор)
float2 waveNumber, // Волновое число (k = 2π/λ, где λ - длина волны)
float2 amplitude, // Амплитуда (максимальная высота)
float2 speed, // Скорость распространения
float2 steepness,
float3 vertexPosition,
float time,
out float3 res
)
{
                // 1. Нормализуем направление волны
    float3 totalOffset = float3(0, 0, 0);
    
    // ВОЛНА 1
    float2 dir1 = normalize(float2(direction.x, direction.y)); // Направление ≈ 26° от оси X

    
    // Вычисляем фазу для первой волны
    float phase1 = waveNumber.x * dot(dir1, vertexPosition.xz) - speed.x * time;
    
    // Горизонтальное смещение (X и Z)
    totalOffset.x += steepness.x * amplitude.x * dir1.x * cos(phase1);
    totalOffset.z += steepness.x * amplitude.x * dir1.y * cos(phase1);
    
    // Вертикальное смещение (Y)
    totalOffset.y += amplitude.x * sin(phase1);
    
    // ВОЛНА 2
    float2 dir2 = normalize(float2(direction.z, direction.w)); // Направление ≈ 107° от оси X
    
    float phase2 = waveNumber.y * dot(dir2, vertexPosition.xz) - speed.y * time;
    
    totalOffset.x += steepness.y * amplitude.y * dir2.x * cos(phase2);
    totalOffset.z += steepness.y * amplitude.y * dir2.y * cos(phase2);
    totalOffset.y += amplitude.y * sin(phase2);
    
    res = totalOffset;
}

void SimpleGerstnerSoloWave_float
(
float2 direction, // Направление волны (нормализованный вектор)
float waveNumber, // Волновое число (k = 2π/λ, где λ - длина волны)
float amplitude, // Амплитуда (максимальная высота)
float speed, // Скорость распространения
float steepness,
float3 vertexPosition,
float time,
out float3 res
)
{
    float3 totalOffset = float3(0, 0, 0);
    
    float2 dir1 = normalize(float2(direction.x, direction.y)); 

    float phase1 = waveNumber.x * dot(dir1, vertexPosition.xz) - speed.x * time;
    
    totalOffset.x += steepness.x * amplitude.x * dir1.x * cos(phase1);
    totalOffset.z += steepness.x * amplitude.x * dir1.y * cos(phase1);
    totalOffset.y += amplitude.x * sin(phase1);
    
    res = totalOffset;
}