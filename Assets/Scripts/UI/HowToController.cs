using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToController : MonoBehaviour
{
	[SerializeField]
	private GameObject[] pages;
	private int curr_page = 0;
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

	public void Next() {
		curr_page++;
	}

	public void Prev() {
		curr_page--;
	}

	public void Return() {
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}

	private void UpdatePages() {
		for (int i = 0; i < pages.Length; i++) {
			pages[i].SetActive(i == curr_page);
		}
	}
}
