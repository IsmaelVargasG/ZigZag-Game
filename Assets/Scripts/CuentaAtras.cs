using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CuentaAtras : MonoBehaviour
{
    public Button start;
    public Image numero;
    public Sprite[] numeros;

    // Start is called before the first frame update
    void Start()
    {
        start.onClick.AddListener(Empezar);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Empezar(){
        numero.gameObject.SetActive(true);
        start.gameObject.SetActive(false);

        StartCoroutine(PonerNumeros());
    }

    IEnumerator PonerNumeros(){
        for(int i = numeros.Length-1; i >= 0; i--){
            numero.sprite = numeros[i];
            yield return new WaitForSeconds(1);
        }

        SceneManager.LoadScene("Nivel1");
    }

}
