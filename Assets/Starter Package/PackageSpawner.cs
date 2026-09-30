using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PackageSpawner : MonoBehaviour
{
    public DrivingSurfaceManager DrivingSurfaceManager;
    public PackageBehaviour Package;
    public GameObject PackagePrefab;

    public static Vector3 RandomInTriangle(Vector3 v1, Vector3 v2)
    {
        float u = Random.Range(0.0f, 1.0f);
        float v = Random.Range(0.0f, 1.0f);
        if (v + u > 1)
        {
            v = 1 - v;
            u = 1 - u;
        }

        return (v1 * u) + (v2 * v);
    }

    public static Vector3 FindRandomLocation(ARPlane plane)
    {
        var mesh = plane.GetComponent<ARPlaneMeshVisualizer>().mesh;
        var triangles = mesh.triangles;
        
        // Evita errores fuera de rango en los triángulos de la malla
        int triangleIndex = Random.Range(0, triangles.Length / 3) * 3;
        var vertices = mesh.vertices;
        var randomInTriangle = RandomInTriangle(vertices[triangles[triangleIndex]], vertices[triangles[triangleIndex + 1]]);
        var randomPoint = plane.transform.TransformPoint(randomInTriangle);

        return randomPoint;
    }

    public void SpawnPackage(ARPlane plane)
    {
        var packageClone = GameObject.Instantiate(PackagePrefab);
        packageClone.transform.position = FindRandomLocation(plane);

        Package = packageClone.GetComponent<PackageBehaviour>();
    }

    private void Update()
    {
        // Solo spawnea o mantiene el paquete si el juego realmente ha comenzado
        if (GameManager.Instance != null && !GameManager.Instance.IsJuegoActivo()) return;

        var lockedPlane = DrivingSurfaceManager.LockedPlane;
        if (lockedPlane != null)
        {
            // Al ser destruido en PackageBehaviour, Package vuelve a ser null y genera otro
            if (Package == null)
            {
                SpawnPackage(lockedPlane);
            }
            else
            {
                // Corrección para mantener la altura al nivel del plano AR
                Vector3 pos = Package.transform.position;
                pos.y = lockedPlane.center.y;
                Package.transform.position = pos;
            }
        }
    }

    /// <summary>
    /// Destruye el paquete activo si existe (para limpiar la escena).
    /// </summary>
    public void LimpiarPaquete()
    {
        if (Package != null)
        {
            Destroy(Package.gameObject);
            Package = null;
        }
    }
}