using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pausa : MonoBehaviour
{
    [SerializeField] GameObject botonPausa;
    [SerializeField] GameObject menuPausa;
  
   private void Update()
   {
   if(Input.GetKeyDown(KeyCode.Escape))
   {
    pausa();
   } 
   }
public void pausa()
{

Time.timeScale = 0f;
botonPausa.SetActive(false);
menuPausa.SetActive(true);
}

public void Reaundar()
{
    
    Time.timeScale = 1f;
    botonPausa.SetActive(true);
    menuPausa.SetActive(false);
}
public void salir()
{
  SceneManager.LoadScene(0);
}
public void reiniciar()
{
   
Time.timeScale = 1f;
SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}

}
