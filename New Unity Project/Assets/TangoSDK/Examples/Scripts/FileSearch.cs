//-----------------------------------------------------------------------
// <copyright file="FileSearch.cs" company="Google">
//
// Copyright 2014 Google. Part of the Tango project. CONFIDENTIAL. AUTHORIZED USE ONLY. DO NOT REDISTRIBUTE.
//
// </copyright>
//-----------------------------------------------------------------------

using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;

/// <summary>
/// Class to check if 10kBRISK , 10kFREAK tree and weight files exist in sdcard.
/// These files are need to run sparse mapping based apps.
/// </summary>
public class FileSearch : MonoBehaviour
{
    /// <summary>
    /// Check status as soon as app starts, before vio handler is initialized.
    /// </summary>
    private void Awake()
    {
        _CheckFiles();
    }
	
    /// <summary>
    /// Checks if files exist, if they don't, then we switch to a loading scene.
    /// This is a temporary fix, it will be removed later on.
    /// </summary>mmary>
    private void _CheckFiles()
    {
        int numberOfFiles = FilesInAssets.m_fileNames.Length;
        string[] filesInSDCard = Directory.GetFiles("sdcard", "*.*")
            .Where(s => s.EndsWith(".tree") || s.EndsWith(".weights")).ToArray();
        if (filesInSDCard.Length < numberOfFiles)
        {
            Application.LoadLevel("DependenciesLoader");
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
