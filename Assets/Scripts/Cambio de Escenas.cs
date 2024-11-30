using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiodeEscenas : MonoBehaviour
{
 public void optionScene()
 {
    SceneManager.LoadScene(2);
 }
 public void PlayScene()
 {
    SceneManager.LoadScene(0);
 }
 public void Salirjuego()
 {
    Application.Quit();
 }
 public void menustart()
 {
    SceneManager.LoadScene(1);
 }
}
