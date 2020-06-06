﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BrickColorList : MonoBehaviour {

    [SerializeField]
    private Color color0 = Color.white; // Red
    [SerializeField]
    private Color color1 = Color.white; // Green
    [SerializeField]
    private Color color2 = Color.white; // Blue
    [SerializeField]
    private Color color3 = Color.white; // White
    [SerializeField]
    private Color color4 = Color.white; // Black
    [SerializeField]
    private Color color5 = Color.white; // Yellow
    [SerializeField]
    private Color color6 = Color.white; // Orange
    [SerializeField]
    private Color color7 = Color.white; // Cyan/Light blue

    [SerializeField]
    private Color color8 = Color.white; // Light gray
    [SerializeField]
    private Color color9 = Color.white; // Dark gray
    [SerializeField]
    private Color color10 = Color.white; // Lime
    [SerializeField]
    private Color color12 = Color.white; // Brown
    [SerializeField]
    private Color color13 = Color.white; // Tan
    [SerializeField]
    private Color color15 = Color.white; // Dark green

    private List<Color> allColors;

    private void Awake() {
        this.allColors = new List<Color>(8);
        this.allColors.Add(this.color0);
        this.allColors.Add(this.color1);
        this.allColors.Add(this.color2);
        this.allColors.Add(this.color3);
        this.allColors.Add(this.color4);
        this.allColors.Add(this.color5);
        this.allColors.Add(this.color6);
        this.allColors.Add(this.color7);
        this.allColors.Add(this.color8);
        this.allColors.Add(this.color9);
        this.allColors.Add(this.color10);
        this.allColors.Add(this.color12);
        this.allColors.Add(this.color13);
        this.allColors.Add(this.color15);
    }

    public Color colorFromToken(TokenColor token) {
        return this.allColors[(int)token];
    }

    public TokenColor getTokenFromColor(Color target) {
        float colorDiffs = this.allColors.Select(n => this.ColorDiff(n, target)).Min(n => n);
        return (TokenColor)this.allColors.FindIndex(n => this.ColorDiff(n, target) == colorDiffs);
    }

    // distance in RGB space
    private float ColorDiff(Color c1, Color c2) {
        return Mathf.Sqrt((c1.r - c2.r) * (c1.r - c2.r)
            + (c1.g - c2.g) * (c1.g - c2.g)
            + (c1.b - c2.b) * (c1.b - c2.b));
    }
}