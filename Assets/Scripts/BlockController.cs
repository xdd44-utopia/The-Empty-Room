using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
	public Texture2D tex;
	public Material mat;
	private const int size = 25;
	private const float scale = 100f;
	private int texWidth;
	private int texHeight;
	// Start is called before the first frame update
	void Start()
	{
		GetComponent<Renderer>().material = new Material(mat);
		setUpUV();
		texWidth = tex.width;
		texHeight = tex.height;
	}

	// Update is called once per frame
	void Update()
	{
		updateTexture();
	}

	void updateTexture() {
		Texture2D newTex = new Texture2D(size * 3, size * 3, TextureFormat.RGBA32, false);
		for (int i=0;i<size;i++) {
			for (int j=0;j<size;j++) {
				newTex.SetPixel((size - i) + size, (size - j), tex.GetPixel((int)(transform.position.x * size + i) % texWidth, (int)(transform.position.z * size + j) % texHeight));
				// newTex.SetPixel(i + size, j + size, tex.GetPixel((int)(transform.position.x * scale + i) % texWidth, (int)(transform.position.z * scale + j) % texHeight));
				// newTex.SetPixel(i + size, j + 2 * size, tex.GetPixel((int)(transform.position.x * scale + i) % texWidth, (int)(transform.position.z * scale + j) % texHeight));
				// newTex.SetPixel(i + 2 * size, j, tex.GetPixel((int)(transform.position.x * scale + i) % texWidth, (int)(transform.position.z * scale + j) % texHeight));
				// newTex.SetPixel(i + 2 * size, j + size, tex.GetPixel((int)(transform.position.x * scale + i) % texWidth, (int)(transform.position.z * scale + j) % texHeight));
				// newTex.SetPixel(i + 2 * size, j + 2 * size, tex.GetPixel((int)(transform.position.x * scale + i) % texWidth, (int)(transform.position.z * scale + j) % texHeight));
			}
		}
		Color applyColor = tex.GetPixel((int)(transform.position.x * scale) % texWidth, (int)(transform.position.z * scale) % texHeight);
		newTex.Apply();
		GetComponent<Renderer>().material.SetTexture("_EmissionMap", newTex);
		Texture2D checkTex = (Texture2D)GetComponent<Renderer>().material.GetTexture("_EmissionMap");
		Color checkColor = checkTex.GetPixel(0, 0);
		// Debug.Log(applyColor + " "  + checkColor);
	}

	void setUpUV() {
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Vector2[] uvs = mesh.uv;
		
		// Front
		uvs[0]  = new Vector2(0.0f, 0.0f);
		uvs[1]  = new Vector2(0.333f, 0.0f);
		uvs[2]  = new Vector2(0.0f, 0.333f);
		uvs[3]  = new Vector2(0.333f, 0.333f);
		
		// Top
		uvs[8]  = new Vector2(0.334f, 0.0f);
		uvs[9]  = new Vector2(0.666f, 0.0f);
		uvs[4]  = new Vector2(0.334f, 0.333f);
		uvs[5]  = new Vector2(0.666f, 0.333f);
		
		// Back
		uvs[10] = new Vector2(0.667f, 0.0f);
		uvs[11] = new Vector2(1.0f, 0.0f);
		uvs[6]  = new Vector2(0.667f, 0.333f);
		uvs[7]  = new Vector2(1.0f, 0.333f);
		
		// Bottom
		uvs[12] = new Vector2(0.0f, 0.334f);
		uvs[14] = new Vector2(0.333f, 0.334f);
		uvs[15] = new Vector2(0.0f, 0.666f);
		uvs[13] = new Vector2(0.333f, 0.666f);                
		
		// Left
		uvs[16] = new Vector2(0.334f, 0.334f);
		uvs[18] = new Vector2(0.666f, 0.334f);
		uvs[19] = new Vector2(0.334f, 0.666f);
		uvs[17] = new Vector2(0.666f, 0.666f);    
		
		// Right        
		uvs[20] = new Vector2(0.667f, 0.334f);
		uvs[22] = new Vector2(1.00f, 0.334f);
		uvs[23] = new Vector2(0.667f, 0.666f);
		uvs[21] = new Vector2(1.0f, 0.666f);    
		
		mesh.uv = uvs;
	}
}
