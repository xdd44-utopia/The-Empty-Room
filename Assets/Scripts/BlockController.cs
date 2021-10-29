using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
	public Texture2D texSide;
	public Texture2D texTop;
	public Material mat;
	private const int size = 120;
	private const float scale = 100f;
	private int texWidth;
	private int texHeight;
	// Start is called before the first frame update
	void Start()
	{
		GetComponent<Renderer>().material = new Material(mat);
		setUpUV();
		texWidth = texSide.width;
		texHeight = texSide.height;
	}

	// Update is called once per frame
	void Update()
	{
		updateTexture();
	}

	void updateTexture() {
		Texture2D newTex = new Texture2D(size * 3, size * 3, TextureFormat.RGBA32, false);
		for (int i=0;i<size * 3; i++) {
			for (int j=0;j<size * 3; j++) {
				newTex.SetPixel(i, j, new Color(0, 0, 0, 1));
			}
		}
		for (int i=0;i<size;i++) {
			for (int j=0;j<size;j++) {
				newTex.SetPixel((size - i), j, texSide.GetPixel((int)(transform.position.x * size + i) % texWidth, (int)(transform.position.y * size + j) % texHeight));
				newTex.SetPixel((size - i), j + size, texTop.GetPixel((int)(transform.position.x * size + i) % texWidth, (int)(transform.position.z * size + j) % texWidth));
				newTex.SetPixel((size - i) + size, (size - j), texTop.GetPixel((int)(transform.position.x * size + i) % texWidth, (int)(transform.position.z * size + j) % texWidth));
				newTex.SetPixel((size - i) + size, j + size, texSide.GetPixel((int)(transform.position.z * size + i) % texWidth, (int)(transform.position.y * size + j) % texHeight));
				newTex.SetPixel((size - i) + 2 * size, (size - j), texSide.GetPixel((int)(transform.position.x * size + i) % texWidth, (int)(transform.position.y * size + j) % texHeight));
				newTex.SetPixel(i + 2 * size, j + size, texSide.GetPixel((int)(transform.position.z * size + i) % texWidth, (int)(transform.position.y * size + j) % texHeight));
			}
		}
		newTex.Apply();
		GetComponent<Renderer>().material.SetTexture("_EmissionMap", newTex);
		Texture2D checkTex = (Texture2D)GetComponent<Renderer>().material.GetTexture("_EmissionMap");
	}

	void setUpUV() {
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Vector2[] uvs = mesh.uv;
		
		// Front
		uvs[0]  = new Vector2(0.0f, 0.0f);
		uvs[1]  = new Vector2(1.0f / 3.0f, 0.0f);
		uvs[2]  = new Vector2(0.0f, 1.0f / 3.0f);
		uvs[3]  = new Vector2(1.0f / 3.0f, 1.0f / 3.0f);
		
		// Top
		uvs[8]  = new Vector2(1.0f / 3.0f, 0.0f);
		uvs[9]  = new Vector2(2.0f / 3.0f, 0.0f);
		uvs[4]  = new Vector2(1.0f / 3.0f, 1.0f / 3.0f);
		uvs[5]  = new Vector2(2.0f / 3.0f, 1.0f / 3.0f);
		
		// Back
		uvs[10] = new Vector2(2.0f / 3.0f, 0.0f);
		uvs[11] = new Vector2(1.0f, 0.0f);
		uvs[6]  = new Vector2(2.0f / 3.0f, 1.0f / 3.0f);
		uvs[7]  = new Vector2(1.0f, 1.0f / 3.0f);
		
		// Bottom
		uvs[12] = new Vector2(0.0f, 1.0f / 3.0f);
		uvs[15] = new Vector2(1.0f / 3.0f, 1.0f / 3.0f);
		uvs[13] = new Vector2(0.0f, 2.0f / 3.0f);
		uvs[14] = new Vector2(1.0f / 3.0f, 2.0f / 3.0f);                
		
		// Left
		uvs[16] = new Vector2(1.0f / 3.0f, 1.0f / 3.0f);
		uvs[19] = new Vector2(2.0f / 3.0f, 1.0f / 3.0f);
		uvs[17] = new Vector2(1.0f / 3.0f, 2.0f / 3.0f);
		uvs[18] = new Vector2(2.0f / 3.0f, 2.0f / 3.0f);    
		
		// Right        
		uvs[20] = new Vector2(2.0f / 3.0f, 1.0f / 3.0f);
		uvs[23] = new Vector2(1.00f, 1.0f / 3.0f);
		uvs[21] = new Vector2(2.0f / 3.0f, 2.0f / 3.0f);
		uvs[22] = new Vector2(1.0f, 2.0f / 3.0f);    
		
		mesh.uv = uvs;
	}
}
