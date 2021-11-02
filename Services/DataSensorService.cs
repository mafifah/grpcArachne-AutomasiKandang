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
    public class DataSensorService : DataSensor.DataSensorBase
    {
        private readonly ILogger<DataSensorService> _logger;
        private readonly ServerDbContext _db;
        public DataSensorService(ILogger<DataSensorService> logger, ServerDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public override Task<DataResponse> GetDataTerbaru(DataRequest request, ServerCallContext context)
        {
            var query = (from T1DataSensor in _db.T1DataSensorDbSet
                         where T1DataSensor.IdPerangkat == request.IdPerangkat
                         select new DataResponse
                         {
                             IdDataSensor = T1DataSensor.IdDataSensor,
                             IdPerangkat = request.IdPerangkat,
                             Suhu = T1DataSensor.Suhu.ToString(),
                             Kelembaban = T1DataSensor.Kelembaban.ToString(),
                             Tanggal = T1DataSensor.Tanggal,
                             Waktu = T1DataSensor.Waktu
                         }).AsNoTracking().AsEnumerable();
            return Task.FromResult(query.FirstOrDefault());
        }

        public override async Task GetSemuaData(DataRequest request, IServerStreamWriter<DataResponse> responseStream, ServerCallContext context)
        {
            var tanggalHariIni = DateTime.Now.ToString("yyyy-MM-dd");
            var query = (from T1DataSensor in _db.T1DataSensorDbSet
                         where T1DataSensor.IdPerangkat == request.IdPerangkat
                         && T1DataSensor.Tanggal == tanggalHariIni
                         select new DataResponse
                         {
                             IdDataSensor = T1DataSensor.IdDataSensor,
                             IdPerangkat = request.IdPerangkat,
                             Suhu = T1DataSensor.Suhu.ToString(),
                             Kelembaban = T1DataSensor.Kelembaban.ToString(),
                             Tanggal = T1DataSensor.Tanggal,
                             Waktu = T1DataSensor.Waktu
                         }).AsNoTracking().AsEnumerable();
            foreach (var ListData in query)
            {
                await responseStream.WriteAsync(ListData);
            }
        }

        public override Task<Response> InsertDataBaru(InsertDataRequest request, ServerCallContext context)
        {
            Response _reply;
            try
            {
                DbT1DataSensor t1DataSensor = new DbT1DataSensor();
                t1DataSensor.IdPerangkat = (int)request.IdPerangkat;
                t1DataSensor.Suhu = request.Suhu;
                t1DataSensor.Kelembaban = request.Kelembaban;
                t1DataSensor.Tanggal = DateTime.Now.ToString("yyyy-MM-dd");
                t1DataSensor.Waktu = DateTime.Now.ToString("HH:mm:ss");
                _db.T1DataSensorDbSet.AddRange(t1DataSensor);
                _db.SaveChanges();
                _reply = new Response() { Message = "Data Berhasil Diinputkan ke database" };
            }
            catch (Exception e)
            {

                Metadata metadata = new Metadata { { "Error", "Error : " + e.InnerException.Message } };
                throw new RpcException(new Status(StatusCode.Unknown, "Unknown"), metadata);

                _reply = new Response() { Message = "Data Gagal terkirim ke server" };
            }
            return Task.FromResult(_reply);
        }
    }
}
