using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This component attached on the craftRecipe UI prefabs
/// Store the information of receipe
/// Set recipe by CraftRecipeDisplay.cs when spawn
/// Destroyed by CraftRecipeDisplay.cs
/// </summary>
public class CraftRecipeBar : MonoBehaviour
{
    public Recipe recipe;
    public ReturnIconItemInformation resultItemInformation;
    public GameObject IngredientSlotPrefab;
    public Transform GridlayoutGrounp;

    public void Initlize(Recipe SetRecipe)
    {
        recipe = SetRecipe;
        resultItemInformation.DisplayItem = ItemDatabase.Instance.GetItemById(recipe.outputItemID);
        resultItemInformation.amount = recipe.getOutputAmount();
        resultItemInformation.SetUIElement();

        int ingredientNumber = recipe.getIngredientsTypeAmount();
        for (int i = 0; i < ingredientNumber; i++)
        {
            GameObject slot = Instantiate(IngredientSlotPrefab, GridlayoutGrounp);
            ReturnIconItemInformation itemInformation = slot.GetComponent<ReturnIconItemInformation>();
            int itemID = recipe.getIngredientID(i);
            ItemData itemDisplayed = ItemDatabase.Instance.GetItemById(itemID);
            itemInformation.DisplayItem = itemDisplayed;
            itemInformation.amount = recipe.getIngredientAmount(i);
            itemInformation.SetUIElement();
        }
    }
    
}
