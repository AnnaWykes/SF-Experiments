using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using ProductsActor.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Actors.Query;
using System.Threading;
using System.Fabric;
using Newtonsoft.Json.Serialization;

namespace MyWebSite.Controllers
{
    [Produces("application/json")]
    [Route("api/ProductsApi")]
    public class ProductsApiController : Controller
    {
        private readonly FabricClient fabricClient;
        private readonly ConfigSettings configSettings;
        private readonly StatelessServiceContext serviceContext;
        private static ActorId ThisActorId;

        public ProductsApiController(StatelessServiceContext serviceContext, ConfigSettings settings, FabricClient fabricClient)
        {
            this.serviceContext = serviceContext;
            this.configSettings = settings;
            this.fabricClient = fabricClient;
            ThisActorId = ActorId.CreateRandom();
            // return View();
        }


        // POST api/actorbackendservice
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            ThisActorId = new ActorId("{6508761687969923836}");
            string serviceUri = this.serviceContext.CodePackageActivationContext.ApplicationName + "/" + "ProductsActorService";

            IProductsActor proxy = ActorProxy.Create<IProductsActor>(ThisActorId, new Uri(serviceUri));

            var result = proxy.GetAll(CancellationToken.None);
            // result.json

            var serializer = new JsonResult(result.Result);
            // string outputOfInts = serializer.Serialize(sequenceOfInts);

            //proxy.

            return serializer;

        }
    }
}