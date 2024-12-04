using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class CambiodeEscenas : MonoBehaviour
{
  private void Awake()
  
  {


  }

 public IEnumerator CambiarEscenadespues()
 {
yield return new WaitForSeconds(2f);

 }
 public IEnumerator playescene()
 {
   yield return new WaitForSeconds(5f);
   SceneManager.LoadScene(1);
 }

 public void PlayScene()
 {
      StartCoroutine(CambiarEscenadespues());
      SceneManager.LoadScene(1);
 }
 public void Salirjuego()
 {
    Application.Quit();
 }
public void menuincio()
{
   SceneManager.LoadScene(0);
}

}

