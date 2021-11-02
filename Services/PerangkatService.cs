using Grpc.Core;
using grpcArachne.Data;
using grpcArachne.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace grpcArachne.Services
{
    public class PerangkatService : Perangkat.PerangkatBase
    {
        private readonly ILogger<PerangkatService> _logger;
        private readonly ServerDbContext _db;
        public PerangkatService(ILogger<PerangkatService> logger, ServerDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public override Task<PerangkatResponse> GetStatusPerangkat(PerangkatRequest request, ServerCallContext context)
        {
            var query = (from T0Perangkat in _db.T0PerangkatDbSet
                         where T0Perangkat.IdPerangkat == request.IdPerangkat
                         select new PerangkatResponse
                         {
                             IdPerangkat = request.IdPerangkat,
                             NomorSeri = T0Perangkat.No_Serial,
                             StatusPerangkat = T0Perangkat.StatusPerangkat,
                             StatusRoller = T0Perangkat.StatusRoller,
                         }).AsNoTracking().AsEnumerable();
            return Task.FromResult(query.FirstOrDefault());
        }

        public override Task<Reply> UpdateStatusPerangkat(UpdatePerangkatRequest request, ServerCallContext context)
        {
            Reply _reply;
            try
            {
                var query = from T0Perangkat in _db.T0PerangkatDbSet where T0Perangkat.IdPerangkat == request.IdPerangkat
                            select T0Perangkat.IdPerangkat;

                if (query.Any())
                {

                    DbT0Perangkat t0Perangkat = new DbT0Perangkat();

                    t0Perangkat.IdPerangkat = (int)request.IdPerangkat;
                    t0Perangkat.No_Serial = request.NomorSeri;
                    t0Perangkat.StatusPerangkat = (int)request.StatusPerangkat;
                    t0Perangkat.StatusRoller = (int)request.StatusRoller;
                    
                    _db.T0PerangkatDbSet.UpdateRange(t0Perangkat);
                    _db.SaveChanges();
                }
                _reply = new Reply() { Message = "Data Berhasil DiUpdate" };
            }
            catch (Exception e)
            {

                Metadata metadata = new Metadata { { "Error", "Error : " + e.Message } };
                throw new RpcException(new Status(StatusCode.Unknown, "Unknown"), metadata);

                _reply = new Reply() { Message = "Data Gagal terkirim ke server" };
            }
            return Task.FromResult(_reply);
        }
    }
}
