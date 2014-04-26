/********************************************************************************************
 *      Author :    Chase Cobb
 *      
 *      File :      DebugLogger.cs
 *      
 *      Purpose :   To wrap the Debug.Log functionality of Unit and add support for different
 *                  levels of information. This allows levels of debugging to be toggled to 
 *                  prevent spamming of the console. The defines for the different levels of 
 *                  debug information can be found in the 'smcs.rsp' file in the root of the 
 *                  Assets folder. After adding/removing defines from the smcs.rsp file you
 *                  must save the scene for the changes to take effect.
 * 
 *      Note :      Feel free to edit this functionality to suit your needs and contact me with 
 *                  any questions. (chase@8bitroots.com/chase@paracosm.io)
 *********/

#define DEBUG_LEVEL_INFO
#define DEBUG_LEVEL_WARN
#define DEBUG_LEVEL_ERROR
#define DEBUG_LEVEL_CRITICAL

using UnityEngine;
using System.Collections;
using System;
 
public static class DebugLogger
{
    private static string WARN_MESSAGE =            "Warning : ";
    private static string ERROR_MESSAGE =           "Error : ";
    private static string CRITICAL_MESSAGE =        "Critical : ";
    private static string INFO_MESSAGE =            "Information : ";
     
    //Each enumerated value should be a power of two
    [Flags] //Flags keyword doesn't seem to work correctly in Unity :(
    public enum EDebugLevel
    {
        DEBUG_WARN = 0x1,
        DEBUG_ERROR = 0x2,
        DEBUG_CRITICAL = 0x4,
        DEBUG_INFO = 0x8
    }
 
    /// <summary>
    /// Function responsible for writing to the debug log
    /// </summary>
    /// <param name="eLevel">Debug level of this message, from enum EDebugLevel
    /// <param name="cMessage">The message to write
    public static void WriteToLog(EDebugLevel eLevel, object cMessage)
    {
#if DEBUG_LEVEL_WARN
        if ((eLevel & EDebugLevel.DEBUG_WARN) != 0)
        {
            Debug.Log(WARN_MESSAGE + cMessage);
        }
#endif //DEBUG_LEVEL_WARN
        
#if DEBUG_LEVEL_ERROR
        if ((eLevel & EDebugLevel.DEBUG_ERROR) != 0)
        {
            Debug.Log(ERROR_MESSAGE + cMessage);
        }
#endif //DEBUG_LEVEL_ERROR
        
#if DEBUG_LEVEL_CRITICAL
        if ((eLevel & EDebugLevel.DEBUG_CRITICAL) != 0)
        {
            Debug.Log(CRITICAL_MESSAGE + cMessage);
        }
#endif //DEBUG_LEVEL_CRITICAL
        
#if DEBUG_LEVEL_INFO
        if ((eLevel & EDebugLevel.DEBUG_INFO) != 0)
        {
            Debug.Log(INFO_MESSAGE + cMessage);
        }
#endif //DEBUG_LEVEL_INFO
    }
}
