using InputHandler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Shapes;


public class World : MonoBehaviour
{
    protected InputRequest TouchableWaterAreaInputRequest;
    private static List<Vector2[]> ObstPoints = new List<Vector2[]>();

    private static Mesh fragmentMesh;
    public Material fragmentMaterial;


    // Propiedades para renderizado por instancias en GPU extraer clase

    public Shader waveFragmentShader;
    private static List<Matrix4x4> positions = new List<Matrix4x4>();
    private static int positionIndex = 0;
    private static int positionMaxIndex = 0;
    private static int positionCurrentMaxIndex = 0;
    private static int interfaceBlockingElements = (1 << (int)ELayer.Obstacles) + (1 << (int)ELayer.WaterBlock);


    private float width;
    private float height;

    void Awake()
    {
        width = (float)Screen.width / 2.0f;
        height = (float)Screen.height / 2.0f;
        InitInputRequests();
        InputManager.AddInputRequest(TouchableWaterAreaInputRequest);
    }


    private void InitInputRequests()
    {
        TouchableWaterAreaInputRequest = new InputRequest(new List<InputDescriptor>{
              new InputDescriptor(IHT.MOUSE_PRIMARY_DOWN,(int)ELayer.TouchableWater, interfaceBlockingElements,
                (object inputInfo) => EventManager.TriggerEvent("WATERAREA:Touch", (((RaycastHit2D)inputInfo).point))
              ),
              new InputDescriptor(IHT.MOUSE_PRIMARY_DOWN,(int)ELayer.Obstacles, (1 << (int)ELayer.WaterBlock),
                (object inputInfo) => EventManager.TriggerEvent("OBSTACLE:Touch", (((RaycastHit2D)inputInfo)))
              ),
              new InputDescriptor(IHT.MOUSE_PRIMARY_DOWN,(int)ELayer.WaterBlock, 0,
                (object inputInfo) => EventManager.TriggerEvent("WATERBLOCK:Touch", (((RaycastHit2D)inputInfo).point))
              ),
              new InputDescriptor(IHT.MOUSE_PRIMARY_UP,(int)ELayer.TouchableWater, 0,
                (object inputInfo) => EventManager.TriggerEvent("WATERAREA:UnTouch", (((RaycastHit2D)inputInfo).point))
              ),

              new InputDescriptor(IHT.TOUCH_DOWN,(int)ELayer.TouchableWater,interfaceBlockingElements,
                (object inputInfo) => EventManager.TriggerEvent("WATERAREA:Touch", (((RaycastHit2D)inputInfo).point))
              ),
              new InputDescriptor(IHT.TOUCH_DOWN,(int)ELayer.Obstacles, (1 << (int)ELayer.WaterBlock),
                (object inputInfo) => EventManager.TriggerEvent("OBSTACLE:Touch", (((RaycastHit2D)inputInfo)))
              ),
              new InputDescriptor(IHT.TOUCH_DOWN,(int)ELayer.WaterBlock, 0,
                (object inputInfo) => EventManager.TriggerEvent("WATERBLOCK:Touch", (((RaycastHit2D)inputInfo).point))
              ),
              new InputDescriptor(IHT.TOUCH_UP,(int)ELayer.TouchableWater,interfaceBlockingElements,
                (object inputInfo) => EventManager.TriggerEvent("WATERAREA:UnTouch", (((RaycastHit2D)inputInfo).point))
              ),


        });
    }

    void Start()
    {
        //fillObstaclePoints();

        fragmentMesh = MeshCreator.WaveFragment(0.1f, 0.1f);
        if (fragmentMaterial == null)
        {
            fragmentMaterial = new Material(waveFragmentShader);
            fragmentMaterial.enableInstancing = true;
            fragmentMaterial.renderQueue = 2100;
        }

    }

    // Update is called once per frame
    void Update()
    {
        ShowWavesInGPU();
    }


    private void FixedUpdate()
    {
        //if (positionIndex > 0)
        //{
        positionCurrentMaxIndex = positionIndex;
        //}
        positionIndex = 0;
    }
    /**
     introduce ne la gpu el nuevo set de matrices que determina la posicion de las olas en bloques de 1023 matrices
         */
    private Matrix4x4[] drawBuffer = new Matrix4x4[1023];
    private void ShowWavesInGPU()
    {

        int currentCount = 0;
        {
            while (positionCurrentMaxIndex - currentCount > 0)
            {
                int batchSize = Math.Min(1023, positionCurrentMaxIndex - currentCount);
                positions.CopyTo(currentCount, drawBuffer, 0, batchSize);
                Graphics.DrawMeshInstanced(fragmentMesh, 0, fragmentMaterial, drawBuffer, batchSize, new MaterialPropertyBlock(), UnityEngine.Rendering.ShadowCastingMode.Off, false, 11);
                currentCount += batchSize;
            }
        }

    }
    /**
     *  actualiza las matrices de posicion de los meshes que se van a poner en la grafica
     */
    private static void DrawWaveFragment(float x, float y, Vector2 direction)
    {
        if (positionIndex < positionMaxIndex)
        {

            Vector2 perpendicularSpeed = Vector2.Perpendicular(direction).normalized * (-15f * (direction.magnitude));

            positions[positionIndex] = new Matrix4x4
            {
                m00 = perpendicularSpeed.x,
                m10 = perpendicularSpeed.y,
                m20 = 0,
                m30 = 0,
                m01 = direction.x * 2,
                m11 = direction.y * 2,
                m21 = 0,
                m31 = 0,
                m02 = 0,
                m12 = 0,
                m22 = 1,
                m32 = 0,
                m03 = x,
                m13 = y,
                m23 = 10,
                m33 = 1,
            };

            positionIndex++;
        }
        else
        {
            positions.Add(new Matrix4x4
            {
                m00 = 1 / direction.magnitude,
                m10 = 0,
                m20 = 0,
                m30 = 0,
                m01 = 0,
                m11 = direction.magnitude,
                m21 = 0,
                m31 = 0,
                m02 = 0,
                m12 = 0,
                m22 = 1,
                m32 = 0,
                m03 = x,
                m13 = y,
                m23 = 10,
                m33 = 1,
            });

            positionIndex++;
            positionMaxIndex++;
        }


    }

