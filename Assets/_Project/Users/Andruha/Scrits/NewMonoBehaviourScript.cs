using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RoadGenerator : MonoBehaviour
{
    [Header("Настройки дорожки")]
    [SerializeField] private float roadWidth = 5f;
    [SerializeField] private float segmentLength = 2f;
    [SerializeField] private Material roadMaterial;

    [Header("Настройки генерации")]
    [SerializeField] private bool generateOnStart = true;
    [SerializeField] private bool destroyExisting = true;

    private LineRenderer lineRenderer;
    private List<GameObject> roadSegments = new List<GameObject>();

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        if (generateOnStart)
        {
            GenerateRoad();
        }
    }

    public void GenerateRoad()
    {
        // Удаляем старые сегменты если нужно
        if (destroyExisting)
        {
            DestroyRoad();
        }

        // Проверяем наличие точек в кривой
        if (lineRenderer.positionCount < 2)
        {
            Debug.LogWarning("Кривая должна содержать как минимум 2 точки!");
            return;
        }

        // Проходим по кривой и создаем сегменты дороги
        for (int i = 0; i < lineRenderer.positionCount - 1; i++)
        {
            Vector3 startPoint = transform.TransformPoint(lineRenderer.GetPosition(i));
            Vector3 endPoint = transform.TransformPoint(lineRenderer.GetPosition(i + 1));

            CreateRoadSegment(startPoint, endPoint, i);
        }
    }

    void CreateRoadSegment(Vector3 startPoint, Vector3 endPoint, int index)
    {
        // Вычисляем направление и длину сегмента
        Vector3 direction = (endPoint - startPoint).normalized;
        float distance = Vector3.Distance(startPoint, endPoint);
        int segmentsCount = Mathf.CeilToInt(distance / segmentLength);

        // Создаем сегменты между двумя точками кривой
        for (int i = 0; i < segmentsCount; i++)
        {
            float t = i / (float)segmentsCount;
            float tNext = (i + 1) / (float)segmentsCount;

            Vector3 segmentStart = Vector3.Lerp(startPoint, endPoint, t);
            Vector3 segmentEnd = Vector3.Lerp(startPoint, endPoint, tNext);

            // Создаем игровой объект для сегмента дороги
            GameObject segment = GameObject.CreatePrimitive(PrimitiveType.Plane);
            segment.name = $"RoadSegment_{index}_{i}";

            // Позиционируем сегмент
            Vector3 segmentCenter = (segmentStart + segmentEnd) / 2f;
            segment.transform.position = segmentCenter;

            // Поворачиваем сегмент вдоль направления
            segment.transform.rotation = Quaternion.LookRotation(segmentEnd - segmentStart);

            // Масштабируем сегмент
            float segmentDistance = Vector3.Distance(segmentStart, segmentEnd);
            segment.transform.localScale = new Vector3(
                roadWidth / 10f,
                1f,
                segmentDistance / 10f
            );

            // Назначаем материал
            if (roadMaterial != null)
            {
                segment.GetComponent<Renderer>().material = roadMaterial;
            }

            // Делаем сегмент дочерним объектом
            segment.transform.SetParent(transform);

            roadSegments.Add(segment);
        }
    }

    public void DestroyRoad()
    {
        foreach (GameObject segment in roadSegments)
        {
            if (segment != null)
            {
                Destroy(segment);
            }
        }
        roadSegments.Clear();
    }

    // Метод для обновления дороги при изменении кривой в редакторе
#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();

        if (lineRenderer != null && lineRenderer.positionCount > 1)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < lineRenderer.positionCount - 1; i++)
            {
                Vector3 start = transform.TransformPoint(lineRenderer.GetPosition(i));
                Vector3 end = transform.TransformPoint(lineRenderer.GetPosition(i + 1));
                Gizmos.DrawLine(start, end);
                Gizmos.DrawSphere(start, 0.1f);
            }

            if (lineRenderer.positionCount > 0)
            {
                Vector3 last = transform.TransformPoint(lineRenderer.GetPosition(lineRenderer.positionCount - 1));
                Gizmos.DrawSphere(last, 0.1f);
            }
        }
    }
#endif
}