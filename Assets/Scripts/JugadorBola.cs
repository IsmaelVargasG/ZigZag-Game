using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JugadorBola : MonoBehaviour
{
    public Camera camara;
    public GameObject suelo;
    public float velocidad = 5.0f;

    private Vector3 offset;
    private float ValX, ValZ;
    private Vector3 DireccionActual;
    private float altura;
    private Vector3 rotacion;

    // Start is called before the first frame update
    void Start()
    {
        offset = camara.transform.position - transform.position;
        CrearSueloInicial();
        DireccionActual = Vector3.forward;
        altura = transform.position.y;
        rotacion = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        camara.transform.position = transform.position + offset;
        if(Input.GetKeyUp(KeyCode.Space)){
            CambiarDireccion();
        }
        transform.Translate(DireccionActual * velocidad * Time.deltaTime);
        if(transform.position.y > altura){
            transform.position = new Vector3(transform.position.x, altura, transform.position.z);
        }
        transform.eulerAngles = rotacion;
    }

    void CrearSueloInicial(){
        for(int i = 0; i < 3; i++){
            ValZ += 6.0f;
            Instantiate(suelo, new Vector3(ValX, 0, ValZ), Quaternion.identity);
        }
    }

    private void OnCollisionExit(Collision other){
    
        if(other.gameObject.tag == "Suelo"){
            StartCoroutine(BorrarSuelo(other.gameObject));
        }
    }

    IEnumerator BorrarSuelo(GameObject sueloC){
        float aleatorio = Random.Range(0.0f, 1.0f);
        if(aleatorio > 0.5){
            ValX += 6.0f;
        }
        else{
            ValZ += 6.0f;
        }

        
        Instantiate(suelo, new Vector3(ValX, 0, ValZ), Quaternion.identity);
        yield return new WaitForSeconds(0.1f);
        sueloC.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        sueloC.gameObject.GetComponent<Rigidbody>().useGravity = true;
        yield return new WaitForSeconds(2);
        Destroy(sueloC);
    }

    void CambiarDireccion(){
        if(DireccionActual == Vector3.forward){
            DireccionActual = Vector3.right;
        }
        else{
            DireccionActual = Vector3.forward;
        }
    }
}
