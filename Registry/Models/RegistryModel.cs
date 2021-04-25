using APIClasses.Registry;
using APIClasses.Security;
using Newtonsoft.Json;
using ServerInterfaceLib;
using ServiceProvider.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using APIClasses;

namespace Registry.Models
{
    public class RegistryModel
    {
        //Singleton management
        private static string _dataModelPath = "C:/dc-assignment-1/data.txt";
        private static string authUrl = NetworkPaths.AuthenticatorUrl;

        public static RegistryModel Instance
        {
            get;
        } = new RegistryModel(_dataModelPath);


        private readonly string _dataPath;

        private IAuthenticationServer _authServer;

        public RegistryModel(string dataPath)
        {
            _dataPath = dataPath;

            //Create connection factory for connection to auth server
            NetTcpBinding tcp = new NetTcpBinding();
            ChannelFactory<IAuthenticationServer> serverChannelFactory = new ChannelFactory<IAuthenticationServer>(tcp, authUrl);
            _authServer = serverChannelFactory.CreateChannel();

            //Create file at path in case it doesn't exist
            using (File.AppendText(dataPath)) ;
        }

        public List<ServiceData> Search(string query)
        {
            //Get JSON data
            JsonSerializer serializer = new JsonSerializer();

            List<ServiceData> registry = OpenRegistry();

            //Query for elements containing search query
            return registry.Where(data => data.Description.Contains(query)).ToList();
        }

        /// <summary>
        /// Retrieves a list of all registry services
        /// </summary>
        /// <returns></returns> list of all registry 
        public List<ServiceData> All()
        {
            return OpenRegistry();
        }

        public void Publish(ServiceData newData)
        {
            List<ServiceData> registry = OpenRegistry();
            
            //Check service with this or name endpoint does not already exist in the database
            if (registry.Any(data => data.ApiEndpoint.Equals(newData.ApiEndpoint)))
            {
                throw new RegistryException($"API endpoint '{newData.ApiEndpoint}' already exists in registry");
            }

            if (registry.Any(data => data.Name.Equals(newData.Name)))
            {
                throw new RegistryException($"Name '{newData.Name}' already exists in registry");
            }

            //Add to registry
            registry.Add(newData);

            SaveRegistry(registry);
        }
        /// <summary>
        /// Remove an item with the given API endpoint from the registry if it exists.
        /// </summary>
        /// <param name="unpublishRequest"></param> API endpoint of element to remove
        /// <returns></returns> Whether the element was found & removed
        public bool Unpublish(UnpublishRequest unpublishRequest)
        {
            List<ServiceData> registry = OpenRegistry();

            //Remove element from registry if it exists
            bool found = false;
            int ii = 0;
            foreach (ServiceData data in registry)
            {
                if (data.ApiEndpoint.Equals(unpublishRequest.ApiEndpoint))
                {
                    registry.RemoveAt(ii);
                    found = true;
                    break;
                }

                ii++;
            }

            //Save the updated registry
            SaveRegistry(registry);
            
            return found;
        }

        /// <summary>
        /// Tests authentication on the authentication server.
        /// </summary>
        /// <param name="token">Authentication token to test</param>
        /// <returns></returns>
        public bool TestAuthentication(int token)
        {
            string validationResult= _authServer.Validate(token);

            bool result;

            try
            {
                result = validationResult.Equals("validated");
            }
            catch (CommunicationException)
            {
                throw new AuthenticationException("Could not connect to authentication server");

            }


            return result;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private List<ServiceData> OpenRegistry()
        {
            List<ServiceData> registry;

            try
            {
                registry = JsonConvert.DeserializeObject<List<ServiceData>>(File.ReadAllText(_dataPath)); //TODO validation here
            }
            catch (Exception e)
            {
                if (e is IOException || e is JsonException)
                {
                    //Try to reset the registry file (if this fails server crash is appropriate)
                    Console.WriteLine("Failed to open registry, resetting file...");
                    using (File.Create(_dataPath)) ;

                    //Make registry empty list (since has been reset)
                    registry = new List<ServiceData>();
                }
                else
                {
                    throw;
                }
            }

            if (registry == null)
            {
                registry = new List<ServiceData>();
            }

            return registry;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void SaveRegistry(List<ServiceData> registry)
        {
            //Convert registry to JSON
            string registryJson = JsonConvert.SerializeObject(registry);

            //Overwrite entire registry file with new registry details
            File.WriteAllText(_dataPath, registryJson);
        }
    }
}