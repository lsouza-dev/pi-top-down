using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class menuprincipal : MonoBehaviour
{
   public static menuprincipal instancia { get; private set; }
   public GameObject opçao, credito, dlc;


   private void Awake()
   {
      if (instancia == null)
         instancia = this;
   }
   void Start()
   {
      opçao.gameObject.SetActive(false);
      credito.gameObject.SetActive(false);
      dlc.gameObject.SetActive(false);
   }

   public void iniciar()
   {
      SceneManager.LoadScene("Game");
   }
   public void opções()
   {
      opçao.gameObject.SetActive(true);
   }
   public void creditos()
   {
      credito.gameObject.SetActive(true);
   }
   public void sair()
   {
      Application.Quit();
   }

   public void maiscoisa()
   {
      dlc.gameObject.SetActive(true);
   }

   public void fecharJanela()
   {
      opçao.gameObject.SetActive(false);
      credito.gameObject.SetActive(false);
      dlc.gameObject.SetActive(false);
   }
}
