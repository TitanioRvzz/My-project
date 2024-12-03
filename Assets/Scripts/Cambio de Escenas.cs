using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiodeEscenas : MonoBehaviour
{
   private void Start() 
   {
    
   }
 
 public IEnumerator CambiarEscenadespues()
 {
yield return new WaitForSeconds(2f);
 SceneManager.LoadScene(2);
 }
 public IEnumerator playescene()
 {
   yield return new WaitForSeconds(2f);
   SceneManager.LoadScene(0);
 }
 public IEnumerator menuScene()
 {
   yield return new WaitForSeconds(2f);
   menuincio();
    
 }
 
 
 public void optionScene()
   {
       StartCoroutine(CambiarEscenadespues());
      // SceneManager.LoadScene(2);
   }
    
 
 public void PlayScene()
 {
      StartCoroutine(CambiarEscenadespues());
      SceneManager.LoadScene(0);
 }
 public void Salirjuego()
 {
    Application.Quit();
 }
public void menuincio()
{
   SceneManager.LoadScene(1);
}

}

