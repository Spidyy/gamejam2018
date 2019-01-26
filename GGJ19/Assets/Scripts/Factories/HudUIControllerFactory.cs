using UnityEngine;

public static class HudUIControllerFactory
{
    private const string k_prefabPath = "Prefabs/UI/HudUI";

    public static HudUIController CreateHudUIController()
    {
        var resource = Resources.Load<GameObject>(k_prefabPath);
        GameObject go = GameObject.Instantiate(resource);

        HudUIController controller = go.GetComponent<HudUIController>();

        Debug.Assert(controller != null, "No Hud");

        return controller;
    }
}