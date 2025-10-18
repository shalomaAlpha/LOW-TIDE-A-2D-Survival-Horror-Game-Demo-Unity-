using UnityEngine;


[CreateAssetMenu(fileName = "NewRecipeBook", menuName = "Crafting/RecipeBook")]
public class RecipeBook : ScriptableObject
{
    [Header("All Recipes in Game")]
    public Recipe[] recipes;
}

