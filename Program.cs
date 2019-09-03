using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;

namespace ImportacaoGPX
{
    class Program
    {
        static void Main(string[] args)
        {
            FileInfo fi = new FileInfo(args[0]);
            Corrida corrida;
            if (fi.Extension.ToLower() == ".gpx")
            {
                corrida = ImportaGPX(fi);
            }
            else
            {
                corrida = ImportaTCX(fi);
            }
            Grava(corrida);
            Console.WriteLine("fim");
        }

        static void Grava(Corrida corrida)
        {
            using (var db = new DB())
            {
                db.Entry(corrida).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                db.SaveChanges();
                foreach (Track track in corrida.Tracks)
                {
                    track.IdCorrida = corrida.Id;
                    db.Entry(track).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                }
                db.SaveChanges();
            }
        }
        static Corrida ImportaGPX(FileInfo arquivo)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(arquivo.FullName);
            XmlNode Root = doc.DocumentElement;
            XmlNode Meta = Root["metadata"];
            Corrida corrida = new Corrida();
            corrida.Tracks = new List<Track>();
            corrida.Inicio = DateTime.Parse(Meta["time"].InnerText);
            XmlNodeList trks = doc.GetElementsByTagName("trkpt");

            foreach (XmlNode ponto in trks)
            {
                Track track = new Track();
                track.lat = float.Parse(ponto.Attributes["lat"].Value);
                track.lon = float.Parse(ponto.Attributes["lon"].Value);
                track.Elevacao = float.Parse(ponto["ele"].InnerText);
                track.Hora = DateTime.Parse(ponto["time"].InnerText);
                track.Batimento = int.Parse(ponto["extensions"]["ns3:TrackPointExtension"]["ns3:hr"].InnerText);
                track.CadenciaPasso = int.Parse(ponto["extensions"]["ns3:TrackPointExtension"]["ns3:cad"].InnerText);
                corrida.Tracks.Add(track);
            }

            return corrida;
        }

        static Corrida ImportaTCX(FileInfo arquivo)
        {
            Corrida corrida = new Corrida();
            corrida.Tracks = new List<Track>();
            XmlDocument doc = new XmlDocument();
            doc.Load(arquivo.FullName);
            XmlNode Root = doc.DocumentElement;
            corrida.Inicio = DateTime.Parse(Root["Activities"]["Activity"]["Id"].InnerText);
            XmlNodeList trks = doc.GetElementsByTagName("Trackpoint");
            foreach (XmlNode ponto in trks)
            {
                Track track = new Track();
                if (ponto["Position"] != null)
                {
                    track.lat = float.Parse(ponto["Position"]["LatitudeDegrees"].InnerText);
                    track.lon = float.Parse(ponto["Position"]["LongitudeDegrees"].InnerText);

                }
                if (ponto["AltitudeMeters"] != null)
                {
                    track.Elevacao = float.Parse(ponto["AltitudeMeters"].InnerText);
                }

                track.Hora = DateTime.Parse(ponto["Time"].InnerText);
                track.Batimento = int.Parse(ponto["HeartRateBpm"]["Value"].InnerText);
                track.CadenciaPasso = int.Parse(ponto["Extensions"]["ns3:TPX"]["ns3:RunCadence"].InnerText);
                corrida.Tracks.Add(track);
            }


            return corrida;
        }
    }
}