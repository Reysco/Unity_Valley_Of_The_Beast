using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemToolBarPanel : ItemPanel
{
    [SerializeField] ToolBarController toolbarController;

    private void Start()
    {
        Init();
        toolbarController.onChange += Highlight;
        Highlight(0);
    }

    public override void OnClick(int id)
    {
        //Dessa forma, se clicar com o mouse ele irá apenas selecionar o item (que é igual do video)
        //toolbarController.Set(id);
        //Highlight(id);

        //Dessa forma, se clicar com o mouse, ele irá poder trocar os itens de lugar
        GameManager.instance.dragAndDropController.OnClick(inventory.slots[id]);
        Show();
    }

    int currentSelectedTool;

    public void Highlight(int id)
    {
        buttons[currentSelectedTool].Highlight(false);
        currentSelectedTool = id;
        buttons[currentSelectedTool].Highlight(true);
    }

    public override void Show()
    {
        base.Show();
        toolbarController.UpdateHighlightIcon();
    }
}
