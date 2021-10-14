using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormController : MonoBehaviour
{
	public GameObject cellPrefab;
	private GameObject[,] ground;
	private GameObject[,] frontWall;
	private GameObject[,] backWall;
	private GameObject[,] leftWall;
	private GameObject[,] rightWall;
	private GameObject[,] ceiling;
	private const int size = 64;
	private const int height = 32;
	void Start()
	{
		ground = new GameObject[size * 2, size * 2];
		for (int i = 0; i < size * 2; i++) {
			for (int j = 0; j < size * 2; j++) {
				ground[i,j] = (GameObject)Instantiate(cellPrefab, new Vector3(i + 0.5f - size / 2, - size / 2, j + 0.5f - size / 2), Quaternion.identity);
				ground[i,j].transform.localScale = new Vector3(1, size, 1);
			}
		}
		frontWall = new GameObject[size * 2, height * 3];
		for (int i = 0; i < size * 2; i++) {
			for (int j = 0; j < height * 3; j++) {
				frontWall[i,j] = (GameObject)Instantiate(cellPrefab, new Vector3(i + 0.5f - size / 2, j + 0.5f - size / 2, size * 3 / 2), Quaternion.identity);
				frontWall[i,j].transform.localScale = new Vector3(1, 1, size);
			}
		}
		backWall = new GameObject[size * 2, height * 3];
		for (int i = 0; i < size * 2; i++) {
			for (int j = 0; j < height * 3; j++) {
				backWall[i,j] = (GameObject)Instantiate(cellPrefab, new Vector3(i + 0.5f - size / 2, j + 0.5f - size / 2, - size / 2), Quaternion.identity);
				backWall[i,j].transform.localScale = new Vector3(1, 1, size);
			}
		}
		leftWall = new GameObject[size * 2, height * 3];
		for (int i = 0; i < size * 2; i++) {
			for (int j = 0; j < height * 3; j++) {
				leftWall[i,j] = (GameObject)Instantiate(cellPrefab, new Vector3(- size / 2, j + 0.5f - size / 2, i + 0.5f - size / 2), Quaternion.identity);
				leftWall[i,j].transform.localScale = new Vector3(size, 1, 1);
			}
		}
		rightWall = new GameObject[size * 2, height * 3];
		for (int i = 0; i < size * 2; i++) {
			for (int j = 0; j < height * 3; j++) {
				rightWall[i,j] = (GameObject)Instantiate(cellPrefab, new Vector3(size * 3 / 2, j + 0.5f - size / 2, i + 0.5f - size / 2), Quaternion.identity);
				rightWall[i,j].transform.localScale = new Vector3(size, 1, 1);
			}
		}
		ceiling = new GameObject[size * 2, size * 2];
		for (int i = 0; i < size * 2; i++) {
			for (int j = 0; j < size * 2; j++) {
				ceiling[i,j] = (GameObject)Instantiate(cellPrefab, new Vector3(i + 0.5f - size / 2, size, j + 0.5f - size / 2), Quaternion.identity);
				ceiling[i,j].transform.localScale = new Vector3(1, size, 1);
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}
