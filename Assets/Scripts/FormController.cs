using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormController : MonoBehaviour
{
	public GameObject cellPrefab;
	public GameObject lightPrefab;
	public Material emissiveMaterial;
	public Material defaultMaterial;
	private GameObject[,] ground;
	private GameObject[,] ceiling;
	private float[,] groundTargetHeight;
	private float[,] ceilingTargetHeight;
	private GameObject[,] ceilingLight;
	private float[,] ceilingTargetIntensity;
	private const int size = 32;
	private const float blockSize = 1;
	private const int height = 4;


	private int lightingStep = 0;
	private int lightingStepDir = 1;
	private float lightingUpdateTimer = 0f;
	private float lightingUpdateDuration = 0.5f;
	private const float maxIntensity = 0.5f;
	private const float lightingLerpSpeed = 0.05f;

	private int formStepX = 0;
	private int formStepY = 0;
	private float formUpdateTimer = 0f;
	private float formUpdateDuration = 1f;
	private const float formLerpSpeed = 0.075f;
	private const float formSpeed = 3f;

	void Start()
	{
		generateRoom();
		updateLighting();
	}

	// Update is called once per frame
	void Update()
	{
		updateHeight();
		updateLighting();
		if (formUpdateTimer > formUpdateDuration) {
			updateForm_corridor();
			formUpdateTimer = 0;
		}

		lightingUpdateTimer += Time.deltaTime;
		formUpdateTimer += Time.deltaTime;

	}

	void updateHeight() {
		for (int i = 0; i < size * 2; i++) {
			for (int j = 0; j < size * 2; j++) {
				ground[i,j].transform.localScale = new Vector3(blockSize, Mathf.Lerp(ground[i,j].transform.localScale.y, (groundTargetHeight[i, j] + size / 2) * 2, (groundTargetHeight[i, j] == ceilingTargetHeight[i, j] ? formLerpSpeed * 10f : formLerpSpeed)), blockSize);
				ceiling[i,j].transform.localScale = new Vector3(blockSize, Mathf.Lerp(ceiling[i,j].transform.localScale.y, (height + size / 2 - ceilingTargetHeight[i, j]) * 2, formLerpSpeed), blockSize);
			}
		}
	}

	void updateLighting() {
		for (int i=0;i<size * 2;i++) {
			for (int j=0;j<size * 2;j++) {
				float tInt = ceiling[i, j].GetComponent<MeshRenderer>().material.GetColor("_EmissionColor").r;
				tInt = Mathf.Lerp(tInt, ceilingTargetIntensity[i, j], (ceilingTargetIntensity[i, j] > 0.2f ? lightingLerpSpeed * 10f : lightingLerpSpeed));
				tInt = (tInt > 1 ? 1 : tInt);
				if (tInt > 0.025f) {
					if (i % 2 == 0 && j % 2 == 0) {
						ceilingLight[i,j].SetActive(true);
						ceilingLight[i,j].GetComponent<Light>().intensity = tInt * maxIntensity;
					}
					else {
						ceilingLight[i,j].SetActive(false);
					}
					ceiling[i, j].GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", new Color(tInt, tInt, tInt));
				}
				else {
					ceilingLight[i,j].SetActive(false);
					ceiling[i, j].GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", new Color(0, 0, 0));
				}
			}
		}
	}

	void updateForm_noise() {
		for (int i = 0; i < size * 2; i++) {
			for (int j = 0; j < size * 2; j++) {
				float ceilingPicker = Mathf.PerlinNoise(
						formSpeed * (float)(i + formStepX) / size * 2,
						formSpeed * (float)(j + formStepY + 177) / size * 2
					);
				ceilingTargetHeight[i, j] = ceilingPicker * height + height;
				ceilingTargetIntensity[i, j] = (ceilingPicker < 0.4f) ? (0.4f - ceilingPicker) / 0.4f : 0;
				groundTargetHeight[i, j] =
					Mathf.PerlinNoise(
						formSpeed * (float)(i - formStepX + 123) / size,
						formSpeed * (float)(j - formStepY) / size
					) * 2;
				groundTargetHeight[i, j] *= groundTargetHeight[i, j];
				groundTargetHeight[i, j] = (groundTargetHeight[i, j] > height * 0.65f && (Mathf.Abs(i - size) > 2 || Mathf.Abs(j - size) > 2)) ? ceilingTargetHeight[i, j] : groundTargetHeight[i, j] - 0.2f;
			}
		}
		formStepX++;
		formStepY--;
	}

	void updateForm_corridor() {
		for (int i = 0; i < size * 2; i++) {
			for (int j = 0; j < size * 2; j++) {
				if ((i >= size - 2 && i < size + 2) || (i >= size - 3 && i < size + 3 && j % 3 != 0)) {
					float ceilingPicker = Mathf.PerlinNoise(
							formSpeed * (float)(i + formStepX) / size * 2,
							formSpeed * (float)(j + formStepY + 177) / size * 2
						);
					ceilingTargetHeight[i, j] = ceilingPicker * height + height;
					ceilingTargetIntensity[i, j] = (ceilingPicker < 0.4f) ? (0.4f - ceilingPicker) / 0.4f : 0;
					groundTargetHeight[i, j] =
						Mathf.PerlinNoise(
							formSpeed * (float)(i - formStepX + 123) / size,
							formSpeed * (float)(j - formStepY) / size
						) * 2;
				}
				else {
					groundTargetHeight[i, j] = height * 2;
					ceilingTargetHeight[i, j] = height * 2;
					ceilingTargetIntensity[i, j] = 0;
				}
			}
		}
		formStepX++;
		formStepY--;
	}

	void generateRoom() {
		groundTargetHeight = new float[size * 2, size * 2];
		for (int i = 0; i < size * 2; i++) {
			for (int j = 0; j < size * 2; j++) {
				groundTargetHeight[i, j] = 0;
			}
		}
		ceilingTargetHeight = new float[size * 2, size * 2];
		ceilingTargetIntensity = new float[size * 2, size * 2];
		for (int i = 0; i < size * 2; i++) {
			for (int j = 0; j < size * 2; j++) {
				ceilingTargetHeight[i, j] = height;
				ceilingTargetIntensity[i, j] = 0;
			}
		}
		ground = new GameObject[size * 2, size * 2];
		for (int i = 0; i < size * 2; i++) {
			for (int j = 0; j < size * 2; j++) {
				ground[i,j] = (GameObject)Instantiate(cellPrefab, new Vector3((i + 0.5f - size) * blockSize, - size / 2, (j + 0.5f - size) * blockSize), Quaternion.identity);
				ground[i,j].transform.localScale = new Vector3(blockSize, size, blockSize);
			}
		}
		ceiling = new GameObject[size * 2, size * 2];
		for (int i = 0; i < size * 2; i++) {
			for (int j = 0; j < size * 2; j++) {
				ceiling[i,j] = (GameObject)Instantiate(cellPrefab, new Vector3((i + 0.5f - size) * blockSize, size / 2 + height, (j + 0.5f - size) * blockSize), Quaternion.identity);
				ceiling[i,j].transform.localScale = new Vector3(blockSize, size, blockSize);
				ceiling[i, j].GetComponent<MeshRenderer>().material = new Material(emissiveMaterial);
			}
		}
		ceilingLight = new GameObject[size * 2, size * 2];
		for (int i = 0; i < size * 2; i++) {
			for (int j = 0; j < size * 2; j++) {
				ceilingLight[i,j] = (GameObject)Instantiate(lightPrefab, new Vector3(0, 0, 0), Quaternion.identity);
				ceilingLight[i,j].SetActive(false);
				ceilingLight[i,j].GetComponent<Light>().intensity = 0;
				ceilingLight[i,j].transform.SetParent(ceiling[i,j].transform);
				ceilingLight[i,j].transform.localPosition = new Vector3(0, -0.55f, 0);
			}
		}
	}
}
