using UnityEngine;
using System.Collections.Generic;

public class HeatMapColorizer : MonoBehaviour
{
    public float allowableDistance = 2f; 
    public float colorAdjuster = 0.2f;  

    private Color color = Color.blue;

    private bool setColorsDone = false;
    private float Xshift;
    private GameObject[] heatBoxes;

    private Dictionary<Vector3, GameObject> cubeDictionary = new Dictionary<Vector3, GameObject>();

    void Start() {
    heatBoxes = GameObject.FindGameObjectsWithTag("heatBox");

        foreach (GameObject item in heatBoxes)
        {
            Renderer renderer = item.GetComponent<Renderer>();
            renderer.material.color = color;

            cubeDictionary.Add(item.transform.position, item);
        }

        HeatMapReader heatMapReader = FindObjectOfType<HeatMapReader>();
        if (heatMapReader != null)
        {
            Xshift = heatMapReader.Xshift;
        }
        else
        {
            Debug.LogError("Could not find the HeatMapReader component in the scene.");
        }
    }


    void Update()
    {
        if (HeatMapReader.finished && !setColorsDone)
            HeatMapReader.finished = false;
            SetColor();
    }

    void SetColor()
    {
        List<Vector3> positions = HeatMapReader.nearestPositions;
        HeatMapReader.nearestPositions = new List<Vector3>();

        foreach (Vector3 position in positions)
        {
            Vector3 adjustedPosition = position;
            adjustedPosition.x += Xshift;

            if (cubeDictionary.ContainsKey(adjustedPosition))
            {
                GameObject cube = cubeDictionary[adjustedPosition];
                Renderer renderer = cube.GetComponent<Renderer>();
                Color currentColor = renderer.material.color;

                if (currentColor.g < 1f && currentColor.b != 0f)
                {
                    currentColor.g += colorAdjuster;
                    currentColor.b -= colorAdjuster;
                }
                else
                {
                    currentColor.r += colorAdjuster;
                    currentColor.g -= colorAdjuster;
                }
                renderer.material.color = currentColor;
            }
        }
        setColorsDone = true;
    }


}

