using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Threading;

namespace MainApp
{
    public class Spaces
    {
        public Spaces()
        {
        }

        //default create pool uses all available disks for pool creation

        public bool DefaultCreatePool()
        {
            ManagementClass subSystem = new ManagementClass("root\\Microsoft\\Windows\\Storage", "MSFT_StorageSubSystem", null);

            ManagementClass storagePool = new ManagementClass("root\\Microsoft\\Windows\\Storage", "MSFT_StoragePool", null);

            ManagementBaseObject inParams = null;
            ManagementObject instance = null;

            foreach (ManagementObject subsystem in subSystem.GetInstances())
            {
                instance = subsystem;
                inParams = subsystem.GetMethodParameters("CreateStoragePool");
            }
            inParams.SetPropertyValue("PhysicalDisks", GetDisks().ToArray());

            inParams.SetPropertyValue("FriendlyName", "default_name");

            try
            {
                ManagementBaseObject outParams = instance.InvokeMethod("CreateStoragePool", inParams, null);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        //creates storage pool 
        public bool CreatePool(ArrayList disks, string poolName)
        {
            ManagementClass subSystem = new ManagementClass("root\\Microsoft\\Windows\\Storage", "MSFT_StorageSubSystem", null);

            ManagementClass storagePool = new ManagementClass("root\\Microsoft\\Windows\\Storage", "MSFT_StoragePool", null);

            ManagementBaseObject inParams = null;
            ManagementObject instance = null;

            foreach (ManagementObject subsystem in subSystem.GetInstances())
            {
                instance = subsystem;
                inParams = subsystem.GetMethodParameters("CreateStoragePool");
            }

            //UInt32 CreateStoragePool(
            //  [in]   String FriendlyName,
            //  [in]   UInt16 Usage,
            //  [in]   String OtherUsageDescription,
            //  [in]   String PhysicalDisks[],
            //  [in]   String ResiliencySettingNameDefault,
            //  [in]   UInt16 ProvisioningTypeDefault,
            //  [in]   UInt64 LogicalSectorSizeDefault,
            //  [in]   Boolean EnclosureAwareDefault,
            //  [in]   Boolean RunAsJob,
            //  [out]  String CreatedStoragePool,
            //  [out]  MSFT_StorageJob REF CreatedStorageJob,
            //  [out]  String ExtendedStatus
            //);

            inParams.SetPropertyValue("PhysicalDisks", disks.ToArray());

            inParams.SetPropertyValue("FriendlyName", poolName);

            try
            {
                ManagementBaseObject outParams = instance.InvokeMethod("CreateStoragePool", inParams, null);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }



        public ArrayList GetDisks()
        {
            ManagementClass disk = new ManagementClass("root\\Microsoft\\Windows\\Storage", "MSFT_PhysicalDisk", null);

            ArrayList diskList = new ArrayList();

            foreach (ManagementObject objectId in disk.GetInstances())
            {
                if ((Boolean)objectId.GetPropertyValue("CanPool") == true)
                {
                    diskList.Add(objectId);
                }
            }

            return diskList;
        }

        public ArrayList GetDisksFromPool()
        {
            ManagementClass disk = new ManagementClass("root\\Microsoft\\Windows\\Storage", "MSFT_PhysicalDisk", null);

            ArrayList diskList = new ArrayList();
            
            foreach (ManagementObject objectId in disk.GetInstances())
            {
                
                if ((bool)objectId.GetPropertyValue("CanPool") == false)
                {
                    diskList.Add(objectId);
                }
            }

            return diskList;
        }


        public bool AddPhysicalDisk(ArrayList disks, string poolName)
        {
            ManagementClass storagePool = new ManagementClass("root\\Microsoft\\Windows\\Storage", "MSFT_StoragePool", null);

            ManagementBaseObject inParams = null;
            ManagementObject StoragePool = null;
            foreach (ManagementObject pool in storagePool.GetInstances())
            {
                if ((String)pool.GetPropertyValue("FriendlyName") == poolName)
                {
                    inParams = pool.GetMethodParameters("AddPhysicalDisk");
                    StoragePool = pool;
                }
            }

            inParams.SetPropertyValue("PhysicalDisks", GetDisks().ToArray());

            inParams.SetPropertyValue("RunAsJob", false);

            inParams.SetPropertyValue("Usage", 1);

            //UInt32 AddPhysicalDisk(
            //[in]   String PhysicalDisks[],
            //[in]   UInt16 Usage,
            //[in]   Boolean RunAsJob,
            //[out]  MSFT_StorageJob REF CreatedStorageJob,
            //[out]  String ExtendedStatus
            //);

            try
            {
                ManagementBaseObject outParams = StoragePool.InvokeMethod("AddPhysicalDisk", inParams, null);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }


        public bool CreateVirtualDisk(string poolName, string diskName, string resilienceType, string driveLetter)
        {
            ManagementClass storagePool = new ManagementClass("root\\Microsoft\\Windows\\Storage", "MSFT_StoragePool", null);
            ManagementClass disk = new ManagementClass("root\\Microsoft\\Windows\\Storage", "MSFT_Disk", null);
            
            ManagementBaseObject inParams = null;
            ManagementBaseObject inParamsDisk = null;
            ManagementObject StoragePool = null;
            ManagementObject Disk = null;
            
            foreach (ManagementObject pool in storagePool.GetInstances())
            {
                if ((String)pool.GetPropertyValue("FriendlyName") == poolName)
                {
                    inParams = pool.GetMethodParameters("CreateVirtualDisk");
                    StoragePool = pool;
                }
                
            }

            if (StoragePool == null) 
            {
                    return false;
            }
            //UInt32 CreateVirtualDisk(
            //  [in]   String FriendlyName,
            //  [in]   UInt64 Size,
            //  [in]   Boolean UseMaximumSize,
            //  [in]   UInt16 ProvisioningType,
            //  [in]   String ResiliencySettingName,
            //  [in]   UInt16 Usage,
            //  [in]   String OtherUsageDescription,
            //  [in]   UInt16 NumberOfDataCopies,
            //  [in]   UInt16 PhysicalDiskRedundancy,
            //  [in]   UInt16 NumberOfColumns,
            //  [in]   Boolean AutoNumberOfColumns,
            //  [in]   UInt64 Interleave,
            //  [in]   Boolean IsEnclosureAware,
            //  [in]   String PhysicalDisksToUse[],
            //  [in]   Boolean RunAsJob,
            //  [out]  String CreatedVirtualDisk,
            //  [out]  MSFT_StorageJob REF CreatedStorageJob,
            //  [out]  String ExtendedStatus
            //);

            inParams.SetPropertyValue("UseMaximumSize", true);
            inParams.SetPropertyValue("FriendlyName", diskName);
            inParams.SetPropertyValue("ResiliencySettingName", resilienceType);
            inParams.SetPropertyValue("Usage", 2);
            

            try
            {
                ManagementBaseObject outParams = StoragePool.InvokeMethod("CreateVirtualDisk", inParams, null);
            }
            catch (Exception)
            {
                return false;
            }

            foreach (ManagementObject item in disk.GetInstances())
            {
                var cos = item.GetPropertyValue("FriendlyName");
                if ((String)item.GetPropertyValue("FriendlyName") == "Microsoft Storage Space Device")
                {
                    Disk = item;
                    inParamsDisk = Disk.GetMethodParameters("CreatePartition");
                }
            }

            Thread.Sleep(10000);

            try
            {
                ManagementBaseObject outParams = Disk.InvokeMethod("Initialize", null, null);
            }
            catch { }

            //UInt32 CreatePartition(
            //  [in]   UInt64 Size,
            //  [in]   Boolean UseMaximumSize,
            //  [in]   UInt64 Offset,
            //  [in]   UInt32 Alignment,
            //  [in]   Char16 DriveLetter,
            //  [in]   Boolean AssignDriveLetter,
            //  [in]   UInt16 MbrType,
            //  [in]   String GptType,
            //  [in]   Boolean IsHidden,
            //  [in]   Boolean IsActive,
            //  [out]  String CreatedPartition,
            //  [out]  String ExtendedStatus
            //);
            
            inParamsDisk.SetPropertyValue("UseMaximumSize", true);
            inParamsDisk.SetPropertyValue("DriveLetter", driveLetter);
            

            try
            {
                ManagementBaseObject outParams2 = Disk.InvokeMethod("CreatePartition", inParamsDisk, null);
            }
            catch { }

            //ManagementClass volume = new ManagementClass("root\\cimv2", "Win32_Volume", null);

            //ManagementObject Volume = null;
            //ManagementBaseObject inParamsVol = null;

            //foreach (ManagementObject item in volume.GetInstances())
            //{
            //    if ((String)item.GetPropertyValue("DriveLetter") == driveLetter)
            //    {
            //        Volume = item;
            //        inParamsVol = Volume.GetMethodParameters("Format");

            //    }
            //}

            //inParamsVol.SetPropertyValue("FileSystem", "NTFS");
            //inParamsVol.SetPropertyValue("QuickFormat", true);
            //inParamsVol.SetPropertyValue("ClusterSize", 4096);
            //inParamsVol.SetPropertyValue("Label", diskName);

            //try
            //{
            //    ManagementBaseObject outParam = Volume.InvokeMethod("Format", inParamsVol, null);
            //}
            //catch
            //{

            //}

            return true;
        }


        public bool RemovePhysicalDisk(string poolName, ArrayList disksToRemove)
        {
            ManagementClass storagePool = new ManagementClass("root\\Microsoft\\Windows\\Storage", "MSFT_StoragePool", null);

            ManagementBaseObject inParams = null;
            ManagementObject StoragePool = null;

            foreach (ManagementObject pool in storagePool.GetInstances())
            {
                if ((String)pool.GetPropertyValue("FriendlyName") == poolName)
                {
                    inParams = pool.GetMethodParameters("CreateVirtualDisk");
                    StoragePool = pool;
                }
            }

            //UInt32 RemovePhysicalDisk(
            //  [in]   String PhysicalDisks[],
            //  [in]   Boolean RunAsJob,
            //  [out]  MSFT_StorageJob REF CreatedStorageJob,
            //  [out]  String ExtendedStatus
            //);

            inParams.SetPropertyValue("PhysicalDisks", disksToRemove);

            try
            {
                ManagementBaseObject outParams = StoragePool.InvokeMethod("RemovePhysicalDisk", inParams, null);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        public bool DeleteLogicalDisk(string diskName)
        {
            ManagementClass VirtualDisk = new ManagementClass("root\\Microsoft\\Windows\\Storage", "MSFT_VirtualDisk", null);

            ManagementObject Disk = null;

            foreach (ManagementObject disk in VirtualDisk.GetInstances())
            {
                if ((String)disk.GetPropertyValue("FriendlyName") == diskName)
                {
                    Disk = disk;
                }
            }
            //UInt32 DeleteObject(
            //  [in]   Boolean RunAsJob,
            //  [out]  MSFT_StorageJob REF CreatedStorageJob,
            //  [out]  String ExtendedStatus
            //);
            if (Disk != null)
            {
                try
                {
                    ManagementBaseObject outParam = Disk.InvokeMethod("DeleteObject", null, null);
                }
                catch { }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteStoragePool(string poolName)
        {
            ManagementClass storagePool = new ManagementClass("root\\Microsoft\\Windows\\Storage", "MSFT_StoragePool", null);

            ManagementClass VirtualDisk = new ManagementClass("root\\Microsoft\\Windows\\Storage", "MSFT_VirtualDisk", null);

            ManagementObject StoragePool = null;

            foreach (ManagementObject pool in storagePool.GetInstances())
            {
                if ((String)pool.GetPropertyValue("FriendlyName") == poolName)
                {
                    StoragePool = pool;
                }
            }

            ManagementObject Disk = null;

            foreach (ManagementObject disk in VirtualDisk.GetInstances())
            {
                if ((String)disk.GetPropertyValue("FriendlyName") != "")
                {
                    Disk = disk;
                }
            }
            //UInt32 DeleteObject(
            //  [in]   Boolean RunAsJob,
            //  [out]  MSFT_StorageJob REF CreatedStorageJob,
            //  [out]  String ExtendedStatus
            //);
            if (Disk != null)
            {
                try
                {
                    ManagementBaseObject outParam = Disk.InvokeMethod("DeleteObject", null, null);
                }
                catch { }
            }

            if (StoragePool == null)
            {
                return false;
            }
            try
            {
                ManagementBaseObject outParams = StoragePool.InvokeMethod("DeleteObject", null, null);
            }
            catch 
            {}
            
            return true;
        }

        public List<string> GetListOfAvailablePools()
        {
            
            ManagementClass storagePool = new ManagementClass("root\\Microsoft\\Windows\\Storage", "MSFT_StoragePool", null);

            List<string> PoolsList = new List<string>();

            foreach (ManagementObject pool in storagePool.GetInstances())
            {
                if ((String)pool.GetPropertyValue("FriendlyName") != "Primordial")
                {
                    PoolsList.Add((String)pool.GetPropertyValue("FriendlyName"));
                }
            }

            return PoolsList;
        }

        internal List<string> GetListOfLogicalDisks()
        {
            ManagementClass virtualDisk = new ManagementClass("root\\Microsoft\\Windows\\Storage", "MSFT_VirtualDisk", null);

            List<string> DisksList = new List<string>();

            foreach (ManagementObject disk in virtualDisk.GetInstances())
            {
                if ((String)disk.GetPropertyValue("FriendlyName") != "Primordial")
                {
                    DisksList.Add((String)disk.GetPropertyValue("FriendlyName"));
                }
            }

            return DisksList;
        }

        internal List<char> GetListOfAvailableLetters()
        {
            ManagementClass partition = new ManagementClass("root\\Microsoft\\Windows\\Storage", "MSFT_Volume", null);

            List<char> Letters = new List<char>() { 'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z' };

            foreach (ManagementObject item in partition.GetInstances())
            {
                char letter = (char)item.GetPropertyValue("DriveLetter");
                if (Letters.Contains(letter))
                {
                    Letters.RemoveAt(Letters.IndexOf((char)item.GetPropertyValue("DriveLetter")));
                    //Letters.Add((String)item.GetPropertyValue("FriendlyName"));
                }
            }

            return Letters;
        }
    }
}
