using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteractController : MonoBehaviour
{
    CharacterController2D characterController;
    Rigidbody2D rgbd2d;
    [SerializeField] float offsetDistance = 1f; //distancia maxima do player para interagir
    [SerializeField] float sizeOfInteractableArea = 1.2f; //tamanho da area de intera��o do player
    Character character;
    [SerializeReference] HighlightController highlightController;

    // Verificar se est� em conversa com NPC
    public DialogueSystem dialogue;

    // LineRenderers para visualiza��o
    LineRenderer interactableAreaRenderer;

    private void Awake()
    {
        characterController = GetComponent<CharacterController2D>();
        rgbd2d = GetComponent<Rigidbody2D>();
        character = GetComponent<Character>();

        // Inicializar o LineRenderer
        interactableAreaRenderer = CreateLineRenderer(Color.blue);
    }

    private void Update()
    {
        Check();
        UpdateRangeVisuals(); // Atualiza a visualiza��o do c�rculo

        if (Input.GetMouseButtonDown(1) && dialogue.inConversation == false) //se eu apertar botao direito, interagirei com objeto
        {
            Interact();
        }
    }

    private void Check()
    {
        Vector2 position = rgbd2d.position + characterController.lastMotionVector * offsetDistance;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, sizeOfInteractableArea);

        foreach (Collider2D c in colliders)
        {
            Interactable hit = c.GetComponent<Interactable>();
            if (hit != null)
            {
                highlightController.Highlight(hit.gameObject);
                return;
            }
        }

        highlightController.Hide();
    }

    private void Interact()
    {
        //verificar se o personagem est� perto e est� virado para o lado correto
        Vector2 position = rgbd2d.position + characterController.lastMotionVector * offsetDistance;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, sizeOfInteractableArea);

        foreach (Collider2D c in colliders)
        {
            Interactable hit = c.GetComponent<Interactable>();
            if (hit != null)
            {
                hit.Interact(character);
                break;
            }
        }
    }

    // M�todo para criar e configurar um LineRenderer
    private LineRenderer CreateLineRenderer(Color color)
    {
        GameObject lineObj = new GameObject("LineRenderer");
        lineObj.transform.parent = transform;

        // Ajustar a posi��o para (0, 0, 0.1)
        lineObj.transform.localPosition = new Vector3(0, 0, 0.1f);

        LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();

        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = 0; // Inicialmente n�o desenha nada
        lineRenderer.useWorldSpace = false; // Para que o LineRenderer use o espa�o local

        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.material.color = color;

        return lineRenderer;
    }



    // M�todo para atualizar a visualiza��o do range
    private void UpdateRangeVisuals()
    {
        Vector2 interactPosition = characterController.lastMotionVector * offsetDistance;
        DrawCircle(interactableAreaRenderer, interactPosition, sizeOfInteractableArea);
    }

    // M�todo para desenhar um c�rculo usando LineRenderer
    private void DrawCircle(LineRenderer lineRenderer, Vector3 offset, float radius)
    {
        int segments = 50;
        lineRenderer.positionCount = segments + 1;
        float angle = 0f;
        for (int i = 0; i <= segments; i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            float y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            float z = 0.1f; // Ajuste no eixo Z
            lineRenderer.SetPosition(i, new Vector3(x, y, z) + offset);
            angle += (360f / segments);
        }
    }

}