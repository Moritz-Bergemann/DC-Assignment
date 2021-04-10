using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using APIClasses.Registry;

namespace Registry.Models
{
    public class RegistryModel
    {
        //TODO make fileIO stuff atomic

        //Singleton management
        private static string _dataModelPath = "./data.txt";
        public static RegistryModel Instance
        {
            get;
        } = new RegistryModel(_dataModelPath);


        private string _dataPath;

        public RegistryModel(string dataPath)
        {
            _dataPath = dataPath;
        }

        public List<RegistryData> Search(string query)
        {

        }

        /// <summary>
        /// Retrieves a list of all registry services
        /// </summary>
        /// <returns></returns> list of all registry 
        public List<RegistryData> All()
        {

        }
    }
}