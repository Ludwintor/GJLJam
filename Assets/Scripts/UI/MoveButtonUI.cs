using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GJLJam
{
    public class MoveButtonUI : MonoBehaviour
    {
        public Buttons MoveRepresentation => moveRepresentation;

        [SerializeField]
        private Buttons moveRepresentation;

        private Image spriteButton;
        private TextMeshProUGUI movesText;

        private void Awake()
        {
            spriteButton = GetComponent<Image>();
            movesText = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void UpdateUI(int count, Color color)
        {
            movesText.SetText(count.ToString());
            spriteButton.color = color;
        }
    }
}
