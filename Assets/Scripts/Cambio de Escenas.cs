using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class CambiodeEscenas : MonoBehaviour
{

 public IEnumerator CambiarEscenadespues()
 {
yield return new WaitForSeconds(2f);
 SceneManager.LoadScene(2);
 }
 public IEnumerator playescene()
 {
   yield return new WaitForSeconds(5f);
   SceneManager.LoadScene(0);
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

