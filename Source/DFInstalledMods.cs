﻿/**
 * DeepFreeze Continued...
 * (C) Copyright 2015, Jamie Leighton
 *
 * Kerbal Space Program is Copyright (C) 2013 Squad. See http://kerbalspaceprogram.com/. This
 * project is in no way associated with nor endorsed by Squad.
 *
 *  This file is part of JPLRepo's DeepFreeze (continued...) - a Fork of DeepFreeze. Original Author of DeepFreeze is 'scottpaladin' on the KSP Forums.
 *  This File was not part of the original Deepfreeze but was written by Jamie Leighton.
 *  (C) Copyright 2015, Jamie Leighton
 *
 * Continues to be licensed under the Attribution-NonCommercial-ShareAlike 3.0 (CC BY-NC-SA 4.0)
 * creative commons license. See <https://creativecommons.org/licenses/by-nc-sa/4.0/>
 * for full details.
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RSTUtils;

namespace DF
{
    internal class DFInstalledMods
    {
        // Class used to check what Other mods we are interested in are installed.
        
        private static Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        private static List<ModInfo> modInfo;

        internal class ModInfo
        {
            public string name;
            public bool alreadyChecked;
            public bool installed;

            public ModInfo(string name)
            {
                this.name = name;
            }
        }

        internal static bool IsSMInstalled
        {
            get
            {
                ModInfo info = GetInfo("ShipManifest");
                if (info.alreadyChecked)
                {
                    return info.installed;
                }
                else
                {
                    
                }
                return IsModInstalled("ShipManifest");
            }
        }

        internal static bool IsRTInstalled
        {
            get
            {
                return IsModInstalled("RemoteTech");
            }
        }

        internal static bool IsUSILSInstalled
        {
            get
            {
                return IsModInstalled("USILifeSupport");
            }
        }

        internal static bool IsSnacksInstalled
        {
            get
            {
                return IsModInstalled("Snacks");
            }
        }

        internal static bool IsTACLSInstalled
        {
            get
            {
                return IsModInstalled("TacLifeSupport");
            }
        }

        internal static bool IsRPMInstalled
        {
            get
            {
                return IsModInstalled("RasterPropMonitor");
            }
        }

        internal static bool IsJSITransparentPodsInstalled
        {
            get
            {
                return IsModInstalled("JSIAdvTransparentPods");
            }
        }

        internal static bool IsBGPInstalled
        {
            get
            {
                return IsModInstalled("BackgroundProcessing");
            }
        }

        internal static bool IsBGRInstalled
        {
            get
            {
                return IsModInstalled("BackgroundResources");
            }
        }

        internal static bool IsTexReplacerInstalled
        {
            get
            {
                return IsModInstalled("TextureReplacer");
            }
        }

        internal static bool IskerbalismInstalled
        {
            get
            {
                return IsModInstalled("Kerbalism");
            }
        }

        internal static bool IsModInstalled(string assemblyName)
        {
            ModInfo info = GetInfo(assemblyName);
            if (info.alreadyChecked)
            {
                return info.installed;
            }
            
            try
            {
                Assembly assembly = (from a in assemblies
                                     where a.FullName.Split(',')[0] == assemblyName
                                     select a).First();
                info.alreadyChecked = true;
                info.installed = assembly != null;
                return info.installed;
            }
            catch
            {
                return false;
            }
        }
        
        internal static ModInfo GetInfo(string name)
        {
            if (modInfo == null)
            {
                modInfo = new List<ModInfo>();
            }
            for (int i = 0; i < modInfo.Count; i++)
            {
                ModInfo info = modInfo[i];
                if (modInfo[i].name == name)
                {
                    return modInfo[i];
                }
            }
            ModInfo newInfo = new ModInfo(name);
            modInfo.Add(newInfo);
            return newInfo;
        }


        internal static bool RTVesselConnected(Guid id)
        {
            bool RTVslConnected = false;
            try
            {
                if (IsRTInstalled && RTWrapper.APIReady)
                {
                    //RTVslConnected = (RemoteTech.API.API.HasLocalControl(id) || RemoteTech.API.API.HasAnyConnection(id));
                    RTVslConnected = RTWrapper.RTactualAPI.HasLocalControl(id) || RTWrapper.RTactualAPI.HasAnyConnection(id);
                    //Utilities.Log_Debug("vessel " + id + "haslocal " + RemoteTech.API.API.HasLocalControl(id) + " has any " + RemoteTech.API.API.HasAnyConnection(id));
                }
            }
            catch (Exception ex)
            {
                Utilities.Log("DeepFreeze Exception attempting to check RemoteTech connections. Report this error on the Forum Thread.");
                Utilities.Log("DeepFreeze Err: " + ex);
            }
            return RTVslConnected;
        }

        internal static double RTVesselDelay
        {
            get
            {
                double RTVslDelay = 0f;
                try
                {
                    //RTVslDelay = RemoteTech.API.API.GetShortestSignalDelay(FlightGlobals.ActiveVessel.id);
                    RTVslDelay = RTWrapper.RTactualAPI.GetShortestSignalDelay(FlightGlobals.ActiveVessel.id);
                }
                catch (Exception ex)
                {
                    Utilities.Log("DeepFreeze Exception attempting to check RemoteTech VesselDelay. Report this error on the Forum Thread.");
                    Utilities.Log("DeepFreeze Err: " + ex);
                }
                return RTVslDelay;
            }
        }
    }
}