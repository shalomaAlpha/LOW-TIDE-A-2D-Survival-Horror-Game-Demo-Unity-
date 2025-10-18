using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This component control the spawn and display of the crafting recipe
/// </summary>
public class CraftRecipeDisplay : MonoBehaviour
{
    public static CraftRecipeDisplay Instance;
    public int currentIndex=0;
    public RecipeBook recipeBook;
    public Transform parent;
    public GameObject CraftReceptBarPrefab;

    int totalPage;
    int lastPage;
    const int firstPage=0;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        totalPage = Mathf.CeilToInt((float)recipeBook.recipes.Length/4f);
        lastPage = totalPage - 1;
    }

    public void OnPreviousPagePressed()
    {
        if(currentIndex==0)
        {
            currentIndex = lastPage;
        }
        else
        {
            currentIndex--;
        }
        UpdateCraftPanelRecipes();
    }

    public void OnNextPagePressed()
    {
        if(currentIndex==lastPage)
        {
            currentIndex = firstPage;
        }
        else
        {
            currentIndex++;
        }
        UpdateCraftPanelRecipes();
    }

    public void UpdateCraftPanelRecipes()
    {
        ClearCraftPanel();
        int recipeToSpawn=4;
        if (currentIndex==lastPage)
        {
            recipeToSpawn = (recipeBook.recipes.Length) % 4;
        }
        
        for(int i=0;i< recipeToSpawn; i++)
        {
            GameObject recipeBar = Instantiate(CraftReceptBarPrefab, parent);
            CraftRecipeBar craftRecipeBar = recipeBar.GetComponent<CraftRecipeBar>();

            int index = currentIndex + i;
            Recipe recipe = recipeBook.recipes[index];
            craftRecipeBar.Initlize(recipe);
        }
    }

    void ClearCraftPanel()
    {
        foreach(Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
}
