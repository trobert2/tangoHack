//-----------------------------------------------------------------------
// <copyright file="FilesInAssets.cs" company="Google">
//
// Copyright 2014 Google. Part of the Tango project. CONFIDENTIAL. AUTHORIZED USE ONLY. DO NOT REDISTRIBUTE.
//
// </copyright>
//-----------------------------------------------------------------------

/// <summary>
/// Any File that needs to be added through streaming assets can be added here.
/// </summary>
public class FilesInAssets
{
    /// <summary>
    /// We load 4 files currently from the assets. (10kBRISK.tree, 10kFREAK.tree,10kBRISK.weights, 10kFREAK.weights).
    /// </summary>
    public static string[] m_fileNames = new string[]
    {
        "10kBRISK.tree",
        "10kFREAK.tree",
        "10kBRISK.weights", 
        "10kFREAK.weights"
    };
}
