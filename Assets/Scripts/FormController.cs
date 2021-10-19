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
	private const int size = 16;
	private const float blockSize = 2;
	private const int height = 4;


	private int lightingStep = 0;
	private int lightingStepDir = 1;
	private float lightingUpdateTimer = 0f;
	private float lightingUpdateDuration = 0.5f;

	private int formStep = 0;
	private float formUpdateTimer = 0f;
	private float formUpdateDuration = 0.2f;

	void Start()
	{
		generateRoom();
		updateLighting();
	}

	// Update is called once per frame
	void Update()
	{
		updateHeight();
		if (lightingUpdateTimer > lightingUpdateDuration) {
			//updateLighting();
			lightingUpdateTimer = 0;
		}
		if (formUpdateTimer > formUpdateDuration) {
			updateForm_noise();
			formUpdateTimer = 0;
		}

		lightingUpdateTimer += Time.deltaTime;
		formUpdateTimer += Time.deltaTime;

	}

	void updateHeight() {
		float lerpSpeed = 0.1f;
		for (int i = 0; i < size * 2; i++) {
			for (int j = 0; j < size * 2; j++) {
				ground[i,j].transform.localScale = new Vector3(blockSize, Mathf.Lerp(ground[i,j].transform.localScale.y, (groundTargetHeight[i, j] + size / 2) * 2, lerpSpeed), blockSize);
				ceiling[i,j].transform.localScale = new Vector3(blockSize, Mathf.Lerp(ceiling[i,j].transform.localScale.y, (height + size / 2 - ceilingTargetHeight[i, j]) * 2, lerpSpeed), blockSize);
			}
		}
	}

	void updateLighting() {
		for (int i=0;i<size * 2;i++) {
			for (int j=0;j<size * 2;j++) {
				if (ceilingLight[i, j].activeSelf) {
					ceiling[i, j].GetComponent<MeshRenderer>().material = defaultMaterial;
					ceilingLight[i, j].SetActive(false);
				}
			}
		}
		for (int i=0;i<size * 2;i++) {
			for (int j=0;j<size * 2;j++) {
				if (i % 4 == 2 && j % 4 == 2) {
					ceiling[i, j].GetComponent<MeshRenderer>().material = emissiveMaterial;
					ceilingLight[i, j].SetActive(true);
				}
			}
		}
		lightingStep += lightingStepDir;
		if (lightingStep == size * 2 - 1) {
			lightingStepDir = -1;
		}
		if (lightingStep == 0) {
			lightingStepDir = 1;
		}
	}

	void updateForm_noise() {
		for (int i = 0; i < size * 2; i++) {
			for (int j = 0; j < size * 2; j++) {
				groundTargetHeight[i, j] = Mathf.PerlinNoise((float)((i + formStep + 123) % (size * 2)) / (size / 2), (float)j / (size / 2)) * 2;
				ceilingTargetHeight[i, j] = Mathf.PerlinNoise((float)((i + formStep) % (size * 2)) / (size / 2), (float)j / (size / 2)) * height + height;
			}
		}
		formStep++;
	}

	void generateRoom() {
		groundTargetHeight = new float[size * 2, size * 2];
		for (int i = 0; i < size * 2; i++) {
			for (int j = 0; j < size * 2; j++) {
				groundTargetHeight[i, j] = 0;
			}
		}
		ceilingTargetHeight = new float[size * 2, size * 2];
		for (int i = 0; i < size * 2; i++) {
			for (int j = 0; j < size * 2; j++) {
				ceilingTargetHeight[i, j] = height;
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
			}
		}
		ceilingLight = new GameObject[size * 2, size * 2];
		for (int i = 0; i < size * 2; i++) {
			for (int j = 0; j < size * 2; j++) {
				ceilingLight[i,j] = (GameObject)Instantiate(lightPrefab, new Vector3(0, 0, 0), Quaternion.identity);
				ceilingLight[i,j].SetActive(false);
				ceilingLight[i,j].transform.SetParent(ceiling[i,j].transform);
				ceilingLight[i,j].transform.localPosition = new Vector3(0, -0.51f, 0);
			}
		}
	}
}
