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
	private GameObject[,] frontWall;
	private GameObject[,] backWall;
	private GameObject[,] leftWall;
	private GameObject[,] rightWall;
	private GameObject[,] ceiling;
	private GameObject[,] ceilingLight;
	private const int size = 16;
	private const int height = 8;
	private const int offset = size / 2;



	private int lightingStep = offset;
	private int lightingStepDir = 1;
	private float lightingUpdateTimer = 0f;
	private float lightingUpdateDuration = 0.5f;

	private int formStep = 0;
	private int formStepDir = 1;
	private float formUpdateTimer = 0f;
	private float formUpdateDuration = 0f;


	private float[,] randomSample;
	private float[,] noise;
	private int boxSize = 2;
	private float[,] kernel = new float[5, 5]{
		{1f/273, 4f/273, 7f/273, 4f/273, 1f/273}, 
		{4f/273, 16f/273, 26f/273, 16f/273, 4f/273}, 
		{1f/273, 26f/273, 41f/273, 26f/273, 7f/273}, 
		{4f/273, 16f/273, 26f/273, 16f/273, 4f/273}, 
		{1f/273, 4f/273, 7f/273, 4f/273, 1f/273}
	};
	void Start()
	{
		generateRoom();
		generateNoise();
	}

	// Update is called once per frame
	void Update()
	{
		if (lightingUpdateTimer > lightingUpdateDuration) {
			updateLighting();
			lightingUpdateTimer = 0;
		}
		if (formUpdateTimer > formUpdateDuration) {
			updateForm_noise();
			formUpdateTimer = 0;
		}

		lightingUpdateTimer += Time.deltaTime;
		formUpdateTimer += Time.deltaTime;

	}

	void updateLighting() {
		for (int i=0;i<offset * 2 + size;i++) {
			for (int j=0;j<offset * 2 + size;j++) {
				if (ceilingLight[i, j].activeSelf) {
					ceiling[i, j].GetComponent<MeshRenderer>().material = defaultMaterial;
					ceilingLight[i, j].SetActive(false);
				}
			}
		}
		for (int i=0;i<offset * 2 + size;i++) {
			for (int j=0;j<offset * 2 + size;j++) {
				if (i == lightingStep || j == lightingStep) {
					ceiling[i, j].GetComponent<MeshRenderer>().material = emissiveMaterial;
					ceilingLight[i, j].SetActive(true);
				}
			}
		}
		lightingStep += lightingStepDir;
		if (lightingStep == offset + size - 1) {
			lightingStepDir = -1;
		}
		if (lightingStep == offset) {
			lightingStepDir = 1;
		}
	}

	void updateForm_noise() {
		for (int i = 0; i < size + offset * 2; i++) {
			for (int j = 0; j < size + offset * 2; j++) {
				ceiling[i,j].transform.localScale = new Vector3(1, size + (noise[(i + formStep) % (size + offset * 2), j] - 0.5f) * offset, 1);
			}
		}
		for (int i = 0; i < size + offset * 2; i++) {
			for (int j = 0; j < height + offset * 2; j++) {
				frontWall[i,j].transform.localScale = new Vector3(1, 1, size + (noise[(i + formStep) % (size + offset * 2), j] - 0.5f) * offset * 2);
			}
		}
		for (int i = 0; i < size + offset * 2; i++) {
			for (int j = 0; j < height + offset * 2; j++) {
				backWall[i,j].transform.localScale = new Vector3(1, 1, size + (noise[(i + formStep) % (size + offset * 2), j] - 0.5f) * offset * 2);
			}
		}
		for (int i = 0; i < size + offset * 2; i++) {
			for (int j = 0; j < height + offset * 2; j++) {
				leftWall[i,j].transform.localScale = new Vector3(size + (noise[(i + formStep) % (size + offset * 2), j] - 0.5f) * offset * 2, 1, 1);
			}
		}
		for (int i = 0; i < size + offset * 2; i++) {
			for (int j = 0; j < height + offset * 2; j++) {
				rightWall[i,j].transform.localScale = new Vector3(size + (noise[(i + formStep) % (size + offset * 2), j] - 0.5f) * offset * 2, 1, 1);
			}
		}
		formStep++;
	}

	void generateRoom() {
		ground = new GameObject[size + offset * 2, size + offset * 2];
		for (int i = 0; i < size + offset * 2; i++) {
			for (int j = 0; j < size + offset * 2; j++) {
				ground[i,j] = (GameObject)Instantiate(cellPrefab, new Vector3(i + 0.5f - offset, - offset, j + 0.5f - offset), Quaternion.identity);
				ground[i,j].transform.localScale = new Vector3(1, size, 1);
			}
		}
		frontWall = new GameObject[size + offset * 2, height + offset * 2];
		for (int i = 0; i < size + offset * 2; i++) {
			for (int j = 0; j < height + offset * 2; j++) {
				frontWall[i,j] = (GameObject)Instantiate(cellPrefab, new Vector3(i + 0.5f - offset, j + 0.5f - offset, size + offset), Quaternion.identity);
				frontWall[i,j].transform.localScale = new Vector3(1, 1, size);
			}
		}
		backWall = new GameObject[size + offset * 2, height + offset * 2];
		for (int i = 0; i < size + offset * 2; i++) {
			for (int j = 0; j < height + offset * 2; j++) {
				backWall[i,j] = (GameObject)Instantiate(cellPrefab, new Vector3(i + 0.5f - offset, j + 0.5f - offset, - offset), Quaternion.identity);
				backWall[i,j].transform.localScale = new Vector3(1, 1, size);
			}
		}
		leftWall = new GameObject[size + offset * 2, height + offset * 2];
		for (int i = 0; i < size + offset * 2; i++) {
			for (int j = 0; j < height + offset * 2; j++) {
				leftWall[i,j] = (GameObject)Instantiate(cellPrefab, new Vector3(- offset, j + 0.5f - offset, i + 0.5f - offset), Quaternion.identity);
				leftWall[i,j].transform.localScale = new Vector3(size, 1, 1);
			}
		}
		rightWall = new GameObject[size + offset * 2, height + offset * 2];
		for (int i = 0; i < size + offset * 2; i++) {
			for (int j = 0; j < height + offset * 2; j++) {
				rightWall[i,j] = (GameObject)Instantiate(cellPrefab, new Vector3(size + offset, j + 0.5f - offset, i + 0.5f - offset), Quaternion.identity);
				rightWall[i,j].transform.localScale = new Vector3(size, 1, 1);
			}
		}
		ceiling = new GameObject[size + offset * 2, size + offset * 2];
		for (int i = 0; i < size + offset * 2; i++) {
			for (int j = 0; j < size + offset * 2; j++) {
				ceiling[i,j] = (GameObject)Instantiate(cellPrefab, new Vector3(i + 0.5f - offset, size, j + 0.5f - offset), Quaternion.identity);
				ceiling[i,j].transform.localScale = new Vector3(1, size, 1);
			}
		}
		ceilingLight = new GameObject[size + offset * 2, size + offset * 2];
		for (int i = 0; i < size + offset * 2; i++) {
			for (int j = 0; j < size + offset * 2; j++) {
				ceilingLight[i,j] = (GameObject)Instantiate(lightPrefab, new Vector3(0, 0, 0), Quaternion.identity);
				ceilingLight[i,j].SetActive(false);
				ceilingLight[i,j].transform.SetParent(ceiling[i,j].transform);
				ceilingLight[i,j].transform.localPosition = new Vector3(0, -0.51f, 0);
			}
		}
	}

	void generateNoise() {
		randomSample = new float[size + offset * 2, size + offset * 2];
		noise = new float[size + offset * 2, size + offset * 2];
		for (int i=0;i<size + offset * 2;i++) {
			for (int j=0;j<size + offset * 2;j++) {
				randomSample[i, j] = Random.Range(-1f, 1f);
			}
		}
		for (int i=0;i<size + offset * 2;i++) {
			for (int j=0;j<size + offset * 2;j++) {
				float acc = 0;
				for (int k=-boxSize;k<=boxSize;k++) {
					for (int l=-boxSize;l<=boxSize;l++) {
						acc += randomSample[(i + k + size + offset * 2) % (size + offset * 2), (j + l + size + offset * 2) % (size + offset * 2)] * kernel[k + boxSize, l + boxSize];
					}
				}
				noise[i, j] = acc;
			}
		}
	}
}
