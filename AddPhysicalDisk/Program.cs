using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Management;

//AddPhysicalDisk

namespace test1
{
    class Program
    {
        static void Main(string[] args)
        {
            ManagementClass subSystem = new ManagementClass("root\\Microsoft\\Windows\\Storage", "MSFT_StorageSubSystem", null);

            ManagementClass storagePool = new ManagementClass("root\\Microsoft\\Windows\\Storage", "MSFT_StoragePool", null);

            ManagementClass disk = new ManagementClass("root\\Microsoft\\Windows\\Storage", "MSFT_PhysicalDisk", null);

            //test vs git

            ManagementBaseObject inParams2 = null;
            ManagementObject StoragePool = null;
            foreach (ManagementObject pool in storagePool.GetInstances())
            {
                
                if ((String)pool.GetPropertyValue("FriendlyName") == "Storage pool")
                {
                    Console.WriteLine(pool.GetPropertyValue("FriendlyName"));
                    inParams2 = pool.GetMethodParameters("AddPhysicalDisk");
                    StoragePool = pool;
                }
            }


            //ManagementBaseObject inParams = subSystem.GetMethodParameters("CreateStoragePool");


            foreach (PropertyData property in inParams2.Properties)
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
                    list.Add(objectId);
                    //Console.WriteLine("*******************");
                }

                
                

            }
            //inParams2["PhysicalDisks"] = names[0];
            Array a = list.ToArray();
            inParams2.SetPropertyValue("PhysicalDisks", a);
            
            inParams2.SetPropertyValue("RunAsJob", false);
            
            inParams2.SetPropertyValue("Usage", 1);

            try
            {
                ManagementBaseObject outParams = StoragePool.InvokeMethod("AddPhysicalDisk", inParams2, null);

                foreach (PropertyData prop in outParams.Properties)
                {
                    Console.WriteLine(prop.Value);
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

