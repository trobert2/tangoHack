//-----------------------------------------------------------------------
// <copyright file="WeightsTreeLoader.cs" company="Google">
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
/// Class to copy 10kBRISK , 10kFREAK tree and weight files in sdcard.
/// These files are need to run sparse mapping based apps.
/// </summary>
public class WeightsTreeLoader : MonoBehaviour
{
    private int m_numberOfFilesLoaded;
    private bool[] m_isFileLoaded;

    /// <summary>
    /// Files don't exist, so we copy them
	/// from the assets to the /sdcard/
	/// This is a temporary fix, it will be removed later on.
    /// </summary>
	private void LoadFilesInSDCard()
    {
        for (int i = 0; i < FilesInAssets.m_fileNames.Length; i++)
        {
            StartCoroutine(_CopyFromAssets(i));
        }
	}

    /// <summary>
    /// Coroutine to copy the file contents from assets to sdcard.
    /// </summary>
    /// <param name="fileIndex"> Name of the file to copy.</param>
    /// <returns> Yield until condition is met.</returns>
	private IEnumerator _CopyFromAssets(int fileIndex)
	{
        WWW www = new WWW("jar:file://" + Application.dataPath + "!/assets/" + FilesInAssets.m_fileNames[fileIndex]);
		while (!www.isDone)
		{
			// keep writing data.
			yield return null;
		}

        // If file saving doesn't crash abruptly / is successful
        if (www.error == null)
        {
            File.WriteAllBytes("sdcard/" + FilesInAssets.m_fileNames[fileIndex], www.bytes);
            m_isFileLoaded[fileIndex] = true;
            m_numberOfFilesLoaded++;
        }
	}

    /// <summary>
    /// Initialize in this function.
    /// </summary>
    private void Start()
    {
        m_isFileLoaded = new bool[FilesInAssets.m_fileNames.Length];
        LoadFilesInSDCard();
    }

    /// <summary>
    /// Shows the loading status in terms of percentage.
    /// </summary>
    private void OnGUI()
    {
        GUI.Label(new Rect((Screen.width / 2) - 100, (Screen.height / 2) - 100, 1300, 200),
                  "<size=50> Loading \n" + ((m_numberOfFilesLoaded * 100.0f) / FilesInAssets.m_fileNames.Length).ToString() + " %" + "</size>");
    }

    /// <summary>
    /// Loads the sparse map scene if the files are copied over succesfully.
    /// </summary>
    private void Update()
    {
        for (int i = 0; i < FilesInAssets.m_fileNames.Length; i++)
        {
            if (!m_isFileLoaded[i])
            {
                return;
            }
        }

        // If all files loaded
        Application.LoadLevel("Tango Sparse Mapping");
    }
}
