using GameOfLife;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Web.Hosting;
using System.Security.AccessControl;
using MongoDB.Driver;
using MongoDB.Bson;
using Newtonsoft.Json;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;


namespace GameOfLifeApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]   
    public class GameOfLifeController : ApiController
    {
          
        [HttpPost]
        public NextEvolution NextRound(NextEvolution request)
        {
            return
                new DTONextEvolution(
                    new DefaultEvolution(
                        request.currentRound,
                        request.livingCells.Select(cell => new Coordonnate(cell.coordX, cell.coordY))
                    )
                    .EvolutionnateGame()
                ).Reponse();        
        }


        [HttpPost]
        public NextEvolution[] HistorizeGames(NextEvolution request)
        {
            return
                new DTOArrayNextEvolution(
                    new EvolutionUntil(
                        request.lastRound,
                        new DefaultEvolution(request.livingCells.Select(cell => new Coordonnate(cell.coordX, cell.coordY)))
                    )
                    .EvolutionnateGame()
                    .History()
                ).Reponse();           
        }

        public interface IDTOFactory<T>
        {
            T Reponse();
        }

        public class DTOArrayNextEvolution : IDTOFactory<NextEvolution[]>
        {
            public DTOArrayNextEvolution(IEnumerable<IEvolutionGame> games)
            {
                this.games = games.Select(game => new DTONextEvolution(game));
            }

            private IEnumerable<DTONextEvolution> games;

            public NextEvolution[] Reponse()
            {
                return games.Select(game => game.Reponse()).ToArray();                    
            }
        }

        public class DTONextEvolution : IDTOFactory<NextEvolution>
        {
            public DTONextEvolution(IEvolutionGame game)
            {
                this.game = game;
            }

            private IEvolutionGame game;

            public NextEvolution Reponse()
            {
                return 
                    new NextEvolution()
                    {
                        livingCells = game.LivingCells().Select(coord => new DTOCoordonnate() { coordX = coord.CoordX(), coordY = coord.CoordY() }).ToArray(),
                        currentRound = game.CurrentEvolution()
                    };
            }
        }


        public class DTOArrayRelativeCoordonnate : IDTOFactory<DTORelativeCoordonnate[]>
        {
            public DTOArrayRelativeCoordonnate(IEnumerable<RelativeCoordonnate> relative)
            {
                this.relative = relative;
            }

            private IEnumerable<RelativeCoordonnate> relative;
  
            public DTORelativeCoordonnate[] Reponse()
            {
                return this.relative.Select(relativeCoord => new DTORelativeCoordonnate() { coordX = relativeCoord.CoordXCalculation(), coordY = relativeCoord.CoordYCalculation() }).ToArray();
            }
        }

        [HttpPost]
        public string ReadFigure()
        {
            var files = HttpContext.Current.Request.Files;

            var content = new StreamReader(files[0].InputStream).ReadToEnd();

            return content;
        }

        [HttpPost]
        public DTOFigureTemplate SaveFigureAsTemplate(DTOFigureTemplateRequest templateRequest)
        {          
            var dtos =
                new DTOArrayRelativeCoordonnate(
                    new InRelativeCoordonnates(
                        new DefaultCoordonnates(
                            templateRequest.coordsFigure.Select(cell => new Coordonnate(cell.coordX, cell.coordY))
                        )
                    )
                    .RelativeCoord()
                ).Reponse();


            var client = new MongoDB.Driver.MongoClient("mongodb://yoann:Monaco58898@gameoflife-shard-00-00-iohzq.mongodb.net:27017,gameoflife-shard-00-01-iohzq.mongodb.net:27017,gameoflife-shard-00-02-iohzq.mongodb.net:27017/admin?replicaSet=GameOfLife-shard-0&ssl=true");

            var database = client.GetDatabase("gameOfLife");
            var collection = database.GetCollection<DTOFigureTemplate>("Figures");

            collection.InsertOne(new DTOFigureTemplate() { figureName = templateRequest.figureName, template = dtos });

            return new DTOFigureTemplate() { figureName = templateRequest.figureName, template = dtos };
        }

        [HttpGet]
        public DTOFigureTemplate[] AllFigures()
        {
            var client = new MongoDB.Driver.MongoClient("mongodb://yoann:Monaco58898@gameoflife-shard-00-00-iohzq.mongodb.net:27017,gameoflife-shard-00-01-iohzq.mongodb.net:27017,gameoflife-shard-00-02-iohzq.mongodb.net:27017/admin?replicaSet=GameOfLife-shard-0&ssl=true");

            var database = client.GetDatabase("gameOfLife");
            var collection = database.GetCollection<BsonDocument>("Figures");

            var result = collection.Find(Builders<BsonDocument>.Filter.Empty).ToList();

            return result.Select(doc => BsonSerializer.Deserialize<DTOFigureTemplate>(doc)).ToArray();
        }


    }

  
    public class DTOCoordonnate
    {
        public int coordX { get; set; }
        public int coordY { get; set; }
    }

    public class DTORelativeCoordonnate
    {
        public string coordX { get; set; }
        public string coordY { get; set; }
    }

    public class DTOFigureTemplateRequest
    {
        public string figureName { get; set; }
        public DTOCoordonnate[] coordsFigure { get; set; }

    }

 
    public class DTOFigureTemplate
    {
        [BsonId]
        public ObjectId ID { get; set; }
        public string figureName { get; set; }
        public DTORelativeCoordonnate[] template { get; set; }
    }

    public class RealCoordsRequest
    {
        public string id { get; set; }
        public DTOCoordonnate baseCoord { get; set; }
    }


    public class NextEvolution
    {
        public int currentRound { get; set; }
        public int lastRound { get; set; }
        public DTOCoordonnate[] livingCells { get; set; }
    }






}
