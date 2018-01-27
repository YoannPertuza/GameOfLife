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


namespace GameOfLifeApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]   
    public class GameOfLifeController : ApiController
    {
          
        [HttpPost]
        public NextRound NextRound(NextRound request)
        {
            var nextRoundGame =
                new Game(
                    request.round,
                    request.livingCells.Select(cell => new Coordonnate(cell.coordX, cell.coordY))
                )
                .NextRound();

            return
                new NextRound()
                {
                    livingCells = nextRoundGame.LivingCells().Select(coord => new LivingCell() { coordX = coord.CoordX(), coordY = coord.CoordY()}).ToArray(),
                    round = nextRoundGame.CurrentRound()
                };
        }

        [HttpPost]
        public async Task<string> UploadFigure()
        {
            var uploadPath = HostingEnvironment.MapPath("/") + @"/Uploads";
            Directory.CreateDirectory(uploadPath);
            var provider = new MultipartFormDataStreamProvider(uploadPath);
            await Request.Content.ReadAsMultipartAsync(provider);

            var file = provider.FileData[0];

            return File.ReadAllText(file.LocalFileName);          
        }      
    }

    public class LivingCell
    {
        public int coordX { get; set; }
        public int coordY { get; set; }
    }

    public class NextRound
    {
        public int round { get; set; }
        public LivingCell[] livingCells { get; set; }
    }
}
