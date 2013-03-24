using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Management;

//CreateStoragePool

namespace Spaces
{
    public class StoragePool
    {

        public StoragePool()
        { }

        public void Create()
        {
            ManagementClass subSystem = new ManagementClass("root\\Microsoft\\Windows\\Storage", "MSFT_StorageSubSystem", null);

            ManagementClass storagePool = new ManagementClass("root\\Microsoft\\Windows\\Storage", "MSFT_StoragePool", null);

            ManagementClass disk = new ManagementClass("root\\Microsoft\\Windows\\Storage", "MSFT_PhysicalDisk", null);


            ManagementBaseObject inParams = null;
            ManagementObject instance = null;

            foreach (ManagementObject subsystem in subSystem.GetInstances())
            {
                instance = subsystem;
                inParams = subsystem.GetMethodParameters("CreateStoragePool");
                //Console.WriteLine(subsystem.GetPropertyValue("FriendlyName"));
            }


            foreach (PropertyData property in inParams.Properties)
            {
                Console.WriteLine(property.Name + " " + property.Value);
                Console.WriteLine(property.GetType().GetElementType());
            }
            Console.WriteLine("**********************");

            ArrayList list = new ArrayList();

            foreach (ManagementObject objectId in disk.GetInstances())
            {

                if ((Boolean)objectId.GetPropertyValue("CanPool") == true)
                {
                    Console.WriteLine("disk Addad");
                    list.Add(objectId);
                    //Console.WriteLine("*******************");
                }

            }
                //inParams2["PhysicalDisks"] = names[0];
            Array a = list.ToArray();
            inParams.SetPropertyValue("PhysicalDisks", a);

            inParams.SetPropertyValue("FriendlyName", "my_storage");

            

            try
            {
                ManagementBaseObject outParams = instance.InvokeMethod("CreateStoragePool", inParams, null);

                foreach (PropertyData prop in outParams.Properties)
                {
                    if (prop.Name == "ReturnValue")
                    {
                        Console.WriteLine(prop.Value);
                    }
                }

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                Console.WriteLine(e.GetHashCode());

            }



            Console.ReadKey(true);
        }
    }
}

