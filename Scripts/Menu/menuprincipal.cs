using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class menuprincipal : MonoBehaviour
{
   public static menuprincipal instancia { get; private set; }
   public GameObject opçao, credito, dlc, classes;
   private ToggleGroup toggleGroup;
   [SerializeField] private List<Toggle> toggles = new List<Toggle>();
   [SerializeField] private Toggle activeToggle;


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

   public void EscolherClasse()
   {
      classes.gameObject.SetActive(true);
      // Buscar todos os toggles em cena
      Toggle[] togglesArray = FindObjectsOfType<Toggle>();
      // Adicionar os toggles a lista por ordem alfabética
      toggles.AddRange(togglesArray.OrderBy(t => t.name));
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
      classes.gameObject.SetActive(false);
   }

   public void GetSelectedToggleIndex()
   {
      activeToggle = toggles.FirstOrDefault(t => t.isOn);
      if(activeToggle != null) print($"Toggle selecionado: {toggles.IndexOf(activeToggle)} | Classe: {activeToggle.name}");

      PlayerPrefs.SetInt("PlayerClass", toggles.IndexOf(activeToggle));
   }
}
