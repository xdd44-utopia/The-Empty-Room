using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellGenerator : MonoBehaviour
{
	public GameObject cellPrefab;
	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		for (int i=0;i<25;i++) {
			for (int j=0;j<25;j++) {
				GameObject.Instantiate(cellPrefab, new Vector3(i, 0, j), Quaternion.identity);
			}
		}
	}
}
