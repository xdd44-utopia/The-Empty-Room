using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellGenerator : MonoBehaviour
{
	public GameObject cellPrefab;
	// Start is called before the first frame update
	void Start()
	{
		for (int i=0;i<16;i++) {
			for (int j=0;j<9;j++) {
				GameObject.Instantiate(cellPrefab, new Vector3(0, i, j), Quaternion.identity);
			}
		}
	}

	// Update is called once per frame
	void Update()
	{

	}
}
