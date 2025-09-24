using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Grab.Runtime;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Machine.Runtime
{
    public class CookStationMultiIngredient : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private ParticleSystem particleBurn;
        [SerializeField] [CanBeNull] private ParticleSystem particleFrying;
        [SerializeField] private ParticleSystem? particleCrame;
        [SerializeField] private ParticleSystem? particleFlash1;
        [SerializeField] private ParticleSystem? particleFlash2;
        [SerializeField] private ParticleSystem? particleFlash3;
        [Header("Recettes")]
        [SerializeField] private MultiIngredientRecipe[] recipes;
        [Header("Apparence et feedback")]
        [SerializeField] private Transform output;
        [SerializeField] private GameObject poopPrefab;
        [SerializeField] private GameObject uiReturn;
        [SerializeField] private Image uiProgression;
        [SerializeField] private GameObject uiBarProgression;
        [SerializeField] private GameObject uiIcone;
        [SerializeField] private GameObject uiDone;
        [Header("Points d'apparition des ingrédients (ordre important !)")]
        [SerializeField] private Transform[] ingredientPoints;
        [Header("Scales designers par ingrédient (ordre identique aux points)")]
        [SerializeField] private Vector3[] ingredientScales;
        [Header("Paramètres de la poêle")]
        [SerializeField] private int maxIngredients = 4;
        [Header("Timers de cuisson (secondes)")]
        [SerializeField] private float timerAvantRetourner = 3f;
        [SerializeField] private float timerAvantPlatFini = 3f;
        [SerializeField] private float timerAvantCramé = 2f; // marge de sécurité

        private Coroutine currentCookingRoutine;
        private float elapsed;

        private readonly List<Food> _storedFoods = new();
        private Food[] _storedFoodsArray;
        private bool _isCooking = false;
        private float _progress;

        public bool isRetourned;
        public bool _goFinish { get; private set; }
        public bool isFinished { get; private set; }
        public bool _goReturn { get; private set; }

        public bool IsCooking => _isCooking;


        private void Awake()
        {
            _storedFoodsArray = new Food[ingredientPoints.Length];
        }

        public void AddFood(Food food)
        {
            if (_isCooking)
                return; // cuisson en cours !

            if (_storedFoods.Count >= maxIngredients)
                return; // trop d'ingrédients

            if (food == null) return;

            bool isPartOfAnyRecipe = recipes != null &&
                                     recipes.Any(r => r != null &&
                                             r.IngredientsInput != null &&
                                             r.IngredientsInput.Contains(food.FoodType));
            if (!isPartOfAnyRecipe)
            {
                WrongIngredient(food);
                return;
            }

            if (_storedFoods.Contains(food)) return;

            // Remplacement si même type déjà dans la poêle
            var existing = _storedFoods.FirstOrDefault(f => f.FoodType == food.FoodType);
            if (existing != null)
            {
                int index = _storedFoods.IndexOf(existing);
                _storedFoods.RemoveAt(index);
                Destroy(existing.gameObject);
            }

            _storedFoods.Add(food);
            food.GetComponentInChildren<Grabable>().enabled = false;

            int slotIndex = _storedFoods.Count - 1;
            if (ingredientPoints.Length > slotIndex && ingredientPoints[slotIndex] != null)
            {
                _storedFoodsArray[slotIndex] = food;

                // sauver le parent d’origine
                Transform oldParent = food.transform.parent.parent;

                food.transform.SetParent(ingredientPoints[slotIndex]);
                food.transform.localPosition = Vector3.zero;
                food.transform.localRotation = Quaternion.identity;

                if (food.Rb != null)
                {
                    food.Rb.isKinematic = true;
                    food.Rb.useGravity = false;
                    food.Rb.linearVelocity = Vector3.zero;
                    food.Rb.angularVelocity = Vector3.zero;
                }

                if (ingredientScales.Length > slotIndex)
                    food.transform.localScale = ingredientScales[slotIndex];

                // si l’ancien parent n’est pas null → le détruire après avoir replacé l’enfant
                if (oldParent != null)
                {
                    Destroy(oldParent.gameObject);
                }
            }

            food.gameObject.SetActive(true); // On les voit dans la poêle !
        }


        private void WrongIngredient(Food lastTried)
        {
            // Vide la poêle et spawn le caca
            foreach (var food in _storedFoods)
            {
                if (food != null)
                {
                    // Libère le slot correspondant 
                    int slot = Array.IndexOf(_storedFoodsArray, food);
                    if (slot != -1)
                        _storedFoodsArray[slot] = null;
                    
                    Destroy(food.gameObject);
                }
            }
            
            _storedFoods.Clear();
            
            // Spawn du caca
            if (poopPrefab != null)
                Instantiate(poopPrefab, output.position, Quaternion.identity);
            
            // Jouer les particules de cramé si elles existent
            if (particleCrame != null)
                particleCrame.Play();
        }

        private void SpawnCookedFood(GameObject prefab)
        {
            if (prefab != null)
                Instantiate(prefab, output.position, Quaternion.identity);
        }

        public void TryManualCook()
        {
            if (_isCooking || _storedFoods.Count == 0)
                return;

            // Recherche de la recette
            MultiIngredientRecipe matchedRecipe = null;
            foreach (var recipe in recipes)
            {
                bool allPresent = recipe.IngredientsInput.All(ingredient =>
                    _storedFoods.Any(f => f.FoodType == ingredient));
                bool correctCount = _storedFoods.Count == recipe.IngredientsInput.Count;
                if (allPresent && correctCount)
                {
                    matchedRecipe = recipe;
                    break;
                }
            }

            if (matchedRecipe != null)
            {
                if (currentCookingRoutine != null)
                    StopCoroutine(currentCookingRoutine);

                _isCooking = true;
                currentCookingRoutine = StartCoroutine(CookSequence(matchedRecipe));
            }
            else
            {
                WrongIngredient(null);
            }
        }

        private IEnumerator CookSequence(MultiIngredientRecipe recipe)
        {
            uiIcone.SetActive(false);
            uiBarProgression.SetActive(true);
            _isCooking = true;
            if (particleFlash1 != null)
                particleFlash1.Play();
            if (particleFlash2 != null)
                particleFlash2.Play();
            if (particleFlash3 != null)
                particleFlash3.Play();
            
            float timerRetourner = timerAvantRetourner;
            float timerCrame = timerAvantCramé;
            float timerPlatFini = timerAvantPlatFini;

            // ----------- PHASE 1 : cuisson avant retour ---------
            animator.SetBool("Frying", true);
            if (particleFrying != null)
                particleFrying.Play();
            bool crame = false;
            elapsed = 0f;
            
            while (elapsed < timerRetourner + timerCrame)
            {
                yield return null;
                elapsed += Time.deltaTime;
                _progress = elapsed / timerRetourner;
                uiProgression.fillAmount = _progress;
                
                if (elapsed >= timerRetourner && !_goReturn)
                {
                    _goReturn = true;
                    particleBurn.Play();
                }

                if (isRetourned)
                {
                    break;
                }

                if (elapsed >= timerRetourner + timerCrame)
                {
                    crame = true;
                    break;
                }
            }
            
            if (crame)
            {
                animator.SetBool("Echec", true);
                animator.SetBool("Frying", false);
                particleBurn.Play();

                uiBarProgression.SetActive(false);
                WrongIngredient(null);

                yield return new WaitForSeconds(1f); // le temps que l'anim "Echec" joue

                ResetMachineState();
                animator.SetBool("Echec", false);
                yield break;
            }

            _goReturn = false;

            // ---------- PHASE 2 : après avoir retourné ----------
            elapsed = 0f;
            crame = false;
            
            while (elapsed < timerPlatFini + timerCrame)
            {
                yield return null;
                elapsed += Time.deltaTime;
                _progress = elapsed / timerPlatFini;
                uiProgression.fillAmount = _progress;
                
                if (elapsed >= timerPlatFini && !_goFinish)
                {
                    uiDone.SetActive(true);
                    _goFinish = true;
                    animator.SetBool("Done", true);
                }

                if (isFinished)
                {
                    break;
                }

                if (elapsed >= timerPlatFini + timerCrame)
                {
                    crame = true;
                    break;
                }
            }
            
            if (crame)
            {
                // Animation d'échec pour le cramage en phase 2
                animator.SetBool("Frying", false);
                animator.SetBool("Done", false);
                animator.SetBool("Echec", true);
                particleBurn.Play();
                
                uiBarProgression.SetActive(false);
                WrongIngredient(null);

                ResetMachineState();
                
                // Remettre Echec à false avant de sortir
                animator.SetBool("Echec", false);
                yield break;
            }

            // ----------- FINI ----------
            SpawnCookedFood(recipe.OutputPrefab);

            // Suppression des ingrédients après cuisson réussie
            foreach (var ingredient in recipe.IngredientsInput)
            {
                var foodToRemove = _storedFoods.FirstOrDefault(f => f.FoodType == ingredient);
                if (foodToRemove != null)
                {
                    _storedFoods.Remove(foodToRemove);
                    int slotToFree = Array.IndexOf(_storedFoodsArray, foodToRemove);
                    if (slotToFree != -1)
                        _storedFoodsArray[slotToFree] = null;

                    Destroy(foodToRemove.gameObject);
                }
            }

            // Utiliser la méthode de réinitialisation complète
            ResetMachineState();
            _storedFoods.Clear(); // Garder ce clear car il n'est pas dans ResetMachineState
        }

        // Nouvelle méthode pour réinitialiser complètement l'état de la machine
        private void ResetMachineState()
        {
            // Arrêt des animations
            animator.SetBool("Frying", false);
            animator.SetBool("Done", false);
            animator.SetBool("Flip", false);
            
            // Arrêt des particules
            if (particleFrying.isPlaying && particleFrying != null)
                particleFrying.Stop();
            if (particleBurn.isPlaying)
                particleBurn.Stop();
            if (particleCrame != null && particleCrame.isPlaying)
                particleCrame.Stop();
            
            // Réinitialisation des états
            _isCooking = false;
            isFinished = false;
            isRetourned = false;
            _goFinish = false;
            _goReturn = false;
            _progress = 0f;
            elapsed = 0f;
            
            // Réinitialisation de l'UI
            uiBarProgression.SetActive(false);
            uiIcone.SetActive(true);
            uiDone.SetActive(false);
            uiReturn.SetActive(false);
            uiProgression.fillAmount = 0f;
            
            // Arrêt de la coroutine
            if (currentCookingRoutine != null)
            {
                StopCoroutine(currentCookingRoutine);
                currentCookingRoutine = null;
            }
        }

        public void PlayRetournerAnimation()
        {
            // Ici appelle une animation sur la poêle ou feedback visuel
            animator.SetBool("Flip", true);
            isRetourned = true;
        }

        public void FinishFoodFrying()
        {
            if (!_goFinish) return;
            animator.SetBool("Frying", false);
            isFinished = true;
        }

        private void Update()
        {
            uiProgression.fillAmount = _progress;

            if (_goReturn) uiReturn.SetActive(true);
            else uiReturn.SetActive(false);
        }


        //----------------Animation event zone--------------//

        public void FinishRetournerAnimation()
        {
            animator.SetBool("Flip", false);
            animator.SetBool("Frying", true);
        }

        public void FinishFoodFryingAnimation()
        {
            animator.SetBool("Frying", false);
            animator.SetBool("Done", false);
        }

    }
}