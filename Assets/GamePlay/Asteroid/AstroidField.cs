using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class AstroidField : MonoBehaviour
{
    [SerializeField] int xgrid_size = 10;
    [SerializeField] int ygrid_size = 10;
    [SerializeField] int zgrid_size = 10;


    [SerializeField] AsteroidController asteroid_fab;

    [SerializeField] int x_scale = 10;
    [SerializeField] int y_scale = 5;
    [SerializeField] int z_scale = 10;

    [SerializeField] int radius = 10;



    void Start()
    {
        PlaceAsteroids(5);
    }

    void Update()
    {
        
    }


    void PlaceAsteroids(int scale)
    {
        for (int x = 0; x < x_scale; x++)
        {
            for (int y = 0; y < z_scale; y++)
            {
                InstantiateAsteroid(x, y);
            }
        }
    }

    float CalcAseteroidOffset(float gridsize)
    {
        return UnityEngine.Random.Range(-(gridsize / 2f), gridsize / 2f);
    }

    void InstantiateAsteroid(int x, int z)
    {
        float radians = 2 * (MathF.PI / x_scale ) * x;
        float vertical = MathF.Sin(radians);
        float horizontal = MathF.Cos(radians);

        Vector3 initialCircle = new Vector3(horizontal + CalcAseteroidOffset(0.5f), 0 , vertical + CalcAseteroidOffset(0.5f));
        Vector3 modPosition = transform.position + initialCircle * radius;
        modPosition.y = (zgrid_size * z) + CalcAseteroidOffset(zgrid_size);

        Instantiate(asteroid_fab, modPosition,
            Quaternion.identity,transform);
    }
}
