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

	private int formStepX = 0;
	private int formStepY = 0;
	private float formUpdateTimer = 0f;
	private float formUpdateDuration = 0.2f;
	private float lerpSpeed = 0.1f;

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
			updateForm_noise();
			formUpdateTimer = 0;
		}

		lightingUpdateTimer += Time.deltaTime;
		formUpdateTimer += Time.deltaTime;

	}

	void updateHeight() {
		for (int i = 0; i < size * 2; i++) {
			for (int j = 0; j < size * 2; j++) {
				ground[i,j].transform.localScale = new Vector3(blockSize, Mathf.Lerp(ground[i,j].transform.localScale.y, (groundTargetHeight[i, j] + size / 2) * 2, lerpSpeed), blockSize);
				ceiling[i,j].transform.localScale = new Vector3(blockSize, Mathf.Lerp(ceiling[i,j].transform.localScale.y, (height + size / 2 - ceilingTargetHeight[i, j]) * 2, lerpSpeed), blockSize);
			}
		}
	}

	void updateLighting() {
		Debug.Log(ceilingTargetHeight[0, 0]);
		for (int i=0;i<size * 2;i++) {
			for (int j=0;j<size * 2;j++) {
				if (ceilingTargetHeight[i, j] > height && ceilingTargetHeight[i, j] < 1.25 * height) { //height / 2 ~ 5 * height / 2
					ceiling[i, j].GetComponent<MeshRenderer>().material = emissiveMaterial;
					ceilingLight[i, j].SetActive(true);
				}
				else {
					ceiling[i, j].GetComponent<MeshRenderer>().material = defaultMaterial;
					ceilingLight[i, j].SetActive(false);
				}
			}
		}
	}

	void updateForm_noise() {
		for (int i = 0; i < size * 2; i++) {
			for (int j = 0; j < size * 2; j++) {
				groundTargetHeight[i, j] =
					Mathf.PerlinNoise(
						(float)(i + formStepX + 123) / size,
						(float)(j + formStepY) / size
					) * 2;
				groundTargetHeight[i, j] = groundTargetHeight[i, j] > 1.75f ? height * 4 : groundTargetHeight[i, j];
				ceilingTargetHeight[i, j] =
					Mathf.PerlinNoise(
						(float)(i + formStepX) / size * 2,
						(float)(j + formStepY + 177) / size * 2
					) * height * 2 + height / 2;
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
