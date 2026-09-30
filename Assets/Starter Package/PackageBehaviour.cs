/*
 * Copyright 2021 Google LLC
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Used in <see cref="CarBehaviour"/> to determine a collision with a package. 
 */
public class PackageBehaviour : MonoBehaviour
{
	public float RotationSpeed = 45f;

	private void Update()
	{
		transform.Rotate(Vector3.up, RotationSpeed * Time.deltaTime, Space.Self);
	}
    private void OnTriggerEnter(Collider other)
    {
        // Detecta si lo tocó el carro (puedes verificar por Tag "Player" o por el componente CarBehaviour)
        if (other.CompareTag("Player") || other.GetComponent<CarBehaviour>() != null)
        {
            // 1. Avisar al GameManager que se recolectó un paquete
            if (GameManager.Instance != null)
            {
                GameManager.Instance.RecolectarPaquete();
            }

            // 2. Destruir este objeto en la escena
            Destroy(gameObject);
        }
    }
}