    public static void clearObs()
    {
        ObstPoints.Clear();
    }

    /*
     * add every fragment influence to all the floating balls

      */
    public static void addInfluence(Vector2 position, Vector2 direction)
    {
        DrawWaveFragment(position.x, position.y, direction);

        foreach (NutShell ball in NutShell.InGameNutShells)
        {
            ball.AddWaveFragmentForce(position, direction);
        }

    }

    /**
     coge los collaider de los gameobject con el tag "obstacle" y recorre sus puntos para crear lineas 
        (parejas de puntos ) que generan cambios de direccion a las olas
         */
    public static void fillObstaclePoints()
    {

        if (ObstPoints.Count > 0)
        {
            return;
        }
        ObstPoints.Clear();
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        for (int i = obstacles.Length - 1; i >= 0; i--)
        {
            GameObject currentObstacle = obstacles[i];
            PolygonCollider2D obstacleCollider = currentObstacle.GetComponent<PolygonCollider2D>();
            Vector2[] points = obstacleCollider.points;
            int pointsCuantity = points.Length;
            for (int j = pointsCuantity - 1; j >= 0; j--)
            {
                Vector2[] obst = new Vector2[2];
                obst[0] = Trigonometrics.localPointToGlobal(currentObstacle, points[j]);
                obst[1] = Trigonometrics.localPointToGlobal(currentObstacle, points[(j + 1) % pointsCuantity]);
                ObstPoints.Add(obst);
            }
        }
    }

    /***/
    public static SubPathWave findNextSubPath(SubPathWave subPath)
    {
        // Llenar la lista de puntos de obstáculos antes de calcular intersecciones
        fillObstaclePoints();

        // Inicialización de variables para rastrear la colisión más cercana
        float collisionDistance = float.MaxValue; // Usar el valor máximo en vez de un número arbitrario
        int selectedObstWallIndex = -1;
        Vector2[] collisionObstaclePoints = null; // Inicializar como null para evitar asignaciones innecesarias
        Vector2 collisionPosition = Vector2.zero; // Usar Vector2.zero en vez de new Vector2() para optimización

        // Iterar sobre la lista de puntos de obstáculos
        for (int i = 0; i < ObstPoints.Count; i++)
        {
            Vector2[] points = ObstPoints[i];
            // Verificar si la trayectoria del subPath es paralela al obstáculo
            if (Trigonometrics.areParallels(subPath.velocity, points[1] - points[0]))
            {
                continue; // Si son paralelas, no hay intersección
            }

            // Calcular el punto de intersección entre la trayectoria y el obstáculo
            Vector2 intersection = Trigonometrics.linesIntersection(
                subPath.startPosition, subPath.velocity, points[0], points[1] - points[0]
            );

            // Calcular la distancia desde la posición inicial del subPath hasta la intersección
            float distance = Vector2.Distance(subPath.startPosition, intersection);

            // Validar si la intersección es válida según varias condiciones
            if (distance > 0.05f && distance < collisionDistance &&
                Trigonometrics.pointIsInSemiSegment(subPath.startPosition, subPath.velocity, intersection) &&
                Trigonometrics.pointIsInSegment(points[0], points[1], intersection))
            {
                // Actualizar los datos de la colisión más cercana
                selectedObstWallIndex = i;
                collisionObstaclePoints = points;
                collisionDistance = distance;
                collisionPosition = intersection;
            }
        }

        // Si no se encontró una colisión válida, retornar null
        if (collisionObstaclePoints == null)
        {
            return null;
        }

        // Calcular el nuevo vector de velocidad reflejado tras el choque con el obstáculo
        Vector2 obstacleVector = collisionObstaclePoints[1] - collisionObstaclePoints[0];
        Vector2 newVelocity = Vector2.Reflect(
            subPath.velocity,
            new Vector2(-obstacleVector.y, obstacleVector.x).normalized
        );

        // Calcular el nuevo tiempo en que ocurre la colisión
        float newTime = (collisionDistance / subPath.velocity.magnitude) + subPath.startTime;

        // Retornar un nuevo SubPathWave con los datos de la nueva trayectoria
        return new SubPathWave(collisionPosition, newVelocity, newTime,subPath.wall + "-" + selectedObstWallIndex.ToString());
    }
}