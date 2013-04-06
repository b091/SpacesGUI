using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;

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
                if ((int)objectId.GetPropertyValue("CannotPullReason") == 2)
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


        public bool CreateVirtualDisk(string poolName, string diskName, ArrayList selectedDisks)
        {
            ManagementClass storagePool = new ManagementClass("root\\Microsoft\\Windows\\Storage", "MSFT_StoragePool", null);

            ManagementBaseObject inParams = null;
            ManagementObject StoragePool = null;

            foreach (ManagementObject pool in storagePool.GetInstances())
            {
                if ((String)pool.GetPropertyValue("FriendlyName") == poolName)
                {
                    inParams = pool.GetMethodParameters("CreateVirualDisk");
                    StoragePool = pool;
                }
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

            inParams.SetPropertyValue("FriendlyName", diskName);
            inParams.SetPropertyValue("PhysicalDisksToUse", selectedDisks);

            try
            {
                ManagementBaseObject outParams = StoragePool.InvokeMethod("CreateVirtualDisk", inParams, null);
            }
            catch (Exception)
            {
                return false;
            }

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

        public bool DeleteStoragePool(string poolName)
        {
            ManagementClass storagePool = new ManagementClass("root\\Microsoft\\Windows\\Storage", "MSFT_StoragePool", null);

            ManagementObject StoragePool = null;

            foreach (ManagementObject pool in storagePool.GetInstances())
            {
                if ((String)pool.GetPropertyValue("FriendlyName") == poolName)
                {
                    StoragePool = pool;
                }
            }

            //UInt32 DeleteObject(
            //  [in]   Boolean RunAsJob,
            //  [out]  MSFT_StorageJob REF CreatedStorageJob,
            //  [out]  String ExtendedStatus
            //);
            if (StoragePool == null)
            {
                return false;
            }
            try
            {
                ManagementBaseObject outParams = StoragePool.InvokeMethod("DeleteObject", null, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
            return true;
        }
    }
}
