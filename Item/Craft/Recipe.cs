using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Crafting/Recipe")]
public class Recipe : ScriptableObject
{
    [Header("Input ItemID and Amount")]
    public int[] inputItemsID;
    public int[] inputAmounts;

    [Header("Output ItemID and Amount")]
    public int outputItemID;
    public int outputAmount;

    public int getIngredientID(int index)
    {
        return inputItemsID[index];
    }

    public int getIngredientAmount(int index)
    {
        return inputAmounts[index];
    }

    public int getOutputID()
    {
        return outputItemID;
    }

    public int getOutputAmount()
    {
        return outputAmount;
    }

    public int getIngredientsTypeAmount()
    {
        return inputItemsID.Length;
    }
}



