using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Diagnostics;
public class PathWaveGenerator : MonoBehaviour
{
    [SerializeField]
    private int WaveRepetitions;
    [SerializeField]
    private float RepetitionDelay;
    [SerializeField]
    private float InitialDelay;
    /*
	[SerializeField]
	private float fuerzaImpulso = 3;
	[SerializeField]
	private float velocidad = 1f;
	[SerializeField]
	private float proporcionDistanciaTamanio = 0.14f;
	[SerializeField]
	private float tiempoDisipacionDistancia = 5;
	*/
    private AudioSource sound; //TODO ESTE SE QUEDA POR AHORA ASI HASTA QUE HAGAMOS LAS SUBCLASES
    private float instanteCreacionGeneradorOndas;
    private float instanteUltimaOndaLanzada;

    private float LastTimeToEmitWave = 0;
    private float MaxWaveDuration = 15;
    private Vector2 Direction;

    internal void SetDireciton(Vector2 direction)
    {
        Direction = direction.normalized * Mathf.Lerp(0, 1f, Mathf.InverseLerp(0.00f, 8, Mathf.Min(8, direction.magnitude)));
    }

    private int pathsAmount = 128;
    private PathWave[] paths = null;

    // Use this for initialization
    void Start()
    {
        RepetitionDelay = RepetitionDelay <= 0 ? 10 : RepetitionDelay;
        instanteCreacionGeneradorOndas = 0.001f - InitialDelay;
        LastTimeToEmitWave = -0.001f - InitialDelay - ((WaveRepetitions - 1) * RepetitionDelay);
        sound = GetComponent<AudioSource>();

        StartCoroutine(GeneratePathsCoroutine());
        //generatePaths();



    }

    private void CreateSoundUsableData()
    {
        List<float[]> multiImpact = paths
           .SelectMany(path => path.GetWaveImpacts()) // Flatten into a single collection
           .OrderBy(impact => impact.t) // Order by timestamp
           .GroupBy(impact => impact.w) // Group by wave identifier
           .Select(group =>
           {
               float startTime = group.First().t; // Timestamp of the first impact in the group
               float durationTime = group.Last().t - startTime; // Difference between the last and first timestamp
               float frecuencia = 55f * Mathf.Pow(220f / 55f, (10f - startTime) / 10f);
               float volume = group.Count(); // Number of impacts in the group

               return new float[] { startTime, durationTime, frecuencia, volume };
           })
           .ToList();



        List<WaveImpact> impactsRaw = paths
            .SelectMany(path => path.GetWaveImpacts()) // Flatten into a single array
            .OrderBy(impact => impact.t).ToList();

        //string csoundFTable = $"f 1 0 {impactsRaw.Count} -2 " + string.Join(" ", impactsRaw.Select(t => t.t.ToString("F6", System.Globalization.CultureInfo.InvariantCulture)));

        string csoundInstrumentTable = $"" + string.Join("\r", multiImpact.Select(t =>
        "i 1 " + t[0].ToString("F6", System.Globalization.CultureInfo.InvariantCulture)
        + " " + t[1].ToString("F6", System.Globalization.CultureInfo.InvariantCulture)
        + " " + t[2].ToString("F6", System.Globalization.CultureInfo.InvariantCulture)
        + " " + t[3].ToString("F6", System.Globalization.CultureInfo.InvariantCulture)));


        this.StartCoroutine(SaveToFile(csoundInstrumentTable));
        /// END SOUND
    }
    private IEnumerator SaveToFile(string json)
    {
        string path = Path.Combine(Application.persistentDataPath, "waveInstrument.cso");
        yield return new WaitForEndOfFrame();
        try
        {
            File.WriteAllText(path, json);
            UnityEngine.Debug.Log("✅ Sonido Olas guardado automáticamente.");
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError("❌ Error al guardar: " + e.Message);
        }
    }

    void FixedUpdate()
    {
        instanteCreacionGeneradorOndas += Time.deltaTime;
        LastTimeToEmitWave += Time.deltaTime;


        if (instanteCreacionGeneradorOndas > MaxWaveDuration)
        {
            instanteCreacionGeneradorOndas -= RepetitionDelay;
        }
        if (instanteCreacionGeneradorOndas > 0) //para esperar al delay
        {
            calculateWavePositions();
        }
        if (LastTimeToEmitWave > MaxWaveDuration)
        {
            Destroy(gameObject);
        }
    }

    private void calculateWavePositions()
    {
        for (int i = 0; i < pathsAmount; i++)
        {
            paths[i].setInfluenceInPosition(instanteCreacionGeneradorOndas, RepetitionDelay, LastTimeToEmitWave);
        }
    }


    private void generatePaths()
    {
        PathWave[] newPaths = new PathWave[pathsAmount];
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        for (int i = 0; i < pathsAmount; i++)
        {
            float angle = i * Mathf.PI * 2 / pathsAmount;
            Vector2 directionVector = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) + Direction;

            newPaths[i] = new PathWave(position, directionVector);
        }

        paths = newPaths;
        CreateSoundUsableData();
    }
    public IEnumerator GeneratePathsCoroutine()
    {
        PathWave[] newPaths = new PathWave[pathsAmount];
        Vector2 position = new Vector2(transform.position.x, transform.position.y);

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        for (int i = 0; i < pathsAmount; i++)
        {
            float angle = i * Mathf.PI * 2 / pathsAmount;
            Vector2 directionVector = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) + Direction;
            newPaths[i] = new PathWave(position, directionVector);

            // Verificar tiempo transcurrido
            if (stopwatch.ElapsedMilliseconds >= 10)
            {
                // Esperar al siguiente frame y reiniciar el contador
                yield return null;
                stopwatch.Restart();
            }
        }

        stopwatch.Stop();

        paths = newPaths;
        CreateSoundUsableData();
    }

}