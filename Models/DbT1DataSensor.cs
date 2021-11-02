using System;

namespace grpcArachne.Models
{
    public class DbT1DataSensor
    {
        public int IdDataSensor { get; set; }
        public int IdPerangkat { get; set; }
        public float Suhu { get; set; }
        public float Kelembaban { get; set; }
        public string Tanggal { get; set; }
        public string Waktu { get; set; }
    }
}
