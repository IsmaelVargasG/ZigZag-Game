using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class JugadorBola : MonoBehaviour
{
    public Camera camara;
    public GameObject suelo;
    public GameObject moneda;
    public GameObject obstaculo;
    public float velocidad = 5.0f;
    public TextMeshProUGUI Contador;
    public TextMeshProUGUI Vidas_texto;
    public int Puntuacion = 0;
    public Material daño;
    public GameObject pauseMenuUI;
    public GameObject overMenuUI;
    

    private Material normal;
    private Vector3 offset;
    private float ValX, ValZ;
    private Vector3 DireccionActual;
    private float altura;
    private Vector3 rotacion;
    private int vidas = 3;
    private Queue<Vector3> posicion_suelo;
    private int nivel2 = 200;

    // Start is called before the first frame update
    void Start()
    {
        posicion_suelo = new Queue<Vector3>();
        offset = camara.transform.position - transform.position;
        CrearSueloInicial();
        DireccionActual = Vector3.forward;
        altura = transform.position.y;
        rotacion = transform.eulerAngles;
        Vidas_texto.text = "x " + vidas;
        normal = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        camara.transform.position = transform.position + offset;
        if(Input.GetKeyUp(KeyCode.Space)){
            CambiarDireccion();
        }
        if(Input.GetKeyUp(KeyCode.P)){
            PauseGame();
        }

        transform.Translate(DireccionActual * velocidad * Time.deltaTime);
        if(transform.position.y > altura){
            transform.position = new Vector3(transform.position.x, altura, transform.position.z);
        }
        transform.eulerAngles = rotacion;
        if(transform.position.y < altura - 2){
            if(vidas > 1){
                --vidas;
                Vidas_texto.text = "x " + vidas;
                ResetearPosicion();
            }
            else{
                GameOver();
            }
        }
    }

    void CrearSueloInicial(){
        for(int i = 0; i < 3; i++){
            ValZ += 6.0f;
            Instantiate(suelo, new Vector3(ValX, 0, ValZ), Quaternion.identity);
            posicion_suelo.Enqueue(new Vector3(ValX, 0, ValZ));
        }
    }

    private void OnCollisionExit(Collision other){
        if(other.gameObject.tag == "Suelo"){
            StartCoroutine(BorrarSuelo(other.gameObject));
            posicion_suelo.Dequeue();
        }
    }

    void OnBecameInvisible(){
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Moneda")){
            Puntuacion += 10;
            Contador.text = "SCORE: " + Puntuacion;
            Destroy(other.gameObject);
        }

        if(other.gameObject.CompareTag("Obstaculo")){
            other.gameObject.GetComponent<Collider>().enabled = false;
            StartCoroutine(cambiarColor());
            if(vidas > 1){
                --vidas;
                Vidas_texto.text = "x " + vidas;
            }
            else{
                GameOver();
            }
        }
    }

    IEnumerator cambiarColor(){
        GetComponent<Renderer>().material = daño;
        yield return new WaitForSeconds(0.2f);
        GetComponent<Renderer>().material = normal;
    }

    IEnumerator BorrarSuelo(GameObject sueloC){
        float aleatorio = Random.Range(0.0f, 1.0f);
        float monX = Random.Range(-2.0f, 2.0f);
        float monZ = Random.Range(-2.0f, 2.0f);

        if(aleatorio > 0.5){
            ValX += 6.0f;
        }
        else{
            ValZ += 6.0f;
        }

        aleatorio = Random.Range(0.0f, 1.0f);
        posicion_suelo.Enqueue(new Vector3(ValX, 0, ValZ));
        Instantiate(suelo, new Vector3(ValX, 0, ValZ), Quaternion.identity);
        if(aleatorio > 0.5){
            Instantiate(moneda, new Vector3(ValX+monX, 1, ValZ+monZ), moneda.transform.rotation);
        }
        else if(Puntuacion >= nivel2){
            Instantiate(obstaculo, new Vector3(ValX+monX, 1, ValZ+monZ), Quaternion.identity);
        }

        yield return new WaitForSeconds(0.1f);
        sueloC.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        sueloC.gameObject.GetComponent<Rigidbody>().useGravity = true;
        yield return new WaitForSeconds(2);
        Destroy(sueloC);
    }

    private void ResetearPosicion(){
        Vector3 siguiente = posicion_suelo.Peek();
        transform.position = new Vector3(siguiente.x, altura, siguiente.z);
        transform.eulerAngles = rotacion;
        if(Physics.CheckSphere(new Vector3(siguiente.x + 6, altura, siguiente.z), 1f)){
            DireccionActual = Vector3.right;
        }
        else{
            DireccionActual = Vector3.forward;
        }
    }

    void CambiarDireccion(){
        if(DireccionActual == Vector3.forward){
            DireccionActual = Vector3.right;
        }
        else{
            DireccionActual = Vector3.forward;
        }
    }

    void PauseGame(){
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    void GameOver(){
        overMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }
}
